
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwMonitoringBapsDoneHeader : BaseClass
	{
		public vwMonitoringBapsDoneHeader()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwMonitoringBapsDoneHeader(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public string RowIndex { get; set; }
        public string Descrip { get; set; }
        public string DescripID { get; set; }
        public string TotalSite { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string Mei { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Agu { get; set; }
        public string Sep { get; set; }
        public string Okt { get; set; }
        public string Nov { get; set; }
        public string Des { get; set; }
    }

    public class MonitoringBapsDoneHeaderParam : BaseClass
    {
        public MonitoringBapsDoneHeaderParam()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public MonitoringBapsDoneHeaderParam(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string GroupBy { get; set; }
        public string BapsType { get; set; }
        public string CustomerID { get; set; }
        public string CompanyId { get; set; }
        public int? StipID { get; set; }
        public int? Year { get; set; }
        public int? RegionID { get; set; }
        public int? ProvinceID { get; set; }
        public int? ProductID { get; set; }
        public string PowerTypeID { get; set; }

        public string DataType { get; set; }

    }
}