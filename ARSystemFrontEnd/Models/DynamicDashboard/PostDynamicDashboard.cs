using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARSystem.Domain.Models;

namespace ARSystemFrontEnd.Models
{
    public class PostDynamicDashboard : DatatableAjaxModel
    {
        public mstDashboardTemplate dashboardTemplate = new mstDashboardTemplate();

        public string TemplateName { get; set; }
        public string RendererName { get; set; }
        public string AggregatorName { get; set; }
        public int DataSourceID { get; set; }
        public string ParamJSON { get; set; }
     
    }

    public class PostDataSourceDashboard : DatatableAjaxModel
    {
        public int ID { get; set; }
        public string[]  RoleID { get; set; }
        public string[] ParamFilter { get; set; }
        public string[] ShowColumn { get; set; }
        public string ViewName { get; set; }
        public string Schema { get; set; }
        public string  DataSourceName { get; set; }
        public string DatabaseName { get; set; }
    }
}