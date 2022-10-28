using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostValidasiInvoiceManual : DatatableAjaxModel
    {
        public int mstValidasiManualID { get; set; }
        public string FieldName { get; set; }
        public int isMandatory { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int isActive { get; set; }
        public string Column_Name { get; set; }
    }
}