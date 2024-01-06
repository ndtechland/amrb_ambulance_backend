using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace AMBRD.Models.ApiModels
{
    public class ComplaintsDTO
    {
        public int Id { get; set; }
        public string Subjects { get; set; }
        public string Complaints { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsResolved { get; set; }
        public string Others { get; set; }
        public Nullable<int> Login_Id { get; set; }
        public string Roles { get; set; }
        public Nullable<System.DateTime> ComplaintDate { get; set; }
        public Nullable<int> patsubid { get; set; }
    }

    public class BookingAmbulanceAcceptRejectDTO
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int PatientId { get; set; }
        public decimal Amount { get; set; }
        public string StatusId { get; set; }
        public Nullable<bool> RejectedStatus { get; set; }
    }
    public class getdriverbookinglist
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string MobileNumber { get; set; }
        public string DriverImage { get; set; }
        public string DlNumber { get; set; }
        public Nullable<int> TotalPrice { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> RemainingAmount { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleTypeName { get; set; }
        public Nullable<int> TotalDistance { get; set; }
        public Nullable<double> Lat_Driver { get; set; }
        public Nullable<double> Lang_Driver { get; set; }
        public Nullable<double> UserLat { get; set; }
        public Nullable<double> UserLong { get; set; }
        public Nullable<double> start_Lat { get; set; }
        public Nullable<double> start_Long { get; set; }
        public Nullable<double> end_Lat { get; set; }
        public Nullable<double> end_Long { get; set; }
        public Nullable<double> DriverUserDistance { get; set; }
        public double ExpectedTime { get; set; }
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

    }
    public class GetAcceptedReq_DriverDetail
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string MobileNumber { get; set; }
        public string DriverImage { get; set; }
        public string DlNumber { get; set; }
        public Nullable<int> TotalPrice { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleTypeName { get; set; }
        public Nullable<int> TotalDistance { get; set; }
        public string DeviceId { get; set; }
        public Nullable<double> Lat_Driver { get; set; }
        public Nullable<double> Lang_Driver { get; set; }
        public Nullable<double> end_Long { get; set; }
        public Nullable<double> end_Lat { get; set; }

    }
    public class BookingHistory
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string MobileNumber { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Location { get; set; }
        public string PinCode { get; set; } 


    }
    public class driverpayhistory
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string MobileNumber { get; set; }
        public string Location { get; set; }
        public int PaymentId { get; set; }
        public Nullable<int> TotalPrice { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string IsPay { get; set; }

        public Nullable<double> start_Lat { get; set; }
        public Nullable<double> start_Long { get; set; }
        public Nullable<double> end_Lat { get; set; }
        public Nullable<double> end_Long { get; set; }
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
    }
}