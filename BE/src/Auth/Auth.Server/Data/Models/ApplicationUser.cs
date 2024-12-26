using Microsoft.AspNetCore.Identity;

namespace Auth.Server.Data.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? AvatarUrl { get; set; } 
}
