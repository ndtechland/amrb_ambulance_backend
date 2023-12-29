using AMBRD.Models;
using AMBRD.Models.ApiModels;
using AMBRD.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Web.Http;
using ReturnMessage = AMBRD.Models.ApiModels.ReturnMessage;

namespace AMBRD.API
{
    public class PatientApiController : ApiController
    {
        abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        ReturnMessage rm = new ReturnMessage();

        [HttpGet]
        [Route("api/PatientApi/GetProfileDetail")]
        public IHttpActionResult GetProfileDetail(int Id)
        {
            string qry = @"select p.Id,p.PatientName,p.EmailId,p.MobileNumber,sm.StateName,cm.CityName,p.Location,p.PinCode,p.Gender,p.DOB from Patient as p
join StateMaster as sm on sm.Id=p.StateMaster_Id
join CityMaster as cm on cm.Id=p.CityMaster_Id where p.Id=" + Id + "";
            var Profile = ent.Database.SqlQuery<ProfileDetail>(qry).FirstOrDefault();
            return Ok(Profile);
        }

        [HttpPost]
        [Route("api/PatientApi/EditProfile")]

        public IHttpActionResult EditProfile(EditProfile model)
        {
            try
            {
                var rm = new ReturnMessage();
                var data = ent.Patients.Find(model.Id);
                if (data == null)
                {
                    rm.Status = 0;
                    rm.Message = "No record found";
                    return Ok(rm);  
                }
                data.PatientName = model.PatientName; 
                data.CityMaster_Id = model.CityMaster_Id;
                data.Location = model.Location;
                data.StateMaster_Id = model.StateMaster_Id;
                data.PinCode = model.PinCode;
                ent.SaveChanges(); 
                return Ok("Profile has updated.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);                
            }
          
        }

        [HttpPost]
        [Route("api/PatientApi/PatientComplaints")]
        public IHttpActionResult PatientComplaints(ComplaintsDTO model)
        {
            try
            {
                var rm = new ReturnMessage();
                var data = new PatientComplaint()
                {
                    patsubid = model.patsubid,
                    Others = model.Others,
                    Complaints = model.Complaints,
                    Subjects = model.Subjects,
                    IsDeleted = false,
                    IsResolved = false,
                    Login_Id = model.Login_Id,
                    ComplaintDate = DateTime.Now
                };
                ent.PatientComplaints.Add(data);
                ent.SaveChanges();
                rm.Status = 1;
                rm.Message = "Complaints add Successfully";
                return Ok(rm);

            }
            catch (Exception)
            { 
                return BadRequest("Internal server error");
            } 
        }
         
        [HttpGet, Route("api/PatientApi/GetAcceptedReqDriverDetail")]
        public IHttpActionResult GetAcceptedReqDriverDetail(int Id)
        {
            string qry = @"SELECT DL.Id,D.Id AS DriverId,D.DriverName,D.MobileNumber,D.DlNumber,D.DriverImage,ND.TotalPrice,V.VehicleNumber,VT.VehicleTypeName,ND.TotalDistance FROM Driver as D INNER JOIN NearDriver AS ND ON ND.DriverId=D.Id INNER JOIN Ambulance AS V ON V.Driver_Id=D.Id INNER JOIN DriverLocation AS DL ON D.Id=DL.Driver_Id INNER JOIN VehicleType AS VT ON VT.Id=V.VehicleType_Id WHERE ND.Id=" + Id + " and DL.[Status]=1 and DL.IsPay='N' order by DL.Id desc";
            var data = ent.Database.SqlQuery<GetAcceptedReq_DriverDetail>(qry).FirstOrDefault();
            return Ok(data);
        }

        [HttpPost, Route("api/PatientApi/DriverPayNow")]
        public IHttpActionResult DriverPayNow(BookingAmbulanceAcceptRejectDTO model)
        {
            try
            {
                var data = ent.DriverLocations.Where(a => a.PatientId == model.PatientId && a.Driver_Id == model.DriverId && a.IsPay == "N" && a.Id==model.Id).OrderByDescending(a => a.Id).FirstOrDefault();
                if(data!=null)
                {
                    data.Amount = model.Amount;
                    data.IsPay = "Y";
                    data.PaymentDate = DateTime.Now;
                    ent.SaveChanges();
                    rm.Status = 1;
                    rm.Message = "Payment successfully processed";
                    return Ok(rm);
                }
                else
                {
                    return BadRequest("No record found");
                }
               
            }
            catch (Exception)
            {
                rm.Status = 0;
                return InternalServerError(new Exception("Server Error"));
            }
            

        }

        [HttpGet, Route("api/PatientApi/DriverBookingHistory")]
        public IHttpActionResult DriverBookingHistory(int PatientId)
        {
            //var driverDetails = (from dl in ent.DriverLocations
            //                     join d in ent.Drivers on dl.Driver_Id equals d.Id
            //                     join v in ent.Ambulances on dl.VehicleType_id equals v.VehicleType_Id
            //                     join vt in ent.VehicleTypes on dl.VehicleType_id equals vt.Id
            //                     join p in ent.Patients on dl.PatientId equals p.Id
            //                     where dl.IsPay == "Y" && p.Id == PatientId
            //                     orderby dl.Id descending
            //                     select new getdriverbookinglist
            //                     {
            //                         Id = dl.Id,
            //                         DriverId = d.Id,
            //                         DriverName = d.DriverName,
            //                         MobileNumber = d.MobileNumber,
            //                         DlNumber = d.DlNumber,
            //                         DriverImage = d.DriverImage,
            //                         VehicleNumber = v.VehicleNumber,
            //                         VehicleTypeName = vt.VehicleTypeName,
            //                         TotalDistance = dl.TotalDistance,
            //                         PaidAmount = dl.Amount,
            //                         TotalPrice = dl.TotalPrice,
            //                         RemainingAmount = dl.TotalPrice - dl.Amount,
            //                         UserLat = dl.start_Lat,
            //                         UserLong = dl.start_Long,
            //                         Lat_Driver = dl.Lat_Driver,
            //                         Lang_Driver = dl.Lang_Driver, 
            //                         PaymentDate = dl.PaymentDate,
            //                     }).ToList(); 

            var driverDetails = ent.Database.SqlQuery<getdriverbookinglist>($"WITH RankedDriverLocation AS (SELECT dl.Id,d.Id AS DriverId,d.DriverName,d.MobileNumber,d.DlNumber,d.DriverImage,v.VehicleNumber,vt.VehicleTypeName, dl.TotalDistance, dl.Amount AS PaidAmount,dl.TotalPrice,dl.start_Lat,dl.start_Long,dl.end_Lat,dl.end_Long,dl.PaymentDate,ROW_NUMBER() OVER (PARTITION BY dl.Id ORDER BY dl.Id DESC) AS RowNum FROM DriverLocation dl JOIN Driver d ON dl.Driver_Id = d.Id JOIN Ambulance v ON dl.VehicleType_id = v.VehicleType_Id JOIN VehicleType vt ON dl.VehicleType_id = vt.Id JOIN Patient p ON dl.PatientId = p.Id WHERE dl.IsPay = 'Y' AND p.Id = {PatientId})SELECT Id, DriverId, DriverName, MobileNumber, DlNumber, DriverImage, VehicleNumber,VehicleTypeName,TotalDistance,PaidAmount,TotalPrice,start_Lat,start_Long,end_Lat,end_Long,PaymentDate FROM RankedDriverLocation WHERE  RowNum = 1 ORDER BY Id DESC;").ToList();
             
            return Ok(new { driverDetails });
        }

        [HttpGet, Route("api/PatientApi/GetOnGoingRide_DriverDetail")]
        public IHttpActionResult GetOnGoingRide_DriverDetail(int PatientId)
        {
            var driverDetails = (from dl in ent.DriverLocations
                                 join d in ent.Drivers on dl.Driver_Id equals d.Id
                                 join v in ent.Ambulances on dl.VehicleType_id equals v.VehicleType_Id
                                 join vt in ent.VehicleTypes on dl.VehicleType_id equals vt.Id
                                 join p in ent.Patients on dl.PatientId equals p.Id
                                 where dl.IsPay == "Y" && p.Id == PatientId
                                 orderby dl.Id descending
                                 select new getdriverbookinglist
                                 {
                                     Id = dl.Id,
                                     DriverId = d.Id,
                                     DriverName = d.DriverName,
                                     MobileNumber = d.MobileNumber,
                                     DlNumber = d.DlNumber,
                                     DriverImage = d.DriverImage,
                                     VehicleNumber = v.VehicleNumber,
                                     VehicleTypeName = vt.VehicleTypeName,
                                     TotalDistance = dl.TotalDistance,
                                     PaidAmount = dl.Amount,
                                     TotalPrice = dl.TotalPrice,
                                     RemainingAmount = dl.TotalPrice - dl.Amount,
                                     UserLat = dl.start_Lat,
                                     UserLong = dl.start_Long,
                                     Lat_Driver = dl.Lat_Driver,
                                     Lang_Driver = dl.Lang_Driver, 
                                     PaymentDate = dl.PaymentDate,
                                 }).ToList();

            foreach (var detail in driverDetails)
            {
                // Driver
                var lat1 = (double)detail.Lat_Driver;
                var lon1 = (double)detail.Lang_Driver;

                // User
                var lat2 = (double)detail.UserLat;
                var lon2 = (double)detail.UserLong;

                double rlat1 = Math.PI * lat1 / 180;
                double rlat2 = Math.PI * lat2 / 180;
                double theta = lon1 - lon2;
                double rtheta = Math.PI * theta / 180;

                double dist = Math.Sin(rlat1) * Math.Sin(rlat2) +
                              Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);

                dist = Math.Acos(dist);
                dist = dist * 180 / Math.PI;
                dist = dist * 60 * 1.1515; // Convert miles to kilometers
                dist = dist * 1.609344;   // Convert miles to kilometers

                detail.DriverUserDistance = dist;

                // Calculate expected time
                 
                double expectedTimeMinutes = dist * 4; // 2 minutes per kilometer

                // Convert the expectedTimeMinutes to an integer
                int expectedTimeMinutesInt = Convert.ToInt32(expectedTimeMinutes);

                detail.ExpectedTime = expectedTimeMinutesInt;
            }

            return Ok(new { driverDetails });
        } 

        [HttpPost, Route("api/PatientApi/RequestOffer")]
        public IHttpActionResult RequestOffer(DriverLocationDT model)
        {
            try
            {
                ent.Database.ExecuteSqlCommand("delete from UserOfferPrice where Patient_Id="+model.Patient_Id + ""); 
                List<DriverListNearByUser> driverListNearByUser = new List<DriverListNearByUser>();

                var Driver = ent.Database.SqlQuery<DriverLocationDT>(@"select distinct D.Id AS DriverId,D.Lat, D.Long,D.DriverName,D.charge,D.DlNumber,AL.DeviceId from Driver AS D with(nolock)
INNER JOIN AdminLogin AS AL with(nolock) ON D.AdminLogin_Id = AL.Id
INNER JOIN Ambulance as v on V.VehicleType_Id=d.VehicleType_id
INNER JOIN VehicleType as vt on Vt.Id=v.VehicleType_id
where D.Lat IS NOT NULL and D.Long IS NOT NULL and d.VehicleType_id=" + model.VehicleType_id + " and D.IsApproved=1").ToList();
                foreach (var item in Driver)
                {
                    //==================USER AND DRIVER DISTANCE========================
                    // Driver
 
                    var mod = new UserOfferPrice()
                    {
                        Driver_Id = item.DriverId,
                        Patient_Id = model.Patient_Id,
                        IsAccepted = false,
                        EntryDate = DateTime.Now,
                        OfferPrice = model.OfferPrice,
                    };
                    ent.UserOfferPrices.Add(mod);
                }

                ent.SaveChanges();
                return Ok(new { Message = "Request send successfully." });

            }
            catch (Exception)
            {
                return BadRequest("Server Error");
            }
        }
        
        //Post Api send request single driver
        [HttpPost, Route("api/PatientApi/BookDriver")]
         
        public IHttpActionResult BookDriver(DriverLocationDT model)
        {
            try
            {
                // Check if the driver exists and is available (add your own logic)
                //var selectedDriver = ent.DriverLocations.FirstOrDefault(d => d.Driver_Id == model.Driver_Id && d.IsBooked == false && d.PatientId== model.Patient_Id);

                //if (selectedDriver != null)
                //{
                //    // Update the driver's status to indicate that they are booked
                //    selectedDriver.IsBooked = true; // You may need to use an enum for status
                //    selectedDriver.RejectedStatus = false;  

                    // Create a new booking record in your database
                    var booking = new DriverBooking
                    {
                        Driver_Id = model.Driver_Id,
                        Patient_Id = model.Patient_Id,
                        EntryDate = DateTime.Now, 
                    };

                    ent.DriverBookings.Add(booking);
                    ent.SaveChanges();

                    return Ok(new { Message = "Driver booked successfully"});
                //}
                //else
                //{ 
                //    return BadRequest("Selected driver is not available for booking");
                //}
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
                return BadRequest("Server Error");
            }
        }

        //Post Api send request all driver
        [HttpPost, Route("api/PatientApi/RequestToAll")]
        public IHttpActionResult RequestToAll(DriverLocationDT model)
        {
            try
            { 
                List<DriverListNearByUser> driverListNearByUser = new List<DriverListNearByUser>();

                var Driver = ent.Database.SqlQuery<DriverLocationDT>(@"select * from NearDriver with(nolock) where KM <= 5 order by KM asc ").ToList();
                foreach (var item in Driver)
                {
                    //var selectedDriver = ent.DriverLocations.FirstOrDefault(d => d.Driver_Id == item.DriverId && d.IsBooked == false);

                    //if (selectedDriver != null)
                    //{
                         
                    //    selectedDriver.IsBooked = true;
                    //    selectedDriver.RejectedStatus = false;

                        var booking = new DriverBooking
                         {
                             Driver_Id = item.DriverId,
                             Patient_Id = model.Patient_Id,
                             EntryDate = DateTime.Now, 
                         };

                        ent.DriverBookings.Add(booking);
                        ent.SaveChanges(); 
                    //}
                    //else
                    //{ 
                    //    return BadRequest("Selected driver is not available for booking");
                    //}
                     
                } 
                rm.Status = 1;
                rm.Message = "Request send successfully.";
                return Ok(rm);

            }
            catch (Exception)
            {
                return BadRequest("Server Error");
            }
        }
    }
}
