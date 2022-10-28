using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/ReportInvoiceTower")]
    public class ApiReportInvoiceTowerController : ApiController
    {
        [HttpPost, Route("grid")]
        public IHttpActionResult GetReportInvoiceTowerDataToGrid(PostTrxReportInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwReportInvoiceTowerByInvoice> ReportInvoiceData = new List<ARSystemService.vwReportInvoiceTowerByInvoice>();
                using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetTrxReportInvoiceTowerCount(UserManager.User.UserToken,post.strStartPeriod, post.strEndPeriod, post.strCompanyCode,post.intYearPosting, post.intMonthPosting, post.intWeekPosting, post.invNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    ReportInvoiceData = client.GetTrxReportInvoiceTowerToList(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod,post.strCompanyCode, strOrderBy,post.intYearPosting,post.intMonthPosting,post.intWeekPosting, post.invNo, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = ReportInvoiceData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        
        [HttpPost, Route("InvoicelistGrid")]
        public IHttpActionResult GetSiteListData(ARSystemService.vmGetInvoicePostedList vm)
        {
            try
            {
                List<ARSystemService.vwReportInvoiceTowerByInvoice> ReportInvoiceData = new List<ARSystemService.vwReportInvoiceTowerByInvoice>();
                using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
                {
                    ReportInvoiceData = client.GetReportInvoiceListDataToList(UserManager.User.UserToken, vm).ToList();
                }

                return Ok(ReportInvoiceData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetYear")]
        public IHttpActionResult GetYear()
        {
            try
            {
                List<ARSystemService.vmReportInvoiceYear> list = new List<ARSystemService.vmReportInvoiceYear>();
                using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
                {
                    list = client.GetReportInvoiceYear(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetMonth")]
        public IHttpActionResult GetMonth()
        {
            try
            {
                List<ARSystemService.vmReportInvoiceMonth> list = new List<ARSystemService.vmReportInvoiceMonth>();
                using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
                {
                    list = client.GetReportInvoiceMonth(UserManager.User.UserToken).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetWeek")]
        public IHttpActionResult GetWeek(int intYearPosting, int intMonthPosting)
        {
            try
            {
                List<ARSystemService.vmReportInvoiceWeek> list = new List<ARSystemService.vmReportInvoiceWeek>();
                using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
                {
                    list = client.GetReportInvoiceWeek(UserManager.User.UserToken, intYearPosting, intMonthPosting).ToList();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("DownloadExcelToAXReportInvoice")]
        public IHttpActionResult DownloadReportInvoiceTower(PostTrxPrintInvoiceTowerView post)
        {
            try
            {
                ARSystemService.trxInvoiceHeader InvoiceHeader = new ARSystemService.trxInvoiceHeader();
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = post.HeaderId.ToArray();
                vm.CategoryId = post.CategoryId.ToArray();
                vm.isCNInvoice = post.isCNInvoice.ToArray();

                using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
                {
                    InvoiceHeader = client.DownloadExceltoAX(UserManager.User.UserToken, vm);
                }

                return Ok(InvoiceHeader);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("BySONumber")]
        public IHttpActionResult GetReportInvoiceTowerBySONumberDataToList(string strStartPeriod, string strEndPeriod, string strCompanyCode, string invNo)
        {
            try
            {
                List<ARSystemService.vwReportInvoiceTowerBySoNumber> ReportInvoiceData = new List<ARSystemService.vwReportInvoiceTowerBySoNumber>();
                using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
                {
                    ReportInvoiceData = client.GetTrxReportInvoiceTowerBySONumberToList(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyCode, invNo, "trxInvoiceHeaderID",0, 0).ToList();
                }

                return Ok(ReportInvoiceData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridBySONumber")]
        public IHttpActionResult GetReportInvoiceTowerBySONumberDataToGrid(PostTrxReportInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwReportInvoiceTowerBySoNumber> ReportInvoiceData = new List<ARSystemService.vwReportInvoiceTowerBySoNumber>();
                using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetTrxReportInvoiceTowerBySONumberCount(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.strCompanyCode, post.invNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    ReportInvoiceData = client.GetTrxReportInvoiceTowerBySONumberToList(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod,post.strCompanyCode, post.invNo, strOrderBy,post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = ReportInvoiceData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        
        [HttpPost, Route("GetListIdByInvoice")]
        public IHttpActionResult GetReportInvoiceTowerListIdByInvoice(PostTrxReportInvoiceTowerView post)
        {
            try
            {
                List<ARSystemService.vmCheckAllInvoice> Result = new List<ARSystemService.vmCheckAllInvoice>();
                using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
                {
                    Result = client.GetTrxReportInvoiceTowerByInvoiceListId(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.invNo, post.intYearPosting, post.intMonthPosting, post.intWeekPosting).ToList();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridCompile")]
        public IHttpActionResult GetReportInvoiceCompileDataToGrid(PostTrxReportInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwReportInvoiceCompileByInvoice> ReportInvoiceData = new List<ARSystemService.vwReportInvoiceCompileByInvoice>();
                using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetTrxReportInvoiceCompileCount(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.intYearPosting, post.intMonthPosting, post.intWeekPosting, post.invNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    ReportInvoiceData = client.GetTrxReportInvoiceCompileToList(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, strOrderBy, post.intYearPosting, post.intMonthPosting, post.intWeekPosting, post.invNo, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = ReportInvoiceData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("gridCompileBySONumber")]
        public IHttpActionResult GetReportInvoiceCompileBySONumberDataToGrid(PostTrxReportInvoiceTowerView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.vwReportInvoiceCompileBySoNumber> ReportInvoiceData = new List<ARSystemService.vwReportInvoiceCompileBySoNumber>();
                using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
                {
                    intTotalRecord = client.GetTrxReportInvoiceCompileBySONumberCount(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.invNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    ReportInvoiceData = client.GetTrxReportInvoiceCompileBySONumberToList(UserManager.User.UserToken, post.strStartPeriod, post.strEndPeriod, post.invNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = ReportInvoiceData });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }

}
    
