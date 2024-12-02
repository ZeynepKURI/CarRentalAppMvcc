// Controllers/AccountController.cs
using CarRentalAppMvc.Data;
using CarRentalAppMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalAppMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Users> _passwordHasher;

        public AccountController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Users>();
        }

        // Register (Kayıt)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Users user)
        {
            if (ModelState.IsValid)
            {
                // Şifreyi hash'leyelim
                user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);
                _context.users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // Login (Giriş)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            var user = _context.users.FirstOrDefault(u => u.UserName == userName);
            if (user != null)
            {
                // Şifreyi doğrula
                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    // Kullanıcı giriş yaptı, burada session veya token kullanabilirsiniz
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Geçersiz şifre.";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Geçersiz kullanıcı adı.";
            }
            return View();
        }

        // Logout (Çıkış)
        public IActionResult Logout()
        {
            // Çıkış işlemleri yapılabilir, örneğin session silme
            return RedirectToAction("Login");
        }
    }
}

