using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.Models.ViewModels;
using System.Configuration;

namespace ARSystem.Service
{
    public class BapsDoneService
    {
        public async Task<List<vwRABapsDone>> BapsDoneList(vwRABapsDone param, int RowSkip=0, int PageSize=0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRABapsDoneRepository(context);
            var result = new List<vwRABapsDone>();
            try
            {
                string whereClause = BapsDoneWhereClause(param);
                if(PageSize == 0)
                {
                    result = repo.GetList(whereClause, "");
                }
                else
                {
                    result = repo.GetPaged(whereClause, "", RowSkip,PageSize);
                }
                

                return result;
            }
            catch (Exception ex)
            {
                result.Add(new vwRABapsDone
                {
                    ErrorMessage = Helper.logError(ex.Message.ToString(), "BapsDoneService", "BapsDoneList", ""),
                    ErrorType = (int)Helper.ErrorType.Error
                });
                return result;
            }
            finally
            {
                context.Dispose();
            }
        }

        public async Task<List<vwRABapsDone>> BapsDoneTreeViewList(vwRABapsDone param, int RowSkip = 0, int PageSize = 0, TableViewTypes type = TableViewTypes.TREE)
        {
            //var data = ConfigurationManager.ConnectionStrings["TBIGSYSDB01_DWHARSystem"].ConnectionString;

            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            //var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRABapsDoneRepository(context);
            var result = new List<vwRABapsDone>();
            try
            {
                string whereClause = BapsDoneGridViewWhereClause(param);

                if(type == TableViewTypes.TREE)
                {
                    whereClause = BapsDoneTreeViewWhereClause(param);
                    result = repo.GetTreeViewList(whereClause, param.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss"), param.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss").Replace("00:00:00","23:59:59"), param.TowerTypeID, "bapasData.DoneDate DESC");
                }
                else if(type == TableViewTypes.TREE_ARCHIVE)
                {
                    whereClause = BapsDoneTreeViewWhereClause(param);
                    result = repo.GetTreeViewList(whereClause, param.DoneDate.Value.ToString("yyyy-MM-dd 00:00:00"), param.DoneDate.Value.ToString("yyyy-MM-dd 23:59:59"), param.TowerTypeID, "bapasData.DoneDate DESC");
                }
                else if (type == TableViewTypes.COMPRESSED_ARCHIVE)
                {
                    whereClause = BapsDoneTreeViewWhereClause(param);
                    result = repo.GetGridViewList(whereClause, param.StartDate.Value.ToString("yyyy-MM-dd 00:00:00"), param.EndDate.Value.ToString("yyyy-MM-dd 23:59:59"), param.TowerTypeID, "bapasData.DoneDate DESC");
                }
                else
                {
                    if (PageSize == 0)
                    {
                        result = repo.GetGridViewList(whereClause, param.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss"), param.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss"), param.TowerTypeID, "bapasData.DoneDate ASC");
                    }
                    else
                    {
                        result = repo.GetGridViewPaged(whereClause, param.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss"), param.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss"), param.TowerTypeID, "bapasData.DoneDate DESC", RowSkip, PageSize);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Add(new vwRABapsDone
                {
                    ErrorMessage = Helper.logError(ex.Message.ToString(), "BapsDoneService", "BapsDoneList", ""),
                    ErrorType = (int)Helper.ErrorType.Error
                });
                return result;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int BapsDoneCount(vwRABapsDone param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRABapsDoneRepository(context);
            try
            {
                string whereClause = BapsDoneWhereClause(param);
                return repo.GetCount(whereClause);
            }
            catch
            {
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int BapsDoneTreeViewCount(vwRABapsDone param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwRABapsDoneRepository(context);
            try
            {
                string whereClause = BapsDoneTreeViewWhereClause(param);
                return repo.GetCountView(whereClause, param.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss"), param.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss"), param.TowerTypeID);
            }
            catch
            {
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        private string BapsDoneWhereClause(vwRABapsDone postData)
        {
            string strWhereClause = "";

            if (!string.IsNullOrWhiteSpace(postData.SoNumber))
            {
                strWhereClause += "SoNumber = '" + postData.SoNumber.TrimStart().TrimEnd() + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.CustomerID))
            {
                strWhereClause += "CustomerID = '" + postData.CustomerID.TrimStart().TrimEnd() + "' AND ";
            }
            if (postData.ProductID != 0)
            {
                strWhereClause += "ProductID = '" + postData.ProductID + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.CompanyID))
            {
                strWhereClause += "CompanyID = '" + postData.CompanyID.TrimStart().TrimEnd() + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.SiteID))
            {
                strWhereClause += "SiteID = '" + postData.SiteID.TrimStart().TrimEnd() + "' AND ";
            }
            strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
            return strWhereClause;
        }

        private string BapsDoneTreeViewWhereClause(vwRABapsDone postData)
        {
            string strWhereClause = "";

            if (!string.IsNullOrWhiteSpace(postData.SoNumber))
            {
                strWhereClause += "SoNumber = '" + postData.SoNumber.TrimStart().TrimEnd() + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.CustomerID))
            {
                strWhereClause += "CustomerID = '" + postData.CustomerID.TrimStart().TrimEnd() + "' AND ";
            }
            if (postData.ProductID != 0)
            {
                strWhereClause += "ProductID = '" + postData.ProductID + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.CompanyID))
            {
                strWhereClause += "CompanyID = '" + postData.CompanyID.TrimStart().TrimEnd() + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.SiteID))
            {
                strWhereClause += "SiteID = '" + postData.SiteID.TrimStart().TrimEnd() + "' AND ";
            }

            if (!string.IsNullOrEmpty(postData.TowerTypeID))
            {
                strWhereClause += "TowerTypeID = '" + postData.TowerTypeID + "' AND ";
            }

            if (postData.DoneDate.HasValue)
            {
                strWhereClause += $"bapasData.DoneDate >= '{postData.DoneDate.Value.ToString("yyyy-MM-dd 00:00:00")}' AND bapasData.DoneDate <= '{postData.DoneDate.Value.ToString("yyyy-MM-dd 23:59:59")}'" + "  AND ";
            }

            if (postData.StartDate.HasValue && postData.EndDate.HasValue)
            {
                strWhereClause += $"bapasData.DoneDate >= '{postData.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}' AND bapasData.DoneDate <= '{postData.EndDate.Value.ToString("yyyy-MM-dd 23:59:59")}'" + "  AND ";
            }

            strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
            return strWhereClause;
        }

        private string BapsDoneGridViewWhereClause(vwRABapsDone postData)
        {
            string strWhereClause = "";

            if (!string.IsNullOrWhiteSpace(postData.SoNumber))
            {
                strWhereClause += "SoNumber = '" + postData.SoNumber.TrimStart().TrimEnd() + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.CustomerID))
            {
                strWhereClause += "CustomerID = '" + postData.CustomerID.TrimStart().TrimEnd() + "' AND ";
            }
            if (postData.ProductID != 0)
            {
                strWhereClause += "ProductID = '" + postData.ProductID + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.CompanyID))
            {
                strWhereClause += "CompanyID = '" + postData.CompanyID.TrimStart().TrimEnd() + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(postData.SiteID))
            {
                strWhereClause += "SiteID = '" + postData.SiteID.TrimStart().TrimEnd() + "' AND ";
            }

            if (!string.IsNullOrEmpty(postData.TowerTypeID))
            {
                strWhereClause += "TowerTypeID = '" + postData.TowerTypeID + "' AND ";
            }

            if (postData.DoneDate.HasValue)
            {
                strWhereClause += $"bapasData.DoneDate >= '{postData.DoneDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" + "  AND ";
            }

            if (postData.StartDate.HasValue && postData.EndDate.HasValue)
            {
                strWhereClause += $"bapasData.DoneDate >= '{postData.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}' AND bapasData.DoneDate <= '{postData.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss").Replace("00:00:00", "23:59:59")}'" + "  AND ";
            }

            strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
            return strWhereClause;
        }

    }

}
