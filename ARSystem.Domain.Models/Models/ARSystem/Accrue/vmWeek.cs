
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmWeek : BaseClass
    {
        public vmWeek()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmWeek(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int ID { get; set; }
        public string Week { get; set; }
    }
}
