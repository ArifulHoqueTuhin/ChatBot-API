using Microsoft.AspNetCore.Identity;

namespace ChatBot_API.Models
{
    public class ApplicationUser : IdentityUser
    {    
         public string Name { get; set; } = default!;
    }
}
