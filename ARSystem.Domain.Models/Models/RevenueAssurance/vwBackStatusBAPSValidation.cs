
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwBackStatusBAPSValidation : BaseClass
	{
		public vwBackStatusBAPSValidation()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwBackStatusBAPSValidation(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long? RowIndex { get; set; }
		public string SoNumber { get; set; }
		public string BAPSNumber { get; set; }
		public DateTime? BapsDoneDate { get; set; }
		public DateTime? StartBapsDate { get; set; }
		public DateTime? EndBapsDate { get; set; }
		public int StipSiro { get; set; }
		public string BapsType { get; set; }
		public int? mstRAActivityID { get; set; }
		public string CustomerSiteName { get; set; }
		public string Company { get; set; }
		public string CompanyInvoiceId { get; set; }
		public bool? CheckListDoc { get; set; }
		public bool? BapsValidation { get; set; }
		public bool? BapsPrint { get; set; }
		public bool? BapsInput { get; set; }
		public string ActivityName { get; set; }
	}
}