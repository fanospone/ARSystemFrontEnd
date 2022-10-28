using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public abstract class BaseClass
    {
        public int ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
