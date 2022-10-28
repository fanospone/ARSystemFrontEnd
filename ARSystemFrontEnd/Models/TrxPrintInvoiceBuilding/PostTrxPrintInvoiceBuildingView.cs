using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxPrintInvoiceBuildingView : DatatableAjaxModel
    {
        public string CompanyName { get; set; }
        public string InvoiceTypeId { get; set; }
        public DateTime? StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
        public int InvoiceStatusId { get; set; }
        public List<int> HeaderId { get; set; }
        public int PICAReprintID { get; set; }
        public string ReprintRemark { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Signature { get; set; }
        public string Subject { get; set; }
        public string RemarkCN { get; set; }
        public string InvNo { get; set; }
        public string ApprovalStatus { get; set; }
    }
}