using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMBRD.Models.ApiModels
{
    public class BankDetailDTO
    {
        public int Id { get; set; }
        public Nullable<int> Login_Id { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string HolderName { get; set; }
        public string MobileNumber { get; set; }
        public string CancelCheque { get; set; }
        public Nullable<bool> isverified { get; set; }
    }

    public class GetBankDetail
    {
        public Nullable<int> Login_Id { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string HolderName { get; set; }
        public string MobileNumber { get; set; }
    }
}