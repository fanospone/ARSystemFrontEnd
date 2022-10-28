using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.Models.RevenueAssurance
{
    public class mstBAPSRecurring : BaseClass
    {
        public mstBAPSRecurring()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public mstBAPSRecurring(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int ID { get; set; }
        public string BAPSNumber { get; set; }
        public string CompanyID { get; set; }
        public string CustomerID { get; set; }
        public int? TotalTenant { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Remarks { get; set; }
        public string RemarksApproval { get; set; }
        public DateTime? BAPSSignDate { get; set; }
        public int mstRAActivityID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
