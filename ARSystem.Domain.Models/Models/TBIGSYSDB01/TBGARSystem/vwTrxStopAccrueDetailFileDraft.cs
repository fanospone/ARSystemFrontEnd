
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class vwTrxStopAccrueDetailFileDraft : BaseClass
	{
		public vwTrxStopAccrueDetailFileDraft()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwTrxStopAccrueDetailFileDraft(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public long? trxStopAccrueDetailID { get; set; }
		public string FilePath { get; set; }
		public string FileName { get; set; }
		public string Remarks { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}