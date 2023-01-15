using System.Collections.Generic;

namespace _144Canvas.API.DTOs
{
    public class UserRoleDTO
    {
        public int Id {get; set;}
        public string UserName {get; set;}
        public IEnumerable<string> Roles {get; set;}
    }
}