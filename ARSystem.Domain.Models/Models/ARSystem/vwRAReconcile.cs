using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.Models.ARSystem
{
    public class vwRAReconcile : BaseClass
    {
        public vwRAReconcile()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwRAReconcile(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public long RowIndex { get; set; }
        public long Id { get; set; }
        public string SONumber { get; set; }
        public string CustomerID { get; set; }
        public string Company { get; set; }
        public string CompanyName { get; set; }
        public int? RegionID { get; set; }
        public string RegionalName { get; set; }
        public int? ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public int? ResidenceID { get; set; }
        public string ResidenceName { get; set; }
        public int Year { get; set; }
        public int Term { get; set; }
        public int? Quartal { get; set; }
        public DateTime? StartInvoiceDate { get; set; }
        public DateTime? EndInvoiceDate { get; set; }
        public int InvoiceTypeId { get; set; }
        public string InvoiceTypeName { get; set; }
        public DateTime? StartBapsDate { get; set; }
        public DateTime? EndBapsDate { get; set; }
        public int StipSiro { get; set; }
        public string PONumber { get; set; }
        public DateTime? InitialPODate { get; set; }
        public decimal? BaseLeasePrice { get; set; }
        public decimal? ServicePrice { get; set; }
        public string BaseLeaseCurrency { get; set; }
        public string ServiceCurrency { get; set; }
        public string MLANumber { get; set; }
        public string TenantType { get; set; }
        public string CompanyInvoice { get; set; }
        public string CompanyInvoiceName { get; set; }
        public string CustomerInvoice { get; set; }
        public decimal InflationAmount { get; set; }
        public string InflationCurrency { get; set; }
        public decimal AdditionalAmount { get; set; }
        public string AdditionalCurrency { get; set; }
        public decimal DeductionAmount { get; set; }
        public string DeductionCurrency { get; set; }
        public decimal PenaltySlaAmount { get; set; }
        public string PenaltySlaCurrency { get; set; }
        public decimal? TotalPaymentRupiah { get; set; }
        public decimal? TotalPaymentDollar { get; set; }
        public DateTime? RFIDate { get; set; }
        public DateTime? BaufDate { get; set; }
        public string AuditUser { get; set; }
        public DateTime? AuditDate { get; set; }
        public string BAPSNumber { get; set; }
        public string Address { get; set; }
        public string AreaId { get; set; }
        public string BapsType { get; set; }
        public int StatusRecon { get; set; }
        public string Currency { get; set; }
        public DateTime? ReconcileDate { get; set; }
        public int mstBapsID { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public int? MaxTerm { get; set; }
        public string BOQNumber { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ContenType { get; set; }
        public DateTime? BAPSSignDate { get; set; }
        public int ReconYear { get; set; }
        public string AreaID { get; set; }
        public int? TotalTenant { get; set; }
        public decimal? TotalAmount { get; set; }
        public string BatchID { get; set; }
        public string Remarks { get; set; }
        public decimal? POAmount { get; set; }
        public DateTime? PODate { get; set; }
        public int? CustomerRegionID { get; set; }
        public string CustomerRegionName { get; set; }
        public string RegionName { get; set; }
        public string CreatedBy { get; set; }
        public int ProductID { get; set; }
        public decimal? DropFODistance { get; set; }
    }
}
