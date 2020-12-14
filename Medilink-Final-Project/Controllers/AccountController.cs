using Medilink_Final_Project.Models;
using Medilink_Final_Project.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser
            {
                FullName = register.FullName,
                UserName = register.UserName,
                Email = register.Email
            };
            IdentityResult identityResult = await _userManager.CreateAsync(user, register.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }

            await _userManager.AddToRoleAsync(user, "Member");
            await _signInManager.SignInAsync(user, true);

            return RedirectToAction("Index", "Home");
        }

        
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByEmailAsync(login.Email);
            if(user == null)
            {
                ModelState.AddModelError("", "Email or Password wrong!");
                return View(login);
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, login.IsRemember, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "The account is locked out 5 minute!");
                return View(login);
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password wrong!");
                return View(login);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
