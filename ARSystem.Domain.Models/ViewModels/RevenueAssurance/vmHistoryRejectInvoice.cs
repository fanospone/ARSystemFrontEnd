using ARSystem.Domain.Models.ViewModels.Datatable;
using System.Collections.Generic;

namespace ARSystem.Domain.Models.ViewModels
{
    public class vmHistoryRejectInvoice : DatatableAjaxModel
    {
        public string vReconcileType { get; set; }
        public string vPowerType { get; set; }
        public string vDepartmentCode { get; set; }
        public List<string> vSONumber { get; set; }
        public int vYear { get; set; }
        public int vMonth { get; set; }
    }
}
