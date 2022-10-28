using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.ViewModels.Datatable
{
    public class Datatable<T> : BaseClass
    {
        public Datatable()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public Datatable(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int Count { get; set; }
        public List<T> List { get; set; }
    }
}
