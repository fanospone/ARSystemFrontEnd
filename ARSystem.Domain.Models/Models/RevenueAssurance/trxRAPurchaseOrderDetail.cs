
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxRAPurchaseOrderDetail : BaseClass
	{
		public trxRAPurchaseOrderDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxRAPurchaseOrderDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public int PurchaseOrderID { get; set; }
		public long trxReconcileID { get; set; }
		public decimal? Amount { get; set; }
		public bool? IsSplit { get; set; }
		public string Type { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}