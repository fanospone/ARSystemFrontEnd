
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxRAPurchaseOrder : BaseClass
	{
		public trxRAPurchaseOrder()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxRAPurchaseOrder(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long ID { get; set; }
		public string PONumber { get; set; }
		public DateTime? PODate { get; set; }
		public DateTime? POReceiveDate { get; set; }
		public DateTime? StartPeriod { get; set; }
		public DateTime? EndPeriod { get; set; }
		public decimal? POAmount { get; set; }
		public int? mstRAActivityID { get; set; }
		public string Remarks { get; set; }
		public string mstRABoqID { get; set; }
		public string POType { get; set; }
		public string Currency { get; set; }
		public string CompanyID { get; set; }
		public string CustomerID { get; set; }
		public string Regional { get; set; }
		public int? TotalTenant { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdateDate { get; set; }
		public string UpdateBy { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
    }
}