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
    public class TrxARRMonitoringCashInRemarkQuarterlyService : BaseService
    {
        private DbContext _context;
        private DateTime _dtNow;
        private TrxARRMonitoringCashInRemarkDetailQuarterlyRepository _trxDetailRepo;
        public TrxARRMonitoringCashInRemarkQuarterlyService() : base()
        {
            _context = this.SetContext();
            _dtNow = Helper.GetDateTimeNow();
            _trxDetailRepo = new TrxARRMonitoringCashInRemarkDetailQuarterlyRepository(_context);
        }

        public static string ServiceName = "TrxARRMonitoringCashInRemarkQuarterlyService";
        #region list - Requestor
        public List<TrxARRMonitoringCashInRemarkDetailQuarterly> GetDataToList(string UserID, 
            int Periode = 0, int Quarter = 0, string strOrderBy = " ", int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailQuarterlyRepository(context);
            List<TrxARRMonitoringCashInRemarkDetailQuarterly> list = new List<TrxARRMonitoringCashInRemarkDetailQuarterly>();

            try
            {
                string strWhereClause = "1=1 AND (Status IS NULL OR Status = 'Rejected') ";
                if (Periode != 0)
                {
                    strWhereClause += " AND Periode = " + Periode;
                }
                if (Quarter != 0)
                {
                    strWhereClause += " AND Quarter = " + Quarter;
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
                list.Add(new TrxARRMonitoringCashInRemarkDetailQuarterly((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetDataCount(string UserID, int Periode = 0, int Quarter = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailQuarterlyRepository(context);

            try
            {
                string strWhereClause = "1=1 AND (Status IS NULL OR Status = 'Rejected') ";
                if (Periode != 0)
                {
                    strWhereClause += " AND Periode = " + Periode;
                }
                if (Quarter != 0)
                {
                    strWhereClause += " AND Quarter = " + Quarter;
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
        public TrxARRMonitoringCashInRemarkDetailQuarterly EditRemark(string UserID, TrxARRMonitoringCashInRemarkDetailQuarterly Input)
        {
            var uow = _context.CreateUnitOfWork();
            var TrxBapsDataRepo = new trxBapsDataRepository(_context);
            try
            {
                TrxARRMonitoringCashInRemarkDetailQuarterly trx = _trxDetailRepo.GetByPK
                    (Input.OperatorID, Input.Periode, Input.Quarter);
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
                return new TrxARRMonitoringCashInRemarkDetailQuarterly((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                    MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), UserID));
            }
        }
        #endregion

        #region Trx : Submit
        public DataTable Submit(string Type, int Year, int Quarter,  string user)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new FilterARRMonitoringCashInRemarkRepositoryCustoms(context);
            DataTable dt = new DataTable();
            try
            {
                dt = Repo.SubmitQuarter(Type, Year, Quarter, user);
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
        public List<TrxARRMonitoringCashInRemarkDetailQuarterly> GetDataToListApproval(string UserID, 
            int Periode = 0, int Quarter = 0,  string strOrderBy = " ", int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailQuarterlyRepository(context);
            List<TrxARRMonitoringCashInRemarkDetailQuarterly> list = new List<TrxARRMonitoringCashInRemarkDetailQuarterly>();

            try
            {
                string strWhereClause = "1=1 AND Status = 'Submitted' ";
                if (Periode != 0)
                {
                    strWhereClause += " AND Periode = " + Periode;
                }
                if (Quarter != 0)
                {
                    strWhereClause += " AND Quarter = " + Quarter;
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
                list.Add(new TrxARRMonitoringCashInRemarkDetailQuarterly((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetDataCountApproval(string UserID, int Periode = 0, int Quarter = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new TrxARRMonitoringCashInRemarkDetailQuarterlyRepository(context);

            try
            {
                string strWhereClause = "1=1 AND Status = 'Submitted' ";
                if (Periode != 0)
                {
                    strWhereClause += " AND Periode = " + Periode;
                }
                if (Quarter != 0)
                {
                    strWhereClause += " AND Quarter = " + Quarter;
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
                dt = Repo.ApproveQuarter(Type, Year, Month, Userid, Isapproval, Remark);
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
        public DataTable GetFilterList(string Type, int Year, int Month, string user)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new FilterARRMonitoringCashInRemarkRepositoryCustoms(context);
            DataTable dt = new DataTable();
            try
            {
                dt = Repo.GetFilterList(Type, Year, Month,0);
                return Repo.GetFilterList(Type, Year, Month,0);
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
