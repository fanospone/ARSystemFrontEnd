
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwBAUKRejectDocumentDetail : BaseClass
	{
		public vwBAUKRejectDocumentDetail()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwBAUKRejectDocumentDetail(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string CustomerID { get; set; }
		public string CompanyID { get; set; }
		public int? STIPID { get; set; }
		public int Id { get; set; }
		public string SoNumber { get; set; }
		public string DocumentName { get; set; }
		public byte CheckType { get; set; }
		public string RejectReason { get; set; }
		public int? Month { get; set; }
		public int? Year { get; set; }
		public DateTime ActivityDate { get; set; }
	}
}