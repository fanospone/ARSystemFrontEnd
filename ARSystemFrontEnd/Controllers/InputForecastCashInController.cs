using ARSystem.Domain.Models;
using ARSystem.Service.ARSystem;
using ARSystemFrontEnd.Domain.Models;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Models.TrxInputForecast;
using ARSystemFrontEnd.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("InputForecastCashIn")]
    public class InputForecastCashInController : BaseController
    {
        private readonly InputForecastCashInService _inputForecastCashInService;
        public InputForecastCashInController()
        {
            _inputForecastCashInService = new InputForecastCashInService();
        }
        public string forecastTypeFcVsAct = "ForecastVsActual";
        public string forecastTypeFcVsFc = "ForecastVsForecast";
        public string actionBySectionHead = "Section Head";
        public string actionByDeptHead = "Department Head";
        public string approved = "Approved";
        public string rejected = "Rejected";
        public string act_DeptHeadApproval = "act_DeptHeadApproval";
        public string act_SectionHeadApproval = "act_approvalSectionHead";
        public string act_ClosedCashInPeriod = "act_ClosedCashInPeriod";
        

        [Authorize]
        [Route("Index")]
        public ActionResult Index()
        {
            string actionTokenView = "3472f509-76fc-4967-9b36-5c39b534ba7b";
            ViewBag.IsShowApprovalFormFcVsAct = false;
            ViewBag.IsShowApprovalFormFcVsFc = false;
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserToken = UserManager.User.UserToken;
                    return View("InputForecastCashIn");
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

        }


        [Authorize]
        [ChildActionOnly]
        [Route("ForecastVsActual")]
        public ActionResult ForecastVsActual()
        {
            string actionTokenView = "8563fff3-bd83-4327-98f7-543bb2121292";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                    if (userCredential.ErrorType > 0)
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    var approvalPeriod = DateTime.Today.AddMonths(-1);
                    ViewBag.UserToken = UserManager.User.UserToken;
                    int currentQuarter = Convert.ToInt32(Math.Floor((decimal)(approvalPeriod.Month + 2) / 3));
                    var approvals = new List<trxARCashInForecastApprovalLog>();
                    var approvalLogs = _inputForecastCashInService.GetForecastApprovalLogByPeriod(UserManager.User.UserID, new trxARCashInForecastApprovalLog()
                    {
                        ForecastType = forecastTypeFcVsAct,
                        ForecastQuarter = currentQuarter,
                        ForecastYear = approvalPeriod.Year,
                        ForecastMonth = approvalPeriod.Month
                    }).OrderByDescending(m => m.ActionDate);
                    //is completed?
                    var processOID = approvalLogs.Any() ? approvalLogs.FirstOrDefault().ProcessOID : null;
                    ViewBag.JogetProcessOID = processOID;
                    var approvalStatusCurrentPeriod = _inputForecastCashInService.GetWaitingApprovalProcess(processOID);
                    ViewBag.ApprovalStatusCurrentPeriod = approvalStatusCurrentPeriod;
                    //completed if section head and dept head approved, it will contains min. 2 rows data

                    //section head 
                    var sectionHeadUser = _inputForecastCashInService.GetUserByPosition(UserManager.User.UserID, "Account Receivable Database Section Head");
                    var latestSectionHeadAppr = approvalLogs.Where(m => m.ApprovalType == act_SectionHeadApproval || m.ApprovalType == "act_ReviseSectionHead").OrderByDescending(m => m.ActionDate).FirstOrDefault();
                    var sectionHeadAppr = new trxARCashInForecastApprovalLog()
                    {
                        Action = (latestSectionHeadAppr != null) ? latestSectionHeadAppr.Action : null,
                        ActionDate = (latestSectionHeadAppr != null) ? latestSectionHeadAppr.ActionDate : null,
                        Remarks = (latestSectionHeadAppr != null) ? latestSectionHeadAppr.Remarks : null,
                        ActionBy = (latestSectionHeadAppr != null) ? latestSectionHeadAppr.ActionBy : String.Join(", ", sectionHeadUser.Select(m => String.Format("{0}-{1}", m.UserID, m.UserName))),
                        ApprovalType = act_SectionHeadApproval,
                        ApprovalSequence = 1
                    };
                    approvals.Add(sectionHeadAppr);
                    //check if section head approved, show approal for dept.head
                    if (approvalStatusCurrentPeriod == act_DeptHeadApproval || approvalStatusCurrentPeriod == "act_ReviseSectionHead" || approvalStatusCurrentPeriod == act_ClosedCashInPeriod)
                    {
                        //dept head 
                        var deptHeadUser = _inputForecastCashInService.GetUserByPosition(UserManager.User.UserID, "Account Receivable Database Department Head");
                        var latestDeptHeadAppr = approvalLogs.Where(m => m.ApprovalType == act_DeptHeadApproval).FirstOrDefault();
                        var deptHeadAppr = new trxARCashInForecastApprovalLog()
                        {
                            Action = (latestDeptHeadAppr != null && approvalStatusCurrentPeriod != act_DeptHeadApproval) ? latestDeptHeadAppr.Action : null,
                            ActionDate = (latestDeptHeadAppr != null && approvalStatusCurrentPeriod != act_DeptHeadApproval) ? latestDeptHeadAppr.ActionDate : null,
                            Remarks = (latestDeptHeadAppr != null && approvalStatusCurrentPeriod != act_DeptHeadApproval) ? latestDeptHeadAppr.Remarks : null,
                            ActionBy = (latestDeptHeadAppr != null && latestDeptHeadAppr.ActionBy != null && approvalStatusCurrentPeriod != act_DeptHeadApproval) ? latestDeptHeadAppr.ActionBy : String.Join(", ", deptHeadUser.Select(m => String.Format("{0}-{1}", m.UserID, m.UserName))),
                            ApprovalType = act_DeptHeadApproval,
                            ApprovalSequence = 2
                        };
                        approvals.Add(deptHeadAppr);
                    }
                    var previlegedApproval =
                        (approvalStatusCurrentPeriod == act_ClosedCashInPeriod) ? null :
                        //waiting for department action
                        (approvalStatusCurrentPeriod == act_DeptHeadApproval) ? _inputForecastCashInService.GetUserByPosition(userCredential.UserID, "Account Receivable Database Department Head") :
                        //waiting for section head adtion
                        _inputForecastCashInService.GetUserByPosition(userCredential.UserID, "Account Receivable Database Section Head");
                    ViewBag.WaitingForAction = (previlegedApproval != null) ? String.Join(", ", previlegedApproval.Select(m => m.UserID)) : String.Empty;
                    ViewBag.IsApprovalUser = (previlegedApproval != null) ? previlegedApproval.Any(m => m.UserID == userCredential.UserID) : false;
                    approvals = (processOID == null && ViewBag.IsApprovalUser == false) ? null : approvals;
                    return PartialView(approvals);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

        }

        [Authorize]
        [Route("UpdateForecastVsActual")]
        public ActionResult UpdateForecastVsActual(PostTrxInputForecast post)
        {
            string actionTokenView = "8563fff3-bd83-4327-98f7-543bb2121292";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserToken = UserManager.User.UserToken;
                    try
                    {
                        var model = new uspARCashInForecastVsActual()
                        {
                            OperatorID = post.OperatorID,
                            Year = post.Year,
                            Quarter = post.Quarter
                        };
                        var res = _inputForecastCashInService.GetByOperatorInPeriod(UserManager.User.UserID, model);
                        int currentQuarter = Convert.ToInt32(Math.Floor((decimal)((DateTime.Today.Month-1) + 2) / 3));
                        int numberOfMonthWithinQuarter = 0;
                        int startMonth = currentQuarter * 3 - 2;
                        for (int i = 0; i < 3; i++)
                        {
                            if (DateTime.Today.Month == (startMonth + i))
                            {
                                numberOfMonthWithinQuarter = i + 1;
                            }
                        }
                        res.Month = post.Quarter * 3 + (post.Month - 3);
                        return PartialView("_FormForecastVsActual", res);
                    }
                    catch (Exception ex)
                    {
                        return PartialView(ex);
                    }
                }
                else
                {
                    return PartialView("Token Expired");
                }
            }

        }

        [Authorize]
        [Route("UpdateForecastVsForecast")]
        public ActionResult UpdateForecastVsForecast(PostTrxInputForecast post)
        {
            string actionTokenView = "ff952974-7665-40e8-855e-53338f8a9cb2";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserToken = UserManager.User.UserToken;
                    try
                    {
                        var model = new uspARCashInForecastVsForecast()
                        {
                            OperatorID = post.OperatorID,
                            Year = post.Year,
                            Quarter = post.Quarter
                        };
                        var res = _inputForecastCashInService.GetByOperatorInPeriodFcVsFc(UserManager.User.UserID, model);

                        return PartialView("_FormForecastVsForecast", res);
                    }
                    catch (Exception ex)
                    {
                        return PartialView(ex);
                    }
                }
                else
                {
                    return PartialView("Token Expired");
                }
            }

        }

        [Authorize]
        [ChildActionOnly]
        [Route("ForecastVsForecast")]
        public ActionResult ForecastVsForecast()
        {
            string actionTokenView = "ff952974-7665-40e8-855e-53338f8a9cb2";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                    if (userCredential.ErrorType > 0)
                    {
                        return RedirectToAction("Logout", "Login");
                    }
                    ViewBag.UserToken = UserManager.User.UserToken;
                    var approvalPeriod = DateTime.Today.AddMonths(-1);
                    int currentQuarter = Convert.ToInt32(Math.Floor((decimal)(approvalPeriod.Month + 2) / 3));
                    var approvals = new List<trxARCashInForecastApprovalLog>();



                    var approvalLogs = _inputForecastCashInService.GetForecastApprovalLogByPeriod(UserManager.User.UserID, new trxARCashInForecastApprovalLog()
                    {
                        ForecastType = forecastTypeFcVsFc,
                        ForecastQuarter = currentQuarter,
                        ForecastYear = approvalPeriod.Year,
                        ForecastMonth = approvalPeriod.Month
                    }).OrderByDescending(m => m.ActionDate);
                    //is completed?
                    var processOID = approvalLogs.Any() ? approvalLogs.FirstOrDefault().ProcessOID : null;
                    ViewBag.JogetProcessOID = processOID;
                    var approvalStatusCurrentPeriod = _inputForecastCashInService.GetWaitingApprovalProcess(processOID);
                    ViewBag.ApprovalStatusCurrentPeriod = approvalStatusCurrentPeriod;
                    //completed if section head and dept head approved, it will contains min. 2 rows data

   
                        //section head 
                        var sectionHeadUser = _inputForecastCashInService.GetUserByPosition(UserManager.User.UserID, "Account Receivable Database Section Head");
                        var latestSectionHeadAppr = approvalLogs.Where(m => m.ApprovalType == act_SectionHeadApproval).FirstOrDefault();
                    var sectionHeadAppr = new trxARCashInForecastApprovalLog()
                    {
                        Action = (latestSectionHeadAppr != null) ? latestSectionHeadAppr.Action : null,
                        ActionDate = (latestSectionHeadAppr != null) ? latestSectionHeadAppr.ActionDate : null,
                        Remarks = (latestSectionHeadAppr != null) ? latestSectionHeadAppr.Remarks : null,
                        ActionBy = (latestSectionHeadAppr != null) ? latestSectionHeadAppr.ActionBy : String.Join(", ", sectionHeadUser.Select(m => String.Format("{0}-{1}", m.UserID, m.UserName))),
                        ApprovalType = act_SectionHeadApproval,
                        ApprovalSequence = 1
                    };
                    approvals.Add(sectionHeadAppr);
                    //check if section head approved, show approal for dept.head
                    if (approvalStatusCurrentPeriod == act_DeptHeadApproval || approvalStatusCurrentPeriod == "act_ReviseSectionHead" || approvalStatusCurrentPeriod == act_ClosedCashInPeriod)
                    {
                        //dept head 
                        var deptHeadUser = _inputForecastCashInService.GetUserByPosition(UserManager.User.UserID, "Account Receivable Database Department Head");
                        var latestDeptHeadAppr = approvalLogs.Where(m => m.ApprovalType == act_DeptHeadApproval).FirstOrDefault();
                        var deptHeadAppr = new trxARCashInForecastApprovalLog()
                        {
                            Action = (latestDeptHeadAppr != null && approvalStatusCurrentPeriod != act_DeptHeadApproval) ? latestDeptHeadAppr.Action : null,
                            ActionDate = (latestDeptHeadAppr != null && approvalStatusCurrentPeriod != act_DeptHeadApproval) ? latestDeptHeadAppr.ActionDate : null,
                            Remarks = (latestDeptHeadAppr != null && approvalStatusCurrentPeriod != act_DeptHeadApproval) ? latestDeptHeadAppr.Remarks : null,
                            ActionBy = (latestDeptHeadAppr != null && latestDeptHeadAppr.ActionBy != null && approvalStatusCurrentPeriod != act_DeptHeadApproval) ? latestDeptHeadAppr.ActionBy : String.Join(", ", deptHeadUser.Select(m => String.Format("{0}-{1}", m.UserID, m.UserName))),
                            ApprovalType = act_DeptHeadApproval,
                            ApprovalSequence = 2
                        };
                        approvals.Add(deptHeadAppr);
                    }
                    var previlegedApproval =
                        (approvalStatusCurrentPeriod == act_ClosedCashInPeriod) ? null :
                        //waiting for department action
                        (approvalStatusCurrentPeriod == act_DeptHeadApproval) ? _inputForecastCashInService.GetUserByPosition(userCredential.UserID, "Account Receivable Database Department Head") :
                        //waiting for section head adtion
                        _inputForecastCashInService.GetUserByPosition(userCredential.UserID, "Account Receivable Database Section Head");
                    ViewBag.WaitingForAction = (previlegedApproval != null) ? String.Join(", ", previlegedApproval.Select(m => m.UserID)) : String.Empty;
                    ViewBag.IsApprovalUser = (previlegedApproval != null) ? previlegedApproval.Any(m => m.UserID == userCredential.UserID) : false;
                    approvals = (processOID == null && ViewBag.IsApprovalUser == false) ? null : approvals;
                    return PartialView(approvals);

                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

        }
    }
}