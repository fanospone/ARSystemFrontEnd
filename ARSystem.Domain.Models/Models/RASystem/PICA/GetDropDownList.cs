using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class GetDropDownList : BaseClass
    {
        public GetDropDownList()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public GetDropDownList(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string ID { get; set; }
        public string Value { get; set; }
    }
}
