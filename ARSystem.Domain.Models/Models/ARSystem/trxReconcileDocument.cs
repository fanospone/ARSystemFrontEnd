using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.Models.ARSystem
{
    public class trxReconcileDocument : BaseClass
    {
        public trxReconcileDocument()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public trxReconcileDocument(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int ID { get; set; }
        public string CustomerID { get; set; }
        public int? RegionID { get; set; }
        public string Batch { get; set; }
        public int? ReconYear { get; set; }
        public int? CustomerRegionID { get; set; }
        public int? TotalTenant { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
