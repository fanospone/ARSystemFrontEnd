
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxPICAARData : BaseClass
	{
		public trxPICAARData()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxPICAARData(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxPICAARDataID { get; set; }
		public int? mstPICATypeID { get; set; }
		public int? mstPICADetailID { get; set; }
		public int? trxInvoiceHeaderID { get; set; }
		public int? trxInvoiceHeaderRemainingAmountID { get; set; }
		public string Remark { get; set; }
		public int? mstInvoiceCategoryId { get; set; }
		public int? mstInvoiceStatusId { get; set; }
		public int? mstInvoiceState { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
        public int? trxInvoiceNonRevenueID { get; set; }
	}
}