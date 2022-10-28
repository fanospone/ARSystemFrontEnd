using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostDasboardMonitoringBAPSDone : DatatableAjaxModel
    {
        public string CustomerID { get; set; }
        public string CompanyID { get; set; }
        public int STIPID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int RegionID { get; set; }
        public int ProvinceID { get; set; }
        public int ProductID { get; set; }
        public string PowerTypeID { get; set; }
        public string  BapsType { get; set; }
        public string GroupBy { get; set; }
        public string Desc { get; set; }
        public string DescID { get; set; }
        public int StipSiro { get; set; }
        public string DataType { get; set; }

        //added by ASE
        public string schSONumber { get; set; }
        public string schSiteID { get; set; }
        public string schSiteName { get; set; }
        public string schSiteCustID { get; set; }
        public string schSiteCustName { get; set; }
    }
}