using AMBRD.BL;
using AMBRD.Models;
using AMBRD.Models.ApiModels;
using AMBRD.Models.ViewModels;
using AMBRD.Repositories;
using AMBRD.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Mvc; 

namespace AMBRD.API
{
   
    public class SignupApiController : ApiController
    {
        abdul_amurdEntities11 ent= new abdul_amurdEntities11();
        CommonRepository repos = new CommonRepository();
        GenerateBookingId bk = new GenerateBookingId();

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/SignupApi/PatientRegistration")]
        public IHttpActionResult PatientRegistration(PatientDTO model)
        {
            try
            {
                var rm = new ReturnMessage();
                if (ent.Patients.Any(a => a.PatientName == model.PatientName && a.MobileNumber == model.MobileNumber))
                {
                    var data = ent.Patients.Where(a => a.PatientName == model.PatientName && a.MobileNumber == model.MobileNumber).FirstOrDefault();
                    
                    return BadRequest( "you are already registered with AMBRD");
                    
                }
                if (ent.AdminLogins.Any(a => a.PhoneNumber == model.MobileNumber))
                {
                    rm.Status = 1;
                    rm.Message = "This Mobile Number has already exists.";
                    return Ok(rm);
                }
                if (ent.AdminLogins.Any(a => a.Username == model.EmailId))
                {
                    rm.Message = "This EmailId has already exists.";
                    rm.Status = 1;
                    return Ok(rm);
                }
                var admin = new AdminLogin
                {
                    Username = model.EmailId,
                    PhoneNumber = model.MobileNumber, 
                    Role = "patient"
                };
                ent.AdminLogins.Add(admin);
                ent.SaveChanges();
                 
                var domainModel = new Patient();
                domainModel.AdminLogin_Id = admin.Id;
                domainModel.PatientName = model.PatientName;
                domainModel.StateMaster_Id = model.StateMaster_Id;
                domainModel.CityMaster_Id = model.CityMaster_Id;
                domainModel.MobileNumber = model.MobileNumber; 
                domainModel.EmailId = model.EmailId;
                domainModel.Location = model.Location;
                domainModel.PinCode = model.PinCode; 
                domainModel.Gender = model.Gender; 
                domainModel.DOB = model.DOB; 
                domainModel.IsDeleted = false;
                domainModel.IsApproved = false;
                domainModel.Reg_Date = DateTime.Now; 
                ent.Patients.Add(domainModel);
                ent.SaveChanges();
                string msg = "Welcome to AMBRD. Your User Name :  " + domainModel.EmailId + "(" + domainModel.PatientRegNo + "), Password : " + admin.Password + ".";

                string msg1 = "Welcome to AMBRD. Your User Name :  " + admin.Username + "(" + admin.UserID + "), Password : " + admin.Password + ".";
                rm.Status = 1;
                rm.Message = "Thanks for joining us.You have registered successfully.";
                return Ok(rm);

            }
            catch (Exception ex)
            {
                return InternalServerError (ex);
            }             
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/SignupApi/DriverReg")]
        public IHttpActionResult DriverReg(DriverDTO model)
        {
            try
            {
                var rm = new ReturnMessage();
                string[] allowedExtensions = { ".jpg", ".png", ".jpeg" };

                if (ent.Drivers.Any(a => a.DriverName == model.DriverName && a.MobileNumber == model.MobileNumber))
                {
                    var data = ent.Drivers.Where(a => a.DriverName == model.DriverName && a.MobileNumber == model.MobileNumber).FirstOrDefault(); 

                    return BadRequest("you are already registered with AMBRD");

                }
                if (ent.AdminLogins.Any(a => a.PhoneNumber == model.MobileNumber))
                {
                    rm.Status = 0;
                    return BadRequest ( "This Mobile Number has already exists.");
                    
                }
                if (ent.AdminLogins.Any(a => a.Username == model.EmailId))
                {
                    return BadRequest("This EmailId has already exists.");
                    
                }
                var admin = new AdminLogin
                {
                    Username = model.EmailId,
                    PhoneNumber = model.MobileNumber, 
                    Role = "driver"
                };
                ent.AdminLogins.Add(admin);
                ent.SaveChanges();


                //upload PanImage
                if (model.PanImageBase64 != null)
                {
                    var PanImg = FileOperation.UploadFileWithBase64("Images", model.PanImage, model.PanImageBase64, allowedExtensions);
                    if (PanImg == "not allowed")
                    {
                        return BadRequest("Only png,jpg,jpeg files are allowed as Pan Picture.");

                    }
                    model.PanImage = PanImg;
                }

                //upload DlImage front image 
                if (model.DlImageBase64 != null)
                {
                    var DLImg = FileOperation.UploadFileWithBase64("Images", model.DlImage, model.DlImageBase64, allowedExtensions);
                    if (DLImg == "not allowed")
                    {
                        return BadRequest("Only png,jpg,jpeg files are allowed as DL Picture.");

                    }
                    model.DlImage = DLImg;
                }

                //upload DlImage1 back image
                if (model.DlImage1Base64 != null)
                {
                    var DLImg1 = FileOperation.UploadFileWithBase64("Images", model.DlImage1, model.DlImage1Base64, allowedExtensions);
                    if (DLImg1 == "not allowed")
                    {
                        return BadRequest("Only png,jpg,jpeg files are allowed as DL Picture.");

                    }
                    model.DlImage1 = DLImg1;
                }

                //upload Adharimage front image 
                if (model.AadharImageBase64 != null)
                {
                    var AdharImg1 = FileOperation.UploadFileWithBase64("Images", model.AadharImage, model.AadharImageBase64, allowedExtensions);
                    if (AdharImg1 == "not allowed")
                    {
                        return BadRequest("Only png,jpg,jpeg files are allowed as Aadhar Picture.");

                    }
                    model.AadharImage = AdharImg1;
                }

                //upload Adharimage back image 
                if (model.AadharImage2Base64 != null)
                {
                    var AdharImg2 = FileOperation.UploadFileWithBase64("Images", model.AadharImage2, model.AadharImage2Base64, allowedExtensions);
                    if (AdharImg2 == "not allowed")
                    {
                        return BadRequest("Only png,jpg,jpeg files are allowed as Aadhar Picture.");

                    }
                    model.AadharImage2 = AdharImg2;
                }

                //upload driverimage  
                if (model.DriverImageBase64 != null)
                {
                    var DriverImage = FileOperation.UploadFileWithBase64("Images", model.DriverImage, model.DriverImageBase64, allowedExtensions);
                    if (DriverImage == "not allowed")
                    {
                        return BadRequest("Only png,jpg,jpeg files are allowed as Profile Picture.");

                    }
                    model.DriverImage = DriverImage;
                }


                var domainModel = new Driver();
                domainModel.AdminLogin_Id = admin.Id;
                domainModel.DriverName = model.DriverName;
                domainModel.DriverImage = model.DriverImage;
                domainModel.StateMaster_Id = model.StateMaster_Id;
                domainModel.CityMaster_Id = model.CityMaster_Id; 
                domainModel.MobileNumber = model.MobileNumber;
                domainModel.EmailId = model.EmailId;
                domainModel.Location = model.Location;
                domainModel.PinCode = model.PinCode;  
                domainModel.PanImage = model.PanImage;
                domainModel.PanNumbar = model.PanNumbar;
                domainModel.AadharImage = model.AadharImage;
                domainModel.AadharImage2 = model.AadharImage2;
                domainModel.AadharNumber = model.AadharNumber;
                domainModel.DlNumber = model.DlNumber;
                domainModel.DlImage = model.DlImage; 
                domainModel.DlImage1 = model.DlImage1; 
                domainModel.DlValidity = model.DlValidity; 
                domainModel.Gender = model.Gender; 
                domainModel.IsDeleted = false;
                domainModel.IsApproved = false;
                domainModel.JoiningDate = DateTime.Now;
                domainModel.DOB = model.DOB;
                domainModel.charge = model.charge; 
                ent.Drivers.Add(domainModel);
                ent.SaveChanges();
               
                rm.Status = 1;
                rm.Message = "Thanks for joining us.You have registered successfully but you can not login untill approved by ADMIN.";
                return Ok(rm);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        
    }
}
