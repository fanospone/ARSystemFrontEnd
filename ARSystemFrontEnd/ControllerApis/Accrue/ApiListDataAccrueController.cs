using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;
using ARSystemFrontEnd.Helper;
using System.Web;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ListDataAccrue")]
    public class ApiListDataAccrueController : ApiController
    {
        ListDataAccrueService client = new ListDataAccrueService();
        #region bind dropdownlist filter search
        [HttpGet, Route("company/list")]
        public IHttpActionResult GetCompanyTBiGSysToList()
        {
            try
            {
                List<mstCompany> companyList = new List<mstCompany>();

                companyList = client.GetCompanyList(UserManager.User.UserToken).ToList();
                return Ok(companyList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("customer/list")]
        public IHttpActionResult GetCustomerList(bool? bolIsTelco)
        {
            try
            {
                List<mstCustomer> customerList = new List<mstCustomer>();
                customerList = client.GetCustomerList(UserManager.User.UserToken, bolIsTelco).ToList();
                return Ok(customerList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("regional/list")]
        public IHttpActionResult GetregionalList()
        {
            try
            {
                List<mstRegion> regionList = new List<mstRegion>();
                regionList = client.GetRegionList("", null, "").ToList();
                return Ok(regionList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("department/list")]
        public IHttpActionResult GetdepartmentList()
        {
            try
            {
                List<vwAccrueMstSOW> deptList = new List<vwAccrueMstSOW>();
                deptList = client.GetDepartmentToList("").ToList();
                return Ok(deptList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("departmentAccrue/list")]
        public IHttpActionResult GetdepartmentHCList()
        {
            try
            {
                List<mstAccrueDepartment> deptList = new List<mstAccrueDepartment>();
                deptList = client.GetDepartmentHCToList("").ToList();
                return Ok(deptList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("statusAccrue/list")]
        public IHttpActionResult GetStatusAccrueToList()
        {
            try
            {
                List<mstAccrueStatus> accrueStatusList = new List<mstAccrueStatus>();
                accrueStatusList = client.GetStatusAccrueToList("").ToList();
                return Ok(accrueStatusList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("statusListDataAccrue/list")]
        public IHttpActionResult GetStatusListDataAccrueToList()
        {
            try
            {
                List<mstAccrueStatus> accrueStatusList = new List<mstAccrueStatus>();
                accrueStatusList = client.GetStatusListDataAccrueToList("").ToList();
                return Ok(accrueStatusList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("departmentAccrue/list")]
        public IHttpActionResult GetDepartmentAccrueToList(string typeSOW)
        {
            try
            {
                List<mstAccrueMappingSOW> List = new List<mstAccrueMappingSOW>();
                List = client.GetMappingSOWToList("", typeSOW).ToList();
                return Ok(List);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("rootCause/list")]
        public IHttpActionResult GetRootCauseAccrueToList()
        {
            try
            {
                List<mstAccrueRootCause> List = new List<mstAccrueRootCause>();
                List = client.GetRootCauseToList("").ToList();
                return Ok(List);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("picaAccrue/list")]
        public IHttpActionResult GetPICAAccrueToList(string rootCauseID)
        {
            try
            {
                List<mstAccruePICA> List = new List<mstAccruePICA>();
                List = client.GetPICAToList("", rootCauseID).ToList();
                return Ok(List);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("picaDetailAccrue/list")]
        public IHttpActionResult GetPICADetailAccrueToList(string picaID)
        {
            try
            {
                List<mstAccruePICADetail> List = new List<mstAccruePICADetail>();
                List = client.GetPICADetailToList("", picaID).ToList();
                return Ok(List);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("weekOfMonth/list")]
        public IHttpActionResult GetweekOfMonthToList(string date)
        {
            try
            {
                List<vmWeek> List = new List<vmWeek>();
                List = client.GetWeekNumberOfMonthList(date).ToList();
                return Ok(List);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpGet, Route("weekOfMonth/listGetDate")]
        public IHttpActionResult GetweekOfMonthToListGetDate()
        {
            try
            {
                List<vmWeek> List = new List<vmWeek>();
                List = client.GetWeekNumberOfMonthListGetDate().ToList();
                return Ok(List);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("Month/SetMonthGetDate")]
        public IHttpActionResult SetMonthGetDate()
        {
            try
            {
                string month = "";
                month = DateTime.Now.ToString("MMM-yyyy");
                return Ok(month);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion

        [HttpPost, Route("grid")]
        public IHttpActionResult Grid(PostAccrueView post)
        {
            try
            {
                int intTotalRecord = 0;
                List<dwhAccrueListData> List = new List<dwhAccrueListData>();

                intTotalRecord = client.GetDataAccrueToListCount("", post.vwAccrueList);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                List = client.GetDataAccrueToList("", post.vwAccrueList, strOrderBy, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = List });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("gridDetailAll")]
        public IHttpActionResult gridDetailAll(PostAccrueView post)
        {
            try
            {
                int intTotalRecord = 0;
                string strOrderBy = "";
                List<vwAccrueList> List = new List<vwAccrueList>();

                List = HttpContext.Current.Items["ListIDAll"] as List<vwAccrueList>;
                intTotalRecord = List.Count();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = List });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("GetDataAccrueListId")]
        public IHttpActionResult GetListId(PostAccrueView post)
        {
            try
            {
                List<string> ListId = new List<string>();
                List<dwhAccrueListData> List = client.GetDataAccrueListId("", post.vwAccrueList);
                //HttpContext.Current.Session.Add("ListIDAll", "1111");

                ListId = List.Select(l => l.IDTemp).ToList();
                return Ok(ListId);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("GetAddAccrueDataList")]
        public IHttpActionResult GetAddAccrueData(PostAccrueView post)
        {
            try
            {
                int intTotalRecord = 0;
                List<dwhAccrueListData> List = new List<dwhAccrueListData>();

                intTotalRecord = client.GetAddAccrueDataListCount("", post.ListID.ToList());

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                List = client.GetAddAccrueDataList("", post.ListID.ToList(), strOrderBy, post.start, post.length).ToList();


                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = List });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("SubmitDataAccrue")]
        public IHttpActionResult SubmitDataAccrue(PostAccrueView post)
        {
            try
            {
                trxDataAccrue data = new trxDataAccrue();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    data = new trxDataAccrue(userCredential.ErrorType, userCredential.ErrorMessage);
                else
                    data = client.SubmitDataAccrue(userCredential.UserID, post.ListID.ToList(), post.paramAllData, post.vwAccrueList);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}