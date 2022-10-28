using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class mstProvince : BaseClass
    {
        public mstProvince()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public mstProvince(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
    }
}
