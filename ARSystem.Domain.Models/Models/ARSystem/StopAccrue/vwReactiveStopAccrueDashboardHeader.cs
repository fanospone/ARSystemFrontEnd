
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwReactiveStopAccrueDashboardHeader : BaseClass
	{
		public vwReactiveStopAccrueDashboardHeader()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwReactiveStopAccrueDashboardHeader(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public string RowIndex { get; set; }
        public long ID { get; set; }
        public int AppHeaderID { get; set; }
        public string RequestNumber { get; set; }
        public string DepartName { get; set; }
        public string FileName { get; set; }
        public string RequestStatus { get; set; }
        public int? SoNumberCount { get; set; }
        public decimal? SumRevenue { get; set; }
        public decimal? SumCapex { get; set; }
        public int? RequestTypeID { get; set; }
        public int? ActivityID { get; set; }
        public string Activity { get; set; }
        public string ReqeustStatus { get; set; }
        public DateTime? CraetedDate { get; set; }
        public DateTime? CraetedDate2 { get; set; }
        public string AccrueType { get; set; }
        public bool IsReHold { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}