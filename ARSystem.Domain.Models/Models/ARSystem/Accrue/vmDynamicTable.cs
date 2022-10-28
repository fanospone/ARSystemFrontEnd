
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmDynamicTable : BaseClass
    {
        public vmDynamicTable()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmDynamicTable(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string thead { get; set; }
        public string tbody { get; set; }
    }
}
