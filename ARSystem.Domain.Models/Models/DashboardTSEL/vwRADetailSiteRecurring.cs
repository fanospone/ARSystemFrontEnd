
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vwRADetailSiteRecurring : BaseClass
    {
        public vwRADetailSiteRecurring()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwRADetailSiteRecurring(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string SONumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string CustomerID { get; set; }
        public string RegionalName { get; set; }
        public string ProvinceName { get; set; }
        public string ResidenceName { get; set; }
        public string PoNumber { get; set; }
        public string MLANumber { get; set; }
        public DateTime? StartBapsDate { get; set; }
        public DateTime? EndBapsDate { get; set; }
        public int? Term { get; set; }
        public string BapsType { get; set; }
        public string CustomerInvoice { get; set; }
        public string CompanyInvoice { get; set; }
        public string Company { get; set; }
        public int? StipSiro { get; set; }
        public string Currency { get; set; }
        public DateTime? StartInvoiceDate { get; set; }
        public DateTime? EndInvoiceDate { get; set; }
        public decimal? BaseLeasePrice { get; set; }
        public decimal? ServicePrice { get; set; }
        public decimal? DeductionAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int? YearBill { get; set; }
        public int? MonthBill { get; set; }
        public string MonthBillName { get; set; }
        public int mstBapsTypeId { get; set; }
        public string PowerTypeCode { get; set; }
        public long RowIndex { get; set; }

        public string SowName { get; set; }
        public int AvgLeadTime { get; set; }
        public int AvgSection { get; set; }

        public DateTime? DateConfirm { get; set; }
        public DateTime? RTIDate { get; set; }

        // Modification Or Added By Ibnu Setiawan 31. January 2020 Add Field
        public decimal AvgAmountAchievement { get; set; }
    }
}