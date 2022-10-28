using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystemFrontEnd.Helper;
using ARSystem.Domain.Models;
using ARSystem.Service;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ISPInvoiceInformation")]
    public class ApiISPInvoiceInformationController : ApiController
    {
        private readonly ISPInvoiceInformationService client;
        public ApiISPInvoiceInformationController()
        {
            client = new ISPInvoiceInformationService();

        }
        [HttpPost, Route("InvoiceInformationGrid")]
        public IHttpActionResult GetISPInvoiceInformationGrid(PostISPInvoiceInformation post)
        {
            try
            {
                if (UserManager.User == null)
                {
                    return Ok(new
                    {
                        draw = 0,
                        recordsTotal = 0,
                        recordsFiltered = 0,
                        data = new vwISPInvoiceInformation(2, "Session timeOut, please relogin")
                    });
                }

                int intTotalRecord = 0;
                List<vwISPInvoiceInformation> data = new List<vwISPInvoiceInformation>();
                string strWhereClause = "1=1";

                if (!string.IsNullOrWhiteSpace(post.slCompany))
                {
                    strWhereClause += $" AND CompanyID Like '%{post.slCompany.Trim()}%'";
                }
                if (!string.IsNullOrWhiteSpace(post.slStipCode))
                {
                    strWhereClause += $" AND STIPCode Like '%{post.slStipCode}%'";
                }
                if (!string.IsNullOrWhiteSpace(post.slCustomer))
                {
                    strWhereClause += $" AND OperatorID Like '%{post.slCustomer}%'";
                }
                if (!string.IsNullOrWhiteSpace(post.fSONumber))
                {
                    strWhereClause += $" AND SONumber Like '%{post.fSONumber}%'";
                }
                if (!string.IsNullOrWhiteSpace(post.fSiteID))
                {
                    strWhereClause += $" AND SiteID Like '%{post.fSiteID}%'";
                }
                if (!string.IsNullOrWhiteSpace(post.fSiteName))
                {
                    strWhereClause += $" AND SiteName Like '%{post.fSiteName}%'";
                }
                if (!string.IsNullOrWhiteSpace(post.fSiteIDOpr))
                {
                    strWhereClause += $" AND CustomerSiteID Like '%{post.fSiteIDOpr}%'";
                }
                if (!string.IsNullOrWhiteSpace(post.fSiteNameOpr))
                {
                    strWhereClause += $" AND CustomerSiteName Like '%{post.fSiteNameOpr}%'";
                }
                intTotalRecord = client.GetISPInvoiceInformationCount(strWhereClause);
                string strOrderBy = "";


                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + "" + post.order[0].dir.ToLower();
                if (intTotalRecord == 0)
                {
                    data = client.GetISPInvoiceInformationList(strWhereClause, post.start, 0).ToList();
                }
                else
                    data = client.GetISPInvoiceInformationList(strWhereClause, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = data });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("TrxISPInvoiceInformationBySonumb/{SONumber?}")]
        public IHttpActionResult TrxISPInvoiceInformationBySonumb(string SONumber = "")
        {
            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            if (userCredential.ErrorType > 0)
            {

                return Ok(new vwISPInvoiceInformation(userCredential.ErrorType, userCredential.ErrorMessage));
            }
            try
            {
                if (UserManager.User == null)
                {
                    return Ok(new
                    {
                        draw = 0,
                        recordsTotal = 0,
                        recordsFiltered = 0,
                        data = new vwISPInvoiceInformation(2, "Session timeOut, please relogin")
                    });
                }
                var strWhereClause = $" SONumber='{SONumber}'";
                var trxISP = client.GetISPInvoiceInformationList(strWhereClause, 0, 0, "").FirstOrDefault();
                ///var trxSales = client.GetSalesSystemInformation(strWhereClause, userCredential.UserID);
                return Ok(new
                {
                    trxISP = trxISP,
                    //  trxSales = trxSales
                });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetSalesSystemInformation")]
        public IHttpActionResult GetSalesSystemInformation(string SONumber)
        {

            try
            {
                List<vwISPSalesSystemInformation> result = client.GetSalesSystemList(UserManager.User.UserToken, SONumber).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("GetBAPSNewInformation")]
        public IHttpActionResult GetBAPSNewInformation(string SONumber)
        {

            try
            {
                List<vwISPBAPSNewInformation> result = client.GetBAPSNewList(UserManager.User.UserToken, SONumber).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetInvoiceInformationDetail")]
        public IHttpActionResult GetInvoiceInformationDetail(string SONumber)
        {

            try
            {
                List<vwISPInvoiceInformationDetail> result = client.GetInvoiceInformationDetailList(UserManager.User.UserToken, SONumber).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


    }
}

