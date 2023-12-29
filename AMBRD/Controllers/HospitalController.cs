using AMBRD.Models.ViewModels;
using AMBRD.Models;
using AMBRD.Utilities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using AMBRD.Repositories;
using AMBRD.BL;
using System.Data.SqlClient;

namespace AMBRD.Controllers
{
    public class HospitalController : Controller
    {
        private abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        CommonRepository repos = new CommonRepository();
        GenerateBookingId bk = new GenerateBookingId();
        // GET: Hospital
        public ActionResult Add()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Default");

            var model = new HospitalDTO(); 
            model.States = new SelectList(repos.GetAllStates(), "Id", "StateName");
            return View(model);
        }
        [HttpPost]
        [Obsolete]
        public ActionResult Add(HospitalDTO model)
        {
            try
            {
            
                model.States = new SelectList(repos.GetAllStates(), "Id", "StateName");
                if (ent.Hospitals.Any(a => a.HospitalName == model.HospitalName && a.MobileNumber == model.MobileNumber))
                {
                    var data = ent.Hospitals.Where(a => a.HospitalName == model.HospitalName && a.MobileNumber == model.MobileNumber).FirstOrDefault();
                    var logdata = ent.AdminLogins.Where(a => a.UserID == data.HospitalId).FirstOrDefault();
                    string mssg = "Welcome to AMBRD. Your User Name :  " + logdata.Username + "(" + logdata.UserID + "), Password : " + logdata.Password + ".";

                    TempData["msg"] = "you are already registered with AMBRD";
                    return RedirectToAction("Add", "Hospital");
                }

                var admin = new AdminLogin
                {
                    Username = model.EmailId,
                    PhoneNumber = model.MobileNumber,
                    Password = model.Password,
                    Role = "hospital"
                };
                ent.AdminLogins.Add(admin);
                ent.SaveChanges();


                // Hospital Image 
                if (model.HospitalImageFile == null)
                {
                    TempData["msg"] = "Hospital Image File Picture can not be null"; 
                    return View(model);
                }
                var img = FileOperation.UploadImage(model.HospitalImageFile, "Images");
                if (img == "not allowed")
                {
                    TempData["msg"] = "Only png,jpg,jpeg docs with RC are allowed.";
                    return View(model);
                }
                model.HospitalImage = img; 

                var domainModel = new Hospital();
                domainModel.AdminLogin_Id = admin.Id;
                domainModel.HospitalName = model.HospitalName;
                domainModel.StateMaster_Id = model.StateMaster_Id;
                domainModel.CityMaster_Id = model.CityMaster_Id;
                domainModel.MobileNumber = model.MobileNumber;
                domainModel.LandlineNumber = model.LandlineNumber;
                domainModel.EmailId = model.EmailId;
                domainModel.Location = model.Location;
                domainModel.PinCode = model.PinCode;
                domainModel.Password = model.Password;
                domainModel.HospitalImage = model.HospitalImage;
                domainModel.IsDeleted = false;
                domainModel.IsApproved = false;
                domainModel.HospitalId = bk.GenerateHospitalId();
                admin.UserID = domainModel.HospitalId;
                admin.Confirmpassword = model.ConfirmPassword;
                ent.Hospitals.Add(domainModel);
                ent.SaveChanges();
                string msg = "Welcome to AMBRD. Your User Name :  " + domainModel.EmailId + "(" + domainModel.HospitalId + "), Password : " + admin.Password + ".";


                string msg1 = "Welcome to AMBRD. Your User Name :  " + admin.Username + "(" + admin.UserID + "), Password : " + admin.Password + ".";

                TempData["msg"] = "ok";

            }
            catch (Exception)
            {
                TempData["msg"] = "Server Error";
            } 
            return RedirectToAction("Add");
        } 

        public ActionResult All()
        {
            string qry = @"select * from Hospital where IsDeleted=0 order by Id desc";
            var data = ent.Database.SqlQuery<HospitalDTO>(qry).ToList();

            if (data.Count() == 0)
            {
                TempData["message"] = "No data available.";
                return View(data);
            }

            return View(data);
        }

        public ActionResult UpdateApproveStatus(int id)
        {
            string qry = @"update Hospital set IsApproved= case when IsApproved=1 then 0 else 1 end where Id="+ id;
            ent.Database.ExecuteSqlCommand(qry);
            return RedirectToAction("All");
        }

        public ActionResult DeleteHospital(int id)
        {
            var data = ent.Hospitals.FirstOrDefault(a => a.Id == id);
            if(data != null)
            {
                data.IsDeleted = true;                
                ent.SaveChanges();
                TempData["SuccessMessage"] = "Deleted successfully.";
            }
            
            return RedirectToAction("All");
        }

        public ActionResult EditHospital(int id)
        {
            var data = ent.Hospitals.FirstOrDefault(a => a.Id == id);

            if (data != null)
            {
                var model = new HospitalDTO
                {
                    HospitalName = data.HospitalName,
                    MobileNumber = data.MobileNumber,
                    LandlineNumber = data.LandlineNumber,
                    EmailId = data.EmailId,
                    StateMaster_Id = data.StateMaster_Id,
                    CityMaster_Id = data.CityMaster_Id,
                    Location = data.Location,
                    PinCode = data.PinCode,
                }; 
                model.States = new SelectList(repos.GetAllStates(), "Id", "StateName", model.StateMaster_Id);
                  
                model.StateMaster_Id = (int)data.StateMaster_Id; 
                model.Cities = new SelectList(repos.GetCitiesByState(model.StateMaster_Id), "Id", "CityName");
                 
                model.CityMaster_Id = (int)data.CityMaster_Id;

                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult EditHospital(HospitalDTO model)
        {
            model.States = new SelectList(repos.GetAllStates(), "Id", "StateName", model.StateMaster_Id);

            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var data = ent.Hospitals.FirstOrDefault(a => a.Id == model.Id);
                if (data != null)
                {
                    var DomailModel = ent.Hospitals.Find(model.Id);
                    {
                        DomailModel.HospitalName = model.HospitalName;
                        DomailModel.MobileNumber = model.MobileNumber;
                        DomailModel.LandlineNumber = model.LandlineNumber;
                        DomailModel.EmailId = model.EmailId;
                        DomailModel.Location = model.Location;
                        DomailModel.StateMaster_Id = model.StateMaster_Id;
                        DomailModel.CityMaster_Id = model.CityMaster_Id;
                        DomailModel.PinCode = model.PinCode; 
                    }

                    ent.SaveChanges();
                }

                return RedirectToAction("All");
            }
            catch (Exception)
            {
                TempData["msg"] = "Server error";
            }
            return View();
        }

    }
}