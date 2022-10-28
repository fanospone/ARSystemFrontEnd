using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.Models.Models;

namespace ARSystem.Service
{
    public class DashboardInputTargetService : IDisposable
    {
        private DbContext _context;
        private vwDashboardInputTargetRepository _dashboardRepository;
        private vwDashboardInputTargetDetailRepository _vwDashboardInputTargetDetailRepository;
        private trxRAInputTargetPercentageRepository _trxRAInputTargetPercentageRepository;
        
        public DashboardInputTargetService()
        {
            _context = new DbContext(Helper.GetConnection("ARSystem"));
            _dashboardRepository = new vwDashboardInputTargetRepository(_context);
            _vwDashboardInputTargetDetailRepository = new vwDashboardInputTargetDetailRepository(_context);
            _trxRAInputTargetPercentageRepository = new trxRAInputTargetPercentageRepository(_context);

        }

        public List<vwDashboardInputTarget> GetInputTargetDashboard(string strToken, string strCompanyId, string strCustomerId, int year)
        {
            List<vwDashboardInputTarget> listData = new List<vwDashboardInputTarget>();

            try
            {
                string whereClause = " 1 = 1 ";
                if(year > 0)
                {
                    whereClause += " AND Year = " + year;
                }
                if (!string.IsNullOrWhiteSpace(strCompanyId))
                {
                    whereClause += " AND CompanyInvoiceID = '" + strCompanyId + "'";
                }
                if (!string.IsNullOrWhiteSpace(strCustomerId))
                {
                    whereClause += " AND CustomerID = '" + strCustomerId + "'";
                }
                return _dashboardRepository.GetList(whereClause, year);
            }
            catch (Exception ex)
            {
                listData.Add(new vwDashboardInputTarget((int)Helper.ErrorType.Error, ex.Message.ToString()));
                return listData;
            }
            finally
            {
            }
        }

        public int GetCountInputTargetDetail(string strToken, vwDashboardInputTargetDetail filter)
        {
            List<vwDashboardInputTargetDetail> listDetailData = new List<vwDashboardInputTargetDetail>();

            try
            {
                string whereClause = String.Empty;
                if (!string.IsNullOrWhiteSpace(filter.CompanyInvoiceID))
                {
                    whereClause += String.Format(" CompanyInvoiceID = '{0}' AND ", filter.CompanyInvoiceID);
                }
                if (!string.IsNullOrWhiteSpace(filter.CustomerID))
                {
                    whereClause += String.Format(" CustomerID = '{0}' AND ", filter.CustomerID);
                }
                if (!string.IsNullOrWhiteSpace(filter.DepartmentCode))
                {
                    if(filter.DepartmentCode != "Total")
                    {
                       whereClause += String.Format(" DepartmentCode = '{0}' AND ", filter.DepartmentCode);
                    }
                }
                if (filter.Year != null)
                {
                    whereClause += String.Format(" Year = {0} AND ", filter.Year);
                }
                if (filter.Month != null)
                {
                    whereClause += String.Format(" Month = {0} AND ", filter.Month);
                }


                whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
                return _vwDashboardInputTargetDetailRepository.GetCount(filter.Year.GetValueOrDefault(0), whereClause);
            }
            catch (Exception ex)
            {
                listDetailData.Add(new vwDashboardInputTargetDetail((int)Helper.ErrorType.Error, ex.Message.ToString()));
                return 0;
            }
        }

        public List<vwDashboardInputTargetDetail> GetListInputTargetDetail(string strToken, vwDashboardInputTargetDetail filter, string orderBy, int rowSkip = 0, int pageSize = 0)
        {
            List<vwDashboardInputTargetDetail> listDetailData = new List<vwDashboardInputTargetDetail>();

            try
            {
                string whereClause = String.Empty;
                if (!string.IsNullOrWhiteSpace(filter.CompanyInvoiceID))
                {
                    whereClause += String.Format(" CompanyInvoiceID = '{0}' AND ", filter.CompanyInvoiceID);
                }
                if (!string.IsNullOrWhiteSpace(filter.CustomerID))
                {
                    whereClause += String.Format(" CustomerID = '{0}' AND ", filter.CustomerID);
                }
                if (!string.IsNullOrWhiteSpace(filter.DepartmentCode))
                {
                    if (filter.DepartmentCode != "Total")
                    {
                        whereClause += String.Format(" DepartmentCode = '{0}' AND ", filter.DepartmentCode);
                    }
                }
                if (filter.Year != null)
                {
                    whereClause += String.Format(" Year = {0} AND ", filter.Year);
                }
                if (filter.Month != null)
                {
                    whereClause += String.Format(" Month = {0} AND ", filter.Month);
                }
                whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
                if (pageSize > 0)
                    listDetailData = _vwDashboardInputTargetDetailRepository.GetPaged(filter.Year.GetValueOrDefault(0), whereClause, orderBy, rowSkip, pageSize);
                else
                    listDetailData = _vwDashboardInputTargetDetailRepository.GetPaged(filter.Year.GetValueOrDefault(0), whereClause, orderBy, 0, 0);

                return listDetailData;
            }
            catch (Exception ex)
            {
                listDetailData.Add(new vwDashboardInputTargetDetail((int)Helper.ErrorType.Error, ex.Message.ToString()));
                return listDetailData;

            }
        }

        public trxRAInputTargetPercentage SavetrxRAInputTargetPercentage(string UserID, trxRAInputTargetPercentage forecast)
        {
            trxRAInputTargetPercentage result = new trxRAInputTargetPercentage();
            try
            {
                if(forecast.ID > 0)
                {
                    forecast.UpdatedBy = UserID;
                    forecast.UpdatedDate = DateTime.Now;

                    result = _trxRAInputTargetPercentageRepository.Update(forecast);
                }
                else
                {
                    forecast.CreatedBy = UserID;
                    forecast.UpdatedDate = DateTime.Now;

                    result = _trxRAInputTargetPercentageRepository.Create(forecast);
                }
                return result;
            }
            catch (Exception ex)
            {
                result = (new trxRAInputTargetPercentage((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InputTargetService", "UpdateInputForecastVsActual", UserID)));
                return result;
            }

        }

        public trxRAInputTargetPercentage FindtrxRAInputTargetPercentage(string UserID, trxRAInputTargetPercentage model)
        {
            var result = new trxRAInputTargetPercentage();
            try
            {
                string whereClause = String.Empty;
                if (model.Year > 0)
                {
                    whereClause += String.Format(" Year = {0} AND ", model.Year);
                }
                if (model.Month > 0)
                {
                    whereClause += String.Format(" Month = {0} AND ", model.Month);
                }
                if (!string.IsNullOrWhiteSpace(model.Department))
                {
                    whereClause += String.Format(" Department = '{0}' AND ", model.Department);
                }
                whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";

                var trxRAInputTargetPercentage = _trxRAInputTargetPercentageRepository.FindByFilter(whereClause);
                return trxRAInputTargetPercentage;
            }
            catch (Exception ex)
            {
                result = new trxRAInputTargetPercentage((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InputTargetService", "GetHistoryNewBapsInputTargetByID", UserID));
                return result;
            }
        }

        public void Dispose()
        {
            if(_context != null)
            {
                _context.Dispose();
            }

        }
    }
}
