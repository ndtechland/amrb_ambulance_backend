using AMBRD.Models;
using AMBRD.Models.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace AMBRD.Controllers
{
    public class StateController : Controller
    {
        abdul_amurdEntities11 ent=new abdul_amurdEntities11();
        // GET: State
        public ActionResult AddState()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult AddState(StateDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                if (ent.StateMasters.Where(a => a.IsDeleted == false).Any(a => a.StateName == model.StateName))
                {
                    TempData["msg"] = "The State Name  " + model.StateName + " Already Exists";
                    return RedirectToAction("AddState");
                }
                var domainModel = new StateMaster();
                domainModel.StateName = model.StateName;
                domainModel.IsDeleted = false;
                ent.StateMasters.Add(domainModel);
                ent.SaveChanges();
                TempData["msg"] = "ok";

            }
            catch (Exception )
            {
                //log.Error(ex.Message);
                TempData["msg"] = "Server Error";
            }
            return RedirectToAction("AddState");
        }

        public ActionResult StateList()
        {
            var model = new StateDTO();
            string q = @"select * from StateMaster where IsDeleted=0 order by StateName";
            var data = ent.Database.SqlQuery<Statelist>(q).ToList();
            if (data.Count() == 0)
            {
                TempData["msg"] = "No Record";
                return View(model);
            }
            model.Statelist = data;
            return View(model);
        }

        public ActionResult Deletestate(int id)
        {
            try
            {
                var data =ent.StateMasters.FirstOrDefault(x => x.Id == id);
                if (data != null) 
                {
                    //ent.StateMasters.Remove (data);
                    data.IsDeleted = true;
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "State deleted successfully";
                }
                return RedirectToAction("StateList");
            }
            catch
            {
                throw new Exception("Server error");
            }

        }

        [Obsolete]
        public ActionResult EditSate(int id)
        {
            var data = ent.StateMasters.Find(id);
            Mapper.CreateMap<StateMaster, StateDTO>();
            var model = Mapper.Map<StateDTO>(data);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditSate(StateDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                //Mapper.CreateMap<StateMaster, StateDTO>();
                //var domainModel = Mapper.Map<StateDTO>(model); 
                var existingState = ent.StateMasters.Find(model.Id);
                if (existingState != null)
                {
                    existingState.StateName = model.StateName;
                    ent.SaveChanges();
                    TempData["SuccessMessage"] = "State edit successfully";
                }

                
                return RedirectToAction("StateList");
            }
            catch (Exception)
            {
                //log.Error(ex.Message);
                TempData["msg"] = "Server Error";
                return RedirectToAction("EditSate", model.Id);
            }
        }
    }
}

 