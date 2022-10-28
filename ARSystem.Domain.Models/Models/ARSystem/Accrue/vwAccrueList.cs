
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vwAccrueList : BaseClass
    {
        public vwAccrueList()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwAccrueList(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string IDTemp { get; set; }
        public string SONumber { get; set; }
        public int? RegionID { get; set; }
        public string RegionName { get; set; }
        public DateTime EndDatePeriod { get; set; }
        public string CompanyID { get; set; }
        public string Company { get; set; }
        public string SiteID { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string SiteName { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string STIPNumber { get; set; }
        public string PONumber { get; set; }
        public string BAUK_No { get; set; }
        public string BAPS { get; set; }
        public string InvoiceSONumber { get; set; }
        public string TypeSOW { get; set; }
        public string StatusAccrue { get; set; }
        public decimal? BaseLeasePrice { get; set; }
        public decimal? ServicePrice { get; set; }
        public DateTime? StartBapsDate { get; set; }
        public DateTime? EndBapsDate { get; set; }
        public DateTime? StartDateAccrue { get; set; }
        public DateTime EndDateAccrue { get; set; }
        public string Currency { get; set; }
        public string StatusMasterList { get; set; }
        public string CompanyInvoiceID { get; set; }
        public DateTime? BAPSDate { get; set; }
        public DateTime? RFIDate { get; set; }
        public DateTime? SldDate { get; set; }
        public string Type { get; set; }
        public string DirectorateName { get; set; }
        public string SOW { get; set; }
        public string Accrue { get; set; }
        public string MioAccrue { get; set; }
        public int? Month { get; set; }
        public int? D { get; set; }
        public string OD { get; set; }
        public string ODCATEGORY { get; set; }
        public string Unearned { get; set; }
        public string TenantType { get; set; }
        public decimal? AmountTotal { get; set; }
    }
}