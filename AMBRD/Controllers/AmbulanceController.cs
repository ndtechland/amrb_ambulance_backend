using AMBRD.Models;
using AMBRD.Models.ViewModels;
using AMBRD.Repositories;
using AMBRD.Utilities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace AMBRD.Controllers
{
    public class AmbulanceController : Controller
    {
        // GET: Ambulance
        abdul_amurdEntities11 ent =new abdul_amurdEntities11 ();
        CommonRepository repos = new CommonRepository ();   
        public ActionResult AddAmbulance()
        {
            var model = new AmbulanceDTO();
            model.Drivers = new SelectList(repos.GetAllDrivers(), "Id", "DriverName");
            model.CategoryList = new SelectList(repos.GetVehicleCategory(), "Id", "CategoryName");
            model.VehicleTypes = new List<SelectListItem>(ent.VehicleTypes.Where(a => a.IsDeleted == false).AsEnumerable().Select(a => new SelectListItem { Text = a.VehicleTypeName, Value = a.Id.ToString() }));


            return View(model);
        }

        [HttpPost]
        public ActionResult AddAmbulance(AmbulanceDTO model)
        {
            model.CategoryList = new SelectList(repos.GetVehicleCategory(), "Id", "CategoryName");
            model.VehicleTypes = new List<SelectListItem>(ent.VehicleTypes.Where(a => a.IsDeleted == false).AsEnumerable().Select(a => new SelectListItem { Text = a.VehicleTypeName, Value = a.Id.ToString() }));
            try
            { 
                if (ent.Ambulances.Any(a => a.VehicleNumber == model.VehicleNumber && a.IsDeleted==false))
                {
                    TempData["msg"] = "This Vehicle Number Already Registered With Us.";
                    return View(model);
                }


                if (model.RCImageFileBase64 != null)
                {
                    var RCImg = FileOperation.UploadImage(model.RCImageFileBase64, "Images");
                    if (RCImg == "not allowed")
                    {
                        TempData["msg"] = "Only png,jpg,jpeg docs with RC are allowed.";
                        return View(model);
                    }
                    model.RCImage = RCImg;
                }

                if (model.VehicleCImageFileBase64 != null)
                {
                    var VehicleImg = FileOperation.UploadImage(model.VehicleCImageFileBase64, "Images");
                    if (VehicleImg == "not allowed")
                    {
                        TempData["msg"] = "Only png,jpg,jpeg docs with RC are allowed.";
                        return View(model);
                    }
                    model.VehicleImg = VehicleImg;
                }

                var domainModel = new Ambulance();
                domainModel.VehicleName=model.VehicleName;
                domainModel.VehicleNumber=model.VehicleNumber;
                domainModel.VehicleOwnerName=model.VehicleOwnerName;
                //domainModel.DriverCharges=model.DriverCharges;
                domainModel.VehicleCat_Id=model.VehicleCat_Id;
                domainModel.VehicleType_Id=model.VehicleType_Id;
                domainModel.RCImage = model.RCImage;
                domainModel.VehicleImg = model.VehicleImg;
                domainModel.RC_Number = model.RC_Number;
                domainModel.RC_Validity = model.RC_Validity;
                //domainModel.Driver_Id=model.Driver_Id;
                domainModel.IFSCCode=model.IFSCCode;
                domainModel.AccountNo=model.AccountNo; 
                domainModel.IsDeleted = false;
                domainModel.IsApproved = false;
                domainModel.RegistrationDate = DateTime.Now;

                ent.Ambulances.Add(domainModel);
                ent.SaveChanges();
                TempData["msg"] = "ok";
            }
            catch (Exception )
            { 
                TempData["msg"] = "Server Error";
            }
            return RedirectToAction("AddAmbulance");
        }

        public ActionResult All()
        {
            var model = new AmbulanceDTO();
            string q = @"select a.*,vt.VehicleTypeName from Ambulance a 
join VehicleType vt on a.VehicleType_Id = vt.Id 
where a.IsDeleted=0 order by a.Id desc";
            var data = ent.Database.SqlQuery<AmbulanceDTO>(q).ToList();
            
            if (data.Count() == 0)
            {
                TempData["message"] = "No data available.";
                return View(data);
            }
            
            return View(data);
        }

        public ActionResult DeleteAmbulance(int id)
        {
            try
            {
                var data = ent.Ambulances.FirstOrDefault(a => a.Id == id);
                if (data != null)
                {
                    data.IsDeleted = true;
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Deleted successfully";
                }
                return RedirectToAction("All");
            }
            catch  
            {
                throw new Exception("Server error");
            }
        }

        public ActionResult EditAmbulance(int id)
        {
            var data = ent.Ambulances.FirstOrDefault(a => a.Id ==id);
            if (data != null)
            {
                var model = new AmbulanceDTO
                {
                    VehicleName=data.VehicleName,
                    VehicleNumber=data.VehicleNumber,
                    VehicleOwnerName=data.VehicleOwnerName,
                    DriverCharges = data.DriverCharges,
                    //VehicleCat_Id = data.VehicleCat_Id,
                    //VehicleType_Id = data.VehicleType_Id,
                    BranchName =data.BranchName,
                    BranchAddress=data.BranchAddress,
                    IFSCCode=data.IFSCCode,
                    HolderName=data.HolderName,
                    AccountNo=data.AccountNo,
                };
                //model.CategoryList = new SelectList(repos.GetVehicleCategory(), "Id", "CategoryName", data.VehicleCat_Id);
                //model.VehicleCat_Id = (int)data.VehicleCat_Id;
                //model.VehicleTypelist = new SelectList(repos.GetVehicleTypeByCat(model.VehicleType_Id), "Id", "VehicleTypeName");

                //model.VehicleType_Id = (int)data.VehicleType_Id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
            
        }
        [HttpPost]
        public ActionResult EditAmbulance(AmbulanceDTO model)
        {
           // model.CategoryList = new SelectList(repos.GetVehicleCategory(), "Id", "CategoryName", model.VehicleCat_Id);
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var data = ent.Ambulances.FirstOrDefault(a => a.Id == model.Id);
                if (data != null)
                {
                    var DomailModel = ent.Ambulances.Find(model.Id);
                    {
                        DomailModel.VehicleName = model.VehicleName;
                        DomailModel.VehicleNumber = model.VehicleNumber;
                        //DomailModel.VehicleCat_Id = model.VehicleCat_Id;
                        //DomailModel.VehicleType_Id = model.VehicleType_Id;
                        DomailModel.VehicleOwnerName = model.VehicleOwnerName;
                        DomailModel.DriverCharges = model.DriverCharges;
                        DomailModel.BranchName = model.BranchName;
                        DomailModel.BranchAddress = model.BranchAddress;
                        DomailModel.HolderName = model.HolderName;
                        DomailModel.IFSCCode = model.IFSCCode;
                        DomailModel.AccountNo = model.AccountNo;
                    }
                    
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "Edit successfully";
                }
                
                return RedirectToAction("All");
            }
            catch (Exception)
            {
                TempData["msg"] = "Server error";
            }
            return View();
        }

        public ActionResult UpdateApproveStatus(int id)
        {
            string qry = @"update Ambulance set IsApproved = case when IsApproved=1 then 0 else 1 end where id=" + id;
            ent.Database.ExecuteSqlCommand(qry);
            return RedirectToAction("All");
        }
        
        public JsonResult GetVehicleNumberList(int vehicleTypeId, string term)
        
        {
            var VehicleList = (from N in ent.Ambulances
                               where N.VehicleNumber.StartsWith(term)
                               && N.IsDeleted == false
                               && N.VehicleType_Id == vehicleTypeId
                               select new { N.VehicleNumber, N.Id });

            return Json(VehicleList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDriverName(string term)
        {
            var DriverList = (from d in ent.Drivers
                              where d.DriverName.StartsWith(term)
                              && d.IsDeleted == false
                              select new { d.DriverName, d.Id });
            return Json(DriverList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult VehicleAllotment(int? Id, VehicleAllotmentDTO model)
        {
            //Using AutoSearch get Vehicle Number from passing model 
            if (model.VehicleNumber != null)
            {
                //model.VehicleList = new List<SelectListItem>(ent.VehicleTypes.Select(a => new SelectListItem { Text = a.VehicleTypeName, Value = a.Id.ToString() }));
                var list = ent.Ambulances.Where(a => a.VehicleNumber == model.VehicleNumber).ToList();
                int Ids = list.FirstOrDefault().Id;
                if (Ids != 0)
                {
                    string qry = @"select vt.Id as VehicleTypeId, a.Id as VehicleId, d.Id, d.DriverName, a.VehicleNumber from VehicleType vt join Ambulance a on vt.Id = a.VehicleType_Id join Driver d on d.Id = a.Driver_Id  where a.Id=" + Ids;
                    var data = ent.Database.SqlQuery<VehicleLists>(qry).ToList();
                    model.VehicleLists = data;
                    return View(model);
                }
            }
             
            if (Id != null)
            {
                string qry = @"select D.Id as DriverId, IsNull(d.DriverName,'NA') as DriverName,IsNull(v.VehicleNumber,'NA') AS VehicleNumber,vt.Id as VehicleTypeId,coalesce(cast(V.Id as varchar(255)), 'NA') as VehicleId from Driver d  join VehicleType vt on vt.Id = d.VehicleType_Id left join Vehicle v on d.Id  = v.Driver_Id where vt.Id=" + Id;
                var data = ent.Database.SqlQuery<VehicleLists>(qry).ToList();
                model.VehicleLists = data;
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult UpdateDriver(int? Id)
        {
            TempData["Id"] = Id;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateDriver(VehicleAllotmentDTO model)
        {
            var list = ent.Drivers.Where(a => a.DriverName == model.DriverName).ToList();
            var DriverId = list.FirstOrDefault().Id;
            var Name = list.FirstOrDefault().DriverName;
            if (ent.Ambulances.Any(a => a.Driver_Id == DriverId))
            {
                string VehicleNumber1 = ent.Database.SqlQuery<string>("select VehicleNumber from Ambulance where Driver_Id=" + DriverId).FirstOrDefault();
                TempData["msg"] = "The Selected Driver is Already Running on " + VehicleNumber1;
                return RedirectToAction("UpdateDriver", new { Rid = model.Id });
            }
            string q = @"update Ambulance set Driver_Id = " + DriverId + "  where Id=" + model.Id; 
            ent.Database.ExecuteSqlCommand(q);
            string VehicleNumber = ent.Database.SqlQuery<string>("select VehicleNumber from Ambulance where Driver_Id=" + DriverId).FirstOrDefault();
            TempData["msg"] = "The Vehicle Number" + VehicleNumber + " has been Replaced to " + Name;
            return RedirectToAction("UpdateDriver", new {Rid = model.Id });

        }

        public ActionResult UpdateVehicle(int? Id, VehicleAllotmentDTO model)
        {
            //Using AutoSearch get Vehicle Number from passing model 
            model.VehicleList = new SelectList(repos.GetVehicleType(), "Id", "VehicleTypeName");
            string Q = @"select * from driver where VehicleType_Id is null and IsDeleted = 0";
            var data = ent.Database.SqlQuery<VehicleLists>(Q).ToList();
            model.VehicleLists = data;
            return View(model);
        }

        [HttpGet]
        public ActionResult UpdateVehicles(int? Id)
        {
            var model = new VehicleAllotmentDTO();
            model.VehicleList = new SelectList(repos.GetVehicleType(), "Id", "VehicleTypeName");
            TempData["Id"] = Id;
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateVehicles(VehicleAllotmentDTO model)
        {
            model.VehicleList = new SelectList(repos.GetVehicleType(), "Id", "VehicleTypeName");
            var list = ent.Ambulances.Where(a => a.VehicleNumber == model.VehicleNumber).ToList();
            var VehileId = list.FirstOrDefault().Id;
            var Number = list.FirstOrDefault().VehicleNumber;
            int DriverId = Convert.ToInt32(TempData["Id"]);
            if (ent.Drivers.Any(a => a.Ambulance_Id == VehileId))
            {
                string VehicleNumber1 = ent.Database.SqlQuery<string>("select DriverName from Driver where Ambulance_Id=" + VehileId).FirstOrDefault();
                TempData["msg"] = "The Selected Vehicle is Already Running on " + VehicleNumber1;
                return RedirectToAction("UpdateVehicles", new { Rid = model.Id });
            }
            string q = @"update Ambulance set Driver_Id = " + DriverId + "  where Id=" + VehileId;
            ent.Database.ExecuteSqlCommand(q);

            string qry = @"update Driver set VehicleType_Id = " + model.Id + ",Ambulance_Id="+ VehileId + "  where Id=" + DriverId;
            ent.Database.ExecuteSqlCommand(qry);
            string Name = ent.Database.SqlQuery<string>("select DriverName from Driver where Id=" + DriverId).FirstOrDefault();
            string VehicleNumber = ent.Database.SqlQuery<string>("select VehicleNumber from Ambulance where Id=" + VehileId).FirstOrDefault();
            TempData["msg"] = "The Vehicle Number" + VehicleNumber + " has been Replaced to " + Name;
            
            return RedirectToAction("UpdateVehicles", new { Rid = model.Id });
        }
    }
}