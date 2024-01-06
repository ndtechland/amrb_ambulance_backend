using AMBRD.Models;
using AMBRD.Models.ApiModels;
using AMBRD.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results; 
using System.Web.Services.Description;
using ReturnMessage = AMBRD.Models.ApiModels.ReturnMessage;
using static AMBRD.Models.ViewModels.DriverDTO;
using System.Web.Razor.Parser.SyntaxTree;

namespace AMBRD.API
{
    public class DriverApiController : ApiController
    {
        abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        ReturnMessage rm = new ReturnMessage();

        [HttpGet]
        [Route("api/DriverApi/GetDriverProfile")]
        public IHttpActionResult GetDriverProfile(int Id)
        {
            string qry = @"select d.Id,d.DriverName,d.DriverImage,d.DOB,d.EmailId,d.MobileNumber,sm.StateName,cm.CityName,d.Location,d.PinCode from Driver as d
join StateMaster as sm on sm.Id=d.StateMaster_Id
join CityMaster as cm on cm.Id=d.CityMaster_Id where d.Id=" + Id + "";
            var DriverProfile = ent.Database.SqlQuery<DriverProfileDetail>(qry).FirstOrDefault();
            return Ok(DriverProfile);
        }

        [HttpPost]
        [Route("api/DriverApi/EditDriverProfile")]
        public IHttpActionResult EditDriverProfile(DriverEditProfile model)
        {
            try
            {
                var rm = new ReturnMessage();
                var data = ent.Drivers.Find(model.Id);
                if (data == null)
                {
                    return BadRequest("No record found");
                }
                data.DriverName = model.DriverName; 
                data.CityMaster_Id = model.CityMaster_Id;
                data.Location = model.Location;
                data.StateMaster_Id = model.StateMaster_Id;
                data.PinCode = model.PinCode;
                ent.SaveChanges();
                rm.Status = 1;
                rm.Message = "Profile has updated.";
                return Ok(rm);
            }
            catch (Exception)
            {
                return BadRequest("Server error");
            }

        }

        [HttpPost, Route("api/DriverApi/UpdateDriverLocation")]
        public IHttpActionResult UpdateDriverLocation(UpdateDriverLocation model)
        {
            try
            {
                var data = ent.Drivers.FirstOrDefault(a => a.Id == model.DriverId);
                if (data == null)
                {
                    return NotFound();
                }
                data.Lat = model.Lat;
                data.Long = model.Long;
                ent.SaveChanges();
                return Ok(new { Message = "Update Driver Location SuccessFully" });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Server Error")); 
            }
        } 

        //Post Api Get Near Driver By VehicleTypeId
        [HttpPost, Route("api/DriverApi/GetNearDriverByVehicleTypeId")]
        public IHttpActionResult GetNearDriverByVehicleTypeId(DriverLocationDT model)
        {
            try
            {
                double DriveLat = 0.00;
                double DriveLong = 0.00;
                double distance = 0.00;
                int Charge = 0;
                ent.Database.ExecuteSqlCommand("exec DeleteNearDriver");

                List<DriverListNearByUser> driverListNearByUser = new List<DriverListNearByUser>();

                var Driver = ent.Database.SqlQuery<DriverLocationDT>(@"select distinct D.Id AS DriverId,D.Lat, D.Long,D.DriverName,D.charge,D.DlNumber,AL.DeviceId from Driver AS D with(nolock)
INNER JOIN AdminLogin AS AL with(nolock) ON D.AdminLogin_Id = AL.Id
INNER JOIN Ambulance as v on V.VehicleType_Id=d.VehicleType_id
INNER JOIN VehicleType as vt on Vt.Id=v.VehicleType_id
where D.Lat IS NOT NULL and D.Long IS NOT NULL and d.VehicleType_id=" + model.VehicleType_id + " and D.IsApproved=1 and d.IsActive=1").ToList();
                foreach (var item in Driver)
                {
                    //==================USER AND DRIVER DISTANCE========================
                    // Driver

                    var lat1 = item.Lat;
                    var lon1 = item.Long;

                    //Save DriverLat and Long 
                    DriveLat = lat1;
                    DriveLong = lon1;

                    //User
                    var lat2 = model.start_Lat;
                    var lon2 = model.start_Long;

                    double rlat1 = Math.PI * lat1 / 180;
                    double rlat2 = (double)(Math.PI * lat2 / 180);
                    double theta = (double)(lon1 - lon2);
                    double rtheta = Math.PI * theta / 180;
                    double dist =
                        Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                        Math.Cos(rlat2) * Math.Cos(rtheta);
                    dist = Math.Acos(dist);
                    dist = dist * 180 / Math.PI;
                    dist = dist * 60 * 1.1515;
                    //dist is km

                    // TOTAL DISTANCE (START LOCTION TO DESTINATION)

                    //User
                    var Startlat = model.start_Lat;
                    var Startlong = model.start_Long;
                    var Endlat = model.end_Lat;
                    var Endlong = model.end_Long;


                    // Create the URL for the Google Maps API.
                    string url = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + Startlat + "," + Startlong + "&destinations=" + Endlat + "," + Endlong + "&key=AIzaSyBrbWFXlOYpaq51wteSyFS2UjdMPOWBlQw";

                    // Make the HTTP request to the API.
                    WebRequest request = WebRequest.Create(url);
                    request.Method = "GET";
                    request.Timeout = 30000;
                    WebResponse response = request.GetResponse();

                    string responseText = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    // Parse the response JSON.
                    var json = JsonConvert.DeserializeObject<dynamic>(responseText);

                    // Get the distance between the two places.
                    var distancetext = ExtractDecimalValue((json.rows[0].elements[0].distance.value).ToString());

                    distance = json.rows[0].elements[0].distance.value / 1000;

                    Console.WriteLine("The distance between the two places is " + distance + " meters.");

                    ;
                    int drivId = item.DriverId;
                    string drivName = item.DriverName;
                    string DlNumber = item.DlNumber;
                    //Charge = item.charge ?? 0;
                    //IF the Distance is between 0 to 2
                    if (distance <= 2)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under0_2KM;
                    }
                    //IF the Distance is between 3 to 8
                    else if (distance > 3 && distance < 8)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under3_8KM;
                    }
                    //IF the Distance is between 9 to 15
                    else if (distance > 9 && distance < 15)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under9_15KM;
                    }

                    //IF the Distance is between 16 to 25
                    else if (distance > 16 && distance < 25)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under16_25KM;
                    }

                    //IF the Distance is between 26 to 40
                    else if (distance > 26 && distance < 40)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under26_40KM;
                    }

                    //IF the Distance is between 41 to 60
                    else if (distance > 41 && distance < 60)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under41_60KM;
                    }
                    //IF the Distance is between 61 to 80
                    else if (distance > 61 && distance < 80)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under61_80KM;
                    }
                    //IF the Distance is between 81 to 110
                    else if (distance > 81 && distance < 110)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under81_110KM;
                    }
                    //IF the Distance is between 111 to 150
                    else if (distance > 111 && distance < 150)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under111_150KM;
                    }
                    //IF the Distance is between 151 to 200
                    else if (distance > 151 && distance < 200)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under151_200KM;
                    }
                    //IF the Distance is between 201 to 250
                    else if (distance > 201 && distance < 250)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under201_250KM;
                    }
                    //IF the Distance is between 251 to 300
                    else if (distance > 251 && distance < 300)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under251_300KM;
                    }
                    //IF the Distance is between 301 to 350
                    else if (distance > 301 && distance < 350)
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().under301_350KM;
                    }
                     
                    //IF the Distance is Above 500
                    else
                    {
                        string Q = @"select * from VehicleType where Id=" + model.VehicleType_id;
                        var data = ent.Database.SqlQuery<VehiclePrice>(Q).ToList(); 
                        Charge = (int)data.FirstOrDefault().Above500KM;
                        //amt = totalAmt * Math.Round(dDistance);
                    }

                    string DeviceId = (item.DeviceId == null) ? "0000" : item.DeviceId;
                    string query = @"exec GetNearDriver '" + drivId + "'," + dist + ",'" + drivName + "','" + DlNumber + "'," + Charge + ",'" + DeviceId + "'," + distance + "";

                    
                    driverListNearByUser = ent.Database.SqlQuery<DriverListNearByUser>(query).ToList();

                   
                }

                var mod = new DriverLocation()
                {
                    Lat_Driver = DriveLat,
                    Lang_Driver = DriveLong,
                    //Driver_Id = item.DriverId,
                    start_Lat = model.start_Lat,
                    start_Long = model.start_Long,
                    end_Long = model.end_Long,
                    end_Lat = model.end_Lat,
                    PatientId = model.Patient_Id,
                    VehicleType_id = model.VehicleType_id,
                    NoOfPassengers = model.NoOfPassengers,
                    EntryDate = DateTime.Now,
                    IsPay = "N",
                    Status = "0",
                    IsBooked = false,
                    RideComplete = false,
                    RejectedStatus = false,
                    TotalDistance = Convert.ToInt32(distance),
                    TotalPrice = Charge,
                };
                ent.DriverLocations.Add(mod);
                ent.SaveChanges();
                return Ok(new { model.start_Lat, model.start_Long, model.end_Long, model.end_Lat, model.VehicleType_id, model.Patient_Id, Message = driverListNearByUser });

            }
            catch (Exception)
            {
                return BadRequest("Server Error");
            }
        }

        static string ExtractDecimalValue(string text)
        { 
            string pattern = @"\b\d+\.\d+\b";

            // Use Regex.Match to find the first occurrence of the pattern
            Match match = Regex.Match(text, pattern);

            // Check if a match is found and return the value
            if (match.Success)
            {
                return match.Value;
            }
            else
            { 
                return "No decimal value found";
            }
        }

        [HttpGet, Route("api/DriverApi/AmbulanceBookingRequest")]
        public IHttpActionResult AmbulanceBookingRequest(int DriverId)
        {
            var data = ent.Database.SqlQuery<UserListForBookingAmbulance>(@"select distinct dl.Id,db.Driver_Id,db.Patient_Id as PatientId,P.PatientName, P.MobileNumber, 
dl.end_Lat AS endLat,dl.end_Long As endLong,dl.start_Lat AS startLat,
dl.start_Long AS startLong,AL.DeviceId,dl.TotalPrice,dl.TotalDistance,dl.NoOfPassengers from DriverBooking as db
join DriverLocation as dl on dl.PatientId=db.Patient_Id
join Patient AS P with(nolock) ON db.Patient_Id =P.Id
join AdminLogin as AL on AL.Id=P.AdminLogin_Id 
where dl.[Status] = 0 and dl.RejectedStatus=0 and db.Driver_Id=" + DriverId + " order by dl.Id desc").ToList();

            return Ok(new { UserListForBookingAmbulance = data });
        }

        [HttpPost, Route("api/DriverApi/BookingAmbulanceAccept")]
        public IHttpActionResult BookingAmbulanceAccept(BookingAmbulanceAcceptRejectDTO model)
        {
            var rm = new ReturnMessage();
            var data = ent.DriverLocations.Where(a => a.Id == model.Id).FirstOrDefault();
            data.Status = model.StatusId;
            data.Driver_Id = model.DriverId; 
            ent.SaveChanges();
            rm.Status = 1;
            rm.Message = "Request accepted successfully.";
            return Ok(rm);
        }

        [HttpGet, Route("api/DriverApi/AmbulanceBookingHistory")]
        public IHttpActionResult AmbulanceBookingHistory(int DriverId)
        { 
            string qry = @"select P.Id,P.PatientName,P.MobileNumber,sm.StateName,cm.CityName,P.PinCode,P.Location from DriverLocation as DL left join Patient as P on Dl.PatientId=P.Id left join citymaster as cm with(nolock) on cm.id=P.CityMaster_Id left join statemaster as sm with(nolock) on sm.id=P.StateMaster_Id left join Driver as D on D.Id=DL.Driver_Id where D.Id=" + DriverId + " and DL.IsPay='Y' and DL.RideComplete=1";
            var BookingHistory = ent.Database.SqlQuery<BookingHistory>(qry).ToList();
            return Ok(new { BookingHistory });

        }

        [HttpGet, Route("api/DriverApi/DriverPaymentHistory")]
        public IHttpActionResult DriverPaymentHistory(int DriverId)
        {
            string query = @"select P.Id,P.PatientName,P.MobileNumber,sm.StateName+','+cm.CityName+','+P.Location as Location,DL.Id as PaymentId,DL.TotalPrice,DL.PaymentDate,DL.IsPay,DL.start_Lat,DL.start_Long,DL.end_Lat,DL.end_Long from DriverLocation as DL 
left join Patient as P on Dl.PatientId=P.Id 
left join Driver as D on D.Id=DL.Driver_Id 
join StateMaster sm on sm.Id=P.StateMaster_Id
join CityMaster cm on cm.Id=P.CityMaster_Id
where D.Id=" + DriverId + " and DL.IsPay='Y' order by dl.Id desc";
            var PaymentHistory = ent.Database.SqlQuery<driverpayhistory>(query).ToList();
            return Ok(new { PaymentHistory });

        }

        [HttpPost, Route("api/DriverApi/DriverActiveStatus")]

        public IHttpActionResult DriverActiveStatus(DriverDTO model)
        {
            try
            {
                var data=ent.Drivers.Where(d=>d.Id== model.Id).FirstOrDefault();
                if(data!=null)
                {
                    if (model.IsActiveStatus==true)
                    { 
                        rm.Status = 1;
                        rm.Message = "You are online now.";
                    }
                    else
                    {
                        rm.Message = "You are offline now.";
                    }
                    data.IsActive = model.IsActiveStatus; 
                    ent.SaveChanges(); 
                    return Ok(rm);
                }
                else
                {
                    return BadRequest ("Data not found.");
                }
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Internal server error."));
            }
        }

        [HttpGet, Route("api/DriverApi/OfferRequestList")]
        public IHttpActionResult OfferRequestList(int DriverId)
        {
            var data = ent.Database.SqlQuery<UserListForBookingAmbulance>(@"select distinct op.Id,D.PatientId,P.PatientName, P.MobileNumber, 
D.end_Lat AS endLat,D.end_Long As endLong,D.start_Lat AS startLat,
D.start_Long AS startLong,AL.DeviceId,D.TotalPrice,D.TotalDistance,D.NoOfPassengers,op.OfferPrice
from DriverLocation AS D with(nolock)
INNER JOIN Patient AS P with(nolock) ON D.PatientId =P.Id
inner join AdminLogin as AL on AL.Id=P.AdminLogin_Id 
join UserOfferPrice op on op.Patient_Id=D.PatientId
WHERE D.[Status] = 0 and D.RejectedStatus=0 and op.Driver_Id=" + DriverId + "").ToList();
            var details = data.Select(x=>new {x.OfferPrice,x.DeviceId,x.startLat,x.startLong,x.endLat,x.endLong,x.ReverseStartLatLong_To_Location }).FirstOrDefault();
            return Ok(new { UserListForBookingAmbulance = data, Message = details });             
        }

        [HttpPost, Route("api/DriverApi/AmbulanceReject")]
        public IHttpActionResult AmbulanceReject(BookingAmbulanceAcceptRejectDTO bookingAmbulanceAcceptRejectDTO)
        {
            var data = ent.DriverLocations.Where(a => a.Id == bookingAmbulanceAcceptRejectDTO.Id).FirstOrDefault();
            data.RejectedStatus = true;
            data.Status = "0";
            data.IsBooked = false;
            data.Driver_Id = bookingAmbulanceAcceptRejectDTO.DriverId;
            ent.SaveChanges();
            return Ok("Request rejected successfully.");
        }

        [HttpGet, Route("api/DriverApi/GetOnGoingRide_UserDetail")]
        public IHttpActionResult GetOnGoingRide_UserDetail(int DriverId)
        {
            var UserDetail = ent.Database.SqlQuery<UserdetailOngoingdriver>($"select DL.Id,P.Id as PatientId,P.PatientName,P.MobileNumber,sm.StateName+','+cm.CityName+','+P.Location as Location,DL.TotalPrice,DL.PaymentDate,DL.IsPay,DL.Lat_Driver,DL.Lang_Driver,DL.start_Lat,DL.start_Long,DL.end_Lat,DL.end_Long,DL.TotalDistance,al.DeviceId from DriverLocation as DL left join Patient as P on Dl.PatientId=P.Id left join Driver as D on D.Id=DL.Driver_Id join StateMaster sm on sm.Id=P.StateMaster_Id join CityMaster cm on cm.Id=P.CityMaster_Id JOIN AdminLogin al on al.Id=D.AdminLogin_Id where D.Id={DriverId} and DL.IsPay='Y' and DL.RideComplete='0' order by DL.Id desc").FirstOrDefault();
            if (UserDetail != null)
            {
                // Driver
                var lat1 = (double)UserDetail.Lat_Driver;
                var lon1 = (double)UserDetail.Lang_Driver;

                // User
                var lat2 = (double)UserDetail.start_Lat;
                var lon2 = (double)UserDetail.start_Long;

                double rlat1 = Math.PI * lat1 / 180;
                double rlat2 = Math.PI * lat2 / 180;
                double theta = lon1 - lon2;
                double rtheta = Math.PI * theta / 180;

                double dist = Math.Sin(rlat1) * Math.Sin(rlat2) +
                              Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);

                dist = Math.Acos(dist);
                dist = dist * 180 / Math.PI;
                dist = dist * 60 * 1.1515;
                dist = dist * 1.609344;   // Convert miles to kilometers

                UserDetail.DriverUserDistance = (int)dist;

                // Calculate expected time

                double expectedTimeMinutes = dist * 4; // 2 minutes per kilometer

                // Convert the expectedTimeMinutes to an integer
                int expectedTimeMinutesInt = Convert.ToInt32(expectedTimeMinutes);
                if (expectedTimeMinutesInt == 0)
                {
                    expectedTimeMinutesInt = 5;
                }
                UserDetail.ExpectedTime = expectedTimeMinutesInt;

            }

            return Ok(UserDetail);
        }

        [HttpPost, Route("api/DriverApi/CompleteRide")]

        public IHttpActionResult CompleteRide(DriverLocationDT model)
        {
            var data = ent.DriverLocations.Where(a => a.Id == model.Id).FirstOrDefault();
            if(data != null)
            {
                data.RideComplete = true;
            }
            else
            {
                return BadRequest("Data not found.");
            }
            ent.SaveChanges();
            return Ok("Your ride complete.");
        }
        [HttpGet, Route("api/DriverApi/DriverPayoutHistory")]
        public IHttpActionResult DriverPayoutHistory(int DriverId)
        {
            string query = @"select dp.Id,d.DriverName,dp.PaymentDate as PayoutDate,dp.Amount from DriverPayOut as dp
join Driver as d on d.Id=dp.Driver_Id where dp.Driver_Id="+ DriverId + " order by dp.Id desc";
            var PayoutHistory = ent.Database.SqlQuery<PayoutHistory>(query).ToList();
            return Ok(new { PayoutHistory });

        }
    }
}
