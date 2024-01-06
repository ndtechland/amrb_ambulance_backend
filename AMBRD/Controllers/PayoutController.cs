using AMBRD.Models;
using AMBRD.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMBRD.Controllers
{
    public class PayoutController : Controller
    {
        abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        // GET: Payout
        public ActionResult Driver()
        {
            double commision = ent.Database.SqlQuery<double>(@"select Commission from CommissionMaster where IsDelete=0 and Name='Ambulance'").FirstOrDefault();
            var model = new AmbulancesReport();
            string q = @"select dl.Driver_Id,v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName,v.Id as VehicleId, d.DriverName,
Sum(dl.Amount) as Amount from DriverLocation dl 
 join Driver d on d.Id = dl.Driver_Id 
 join Ambulance v on v.Driver_Id = dl.Driver_Id 
 join Patient p on p.Id = dl.PatientId
where dl.EntryDate between DateAdd(DD,-7,GETDATE() ) and GETDATE() and dl.IsPay='Y' and dl.RideComplete=1 group by v.VehicleNumber, v.VehicleName, v.Id,d.DriverName,dl.Driver_Id";
            var data = ent.Database.SqlQuery<AmbulanceData>(q).ToList();
            
             model.AmbulanceData = data;
            ViewBag.GetCommision = (double?)commision;
            return View(model);
        }

        public ActionResult DriverPay(int Driver_Id, double Amount)
        {
            var model = new DriverPayOut();
            model.Amount = Amount;
            model.IsPaid = false;
            model.IsGenerated = true;
            model.PaymentDate = DateTime.Now.Date;
            model.Driver_Id = Driver_Id;
            ent.DriverPayOuts.Add(model);
            ent.SaveChanges();
            return RedirectToAction("ViewDriverPayoutHistory", new { Id = model.Driver_Id });
        }

        public ActionResult ViewDriverPayoutHistory(int Id, DateTime? date)
        {
            Session["msg"] = Id;
            var model = new ViewPayoutHistory();
            var Name = ent.Database.SqlQuery<string>("select DriverName from Driver where Id=" + Id).FirstOrDefault();
            model.DriverName = Name;
            string qry = @"Select dp.Id, ISNULL(dp.IsPaid, 0) as IsPaid , Dp.IsGenerated, dp.Driver_Id, dp.PaymentDate, dp.Amount,d.DriverName from DriverPayOut as dp join Driver as d on d.Id=dp.Driver_Id where dp.Driver_Id=" + Id;
            var data = ent.Database.SqlQuery<HistoryOfAmbulance_Payout>(qry).ToList();
            if (data.Count() == 0)
            {
                TempData["msg"] = "No Records";
                return View(model);
            }
            else
            {
                model.HistoryOfAmbulance_Payout = data;
            }
            return View(model);
        }


    }
}