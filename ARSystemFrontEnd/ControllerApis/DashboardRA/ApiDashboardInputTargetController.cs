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
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using System.Threading.Tasks;
using ARSystemFrontEnd.Helper;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/ApiDashboardInputTarget")]
    public class ApiDashboardInputTargetController : ApiController
    {
        DashboardInputTargetService _service = new DashboardInputTargetService();

        [HttpPost, Route("GetListDashboard")]
        public IHttpActionResult GetListDashboard(PostDashboardInputTarget post)
        {
            try
            {
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {

                    return Ok(new TrxRAAmountTarget(userCredential.ErrorType, userCredential.ErrorMessage));
                }
                int intTotalRecord = 4;

                List<vwDashboardInputTarget> dInputTarget = new List<vwDashboardInputTarget>();

                //intTotalRecord = client.GetTrxDashbordTSELDataCount(param);

                dInputTarget = _service.GetInputTargetDashboard(UserManager.User.UserToken, post.CompanyInvoiceID, post.CustomerID, post.Year.GetValueOrDefault()).ToList();
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = dInputTarget });

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
        [HttpPost, Route("GetListDetail")]
        public IHttpActionResult GetListDetail(PostDashboardInputTarget post)
        {
            try
            {
                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    return Ok(new TrxRAAmountTarget(userCredential.ErrorType, userCredential.ErrorMessage));
                }
                var param = new vwDashboardInputTargetDetail();
                //non TSEL filter managed in UI Layer (javascript)
                param.Year = post.Year;
                param.CompanyInvoiceID = post.CompanyInvoiceID;
                param.CustomerID = post.CustomerID;
                param.Month = post.Month;
                param.DepartmentCode = post.DepartmentCode;

                int intTotalRecord = 0;

                intTotalRecord = _service.GetCountInputTargetDetail(UserManager.User.UserToken, param);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();
                List<vwDashboardInputTargetDetail> detailInputTarget = new List<vwDashboardInputTargetDetail>();

                detailInputTarget = _service.GetListInputTargetDetail(UserManager.User.UserToken, param, strOrderBy, post.start, post.length).ToList();
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = detailInputTarget });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    [HttpPost, Route("SaveInputTargetPercentage")]
        public IHttpActionResult SaveInputTargetPercentage(PostTrxRAInputTargetPercentage model)
        {
            try
            {
                List<trxRAInputTargetPercentage> dInputTargets = new List<trxRAInputTargetPercentage>();

                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    return Ok(dInputTargets = new List<trxRAInputTargetPercentage>() { new trxRAInputTargetPercentage(userCredential.ErrorType, userCredential.ErrorMessage) });
                }
                for (int month = 1; month <= 12; month++)
                {
                    var param = new trxRAInputTargetPercentage
                    {
                        Year = model.Year,
                        Month = month,
                        Department = model.DepartmentCode
                    };
                    var trxPercentage = new trxRAInputTargetPercentage();
                    var findtrxPercentage =    _service.FindtrxRAInputTargetPercentage(UserManager.User.UserToken, param);
                    if(findtrxPercentage != null)
                    {
                        trxPercentage.ID = findtrxPercentage.ID;
                        trxPercentage.CreatedDate = findtrxPercentage.CreatedDate;
                        trxPercentage.CreatedBy = findtrxPercentage.CreatedBy;
                    }
                    trxPercentage.Year = model.Year;
                    trxPercentage.Month = month;
                    trxPercentage.Department = model.DepartmentCode;

                    trxPercentage.Optimist = (month == 1) ? model.Jan_Optimist.GetValueOrDefault(0) :
                                            (month == 2) ? model.Feb_Optimist.GetValueOrDefault(0) :
                                            (month == 3) ? model.Mar_Optimist.GetValueOrDefault(0) :
                                            (month == 4) ? model.Apr_Optimist.GetValueOrDefault(0) :
                                            (month == 5) ? model.May_Optimist.GetValueOrDefault(0) :
                                            (month == 6) ? model.Jun_Optimist.GetValueOrDefault(0) :
                                            (month == 7) ? model.Jul_Optimist.GetValueOrDefault(0) :
                                            (month == 8) ? model.Aug_Optimist.GetValueOrDefault(0) :
                                            (month == 9) ? model.Sep_Optimist.GetValueOrDefault(0) :
                                            (month == 10) ? model.Oct_Optimist.GetValueOrDefault(0) :
                                            (month == 11) ? model.Nov_Optimist.GetValueOrDefault(0) :
                                            (month == 12) ? model.Dec_Optimist.GetValueOrDefault(0) : 0;
                    trxPercentage.MostLikely = (month == 1) ? model.Jan_MostLikely.GetValueOrDefault(0) :
                                            (month == 2) ? model.Feb_MostLikely.GetValueOrDefault(0) :
                                            (month == 3) ? model.Mar_MostLikely.GetValueOrDefault(0) :
                                            (month == 4) ? model.Apr_MostLikely.GetValueOrDefault(0) :
                                            (month == 5) ? model.May_MostLikely.GetValueOrDefault(0) :
                                            (month == 6) ? model.Jun_MostLikely.GetValueOrDefault(0) :
                                            (month == 7) ? model.Jul_MostLikely.GetValueOrDefault(0) :
                                            (month == 8) ? model.Aug_MostLikely.GetValueOrDefault(0) :
                                            (month == 9) ? model.Sep_MostLikely.GetValueOrDefault(0) :
                                            (month == 10) ? model.Oct_MostLikely.GetValueOrDefault(0) :
                                            (month == 11) ? model.Nov_MostLikely.GetValueOrDefault(0) :
                                            (month == 12) ? model.Dec_MostLikely.GetValueOrDefault(0) : 0;
                    trxPercentage.Pessimist = (month == 1) ? model.Jan_Pessimist.GetValueOrDefault(0) :
                                            (month == 2) ? model.Feb_Pessimist.GetValueOrDefault(0) :
                                            (month == 3) ? model.Mar_Pessimist.GetValueOrDefault(0) :
                                            (month == 4) ? model.Apr_Pessimist.GetValueOrDefault(0) :
                                            (month == 5) ? model.May_Pessimist.GetValueOrDefault(0) :
                                            (month == 6) ? model.Jun_Pessimist.GetValueOrDefault(0) :
                                            (month == 7) ? model.Jul_Pessimist.GetValueOrDefault(0) :
                                            (month == 8) ? model.Aug_Pessimist.GetValueOrDefault(0) :
                                            (month == 9) ? model.Sep_Pessimist.GetValueOrDefault(0) :
                                            (month == 10) ? model.Oct_Pessimist.GetValueOrDefault(0) :
                                            (month == 11) ? model.Nov_Pessimist.GetValueOrDefault(0) :
                                            (month == 12) ? model.Dec_Pessimist.GetValueOrDefault(0) : 0;
                    trxPercentage = _service.SavetrxRAInputTargetPercentage(UserManager.User.UserToken, trxPercentage);

                }

                //var res = _service.SavetrxRAInputTargetPercentage(UserManager.User.UserToken, inputTarget);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return Ok(new PostTrxRAInputTargetPercentage { ErrorMessage = ex.Message });
            }
        }
        [HttpPost, Route("FindInputTargetPercentage")]
        public IHttpActionResult FindInputTargetPercentage(PostTrxRAInputTargetPercentage model)
        {
            try
            {
                //get data per year
                List<trxRAInputTargetPercentage> dInputTargets = new List<trxRAInputTargetPercentage>();

                vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                if (userCredential.ErrorType > 0)
                {
                    return Ok(dInputTargets = new List<trxRAInputTargetPercentage>() { new trxRAInputTargetPercentage(userCredential.ErrorType, userCredential.ErrorMessage) });
                }
                var resInYear = new PostTrxRAInputTargetPercentage();
                resInYear.Year = model.Year;
                resInYear.DepartmentCode = model.DepartmentCode;
                for (int month = 1; month <=12; month++)
                {
                    var param = new trxRAInputTargetPercentage
                    {
                        Year = model.Year,
                        Month = month,
                        Department = model.DepartmentCode
                    };
                    var res = _service.FindtrxRAInputTargetPercentage(UserManager.User.UserToken, param);
                    if(res != null)
                    {
                        if (month == 1)
                        { resInYear.Jan_Optimist = res.Optimist; resInYear.Jan_MostLikely = res.MostLikely; resInYear.Jan_Pessimist = res.Pessimist; }
                        if (month == 2)
                        {   resInYear.Feb_Optimist = res.Optimist; resInYear.Feb_MostLikely = res.MostLikely; resInYear.Feb_Pessimist = res.Pessimist;}
                        if (month == 3)
                        { resInYear.Mar_Optimist = res.Optimist; resInYear.Mar_MostLikely = res.MostLikely; resInYear.Mar_Pessimist = res.Pessimist; }
                        if (month == 4)
                        { resInYear.Apr_Optimist = res.Optimist; resInYear.Apr_MostLikely = res.MostLikely; resInYear.Apr_Pessimist = res.Pessimist; }
                        if (month == 5)
                        { resInYear.May_Optimist = res.Optimist; resInYear.May_MostLikely = res.MostLikely; resInYear.May_Pessimist = res.Pessimist; }
                        if (month == 6)
                        { resInYear.Jun_Optimist = res.Optimist; resInYear.Jun_MostLikely = res.MostLikely; resInYear.Jun_Pessimist = res.Pessimist; }
                        if (month == 7)
                        { resInYear.Jul_Optimist = res.Optimist; resInYear.Jul_MostLikely = res.MostLikely; resInYear.Jul_Pessimist = res.Pessimist; }
                        if (month == 8)
                        { resInYear.Aug_Optimist = res.Optimist; resInYear.Aug_MostLikely = res.MostLikely; resInYear.Aug_Pessimist = res.Pessimist; }
                        if (month == 9)
                        { resInYear.Sep_Optimist = res.Optimist; resInYear.Sep_MostLikely = res.MostLikely; resInYear.Sep_Pessimist = res.Pessimist; }
                        if (month == 10)
                        { resInYear.Oct_Optimist = res.Optimist; resInYear.Oct_MostLikely = res.MostLikely; resInYear.Oct_Pessimist = res.Pessimist; }
                        if (month == 11)
                        { resInYear.Nov_Optimist = res.Optimist; resInYear.Nov_MostLikely = res.MostLikely; resInYear.Nov_Pessimist = res.Pessimist; }
                        if (month == 12)
                        { resInYear.Dec_Optimist = res.Optimist; resInYear.Dec_MostLikely = res.MostLikely; resInYear.Dec_Pessimist = res.Pessimist; }
                    }
                }


                return Ok(resInYear);
            }
            catch (Exception ex)
            {
                return Ok(new PostTrxRAInputTargetPercentage { ErrorMessage = ex.Message});
            }
        }

    }
}
