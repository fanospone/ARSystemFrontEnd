using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

using System.Threading.Tasks;

namespace ARSystem.Service
{
    public class DynamicDashboardService
    {
        
        #region Dynamic Dashboard
        public async Task<List<mstDataSourceDashboard>> GetDataSourceDashboardList(mstDataSourceDashboard model, int RoleID)
        {
            List<mstDataSourceDashboard> mstList = new List<mstDataSourceDashboard>();
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new mstDataSourceDashboardRepository(context);


            try
            {
                mstList = repo.GetList(" dbo.[ufnSplitStringToColumns](RoleID,',','" + RoleID + "')=1");

                for (int i = 0; i < mstList.Count; i++)
                {
                    if (mstList[i].ShowColumn != null & mstList[i].ShowColumn != "")
                        mstList[i].ShowColumn = DataReaderExtensions.CamelToRegular(mstList[i].ShowColumn);
                }

            }
            catch (Exception ex)
            {
                mstList.Add(new mstDataSourceDashboard((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardService", "GetDataSourceDashboardList", model.CreatedBy)));

                return mstList;
            }
            finally
            {
                context.Dispose();
            }
            return mstList;
        }

        public async Task<List<mstDataSourceDashboard>> GetDataSourceDashboard(mstDataSourceDashboard model, int rowSkip, int pageSize)
        {
            List<mstDataSourceDashboard> mstList = new List<mstDataSourceDashboard>();
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new mstDataSourceDashboardRepository(context);


            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(model.DataSourceName))
                {
                    strWhereClause += "DataSourceName  like '%" + model.DataSourceName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(model.DBContext))
                {
                    strWhereClause += "DBContext  like '%" + model.DBContext + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(model.ViewName))
                {
                    strWhereClause += "ViewName like '%" + model.ViewName + "%' AND ";
                }
               

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                mstList = repo.GetPaged(strWhereClause, "ViewName, DataSourceName", rowSkip, pageSize);



            }
            catch (Exception ex)
            {
                mstList.Add(new mstDataSourceDashboard((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardService", "GetDataSourceDashboard", model.CreatedBy)));
                return mstList;
            }
            finally
            {
                context.Dispose();
            }
            return mstList;
        }

        public async Task<int> GetDataSourceDashboardCount(mstDataSourceDashboard model)
        {
            List<mstDataSourceDashboard> mstList = new List<mstDataSourceDashboard>();
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new mstDataSourceDashboardRepository(context);


            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(model.DataSourceName))
                {
                    strWhereClause += "DataSourceName  like '%" + model.DataSourceName + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(model.DBContext))
                {
                    strWhereClause += "DBContext  like '%" + model.DBContext + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(model.ViewName))
                {
                    strWhereClause += "ViewName like '%" + model.ViewName + "%' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                mstList.Add(new mstDataSourceDashboard((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardService", "GetDataSourceDashboardCount", model.CreatedBy)));
                return 0;
            }
            finally
            {
                context.Dispose();
            }

        }

        public async Task<mstDataSourceDashboard> SaveDataSourceDashboard(string strToken, mstDataSourceDashboard model)
        {
            mstDataSourceDashboard result = new mstDataSourceDashboard();
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new mstDataSourceDashboardRepository(context);

            try
            {


                if (model.ID == 0)
                {
                    result = repo.GetList("ViewName = '" + model.ViewName + "' AND DBContext= '" + model.DBContext + "'", "ViewName").FirstOrDefault();
                    if (result != null)
                        model.ID = result.ID;
                }


                if (model.ID == 0)
                {
                    model.CreatedBy = model.CreatedBy;
                    result = repo.Create(model);
                }
                else
                {
                    model.UpdatedBy = model.CreatedBy;
                    result = repo.Update(model);
                }

                return result;
            }
            catch (Exception ex)
            {
                result.ErrorType = (int)Helper.ErrorType.Error;
                result.ErrorMessage = Helper.logError(ex.Message.ToString(), "DashboardService", "SaveDataSourceDashboard", model.CreatedBy);

                return result;
            }
            finally
            {
                context.Dispose();
            }

        }

        public async Task<List<mstDashboardTemplate>> GetDashboardTemlpateList(mstDashboardTemplate model, int intRowSkip = 0, int intPageSize = 0)
        {
            List<mstDashboardTemplate> mstList = new List<mstDashboardTemplate>();
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new mstDashboardTemplateRepository(context);


            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(model.TemplateName))
                {
                    strWhereClause += "TemplateName like '%" + model.TemplateName.TrimStart().TrimEnd() + "%' AND ";
                }

                if (!string.IsNullOrWhiteSpace(model.AggregatorName))
                {
                    strWhereClause += "AggregatorName like '%" + model.AggregatorName.TrimStart().TrimEnd() + "%' AND ";
                }

                if (!string.IsNullOrWhiteSpace(model.RendererName))
                {
                    strWhereClause += "RendererName like '%" + model.RendererName.TrimStart().TrimEnd() + "%' AND ";
                }

                if (model.DataSourceID != 0 && model.DataSourceID != null)
                {
                    strWhereClause += "DataSourceID = " + model.DataSourceID + " AND ";
                }


                // strWhereClause += "a.RoleID = '" + userCredential.UserRoleID + "' AND ";


                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                //mstList = repo.GetPaged(strWhereClause, " TemplateName ASC ", intRowSkip, intPageSize);
                mstList = repo.GetList(strWhereClause, " DataSourceName ASC ");
                for (int i = 0; i < mstList.Count; i++)
                {
                    if (mstList[i].ShowColumn != null & mstList[i].ShowColumn != "")
                        mstList[i].ShowColumn = DataReaderExtensions.CamelToRegular(mstList[i].ShowColumn);
                }

                return mstList;
            }
            catch (Exception ex)
            {
                mstList.Add(new mstDashboardTemplate((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardService", "GetDashboardTemlpateList", model.CreatedBy)));
                return mstList;
            }
            finally
            {
                context.Dispose();
            }

        }
        public async Task<int> GetDashboardTemlpateCount(mstDashboardTemplate model)
        {
            List<mstDashboardTemplate> mstList = new List<mstDashboardTemplate>();
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new mstDashboardTemplateRepository(context);


            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(model.TemplateName))
                {
                    strWhereClause += "TemplateName like '%" + model.TemplateName.TrimStart().TrimEnd() + "%' AND ";
                }

                if (!string.IsNullOrWhiteSpace(model.AggregatorName))
                {
                    strWhereClause += "AggregatorName like '%" + model.AggregatorName.TrimStart().TrimEnd() + "%' AND ";
                }

                if (!string.IsNullOrWhiteSpace(model.RendererName))
                {
                    strWhereClause += "RendererName like '%" + model.RendererName.TrimStart().TrimEnd() + "%' AND ";
                }

                if (model.DataSourceID != 0 && model.DataSourceID != null)
                {
                    strWhereClause += "DataSourceID = " + model.DataSourceID + " AND ";
                }


                // strWhereClause += "a.RoleID = '" + userCredential.UserRoleID + "' AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                mstList.Add(new mstDashboardTemplate((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "DashboardService", "GetDashboardTemlpateList", model.CreatedBy)));
                return 0;
            }
            finally
            {
                context.Dispose();
            }

        }

        public async Task<mstDashboardTemplate> DashboardTemlpateSave(mstDashboardTemplate model)
        {
            mstDashboardTemplate mst = new mstDashboardTemplate();
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new mstDashboardTemplateRepository(context);


            try
            {
                model.IsActive = true;
                if (model.ID > 0)
                    mst = repo.Update(model);
                else
                    mst = repo.Create(model);

                return mst;
            }
            catch (Exception ex)
            {

                mst.ErrorType = (int)Helper.ErrorType.Error;
                mst.ErrorMessage = Helper.logError(ex.Message.ToString(), "DashboardService", "DashboardTemlpateSave", model.CreatedBy);
                return mst;
            }
            finally
            {
                context.Dispose();
            }

        }

        public async Task<DashboardData> GetDashboardData(string userid, int DataSourceID, Dictionary<string, object> Param, int start, int length)
        {
            DashboardData data = new DashboardData();
            mstDataSourceDashboard dataSource = new mstDataSourceDashboard();

            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repoDataSource = new mstDataSourceDashboardRepository(context);


            try
            {
                dataSource = repoDataSource.GetByPK(DataSourceID);
                string vieName = dataSource.DBContext.TrimStart().TrimEnd() + "." + dataSource.DBSchema.TrimStart().TrimEnd() + "." + dataSource.ViewName.TrimStart().TrimEnd();
                if (dataSource != null && await ValidateView(dataSource.ViewName.TrimStart().TrimEnd(), dataSource.DBContext.TrimStart().TrimEnd()))
                {
                    string whereClause = "";

                    foreach (var item in Param)
                    {
                        if (item.Value != "")
                        {
                            string t = "";
                            try
                            {
                                DateTime dt = Convert.ToDateTime(item.Value);
                                t = String.Format("{0:yyyy/MM/dd}", dt).ToString();
                            }
                            catch (Exception)
                            {
                                t = item.Value != null ? item.Value.ToString().TrimStart().TrimEnd() : item.Value.ToString();

                            }

                            whereClause += " AND " + item.Key + " = '" + t + "' ";
                        }

                    }
                    var repo = new DynamicDashboardRepository(context);
                    string selectCol = await repo.GetColumnNameListByView(dataSource.ViewName.TrimStart().TrimEnd(), dataSource.DBContext.TrimStart());
                    data = await repo.GetDashboardData(vieName, dataSource.DBContext.TrimStart(), dataSource.ViewName.TrimStart().TrimEnd(), start, length, whereClause, selectCol);


                }
                else
                {
                    data.ErrorMessage = "View is not valid!";
                }
                return data;
            }
            catch (Exception ex)
            {

                data.ErrorType = (int)Helper.ErrorType.Error;
                data.ErrorMessage = Helper.logError(ex.Message.ToString(), "DashboardService", "GetDashboardData", userid);
                return data;
            }
            finally
            {
                context.Dispose();
            }
        }

        public async Task<int> GetDashboardCountData(string userid, int DataSourceID, Dictionary<string, object> Param)
        {
            DashboardData data = new DashboardData();
            mstDataSourceDashboard dataSource = new mstDataSourceDashboard();

            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repoDataSource = new mstDataSourceDashboardRepository(context);


            try
            {
                dataSource = repoDataSource.GetByPK(DataSourceID);
                string vieName = dataSource.DBContext.TrimStart().TrimEnd() + "." + dataSource.DBSchema.TrimStart().TrimEnd() + "." + dataSource.ViewName.TrimStart().TrimEnd();
                if (dataSource != null && await ValidateView(dataSource.ViewName.TrimStart().TrimEnd(), dataSource.DBContext.TrimStart().TrimEnd()))
                {
                    var repo = new DynamicDashboardRepository(context);
                    string whereClause = "";
                    return await repo.GetDashboardCountData(vieName, whereClause);
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {

                data.ErrorType = (int)Helper.ErrorType.Error;
                data.ErrorMessage = Helper.logError(ex.Message.ToString(), "DashboardService", "GetDashboardData", userid);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public async Task<bool> ValidateView(string viewName, string dbName)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repoDataSource = new DynamicDashboardRepository(context);
            try
            {
                string data = await repoDataSource.GetScriptView(viewName, dbName);
                data = data.ToUpper();

                if (data.Contains("TRUNCATE") || data.Contains("DROP") || data.Contains("INSERT") || data.Contains("DELETE") || data.Contains("UPDATE"))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        public async Task<List<Dictionary<string, string>>> GetColumnList(string viewName, string dbName)
        {
            var data = new List<Dictionary<string, string>>();
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new DynamicDashboardRepository(context);


            try
            {
                data = await repo.GetColumnNameList(viewName, dbName);
                return data;
            }
            catch (Exception ex)
            {

                return data;
            }
            finally
            {
                context.Dispose();
            }
        }

        public async Task<List<Dictionary<string, string>>> GetSchemaDBList(string strToken)
        {
            var data = new List<Dictionary<string, string>>();
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new DynamicDashboardRepository(context);


            try
            {
                data = await repo.GetSchemaDBList();
                return data;
            }
            catch (Exception ex)
            {

                return data;
            }
            finally
            {
                context.Dispose();
            }
        }

        #endregion


    }
}
