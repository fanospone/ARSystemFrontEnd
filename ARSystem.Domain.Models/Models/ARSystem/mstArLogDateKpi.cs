
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class mstArLogDateKpi : BaseClass
	{
		public mstArLogDateKpi()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstArLogDateKpi(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int mstArLogDateKpiId { get; set; }
		public DateTime LogDate { get; set; }
		public string LogDateName { get; set; }
		public int LogWeek { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}