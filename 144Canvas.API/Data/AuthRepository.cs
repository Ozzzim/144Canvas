using System.Threading.Tasks;
using System;
using _144Canvas.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace _144Canvas.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _usm;
        private readonly SignInManager<User> _sim;
        public AuthRepository(DataContext context, UserManager<User> usm, SignInManager<User> sim){
            _context=context;
            _usm=usm;
            _sim=sim;
        }
        public async Task<User> Register(User user, string password){
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password,out passwordHash,out passwordSalt);
            
            //user.PasswordHash = passwordHash;//Pre-Identitymodel password hash
            //user.PasswordSalt = passwordSalt;//Pre-Identitymodel password hash
            user.DateCreated = System.DateTime.Today;
            user.Background_ID = -1;//Be sure that background isn't looked for when ID=-1

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
            return user;
        }
        public async Task<User> Login(string username, string password){
            Console.Write("AuthRep");
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            
            if (user == null)
                return null;
            //if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))//Pre-Identitymodel password hash
            //    return null;
            return user;
        }
        public async Task<bool> UserExists(string username){
            return (await _usm.FindByNameAsync(username))!=null;//await _context.Users.AnyAsync(x => x.UserName == username))
        }

        private void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt){
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt= hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] PasswordHash, byte[] PasswordSalt){//Deprecated?
            using(var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt)){
                var decoded = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                int i=0;
                while(i<decoded.Length){
                    if(decoded[i] != PasswordHash[i])
                        return false;
                    i++;
                }
            }
            return true;
        }

        public async Task<bool> ChangePassword(int id,string oP, string nP){
            User u = await _usm.FindByIdAsync(""+id);//Don't know how to change password with current identity model

            if(u!=null && await _usm.CheckPasswordAsync(u,oP)){//VerifyPasswordHash(oP, u.PasswordHash,u.PasswordSalt)){
                //byte[] passwordHash, passwordSalt;
                //CreatePasswordHash(nP,out passwordHash,out passwordSalt);

                //u.PasswordHash = passwordHash;
                //u.PasswordSalt = passwordSalt;
                //await _context.SaveChangesAsync();
                return (await _usm.ChangePasswordAsync(u, oP,nP)).Succeeded;
            }

            return false;
        }
    }
}