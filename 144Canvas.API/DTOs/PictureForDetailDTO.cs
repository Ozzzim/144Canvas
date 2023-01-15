using System;

namespace _144Canvas.API.DTOs
{
    public class PictureForDetailDTO
    {
        public int ID {get; set;}
        public int UserId {get; set;}
        public UserForReferenceDTO User {get; set;}
        public string URL {get; set;}
        public int ImagedataId {get;set;}
        public string Title {get; set;}
        public string Tags {get; set;}
        public DateTime DateSent{get; set;}
    }
}