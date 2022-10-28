using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostUploadAccrueView : DatatableAjaxModel
    {
        public PostUploadAccrueView()
        {
            ListID = new List<string>();
        }

        public virtual List<string> ListID { get; set; }
        public long ID { get; set; }
        public string SONumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteIDOpr { get; set; }
        public string SiteNameOpr { get; set; }
        public string CompanyID { get; set; }
        public string CompanyInvID { get; set; }
        public string CustomerID { get; set; }
        public DateTime? RFIDate { get; set; }
        public DateTime? SldDate { get; set; }
        public DateTime? RentalStart { get; set; }
        public DateTime? RentalEndNew { get; set; }
        public string TenantType { get; set; }
        public string Type { get; set; }
        public decimal? BaseLeasePrice { get; set; }
        public decimal? ServicePrice { get; set; }
        public decimal? BaseOnMasterListData { get; set; }
        public decimal? BaseOnRevenueListingNew { get; set; }
        public DateTime? StartDateAccrue { get; set; }
        public DateTime? EndDateAccrue { get; set; }
        public decimal? TotalAccrue { get; set; }
        public string MonthYear { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? Week { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsValid { get; set; }
        public bool IsActive { get; set; }
        public int? AccrueStatusID { get; set; }
    }
}