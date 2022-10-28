using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystemFrontEnd.Domain.Models;
using ARSystemFrontEnd.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowService;

namespace ARSystem.Service.ARSystem
{
    public partial class InputForecastCashInService : IDisposable
    {
        private DbContext context;
        private trxARCashInForecastVsActualRepository _trxARCashInForecastVsActualRepository;
        private trxARCashInForecastVsForecastRepository _trxARCashInForecastVsForecastRepository;
        private trxARCashInForecastApprovalLogRepository _trxARCastInForecastApprovalLogRepository;
        private trxARCashInActualDetailsRepository _trxARCashInActualDetailsRepository;
        private vwARUserRoleRepository _vwARUserRoleRepository;
        private readonly Workflow workflow;

        public InputForecastCashInService()
        {
            context = new DbContext(Helper.GetConnection("ARSystem"));
            _trxARCashInForecastVsActualRepository = new trxARCashInForecastVsActualRepository(context);
            _trxARCashInForecastVsForecastRepository = new trxARCashInForecastVsForecastRepository(context);
            _trxARCastInForecastApprovalLogRepository = new trxARCashInForecastApprovalLogRepository(context);
            _trxARCashInActualDetailsRepository = new trxARCashInActualDetailsRepository(context);
            _vwARUserRoleRepository = new vwARUserRoleRepository(context);
            workflow = new Workflow();

        }

        #region FORECAST VS ACTUAL (AK: MONTHLYH)

        public int GetForecastVsActualCount(string UserID, uspARCashInForecastVsActual model)
        {
            try
            {
                string strWhereClause = pGetWhereClauseForecastVsActual(model);

                return _trxARCashInForecastVsActualRepository.GetCount(strWhereClause, model.Year.GetValueOrDefault(), model.Quarter.GetValueOrDefault());
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID);
                return 0;
            }
            finally
            {
            }
        }

        public List<uspARCashInForecastVsActual> GetForecastVsActualList(string UserID, uspARCashInForecastVsActual model, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            List<uspARCashInForecastVsActual> list = new List<uspARCashInForecastVsActual>();
            try
            {
                string strWhereClause = pGetWhereClauseForecastVsActual(model);

                if (intPageSize > 0)
                    list = _trxARCashInForecastVsActualRepository.GetPaged(strWhereClause, model.Year.GetValueOrDefault(), model.Quarter.GetValueOrDefault(), strOrderBy, intRowSkip, intPageSize);
                else
                    list = _trxARCashInForecastVsActualRepository.GetList(strWhereClause, model.Year.GetValueOrDefault(), model.Quarter.GetValueOrDefault(), strOrderBy);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new uspARCashInForecastVsActual((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID)));
                return list;
            }
            finally
            {
            }
        }

        public List<uspARCashInForecastVsActual> CheckPiCaValidation(string UserID, uspARCashInForecastVsActual model, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            List<uspARCashInForecastVsActual> list = new List<uspARCashInForecastVsActual>();
            try
            {
                    list = _trxARCashInForecastVsActualRepository.CheckPiCaValidation(model.Year.GetValueOrDefault(), model.Quarter.GetValueOrDefault(), model.Month.GetValueOrDefault(), strOrderBy);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new uspARCashInForecastVsActual((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID)));
                return list;
            }
            finally
            {
            }
        }
        public List<uspARCashInForecastVsForecast> CheckPiCaValidationForecastVsForecast(string UserID, uspARCashInForecastVsForecast model, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            List<uspARCashInForecastVsForecast> list = new List<uspARCashInForecastVsForecast>();
            try
            {
                list = _trxARCashInForecastVsForecastRepository.CheckPiCaValidation(model.Year.GetValueOrDefault(), model.Quarter.GetValueOrDefault(), strOrderBy);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new uspARCashInForecastVsForecast((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID)));
                return list;
            }
            finally
            {
            }
        }
        
        private string pGetWhereClauseForecastVsActual(uspARCashInForecastVsActual model)
        {
            string strWhereClause = "1=1";
            if (!string.IsNullOrWhiteSpace(model.OperatorID))
            {
                strWhereClause += " AND OperatorID LIKE '%" + model.OperatorID + "%'";
            }

            return strWhereClause;
        }



        public List<uspARCashInForecastVsActual> UpdateInputForecastVsActual(string UserID, List<uspARCashInForecastVsActual> forecast)
        {
            //forecast parameter will always contains of 3 row data (3 months forecast)
            var context = new DbContext(Helper.GetConnection("ARSystem"));

            List<uspARCashInForecastVsActual> result = new List<uspARCashInForecastVsActual>();
            try
            {
                result = _trxARCashInForecastVsActualRepository.Update(forecast);
                return result;
            }
            catch (Exception ex)
            {
                context.Dispose();
                result.Add(new uspARCashInForecastVsActual((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InputTargetService", "UpdateInputForecastVsActual", UserID)));
                return result;
            }

        }

            public uspARCashInForecastVsActual GetByOperatorInPeriod(string UserID, uspARCashInForecastVsActual model)
            {
                var forecast = new uspARCashInForecastVsActual();
                try
                {
                    var context = new DbContext(Helper.GetConnection("ARSystem"));
                    forecast = _trxARCashInForecastVsActualRepository.GetByOperatorInPeriod(model.OperatorID, model.Year.GetValueOrDefault(), model.Quarter.GetValueOrDefault());
                    return forecast;
                }
                catch (Exception ex)
                {
                    context.Dispose();
                    forecast = new uspARCashInForecastVsActual((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InputTargetService", "GetHistoryNewBapsInputTargetByID", UserID));
                    return forecast;
                }
            }


        #endregion

        #region actual detail

        public int GetActualSummaryDetailCount(string UserID, trxARCashInActualDetails model)
        {
            try
            {
                string strWhereClause = pGetWhereClauseActualSummaryDetail(model);

                return _trxARCashInActualDetailsRepository.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID);
                return 0;
            }
            finally
            {
            }
        }

        public List<trxARCashInActualDetails> GetActualSummaryDetailList(string UserID, trxARCashInActualDetails model, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            List<trxARCashInActualDetails> list = new List<trxARCashInActualDetails>();
            try
            {
                string strWhereClause = pGetWhereClauseActualSummaryDetail(model);

                if (intPageSize > 0)
                    list = _trxARCashInActualDetailsRepository.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    list = _trxARCashInActualDetailsRepository.GetList(strWhereClause,  strOrderBy);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new trxARCashInActualDetails((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID)));
                return list;
            }
            finally
            {
            }
        }

        private string pGetWhereClauseActualSummaryDetail(trxARCashInActualDetails model)
        {
            string strWhereClause = "1=1";
            if (model.Year > 0)
            {
                strWhereClause += " AND Year =  " + model.Year + "";
            }
            if (model.Quarter > 0)
            {
                strWhereClause += " AND Quarter = " + model.Quarter + "";
            }
            if (model.Quarter > 0 && model.MonthInQuarter > 0)
            {
                var _month = (model.Quarter * 3 - 3) + model.MonthInQuarter;
                strWhereClause += " AND Month = " + _month + "";
            }
            if(model.InvoiceOperatorID != String.Empty)
            {
                strWhereClause += " AND InvoiceOperatorID = '" + model.InvoiceOperatorID + "'";
            }
            return strWhereClause;
        }



        #endregion

        #region FORECAST VS FORECAST (AK: QUARTER)
        public int GetForecastVsForecastCount(string UserID, uspARCashInForecastVsForecast model)
        {
            try
            {
                string strWhereClause = pGetWhereClauseForecastVsForecast(model);

                return _trxARCashInForecastVsForecastRepository.GetCount(strWhereClause, model.Year.GetValueOrDefault(), model.Quarter.GetValueOrDefault());
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID);
                return 0;
            }
            finally
            {
            }
        }

        public List<uspARCashInForecastVsForecast> GetForecastVsForecastList(string UserID, uspARCashInForecastVsForecast model, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            List<uspARCashInForecastVsForecast> list = new List<uspARCashInForecastVsForecast>();
            try
            {
                string strWhereClause = pGetWhereClauseForecastVsForecast(model);

                if (intPageSize > 0)
                    list = _trxARCashInForecastVsForecastRepository.GetPaged(strWhereClause, model.Year.GetValueOrDefault(), model.Quarter.GetValueOrDefault(), strOrderBy, intRowSkip, intPageSize);
                else
                    list = _trxARCashInForecastVsForecastRepository.GetList(strWhereClause, model.Year.GetValueOrDefault(), model.Quarter.GetValueOrDefault(), strOrderBy);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new uspARCashInForecastVsForecast((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID)));
                return list;
            }
            finally
            {
            }
        }



        private string pGetWhereClauseForecastVsForecast(uspARCashInForecastVsForecast model)
        {
            string strWhereClause = "1=1";
            if (!string.IsNullOrWhiteSpace(model.OperatorID))
            {
                strWhereClause += " AND OperatorID LIKE '%" + model.OperatorID + "%'";
            }

            return strWhereClause;
        }



        public List<uspARCashInForecastVsForecast> UpdateInputForecastVsForecast(string UserID, List<uspARCashInForecastVsForecast> forecast)
        {
            //forecast parameter will always contains of 3 row data (3 months forecast)
            var context = new DbContext(Helper.GetConnection("ARSystem"));

            List<uspARCashInForecastVsForecast> result = new List<uspARCashInForecastVsForecast>();
            try
            {
                result = _trxARCashInForecastVsForecastRepository.Update(forecast);
                return result;
            }
            catch (Exception ex)
            {
                context.Dispose();
                result.Add(new uspARCashInForecastVsForecast((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InputTargetService", "UpdateInputForecastVsActual", UserID)));
                return result;
            }

        }

        public uspARCashInForecastVsForecast GetByOperatorInPeriodFcVsFc(string UserID, uspARCashInForecastVsForecast model)
        {
            var forecast = new uspARCashInForecastVsForecast();
            try
            {
                var context = new DbContext(Helper.GetConnection("ARSystem"));
                forecast = _trxARCashInForecastVsForecastRepository.GetByOperatorInPeriod(model.OperatorID, model.Year.GetValueOrDefault(), model.Quarter.GetValueOrDefault());
                return forecast;
            }
            catch (Exception ex)
            {
                context.Dispose();
                forecast = new uspARCashInForecastVsForecast((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InputTargetService", "GetHistoryNewBapsInputTargetByID", UserID));
                return forecast;
            }
        }


        #endregion

        public List<trxARCashInForecastApprovalLog> GetForecastApprovalLogByPeriod(string UserID, trxARCashInForecastApprovalLog model)
        {
            List<trxARCashInForecastApprovalLog> list = new List<trxARCashInForecastApprovalLog>();
            try
            {
                list = _trxARCastInForecastApprovalLogRepository.GetApprovalLogByPeriod(model.ForecastYear, model.ForecastQuarter, model.ForecastMonth, model.ForecastType);
                var processOID = list.Any() ? list.FirstOrDefault().ProcessOID : null;

                var aa = workflow.GetActivityLogByProcessOID(processOID.GetValueOrDefault()).Where(m => m.ActivityDefinitionId == "act_approvalSectionHead" ||
                m.ActivityDefinitionId == "act_DeptHeadApproval" || m.ActivityDefinitionId == "act_ReviseSectionHead").Select(m => new trxARCashInForecastApprovalLog()
                {
                    Remarks = m.Remark,
                    Action = m.Decision,
                    ActionDate = (m.Decision == "Approved" || m.Decision == "Rejected") ? m.LogDate : null,
                    ActionBy = m.Name,
                    ApprovalType = m.ActivityDefinitionId,
                    ProcessOID = m.ProcessOID,
                    ApprovalSequence = (m.ActivityDefinitionId == "act_DeptHeadApproval") ? 2 : 1
                }).ToList();
                return aa;
            }
            catch (Exception ex)
            {
                list.Add(new trxARCashInForecastApprovalLog((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID)));
                return list;
            }
            finally
            {
            }
        }

        public string GetWaitingApprovalProcess(decimal? processID)
        {
            if(processID != null)
            {
                var app = workflow.GetActivityByProcessId(null, processID);
                if (app != null)
                {
                    return app.ActivityDefinitionId;
                }
            }
            
            return String.Empty;
        }

        public trxARCashInForecastApprovalLog CreateApprovalLog(string UserID, trxARCashInForecastApprovalLog model)
        {
            trxARCashInForecastApprovalLog result = new trxARCashInForecastApprovalLog();
            try
            {
                #region workflow
                if(model.ProcessOID == null)
                {
                    //inititate workflow
                    var variable = new Dictionary<string, object>();
                    variable.Add("status", model.Action);
                    variable.Add("remark", model.Remarks);
                    variable.Add("getUserWhereClause", "1=1 AND HCISPosition like ''%Account Receivable Database Department Head%'' AND Position = (SELECT top 1 Position from dbo.vwARUserRole u2 where u2.UserID = dbo.vwARUserRole.UserID)");

                    var wf = workflow.InitiateWorkflow(UserID, false, "1430_CashInForecastARD", "proc_ForecastCashInARD", variable);
                    model.ProcessOID = wf.processOID;
                    //var varApprovalSectionHead = new Dictionary<string, object>();
                    //varApprovalSectionHead.Add("status", model.Action);
                    //varApprovalSectionHead.Add("remark", model.Remarks);
                    //varApprovalSectionHead.Add("getUserWhereClause", "1=1 AND HCISPosition like ''%Account Receivable Database Department Head%'' AND Position = (SELECT top 1 Position from dbo.vwARUserRole u2 where u2.UserID = dbo.vwARUserRole.UserID)");
                    //var wfSectionHead = workflow.AssignedWorkflow(UserID, true, null, wf.processOID, varApprovalSectionHead);
                    //model.ProcessOID = wf.processOID;
                    //result = _trxARCastInForecastApprovalLogRepository.Create(model);
                }else
                {
                    var varApprovalSectionHead = new Dictionary<string, object>();
                    varApprovalSectionHead.Add("status", model.Action);
                    varApprovalSectionHead.Add("remark", model.Remarks);
                    if(model.ApprovalType == "act_DeptHeadApproval")
                    {
                        varApprovalSectionHead.Add("getUserWhereClause", "1=1 AND HCISPosition like ''%Account Receivable Database Department Head%'' AND Position = (SELECT top 1 Position from dbo.vwARUserRole u2 where u2.UserID = dbo.vwARUserRole.UserID)");
                    }
                    else if (model.ApprovalType == "act_approvalSectionHead")
                    {
                        varApprovalSectionHead.Add("getUserWhereClause", "1=1 AND HCISPosition like ''%Account Receivable Database Section Head%'' AND Position = (SELECT top 1 Position from dbo.vwARUserRole u2 where u2.UserID = dbo.vwARUserRole.UserID)");
                    }
                    var wfSectionHead = workflow.AssignedWorkflow(UserID, true, null, model.ProcessOID, varApprovalSectionHead);

                }

                #endregion


                result = _trxARCastInForecastApprovalLogRepository.Create(model);

                return result;
            }
            catch (Exception ex)
            {
                result  = new trxARCashInForecastApprovalLog((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID));
                return result;
            }
            finally
            {
            }
        }

        public List<vwARUserRole> GetUserByPosition(string UserID, string positionID)
        {
            List<vwARUserRole> result = new List<vwARUserRole>();
            try
            {
                var context = new DbContext(Helper.GetConnection("ARSystem"));
                var repo = new vwARUserRoleRepository(context);

                var data = repo.GetList("HCISPosition like '%" + positionID + "%'", "");
                result = data.Select(m => new vwARUserRole()
                {
                    UserID = m.UserID,
                    UserName = m.UserName,
                    HCISPosition = m.HCISPosition
                }).GroupBy(c => new { c.UserID, c.UserName, c.HCISPosition })
                     .Select(c => c.First()).ToList();
                return result;
            }
            catch (Exception ex)
            {
                result.Add(new vwARUserRole((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxBapsDataService", "GettrxBapsDataCount", UserID)));
                return result;
            }
        }
        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }
    }
}
