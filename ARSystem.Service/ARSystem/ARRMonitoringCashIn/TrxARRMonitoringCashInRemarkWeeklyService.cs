using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Data;
using System.Reflection;


namespace ARSystem.Service
{
    public class TrxARRMonitoringCashInRemarkWeeklyService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;
        private TrxARRMonitoringCashInRemarkDetailWeeklyRepository _trxDetailRepo;
        public TrxARRMonitoringCashInRemarkWeeklyService() : base()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();
            _trxDetailRepo = new TrxARRMonitoringCashInRemarkDetailWeeklyRepository(_context);
        }

        public static string ServiceName = "TrxARRMonitoringCashInRemarkMonthlyService";
        #region list - Requestor
        public List<TrxARRMonitoringCashInRemarkDetailWeekly> GetDataToList(string UserID, int Periode = 0, int Month = 0, int Week = 0, string strOrderBy = " ", int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailWeeklyRepository(context);
            List<TrxARRMonitoringCashInRemarkDetailWeekly> list = new List<TrxARRMonitoringCashInRemarkDetailWeekly>();

            try
            {
                string strWhereClause = "1=1 AND (Status IS NULL OR Status = 'Rejected') ";
                if (Periode != 0)
                {
                    strWhereClause += " AND Periode = " + Periode;
                }
                if (Month != 0)
                {
                    strWhereClause += " AND Month = " + Month;
                }
                if (Week != 0)
                {
                    strWhereClause += " AND Week = " + Week;
                }
                if (string.IsNullOrWhiteSpace(strOrderBy))
                {
                    strOrderBy = "CreatedDate DESC";

                }

                if (intPageSize > 0)
                    list = Repo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    list = Repo.GetList(strWhereClause, strOrderBy);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new TrxARRMonitoringCashInRemarkDetailWeekly((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetDataCount(string UserID, int Periode = 0, int Month = 0, int Week=0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailWeeklyRepository(context);

            try
            {
                string strWhereClause = "1=1 AND (Status IS NULL OR Status = 'Rejected') ";
                if (Periode != 0)
                {
                    strWhereClause += " AND Periode = " + Periode;
                }
                if (Month != 0)
                {
                    strWhereClause += " AND Month = " + Month;
                }
                if (Week != 0)
                {
                    strWhereClause += " AND Week = " + Week;
                }
                return Repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        #endregion

        #region EdiT Remark                
        public TrxARRMonitoringCashInRemarkDetailWeekly EditRemark(string UserID, TrxARRMonitoringCashInRemarkDetailWeekly Input)
        {
            var uow = _context.CreateUnitOfWork();
            var TrxBapsDataRepo = new trxBapsDataRepository(_context);
            try
            {
                TrxARRMonitoringCashInRemarkDetailWeekly trx = _trxDetailRepo.GetByPK
                    (Input.OperatorID, Input.Periode, Input.Month, Input.Week);
                trx.Remarks = Input.Remarks;
                trx.UpdatedBy = UserID;
                trx.UpdatedDate = Helper.GetDateTimeNow();
                _trxDetailRepo.Update(trx);
                uow.SaveChanges();
                return trx;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new TrxARRMonitoringCashInRemarkDetailWeekly((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID));
            }
        }
        #endregion

        #region Trx : Submit
        public DataTable Submit(string Type, int Year, int Month, int Week, string user)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new FilterARRMonitoringCashInRemarkRepositoryCustoms(context);
            DataTable dt = new DataTable();
            try
            {
                dt = Repo.SubmitWeek(Type, Year, Month, Week, user);
                return dt;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), user);
                return dt;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion

        #region Approval
        #region list - Approval
        public List<TrxARRMonitoringCashInRemarkDetailWeekly> GetDataToListApproval(string UserID, int Periode = 0, int Month = 0, int Week = 0, string strOrderBy = " ", int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailWeeklyRepository(context);
            List<TrxARRMonitoringCashInRemarkDetailWeekly> list = new List<TrxARRMonitoringCashInRemarkDetailWeekly>();

            try
            {
                string strWhereClause = "1=1 AND Status = 'Submitted' ";
                if (Periode != 0)
                {
                    strWhereClause += " AND Periode = " + Periode;
                }
                if (Month != 0)
                {
                    strWhereClause += " AND Month = " + Month;
                }
                if (Week != 0)
                {
                    strWhereClause += " AND Week = " + Week;
                }
                if (string.IsNullOrWhiteSpace(strOrderBy))
                {
                    strOrderBy = "CreatedDate DESC";

                }

                if (intPageSize > 0)
                    list = Repo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    list = Repo.GetList(strWhereClause, strOrderBy);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new TrxARRMonitoringCashInRemarkDetailWeekly((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetDataCountApproval(string UserID, int Periode = 0, int Month = 0, int Week=0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailWeeklyRepository(context);

            try
            {
                string strWhereClause = "1=1 AND Status = 'Submitted' ";
                if (Periode != 0)
                {
                    strWhereClause += " AND Periode = " + Periode;
                }
                if (Month != 0)
                {
                    strWhereClause += " AND Month = " + Month;
                }
                if (Week != 0)
                {
                    strWhereClause += " AND Week = " + Week;
                }

                return Repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        #endregion
        #region Action

        public DataTable Approve(string Type, int Year, int Month, int Week, string Userid, int Isapproval, string Remark)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new FilterARRMonitoringCashInRemarkRepositoryCustoms(context);
            DataTable dt = new DataTable();
            try
            {
                dt = Repo.ApproveWeek(Type, Year, Month, Week, Userid, Isapproval, Remark);
                return dt;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), Userid);
                return dt;
            }
            finally
            {
                context.Dispose();
            }
        }
        #region Filters
        public DataTable GetFilterList(string Type, int Year, int Month, string user, int week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new FilterARRMonitoringCashInRemarkRepositoryCustoms(context);
            DataTable dt = new DataTable();
            try
            {
                dt = Repo.GetFilterList(Type, Year, Month, week);
                return Repo.GetFilterList(Type, Year, Month, week);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), user);
                return dt;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion

        #endregion
        #endregion
    }
}
