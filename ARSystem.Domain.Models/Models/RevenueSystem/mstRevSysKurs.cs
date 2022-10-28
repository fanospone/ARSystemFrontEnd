
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class mstRevSysKurs : BaseClass
	{
		public mstRevSysKurs()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstRevSysKurs(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string Currency { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        //new added, used as key for update kurs
        public DateTime? UpdatedStartDate { get; set; }
        public DateTime? UpdatedEndDate { get; set; }

        public decimal Kurs { get; set; }
		public string FileName { get; set; }
		public string ContentType { get; set; }
		public string FilePath { get; set; }
		public string FileExtension { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
        public string Mode { get; set; }
    }
}