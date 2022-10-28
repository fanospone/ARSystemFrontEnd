
namespace ARSystem.Domain.Models.ViewModels.Datatable
{
    public class DatatableColumn
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public DatatableSearch search { get; set; }
    }
}