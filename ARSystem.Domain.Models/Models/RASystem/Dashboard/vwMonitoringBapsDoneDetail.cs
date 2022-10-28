
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwMonitoringBapsDoneDetail : BaseClass
	{
		public vwMonitoringBapsDoneDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwMonitoringBapsDoneDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public Int64 RowIndex { get; set; }
        public string SoNumber { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string CustomerSiteID { get; set; }
		public string CustomerSiteName { get; set; }
		public string TenantType { get; set; }
		public int StipSiro { get; set; }
		public string StipCategory { get; set; }
		public string CompanyInvoiceId { get; set; }
		public string Company { get; set; }
		public string CustomerId { get; set; }
		public string CompanyName { get; set; }
		public string RegionName { get; set; }
		public string ProvinceName { get; set; }
		public string ResidenceName { get; set; }
        public string BAPSNumber { get; set; }
        public DateTime? RFIDate { get; set; }
		public DateTime? BAUKDone { get; set; }
		
		public DateTime? BapsDate { get; set; }
		public DateTime? BapsDoneDate { get; set; }
        public DateTime? PODone { get; set; }
        public decimal? BapsAmount { get; set; }
		public int? PowerTypeID { get; set; }
		public int? mstBapsTypeID { get; set; }
		public int? RegionID { get; set; }
		public int? ResidenceID { get; set; }
		public int? ProvinceID { get; set; }
		public int? STIPID { get; set; }
		public int? ProductID { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public string BapsType { get; set; }

        //Added by ASE
        public string PONumber { get; set; }
        public DateTime? BAPSReceiveDate { get; set; }
        public DateTime? BAPSConfirmDate { get; set; }
        public DateTime? StartLeasedDate { get; set; }
        public DateTime? EndLeasedDate { get; set; }
        public DateTime? StartInvoiceDate { get; set; }
        public DateTime? EndInvoiceDate { get; set; }
        public DateTime? CreateInvoiceDate { get; set; }
        public DateTime? PostingInvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string BAPSDocumentFilePath { get; set; }
        public string BAPSDocumentFileName { get; set; }
        public string BAPSDocumentContentType { get; set; }
        public string BAUKDocument { get; set; }
    }
}