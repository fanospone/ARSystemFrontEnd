
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class TrxRAAmountTarget : BaseClass
    {
        public TrxRAAmountTarget()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public TrxRAAmountTarget(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int ID { get; set; }
        public int ApprovalStatusID { get; set; }
        public string StatusName { get; set; }
        public string CustomerID { get; set; }
        public int Year { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
	}
}