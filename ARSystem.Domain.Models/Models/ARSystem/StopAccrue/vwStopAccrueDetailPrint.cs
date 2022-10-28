
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwStopAccrueDetailPrint : BaseClass
	{
		public vwStopAccrueDetailPrint()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwStopAccrueDetailPrint(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int? countTenant { get; set; }
		public string DetailCase { get; set; }
		public int? InternalCase { get; set; }
		public int? ExternalCase { get; set; }
		public string CustomerName { get; set; }
		public long? TrxStopAccrueHeaderID { get; set; }
        public decimal? TtlRevenue { get; set; }

    }
}