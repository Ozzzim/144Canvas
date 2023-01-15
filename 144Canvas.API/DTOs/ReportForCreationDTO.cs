using _144Canvas.API.Models;

namespace _144Canvas.API.DTOs
{
    public class ReportForCreationDTO
    {
        public int UserID {get; set;}
        public int PictureID {get; set;}
        public string Category {get; set;}
        public string Descryption {get; set;}
    }
}