using System.ComponentModel.DataAnnotations;

namespace ShopDemo.Web.Models
{
    public class LoginModel
    {
        public string? ReturnUrl { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
