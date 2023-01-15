using System.ComponentModel.DataAnnotations;

namespace _144Canvas.API.DTOs
{
    public class UserForRegisterDTO{
        [Required]//Saftey check for empty strings
        public string UserName {get;set;}
        [Required]//Saftey check for empty strings
        public string Password {get;set;}
    }
}