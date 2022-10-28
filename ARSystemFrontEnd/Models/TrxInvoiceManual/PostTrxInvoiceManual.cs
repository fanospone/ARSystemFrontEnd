using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxInvoiceManual : DatatableAjaxModel
    {
        public List<ARSystemService.trxInvoiceManualTemp> ListTrxInvoiceManual { get; set; }
        public string[] ListID { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public long trxInvoiceManualTempID { get; set; }
        public string SONumber { get; set; }
        public string SiteIDOpr { get; set; }
        public string SiteNameOpr { get; set; }
        public string InitialPONumber { get; set; }
        public DateTime? InvoiceStartDate { get; set; }
        public DateTime? InvoiceEndDate { get; set; }
        public decimal? BaseLeasePrice { get; set; }
        public decimal? OMPrice { get; set; }
        public decimal? StipSiro { get; set; }
        public string CompanyID { get; set; }
        public string PriceCurrency { get; set; }
        public string LossPNN { get; set; }
        public string BapsNO { get; set; }
        public string BapsType { get; set; }
        public string OperatorName { get; set; }
        public DateTime? StartLeaseDate { get; set; }
        public DateTime? EndLeaseDate { get; set; }
        public string InvoiceTerm { get; set; }
        public string MLANumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Status { get; set; }

        public List<int> ListId { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public int MstRejectDtlId { get; set; }
        public int isReceive { get; set; }
    }
}