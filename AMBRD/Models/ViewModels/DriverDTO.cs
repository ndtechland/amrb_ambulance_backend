using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class DriverDTO
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
        public string DriverImageBase64 { get; set; }
        public string DlImage { get; set; }
        public string DlImageBase64 { get; set; }
        public string DlImage1Base64 { get; set; }
        public string DlNumber { get; set; }
        public string DlImage1 { get; set; }
        public string DlImage2 { get; set; }
        public string DlImage3 { get; set; }
        public Nullable<System.DateTime> DlValidity { get; set; }
        public string PanNumbar { get; set; }
        public string PanImage { get; set; }
        public string PanImageBase64 { get; set; }
        public string AadharNumber { get; set; }
        public string AadharImage { get; set; }
        public string AadharImage2 { get; set; }
        public string AadharImageBase64 { get; set; }
        public string AadharImage2Base64 { get; set; }
        public Nullable<int> VehicleCatId { get; set; }
        public Nullable<int> VehicleType_Id { get; set; }
        public Nullable<double> Paidamount { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<int> charge { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public Nullable<bool> IsActiveStatus { get; set; }

        public class DriverProfileDetail
        {
            public int Id { get; set; }
            public string DriverName { get; set; }
            public string EmailId { get; set; }
            public string MobileNumber { get; set; }
            public string Location { get; set; }
            public string PinCode { get; set; }
            public string StateName { get; set; }
            public string CityName { get; set; }
            public string DriverImage { get; set; }
            public Nullable<System.DateTime> DOB { get; set; }


        }

        public class DriverEditProfile
        {
            public int Id { get; set; }
            public string DriverName { get; set; }
            public string EmailId { get; set; }
            public string MobileNumber { get; set; }
            public string Location { get; set; }
            public string PinCode { get; set; }
            public string StateName { get; set; }
            public string CityName { get; set; }
            public Nullable<int> StateMaster_Id { get; set; }
            public Nullable<int> CityMaster_Id { get; set; }

        }
    }

    public class DriverListNearByUser
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int KM { get; set; }
        public string Name { get; set; }
        public string DL { get; set; }
        public Nullable<int> Charge { get; set; }
        public Nullable<int> TotalPrice { get; set; }
        public string DeviceId { get; set; }
        public Nullable<int> TotalDistance { get; set; }
    }

    public class DriverLocationDT
    {
        public int Id { get; set; }

        public Nullable<double> end_Lat { get; set; }
        public Nullable<double> end_Long { get; set; }
        public Nullable<double> start_Lat { get; set; }
        public Nullable<double> start_Long { get; set; }

        public Nullable<int> Patient_Id { get; set; }
        public Nullable<int> Driver_Id { get; set; } 
        public Nullable<int> VehicleType_id { get; set; }
        public string DriverName { get; set; }
        public string DlNumber { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public int DriverId { get; set; }
        public string DeviceId { get; set; }
        public Nullable<int> NoOfPassengers { get; set; }
        public string offer { get; set; }
        public int OfferPrice { get; set; }
        public Nullable<int> charge { get; set; }
    }
    public class UpdateDriverLocation
    {
        public int DriverId { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string DriverName { get; set; }
        public string DlNumber { get; set; }
        public string DeviceId { get; set; }

    }
    public class UserListForBookingAmbulance
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string MobileNumber { get; set; }
        public double endLat { get; set; }
        public double endLong { get; set; }
        public double startLat { get; set; }
        public double startLong { get; set; }
        public string DeviceId { get; set; }
        public Nullable<int> TotalPrice { get; set; }
        public Nullable<int> TotalDistance { get; set; }
        public Nullable<int> NoOfPassengers { get; set; }
        public int OfferPrice { get; set; }


        //CODE FOR LAT LONG TO LOCATION 
        public string ReverseStartLatLong_To_Location
        {
            get { return getlocation(startLat.ToString(), startLong.ToString()); }
        }
        public string ReverseEndLatLong_To_Location
        {
            get { return getlocation(endLat.ToString(), endLong.ToString()); }
        }
        public string Duration
        {
            get { return getlocation(endLat.ToString(), endLong.ToString()); }
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
    }

    public class VehiclePrice
    {
        public Nullable<double> under0_2KM { get; set; }
        public Nullable<double> under3_8KM { get; set; }
        public Nullable<double> under9_15KM { get; set; }
        public Nullable<double> under16_25KM { get; set; }
        public Nullable<double> under26_40KM { get; set; }
        public Nullable<double> under41_60KM { get; set; }
        public Nullable<double> under61_80KM { get; set; }
        public Nullable<double> under81_110KM { get; set; }
        public Nullable<double> under111_150KM { get; set; }
        public Nullable<double> under151_200KM { get; set; }
        public Nullable<double> under201_250KM { get; set; }
        public Nullable<double> under251_300KM { get; set; }
        public Nullable<double> under301_350KM { get; set; } 
        public Nullable<double> Above500KM { get; set; }
    }
}