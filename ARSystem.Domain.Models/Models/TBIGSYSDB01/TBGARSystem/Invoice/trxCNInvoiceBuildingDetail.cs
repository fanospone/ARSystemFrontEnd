
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxCNInvoiceBuildingDetail : BaseClass
	{
		public trxCNInvoiceBuildingDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxCNInvoiceBuildingDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxCNInvoiceBuildingDetailID { get; set; }
		public int trxCNInvoiceHeaderID { get; set; }
		public DateTime? StartPeriod { get; set; }
		public DateTime? EndPeriod { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public decimal? Discount { get; set; }
		public int CustomerID { get; set; }
	}
}