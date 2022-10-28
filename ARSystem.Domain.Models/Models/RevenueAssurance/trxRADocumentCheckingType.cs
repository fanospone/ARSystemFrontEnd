
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxRADocumentCheckingType : BaseClass
	{
		public trxRADocumentCheckingType()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxRADocumentCheckingType(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int Id { get; set; }
		public string SoNumber { get; set; }
		public int DocumentId { get; set; }
		public int RAActivity { get; set; }
		public byte CheckType { get; set; }
		public string CheckTypeName { get; set; }
		public string Remark { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
	}
}