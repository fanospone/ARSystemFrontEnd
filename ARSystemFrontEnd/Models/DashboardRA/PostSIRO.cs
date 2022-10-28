using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARSystem.Domain.Models;


namespace ARSystemFrontEnd.Models
{
    public class PostSIRO : DatatableAjaxModel
    {

        public string Type { get; set; }
        public string Key { get; set; }
        public string ProductID { get; set; }
        public string STIPID { get; set; }
        public string Customer { get; set; }
        public string CompanyID { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Desc { get; set; }

        public string paramRow { get; set; }
        public string paramColumn { get; set; }
        public string SoNumber { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string Step { get; set; }


        
        //public string STIPCategory { get; set; }
        //public string RFIDateYear { get; set; }
        //public string RFIDateMonth { get; set; }
        //public string RFIDateWeek { get; set; }
        //public string SecName { get; set; }
        //public string SOWName { get; set; }
        //public string IsOverdue { get; set; }

        //type 
        //STIPDate 
        //SecName 
        //SOWName 
        //ProductID 
        //STIPID 
        //RegionalID  
        //CompanyID  
        //paramRow 
        //paramColumn 
        //YearBill 
        //Customer 
        //SoNumber 
        //SiteID 
        //SiteName 
        //Step 

    }

    public class PostMonthSummary
    {
        
    }
    public class PostSIROProSummary
    {

    }
}