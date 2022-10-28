
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem
{
	public class mstParameter : BaseClass
	{
		public mstParameter()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public mstParameter(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int MstParameterID { get; set; }
		public string ParameterKey { get; set; }
		public string ParameterValue { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? EffectiveDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string ParameterType { get; set; }
	}
}