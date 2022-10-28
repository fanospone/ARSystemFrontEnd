
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class WfHeaders_View : BaseClass
	{
		public WfHeaders_View()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public WfHeaders_View(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
		public int AppHeaderID { get; set; }
		public int JobId { get; set; }
		public string Job { get; set; }
		public string Code { get; set; }
		public string URL { get; set; }
		public string PRL { get; set; }
		public string Initiator { get; set; }
		public string InitiatorName { get; set; }
		public string InitiatorLocation { get; set; }
		public string InitiatorJobTitle { get; set; }
		public string PotentialActivityOwner { get; set; }
		public string ActivityOwner { get; set; }
		public string ActivityOwnerName { get; set; }
		public string Label { get; set; }
		public int ActivityID { get; set; }
		public string Activity { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime LastModifiedDate { get; set; }
		public string LastModifiedBy { get; set; }
		public string LastModifiedByName { get; set; }
		public string Status { get; set; }
		public string StatusDescription { get; set; }
		public string NextFlag { get; set; }
		public string RequestNo { get; set; }
		public string Summary { get; set; }
		public bool IsApproved { get; set; }
		public string DeviationNumber { get; set; }
		public short? Version { get; set; }
        public int PrevActivityID { get; set; }
        public string PrevActivityLabel { get; set; }
    }
}