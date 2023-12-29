//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AMBRD.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Driver
    {
        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<int> AdminLogin_Id { get; set; }
        public string DriverName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public Nullable<int> StateMaster_Id { get; set; }
        public Nullable<int> CityMaster_Id { get; set; }
        public string Location { get; set; }
        public string PinCode { get; set; }
        public string DriverImage { get; set; }
        public string DlImage { get; set; }
        public string DlNumber { get; set; }
        public string DlImage1 { get; set; }
        public string DlImage2 { get; set; }
        public string DlImage3 { get; set; }
        public Nullable<System.DateTime> DlValidity { get; set; }
        public string PanNumbar { get; set; }
        public string PanImage { get; set; }
        public string AadharNumber { get; set; }
        public string AadharImage { get; set; }
        public string AadharImage2 { get; set; }
        public Nullable<int> VehicleType_Id { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<int> charge { get; set; }
        public Nullable<double> Lat { get; set; }
        public Nullable<double> Long { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<int> Ambulance_Id { get; set; }
        public string Gender { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
