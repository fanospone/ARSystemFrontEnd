using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostPICARASystem : DatatableAjaxModel
    {
        public PostPICARASystem()
        {
            DashboardPICARASystem = new vwRADashboardPICA();
        }

        public virtual vwRADashboardPICA DashboardPICARASystem { get; set; }
    }
}