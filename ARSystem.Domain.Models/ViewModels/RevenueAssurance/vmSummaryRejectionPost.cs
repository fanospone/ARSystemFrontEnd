
using ARSystem.Domain.Models.ViewModels.Datatable;

namespace ARSystem.Domain.Models.ViewModels
{
    public class vmSummaryRejectionPost : DatatableAjaxModel
    {
        public string vCompanyID { get; set; }
        public string vOperatorID { get; set; }
        public string vRTIPeriodStart { get; set; }
        public string vRTIPeriodEnd { get; set; }
        public string vGroupBy { get; set; }
        public string vCol { get; set; }
        public string vDepartmentCode { get; set; }
    }
}
