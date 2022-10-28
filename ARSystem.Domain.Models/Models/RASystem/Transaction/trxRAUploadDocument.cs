
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxRAUploadDocument : BaseClass
	{
		public trxRAUploadDocument()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxRAUploadDocument(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public int mstRADocumentTypeID { get; set; }
		public int TransactionID { get; set; }
		public string FilePath { get; set; }
		public string FileName { get; set; }
		public string ContentType { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}