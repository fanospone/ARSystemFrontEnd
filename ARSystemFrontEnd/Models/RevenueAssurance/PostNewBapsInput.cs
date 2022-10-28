using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostNewBapsInput : DatatableAjaxModel
    {
        public string strSoNumber { get; set; }
        public string strCompanyId { get; set; }
        public string strProductId { get; set; }
        public string strGroupBy { get; set; }
        public string strSiteID { get; set; }
        public string strBapsType { get; set; }
        public string strAction { get; set; }
        public string strCustomerId { get; set; }
        public string strTenantTypeID { get; set; }
        public string strDataType { get; set; }
        public int strSiro { get; set; }



        //public string strSiteId { get; set; }
        //public string strCustomerId { get; set; }
        //public string strBapsNumber { get; set; }
        //public string strPowerTypeID { get; set; }
        //public string strServiceAmount { get; set; }
        //public string strInvoiceAmount { get; set; }
        //public string strRegionalID { get; set; }
        //public string strEndBapsDate { get; set; }
        //public string strBapsDoneDate { get; set; }
        //public string strStartBapsDate { get; set; }
        //public string strEffectiveBapsDate { get; set; }
        //public string strStartInvoiceDate { get; set; }
        //public string strEndInvoiceDate { get; set; }
        //public string strRemarks { get; set; }




        //public string strBapsTypeId { get; set; }
        //public string strBulkNumber { get; set; }
        //public string strRegionId { get; set; }
        //public string strStipCategory { get; set; }
        //public int strBulkID { get; set; }


        //public int strIDTrx { get; set; }
        //public string strSiteIDCustomer { get; set; }




    }

    //public class PostSubmitTrxBaps : DatatableAjaxModel
    //{
    //    public string strSoNumber { get; set; }
    //    public string strSiteId { get; set; }
    //    public string strCustomerId { get; set; }
    //    public string strBapsNumber { get; set; }
    //    public string strPowerTypeID { get; set; }
    //    public string strServiceAmount { get; set; }
    //    public string strInvoiceAmount { get; set; }
    //    public string strRegionalID { get; set; }
    //    public string strEndBapsDate { get; set; }
    //    public string strBapsDoneDate { get; set; }
    //    public string strStartBapsDate { get; set; }
    //    public string strEffectiveBapsDate { get; set; }
    //    public string strStartInvoiceDate { get; set; }
    //    public string strEndInvoiceDate { get; set; }
    //    public string strRemarks { get; set; }
    //}
}