
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vwRABapsSite : BaseClass
    {
        public vwRABapsSite()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwRABapsSite(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string SONumber { get; set; }
        public string CustomerID { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public int? StipSiro { get; set; }
        public int? RegionID { get; set; }
        public string RegionName { get; set; }
        public string CompanyInvoiceId { get; set; }
        public int? MstBapsId { get; set; }
        public int? YearBill { get; set; }
        //added 23-8-21
        public int? MonthBill { get; set; }
        public int? ProvinceID { get; set; }
        public int? ProductID { get; set; }
        public int? SowProductId { get; set; }
        public int? SectionProductId { get; set; }
        public string KeySetting { get; set; }
        public long? TargetID { get; set; }
        public int? TargetYear { get; set; }
        public int? TargetMonth { get; set; }
        public string ReconcileType { get; set; }
        public string PowerType { get; set; }
        public string TargetBaps { get; set; }
        public string TargetPower { get; set; }
        //public string mstBapsTypeId { get; set; }
        public string ListIdString { get; set; }
        //department name: TSEL, NonTSEL, new Product & Others, BAPS
        public string DepartmentType { get; set; }
        public string DepartmentCode { get; set; }
        public DateTime? StartInvoiceDate { get; set; }
        public DateTime? EndInvoiceDate { get; set; }
        public decimal? AmountIDR { get; set; }
        public decimal? AmountUSD { get; set; }

    }
}