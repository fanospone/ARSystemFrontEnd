
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwRATaskTodoCreateBAPS : BaseClass
	{
		public vwRATaskTodoCreateBAPS()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwRATaskTodoCreateBAPS(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public Int64 RowIndex { get; set; }
        public string SoNumber { get; set; }
		public string SiteID { get; set; }
		public string SiteName { get; set; }
		public string BAPSNumber { get; set; }
		public string CompanyID { get; set; }
		public string CustomerID { get; set; }
		public int? SIRO { get; set; }
		public string Product { get; set; }
		public string RegionName { get; set; }
		public string BapsType { get; set; }
		public string STIPCode { get; set; }
		public string ActivityName { get; set; }
	}
}