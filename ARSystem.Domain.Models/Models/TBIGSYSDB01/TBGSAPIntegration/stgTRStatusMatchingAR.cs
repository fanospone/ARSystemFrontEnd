
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGSAPIntegration
{
	public class stgTRStatusMatchingAR : BaseClass
	{
		public stgTRStatusMatchingAR()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public stgTRStatusMatchingAR(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long ID { get; set; }
		public string SourceSys { get; set; }
		public string DocheaderText { get; set; }
		public string Entrydate { get; set; }
		public string Entrytime { get; set; }
		public string CompanycodeInvoice { get; set; }
		public string CustomerNumber { get; set; }
		public string CompanycodePayment { get; set; }
        public string RekeningKoranid { get; set; }
        public string DocumentPayment { get; set; }
		public string Tanggaluangmasuk { get; set; }
		public string Currency { get; set; }
		public decimal TotalPayment { get; set; }
		public decimal NilaiInvoice { get; set; }
		public decimal PaidAmount { get; set; }
		public decimal PphAmount { get; set; }
		public decimal Rounding { get; set; }
		public decimal Wapu { get; set; }
		public decimal Rtgs { get; set; }
		public decimal Penalty { get; set; }
		public string PpnExpired { get; set; }
		public string Status { get; set; }
		public int? ExecutionStatus { get; set; }
		public DateTime? ExecutionDate { get; set; }
		public int? ResponseCode { get; set; }
		public string ResponseMessage { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
        public string FlagOthers { get; set; }
        public string DocumentNumber { get; set; }
        public int? FiscalYear { get; set; }
        public int? trxMatchingARID { get; set; }
    }
}