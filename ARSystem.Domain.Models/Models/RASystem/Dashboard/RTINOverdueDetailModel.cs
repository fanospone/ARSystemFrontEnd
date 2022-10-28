using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
   public class RTINOverdueDetailModel : BaseClass
    {
        public RTINOverdueDetailModel()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public RTINOverdueDetailModel(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public Int64 RowIndex { get; set; }
        public string SoNumber { get; set; }
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string CompanyId { get; set; }
        public string CustomerId { get; set; }
        public DateTime StartBapsDate { get; set; }
        public DateTime EndBapsDate { get; set; }
        public int Term { get; set; }
        public int Year { get; set; }

        //added new 2022-05-19
        public int YearTarget { get; set; }
        public int MonthTarget { get; set; }
        public DateTime? RTIDate { get; set; }
        public DateTime? FinanceConfirmDate { get; set; }
        public DateTime? CreateInvoiceDate { get; set; }
        public DateTime? PostingInvoiceDate { get; set; }
        public string BillingCycle { get; set; }
        public string TypeBaps { get; set; }
        public string TypePower { get; set; }

        public DateTime StartInvoiceDate { get; set; }
        public DateTime EndInvoiceDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal AmountRental { get; set; }
        public decimal AmountService { get; set; }

        public decimal? InvoiceAmountUsd { get; set; }
        public decimal? InvoiceAmountIdrKursRate { get; set; }

        public string Currency { get; set; }
        public string CurrentStatus { get; set; }
        public string DataType { get; set; }
        public DateTime BAPSConfirmDate { get; set; }
        public DateTime RTIDoneRADate { get; set; }



    }
}
