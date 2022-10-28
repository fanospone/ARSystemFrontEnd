using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ReserveNumber")]
    public class ApiReserveInvoiceNumberController : ApiController
    {

        [HttpPost, Route("grid")]
        public IHttpActionResult GetReserveNumberListToGrid(PostReserveInvoiceNumber post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vmReserveInvoiceNumber> list = new List<ARSystemService.vmReserveInvoiceNumber>();
                using (var client = new ARSystemService.ItrxReserveInvoiceNumberClient())
                {
                    intTotalRecord = client.GetReserveInvoiceNumberCount(UserManager.User.UserToken, post.OperatorId, post.CompanyId,post.StartDateRequest,post.EndDateRequest);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    list = client.GetReserveInvoiceNumberToList(UserManager.User.UserToken, post.OperatorId, post.CompanyId, post.StartDateRequest, post.EndDateRequest, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Reserve")]
        public IHttpActionResult Reserve(PostReserveInvoiceNumber post)
        {
            try
            {
                ARSystemService.trxReserveInvoiceNumberHeader Header = new ARSystemService.trxReserveInvoiceNumberHeader();
                using (var client = new ARSystemService.ItrxReserveInvoiceNumberClient())
                {

                    Header = client.ReserveNumber(UserManager.User.UserToken, post.OperatorId,post.CompanyId,post.Remarks,post.AmountReserve);
                }

                return Ok(Header);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridReplace")]
        public IHttpActionResult GetReplaceNumberListToGrid(PostReplaceInvoiceNumber post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwReserveInvoiceNumberDetail> list = new List<ARSystemService.vwReserveInvoiceNumberDetail>();
                using (var client = new ARSystemService.ItrxReserveInvoiceNumberClient())
                {
                    intTotalRecord = client.GetReplaceInvoiceNumberCount(UserManager.User.UserToken, post.OperatorId, post.CompanyId,post.ReserveNo,post.Year);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    list = client.GetReplaceInvoiceNumberToList(UserManager.User.UserToken, post.OperatorId, post.CompanyId,post.ReserveNo,post.Year, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetInvoiceNumber")]
        public IHttpActionResult GetInvoiceNumberList()
        {
            try
            {
                ARSystemService.vmStringResult StringResult = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxReserveInvoiceNumberClient())
                {
                    StringResult = client.GetPostedInvoice(UserManager.User.UserToken);
                }

                return Ok(StringResult);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetInvoiceNumberProperties")]
        public IHttpActionResult GetInvoiceNumberProperties(string InvNo)
        {
            try
            {
                ARSystemService.vmInvoiceNumberProperties StringResult = new ARSystemService.vmInvoiceNumberProperties();
                using (var client = new ARSystemService.ItrxReserveInvoiceNumberClient())
                {
                    StringResult = client.GetInvoiceNumberProperties(UserManager.User.UserToken, InvNo);
                }

                return Ok(StringResult);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("ReplaceInvoiceNumber")]
        public IHttpActionResult ReplaceInvoiceNumber(PostReplaceInvoiceNumber post)
        {
            try
            {
                post.DataReservedInvoiceNumber.PairedNo = post.InvNoHeader;
                ARSystemService.trxReserveInvoiceNumberDetail ReserveInvoiceDetail = new ARSystemService.trxReserveInvoiceNumberDetail();
                using (var client = new ARSystemService.ItrxReserveInvoiceNumberClient())
                {
                    ReserveInvoiceDetail = client.ReplaceInvoiceNumber(UserManager.User.UserToken, post.DataReservedInvoiceNumber);
                }

                return Ok(ReserveInvoiceDetail);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("ReleaseNumber")]
        public IHttpActionResult ReleaseNumber(string InvNo)
        {
            try
            {
                ARSystemService.vmStringResult StringResult = new ARSystemService.vmStringResult();
                using (var client = new ARSystemService.ItrxReserveInvoiceNumberClient())
                {
                    StringResult = client.ReleaseNumber(UserManager.User.UserToken, InvNo);
                }

                return Ok(StringResult);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

    }
}