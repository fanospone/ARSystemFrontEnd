using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystemFrontEnd.Helper;
using ARSystem.Service.ARSystem;
using ARSystem.Domain.Models.HTBGDWH01.TBGARSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/HistoryCNInvoiceARData")]
    public class ApiHistoryCNInvoiceARDataController : ApiController
    {
        private readonly HistoryCNInvoiceService _historyCNService;

        public ApiHistoryCNInvoiceARDataController()
        {
            var token = UserManager.User.UserToken;
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(token);
            _historyCNService = new HistoryCNInvoiceService();
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetHistoryCNInvoiceARDataDataToGrid(PostTrxHistoryCNInvoiceARDataView post)
        {
            try
            {
                var token = UserManager.User.UserToken;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(token);

                int intTotalRecord = 0;
                
                intTotalRecord = _historyCNService.GetHistoryCNInvoiceARDataCount(token, userCredential, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.strOperator, post.InvNo, post.TaxNo, post.CNStatus, post.InvoiceTypeId, post.ProccessType, post.ReplacementStatus, post.ReplaceDate, post.ReplaceInvoice);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                var listData = _historyCNService.GetHistoryCNInvoiceARDataToList(token, userCredential, post.strStartPeriod, post.strEndPeriod, post.strCompanyId, post.strOperator, post.InvNo, post.TaxNo, post.CNStatus, post.InvoiceTypeId, post.ProccessType, post.ReplacementStatus, post.ReplaceDate, post.ReplaceInvoice, strOrderBy, post.start, post.length).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = listData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("InvHeader/{OperatorID}")]
        public IHttpActionResult GetInvHeaderToList(string OperatorID)
        {
            try
            {
                var token = UserManager.User.UserToken;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(token);

                var invHeader = _historyCNService.GetInvHeaderToList(token, userCredential, OperatorID, "InvNo ASC").ToList();

                return Ok(invHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ManageReplacement")]
        public IHttpActionResult ManageReplacement(List<idxFINInvoiceReplacement> invoiceReplacement)
        {
            try
            {
                var token = UserManager.User.UserToken;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(token);
                var cnInvoiceNo = invoiceReplacement.FirstOrDefault().CNInvoiceNo;
                invoiceReplacement = _historyCNService.ManageReplacement(token, userCredential, invoiceReplacement, cnInvoiceNo);
                var ccc = Ok(invoiceReplacement);
                return Ok(invoiceReplacement);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ValidateReplacement")]
        public IHttpActionResult ValidateReplacement(List<idxFINInvoiceReplacement> list)
        {
            try
            {
                var token = UserManager.User.UserToken;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(token);
                var isNotValid = _historyCNService.GetValidate(token, userCredential, list);
                return Ok(isNotValid);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("MappedReplacement/{CNInvoiceNo}")]
        public IHttpActionResult MappedReplacement(string CNInvoiceNo)
        {
            try
            {
                var token = UserManager.User.UserToken;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(token);
                var CNInvoiceNos = CNInvoiceNo.Replace('-', '/');
                var invHeader = _historyCNService.GetMappedReplacementList(token, userCredential, CNInvoiceNos, "InvoiceNo ASC").ToList();

                return Ok(invHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}