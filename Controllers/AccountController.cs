using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> UserMgr { get; }
        private SignInManager<User> SignInMgr { get; }
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            UserMgr = userManager;
            SignInMgr = signInManager;

        }

        /*----------Register User----------*/
        [HttpGet]
        public ActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserRegistration(User user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Password = user.Password
                };
                var result = await UserMgr.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    await SignInMgr.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Dashboard", "Application");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        /*----------Login----------*/
        [HttpGet]
        public ActionResult UserLogin()
        {
            return View();
        }

        public async Task<IActionResult> UserLogin(LoginVM user)
        {
            var result = await SignInMgr.PasswordSignInAsync(user.UserName, user.Password, false, false);
            if (result.Succeeded)
                return RedirectToAction("Dashboard", "Application");

            else
                View();

            return
                View();
        }

        /*-----------Logout----------*/
        public async Task<IActionResult> Logout()
        {
            await SignInMgr.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /*----------Account Information----------*/
        [Authorize]
        [HttpGet]
        public ActionResult AccountInfo()
        {
            var userId = UserMgr.GetUserId(HttpContext.User);
            var user = UserMgr.FindByIdAsync(userId).Result;
            return View(user);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountInfo(User userUpdate)
        {
            //Get logged in user
            var userId = UserMgr.GetUserId(HttpContext.User);
            var currentUser = UserMgr.FindByIdAsync(userId).Result;

            //Ensure unique email
            string updateEmail = userUpdate.Email;
            if (UserMgr.FindByEmailAsync(updateEmail).Result == null || UserMgr.FindByEmailAsync(updateEmail).Result == currentUser)
                currentUser.Email = userUpdate.Email;
            else
                return View();

            //Password
            if (userUpdate.Password == null)
            {
                currentUser.Password = currentUser.Password;
            }
            else
            {
                currentUser.Password = userUpdate.Password;
                await UserMgr.RemovePasswordAsync(currentUser);
                await UserMgr.AddPasswordAsync(currentUser, userUpdate.Password);
            }

            //Name
            currentUser.FirstName = userUpdate.FirstName;
            currentUser.LastName = userUpdate.LastName;

            IdentityResult x = await UserMgr.UpdateAsync(currentUser);

            if (x.Succeeded)
            {
                return RedirectToAction("Dashboard", "Application");
            }
            else
            {
                ViewBag.ErrorMessage = "Error";
                return View(userUpdate);
            }
        }
    }
}
