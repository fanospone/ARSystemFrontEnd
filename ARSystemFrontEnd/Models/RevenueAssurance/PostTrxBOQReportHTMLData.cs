using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    //public class PostTrxBOQReportHTMLData : DatatableAjaxModel
    //{
    //    public long? No { get; set; }
    //    public int ID { get; set; }
    //    public string BatchID { get; set; }
    //    public string PrintID { get; set; }
    //    public string BOQNumber { get; set; }
    //    public string CompanyName { get; set; }
    //    public string Area { get; set; }
    //    public string RegionName { get; set; }
    //    public string SiteID { get; set; }
    //    public string PrevSiteID { get; set; }
    //    public string SiteName { get; set; }
    //    public string Decription { get; set; }
    //    public decimal? Longitude { get; set; }
    //    public decimal? Latitude { get; set; }
    //    public string Address { get; set; }
    //    public string InitialPONumber { get; set; }
    //    public string InitialPODate { get; set; }
    //    public string Mla { get; set; }
    //    public string FirstBAPSDate { get; set; }       
    //    public string RFIDate { get; set; }
    //    public string bapsStartDate { get; set; }
    //    public string bapsEndDate { get; set; }
    //    public int? ContractPeriod { get; set; }
    //    public decimal? BaseLeasePrice { get; set; }
    //    public decimal? ServicePrice { get; set; }
    //    public decimal? AmountPerMonth { get; set; }
    //    public decimal? AmountPerYear { get; set; }
    //    public decimal? AmountBilled { get; set; }
    //    public decimal? AmountDeduction { get; set; }
    //    public decimal? AmountAfterDeduction { get; set; }
    //    public int? Term { get; set; }
    //    public string StartInvoiceDate { get; set; }
    //    public string EndInvoiceDate { get; set; }
    //    public string OprCompanyName { get; set; }
    //    public string OprPICName { get; set; }
    //    public string OprPosition { get; set; }
    //    public string CompCompanyName { get; set; }
    //    public string CompPICName { get; set; }
    //    public string CompPosition { get; set; }

    //}


    public class PostTrxBOQReportHTMLData
    {

      public  List<ARSystemService.vwBOQDataReport> listModel = new List<ARSystemService.vwBOQDataReport>();
        
      public List<ARSystemService.trxRABOQSignatory> listApprove = new List<ARSystemService.trxRABOQSignatory>();
      public string htmlHeader { get; set; }
      public string htmlString { get; set; }
      public string htmlApproval { get; set; }

        //public string OprCompanyName { get; set; }
        //public string OprPICName { get; set; }
        //public string OprPosition { get; set; }
        //public string ComCompanyName { get; set; }
        //public string ComPICName { get; set; }
        //public string ComPosition { get; set; }        

    }
} 