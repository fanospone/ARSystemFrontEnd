using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Providers;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Service.RevenueAssurance;
using ARSystem.Domain.Models.ViewModels;
using ARSystem.Domain.Models.ViewModels.Datatable;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/HistoryRejectInvoice")]
    public class ApiHistoryRejectInvoiceController : ApiController
    {
        private HistoryRejectInvoiceService _services;

        public ApiHistoryRejectInvoiceController()
        {
            _services = new HistoryRejectInvoiceService();
        }

        private void pDisposeService()
        {
            _services.Dispose();
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetHistoryRejectInvoice(vmHistoryRejectInvoice param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vwHistoryRejectInvoice data = new vwHistoryRejectInvoice(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    if (param.vSONumber == null)
                    {
                        param.vSONumber = new List<string>();
                    }

                    Datatable<vwHistoryRejectInvoice> result = _services.GetDataHistoryRejectInvoice(userCredential.UserID, param, param.vSONumber.ToList());

                    return Ok(new { draw = param.draw, recordsTotal = result.Count, recordsFiltered = result.Count, data = result.List });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                pDisposeService();
            }
        }
    }
}