
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class trxInvoiceMatchingAR : BaseClass
	{
		public trxInvoiceMatchingAR()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxInvoiceMatchingAR(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long ID { get; set; }
		public string RekeningKoranID { get; set; }
		public string DocumentPayment { get; set; }
		public string CompanyCode { get; set; }
		public string TanggalUangMasuk { get; set; }
		public DateTime InsertDate { get; set; }
		public string InsertTime { get; set; }
		public int? trxInvoiceHeaderID { get; set; }
		public int? trxInvoiceNonRevenueID { get; set; }
		public int? BatchNumber { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? Status { get; set; }
        public int? IsOtherRevision { get; set; }
        public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
	}
}