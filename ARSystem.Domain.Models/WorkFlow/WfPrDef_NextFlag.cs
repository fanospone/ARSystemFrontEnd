
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class WfPrDef_NextFlag : BaseClass
	{
		public WfPrDef_NextFlag()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public WfPrDef_NextFlag(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int FlagID { get; set; }
		public string NextFlag { get; set; }
		public int? FromActivityID { get; set; }
		public int? ToActivityID { get; set; }
		public string SortOrder { get; set; }
	}
}