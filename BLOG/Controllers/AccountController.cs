using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BLOG.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace BLOG.Controllers
{
    public class AccountController : Controller
    {
        private readonly BlogContext _context;
        private readonly string _webRootPath;

        public AccountController(BlogContext context, IWebHostEnvironment env)
        {
            _context = context;
            _webRootPath = env.WebRootPath; // wwwroot yolunu almak için
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Giriş formunu göstermek için
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View(); // Kayıt formunu göstermek için
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(kullanicilar model, IFormFile? profilFoto)
        {
            if (ModelState.IsValid)
            {
                // Şifreyi hashle ve veritabanına kaydet
                model.Sifre = HashPassword(model.Sifre);

                // Profil fotoğrafı yükleme işlemi
                if (profilFoto != null && profilFoto.Length > 0)
                {
                    var fileName = Path.GetFileName(profilFoto.FileName);
                    var filePath = Path.Combine(_webRootPath, "images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilFoto.CopyToAsync(fileStream);
                    }

                    model.PPUrl = $"/images/{fileName}";
                }

                // Kullanıcıyı veritabanına ekle
                _context.Kullanicilar.Add(model);
                await _context.SaveChangesAsync();

                // Başarıyla kayıt yapıldığını belirten bir mesaj veya yönlendirme
                return RedirectToAction("Register"); // Kayıttan sonra giriş sayfasına yönlendir
            }

            return View(model); // Hatalı form gönderildiğinde tekrar göster
        }

        [HttpPost]
        public IActionResult Register(kullanicilar model)
        {
            // Kullanıcı adı ve şifreyi doğrulama
            var user = _context.Kullanicilar
                .FirstOrDefault(u => u.KullaniciAd == model.KullaniciAd);

            if (user != null && VerifyPassword(model.Sifre, user.Sifre))
            {
                // Giriş başarılı, Index sayfasına yönlendirme
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Giriş başarısız, hata mesajı gösterme
                ModelState.AddModelError("Sifre", "Kullanıcı adı veya şifre yanlış.");
                return View("Register", model); // Giriş formunu tekrar göster
            }
        }

        private string HashPassword(string password)
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            var hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32);

            var hashedPassword = Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hashed);
            return hashedPassword;
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            var parts = storedPassword.Split(':');
            if (parts.Length != 2)
                return false;

            var storedSalt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]);

            var enteredHash = KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: storedSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32);

            return enteredHash.SequenceEqual(storedHash);
        }
    }
}







