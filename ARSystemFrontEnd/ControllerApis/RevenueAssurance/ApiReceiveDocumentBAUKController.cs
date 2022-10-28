using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Providers;
using ARSystemFrontEnd.Models;
using ARSystem.Domain.Models;
using ARSystem.Service;


namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ReceiveDoc")]
    public class ApiReceiveDocumentBAUKController : ApiController
    {
        private readonly ReceiveDocumentBAUKService docBAUKService;

        public ApiReceiveDocumentBAUKController()
        {
            docBAUKService = new ReceiveDocumentBAUKService();
        }

        [HttpPost, Route("summary")]
        public IHttpActionResult GetSummary(PostReceiveDocumentBAUK post)
        {
            try
            {
                int intTotalRecord = 0;
                var result = new List<vwRAReceiveDocumentBAUK>();

                var param = new vwRAReceiveDocumentBAUK();
                param.SONumber = post.vSONumber;
                param.SiteID = post.vSiteID;
                param.SiteName = post.vSiteName;
                param.StartSubmit = post.vStartSubmit;
                param.EndSubmit = post.vEndSubmit;
                param.CustomerID = post.vCustomerID;
                param.CompanyID = post.vCompanyID;
                param.StatusDoc = post.vStatusDoc;
                param.ProductID = post.vProductID;
                param.STIPID = post.vStip;

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)   
                {
                    intTotalRecord = 0;
                    result.Add(new vwRAReceiveDocumentBAUK(userCredential.ErrorType, userCredential.ErrorMessage));
                }
                else
                {
                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    intTotalRecord = docBAUKService.GetReceiveDocumentBAUKCount(param, userCredential.UserID);
                    result = docBAUKService.GetReceiveDocumentBAUK(param, userCredential.UserID, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost, Route("history")]
        public IHttpActionResult GetHistory(PostReceiveDocumentBAUK post)
        {
            try
            {
                List<trxReceiveDocumentBAUK> list = new List<trxReceiveDocumentBAUK>();

                list = docBAUKService.GetHistory(UserManager.User.UserToken, post.vSONumber).ToList();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("CheckList")]
        public IHttpActionResult GetCheckList(PostReceiveDocumentBAUK post)
        {
            try
            {
                List<vwRAReceiveDocumentBAUK> CheckList = new List<vwRAReceiveDocumentBAUK>();

                CheckList = docBAUKService.GetCheckList(UserManager.User.UserToken, post.strSONumber.ToList(), post.vAction).ToList();

                return Ok(CheckList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("PICReceiveDoc")]
        public IHttpActionResult GetPICReceiveDoc()
        {
            try
            {
                List<vwPICReceiveDocumentBAUK> list = new List<vwPICReceiveDocumentBAUK>();

                list = docBAUKService.GetPICReceiveDocumentBAUK(UserManager.User.UserToken).ToList();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SaveReceiveDocBulky")]
        public IHttpActionResult SaveReceiveDocumentBulky(PostReceiveDocumentBAUK post)
        {
            try
            {
                List<trxReceiveDocumentBAUK> trxReceiveDocument = new List<trxReceiveDocumentBAUK>();

                foreach (var item in post.ListTrxReceive)
                {
                    trxReceiveDocument.Add(new trxReceiveDocumentBAUK
                    {
                        BaukSubmitBySystem = item.BaukSubmitBySystem,
                        SONumber = item.SONumber,
                        SiteID = item.SiteID,
                        SiteName = item.SiteName,
                        CustomerName =item.CustomerName,
                        Remarks = item.Remarks,
                        StatusDocument = item.StatusDocument,
                        PICReceive = item.PICReceive,
                        ReceiveDate = item.ReceiveDate,
                        CreatedDate = System.DateTime.Now
                    });
                }

                List<trxReceiveDocumentBAUK> result = docBAUKService.SaveReceiveDocBulky(UserManager.User.UserToken, trxReceiveDocument);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetReceiveBySonumb")]
        public IHttpActionResult GetReceiveBySonumb(PostReceiveDocumentBAUK post)
        {
            try
            {
                List<vwRAReceiveDocumentBAUK> list = new List<vwRAReceiveDocumentBAUK>();

                list = docBAUKService.GetReceiveDocumentBySonumb(UserManager.User.UserToken, post.vSONumber).ToList();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Submit")]
        public IHttpActionResult SubmitTrx(PostReceiveDocumentBAUK post)
        {
            try
            {
                var result = new trxReceiveDocumentBAUK();

                var trxDoc = new trxReceiveDocumentBAUK();
                trxDoc.SONumber = post.vSONumber;
                trxDoc.SiteID = post.vSiteID;
                trxDoc.SiteName = post.vSiteName;
                trxDoc.CustomerName = post.vCustomerID;
                trxDoc.Remarks = post.vRemarks;
                trxDoc.StatusDocument = post.vAction;
                trxDoc.PICReceive = post.vPICReceive;
                trxDoc.ReceiveDate = post.vReceiveDate;
                trxDoc.CreatedDate = System.DateTime.Now;

                result = docBAUKService.Savetrx(UserManager.User.UserToken, trxDoc);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("CustomerReceiveBAUK")]
        public IHttpActionResult GetCustomerReceiveBAUK()
        {
            try
            {
                List<vwCustomerReceiveBAUK> list = new List<vwCustomerReceiveBAUK>();

                list = docBAUKService.GetCustomerReceiveBAUK(UserManager.User.UserToken).ToList();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}