using System;
using System.Collections.Generic;
using _144Canvas.API.Models;
using Microsoft.AspNetCore.Http;

namespace _144Canvas.API.DTOs
{
    public class PictureForCreationDTO
    {
        //public int ID {get; set;}
        //public User User {get; set;}//Not necessary?
        public int UserId {get; set;}
        public string URL {get; set;}
        public string Title {get; set;}
        //public DateTime DateSent{get; set;}
        public string Tags {get; set;}
        public IFormFile ImageData {get;set;}

        /*public PictureForCreationDTO(){
            DateSent=DateTime.Now;
        }*/
    }
}