using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vwDashboardInputTargetDetail : BaseClass
    {
        public vwDashboardInputTargetDetail()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwDashboardInputTargetDetail(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string SONumber { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public string DepartmentCode { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string CustomerID { get; set; }
        public string CompanyInvoiceID { get; set; }
        public DateTime? StartInvoiceDate { get; set; }
        public DateTime? EndInvoiceDate { get; set; }
        public decimal? AmountIDR { get; set; }
        public decimal? AmountUSD { get; set; }
        public string RegionalName { get; set; }

        public string DepartmentName { get; set; }
    }
}
