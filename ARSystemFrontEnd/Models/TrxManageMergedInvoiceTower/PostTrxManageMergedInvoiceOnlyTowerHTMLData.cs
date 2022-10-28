using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxManageMergedInvoiceOnlyTowerHTMLData : DatatableAjaxModel
    {
        public int PrintCount { get; set; }
        public string PrintStatus { get; set; }
        public int trxInvoiceHeaderID { get; set; }
        public int? trxInvoiceHeaderParentId { get; set; }
        public string InvNo { get; set; }
        public decimal? InvSumADPP { get; set; }
        public decimal? InvTotalAPPN { get; set; }
        public decimal? InvTotalPenalty { get; set; }
        public decimal? Discount { get; set; }
        public decimal? InvTotalAmount { get; set; }
        public string InvCompanyId { get; set; }
        public string Company { get; set; }
        public string Currency { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string InvSubject { get; set; }
        public string Terbilang { get; set; }
        public string InvRemarksPrint { get; set; }
        public string InvStatus { get; set; }
        public string InvPrintDate { get; set; }
        public string BankName { get; set; }
        public string AccNo { get; set; }
        public string Section { get; set; }
        public string Address { get; set; }
        public string Address3 { get; set; }
        public string OperatorDesc { get; set; }
        public bool? IsPPHFinal { get; set; }
        public string PPHType { get; set; }
        public string InvAdditionalNote { get; set; }
        public string ReprintString { get; set; }
    }
}