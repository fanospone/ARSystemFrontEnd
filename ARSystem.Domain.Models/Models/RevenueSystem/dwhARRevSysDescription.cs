
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class dwhARRevSysDescription : BaseClass
	{
		public dwhARRevSysDescription()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public dwhARRevSysDescription(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public Int64 RowIndex { get; set; }
        public string SoNumber { get; set; }
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public short StipSiro { get; set; }
        public string RegionName { get; set; }
        public short? RegionId { get; set; }
        public string UserNumber { get; set; }
        public string CustomerId { get; set; }
        public string SiteStatus { get; set; }
        public string ContractStatus { get; set; }
        public string DocumentProcess { get; set; }
        public string CompanyId { get; set; }
        public DateTime? RfiDate { get; set; }
        public DateTime? SLDDate { get; set; }
        public DateTime? BapsDate { get; set; }
        public DateTime? RentalStartDate { get; set; }
        public DateTime? RentalEndDate { get; set; }
        public string TenantType { get; set; }
        public decimal? RFInvoice { get; set; }
        public decimal? MFInvoice { get; set; }
        public decimal? PriceBaseOnML { get; set; }
        public decimal? PriceBaseOnInvoice { get; set; }
        public decimal? BalanceAccrued { get; set; }
        public int? LastInvoicePeriod { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public byte? AgingDays { get; set; }
        public int DataYear { get; set; }
        public string DataMonth { get; set; }
        public int DataMonthNumber { get; set; }
        public DateTime? StartSLDDate { get; set; }
        public DateTime? EndSLDDate { get; set; }
        public string DepartmentName { get; set; }
        public short? StipId { get; set; }
        public string StipCategory { get; set; }
        /*addtional*/
        public DateTime? RfiStartDate { get; set; }
        public DateTime? RfiEndDate { get; set; }
    }
}