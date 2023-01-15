using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using _144Canvas.API.Data;
using AutoMapper;
using _144Canvas.API.DTOs;
using _144Canvas.API.Models;
using System.Text.RegularExpressions;

namespace _144Canvas.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly IOFFCanvasRepository _repo;
        private readonly IMapper _mapper;

        //private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

        public PicturesController(IOFFCanvasRepository ioffr, IMapper mapper/*, IOptions<CloudinarySettings> cloudinaryConfig*/){
            _repo=ioffr;
            _mapper=mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPictures(){
            IEnumerable<Picture> pictures = await _repo.GetPictures();
            pictures = pictures.OrderByDescending(picture => picture.DateSent);
            var returnPictures = _mapper.Map<IEnumerable<PictureForListDTO>>(pictures);
            
            Console.WriteLine(returnPictures.ElementAt(0).Title);
            return Ok(returnPictures);
            //return new string[] { "value1", "value2" };
        }

        ///*
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPicture(int id){
            var picture = await _repo.GetPicture(id);
            var returnPicture = _mapper.Map<PictureForDetailDTO>(picture);
           
            return Ok(returnPicture);
        }//*/

        [AllowAnonymous]
        [HttpGet("search/{title}/{tags}/{user}/{order}")]
        public async Task<IActionResult> GetPictureSearch(string title,string tags,string user,string order){
            bool[] searchSpan=new bool[3];
            searchSpan[0]=title!=null && title!="*" && title.Length>0;
            searchSpan[1]=tags!=null && tags!="*" && tags.Length>0;
            searchSpan[2]=user!=null && user!="*" && user.Length>0;

            IEnumerable<Picture> pictures = await _repo.GetPictures();
            //Console.WriteLine("REQUEST COMPLETED, Filtering results:"+searchSpan[0]+","+searchSpan[1]+","+searchSpan[2]+",");
            pictures = pictures.Where(p => (
                (!searchSpan[0] || p.Title.Contains(title,StringComparison.OrdinalIgnoreCase)) && 
                (!searchSpan[1] || p.CheckTags(tags)) &&
                (!searchSpan[2] || p.User.UserName.Contains(user,StringComparison.OrdinalIgnoreCase))
            ));
            switch(order){
                case "popularity":
                    pictures=pictures.OrderByDescending(p => p.getLikes());
                    break;
                case "date":
                    pictures=pictures.OrderByDescending(p => p.DateSent);
                    break;
                case "title":
                    pictures=pictures.OrderBy(p => p.Title);
                    break;
                case "user":
                    pictures=pictures.OrderBy(p => p.User.UserName);
                    break;
                default:
                    break;
            }
            
            var returnPicture = _mapper.Map<IEnumerable<PictureForListDTO>>(pictures);
           
            return Ok(returnPicture);
        }

        ///*
        [Authorize (Roles = "User")]
        [HttpPost("post")]
        public async Task<IActionResult> PostPicture(PictureForCreationDTO pfcdto){
            Console.WriteLine("\n\nController: Posting a picture...\n\n");

            try{
                Console.WriteLine("Controller: Posting a picture...");
                Imagedata createdIData=null;
                //var pic = _mapper.Map<Picture>(pfcdto);
                if(pfcdto.URL.StartsWith("data:image/png;base64,")){
                    var idata = new Imagedata{
                        imageData = Convert.FromBase64String(Regex.Match(pfcdto.URL, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value) //(byte[])pfcdto.ImageData
                    };

                    createdIData = await _repo.PostImage(idata);
                }else
                    return Forbid();

                
                ///*
                var pic = new Picture{
                    //User=pfcdto.User,
                    UserId=pfcdto.UserId,
                    //URL=pfcdto.URL,
                    ImagedataId=createdIData.ID,
                    //Imagedata=createdIData,
                    Title=pfcdto.Title,
                    Tags=pfcdto.Tags
                };//*/

                
                var createdPicture = await _repo.PostPicture(pic);
                
                if(await _repo.SaveAll())
                    return StatusCode(201);
            }catch(Exception e){
                Console.WriteLine(e);
            }
            return StatusCode(400);
            
        }//*/

        [Authorize (Roles = "Mod,Admin")]
        [HttpPost("remove")]
        public async Task<IActionResult> RemovePicture(ReportForRemovalDTO rfrdto){
            //if(true){
                 Console.WriteLine("REMOVING "+rfrdto.PictureID);
                await _repo.RemovePicture(rfrdto.PictureID);
                
                if(await _repo.SaveAll())
                    return StatusCode(201);

                return Ok("Deleted");
                //return Ok(reports);
            //}
            //return StatusCode(400);
            //return new string[] { "value1", "value2" };
            //return Unauthorized();
        }

        [AllowAnonymous]
        [HttpGet("storage/{id}")]
        public async Task<IActionResult> GetImage(int id){
            Imagedata idata = await _repo.GetImage(id);
            //var image = Image.FromStream();
            //Imagedata idata = await _repo.GetImage(id);
            return File(idata.imageData, "image/png");
        }
    }
}