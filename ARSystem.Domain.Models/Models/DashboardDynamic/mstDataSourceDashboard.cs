
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstDataSourceDashboard : BaseClass
	{
		public mstDataSourceDashboard()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstDataSourceDashboard(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public Int64 RowIndex { get; set; }
        public int ID { get; set; }
		public string RoleID { get; set; }
        public string Role { get; set; }
        public string DataSourceName { get; set; }
		public string DBContext { get; set; }
        public string DBSchema { get; set; }
        public string ViewName { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
        public string ParamFilter { get; set; }
        public string ShowColumn { get; set; }
    }
}