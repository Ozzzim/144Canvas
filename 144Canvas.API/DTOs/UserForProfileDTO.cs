using System;
using System.Collections.Generic;
using _144Canvas.API.Models;

namespace _144Canvas.API.DTOs
{
    public class UserForProfileDTO
    {
        public int ID {get; set;}
        public string UserName {get; set;}
        public string Descryption {get; set;}
        public string BackgroundURL{get; set;}
        public DateTime DateCreated {get; set;}
        public ICollection<PictureForDetailDTO> Pictures {get; set;}
        public ICollection<CommentForProfileDTO> UserComments {get;set;}
        //public ICollection<Picture> Pictures {get; set;}
        //public ICollection<Comment> UserComments {get;set;}
    
    }
}