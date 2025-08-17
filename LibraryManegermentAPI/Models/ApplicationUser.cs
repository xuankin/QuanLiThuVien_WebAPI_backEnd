using Microsoft.AspNetCore.Identity;

namespace LibraryManegermentAPI.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? Initials { get; set; }
    }   

}
