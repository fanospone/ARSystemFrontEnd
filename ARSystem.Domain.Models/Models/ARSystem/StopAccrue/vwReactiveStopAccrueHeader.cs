
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
	public class vwReactiveStopAccrueHeader : BaseClass
	{
		public vwReactiveStopAccrueHeader()
		{
			this.ErrorType = 0;
			this.ErrorMessage = null;
		}
		public vwReactiveStopAccrueHeader(int errorType, string errorMessage)
		{
			this.ErrorType = errorType;
			this.ErrorMessage = errorMessage;
		}
        public Int64 RowIndex { get; set; }
        public long ID { get; set; }
		public string RequestNumber { get; set; }
		public string InitiatorName { get; set; }
		public string Initiator { get; set; }
		public int? RequestTypeID { get; set; }
		public DateTime? StartEffectiveDate { get; set; }
		public DateTime? EndEffectiveDate { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string Status { get; set; }
		public string AccrueType { get; set; }
		public int? AppHeaderID { get; set; }
		public int? PrevAppHeaderID { get; set; }
		public int IsReHoldReady { get; set; }
		public bool? IsReHold { get; set; }
		public string FileName { get; set; }
        public string UserRole { get; set; }
        public string ActivityName { get; set; }
        public string ActivityLabel { get; set; }
        public string PrevActivityName { get; set; }
        public string PrevActivityLabel { get; set; }
        public int? ActivityID { get; set; }
        public string Activity { get; set; }
        public int? PrevActivityID { get; set; }
        public string ActivityOwner { get; set; }
        public string ActivityOwnerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ProcessOID { get; set; }
        public string ResourceRequesterId { get; set; }
    }
}