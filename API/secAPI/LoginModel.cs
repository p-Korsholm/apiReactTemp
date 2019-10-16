using System;
using System.ComponentModel.DataAnnotations;

namespace secAPI
{
    public class LoginModel
    {
        [Required(ErrorMessage = "username cannot be empty")]
        public string username { get; set; }
        [Required(ErrorMessage = "password cannot be empty")]
        public string password { get; set; }
    }
}
