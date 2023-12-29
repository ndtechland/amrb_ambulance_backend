using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class CommissionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<double> Commission { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public IEnumerable<CommissionList> CommissionList { get; set; }
    }

    public class CommissionList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<double> Commission { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}