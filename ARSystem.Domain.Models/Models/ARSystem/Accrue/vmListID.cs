using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmListID : BaseClass
    {
        public vmListID()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmListID(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public long ID { get; set; }
        public string Type { get; set; }
    }
}
