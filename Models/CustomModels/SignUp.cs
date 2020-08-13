using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCISM_WBRMSystem.Models.CustomModels
{
    public class SignUp
    {
        public string IdNumber { get; set; }
        public string Usertype { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string CourseYr { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        
    }
}