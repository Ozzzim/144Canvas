using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using _144Canvas.API.Data;
using _144Canvas.API.Models;
using Microsoft.AspNetCore.Mvc;
using _144Canvas.API.DTOs;
using System.Security.Claims;
using Microsoft./*System.*/IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace _144Canvas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _rep;
        private readonly IConfiguration _con;
        private readonly UserManager<User> _usm;
        private readonly SignInManager<User> _sim;
        public AuthController(IAuthRepository rep, IConfiguration con, UserManager<User> usm, SignInManager<User> sim){
            _rep=rep;
            _con=con;
            _usm=usm;
            _sim=sim;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO ufrdto)
        {
            /*Pre Identity model
            ufrdto.Username= ufrdto.Username.ToLower();

            if(await _rep.UserExists(ufrdto.Username))
                return BadRequest("User already exists");

            var userToCreate = new User{
                UserName = ufrdto.Username
            };

            var createdUser = await _rep.Register(userToCreate,ufrdto.Password);
            */

            var userToCreate = new User{
                UserName = ufrdto.UserName,
                DateCreated = DateTime.Today
            };

            var result = await _usm.CreateAsync(userToCreate,ufrdto.Password);
            

            if(result.Succeeded){
                User newUser = await _usm.FindByNameAsync(ufrdto.UserName);
                result = await _usm.AddToRoleAsync(newUser,"User");
                return StatusCode(201);
            }
            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO usldto){
            Console.WriteLine("AuthCon");
            var user = await _usm.FindByNameAsync(usldto.UserName);//_rep.Login(usldto.Username.ToLower(), usldto.Password);// Pre Identity Model

            if(user == null)
                return BadRequest();
            Console.WriteLine("UserExists");
            var result = await _sim.CheckPasswordSignInAsync(user, usldto.Password, false);

            if(result.Succeeded){
                Console.WriteLine("Password correct");
                return Ok(new {token=GenerateJWTToken(user).Result});//new {token = tokenHandler.WriteToken(token)});
            }
            return BadRequest();
            /*Pre Identity model
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_con.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
                                                {
                                                    Subject = new ClaimsIdentity(claims),
                                                    Expires = DateTime.Now.AddDays(1),
                                                    SigningCredentials = credentials
                                                });
            //*/

            
        }

        private async Task<string> GenerateJWTToken(User user){
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _usm.GetRolesAsync(user);
            foreach(var role in roles){
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_con.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
                                                {
                                                    Subject = new ClaimsIdentity(claims),
                                                    Expires = DateTime.Now.AddDays(1),
                                                    SigningCredentials = credentials
                                                });
            return tokenHandler.WriteToken(token);
        }
        [Authorize]
        [HttpPost("password")]
        public async Task<IActionResult> ChangePassword(UserPasswordUpdateDTO upudto){
            Console.WriteLine("AuthCon: Password change");
            if(upudto.nPass==upudto.nPassR && await _rep.ChangePassword(upudto.userID,upudto.cPass,upudto.nPass))
                return Ok();
            return BadRequest();
        }

        [Authorize (Roles = "Admin")]
        [HttpPost("role/{uid}")]
        public async Task<IActionResult> ChangeRole(int uid, RoleEditDTO redto){
            //List<string> roleList=redto.roles.List<string>();
            foreach(string role in redto.roleNames){
                if(role == "Admin")
                    return BadRequest("You can't change admins, access database directly");
            }

            var user = await _usm.FindByIdAsync(""+uid);
            var userRoles = await _usm.GetRolesAsync(user);
            //var selectedRoles = redto.roleNames;
            redto.roleNames = redto.roleNames ?? new string[] {};

            var result = await _usm.AddToRolesAsync(user, redto.roleNames.Except(userRoles));

            if(!result.Succeeded)
                return BadRequest();

            result = await _usm.RemoveFromRolesAsync(user, redto.roleNames.Intersect(userRoles));

            if(!result.Succeeded)
                return BadRequest();

            return Ok();
        }
    }
}