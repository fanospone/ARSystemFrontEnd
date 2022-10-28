using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmSaveBAPSBulky : BaseClass
    {
        public vmSaveBAPSBulky()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmSaveBAPSBulky(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int ID { get; set; }
        public int trxReconcileID { get; set; }
        public string SONumber { get; set; }
        public string BAPSNumber { get; set; }
        public string CompanyID { get; set; }
        public string CustomerID { get; set; }
        public int? TotalTenant { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime BAPSSignDate { get; set; }
        public string Remarks { get; set; }
        public string RemarksApproval { get; set; }
        public int mstRAActivityID { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string CustomerMLANumber { get; set; }
        public decimal? BaseLeasePrice { get; set; }
        public decimal? DeductionAmount { get; set; }
        public decimal? ServicePrice { get; set; }
        public decimal? AmountIDR { get; set; }
        public DateTime? RFIOprDate { get; set; }
        public trxRAUploadDocument UploadDoc { get; set; }
        public List<trxReconcile> ListTrxBAPS { get; set; }


        //public List<mstBAPSDocument> ListDoc { get; set; } 
        public List<int> detailIDs { get; set; }


    }
}
