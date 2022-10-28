using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vwRADashboardPICA : BaseClass
    {
        public vwRADashboardPICA()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwRADashboardPICA(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string RowIndex { get; set; }
        public string SONumber { get; set; }
        public string SiteId { get; set; }
        public string SiteIdOpr { get; set; }
        public string SiteName { get; set; }
        public string CustomerInvoice { get; set; }
        public string SiteNameOpr { get; set; }
        public string CustomerId { get; set; }
        public string CompanyId { get; set; }
        public string StipSiro { get; set; }
        public string BapsType { get; set; }
        public DateTime? StartBapsDate { get; set; }
        public DateTime? EndBapsDate { get; set; }
        public DateTime? StartDateInvoice { get; set; }
        public DateTime? EndDateInvoice { get; set; }
        public decimal AmountRental { get; set; }
        public decimal AmountService { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string ActivityName { get; set; }
        public int ActivityID { get; set; }
        public string ProductName { get; set; }
        public string CompanyInvoice { get; set; }
        public DateTime? EndTarget { get; set; }
        public string Durasi { get; set; }
        public DateTime? StartTarget { get; set; }
        public DateTime? StartActual { get; set; }
        public DateTime? EndActual { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LTActual { get; set; }
        public string CategoryPICA { get; set; }
        public string PICA { get; set; }
        public string DetailPICA { get; set; }
        public DateTime? TargetPICA { get; set; }
        public int ProductID { get; set; }
        public string StipCode { get; set; }

    }
}
