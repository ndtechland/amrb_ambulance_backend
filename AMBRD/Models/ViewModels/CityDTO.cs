using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMBRD.Models.ViewModels
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public Nullable<int> State_Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public SelectList States { get; set; }
        public IEnumerable<Citylist> Citylist { get; set; }
    }
    public class Citylist
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public Nullable<int> State_Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; } 
    }
}