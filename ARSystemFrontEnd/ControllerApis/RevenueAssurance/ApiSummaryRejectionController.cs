using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;

using ARSystem.Service.RevenueAssurance;
using ARSystem.Domain.Models.ViewModels;
using ARSystem.Domain.Models;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.ControllerApis.RevenueAssurance
{
    [RoutePrefix("api/SummaryRejection")]
    public class ApiSummaryRejectionController : ApiController
    {
        private SummaryRejectionService _services;

        public ApiSummaryRejectionController()
        {
            _services = new SummaryRejectionService();
        }

        private void pDisposeService()
        {
            _services.Dispose();
        }

        [HttpPost, Route("summary")]
        public IHttpActionResult GetSummaryRejection(vmSummaryRejectionPost param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vmSummaryRejection data = new vmSummaryRejection(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    int intTotalRecord = 0;

                    List<vmSummaryRejection> result = _services.GetSummaryRejection(userCredential.UserID, param);
                    intTotalRecord = result.Count();

                    return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
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

        [HttpPost, Route("detail")]
        public IHttpActionResult GetSummaryRejectionDetail(vmSummaryRejectionPost param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vmSummaryRejection data = new vmSummaryRejection(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    int intTotalRecord = 0;

                    List<vmSummaryRejection> result = _services.GetSummaryRejectionDetail(userCredential.UserID, param);
                    intTotalRecord = result.Count();

                    return Ok(new { recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
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