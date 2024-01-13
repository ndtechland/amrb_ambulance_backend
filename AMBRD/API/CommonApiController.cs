using AMBRD.Models;
using AMBRD.Models.ApiModels;
using AMBRD.Models.ViewModels;
using AMBRD.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Services.Description;

namespace AMBRD.API
{
    public class CommonApiController : ApiController
    {
        abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        ReturnMessage rm = new ReturnMessage();

        [HttpGet]
        [Route ("api/CommonApi/GetBanner")]
        public IHttpActionResult GetBanner()
        {
            string qry = @"select * from Banner";
            var Banner = ent.Database.SqlQuery<Banner>(qry).ToList();
            return Ok(new { Banner });
        }

        [HttpGet]
        [Route("api/CommonApi/GetGallery")]
        public IHttpActionResult GetGallery()
        {
            string qry = @"select * from Gallery";
            var Gallery = ent.Database.SqlQuery<Gallery>(qry).ToList();
            return Ok(new { Gallery });
        }

        [HttpGet]
        [Route("api/CommonApi/GetAllService")]
        public IHttpActionResult GetAllService()
        {          
            string qry = @"select * from OurService";
            var OurService = ent.Database.SqlQuery<OurService>(qry).ToList();
            return Ok(new { OurService });
        }

        [HttpGet]
        [Route("api/CommonApi/GetIndividualService")]
        //get service by Id
        public IHttpActionResult GetIndividualService(int Id)
        {
            string qry = @"select * from OurService where Id = " + Id;
            var OurService = ent.Database.SqlQuery<OurService>(qry).FirstOrDefault();

            if (OurService != null)
            { 
                var result = new
                {
                    Id = OurService.Id,
                    ServiceName = OurService.ServiceName,
                    Image = OurService.Image,
                    Description = OurService.Description
                };

                return Ok(result);
            }
            else
            { 
                return NotFound();
            }
        }

        [HttpGet]
        [Route("api/CommonApi/GetAllOtherService")]
        public IHttpActionResult GetAllOtherService()
        {
            string qry = @"select * from OtherService";
            var OtherServices = ent.Database.SqlQuery<OtherService>(qry).ToList();
            return Ok(new { OtherServices });
        }

        [HttpGet]
        [Route("api/CommonApi/GetIndividualOtherService")]
        public IHttpActionResult GetIndividualOtherService(int Id)
        {
            string qry = @"select * from OtherService where Id = " + Id;
            var OtherServices = ent.Database.SqlQuery<OtherService>(qry).FirstOrDefault();

            if (OtherServices != null)
            {
                var result = new
                {
                    Id = OtherServices.Id,
                    ServiceName = OtherServices.ServiceName,
                    Image = OtherServices.Image,
                    Description = OtherServices.Description
                };

                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("api/CommonApi/GetTestimonial")]
        public IHttpActionResult GetTestimonial()
        {
            string qry = @"select * from Testimonial";
            var Testimonial = ent.Database.SqlQuery<Testimonial>(qry).ToList();
            return Ok(new { Testimonial });
        }

        [HttpPost]
        [Route("api/CommonApi/Contactus")]

        public IHttpActionResult Contactus(ContactU model)
        {
            try
            {
                var data = new ContactU()
                {
                    FirstName=model.FirstName,
                    LastName=model.LastName,
                    Email=model.Email,
                    Phone=model.Phone,
                    Message=model.Message,
                    Address=model.Address,
                    SubmissionDate=DateTime.Now,

                };
                ent.ContactUs.Add(data);
                ent.SaveChanges();
                return Ok("Detail add successfully.");
            } 
            catch (Exception ex) 
            {
                
                return BadRequest(ex.Message);
            
            }
        }

        [HttpGet]
        [Route("api/CommonApi/GetAboutUs")]
        public IHttpActionResult GetAboutUs()
        {
            var aboutContent = ent.AboutContents.OrderByDescending(a=>a.ID).FirstOrDefault();

            if (aboutContent != null)
            {
                var services = ent.AboutContentServices
                    .Where(acs => acs.AboutContentID == aboutContent.ID)
                    .Select(acs => acs.Service)
                    .ToList();

                return Ok(new
                {
                    ID = aboutContent.ID,
                    ImageFile = aboutContent.ImageFile,
                    Heading = aboutContent.Heading,
                    Paragraph = aboutContent.Paragraph,
                    Services = services,
                    Status = 200
                });
            }
            else
            {
                return BadRequest("AboutContents data not found");
            }
        }

        [HttpGet]
        [Route("api/CommonApi/GetBlogs")]

        public IHttpActionResult GetBlogs()
        {
            string qry = @"select * from Blog";
            var Blogs = ent.Database.SqlQuery<Blog>(qry).ToList();

            if(Blogs.Count > 0)
            {
                return Ok(new { Blogs });
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpGet]
        [Route("api/CommonApi/GetBlogsById")]
        public IHttpActionResult GetBlogsById(int Id)
        {
            string qry = @"select * from Blog where Id="+ Id + "";
            var Blogs = ent.Database.SqlQuery<Blog>(qry).FirstOrDefault();

            return Ok( Blogs );  
             

        }
        [HttpGet]
        [Route("api/CommonApi/GetAllStates")]
        [Obsolete]
        public IHttpActionResult GetAllStates()
        {
            Mapper.CreateMap<StateMaster, StateDTO>();
            var State =Mapper.Map <IEnumerable<StateDTO>> (ent.StateMasters.Where(a => a.IsDeleted == false).OrderBy(a => new { a.StateName })).ToList();
            return Ok(new { State });
        }
        [HttpGet]
        [Route("api/CommonApi/GetCitiesByState")]
        [Obsolete]
        public IHttpActionResult GetCitiesByState(int stateId)
        {
            Mapper.CreateMap<CityMaster, CityDTO>();
            var District = Mapper.Map<IEnumerable<CityDTO>>(ent.CityMasters.Where(a => a.State_Id == stateId && a.IsDeleted == false).OrderBy(a => new { a.CityName })).ToList();
            return Ok(new { District });
        }        
  
        [HttpPost]
        [Route("api/CommonApi/AddBankDetail")]

        public IHttpActionResult AddBankDetail(BankDetailDTO model)
        {
            try
            {
                var rm = new ReturnMessage();
                if (ent.BankDetails.Any(b => b.Login_Id == model.Login_Id))
                {
                    return BadRequest("Already exists.");
                } 
                
                var DomainModel = new BankDetail()
                {
                    Login_Id = model.Login_Id,
                    AccountNumber = model.AccountNumber,
                    IFSCCode = model.IFSCCode,
                    BranchName = model.BranchName,
                    BranchAddress = model.BranchAddress,
                    HolderName = model.HolderName,
                    MobileNumber = model.MobileNumber,
                    isverified = true,
                };

                ent.BankDetails.Add(DomainModel);
                ent.SaveChanges();
                rm.Status = 1;
                rm.Message = "Bank details added successfully.";
                return Ok(rm);
            }
            catch (Exception ex)
            {
                return BadRequest("Server error");
            }
        }

        [HttpGet]
        [Route("api/CommonApi/GetVehicleCategory")]
        public IHttpActionResult GetVehicleCategory()
        {
            Mapper.CreateMap<VehicleCategory, VehicleCategoryDTO>();
            var Cat = Mapper.Map<IEnumerable<VehicleCategoryDTO>>(ent.VehicleCategories.Where(a => a.IsDeleted == false).OrderBy(a => new { a.CategoryName })).ToList();
            return Ok(new { Cat });
        }
        [HttpGet]
        [Route("api/CommonApi/GetVehicleTypeByCat")]
        [Obsolete]
        public IHttpActionResult GetVehicleTypeByCat(int CatId)
        {
            Mapper.CreateMap<VehicleType, VehicleTypeDTO>();
            var Type = Mapper.Map<IEnumerable<VehicleTypeDTO>>(ent.VehicleTypes.Where(a => a.VehicleCatId == CatId && a.IsDeleted == false).OrderBy(a => new { a.VehicleTypeName })).ToList();
            return Ok(new { Type });
        }
        [HttpGet, Route("api/CommonApi/GetVehicleType")]
        public IHttpActionResult GetVehicleType()
        {
            dynamic obj = new ExpandoObject();
            Mapper.CreateMap<VehicleType, VehicleTypeNames>();
            obj.Vehicles = Mapper.Map<IEnumerable<VehicleTypeNames>>(ent.VehicleTypes.Where(a => a.IsDeleted == false).OrderBy(a => a.VehicleTypeName).ToList());
            return Ok(obj);
        }

        [HttpPost, Route("api/CommonApi/LoginWithMobile")]
        public IHttpActionResult LoginWithMobile(LoginModel model)
        {
            try
            {
                var result = ent.AdminLogins.FirstOrDefault(x => x.PhoneNumber == model.MobileNumber);
                if (result != null)
                {
                    //int otpValue = new Random().Next(1000, 9999);
                    int otpValue = 2222;
                    result.OTP = otpValue;
                    ent.SaveChanges();

                    string mobile = "918601703418";
                    string msg = "Dear User,";
                    msg += "Your OTP for login to AMBRD is " + otpValue + ". Valid for 10 minutes. Please do not share this OTP.";
                    msg += "Regards,";
                    msg += "AMBRD Team";
                    string dltid = "153995AnO2vKHdOPk59294dd7";
                    Utilities.Message.SendSms(model.MobileNumber, msg, dltid);
                 
                    //if (isvalid)
                    //{
                        return Ok(new { status = 200, message = "Otp Send SuccessFully" });
                    //}
                    //else
                    //{
                    //    return BadRequest("Otp Not Send");
                    //}

                } 
                return BadRequest("Invalid mobile number."); 
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }

        [HttpPost, Route("api/CommonApi/MobileOtpVerify")]
        public IHttpActionResult MobileOtpVerify(LoginModel model)
        {
            try
            {
                dynamic obj = new ExpandoObject();

                // Check if the mobile number exists in AdminLogins and OTP matches 
                var login = ent.AdminLogins.FirstOrDefault(x => x.OTP == model.OTP && x.PhoneNumber == model.MobileNumber);

                if (login != null)
                {
                    if (login != null)
                    {
                        login.OTP = 0;
                    }
                    ent.SaveChanges();
                    string role = login.Role;
                    switch (role)
                    {
                        case "driver":
                            var data = ent.Drivers.FirstOrDefault(a => a.AdminLogin_Id == login.Id );
                            if ((bool)!data.IsApproved)
                            {  
                                return BadRequest("Your account is not approved by admin yet");
                            }
                            if ((bool)data.IsDeleted)
                            { 
                                return BadRequest("Your account is not available");
                            }
                            var driver = ent.Drivers.FirstOrDefault(a => a.AdminLogin_Id == login.Id && a.IsDeleted == false);

                            // Check if the driver is allotted a vehicle
                            var vehicle = ent.Ambulances.Any(a => a.Driver_Id == driver.Id);
                            if (!vehicle)
                            { 
                                return BadRequest("You haven't been allotted a vehicle yet. Kindly contact the Administrator.");
                            }

                            var driverData = (from d in ent.Drivers
                                              join s in ent.StateMasters on d.StateMaster_Id equals s.Id
                                              join c in ent.CityMasters on d.CityMaster_Id equals c.Id
                                              join v in ent.Ambulances on d.Id equals v.Driver_Id
                                              join vt in ent.VehicleTypes on v.VehicleType_Id equals vt.Id
                                              where d.AdminLogin_Id == login.Id
                                              select new
                                              {
                                                  UserId = d.Id,
                                                  d.DriverName,
                                                  d.EmailId,
                                                  d.MobileNumber,
                                                  d.DlNumber,
                                                  s.StateName,
                                                  c.CityName,
                                                  d.StateMaster_Id,
                                                  d.CityMaster_Id,
                                                  d.Location,
                                                  d.AdminLogin_Id,
                                              }).FirstOrDefault();
                            if (driverData != null)
                            {
                                obj.role = "driver";
                                obj.data = driverData;
                                obj.Message = "Successfully Logged In";
                                obj.Status = 1;
                            }
                            else
                            {
                                obj.role = "driver";
                                obj.Message = "Successfully Logged In";
                                obj.Status = 1;
                            }
                            return Ok(obj);

                        case "patient":
                            var patientdata = ent.Patients.FirstOrDefault(a => a.AdminLogin_Id == login.Id);
                            if ((bool)patientdata.IsDeleted)
                            { 
                                return BadRequest("Your account is not available");
                            }
                            var patient = ent.Patients.FirstOrDefault(a => a.AdminLogin_Id == login.Id && a.IsDeleted == false);
                            
                            var patientData = (from d in ent.Patients
                                               join s in ent.StateMasters on d.StateMaster_Id equals s.Id
                                               join c in ent.CityMasters on d.CityMaster_Id equals c.Id
                                               where d.AdminLogin_Id == login.Id
                                               select new
                                               {
                                                   UserId = d.Id,
                                                   d.EmailId,
                                                   d.MobileNumber,
                                                   d.Location,
                                                   d.PatientName,
                                                   s.StateName,
                                                   c.CityName,
                                                   d.StateMaster_Id,
                                                   d.CityMaster_Id,
                                                   d.AdminLogin_Id,
                                                   d.PinCode,
                                               }).FirstOrDefault();

                            obj.role = "patient";
                            obj.data = patientData;
                            obj.Message = "Successfully Logged In";
                            obj.Status = 1;
                            return Ok(obj);

                        default:
                             
                            return BadRequest("Invalid role of user.");
                    } 
                }
                else
                {                    
                    return BadRequest("Invalid OTP");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }             
        }

        [HttpPost]
        [Route("api/CommonApi/UpdateDeviceId")]
        public IHttpActionResult UpdateDeviceId(AdminLoginDTO model)
        {
            try
            {
                string qry = @"Update AdminLogin Set DeviceId='" + model.DeviceId + "' Where Id='" + model.AdminLoginId + "'";
                ent.Database.ExecuteSqlCommand(qry);
                ent.SaveChanges();
                rm.Status = 1;
                rm.Message = "Device Id Updated Successfully.";
                return Ok(rm);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Server Error"));
            }    
        }

        [HttpGet]
        [Route("api/CommonApi/AssociateHospitalList")]
        public IHttpActionResult AssociateHospitalList()
        {
            string qry = @"select h.Id,h.HospitalName,h.HospitalImage,sm.StateName+' '+cm.CityName+' '+h.Location as Location  from Hospital as h
join StateMaster sm on sm.Id=h.StateMaster_Id
join CityMaster cm on cm.Id=h.CityMaster_Id";
            var AssociateHospital=ent.Database.SqlQuery<AssociateHospital>(qry);

            if(AssociateHospital==null)
            {
                return NotFound();
            }
            return Ok(new { AssociateHospital });
        }

        [HttpPost]
        [Route("api/CommonApi/UpdateBankDetail")]
        public IHttpActionResult UpdateBankDetail(BankDetailDTO model)
        {
            try
            {
                var data = ent.BankDetails.Where(b => b.Login_Id == model.Login_Id).FirstOrDefault();
                if (data == null)
                {
                    return BadRequest("No record found");
                }
                data.BranchName = model.BranchName;
                data.BranchAddress = model.BranchAddress;
                data.HolderName = model.HolderName;
                data.IFSCCode = model.IFSCCode;
                data.AccountNumber = model.AccountNumber;
                ent.SaveChanges();
                rm.Status = 1;
                rm.Message = "Bank details has updated.";
                return Ok(rm);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Internal server error."));
            }
        }

        [HttpGet]
        [Route("api/CommonApi/GetBankDetail")]
        public IHttpActionResult GetBankDetail(int AdminLoginId)
        {
            string qry = @"select Login_Id,AccountNumber,IFSCCode,BranchName,BranchAddress,HolderName,MobileNumber from BankDetails Where Login_Id="+ AdminLoginId + "";
            var BankDetail = ent.Database.SqlQuery<GetBankDetail>(qry).FirstOrDefault();

            if (BankDetail == null)
            {
                return NotFound();
            }
            return Ok(BankDetail);
        }
    }
}
