using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using System.Threading.Tasks;
using ARSystemFrontEnd.Helper;
using System.Data;
using Newtonsoft.Json;
using ARSystem.Domain.Models.ViewModels;
using ARSystem.Service.ARSystem.Dashboard;
using ARSystem.Domain.Models.ViewModels.Datatable;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/DashboardInvoiceProduction")]
    public class ApiDashboardInvoiceProductionController : ApiController
    {
        private InvoiceProductionService _services;

        public ApiDashboardInvoiceProductionController()
        {
            _services = new InvoiceProductionService();
        }

        private void pDisposeService()
        {
            _services.Dispose();
        }

        [HttpPost, Route("getSummaryOutStanding")]
        public IHttpActionResult GetSummaryOutStanding(vmInvoiceProductionPost param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vmInvoiceProduction data = new vmInvoiceProduction(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    var result = _services.GetSummaryOutStanding(userCredential.UserID, param);
                    var createInvoiceValue = result.Where(x => x.Type == "CreateInvoice").Select(x => x.Value).FirstOrDefault();
                    var postingValue = result.Where(x => x.Type == "Posting").Select(x => x.Value).FirstOrDefault();
                    var submitDocInvoiceValue = result.Where(x => x.Type == "SubmitInvoice").Select(x => x.Value).FirstOrDefault();
                    var receiptARRValue = result.Where(x => x.Type == "ReceiptARR").Select(x => x.Value).FirstOrDefault();
                    var submitOperatorValue = result.Where(x => x.Type == "SubmitOperator").Select(x => x.Value).FirstOrDefault();
                    var lt1Value = result.Where(x => x.Type == "LT1").Select(x => x.Value).FirstOrDefault();
                    var lt2Value = result.Where(x => x.Type == "LT2").Select(x => x.Value).FirstOrDefault();
                    var lt3Value = result.Where(x => x.Type == "LT3").Select(x => x.Value).FirstOrDefault();
                    var matchingARValue = result.Where(x => x.Type == "MatchingAR").Select(x => x.Value).FirstOrDefault();

                    return Ok(new
                    {
                        CreateInvoice = createInvoiceValue,
                        Posting = postingValue,
                        SubmitDocInvoice = submitDocInvoiceValue,
                        ReceiptARR = receiptARRValue,
                        SubmitOperator = submitOperatorValue,
                        LT1 = lt1Value,
                        LT2 = lt2Value,
                        LT3 = lt3Value,
                        MatchingAR = matchingARValue
                    });
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

        [HttpPost, Route("getSummaryProduction")]
        public IHttpActionResult GetSummaryProduction(vmInvoiceProductionPost param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vmInvoiceProduction data = new vmInvoiceProduction(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    var result = _services.GetSummaryProduction(userCredential.UserID, param);
                    var createInvoiceValue = result.Where(x => x.Type == "CreateInvoice").Select(x => x.Value).FirstOrDefault();
                    var postingValue = result.Where(x => x.Type == "Posting").Select(x => x.Value).FirstOrDefault();
                    var submitDocInvoiceValue = result.Where(x => x.Type == "SubmitInvoice").Select(x => x.Value).FirstOrDefault();
                    var receiptARRValue = result.Where(x => x.Type == "ReceiptARR").Select(x => x.Value).FirstOrDefault();
                    var submitOperatorValue = result.Where(x => x.Type == "SubmitOperator").Select(x => x.Value).FirstOrDefault();
                    var lt1Value = result.Where(x => x.Type == "LT1").Select(x => x.Value).FirstOrDefault();
                    var lt2Value = result.Where(x => x.Type == "LT2").Select(x => x.Value).FirstOrDefault();
                    var lt3Value = result.Where(x => x.Type == "LT3").Select(x => x.Value).FirstOrDefault();
                    var matchingARValue = result.Where(x => x.Type == "MatchingAR").Select(x => x.Value).FirstOrDefault();

                    return Ok(new
                    {
                        CreateInvoice = createInvoiceValue,
                        Posting = postingValue,
                        SubmitDocInvoice = submitDocInvoiceValue,
                        ReceiptARR = receiptARRValue,
                        SubmitOperator = submitOperatorValue,
                        LT1 = lt1Value,
                        LT2 = lt2Value,
                        LT3 = lt3Value,
                        MatchingAR = matchingARValue
                    });
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

        [HttpPost, Route("getHeader")]
        public IHttpActionResult GetHeader(vmInvoiceProductionPost param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vmInvoiceProduction data = new vmInvoiceProduction(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    Datatable<vmInvoiceProduction> result = _services.GetHeaderList(userCredential.UserID, param);

                    return Ok(new { data = result.List });
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

        [HttpPost, Route("getDetail")]
        public IHttpActionResult GetDetail(vmInvoiceProductionPost param)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                {
                    vmInvoiceProduction data = new vmInvoiceProduction(userCredential.ErrorType, userCredential.ErrorMessage);
                    return Ok(data);
                }
                else
                {
                    Datatable<vmInvoiceProduction> result = _services.GetDetailList(userCredential.UserID, param);

                    return Ok(new { data = result.List });
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

        //[HttpPost, Route("GetHeaderList")]
        //public IHttpActionResult GetHeaderList(PostvwDashboardInvoiceProductionHeader post)
        //{
        //    try
        //    {
        //        string strWhereClause = " 1=1 ";
        //            //"DepartmentName !='Not Mapping'  AND CustomerID IS NOT NULL AND TenantType IS NOT NULL ";

        //        //if (post.paramRow != null)
        //        //{
        //        //    if (post.type == "GroupBySection")
        //        //        strWhereClause += " AND DepartmentName = '" + post.paramRow.Trim() + "'";
        //        //    else if (post.type == "GroupBySOW")
        //        //        strWhereClause += " AND TenantType = '" + post.paramRow.Trim() + "'";
        //        //    else if (post.type == "GroupByProduct")
        //        //        strWhereClause += " AND ProductName = '" + post.paramRow + "'";
        //        //    else if (post.type == "GroupBySTIPCategory")
        //        //        strWhereClause += " AND StipCategory = '" + post.paramRow + "'";
        //        //    else if (post.type == "GroupByRegional")
        //        //        strWhereClause += " AND RegionName = '" + post.paramRow + "'";
        //        //    else if (post.type == "GroupByOperator")
        //        //        strWhereClause += " AND CustomerInvoice = '" + post.paramRow + "'";
        //        //    else if (post.type == "GroupByCompany")
        //        //        strWhereClause += " AND CustomerID = '" + post.paramRow + "'";

        //        //}
        //        //if (post.YearBill != null)
        //        //{
        //        //    strWhereClause += " AND YearInvoiceCategory = '" + post.YearBill + "'";
        //        //}
        //        //if (post.paramColumn != null && post.paramColumn != "13" && post.paramColumn.Length != 0)
        //        //{
        //        //    strWhereClause += " AND MONTH(StartInvoiceDate) = '" + fnGetMonthHeaderString(post.paramColumn) + "'";
        //        //}

        //        //if (post.STIPDate != null)
        //        //    strWhereClause += " AND YEAR(StipDate) = '" + post.STIPDate.ToString() + "'";
        //        //if (post.RFIDate != null)
        //        //    strWhereClause += " AND YEAR(RFIDate) = '" + post.RFIDate.ToString() + "'";
        //        //if (post.SecName != null)
        //        //    strWhereClause += " AND DepartmentCode = '" + post.SecName.ToString().Trim() + "'";
        //        //if (post.SOWName != null)
        //        //    strWhereClause += " AND TenantType = '" + post.SOWName.ToString().Trim() + "'";
        //        //if (post.ProductID != null)
        //        //    strWhereClause += " AND ProductID = " + post.ProductID;
        //        //if (post.STIPID != null)
        //        //    strWhereClause += " AND STIPID = " + post.STIPID;
        //        //if (post.RegionalID != null)
        //        //    strWhereClause += " AND RegionID = " + post.RegionalID;
        //        //if (post.CompanyID != null)
        //        //    strWhereClause += " AND CompanyInvoice = '" + post.CompanyID + "'";
        //        //if (post.Customer != null)
        //        //    strWhereClause += " AND CustomerID = '" + post.Customer + "'";

        //        //if (post.SoNumber != null && post.SoNumber != "")
        //        //{
        //        //    strWhereClause += " AND SoNumber = '" + post.SoNumber + "'";
        //        //}

        //        //if (post.SiteID != null && post.SiteID != "")
        //        //{
        //        //    strWhereClause += " AND SiteID = '" + post.SiteID + "'";
        //        //}

        //        //if (post.SiteName != null && post.SiteName != "")
        //        //{
        //        //    strWhereClause += " AND SiteName LIKE '%" + post.SiteName + "%'";
        //        //}
        //        var CountOfRows = summary.GetHeaderListCount(strWhereClause);

        //        string strOrderBy = "";
        //        if (post.order != null)
        //            if (post.columns[post.order[0].column].data != "0")
        //                strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();


        //        var data = summary.GetHeaderList(strWhereClause, post.start, post.length, strOrderBy);

        //        return Ok(new { draw = post.draw, recordsTotal = CountOfRows, recordsFiltered = CountOfRows, data = data });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.InnerException);
        //    }
        //}
        //[HttpPost, Route("GetDetailList")]
        //public IHttpActionResult GetDetailList(PostvwDashboardInvoiceProductionDetails post)
        //{
        //    try
        //    {
        //        string strWhereClause = " 1=1 ";
        //        //"DepartmentName !='Not Mapping'  AND CustomerID IS NOT NULL AND TenantType IS NOT NULL ";

        //        //if (post.paramRow != null)
        //        //{
        //        //    if (post.type == "GroupBySection")
        //        //        strWhereClause += " AND DepartmentName = '" + post.paramRow.Trim() + "'";
        //        //    else if (post.type == "GroupBySOW")
        //        //        strWhereClause += " AND TenantType = '" + post.paramRow.Trim() + "'";
        //        //    else if (post.type == "GroupByProduct")
        //        //        strWhereClause += " AND ProductName = '" + post.paramRow + "'";
        //        //    else if (post.type == "GroupBySTIPCategory")
        //        //        strWhereClause += " AND StipCategory = '" + post.paramRow + "'";
        //        //    else if (post.type == "GroupByRegional")
        //        //        strWhereClause += " AND RegionName = '" + post.paramRow + "'";
        //        //    else if (post.type == "GroupByOperator")
        //        //        strWhereClause += " AND CustomerInvoice = '" + post.paramRow + "'";
        //        //    else if (post.type == "GroupByCompany")
        //        //        strWhereClause += " AND CustomerID = '" + post.paramRow + "'";

        //        //}
        //        //if (post.YearBill != null)
        //        //{
        //        //    strWhereClause += " AND YearInvoiceCategory = '" + post.YearBill + "'";
        //        //}
        //        //if (post.paramColumn != null && post.paramColumn != "13" && post.paramColumn.Length != 0)
        //        //{
        //        //    strWhereClause += " AND MONTH(StartInvoiceDate) = '" + fnGetMonthHeaderString(post.paramColumn) + "'";
        //        //}

        //        //if (post.STIPDate != null)
        //        //    strWhereClause += " AND YEAR(StipDate) = '" + post.STIPDate.ToString() + "'";
        //        //if (post.RFIDate != null)
        //        //    strWhereClause += " AND YEAR(RFIDate) = '" + post.RFIDate.ToString() + "'";
        //        //if (post.SecName != null)
        //        //    strWhereClause += " AND DepartmentCode = '" + post.SecName.ToString().Trim() + "'";
        //        //if (post.SOWName != null)
        //        //    strWhereClause += " AND TenantType = '" + post.SOWName.ToString().Trim() + "'";
        //        //if (post.ProductID != null)
        //        //    strWhereClause += " AND ProductID = " + post.ProductID;
        //        //if (post.STIPID != null)
        //        //    strWhereClause += " AND STIPID = " + post.STIPID;
        //        //if (post.RegionalID != null)
        //        //    strWhereClause += " AND RegionID = " + post.RegionalID;
        //        //if (post.CompanyID != null)
        //        //    strWhereClause += " AND CompanyInvoice = '" + post.CompanyID + "'";
        //        //if (post.Customer != null)
        //        //    strWhereClause += " AND CustomerID = '" + post.Customer + "'";

        //        //if (post.SoNumber != null && post.SoNumber != "")
        //        //{
        //        //    strWhereClause += " AND SoNumber = '" + post.SoNumber + "'";
        //        //}

        //        //if (post.SiteID != null && post.SiteID != "")
        //        //{
        //        //    strWhereClause += " AND SiteID = '" + post.SiteID + "'";
        //        //}

        //        //if (post.SiteName != null && post.SiteName != "")
        //        //{
        //        //    strWhereClause += " AND SiteName LIKE '%" + post.SiteName + "%'";
        //        //}
        //        var CountOfRows = summary.GetDetailsListCount(strWhereClause);

        //        string strOrderBy = "";
        //        if (post.order != null)
        //            if (post.columns[post.order[0].column].data != "0")
        //                strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();


        //        var data = summary.GetDetailsList(strWhereClause, post.start, post.length, strOrderBy);

        //        return Ok(new { draw = post.draw, recordsTotal = CountOfRows, recordsFiltered = CountOfRows, data = data });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.InnerException);
        //    }
        //}

    }
}