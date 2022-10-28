using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class mstARSystemConstants : BaseClass
    {
        public mstARSystemConstants()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public mstARSystemConstants(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

    }
}
