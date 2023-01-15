using System;

namespace _144Canvas.API.DTOs
{
    public class CommentForProfileDTO
    {
        public int UserID {get; set;}
        //public User CommentingUser{get; set;}
        public int PictureID{get; set;}
        public PictureForReferenceDTO CommentedPicture{get; set;}
        public UserForReferenceDTO CommentingUser{get;set;}
        public string Content{get; set;}
        public DateTime DateSent{get; set;}
    }
}