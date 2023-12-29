using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class StateDTO
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public IEnumerable<Statelist> Statelist { get; set; }
    }
    public class Statelist
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
    public class StateNamelist
    {
        public int Id { get; set; }
        public string StateName { get; set; }
    }
}