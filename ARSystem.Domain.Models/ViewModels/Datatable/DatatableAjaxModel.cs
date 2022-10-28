using System.Collections.Generic;

namespace ARSystem.Domain.Models.ViewModels.Datatable
{
    public class DatatableAjaxModel
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<DatatableColumn> columns { get; set; }
        public DatatableSearch search { get; set; }
        public List<DatatableOrder> order { get; set; }
    }
}