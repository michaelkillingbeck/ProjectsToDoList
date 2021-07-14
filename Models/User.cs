namespace ProjectsToDoList.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    
    public class User : IdentityUser
    {
        public override String ToString()
        {
            return $"Username: {this.UserName}, Normalized: {this.NormalizedUserName}";
        }
    }
}