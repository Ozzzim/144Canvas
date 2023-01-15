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
    public class LikesController: ControllerBase {
        private readonly IOFFCanvasRepository _repo;

        public LikesController(IOFFCanvasRepository ioffr){
            _repo=ioffr;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> HelloMessage(){
            await Task.Run(() => {});//Filler
            return Ok("Likes say hello");
            //return new string[] { "value1", "value2" };
        }

        [AllowAnonymous]
        [HttpGet("count/{pictureID}")]
        public async Task<IActionResult> LikeCount(int pictureID){
            int likesCount = await _repo.GetLikeCount(pictureID);

            return Ok(likesCount);
        }

        [HttpGet("{userID}/{pictureID}")]
        public async Task<IActionResult> GetLike(int userID, int pictureID){
            var like = await _repo.GetLike(userID, pictureID);

            return Ok(like);
        }

        [HttpPost("like")]
        public async Task<IActionResult> Like(LikeForCreationDTO lfcdto){
            try{
                ///*
                var like = new Like{
                    UserID=lfcdto.UserID,
                    PictureID=lfcdto.PictureID
                };//*/
                
                var createdLike = await _repo.Like(like);
            if(await _repo.SaveAll())
                    return StatusCode(201);
            }catch(Exception e){
                Console.WriteLine("EEEERROOR "+e);
            }
            return StatusCode(400);
        }

        [HttpPost("unlike")]
        public async Task<IActionResult> Unlike(LikeForCreationDTO lfcdto){
            try{
                ///*
                var like = new Like{
                    UserID=lfcdto.UserID,
                    PictureID=lfcdto.PictureID
                };//*/
                
                //var deletedLike = 
                await _repo.UnLike(like);
            if(await _repo.SaveAll())
                    return StatusCode(201);
            }catch(Exception e){
                Console.WriteLine("EEEERROOR "+e);
            }
            return StatusCode(400);
        }
    }
}