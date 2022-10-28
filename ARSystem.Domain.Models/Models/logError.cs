
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class logError : BaseClass
    {
        public logError()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public logError(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public long ErrorId { get; set; }
        public string ErrorMessages { get; set; }
        public string Service { get; set; }
        public string Method { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}