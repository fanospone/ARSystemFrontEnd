using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.Models.RevenueAssurance
{
    public class vmNewBapsData : BaseClass
    {
        //copy from backend service
        public vmNewBapsData()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmNewBapsData(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public Int64 RowIndex { get; set; }
        public string SoNumber { get; set; }
        public string TenantID { get; set; }
        public int? ColoTypeID { get; set; }
        public string CompanyID { get; set; }
        public string CustomerID { get; set; }
        public string CompanyInvoice { get; set; }
        public string CustomerInvoice { get; set; }
        public string SiteID { get; set; }
        public string CustomerSiteID { get; set; }
        public string CustomerSiteName { get; set; }
        public string SiteAddress { get; set; }
        public int? ResidenceID { get; set; }
        public string SiteName { get; set; }
        public string Product { get; set; }
        public int? ProductID { get; set; }
        public string MLANumber { get; set; }
        public DateTime? MLADate { get; set; }
        public int? SIRO { get; set; }
        public string STIPNumber { get; set; }
        public string CompanyName { get; set; }
        public int? RegionID { get; set; }
        public string RegionName { get; set; }
        public int? ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public string AreaID { get; set; }
        public string AreaName { get; set; }
        public string ResidenceName { get; set; }
        public string TowerTypeID { get; set; }
        public string SiteTypeID { get; set; }
        public int? StipID { get; set; }
        public decimal? OmPrice { get; set; }
        public decimal? PriceAmount { get; set; }
        public decimal? SiteHeight { get; set; }
        public int? LeasePriod { get; set; }
        public bool CheckListDoc { get; set; }
        public bool BapsValidation { get; set; }
        public bool BapsPrint { get; set; }
        public bool BapsInput { get; set; }
        public string StipCode { get; set; }
        public string BaukNumber { get; set; }
        public DateTime? BaukDate { get; set; }
        public decimal? PoAmount { get; set; }
        public DateTime? PoDate { get; set; }
        public bool BapsPrintInput { get; set; }
        public int ActivityID { get; set; }
        public string Label { get; set; }
        public string PoType { get; set; }
        public int? PowerTypeID { get; set; }
        public int? ID { get; set; }
        public string PONumber { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string BAPSNumber { get; set; }
        public int trxBapsRejectId { get; set; }
        public int PowerType { get; set; }
        public DateTime? BapsDate { get; set; }
        public int InvoiceType { get; set; }

        public DateTime? SplitStartDate { get; set; }
        public DateTime? SplitEndDate { get; set; }
        public decimal? SplitAmount { get; set; }

        public DateTime? SplitStartDate2 { get; set; }
        public DateTime? SplitEndDate2 { get; set; }
        public decimal? SplitAmount2 { get; set; }
        public DateTime? POInputDate { get; set; }

        public decimal? BaseLeasePrice { get; set; }
        public int? ProrateFormulation { get; set; }
        public decimal? InstallationAmount { get; set; }
        public decimal? DropFODistance { get; set; }
    }
}
