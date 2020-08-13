using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DCISM_WBRMSystem.Models
{
    public class Account
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}