using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using _144Canvas.API.Data;
using AutoMapper;
using _144Canvas.API.DTOs;
using _144Canvas.API.Models;

namespace _144Canvas.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IOFFCanvasRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly UserManager<User> _usm;

        public UsersController(IOFFCanvasRepository ioffr, IMapper mapper,UserManager<User> usm,DataContext context){
            _repo=ioffr;
            _mapper=mapper;
            _usm=usm;
            _context=context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(){
            var users = await _repo.GetUsers();
            var returnUsers = _mapper.Map<IEnumerable<UserForListDTO>>(users);

            return Ok(returnUsers);
            //return new string[] { "value1", "value2" };
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id){
            var user = await _repo.GetUser(id);
            var returnUser = _mapper.Map<UserForProfileDTO>(user);
            //Console.WriteLine("Request for a single user has arrived..."+returnUser.UserComments.Count+" "+returnUser.Pictures.Count);

            return Ok(returnUser);
        }
        ///*
        [Authorize (Roles = "Admin,Mod")]
        [HttpGet("roles")]
        public async Task<IActionResult> GetUserWRoles(){
           return Ok(await _repo.GetUsersRoles());
        }//*/

        [Authorize (Roles = "User")]
        [HttpGet("search/{query}/{order}")]
        public async Task<IActionResult> GetSearch(string query,string order){
            IEnumerable<User> users;
            if(query==null || query=="*")
                users = await _repo.GetUsers();
            else
                users = await _repo.GetUserSearch(query);

            switch(order){
                case "date":
                    users=users.OrderBy(u => u.DateCreated);
                    break;
                case "name":
                    users=users.OrderBy(u => u.UserName);
                    break;
                default:
                    break;
            }
            
            var returnUsers = _mapper.Map<IEnumerable<UserForListDTO>>(users);
            return Ok(returnUsers);
        }

        [AllowAnonymous]
        [HttpGet("comments/{id}")]
        public async Task<IActionResult> GetUserComments(int id){//DELETE ME PLEASE !!!!
            Console.WriteLine("Request arrived...");
            var comments = await _repo.GetUserComments(id);
            var returnComments = _mapper.Map<IEnumerable<CommentForProfileDTO>>(comments);

            //return Ok(returnComments);
            return Ok(comments);
        }
        
        [Authorize (Roles = "User")]
        [HttpPost("{id}/like/{pictureID}")]
        public async Task<IActionResult> Like(int id, int pictureID){
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id,pictureID);

            if(like != null)
                return BadRequest("You already liked this picture!");
            
            if(await _repo.GetPicture(pictureID)==null)
                return NotFound();
            
            like=new Like{
                UserID=id,
                PictureID=pictureID,
            };

            _repo.Add<Like>(like);

            if(await _repo.SaveAll())
                return Ok();

            return BadRequest("Critical like fail");
        }
        
        [Authorize (Roles = "User")]
        [HttpPost("{id}/unlike/{pictureID}")]
        public async Task<IActionResult> Unlike(int id, int pictureID){
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id,pictureID);

            if(like == null)
                return BadRequest("You haven't liked this picture!");
            
            if(await _repo.GetPicture(pictureID)==null)
                return NotFound();

            _repo.Delete<Like>(like);

            if(await _repo.SaveAll())
                return Ok();

            return BadRequest("Critical unlike fail");
        }

        [Authorize (Roles = "User")]
        [HttpPost("changeBG")]
        public async Task<IActionResult> ChangeBackground(UserBackgroundUpdateDTO ubudto){
            Picture newPic=await _repo.GetPicture(ubudto.newPicID);
            
            if(newPic==null)
                return BadRequest("Image does not exist!");
            if(ubudto.userID!=newPic.UserId)
                return BadRequest("Image does not belong to the user!");

            await _repo.UpdateUserBackground(ubudto.userID,ubudto.newPicID);

            return Ok();
        }
        
        [Authorize (Roles = "User")]
        [HttpPost("changeAbout")]
        public async Task<IActionResult> ChangeAbout(UserAboutUpdateDTO uaudto){
            string about;
            if(uaudto.about==null)
                about="";
            else
                about= uaudto.about;
            await _repo.UpdateUserAbout(uaudto.userID,about);

            return Ok();
        }

        [Authorize (Roles = "Admin")]
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveUser(ReportForRemovalDTO rfrdto){
            User u=await _usm.FindByIdAsync(rfrdto.UserID+"");
            if(u==null)
                return BadRequest();
            if(!await _usm.IsInRoleAsync(u,"Admin")){
                //Console.WriteLine("REMOVING "+rfrdto.UserID);
                //await _repo.RemoveUser(rfrdto.UserID);
                await RemoveUsersAllImageData(rfrdto.UserID);
                IdentityResult ir = await _usm.DeleteAsync(u);
                //if(await _repo.SaveAll())
                //    return StatusCode(201);
                if(ir.Succeeded)
                    return Ok();
                else
                    return BadRequest(ir.Errors);
                //return Ok(reports);
            }
            return Forbid("Admins have to be deleted manually!");
            //return new string[] { "value1", "value2" };
            //return Unauthorized();
        }
        
        private async Task RemoveUsersAllImageData(int userID){
            List<int> idataIDlist=new List<int>();
            foreach(Picture p in await _repo.GetUserPictures(userID)){
                idataIDlist.Add(p.ImagedataId);
            }
            foreach(int i in idataIDlist){
                var idata = await _context.Imagedatas.FirstOrDefaultAsync<Imagedata>(id => id.ID == i);
                _context.Imagedatas.Remove(idata);
            }
            
            return;
        }
        /*private readonly DataContext _context;
        public UsersController(DataContext dc){_context=dc;}

        // GET api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers(){
            var users = await _context.Users.ToListAsync();
            return Ok(users);
            //return new string[] { "value1", "value2" };
        }

        // GET api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id){
            var user = await _context.Users.FirstOrDefaultAsync(x => x.ID == id);
            return Ok(user);
        }

        // POST api/Users
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/Users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
