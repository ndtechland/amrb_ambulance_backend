//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AMBRD.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PatientComplaint
    {
        public int Id { get; set; }
        public string Subjects { get; set; }
        public string Complaints { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsResolved { get; set; }
        public string Others { get; set; }
        public Nullable<int> Login_Id { get; set; }
        public string Roles { get; set; }
        public Nullable<System.DateTime> ComplaintDate { get; set; }
        public Nullable<int> patsubid { get; set; }
    }
}
