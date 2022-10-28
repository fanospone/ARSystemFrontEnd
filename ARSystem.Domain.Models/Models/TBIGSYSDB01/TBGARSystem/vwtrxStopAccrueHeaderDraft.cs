
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vwtrxStopAccrueHeaderDraft : BaseClass
    {
        public vwtrxStopAccrueHeaderDraft()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwtrxStopAccrueHeaderDraft(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public Int64 RowIndex { get; set; }
        public long ID { get; set; }
        public int AppHeaderID { get; set; }
        public string RequestNumber { get; set; }
        public string Initiator { get; set; }
        public string ActivityOwner { get; set; }
        public string Status { get; set; }
        public int RoleID { get; set; }
        public int? RequestTypeID { get; set; }
        public string RequestType { get; set; }
        public DateTime? StartEffectiveDate { get; set; }
        public DateTime? EndEffectiveDate { get; set; }
        public int ActivityID { get; set; }
        public int PrevActivityID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public string Remarks { get; set; }
        public string NextFlag { get; set; }
        public string ActivityLabel { get; set; }
        public string PrevActivityLabel { get; set; }
        public int PrevAppHeaderID { get; set; }

        public string FileName { get; set; }
        public string DepartInitial { get; set; }
        public string DeptCode { get; set; }

    }
}