using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMBRD.Models.ViewModels
{
    public class AmbulanceDTO
    {
        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleName { get; set; }
        public string VehicleOwnerName { get; set; }
        public int? Driver_Id { get; set; }
        public Nullable<int> VehicleCat_Id { get; set; }
        public Nullable<int> VehicleType_Id { get; set; }
        public Nullable<System.DateTime> Validity { get; set; }
        public Nullable<System.DateTime> InsurranceDate { get; set; }
        public Nullable<System.DateTime> PollutionDate { get; set; }
        public Nullable<double> DriverCharges { get; set; }
        public string VehicleImg { get; set; }
        public string CancelChequeFile { get; set; }
        public HttpPostedFileBase CancelChequeFileBase64 { get; set; }

        public string RCImage { get; set; }
        public HttpPostedFileBase RCImageFileBase64 { get; set; }
        public HttpPostedFileBase VehicleCImageFileBase64 { get; set; }
        public Nullable<System.DateTime> RC_Validity { get; set; }
        public string RC_Number { get; set; }

        public SelectList Drivers { get; set; }
        public SelectList CategoryList { get; set; }
        public SelectList DriverList { get; set; }
        public SelectList VehicleTypelist { get; set; }
        public List<SelectListItem> VehicleTypes { get; set; }
        public IEnumerable<VehicleList> VehicleList { get; set; }
        public string CategoryName { get; set; }
        public string Type { get; set; }
        public string VehicleTypeName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string HolderName { get; set; }
    }
    public class VehicleList
    {
        public int Id { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleName { get; set; }
        public string VehicleOwnerName { get; set; }
        public Nullable<int> Driver_Id { get; set; }
        public Nullable<int> VehicleCat_Id { get; set; }
        public Nullable<int> VehicleType_Id { get; set; }
        public Nullable<System.DateTime> Validity { get; set; }
        public Nullable<System.DateTime> InsurranceDate { get; set; }
        public Nullable<System.DateTime> PollutionDate { get; set; }
        public Nullable<double> DriverCharges { get; set; }
        public string VehicleImg { get; set; }
        public string CancelChequeFile { get; set; }
    }

}