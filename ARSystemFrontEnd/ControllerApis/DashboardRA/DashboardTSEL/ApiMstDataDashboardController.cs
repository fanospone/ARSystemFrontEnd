using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Domain.Models;
using ARSystem.Service;
using ARSystemFrontEnd.Helper;

using System.Net.Http;


namespace ARSystemFrontEnd.ControllerApis.DashboardRA.DashboardTSEL
{
    [RoutePrefix("api/MstDataDashboard")]

    public class ApiMstDataDashboardController : ApiController
    {
        // GET: ApiMstDataDashboard
        [HttpGet, Route("CustomerInvoice")]
        public IHttpActionResult Index()
        {
            try
            {
                List<mstRACustomerInvoice> customer = new List<mstRACustomerInvoice>();

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

    }
}