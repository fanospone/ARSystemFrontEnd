
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxStopAccrueDetailFile : BaseClass
	{
		public trxStopAccrueDetailFile()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxStopAccrueDetailFile(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public long ID { get; set; }
		public long? trxStopAccrueDetailID { get; set; }
		public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Remarks { get; set; }
        public DateTime? CreatedDate { get; set; }
		
	}
}