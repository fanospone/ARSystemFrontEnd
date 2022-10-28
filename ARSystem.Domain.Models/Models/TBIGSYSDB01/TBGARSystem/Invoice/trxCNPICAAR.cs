
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxCNPICAAR : BaseClass
	{
		public trxCNPICAAR()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxCNPICAAR(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxCNPICAARID { get; set; }
		public int? mstPICATypeID { get; set; }
		public int? mstPICAMajorID { get; set; }
		public int? mstPICADetailID { get; set; }
		public int? trxCNInvoiceHeaderID { get; set; }
		public int? trxCNInvoiceHeaderRemainingAmountID { get; set; }
		public bool? NeedCN { get; set; }
		public string Remark { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? mstPICATypeIDSection { get; set; }
		public int? mstPICADetailIDSection { get; set; }
        public int? trxCNInvoiceNonRevenueID { get; set; }
    }
}