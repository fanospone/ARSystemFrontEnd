
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class mstRevSysInflationRate : BaseClass
	{
		public mstRevSysInflationRate()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstRevSysInflationRate(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int ID { get; set; }
		public int? Year { get; set; }
		public decimal? Percentage { get; set; }
		public string FileName { get; set; }
		public string ContentType { get; set; }
		public string FilePath { get; set; }
		public string FileExtension { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}