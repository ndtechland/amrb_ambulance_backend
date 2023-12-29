using AMBRD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Repositories
{

    public class CommonRepository
    {
        abdul_amurdEntities11 ent = new abdul_amurdEntities11();
        public IEnumerable<StateMaster> GetAllStates()
        {
            var data = ent.StateMasters.Where(a => a.IsDeleted==false).OrderBy(a => new { a.StateName }).ToList();
            return data;
        }
        public IEnumerable<CityMaster> GetCitiesByState(int? stateId)
        {
            var data = ent.CityMasters.Where(a => a.State_Id == stateId && a.IsDeleted==false).OrderBy(a => new { a.CityName }).ToList();
            return data;
        }
        public IEnumerable<VehicleCategory> GetVehicleCategory()
        {
            var data = ent.VehicleCategories.Where(a => a.IsDeleted == false).OrderBy(a => new { a.CategoryName }).ToList();
            return data;
        }

        public IEnumerable<Driver> GetAllDrivers()
        {
            var data = ent.Drivers.Where(a => a.IsDeleted==false).OrderBy(a => new { a.DriverName }).ToList();
            return data;
        }

        public IEnumerable<VehicleType> GetVehicleType()
        {
            var data = ent.VehicleTypes.Where(a => a.IsDeleted==false).OrderBy(a => new { a.VehicleTypeName });
            return data;
        }
        public IEnumerable<VehicleType> GetVehicleTypes()
        {
            string qry = @"select * from VehicleType join VehicleCategory on VehicleType.VehicleCatId = VehicleCategory.Id where VehicleType.IsDeleted  = 0 and VehicleCategory.IsDeleted  = 0";
            var data = ent.Database.SqlQuery<VehicleType>(qry).ToList();
            return data;
        }
        public IEnumerable<VehicleType> GetVehicleTypeByCat(int? Cat_Id)
        {
            var data = ent.VehicleTypes.Where(a => a.VehicleCatId == Cat_Id && a.IsDeleted == false).OrderBy(a => new { a.VehicleTypeName }).ToList();
            return data;
        }
    }
}