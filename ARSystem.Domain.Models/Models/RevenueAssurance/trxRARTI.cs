
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxRARTI : BaseClass
	{
		public trxRARTI()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxRARTI(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string Year { get; set; }
		public string CustomerID { get; set; }
		public string CompanyID { get; set; }
		public string Type { get; set; }
		public string Regional { get; set; }
		public string Area { get; set; }
		public string Currency { get; set; }
		public string FilePath { get; set; }
		public string FileName { get; set; }
        public string ContenType { get; set; }
        public string BAPSNumber { get; set; }
        public string PONumber { get; set; }
        public decimal? BAPSAmount { get; set; }
		public int? TotalSite { get; set; }
		public int? TotalBoq { get; set; }
		public int? TotalPO { get; set; }
		public decimal? TotalAmount { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
        public string SONumber { get; set; }
        public string RegionID { get; set; }
        public string RegionName { get; set; }
        public string Filter { get; set; }
        public int isRaw { get; set; }
	}
}