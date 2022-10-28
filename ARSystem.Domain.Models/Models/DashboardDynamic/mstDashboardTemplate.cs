
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstDashboardTemplate : BaseClass
	{
		public mstDashboardTemplate()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstDashboardTemplate(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;

        }
        public Int64 RowIndex { get; set; }
        public int ID { get; set; }
		public int RoleID { get; set; }
		public string TemplateName { get; set; }
		public string RendererName { get; set; }
		public string AggregatorName { get; set; }
		public string AggregatorVals { get; set; }
		public string PivotColumn { get; set; }
		public string PivotRow { get; set; }
		public int? DataSourceID { get; set; }
        public string DataSource { get; set; }
        public string TemplateDesc { get; set; }
		public string JSONConfig { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }
		public string CreatedBy { get; set; }
		public string UpdatedBy { get; set; }
        public string ParamFilter { get; set; }
        public string Filtering { get; set; }
        public string ShowColumn { get; set; }
    }
}