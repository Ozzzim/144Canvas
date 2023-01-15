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

namespace _144Canvas.API.Controllers
{
    [Authorize /*(Roles = "Admin,Mod")*/]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController: ControllerBase {
        private readonly IOFFCanvasRepository _repo;
        private readonly IMapper _mapper;

        //private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

        public ReportsController(IOFFCanvasRepository ioffr, IMapper mapper/*, IOptions<CloudinarySettings> cloudinaryConfig*/){
            _repo=ioffr;
            _mapper=mapper;
        }

        [Authorize (Roles = "Mod,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetReports(){
            if(true){
                IEnumerable<Report> reports = await _repo.GetReports(); 
                //reports = reports.OrderByDescending(reports => report.DateSent);//report has no date. Does it need one though?
                
                Console.WriteLine("Pictures loaded");
                var returnReports = _mapper.Map<IEnumerable<ReportForDisplayDTO>>(reports);
                Console.WriteLine("Pictures mapped: "+((IEnumerable<ReportForDisplayDTO>)returnReports));
                foreach (ReportForDisplayDTO rfddto in ((IEnumerable<ReportForDisplayDTO>)returnReports)){
                    Console.WriteLine(rfddto.Category);
                }
                return Ok(returnReports);
                //return Ok(reports);
            }
            //return new string[] { "value1", "value2" };
            //return Unauthorized();
        }

        
        //[Authorize (Roles = "Mod,Admin")]
        //[HttpGet("{id}")]
        /*public async Task<IActionResult> GetReport(int id){//Do I need that?
            if(false){
                Report report; 
                //= await _repo.GetReport(id);
                //reports = reports.OrderByDescending(reports => report.DateSent);//report has no date. Does it need one though?
                
                return Ok(report);
            }
            //return new string[] { "value1", "value2" };
            return Unauthorized();
        }*/

        [Authorize (Roles = "User,Mod,Admin")]
        [HttpPost("post")]
        public async Task<IActionResult> CreateReport(ReportForCreationDTO rfcdto){
            try{
                ReportCategory rCat;
                switch(rfcdto.Category){
                    case "1":
                        rCat=ReportCategory.OffensiveImagery;
                        break;
                    case "2":
                        rCat=ReportCategory.OffensiveUser;
                        break;
                    case "3":
                        rCat=ReportCategory.Advertisement;
                        break;
                    case "4":
                        rCat=ReportCategory.Plagiarism;
                        break;
                    default:
                        rCat=ReportCategory.Other;
                        break;
                }
                ///*
                var rep = new Report{
                    UserID = rfcdto.UserID,
                    PictureID = rfcdto.PictureID,
                    Category = rCat,
                    Descryption = rfcdto.Descryption
                };

                var createdReport = await _repo.PostReport(rep);
                
                if(await _repo.SaveAll())
                    return StatusCode(201);
            }catch(Exception e){
                Console.WriteLine(e);
            }
            return StatusCode(400);
        }
        
        [Authorize (Roles = "Mod,Admin")]
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveReport(ReportForRemovalDTO rfrdto){
            //if(true){
                bool wasAlive = await _repo.RemoveReport(rfrdto.UserID,rfrdto.PictureID);
                
                if(await _repo.SaveAll()){
                    if(wasAlive){
                        return StatusCode(200, new OkResult());
                    } else {
                        return StatusCode(200, new NotFoundResult());
                    }
                }
                    

                return Ok("Deleted");
                //return Ok(reports);
            //}
            //return StatusCode(400);
            //return new string[] { "value1", "value2" };
            //return Unauthorized();
        }
    }
}