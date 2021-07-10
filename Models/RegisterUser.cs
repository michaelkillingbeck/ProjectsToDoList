namespace ProjectsToDoList.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RegisterUser
    {        
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required]
        public String UserName { get; set; }
    }
}