using Electronic_Mall.Data;
using Electronic_Mall.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Electronic_Mall.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        //  /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

     
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //+User
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    // Userid Roleid Username Passwordhash Email Role
                    Username = model.Username,
                    Email = model.Email,
                    Passwordhash = _passwordHasher.HashPassword(null, model.Password),
                    Roleid = GetDefaultRoleId() 
                };

                _context.Add(user);
                await _context.SaveChangesAsync();
                //TempData بدلاً من ViewBag.success = "it added";
                //لنقل الرسائل بين الـ Actions
                // إعداد رسالة نجاح لعرضها في الـ View
                TempData["SuccessMessage"] = "تم إضافة المستخدم بنجاح!"; 
             
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        private int GetDefaultRoleId()
        {
               // الرقم 2 يمثل "Regular_user" في الداتا بيس
                         return 2;

         /*   var defaultRole = _context.UserRoles.FirstOrDefault(r => r.Rolename == "Regular_user");
            if (defaultRole != null)
            {
                return defaultRole.Roleid;
            }
            else
            {
                throw new Exception("Default role 'Regular_user' not found. Please ensure it is created in the database.");
            }*/
        }


        //Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            model.Username = "marah"; 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                                         .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user != null && VerifyPasswordHash(model.Password, user.Passwordhash))
                {
                    // Use cookies to maintain authenticated user status between requests.
                    // This method creates encrypted authentication cookies that are managed by ASP.NET Core.
                    // Cookies are the mechanism used here instead of HTTP sessions because sessions store data on the server and require
                    // And manage user sessions across requests without having to re-authenticate with each request.
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("UserId", user.Userid.ToString()),
              
                };

                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة.");
                }
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            
            return true;
        }
    }
}