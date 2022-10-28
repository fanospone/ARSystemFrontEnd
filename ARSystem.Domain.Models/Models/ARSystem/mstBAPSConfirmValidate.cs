
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstBAPSConfirmValidate : BaseClass
	{
		public mstBAPSConfirmValidate()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstBAPSConfirmValidate(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public string TypeValidation { get; set; }
		public string OperatorID { get; set; }
		public int? MinValidateDay { get; set; }
		public int? MaxValidateDay { get; set; }
		public string StipSiroId { get; set; }
		public string InvoiceType { get; set; }
        public string BapsType { get; set; }
        public bool? IsActive { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}