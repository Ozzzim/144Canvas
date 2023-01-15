using _144Canvas.API.Models;

namespace _144Canvas.API.DTOs
{
    public class ReportForDisplayDTO
    {
        public int UserID {get; set;}
        public User ReportingUser {get; set;}
        public int PictureID {get; set;}
        public Picture ReportedPicture {get; set;}
        public string Category {get; set;}
        public string Descryption {get; set;}
    }
}