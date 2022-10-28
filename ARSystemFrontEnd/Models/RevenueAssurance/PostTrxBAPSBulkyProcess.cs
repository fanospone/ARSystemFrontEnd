using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxBAPSBulkyProcess : DatatableAjaxModel
    {
        public int ID { get; set; }
        public int trxReconcileID { get; set; }
        public string SoNumber { get; set; }
        public string BAPSNumber { get; set; }
        public string CompanyID { get; set; }
        public string CustomerID { get; set; }
        public int? TotalTenant { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Remarks { get; set; }
        public string RemarksApproval { get; set; }
        public int mstRAActivityID { get; set; }
        public DateTime BAPSSignDate { get; set; }
        public List<int> detailIDs { get; set; }
        public List<trxReconcile> ListTrxBAPS { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string CustomerMLANumber { get; set; }
        public decimal? BaseLeasePrice { get; set; }
        public decimal? DeductionAmount { get; set; }
        public decimal? ServicePrice { get; set; }
        public decimal? AmountIDR { get; set; }
        public string RFIOprDate { get; set; }

    }
} 