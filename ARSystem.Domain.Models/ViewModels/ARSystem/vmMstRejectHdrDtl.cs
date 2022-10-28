using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmMstRejectHdrDtl : BaseClass
    {
        public vmMstRejectHdrDtl()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmMstRejectHdrDtl(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int mstPICATypeID { get; set; }
        public string Description { get; set; }
        public string Recipient { get; set; }
        public string CC { get; set; }
        public bool isActive { get; set; }
        public int mstUserGroupId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public List<mstPICADetail> DetailList { get; set; }
    }
}
