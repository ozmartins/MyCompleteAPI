using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required()]
        [EmailAddress()]
        public string Email { get; set; }
        
        [Required()]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }        
        
        [Required()]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginUserViewModel
    {
        [Required()]
        [EmailAddress()]
        public string Email { get; set; }

        [Required()]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }        
    }

    public class UserTokenViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<ClaimViewModel> Claims { get; set; }
    }

    public class LoginResponseViewModel
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserTokenViewModel UserToken { get; set; }
    }

    public class ClaimViewModel
    {
        public string Value { get; set; }
        public string Type { get; set; }        
    }


}
