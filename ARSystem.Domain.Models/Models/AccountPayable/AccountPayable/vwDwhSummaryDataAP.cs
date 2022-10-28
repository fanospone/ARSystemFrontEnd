
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwDwhSummaryDataAP : BaseClass
	{
		public vwDwhSummaryDataAP()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwDwhSummaryDataAP(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string Product { get; set; }
		public int? Qty { get; set; }
		public decimal? CAPEX { get; set; }
		public decimal? OPEX { get; set; }
		public decimal? COR { get; set; }
		public decimal? HCPT { get; set; }
		public decimal? ISAT { get; set; }
		public decimal? SMART { get; set; }
		public decimal? TSEL { get; set; }
		public decimal? SMART8 { get; set; }
		public decimal? XL { get; set; }
		public decimal? DLL { get; set; }
		public decimal? M8 { get; set; }
	}
}