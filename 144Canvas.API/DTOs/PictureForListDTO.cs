using System;
using System.Collections.Generic;

namespace _144Canvas.API.DTOs
{
    public class PictureForListDTO
    {
        public int ID {get; set;}
        public UserForReferenceDTO User {get; set;}
        public string URL {get; set;}
        public int ImagedataId {get;set;}

        public string Title {get; set;}
        public DateTime DateSent{get; set;}
        public string Tags {get; set;}

        //public ICollection<LikeCountDTO> Likes {get; set;}
        public ICollection<CommentCountDTO> Comments {get; set;}
        /*
        public int ID {get; set;}
        public User User {get; set;}
        public int UserId {get; set;}
        public string URL {get; set;}
        public string Title {get; set;}
        public DateTime DateSent{get; set;}
        public string Tags {get; set;}
        //public ICollection<string> Tags {get; set;}
        public bool IsProfileBackground {get; set;}
        
        public ICollection<Like> Likes {get; set;}
        public ICollection<Comment> Comments {get; set;}
        public ICollection<Report> Reports {get; set;}
        */
    }
}