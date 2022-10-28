
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class trxSSRDropFO : BaseClass
	{
		public trxSSRDropFO()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public trxSSRDropFO(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int trxSSRDropFOID { get; set; }
		public string SONumber { get; set; }
		public string AddressMMP { get; set; }
		public decimal? LatitudeMMP { get; set; }
		public decimal? LongitudeMMP { get; set; }
		public int? Azimuth { get; set; }
		public decimal? Distance { get; set; }
		public bool? IsSubmit { get; set; }
		public DateTime? SSRDone { get; set; }
		public string Remark { get; set; }
		public string ApprovalRemark { get; set; }
		public string MMPID { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
	}
}