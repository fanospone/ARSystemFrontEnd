
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwISPBAPSNewInformation : BaseClass
	{
		public vwISPBAPSNewInformation()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwISPBAPSNewInformation(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public string SoNumber { get; set; }
		public string BAPSNumber { get; set; }
		public DateTime? BapsDate { get; set; }
		public DateTime? EffectiveBapsDate { get; set; }
		public DateTime? StartEffectiveDate { get; set; }
		public DateTime? EndEffectiveDate { get; set; }
		public decimal? BaseLeasePrice { get; set; }
		public decimal? ServicePrice { get; set; }
		public int StipSiro { get; set; }
	}
}