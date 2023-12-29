using AMBRD.Models;
using AMBRD.Models.ViewModels;
using AMBRD.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace AMBRD.Controllers
{
    public class CityController : Controller
    {
        abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        CommonRepository repos = new CommonRepository();
        // GET: City
        public ActionResult AddCity()
        {
            var model = new CityDTO();
            model.States = new SelectList(repos.GetAllStates(), "Id", "StateName");
            return View(model);
        }
        [HttpPost]
        public ActionResult AddCity(CityDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.States = new SelectList(repos.GetAllStates(), "Id", "StateName");
                    return View(model);
                }
                if (ent.CityMasters.Where(a=>a.IsDeleted==false).Any(a => a.CityName == model.CityName))
                {
                    TempData["msg"] = "The City Name  " + model.CityName + " Already Exists";
                    return RedirectToAction("AddCity");
                }
                var domainModel = new CityMaster();
                domainModel.CityName = model.CityName;
                domainModel.State_Id = model.State_Id;       
                domainModel.IsDeleted = false;       
                ent.CityMasters.Add(domainModel);
                ent.SaveChanges();
                TempData["msg"] = "ok";

            }
            catch (Exception )
            {
                //log.Error(ex.Message);
                TempData["msg"] = "Server Error";
            }
            return RedirectToAction("AddCity");
        }
        public ActionResult CityList()
        {
            var model = new CityDTO();
            string q = @"select cm.Id,sm.StateName,cm.CityName from CityMaster as cm join StateMaster as sm on sm.Id=cm.State_Id where cm.IsDeleted=0 and sm.IsDeleted=0 order by cm.CityName";
            var data = ent.Database.SqlQuery<Citylist>(q).ToList();
            if (data.Count() == 0)
            {
                TempData["msg"] = "No Record";
                return View(model);
            }
            model.Citylist = data;
            return View(model);
        }

        public ActionResult DeleteCity(int id)
        {
            try
            {
                var data = ent.CityMasters.FirstOrDefault(a => a.Id == id);
                if(data != null)
                {
                    data.IsDeleted = true;
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "City deleted successfully";
                }
                return RedirectToAction("CityList");
            }
            catch
            {
                throw new Exception("Server error");
            }
        }

        [Obsolete]
        public ActionResult EditCity(int id)
        {  
            var data = ent.CityMasters.Find(id);
            Mapper.CreateMap<CityMaster, CityDTO>();
            var model = Mapper.Map<CityDTO>(data);
            model.States = new SelectList(repos.GetAllStates(), "Id", "StateName", model.State_Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditCity(CityDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var existingCity = ent.CityMasters.Find(model.Id);
                if(existingCity != null)
                {
                    existingCity.CityName = model.CityName;
                    existingCity.State_Id = model.State_Id;
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "City edit successfully";
                }                
                return RedirectToAction("CityList");
            }
            catch
            {
                TempData["msg"] = "Server Error";
                return RedirectToAction("EditCity", model.Id);
            }            
        }
    }
}