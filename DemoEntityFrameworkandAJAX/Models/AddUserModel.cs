using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoEntityFrameworkandAJAX.Models
{
    public class AddUserModel
    {
        [Required]
        public string UserName { set; get; }
        public string FullName { set; get; }
        [Required]
        public string Password { set; get; }
        [Compare("Password", ErrorMessage ="passwords dont match")]
        public string ConfirmPassword { set; get; }
    }
}