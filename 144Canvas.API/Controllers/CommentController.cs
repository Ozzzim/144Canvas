using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using _144Canvas.API.Data;
using AutoMapper;
using _144Canvas.API.DTOs;
using _144Canvas.API.Models;

namespace _144Canvas.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController: ControllerBase{
        private readonly IOFFCanvasRepository _repo;
        private readonly IMapper _mapper;
        
        public CommentController(IOFFCanvasRepository ioffr, IMapper mapper){
            _repo=ioffr;
            _mapper=mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetComments(){
            await Task.Run(() => {});//Filler
            return Ok("Comments say hello");
            //return new string[] { "value1", "value2" };
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserComments(int id){
            Console.WriteLine("Request arrived...");
            var comments = await _repo.GetUserComments(id);
            var returnComments = _mapper.Map<IEnumerable<CommentForProfileDTO>>(comments);

            //return Ok(returnComments);
            return Ok(returnComments);
        }

        [AllowAnonymous]
        [HttpGet("picture/{id}")]
        public async Task<IActionResult> GetPictureComments(int id){
            Console.WriteLine("Request arrived...");
            var comments = await _repo.GetPictureComments(id);
            var returnComments = _mapper.Map<IEnumerable<CommentForProfileDTO>>(comments);

            //return Ok(returnComments);
            return Ok(returnComments);
        }

        [Authorize (Roles = "User")]
        [HttpPost("post")]
        public async Task<IActionResult> PostComment(CommentForCreationDTO cfcdto){
            try{
                Console.WriteLine("Controller: Posting a picture...");
                
                //var pic = _mapper.Map<Picture>(pfcdto);
                
                ///*
                var com = new Comment{
                    //User=pfcdto.User,
                    UserID=cfcdto.UserID,
                    PictureID=cfcdto.PictureID,
                    Content=cfcdto.Content
                };//*/
                var createdCom = await _repo.PostComment(com);
                Console.WriteLine("Controller: Finished posting a comment...");
                
                if(await _repo.SaveAll())
                    return StatusCode(201);
            }catch(Exception e){
                Console.WriteLine("EEEERROOR "+e);
            }
            return StatusCode(400);
        }
    }
}