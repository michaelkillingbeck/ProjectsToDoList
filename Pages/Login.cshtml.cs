namespace ProjectsToDoList.Pages
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using ProjectsToDoList.Models;
    using System.Threading.Tasks;
    using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly SignInManager<User> _signInManager;

        [BindProperty]
        public LoginUser SignInUser { get; set; }

        public LoginModel(ILogger<LoginModel> logger, SignInManager<User> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                SignInResult result = 
                    await _signInManager.PasswordSignInAsync(SignInUser.UserName, SignInUser.Password, false, false);

                if(result.Succeeded)
                {
                    return RedirectToPage("Index");
                }

                ModelState.AddModelError(string.Empty, "Login attempted failed");
            }

            return Page();
        }
    }
}