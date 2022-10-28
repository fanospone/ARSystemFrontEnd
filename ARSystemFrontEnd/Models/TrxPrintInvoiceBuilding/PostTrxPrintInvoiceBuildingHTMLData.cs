using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxPrintInvoiceBuildingHTMLData : DatatableAjaxModel
    {
        public string DT_RowId { get; set; }
        public int trxInvoiceHeaderID { get; set; }
        public string InvNo { get; set; }
        public string InvTemp { get; set; }
        public string InvPrintDate { get; set; }
        public string PostedBy { get; set; }
        public DateTime? PostedDate { get; set; }
        public decimal? InvSumADPP { get; set; }
        public decimal? InvTotalAmount { get; set; }
        public decimal? InvTotalAPPN { get; set; }
        public decimal? InvTotalAPPH { get; set; }
        public decimal? Discount { get; set; }
        public decimal? InvTotalPenalty { get; set; }
        public string Currency { get; set; }
        public string TaxInvoiceNo { get; set; }
        public string Remark { get; set; }
        public DateTime? VerificationDate { get; set; }
        public string VerificationStatus { get; set; }
        public string VerifiedBy { get; set; }
        public string InvoiceTypeId { get; set; }
        public string Term { get; set; }
        public string Company { get; set; }
        public string CompanyType { get; set; }
        public decimal? Area { get; set; }
        public decimal? MeterPrice { get; set; }
        public decimal? MonthlyPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? mstInvoiceStatusId { get; set; }
        public string InvPaidStatus { get; set; }
        public string Status { get; set; }
        public int PrintCount { get; set; }
        public string PrintStatus { get; set; }
        public string BillingAddress { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string LegalAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string BankName { get; set; }
        public string AccNo { get; set; }
        public string CompanyTBG { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string InvSubject { get; set; }
        public string Section { get; set; }
        public string ProvinceName { get; set; }
        public string RegencyName { get; set; }
        public string InvCompanyId { get; set; }
        public string CustomerCode { get; set; }
        public int CustomerID { get; set; }
        public string Terbilang { get; set; }
        public string InvRemarksPrint { get; set; }
        public string InvStatus { get; set; }
        public DateTime? StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
        public string ChecklistStatus { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsPPHFinal { get; set; }
        public string PPHType { get; set; }
        public string ReprintString { get; set; }

        public string PPHValue { get; set; }

    }
}