using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class trxReconcileID : BaseClass
    {
        public trxReconcileID()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public trxReconcileID(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public long ID { get; set; }
        public int mstRAActivity { get; set; }
    }
}
