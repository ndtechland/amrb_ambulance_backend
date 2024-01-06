using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ViewModels
{
    public class ViewPayoutHistory
    {
        public string DriverName { get; set; }
        public IEnumerable<HistoryOfAmbulance_Payout> HistoryOfAmbulance_Payout { get; set; }
    }
    public class HistoryOfAmbulance_Payout
    {

        public int Id { get; set; }
        public int Driver_Id { get; set; }
        public string DriverName { get; set; }
        public string VehicleName { get; set; }
        public string VehicleNumber { get; set; }
        public string PAN { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double Amount { get; set; }

        public bool? IsGenerated { get; set; }
        public bool? IsPaid { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string HolderName { get; set; }
    }
}