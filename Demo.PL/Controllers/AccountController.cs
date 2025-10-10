using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;

        public AccountController(UserManager<AuthUser> userManager, SignInManager<AuthUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region Register
        // BaseURL/Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var User = new AuthUser()
                {
                    UserName = model.Email.Split("@")[0],
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree,
                };
                var Result = await _userManager.CreateAsync(User, model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var Error in Result.Errors)
                        ModelState.AddModelError(string.Empty, Error.Description);
                }
            }
            return View(model);
        }
        #endregion

        #region Sign In - Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    // Login
                    var Result = await _userManager.CheckPasswordAsync(User, model.Password);
                    if (Result)
                    {
                        // Login
                        var LoginResult = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false); // Create Token
                        if (LoginResult.Succeeded)
                            return RedirectToAction("Index", "Home");

                    }
                    else
                        ModelState.AddModelError(string.Empty, "Password is Incorrect!");

                }
                else
                    ModelState.AddModelError(string.Empty, "Email is not Exists");
            }
            return View(model);
        }
        #endregion

        #region Sign Out
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region Forget Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPassswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var Token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    // Valid For Only One Time For This User!
                    // https://localhost:44315/Account/ResetPassword?email=yehiadakhly2004@gmail.com?Token=ajkbgfnkkldengnlkndnklkgfdsn
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, token = Token }, Request.Scheme);
                    var Email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = ResetPasswordLink
                    };
                    EmailSettings.SendEmail(Email);
                    return RedirectToAction(nameof(CheckYourIndox));
                }
                else
                    ModelState.AddModelError(string.Empty, "Email is not Exists!");
            }
            return View("ForgetPassword", model);
            // Reset Password
        }
        public IActionResult CheckYourIndox()
        {
            return View();
        }
        #endregion

        #region Reset Password
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid) 
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var User = await _userManager.FindByEmailAsync(email);
                var Result = await _userManager.ResetPasswordAsync(User, token, model.NewPassword);
                if (Result.Succeeded) 
                    return RedirectToAction(nameof(Login));
                else
                    foreach (var Error in Result.Errors)
                        ModelState.AddModelError(string.Empty, Error.Description);
            }
            return View(model);
        }
        #endregion
    }
}
// P@ssw0rd
// Pa$$w0rd 
