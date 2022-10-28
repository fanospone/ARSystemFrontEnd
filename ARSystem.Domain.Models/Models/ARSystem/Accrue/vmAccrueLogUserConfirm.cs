using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmAccrueLogUserConfirm : BaseClass
    {
        public vmAccrueLogUserConfirm()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmAccrueLogUserConfirm(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string SONumber { get; set; }        
        public string Remarks { get; set; }
        public string SOW { get; set; }
        public string PICA { get; set; }
        public string PICADetail { get; set; }
        public string RootCause { get; set; }
        public string TargetUser { get; set; }
    }
}
