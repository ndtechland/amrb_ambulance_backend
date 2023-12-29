using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class ChangepasswordDTO
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Confirmpassword { get; set; }
    }
}