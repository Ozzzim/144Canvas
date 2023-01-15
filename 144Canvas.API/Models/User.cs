using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;

namespace _144Canvas.API.Models
{
using System.ComponentModel.DataAnnotations.Schema;
    public class User : IdentityUser<int>
    {
        //public int ID {get; set;}
        //public string Nickname {get; set;}
        public string Descryption {get; set;}
        public int Background_ID{get; set;}
        //public byte[] PasswordHash{get; set;}
        //public byte[] PasswordSalt{get; set;}
        public DateTime DateCreated {get; set;}
        
        /*[ForeignKey("Picture")]
        public int ProfilePictureId {get; set;}
        public Picture ProfilePicture {get; set;}
        //*/

        //Relationships

        public ICollection<Picture> Pictures {get; set;}
        //public Picture ProfilePicture {get; set;}
        public ICollection<Comment> UserComments {get; set;}
        public ICollection<Like> Liked {get; set;}
        public ICollection<Report> Reports {get; set;}
        public ICollection<UserRole> UserRoles {get; set;}
    }
}