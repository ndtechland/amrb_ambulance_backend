using AMBRD.Models.CommonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class HospitalDTO: StateCityAbs
    {
        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public string HospitalName { get; set; }
        public Nullable<int> StateMaster_Id { get; set; }
        public Nullable<int> CityMaster_Id { get; set; }
        public string MobileNumber { get; set; }
        public string LandlineNumber { get; set; }
        public string EmailId { get; set; }
        public string Location { get; set; }
        public string PinCode { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string HospitalId { get; set; }
        public Nullable<int> AdminLogin_Id { get; set; }
        public string HospitalImage { get; set; }
        public HttpPostedFileBase HospitalImageFile { get; set; }
    }

    public class AssociateHospital  
    {
        public int Id { get; set; } 
        public string HospitalName { get; set; }  
        public string Location { get; set; } 
        public string HospitalImage { get; set; } 
    }
}