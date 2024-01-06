using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class AdminLoginDTO
    {
        public int Id { get; set; }
        public int AdminLoginId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string UserID { get; set; }
        public string Confirmpassword { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }
    }
    public class TrasnsactionPwdRequestModel
    {
        public string Password { get; set; }
    }
}