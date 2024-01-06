using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    public class DriverPaymentHis
    {
        public int Id { get; set; }
        public string DriverName { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<double> start_Lat { get; set; }
        public Nullable<double> start_Long { get; set; }
        public Nullable<double> end_Lat { get; set; }
        public Nullable<double> end_Long { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        //CODE FOR LAT LONG TO LOCATION 
        public string PickupLocation
        {
            get { return getlocation(start_Lat.ToString(), start_Long.ToString()); }
        }
        public string DropLocation
        {
            get { return getlocation(end_Lat.ToString(), end_Long.ToString()); }
        }


        private string getlocation(string latitude, string longitude)
        {

            string url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + latitude + "," + longitude + "&key=AIzaSyBrbWFXlOYpaq51wteSyFS2UjdMPOWBlQw";

            // Make the HTTP request.
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 10000;

            // Get the response.
            WebResponse response = request.GetResponse();
            string responseText = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Parse the response JSON.
            var json = JsonConvert.DeserializeObject<dynamic>(responseText);

            // Get the location from the JSON.
            var location = json.results[0].formatted_address;
            return location;
        }

        //END CODE FOR LAT LONG TO LOCATION 
        public string Location { get; set; }
        public Nullable<decimal> Amount { get; set; }


    }


}