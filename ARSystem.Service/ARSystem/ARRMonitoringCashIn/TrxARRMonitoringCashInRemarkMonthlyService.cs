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
    public class TrxARRMonitoringCashInRemarkMonthlyService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;
        private TrxARRMonitoringCashInRemarkDetailMonthlyRepository _trxDetailRepo;
        public TrxARRMonitoringCashInRemarkMonthlyService() : base()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();
            _trxDetailRepo = new TrxARRMonitoringCashInRemarkDetailMonthlyRepository(_context);            
        }

        public static string ServiceName = "TrxARRMonitoringCashInRemarkMonthlyService";

        #region list - Requestor
        public List<TrxARRMonitoringCashInRemarkDetailMonthly> GetDataToList(string UserID, int Periode = 0, int Month = 0, string strOrderBy=" " , int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailMonthlyRepository(context);
            List<TrxARRMonitoringCashInRemarkDetailMonthly> list = new List<TrxARRMonitoringCashInRemarkDetailMonthly>();

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
                list.Add(new TrxARRMonitoringCashInRemarkDetailMonthly((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetDataCount(string UserID, int Periode = 0, int Month = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailMonthlyRepository(context);
          
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
        public TrxARRMonitoringCashInRemarkDetailMonthly EditRemark(string UserID, TrxARRMonitoringCashInRemarkDetailMonthly Input)
        {
            var uow = _context.CreateUnitOfWork();
            var TrxBapsDataRepo = new trxBapsDataRepository(_context);
            try
            {
                TrxARRMonitoringCashInRemarkDetailMonthly trx = _trxDetailRepo.GetByPK
                    (Input.OperatorID, Input.Periode, Input.Month);
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
                return new TrxARRMonitoringCashInRemarkDetailMonthly((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID));
            }
        }
        #endregion

        #region Trx : Submit
        public DataTable Submit(string Type, int Year, int Month, string user)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new FilterARRMonitoringCashInRemarkRepositoryCustoms(context);
            DataTable dt = new DataTable();
            try
            {
                dt = Repo.SubmitMonth(Type, Year, Month, user);
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
        public List<TrxARRMonitoringCashInRemarkDetailMonthly> GetDataToListApproval(string UserID, int Periode = 0, int Month = 0, string strOrderBy = " ", int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailMonthlyRepository(context);
            List<TrxARRMonitoringCashInRemarkDetailMonthly> list = new List<TrxARRMonitoringCashInRemarkDetailMonthly>();

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
                list.Add(new TrxARRMonitoringCashInRemarkDetailMonthly((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetDataCountApproval(string UserID, int Periode = 0, int Month = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailMonthlyRepository(context);

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

        public DataTable Approve(string Type, int Year, int Month, string Userid, int Isapproval, string Remark)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new FilterARRMonitoringCashInRemarkRepositoryCustoms(context);
            DataTable dt = new DataTable();
            try
            {
                dt= Repo.ApproveMonth(Type, Year, Month, Userid, Isapproval, Remark);
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

        #endregion
        #endregion


        #region Filters
        public DataTable GetFilterList(string Type, int Year, int Month, string user)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new FilterARRMonitoringCashInRemarkRepositoryCustoms(context);
            DataTable dt = new DataTable();
            try
            {
                dt = Repo.GetFilterList(Type, Year, Month, 0);
                return Repo.GetFilterList(Type, Year, Month, 0);
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
    }
}
