using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostBAUKDetail : DatatableAjaxModel
    {
        public string SONumber { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string CompanyID { get; set; }
        public string Company { get; set; }
        public string STIPCode { get; set; }
        public string Product { get; set; }
        public string RegionName { get; set; }
        public string ProvinceName { get; set; }
        public string ResidenceName { get; set; }
        public decimal STIPAmount { get; set; }
        public int LeadTime { get; set; }
        public DateTime? BAUKFirstSubmitDate { get; set; }
        public DateTime? BAUKLastSubmitDate { get; set; }
        public DateTime? BAUKApprovalDate { get; set; }
        public DateTime? BAUKForecastDate { get; set; }
        public string BAUKStatus { get; set; }
        public DateTime? RFIDoneDate { get; set; }
        public List<string> CustomerIDs { get; set; }
        public List<string> CompanyIDs { get; set; }
        public List<int> STIPIDs { get; set; }
        public List<int> ProductIDs { get; set; }
        public List<int> Months { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string GroupBy { get; set; }
        public string strCustomer { get; set; }
        public string strCompany { get; set; }
        public string strSTIP { get; set; }
        public string strProduct { get; set; }
        public string strMonth { get; set; }
        public string SelectedData { get; set; }
    }
}