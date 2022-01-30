﻿using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email

            };
            user.IsActive = true;
            IdentityResult identity = await _userManager.CreateAsync(user, registerVM.Password);
            if (!identity.Succeeded)
            {
                foreach (var item in identity.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Member");
            await _signInManager.SignInAsync(user, true);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult CheckInSign()
        {

            return Content(User.Identity.IsAuthenticated.ToString());

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser dbUser = await _userManager.FindByIdAsync(loginVM.UserName);

            if (dbUser == null)
            {
                ModelState.AddModelError("", "UserName or Password Invalid");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(dbUser, loginVM.Password, true, true);

            if (!dbUser.IsActive)
            {
                ModelState.AddModelError("", "user is a deactive");
            }
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "is lockout");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password Invalid");
                return View();
            }
            var roles = await _userManager.GetRolesAsync(dbUser);

            if (roles[0]=="Admin")
            {
                return RedirectToAction("Index","Dashboard",new {area="AdminArea"});
            }

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");


        }







        public IActionResult Index()
        {
            return View();
        }


        public async Task CreateRole()
        {
            if (! await _roleManager.RoleExistsAsync("Admin"))
            {

                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }
            if (! await _roleManager.RoleExistsAsync("Member")) 
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });

            }


        }









        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> ForgetPassword(ForgetPassword forget)
        {
            AppUser user = await _userManager.FindByEmailAsync(forget.User.Email);
            if (user == null) NotFound();


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token }, Request.Scheme);
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("guntebrustemov@gmail.com", "Reset");
                mail.To.Add(user.Email);
                mail.Subject = "Reset Password";
                mail.Body = $"<a href={link}>Go to Reset Password</a>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("guntebrustemov", "gunteb7@");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                }
                return RedirectToAction("Home", "Index");
            }
        }

        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) NotFound();

            ForgetPassword forgetPassword = new ForgetPassword
            {
                Token = token,
                User = user
            };
            return View(forgetPassword);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("ResetPassword")]

        public async Task<IActionResult> ResetPassword(ForgetPassword model)
        {
            AppUser user = await _userManager.FindByEmailAsync(model.User.Email);
            if (user == null) NotFound();

            ForgetPassword forgetPassword = new ForgetPassword
            {
                Token = model.Token,
                User = user
            };
            //if (!ModelState.IsValid) return View(forgetPassword);


            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);

            }
            return RedirectToAction("Index", "Home");
        }
    }
}
