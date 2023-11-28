using Microsoft.Build.Framework;

namespace Core6JwtFront.Models {
    public class LoginInfo {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
