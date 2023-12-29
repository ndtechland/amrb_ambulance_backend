using AMBRD.Models.ViewModels;
using AMBRD.Models.ApiModels;
using AMBRD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AMBRD.Models.ViewModels.DriverDTO;

namespace AMBRD.Controllers
{
    public class DriverController : Controller
    {
        // GET: Driver
        private abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        public ActionResult All()
        {
            string qry = @"select d.Id,d.IsApproved,d.DriverName,d.EmailId,d.MobileNumber,d.Location,d.DlNumber,d.DlImage,cm.CityName,sm.StateName from Driver as d
Join StateMaster as sm on sm.Id= d.StateMaster_Id
Join CityMaster as cm on cm.Id=d.CityMaster_Id
where d.IsDeleted=0 order by Id desc";
            var data = ent.Database.SqlQuery<DriverDTO>(qry).ToList();

            if (data.Count() == 0)
            {
                TempData["message"] = "No data available.";
                return View(data);
            }
            return View(data);
        }

        public ActionResult DeleteDriver(int id)
        {
            try
            {
                var data = ent.Drivers.FirstOrDefault(a => a.Id == id);
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

        public ActionResult UpdateApproveStatus(int id)
        {
            string qry = @"update Driver set IsApproved = case when IsApproved=1 then 0 else 1 end where id=" + id;
            ent.Database.ExecuteSqlCommand(qry);
            return RedirectToAction("All");
        }
    }
}