using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using _144Canvas.API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Text.RegularExpressions;

namespace _144Canvas.API.Data
{
    public class Seed
    {
        public static void SeedRoles(RoleManager<Role> rm){
            if(!rm.Roles.Any()){

                rm.CreateAsync(new Role{Name = "User"}).Wait();
                rm.CreateAsync(new Role{Name = "Artist"}).Wait();
                rm.CreateAsync(new Role{Name = "Admin"}).Wait();
                rm.CreateAsync(new Role{Name = "Mod"}).Wait();
                System.Console.WriteLine("Seeded in "+4+" new roles.");
            }
        }

        public static async void SeedDefaultAdmin(string adminName,UserManager<User> um, RoleManager<Role> rm){
            User searchedUser=await um.FindByNameAsync(adminName);

            if(searchedUser==null){
                Random rnd = new Random();
                string generatedPassword ="";
                for(int i=0; i<10; i++){
                    generatedPassword+=rnd.Next(100)%10;
                }
                var result= um.CreateAsync(new User{
                    UserName=adminName, 
                    Descryption= "Hi, I'm a default admin.",
                }
                , (generatedPassword!="" ? generatedPassword : "PasswordsGenIsBroken")
                ).Result;
                if(result.Succeeded){
                    
                    searchedUser=um.FindByNameAsync(adminName).Result;
                    await um.AddToRolesAsync(searchedUser, new[] {"Admin","Mod"});
                }
                System.Console.WriteLine("\tSeeded in new default admin. It's password is: "+generatedPassword);
            }
        }

        public async static void UpdateOldPictures(IOFFCanvasRepository offcr){
            List<Picture> pictures=(await offcr.GetPictures()).ToList<Picture>();
            int changedSomething=0;
            foreach(Picture picture in pictures){
                if(picture.ImagedataId==-1){
                    var idata = new Imagedata{
                        imageData = Convert.FromBase64String(Regex.Match(picture.URL, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value) //(byte[])pfcdto.ImageData
                    };

                    var createdIData = await offcr.PostImage(idata);
                    picture.ImagedataId=createdIData.ID;
                    picture.URL=null;
                    changedSomething++;
                }
            }
            if(changedSomething>0){
                await offcr.SaveAll();
                System.Console.WriteLine("\n\nUpdated "+changedSomething+" picture records\n\n");
            }

        }
        public static void SeedUsers(/*DataContext dc*/UserManager<User> um){
            if(!um.Users.Any()){
            //if(!dc.Users.Any()){
                var userData= System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach(var user in users){
                    var result = um.CreateAsync(user, "12345").Result;
                    if(result.Succeeded){
                        User searchedUser=um.FindByNameAsync(user.UserName).Result;
                        um.AddToRoleAsync(searchedUser, "User");
                    }
                    /*Pre Identity model stuff
                    byte[] passwordhash, passwordSalt;
                    CreatePasswordHash("password", out passwordhash, out passwordSalt);

                    //user.PasswordHash=passwordhash;//Pre-Identitymodel password hash
                    //user.PasswordSalt=passwordSalt;

                    user.UserName = user.UserName.ToLower();
                    dc.Users.Add(user);//*/
                }

                //dc.SaveChanges();
                System.Console.WriteLine("Seeded in "+((List<User>)users).Count+" new users.");
            }
        }
        private static void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt){
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt= hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}