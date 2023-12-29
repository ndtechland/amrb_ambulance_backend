using AMBRD.Models;
using AMBRD.Models.ViewModels;
using AMBRD.Repositories;
using AMBRD.Utilities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMBRD.Controllers
{
    public class VehicleController : Controller
    {
        // GET: Vehicle
        abdul_amurdEntities11 ent=new abdul_amurdEntities11();
        CommonRepository repos=new CommonRepository();
        public ActionResult AddVehicleCategory()
        {
            var model = new VehicleCategoryDTO();
            string qry = " select * from VehicleCategory where IsDeleted= 0 order by CategoryName asc";
            var data = ent.Database.SqlQuery<CategoryList>(qry).ToList();
            if (data.Count() == 0)
            {
                TempData["list"] = "No Records";
                return View(model);
            }
            model.CategoryList = data;
            return View(model);
        }

        [HttpPost]
        public ActionResult AddVehicleCategory(VehicleCategoryDTO model)
        {
            var DomainModel = new VehicleCategory()
            {
                CategoryName= model.CategoryName,
                AmbulanceType_Id= model.AmbulanceType_Id,
                IsDeleted= false, 
            };
            if (model.AmbulanceType_Id == 1)
            {
                DomainModel.Type = "Regular";
            }
            else if (model.AmbulanceType_Id == 2)
            {
                DomainModel.Type = "Road Accident";
            }
            else if (model.AmbulanceType_Id == 3)
            {
                DomainModel.Type = "Funeral/MortuaryService";

            }

            ent.VehicleCategories.Add(DomainModel);
            ent.SaveChanges();
            TempData["msg"] = "ok";
            return RedirectToAction("AddVehicleCategory");
        }

        public ActionResult DeleteCategory(int Id)
        {
            var data=ent.VehicleCategories.FirstOrDefault(c => c.Id == Id);
            if (data != null)
            {
                data.IsDeleted = true;
                ent.SaveChanges();
                TempData["SuccessMessage"] = "Deleted successfully";
                
            }
            return RedirectToAction("AddVehicleCategory");
        }

        public ActionResult EditCategory(int id)
        {
            var data = ent.VehicleCategories.Find(id);

            if (data != null)
            {
                var model = new VehicleCategoryDTO
                {
                    CategoryName = data.CategoryName,  
                    Type = data.Type,
                    AmbulanceType_Id = data.AmbulanceType_Id, 
                };

                return View(model);
            }
            else
            {
                return HttpNotFound(); 
            }
        } 

        [HttpPost]
        public ActionResult EditCategory(VehicleCategoryDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var domainModel = ent.VehicleCategories.Find(model.Id);
                if (domainModel != null)
                {
                    domainModel.CategoryName = model.CategoryName;  
                } 
                if (model.AmbulanceType_Id == 1)
                {
                    domainModel.Type = "Regular";
                }
                else if (model.AmbulanceType_Id == 2)
                {
                    domainModel.Type = "Road Accident";
                }
                else if (model.AmbulanceType_Id == 3)
                {
                    domainModel.Type = "Funeral/MortuaryService";

                }                 
                ent.SaveChanges();
                return RedirectToAction("AddVehicleCategory");
            }
            catch
            {
                TempData["msg"] = "Server Error";
                return RedirectToAction("AddVehicleCategory", model.Id);
            }

        }

        public ActionResult AddVehicleType()
        {
            var model = new VehicleTypeDTO();
            model.CategoryList = new SelectList(repos.GetVehicleCategory(), "Id", "CategoryName");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddVehicleType(VehicleTypeDTO model)
        {
            try
            {
                model.CategoryList = new SelectList(repos.GetVehicleCategory(), "Id", "CategoryName");
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
              
                var domainModel = new VehicleType()
                {
                    VehicleTypeName = model.VehicleTypeName,
                    VehicleCatId = model.VehicleCatId,
                    under0_2KM = model.under0_2KM,
                    under3_8KM = model.under3_8KM,
                    under9_15KM = model.under9_15KM,
                    under16_25KM = model.under16_25KM,
                    under26_40KM = model.under26_40KM,
                    under41_60KM = model.under41_60KM,
                    under61_80KM = model.under61_80KM,
                    under81_110KM = model.under81_110KM,
                    under111_150KM = model.under111_150KM,
                    under151_200KM = model.under151_200KM,
                    under201_250KM = model.under201_250KM,
                    under251_300KM = model.under251_300KM,
                    under301_350KM = model.under301_350KM,
                    //under401_450KM = model.under401_450KM,
                    //under451_500KM = model.under451_500KM,
                    Above500KM = model.Above500KM,
                    IsDeleted = false,

                };
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    domainModel.VehicleImage = uploadResult;

                }
                ent.VehicleTypes.Add(domainModel);
                ent.SaveChanges();
                TempData["msg"] = "ok";
            }
            catch (Exception)
            {
                //log.Error(ex.Message);
                TempData["msg"] = "Server Error";
            }
            return RedirectToAction("AddVehicleType");
        }

        public ActionResult AllVehicleTypelist(int? CategoryId)
        {
            var model = new VehicleTypeDTO();
            model.CategoryList = new SelectList(repos.GetVehicleCategory(), "Id", "CategoryName");
            string q = @"select * from VehicleType where IsDeleted=0  order by VehicleTypeName asc";
            var data = ent.Database.SqlQuery<ListVehicleType>(q).ToList();
            if (data.Count() == 0)
            {
                TempData["msg"] = "No Records";
                return View(model);
            }
            //if (CategoryId != 0 && CategoryId != null)
            //{
            //    string q1 = @"select * from VehicleType vt join VehicleCategory vc on vc.Id = vt.VehicleCatId where vt.IsDeleted=0 and vc.IsDeleted=0 and vt.VehicleCatId=0 and vc.IsDeleted=0 and vt.VehicleCatId=" + CategoryId + " order by CategoryName asc";
            //    var data1 = ent.Database.SqlQuery<ListVehicleType>(q1).ToList();
            //    if (data1.Count() == 0)
            //    {
            //        TempData["msg"] = "No Records";
            //        return View(model);
            //    }
            //    model.ListVehicleType = data1;
            //    return View(model);
            //}
            model.ListVehicleType = data;
            return View(model);
        }

        public ActionResult DeleteVehicleType (int Id)
        {
            var data=ent.VehicleTypes.FirstOrDefault(x => x.Id == Id);
            if(data != null)
            {
                data.IsDeleted=true ;
                ent.SaveChanges();
                TempData["SuccessMessage"] = "Deleted successfully";
            }
            return RedirectToAction("AllVehicleTypelist");
        }

        public ActionResult EditVehicleType(int Id)
        {
            var data = ent.VehicleTypes.Find(Id);

            if (data != null)
            {
                var model = new VehicleTypeDTO
                {
                    VehicleTypeName = data.VehicleTypeName, 
                    VehicleCatId = data.VehicleCatId,
                    under0_2KM = data.under0_2KM,
                    under3_8KM = data.under3_8KM,
                    under9_15KM = data.under9_15KM,
                    under16_25KM = data.under16_25KM,
                    under26_40KM = data.under26_40KM,
                    under41_60KM = data.under41_60KM,
                    under61_80KM = data.under61_80KM,
                    under81_110KM = data.under81_110KM,
                    under111_150KM = data.under111_150KM,
                    under151_200KM = data.under151_200KM,
                    under201_250KM = data.under201_250KM,
                    under251_300KM = data.under251_300KM,
                    under301_350KM = data.under301_350KM,
                    //under401_450KM = data.under401_450KM,
                    //under451_500KM = data.under451_500KM,
                    Above500KM = data.Above500KM,


                };
                 
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult EditVehicleType(VehicleTypeDTO model)
        {
            model.CategoryList = new SelectList(repos.GetVehicleCategory(), "Id", "CategoryName", model.VehicleCatId);
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                
                model.CategoryList = new SelectList(repos.GetVehicleCategory(), "Id", "CategoryName");
                var existingVehicleType = ent.VehicleTypes.Find(model.Id);
                {
                    existingVehicleType.VehicleTypeName = model.VehicleTypeName;
                    existingVehicleType.VehicleCatId = model.VehicleCatId;
                    existingVehicleType.under0_2KM = model.under0_2KM;
                    existingVehicleType.under3_8KM = model.under3_8KM;
                    existingVehicleType.under9_15KM = model.under9_15KM;
                    existingVehicleType.under16_25KM = model.under16_25KM;
                    existingVehicleType.under26_40KM = model.under26_40KM;
                    existingVehicleType.under41_60KM = model.under41_60KM;
                    existingVehicleType.under61_80KM = model.under61_80KM;
                    existingVehicleType.under81_110KM = model.under81_110KM;
                    existingVehicleType.under111_150KM = model.under111_150KM;
                    existingVehicleType.under151_200KM = model.under151_200KM;
                    existingVehicleType.under201_250KM = model.under201_250KM;
                    existingVehicleType.under251_300KM = model.under251_300KM;
                    existingVehicleType.under301_350KM = model.under301_350KM;
                    //existingVehicleType.under401_450KM = model.under401_450KM;
                    //existingVehicleType.under451_500KM = model.under451_500KM;
                    existingVehicleType.Above500KM = model.Above500KM;
                };
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentLength > 2 * 1024 * 1024)
                    {
                        TempData["msg"] = "Image should not succeed 2 mb";
                        return View(model);
                    }
                    var uploadResult = FileOperation.UploadImage(model.ImageFile, "Images");
                    if (uploadResult == "not allowed")
                    {
                        TempData["msg"] = "Only .jpg,.jpeg,.png and .gif files are allowed";
                        return View(model);
                    }

                    existingVehicleType.VehicleImage = uploadResult;

                }
                ent.SaveChanges();
                TempData["SuccessMessage"] = "Edit successfully";
                return RedirectToAction("AllVehicleTypelist");
            }
            catch (Exception)
            {
                //log.Error(ex.Message);
                TempData["msg"] = "Server Error";
                return RedirectToAction("Edit", model.Id);
            }
        }        
    }
}