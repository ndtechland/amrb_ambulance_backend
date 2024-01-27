using AMBRD.Models;
using AMBRD.Models.ViewModels;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMBRD.Controllers
{
    public class AmbulanceReportController : Controller
    {
        // GET: AmbulanceReport
        abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        public ActionResult Ambulance()
        {
            return View();
        }

        public ActionResult Daily(string term, DateTime? date)
        {
            var model = new AmbulanceReportDTO();
            
            var qry = @"select trm.Id,convert (varchar,trm.EntryDate,107) as EntryDate,v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName, 
v.Id as VehicleId
from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id
join Ambulance v on v.Driver_Id = trm.Driver_Id
join Patient p on p.Id = trm.PatientId
where trm.EntryDate > CAST(GETDATE() AS DATE) and trm.IsPay='Y' and trm.RideComplete=1 group by v.VehicleNumber, v.VehicleName,v.Id,trm.Id,trm.EntryDate";
            var data = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry).ToList();
            if (date == null)
            {
                if (data.Count() == 0)
                {
                    TempData["msg"] = "No Record of Current Date";
                }
                else
                {
                    model.AmbulanceYMWDRecord = data;
                }
            }
           
            return View(model);
        }

        public ActionResult DailyRecord(int Id, DateTime? sdate, DateTime? edate)
        {
            var model = new AmbulanceReportDTO(); 
            var qry = @"select trm.Id,p.PatientName, v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName,trm.TotalPrice as Amount,trm.TotalDistance as Distance, d.DriverName,v.Id as VehicleId,
trm.start_Lat,trm.start_Long,trm.end_Lat,trm.end_Long
from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id 
join Ambulance v on v.Driver_Id = trm.Driver_Id 
join Patient p on p.Id = trm.PatientId 
where Month(trm.EntryDate) = Month(GetDate()) and trm.Id=" + Id;
            var data = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry).ToList();
            if (sdate == null && edate == null)
            {
                if (data.Count() == 0)
                {
                    TempData["msg"] = "No Record of Current Date";
                }
                else
                {
                    model.AmbulanceYMWDRecord = data;
                    //ViewBag.Payment = payment;
                    //ViewBag.Total = model.LabList.Sum(a => a.Amount);
                }
            }
            else
            {
                var qry1 = @"select trm.Id,p.PatientName, v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName,trm.TotalPrice as Amount,trm.TotalDistance as Distance, d.DriverName,v.Id as VehicleId,
trm.start_Lat,trm.start_Long,trm.end_Lat,trm.end_Long
from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id 
join Ambulance v on v.Driver_Id = trm.Driver_Id 
join Patient p on p.Id = trm.PatientId 
where v.Id = " + Id + " and Convert(Date,trm.EntryDate)  between '" + sdate + "' and '" + edate + "'group by v.VehicleNumber, v.VehicleName, v.Id";
                var data1 = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry1).ToList();
                if (data1.Count() == 0)
                {
                    TempData["msg"] = "Your Selected Date Doesn't Contain any Information.";
                }
                else
                {
                    //ViewBag.Payment = payment;
                    model.AmbulanceYMWDRecord = data1;
                    //ViewBag.Total = model.LabList.Sum(a => a.Amount);
                    return View(model);
                }
            }
            return View(model);
        }
        
        public ActionResult Weekly(string term, DateTime? sdate)
        {
            var model = new AmbulanceReportDTO(); 
            var qry = @"select dl.Id,convert (varchar,dl.EntryDate,107) as EntryDate,v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName, 
v.Id as VehicleId
from DriverLocation dl 
join Driver d on d.Id = dl.Driver_Id
join Ambulance v on v.Driver_Id = dl.Driver_Id
join Patient p on p.Id = dl.PatientId
where dl.EntryDate between DateAdd(DD,-7,GETDATE() ) and GETDATE() and dl.IsPay='Y' and dl.RideComplete=1  group by v.VehicleNumber, v.VehicleName,v.Id,dl.Id,dl.EntryDate";
            var data = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry).ToList();
            
            if (sdate == null)
            {
                if (data.Count() == 0)
                {
                    TempData["msg"] = "No Record of Current Week";
                }
                else
                {
                    model.AmbulanceYMWDRecord = data;
                }
            }
            else
            {
                var qry1 = @"SELECT trm.Id,v.VehicleNumber,ISNULL(v.VehicleName, 'NA') AS VehicleName,v.Id AS VehicleId FROM DriverLocation trm
JOIN Driver d ON d.Id = trm.Driver_Id
JOIN Ambulance v ON v.Driver_Id = trm.Driver_Id
JOIN Patient p ON p.Id = trm.PatientId
WHERE cast(trm.EntryDate as Date) = '" + Convert.ToDateTime(sdate).ToString("yyyy/MM/dd") + "' GROUP BY v.VehicleNumber, v.VehicleName, v.Id, trm.Id";
                var data1 = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry1).ToList();
                if (data1.Count() == 0)
                {
                    TempData["msg"] = "Your Selected Date Doesn't Contain any Information.";
                }
                else
                {
                    model.AmbulanceYMWDRecord = data1;
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult WeeklyReport(int Id, DateTime? date)
        {
            var model = new AmbulanceReportDTO();

            var qry = @"select p.PatientName, v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName,trm.TotalPrice as Amount, trm.TotalDistance as Distance, d.DriverName,v.Id as VehicleId,trm.start_Lat,trm.start_Long,trm.end_Lat,trm.end_Long from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id
join Ambulance v on v.Driver_Id = trm.Driver_Id
join Patient p on p.Id = trm.PatientId
where trm.EntryDate between DateAdd(DD,-7,GETDATE() ) and GETDATE() and trm.Id=" + Id;
            var data = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry).ToList();
            if (date == null)
            {
                if (data.Count() == 0)
                {
                    TempData["msg"] = "No Record of Current Date";
                }
                else
                {
                    model.AmbulanceYMWDRecord = data;
                }
            }
            else
            {
                DateTime dateCriteria = date.Value.AddDays(-7);
                string Tarikh = dateCriteria.ToString("dd/MM/yyyy");
                var qry1 = @"select p.PatientName, v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName,trm.TotalPrice as Amount, trm.TotalDistance as Distance, d.DriverName,v.Id as VehicleId,trm.start_Lat,trm.start_Long,trm.end_Lat,trm.end_Long from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id
join Ambulance v on v.Driver_Id = trm.Driver_Id
join Patient p on p.Id = trm.PatientId
where v.Id = " + Id + " and trm.EntryDate between '" + dateCriteria + "' and '" + date + "'  group by v.VehicleNumber, v.VehicleName, v.Id";
                var data1 = ent.Database.SqlQuery<Ambulance>(qry1).ToList();
                if (data1.Count() == 0)
                {
                    TempData["msg"] = "Your Selected Date Doesn't Contain any Information.";
                }
                else
                {
                    model.AmbulanceYMWDRecord = data;
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult Monthly(string term, DateTime? sdate, DateTime? edate)
        {
            var model = new AmbulanceReportDTO();
            var qry = @"select trm.Id,convert (varchar,trm.EntryDate,107) as EntryDate,v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName, 
v.Id as VehicleId
from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id
join Ambulance v on v.Driver_Id = trm.Driver_Id
join Patient p on p.Id = trm.PatientId
where Month(trm.EntryDate) = Month(GetDate()) and trm.IsPay='Y' and trm.RideComplete=1 group by v.VehicleNumber, v.VehicleName,v.Id,trm.Id,trm.EntryDate";
            var data = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry).ToList();
            if (sdate == null && edate == null)
            {
                if (data.Count() == 0)
                {
                    TempData["msg"] = "No Record of Current Month";
                }
                else
                {
                    model.AmbulanceYMWDRecord = data;
                    //ViewBag.Payment = payment;
                    //ViewBag.Total = model.LabList.Sum(a => a.Amount);
                }
            }
            else
            {
                var qry1 = @"select v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName, 
v.Id as VehicleId
from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id
join Ambulance v on v.Driver_Id = trm.Driver_Id
join Patient p on p.Id = trm.PatientId
where  Convert(Date,trm.EntryDate)  between Convert(datetime,'" + sdate + "',103) and Convert(datetime,'" + edate + "',103) and trm.IsPay='Y' group by v.VehicleNumber, v.VehicleName, v.Id";
                var data1 = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry1).ToList();
                if (data1.Count() == 0)
                {
                    TempData["msg"] = "Your Selected Date Doesn't Contain any Information.";
                }
                else
                { 
                    model.AmbulanceYMWDRecord = data1; 
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult MonthlyRecord(int Id, DateTime? sdate, DateTime? edate)
        {
            var model = new AmbulanceReportDTO();
            //double payment = ent.Database.SqlQuery<double>(@"select Commission from CommissionMaster where IsDeleted=0 and Name='" + term + "'").FirstOrDefault();
            var qry = @"select trm.id,p.PatientName, v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName,trm.TotalPrice as Amount, 
trm.TotalDistance as Distance, d.DriverName,v.Id as VehicleId,trm.start_Lat,trm.start_Long,trm.end_Lat,trm.end_Long 
from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id
join Ambulance v on v.Driver_Id = trm.Driver_Id
join Patient p on p.Id = trm.PatientId
where  Month(trm.EntryDate) = Month(GetDate()) and trm.Id=" + Id;
            var data = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry).ToList();
            if (sdate == null && edate == null)
            {
                if (data.Count() == 0)
                {
                    TempData["msg"] = "No Record of Current Date";
                }
                else
                {
                    model.AmbulanceYMWDRecord = data;
                    //ViewBag.Payment = payment;
                    //ViewBag.Total = model.LabList.Sum(a => a.Amount);
                }
            }
            else
            {
                var qry1 = @"select trm.id,p.PatientName, v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName, 
trm.Amount, trm.ToatlDistance as Distance, d.DriverName, 
v.Id as VehicleId,trm.start_Lat,trm.start_Long,trm.end_Lat,trm.end_Long
from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id
join Ambulance v on v.Driver_Id = trm.Driver_Id
join Patient p on p.Id = trm.PatientId
where v.Id = " + Id + " and Convert(Date,trm.EntryDate)  between '" + sdate + "' and '" + edate + "'group by v.VehicleNumber, v.VehicleName, v.Id";
                var data1 = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry1).ToList();
                if (data1.Count() == 0)
                {
                    TempData["msg"] = "Your Selected Date Doesn't Contain any Information.";
                }
                else
                {
                    //ViewBag.Payment = payment;
                    model.AmbulanceYMWDRecord = data1;
                    //ViewBag.Total = model.LabList.Sum(a => a.Amount);
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult Yearly(string term, int? year)
        {
            var model = new AmbulanceReportDTO(); 
            var qry = @"select trm.Id,convert (varchar,trm.EntryDate,107) as EntryDate,v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName, 
v.Id as VehicleId
from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id
join Ambulance v on v.Driver_Id = trm.Driver_Id
join Patient p on p.Id = trm.PatientId
where Year(trm.EntryDate) = Year(GetDate()) and trm.IsPay='Y' and trm.RideComplete=1 group by v.VehicleNumber, v.VehicleName,v.Id,trm.Id,trm.EntryDate";
            var data = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry).ToList();
            if (year == null)
            {
                if (data.Count() == 0)
                {
                    TempData["msg"] = "No Record of Current Year";
                }
                else
                {
                    model.AmbulanceYMWDRecord = data; 
                }
            }
            else
            {
                var qry1 = @"select v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName, 
v.Id as VehicleId
from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id
join Ambulance v on v.Driver_Id = trm.Driver_Id
join Patient p on p.Id = trm.PatientId
where Year(trm.EntryDate) = '" + year + "' group by v.VehicleNumber, v.VehicleName, v.Id";
                var data1 = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry1).ToList();
                if (data1.Count() == 0)
                {
                    TempData["msg"] = "Your Selected Date Doesn't Contain any Information.";
                }
                else
                { 
                    model.AmbulanceYMWDRecord = data1; 
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult YearlyRecord(int Id, int? year)
        {
            var model = new AmbulanceReportDTO(); 
            var qry = @"select trm.Id,p.PatientName, v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName, 
trm.TotalPrice as Amount, trm.TotalDistance as Distance, d.DriverName, 
v.Id as VehicleId,trm.start_Lat,trm.start_Long,trm.end_Lat,trm.end_Long
from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id
join Ambulance v on v.Driver_Id = trm.Driver_Id
join Patient p on p.Id = trm.PatientId
where Year(trm.EntryDate) = Year(GetDate()) and trm.Id=" + Id;
            var data = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry).ToList();
            if (year == null)
            {
                if (data.Count() == 0)
                {
                    TempData["msg"] = "No Record of Current Date";
                }
                else
                {
                    model.AmbulanceYMWDRecord = data; 
                }
            }
            else
            {
                var qry1 = @"select trm.Id,p.PatientName, v.VehicleNumber, IsNull(v.VehicleName,'NA') as VehicleName, 
trm.TotalPrice as Amount, trm.TotalDistance as Distance, d.DriverName, 
v.Id as VehicleId,trm.start_Lat,trm.start_Long,trm.end_Lat,trm.end_Long
from DriverLocation trm 
join Driver d on d.Id = trm.Driver_Id
join Ambulance v on v.Driver_Id = trm.Driver_Id
join Patient p on p.Id = trm.PatientId
where Year(trm.EntryDate) = '" + year + "' and trm.Id=" + Id;
                var data1 = ent.Database.SqlQuery<AmbulanceYMWDRecord>(qry1).ToList();
                if (data1.Count() == 0)
                {
                    TempData["msg"] = "Your Selected Date Doesn't Contain any Information.";
                }
                else
                { 
                    model.AmbulanceYMWDRecord = data1; 
                    return View(model);
                }
            }
            return View(model);
        }
    }
}