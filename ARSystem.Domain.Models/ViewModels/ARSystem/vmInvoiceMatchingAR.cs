using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGSAPIntegration;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using System.Collections.Generic;
using System;

namespace ARSystem.Domain.Models.ViewModels
{
    public class vmInvoiceMatchingAR : DatatableAjaxModel
    {
        public string vStartPaid { get; set; }
        public string vEndPaid { get; set; }
        public string vCompanyID { get; set; }
        public string vInvoiceNo { get; set; }
        public List<stgTRStatusMatchingAR> ListMatchingARCollection { get; set; }
        public string vDocumentType { get; set; }
        public string vCompanyCode { get; set; }
        public DateTime? vTglUangMasuk { get; set; }
        //public trxInvoiceMatchingAR InvoiceMatchingAR { get; set; }
        public int trxInvoiceMatchingARID { get; set; }
        public string vDocumentPayment { get; set; }
        public string vStatus { get; set; }
        public vwMatchingARLogDocumentPayment DocumentPaymentLog { get; set; }
    }
}
