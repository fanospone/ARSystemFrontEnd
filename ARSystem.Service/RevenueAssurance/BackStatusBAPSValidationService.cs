using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

namespace ARSystem.Service
{
    public class BackStatusBAPSValidationService
    {
        public List<vwBackStatusBAPSValidation> GetBapsList(vwBackStatusBAPSValidation param, string strSearch,string UserID, string strOrderBy, bool isGetPaged, int intRowSkip = 0, int intPageSize = 0)
        {
            return pGetBapsList(param, strSearch, UserID, strOrderBy, isGetPaged, intRowSkip, intPageSize);
        }

        public int GetBapsCount(vwBackStatusBAPSValidation param, string strSearch, string UserID)
        {
            return pGetBapsCount(param, strSearch, UserID);
        }

        public bool ProcessBackStatus(string remark, List<BackStatusBAPSValidationData> dataList, string UserID)
        {
            return pProcessBackStatus(remark, dataList, UserID);
        }

        public List<int> GetListStipSiro(string UserID)
        {
            return pGetListStipSiro(UserID);
        }


        #region private
        private List<vwBackStatusBAPSValidation> pGetBapsList(vwBackStatusBAPSValidation param, string strSearch, string UserID, string strOrderBy, bool isGetPaged,int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwBackStatusBAPSValidationRepository(context);
            List<vwBackStatusBAPSValidation> list = new List<vwBackStatusBAPSValidation>();

            try
            {
                string strWhereClause = MapParameter(param);
                string strSearchWhereClause = GenerateSearchWhereClause(strSearch);
                if (!string.IsNullOrWhiteSpace(strSearchWhereClause))
                    strWhereClause = strWhereClause + " AND " + strSearchWhereClause;

                if (isGetPaged)
                    list = repo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    list = repo.GetList(strWhereClause);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwBackStatusBAPSValidation((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "BackStatusBAPSValidationService", "GetBAPSList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        private int pGetBapsCount(vwBackStatusBAPSValidation param, string strSearch, string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwBackStatusBAPSValidationRepository(context);

            try
            {
                string strWhereClause = MapParameter(param);
                string strSearchWhereClause = GenerateSearchWhereClause(strSearch);
                if (!string.IsNullOrWhiteSpace(strSearchWhereClause))
                    strWhereClause = strWhereClause + " AND " + strSearchWhereClause;

                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "BackStatusBAPSValidationService", "GetBapsCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        private bool pProcessBackStatus(string remark, List<BackStatusBAPSValidationData> dataList , string UserID) {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwBackStatusBAPSValidationRepository(context);

            try
            {
                repo.ProcessBackStatus(remark, dataList, UserID);
                return true;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "BackStatusBAPSValidationService", "ProcessBackStatus", UserID);
                return false;
            }
            finally
            {
                context.Dispose();
            }
        }

        private List<int> pGetListStipSiro(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwBackStatusBAPSValidationRepository(context);

            try
            {
                return repo.GetListStipSiro();
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "BackStatusBAPSValidationService", "GetListStipSiro", UserID);
                return new List<int>();
            }
            finally
            {
                context.Dispose();
            }
        }

        private static string MapParameter(vwBackStatusBAPSValidation param)
        {
            string strWhereClause = "1=1";
            if (!string.IsNullOrWhiteSpace(param.BapsType))
            {
                strWhereClause += " AND BapsType LIKE '%" + param.BapsType + "%' ";
            }

            //this param is set to -1 if empty from ui
            if (param.StipSiro != -1)
            {
                strWhereClause += " AND StipSiro LIKE '%" + param.StipSiro + "%' ";
            }

            if (!string.IsNullOrWhiteSpace(param.SoNumber))
            {
                strWhereClause += " AND SoNumber LIKE '%" + param.SoNumber + "%' ";
            }

            if (!string.IsNullOrWhiteSpace(param.CustomerSiteName))
            {
                strWhereClause += " AND CustomerSiteName LIKE '%" + param.CustomerSiteName + "%' ";
            }

            return strWhereClause;
        }

        private string GenerateSearchWhereClause(string searchValue)
        {
            string strFilterSearch = "";
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                strFilterSearch += " ( RowIndex LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR SoNumber LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR BAPSNumber LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR StipSiro LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR BapsType LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR CustomerSiteName LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR CheckListDoc LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR BapsValidation LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR BapsPrint LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR BapsInput LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR ActivityName LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR StartBapsDate LIKE '%" + searchValue + "%' ";
                strFilterSearch += " OR EndBapsDate LIKE '%" + searchValue + "%' )";
            }

            return strFilterSearch;
        }
        #endregion
    }
}
