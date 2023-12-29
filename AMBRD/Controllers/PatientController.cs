using AMBRD.Models.ViewModels;
using AMBRD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMBRD.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        private abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        public ActionResult All()
        {
            string qry = @"select p.Id,p.IsApproved,p.PatientRegNo,p.PatientName,p.EmailId,p.MobileNumber,p.Location,cm.CityName,sm.StateName from Patient as p
Join StateMaster as sm on sm.Id= p.StateMaster_Id
Join CityMaster as cm on cm.Id=p.CityMaster_Id
where p.IsDeleted=0 order by Id desc";
            var data = ent.Database.SqlQuery<PatientDTO>(qry).ToList();

            if (data.Count() == 0)
            {
                TempData["message"] = "No data available.";
                return View(data);
            }
            return View(data);
        }

        public ActionResult DeletePatient(int id)
        {
            try
            {
                var data = ent.Patients.FirstOrDefault(a => a.Id == id);
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

    }
}