using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class LoginModel
    {
        public string Password { get; set; }
        public string UserID { get; set; }
        public string MobileNumber { get; set; }
        public int OTP { get; set; }

    }
}