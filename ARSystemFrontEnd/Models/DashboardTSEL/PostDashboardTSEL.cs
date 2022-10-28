using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARSystem.Domain.Models;

namespace ARSystemFrontEnd.Models
{
    public class PostDashboardTSEL : DatatableAjaxModel
    {
        public string strCompanyInvoice { get; set; }
        public string strCustomerInvoice { get; set; }
        public int strSection { get; set; }
        public int strSOW { get; set; }
        public int strBillingYear { get; set; }
        public int strBillingMonth { get; set; }
        public int strRegional { get; set; }
        public int strProvince { get; set; }
        public int strTenantType { get; set; }
        public int isReceive { get; set; }
        public string schSONumber { get; set; }
        public List<string> strSONumberMultiple { get; set; }
        public string schSiteID { get; set; }
        public string schSiteName { get; set; }
        public string schCustomerSiteID { get; set; }
        public string schCustomerSiteName { get; set; }
        public string schCustomerID { get; set; }
        public string schRegionName { get; set; }
        public int schYearBill { get; set; }
        public int? schStipSiro { get; set; }
        public int strYearTargetHistory { get; set; }
        public int strMonthTargetHistory { get; set; }

        public List<string> KeySetting { get; set; }
        //public List<string> YearBill { get; set; }
        public vwRABapsSite vwRABapsSite { get; set; }
        public List<vwRABapsSite> vwRABapsSiteList { get; set; }
        public string YearTarget { get; set; }
        public string MonthBill { get; set; }
        public string BapsType { get; set; }
        public string PowerType { get; set; }
        public string YearTargetHS { get; set; }
        public string MonthTargetHS { get; set; }

        // Added or Mod by Ibnu Setiawan Friday, January 24, 2020  
        public string YearBill { get; set; }
        public string MonthBillName { get; set; }
        public string Targets { get; set; }

        //Add by ASE
        public string SecName { get; set; }
        public string SOWName { get; set; }
        public string IsOverdue { get; set; }

        public string type { get; set; }
        public int? STIPDate { get; set; }
        public int? RFIDate { get; set; }
        public int? SectionID { get; set; }
        public int? SOWID { get; set; }
        public int? ProductID { get; set; }
        public int? STIPID { get; set; }
        public int? RegionalID { get; set; }
        public string CompanyID { get; set; }

        public string Customer { get; set; }
        public string paramRow { get; set; }
        public string paramColumn { get; set; }
	//Added By Rama
	public string slReconType { get; set; }
        public string slPWRType { get; set; }

        public string SoNumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        //Add List
        //public List<MstRecurring> mstRecurring { get; set; }
        //update 23-8-2021
        // update 23-8-2021
        public DateTime? StartInvoiceDate { get; set; }
        public DateTime? EndInvoiceDate { get; set; }
        public string DepartmentCode { get; set; }
    }
}