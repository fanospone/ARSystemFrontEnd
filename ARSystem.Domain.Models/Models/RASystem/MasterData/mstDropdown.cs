using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class mstDropdown : BaseClass
    {
        public mstDropdown()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public mstDropdown(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
