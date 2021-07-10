namespace ProjectsToDoList.Pages
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.Models;
    using System;
    using System.Threading.Tasks;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {       
        private readonly ILogger<RegisterModel> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UserManager<User> _userManager;

        [BindProperty]
        public RegisterUser NewUser { get; set; }

        public RegisterModel(ILogger<RegisterModel> logger, IPasswordHasher<User> passwordHasher, UserManager<User> userManager)
        {
            _logger = logger;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                User newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    NormalizedUserName = NewUser.UserName,
                    UserName = NewUser.UserName
                };
                
                Boolean validPassword = true;
                
                foreach(IPasswordValidator<User> passwordValidator in _userManager.PasswordValidators)
                {
                    IdentityResult result = await passwordValidator.ValidateAsync(_userManager, null, NewUser.Password);

                    if(result.Succeeded == false)
                    {
                        validPassword = false;
                        break;
                    }
                } 

                if(validPassword == false)
                {
                    ModelState.AddModelError("NewUser.Password", "Password is not valid");
                    return Page();
                }

                String hashedPasword = _passwordHasher.HashPassword(newUser, NewUser.Password);
                newUser.PasswordHash = hashedPasword;

                await _userManager.CreateAsync(newUser);

                return RedirectToPage("Login");
            }
            else
            {
                return Page();
            }
        }
    }
}
