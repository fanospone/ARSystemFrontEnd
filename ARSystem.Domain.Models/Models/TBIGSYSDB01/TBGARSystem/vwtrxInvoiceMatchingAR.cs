
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwtrxInvoiceMatchingAR : BaseClass
	{
		public vwtrxInvoiceMatchingAR()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwtrxInvoiceMatchingAR(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public Int64 RowNumber { get; set; }
        public int trxMatchingARID { get; set; }
        public string DocumentHeaderText { get; set; }
        public string InvoiceNumber { get; set; }
		public DateTime? InvPaidDate { get; set; }
        public string RekeningKoranid { get; set; }
        public string Documentpayment { get; set; }
		public string InsertDate { get; set; }
		public string InsertTime { get; set; }
		public string CompanyCodeInvoice { get; set; }
		public string Customer { get; set; }
        public string COAOthers { get; set; }
		public string CompanyCodePayment { get; set; }
		public string Tanggaluangmasuk { get; set; }
		public string Currency { get; set; }
		public decimal Totalpayment { get; set; }
		public decimal NilaiInvoice { get; set; }
		public decimal PaidAmount { get; set; }
		public decimal PPHAmount { get; set; }
		public decimal Rounding { get; set; }
		public decimal WAPU { get; set; }
		public decimal RTGS { get; set; }
		public decimal Penalty { get; set; }
		public string PPNExpired { get; set; }
        public string Status { get; set; }
        public string ResponseMessage { get; set; }
        public string PaymentType { get; set; }
		public string Keterangan { get; set; }
        public int IsOtherRevision { get; set; }

    }
}