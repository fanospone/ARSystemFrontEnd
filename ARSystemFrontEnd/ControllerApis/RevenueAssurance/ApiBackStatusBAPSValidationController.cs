using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using System.Web;
using System.Collections.Specialized;
using ARSystemFrontEnd.Helper;
using System.IO;
using System.Configuration;
using ARSystem.Domain.Models;
using ARSystem.Service;
using DocumentFormat.OpenXml.Office.CustomUI;
using System.Collections;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/BackStatusBAPSValidation")]
    public class ApiBackStatusBAPSValidationController : ApiController
    {
        private readonly BackStatusBAPSValidationService backStatusService;

        public ApiBackStatusBAPSValidationController()
        {
            backStatusService = new BackStatusBAPSValidationService();
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetBAPSList(PostBackStatusBAPSValidationView post)

        {
            try
            {
                int intTotalRecord = 0;
                var result = new List<vwBackStatusBAPSValidation>();

                var param = new vwBackStatusBAPSValidation();
                param.CustomerSiteName = post.CustomerSiteName;
                param.SoNumber = post.SoNumber;
                param.BapsType = post.BapsType;
                param.StipSiro = post.StipSiro;

                string strSearch = post.search == null ? "" : post.search.value;

                string strOrderBy = "";
                if (post.order != null)
                    strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    intTotalRecord = 0;
                    result.Add(new vwBackStatusBAPSValidation(userCredential.ErrorType, userCredential.ErrorMessage));
                }
                else
                {
                    intTotalRecord = backStatusService.GetBapsCount(param, strSearch, userCredential.UserID);
                    result = backStatusService.GetBapsList(param, strSearch, userCredential.UserID, strOrderBy, true, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        

        [HttpPost, Route("list")]
        public IHttpActionResult GetAllBAPSList(PostBackStatusBAPSValidationView post)
        {
            try
            {
                var result = new List<vwBackStatusBAPSValidation>();

                var param = new vwBackStatusBAPSValidation();
                param.CustomerSiteName = post.CustomerSiteName;
                param.SoNumber = post.SoNumber;
                param.BapsType = post.BapsType;
                param.StipSiro = post.StipSiro;

                string strSearch = post.search == null ? "" : post.search.value;

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    result.Add(new vwBackStatusBAPSValidation(userCredential.ErrorType, userCredential.ErrorMessage));
                else
                    result = backStatusService.GetBapsList(param, strSearch, userCredential.UserID, "", false).ToList();

                return Ok(new { data = result });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost, Route("count")]
        public IHttpActionResult GetAllBAPSCount(PostBackStatusBAPSValidationView post)
        {
            try
            {
                int intTotalRecord = 0;

                var param = new vwBackStatusBAPSValidation();
                param.CustomerSiteName = post.CustomerSiteName;
                param.SoNumber = post.SoNumber;
                param.BapsType = post.BapsType;
                param.StipSiro = post.StipSiro;

                string strSearch = post.search == null ? "" : post.search.value;

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    intTotalRecord = 0;
                else
                    intTotalRecord = backStatusService.GetBapsCount(param, strSearch, userCredential.UserID);

                return Ok(new { data = intTotalRecord });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet, Route("GetListStipSiro")]
        public IHttpActionResult GetAllStipSiro()
        {
            try
            {
                List<int> siroList = new List<int>();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    siroList = new List<int>();
                else
                    siroList = backStatusService.GetListStipSiro(userCredential.UserID);

                return Ok(siroList);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("submit")]
        public IHttpActionResult ProcessBackStatus(PostBackStatusBAPSValidationSubmit post)
        {
            try
            {
                bool isSuccess = false;

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                    isSuccess = false;
                else
                    isSuccess = backStatusService.ProcessBackStatus(post.remark, post.dataList, userCredential.UserID);

                return Ok(new { data = isSuccess });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}