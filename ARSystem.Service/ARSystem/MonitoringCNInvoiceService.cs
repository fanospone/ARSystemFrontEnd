using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Models.ViewModels;
using ARSystem.Domain.Repositories.HTBGDWH01.TBGARSystem;
using ARSystem.Domain.Models.HTBGDWH01.TBGARSystem;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories.Repositories.RepositoryCustoms;

namespace ARSystem.Service.ARSystem
{
    public class MonitoringCNInvoiceService : BaseService
    {
        //protected readonly DbContext _context;
        protected readonly DbContext context;
        protected readonly DateTime _dtNow;
        private readonly vwDashBoardMonitoringCNInvoiceRepository _vwMonitoringCNRepo;
        private readonly vwDashBoardMonitoringCNInvoiceDetailRepository _vwMonitoringCNDetailRepo;
        private readonly uspvwMonitoringCNInvoiceCustomRepository _vwMonitoringCNCustomRepo;

        public MonitoringCNInvoiceService()
        {
            //_context = this.SetContext();
            //context = new DbContext(Helper.GetConnection("ARSystem"));
            //context = new DbContext(Helper.GetConnection("HTBGDWH01_TBGARSystemWH"));
            context = new DbContext(Helper.GetConnection("ARSystemDWH"));

            _dtNow = Helper.GetDateTimeNow();

            _vwMonitoringCNRepo = new vwDashBoardMonitoringCNInvoiceRepository(context);
            _vwMonitoringCNDetailRepo = new vwDashBoardMonitoringCNInvoiceDetailRepository(context);
            _vwMonitoringCNCustomRepo = new uspvwMonitoringCNInvoiceCustomRepository(context);
        }

        public int GetMonitoringCNInvoiceListCount(string token, vmUserCredential cred, string strWhereClause)
        {
            List<vwDashBoardMonitoringCNInvoice> dataList = new List<vwDashBoardMonitoringCNInvoice>();

            try
            {
                //return _vwMonitoringCNRepo.GetCount(strWhereClause);
                return _vwMonitoringCNCustomRepo.GetCountMonitoring(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString() + ";" + ex.StackTrace.ToString(),
                 MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), cred.UserID);
                return 0;
            }
            finally
            {
                this.Dispose();
            }
        }

        public int GetMonitoringCNInvoiceListDetailCount(string token, vmUserCredential cred, string strWhereClause)
        {
            List<vwDashBoardMonitoringCNInvoiceDetail> dataList = new List<vwDashBoardMonitoringCNInvoiceDetail>();

            try
            {
                return _vwMonitoringCNDetailRepo.GetCount(strWhereClause);
                
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString() + ";" + ex.StackTrace.ToString(),
                 MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), cred.UserID);
                return 0;
            }
            finally
            {
                this.Dispose();
            }
        }

        public List<vwDashBoardMonitoringCNInvoice> GetMonitoringCNInvoiceList(string token, vmUserCredential cred, /*string CustomerID, string CompanyID,*/ string strWhereClause, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            List<vwDashBoardMonitoringCNInvoice> dataList = new List<vwDashBoardMonitoringCNInvoice>();

            if (cred.ErrorType > 0)
            {
                dataList.Add(new vwDashBoardMonitoringCNInvoice(cred.ErrorType, cred.ErrorMessage));
                return dataList;
            }

            try
            {
                if (intPageSize > 0)
                {
                    //return _vwMonitoringCNRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                    return _vwMonitoringCNCustomRepo.GetPagedMonitoring(/*CustomerID, CompanyID,*/ strWhereClause, strOrderBy, intRowSkip, intPageSize);
                }
                //return _vwMonitoringCNRepo.GetList(strWhereClause, strOrderBy);
                return _vwMonitoringCNCustomRepo.GetListMonitoring(/*CustomerID, CompanyID,*/ strWhereClause, strOrderBy);
            }
            catch (Exception ex)
            {
                dataList.Add(new vwDashBoardMonitoringCNInvoice((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                 MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), cred.UserID)));
                return dataList;
            }
            finally
            {
                this.Dispose();
            }
        }

        public List<vwDashBoardMonitoringCNInvoiceDetail> GetMonitoringCNInvoiceListDetail(string token, vmUserCredential cred, string strWhereClause, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            List<vwDashBoardMonitoringCNInvoiceDetail> dataList = new List<vwDashBoardMonitoringCNInvoiceDetail>();

            if (cred.ErrorType > 0)
            {
                dataList.Add(new vwDashBoardMonitoringCNInvoiceDetail(cred.ErrorType, cred.ErrorMessage));
                return dataList;
            }

            try
            {
                if (intPageSize > 0)
                {
                    return _vwMonitoringCNDetailRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                }
                return _vwMonitoringCNDetailRepo.GetList(strWhereClause, strOrderBy);
            }
            catch (Exception ex)
            {
                dataList.Add(new vwDashBoardMonitoringCNInvoiceDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(),
                 MethodBase.GetCurrentMethod().DeclaringType.ToString(), Helper.GetCurrentMethod(), cred.UserID)));
                return dataList;
            }
            finally
            {
                this.Dispose();
            }
        }
    }
}
