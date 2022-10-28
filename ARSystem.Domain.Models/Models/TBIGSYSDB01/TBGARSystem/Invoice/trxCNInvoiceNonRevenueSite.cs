using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
  public class trxCNInvoiceNonRevenueSite : BaseClass
  {
        public trxCNInvoiceNonRevenueSite()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public trxCNInvoiceNonRevenueSite(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public int trxCNInvoiceNonRevenueSiteID { get; set; }
        public int? trxInvoiceNonRevenueID { get; set; }
        public string SONumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteIDCustomer { get; set; }
        public string SiteNameCustomer { get; set; }
        public string CompanyID { get; set; }
        public string OperatorID { get; set; }
        public DateTime? StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
        public Decimal? Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
  }
}
