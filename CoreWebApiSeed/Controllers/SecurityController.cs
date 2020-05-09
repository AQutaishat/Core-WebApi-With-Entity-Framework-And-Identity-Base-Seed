using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApiWithEntityFrameworkAndIdentityBaseSeed.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
       
        public SecurityController(SignInManager<IdentityUser>  signInManager, UserManager<IdentityUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public async Task<bool> Login(string UserName,  string Password)
        {
            var result = await _signInManager.PasswordSignInAsync(UserName, Password, isPersistent: true, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                //_logger.LogInformation("User logged in.");
                //return LocalRedirect(returnUrl);
                //var user = await _userManager.FindByEmailAsync(Input.Email);
                //var user = _usersService.FindByNameOrEmail(UserName);
                return true;
            }
            if (result.RequiresTwoFactor)
            {
                //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                throw new Exception("RequiresTwoFactor");
            }
            if (result.IsLockedOut)
            {
                //_logger.LogWarning("User account locked out.");
                //return RedirectToPage("./Lockout");
                throw new Exception("User account locked out.");
            }
            else
            {
                //var user = await _usersService.FindByNameOrEmail(UserName);
                //if (user != null)
                //{
                //    await _signInManager.SignInAsync(user, true);
                //    return user;
                //}

                throw new Exception("Invalid login attempt.");
            }

        }

        [HttpGet]
        [Route("Logout")]
        public async void Logout()
        {
            await _signInManager.SignOutAsync();
        }

        [HttpGet]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IdentityUser> Register(string UserName, string Email, string Password)
        {
            var User = new IdentityUser()
            {
                UserName = UserName,
                //PhoneNumber = inputs.PhoneNumber,
                Email = Email,
                PasswordHash = Password
            };
            var Result= await this.Add(User);
            return Result;
        }

        [NonAction]
        private async Task<IdentityUser> Add(IdentityUser Entity)
        {
            //------------------------------------------------
            //Validation
            if (String.IsNullOrWhiteSpace(Entity.UserName))
            {
                throw new Exception("User Already Exists");

            }

            var usr =await _userManager.FindByNameAsync(Entity.UserName?.Trim());
            if (usr != null)
            {
                throw new Exception("User Already Exists");
            }

            //------------------------------------------------
            //Creation
            var CreateResult = _userManager.CreateAsync(Entity, Entity.PasswordHash).Result;
            if (!CreateResult.Succeeded)
            {
                var Errors = String.Join(", ", CreateResult.Errors.Select(x => x.Description).ToArray());
                throw new Exception("Error Creating New User, " + Errors);
            }
            //------------------------------------------------
            //Return Result
            var Result = _userManager.FindByNameAsync(Entity.UserName);


            return await Result;
        }
    }
}
