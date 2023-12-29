using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<int> AdminLogin_Id { get; set; }
        public string PatientRegNo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PatientName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<int> StateMaster_Id { get; set; }
        public Nullable<int> CityMaster_Id { get; set; }
        public string Location { get; set; }
        public string PinCode { get; set; }
        public Nullable<System.DateTime> Reg_Date { get; set; }
        public Nullable<double> walletAmount { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public IEnumerable<ProfileDetail> PatientList { get; set; }
    }

    public class ProfileDetail
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string Location { get; set; }
        public string PinCode { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }

    }

    public class EditProfile
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string Location { get; set; }
        public string PinCode { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<int> StateMaster_Id { get; set; }
        public Nullable<int> CityMaster_Id { get; set; }

    }

    public class UserWallet
    {

        public int UserId { get; set; }
        public Nullable<double> walletAmount { get; set; }

    }

    
}