using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLOG.Models
{
    public class kullanicilar : IValidatableObject
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        public string KullaniciAd { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        public string Sifre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [NotMapped]
        public string SifreTekrar { get; set; } = string.Empty;

        public string PPUrl { get; set; } = string.Empty;

        public DateTime Tarih { get; set; } = DateTime.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var context = (BlogContext)validationContext.GetService(typeof(BlogContext));

            if (context != null)
            {
                if (context.Kullanicilar.Any(u => u.KullaniciAd == KullaniciAd))
                {
                    yield return new ValidationResult("Bu kullanıcı adı kayıtlı", new[] { nameof(KullaniciAd) });
                }

                if (context.Kullanicilar.Any(u => u.Email == Email))
                {
                    yield return new ValidationResult("Bu e-posta adresi kayıtlı", new[] { nameof(Email) });
                }
            }
            if (Sifre != SifreTekrar)
            {
                yield return new ValidationResult("Lütfen aynı şifreyi girin", new[] { nameof(SifreTekrar) });
            }
        }
    }
}
