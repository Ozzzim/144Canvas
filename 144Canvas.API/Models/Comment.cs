using System;

namespace _144Canvas.API.Models
{
    public class Comment
    {
        public int ID {get; set;}
        public int UserID {get; set;}
        public User CommentingUser{get; set;}
        public int PictureID{get; set;}
        public Picture CommentedPicture{get; set;}
        public string Content{get; set;}
        public DateTime DateSent{get; set;}
    }
}