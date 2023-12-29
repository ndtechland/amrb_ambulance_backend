using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class VehicleCategoryDTO
    {
        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CategoryName { get; set; }
        public string Type { get; set; }
        public Nullable<int> AmbulanceType_Id { get; set; }
        public IEnumerable<CategoryList> CategoryList { get; set; }
    }
    public class CategoryList
    {
        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CategoryName { get; set; }
        public string Type { get; set; }
        public Nullable<int> AmbulanceType_Id { get; set; }
    }
}