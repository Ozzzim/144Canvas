using Microsoft.AspNetCore.Identity;

namespace _144Canvas.API.Models
{
    public class UserRole: IdentityUserRole<int>
    {
        public virtual User User {get; set;}
        public virtual Role Role {get; set;}
    }
}