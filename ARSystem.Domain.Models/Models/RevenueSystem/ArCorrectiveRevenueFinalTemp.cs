
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class ArCorrectiveRevenueFinalTemp : BaseClass
	{
		public ArCorrectiveRevenueFinalTemp()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public ArCorrectiveRevenueFinalTemp(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public Int64 RowIndex { get; set; }
        public int Id { get; set; }
		public string SoNumber { get; set; }
		public string RemarkAdjusment { get; set; }
		public byte MonthPeriod { get; set; }
		public short YearPeriod { get; set; }
		public string SiteId { get; set; }
		public string SiteName { get; set; }
		public decimal TotalAdjusment { get; set; }
        public decimal AdjPPSNetRevenue { get; set; }
        public decimal NetRevenue { get; set; }
        public decimal AdjPPSNormalRevenue { get; set; }
        public decimal NormalRevenue { get; set; }
		public decimal BalanceAccrue { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
	}
}