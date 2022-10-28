using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Data;

namespace ARSystem.Service
{
    public class DashboardTSELOverdueService
    {
        public List<vmDashboardTSELOverdue> GetDataTSELOverduePercentage(string Year, string Type)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new DashboardTSELOverdueRepository(context);
            List<vmDashboardTSELOverdue> ResultList = new List<vmDashboardTSELOverdue>();
            try
            {
                return Repo.GetDataTSELOverduePercentage(Type, Year);
            }
            catch (Exception ex)
            {
                //context.Dispose();
                //return 0;
                ResultList.Add(new vmDashboardTSELOverdue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELOverdueService", "GetDataTSELOverduePercentage", "")));
                return ResultList;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmDashboardTSELOverdue> GetDataTSELOverdueChart(string Year, string Type)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new DashboardTSELOverdueRepository(context);
            List<vmDashboardTSELOverdue> ResultList = new List<vmDashboardTSELOverdue>();
            
            try
            {
                return Repo.GetDataTSELOverdueChartList(Type, Year);
            }
            catch (Exception ex)
            {
                ResultList.Add(new vmDashboardTSELOverdue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELOverdueService", "GetDataTSELOverdueChart", "")));
                return ResultList;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmDashboardTSELOverdue> GetMasterSTIPListDropdown()
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new DashboardTSELOverdueRepository(context);
            List<vmDashboardTSELOverdue> ResultList = new List<vmDashboardTSELOverdue>();
            try
            {
                return Repo.GetMasterSTIPList();
            }
            catch (Exception ex)
            {
               
                ResultList.Add(new vmDashboardTSELOverdue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELOverdueService", "GetMasterSTIPListDropdown", "")));
                return ResultList;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmDashboardTSELOverdue> GetMasterCompanyListDropdown()
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new DashboardTSELOverdueRepository(context);
            List<vmDashboardTSELOverdue> ResultList = new List<vmDashboardTSELOverdue>();
            try
            {
                //ResultList = Repo.GetMasterCompanyList(); 
                return Repo.GetMasterCompanyList();
            }
            catch (Exception ex)
            {

                ResultList.Add(new vmDashboardTSELOverdue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELOverdueService", "GetMasterCompanyListDropdown", "")));
                return ResultList;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmDashboardTSELOverdue> GetProductListDropdown(string sectionID, string sowID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new DashboardTSELOverdueRepository(context);
            List<vmDashboardTSELOverdue> ResultList = new List<vmDashboardTSELOverdue>();
            try
            {
                return Repo.GetProductList(sectionID,sowID);
            }
            catch (Exception ex)
            {

                ResultList.Add(new vmDashboardTSELOverdue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELOverdueService", "GetProductListDropdown", "")));
                return ResultList;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vmDashboardTSELOverdue> GetSOWListDropdown(string sectionID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new DashboardTSELOverdueRepository(context);
            List<vmDashboardTSELOverdue> ResultList = new List<vmDashboardTSELOverdue>();
            try
            {
                return Repo.GetSOWList(sectionID);
            }
            catch (Exception ex)
            {

                ResultList.Add(new vmDashboardTSELOverdue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardTSELOverdueService", "GetSOWListDropdown", "")));
                return ResultList;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
