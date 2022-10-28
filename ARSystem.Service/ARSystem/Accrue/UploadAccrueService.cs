using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using ARSystem.Service;
using System.Threading;
using System.Data;
using System.Configuration;

namespace ARSystem.Service
{
    public class UploadAccrueService
    {
        public trxUploadDataAccrue ValidateDataAccrue(List<trxUploadDataAccrue> data)
        {
            return pValidateDataAccrue(data);
        }
        public List<vwtrxUploadDataAccrue> GetValidateAccrueList(vwtrxUploadDataAccrue param, int rowSkip, int pageSize)
        {
            return pGetValidateAccrueList(param, rowSkip, pageSize);
        }
        public trxUploadDataAccrue CancelUpload(string UserID)
        {
            return pCancelUpload(UserID);
        }
        public List<vwtrxUploadDataAccrueStaging> GetHistoryUploadList(vwtrxUploadDataAccrueStaging param, int rowSkip, int pageSize)
        {
            return pGetHistoryUploadList(param, rowSkip, pageSize);
        }
        public trxUploadDataAccrue DeleteAccrue(string UserID, int ID)
        {
            return pDeleteAccrue(UserID, ID);
        }
        public trxUploadDataAccrueStaging DeleteAccrueHistory(string UserID, List<string> ID)
        {
            return pDeleteAccrueHistory(UserID, ID);
        }
        public trxUploadDataAccrueStaging DeleteAccrueHistoryByParams(string UserID, vwtrxUploadDataAccrueStaging param)
        {
            return pDeleteAccrueHistoryByParams(UserID, param);
        }
        public trxUploadDataAccrueStaging SubmitUploadDataAccrue(string UserID)
        {
            return pSubmitUploadDataAccrue(UserID);
        }
        public int GetValidateAccrueListCount(vwtrxUploadDataAccrue param)
        {
            return pGetValidateAccrueListCount(param);
        }
        public int GetDataAccrueStagingListCount(vwtrxUploadDataAccrueStaging param)
        {
            return pGetDataAccrueStagingListCount(param);
        }
        private trxUploadDataAccrue pValidateDataAccrue(List<trxUploadDataAccrue> data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var result = new trxUploadDataAccrue();
            try
            {
                var repo = new trxUploadDataAccrueRepository(context);
                List<trxUploadDataAccrue> tempList;

                tempList = repo.CreateBulky(data);
                return result;
            }
            catch (Exception ex)
            {
                context.Dispose();
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
        private trxUploadDataAccrue pCancelUpload(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxUploadDataAccrueRepository(context);
            var uow = context.CreateUnitOfWork();
            trxUploadDataAccrue param = new trxUploadDataAccrue();
            try
            {
                string whereClause = "1=1";
                Repo.DeleteByFilter(whereClause);

                uow.SaveChanges();
                return new trxUploadDataAccrue();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxUploadDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "pDeleteAccrue", "Delete", UserID));
            }
            finally
            {
                context.Dispose();

            }
        }
        private List<vwtrxUploadDataAccrue> pGetValidateAccrueList(vwtrxUploadDataAccrue param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwtrxUploadDataAccrue> data = new List<vwtrxUploadDataAccrue>();
            try
            {

                var repo = new vwtrxUploadDataAccrueRepository(context);

                string whereClause = "1=1";

                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    whereClause += " AND SONumber = '" + param.SONumber + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteID))
                {
                    whereClause += " AND SiteID = '" + param.SiteID + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.Remarks))
                {
                    whereClause += " AND Remarks LIKE '%" + param.Remarks + "%' ";
                }

                whereClause += "AND IsActive = 1";

                data = repo.GetPaged(whereClause, "ID ASC", rowSkip, pageSize);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwtrxUploadDataAccrue { ErrorMessage = ex.Message });
            }

            return data;
        }
        private List<vwtrxUploadDataAccrueStaging> pGetHistoryUploadList(vwtrxUploadDataAccrueStaging param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwtrxUploadDataAccrueStaging> data = new List<vwtrxUploadDataAccrueStaging>();
            try
            {

                var repo = new vwtrxUploadDataAccrueStagingRepository(context);

                string whereClause = "1=1 AND IsDelete = 0";
                if (param.Week > 0 || param.Week != null)
                {
                    whereClause += " AND Week = '" + param.Week + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.MonthYear))
                {
                    whereClause += " AND Month = '" + int.Parse(DateTime.Parse(param.MonthYear).ToString("MM")) + "' ";
                    whereClause += " AND Year = '" + int.Parse(DateTime.Parse(param.MonthYear).ToString("yyyy")) + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    whereClause += " AND SONumber = '" + param.SONumber + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteID))
                {
                    whereClause += " AND SiteID = '" + param.SiteID + "' ";
                }

                whereClause += " AND AccrueStatusID = '" + param.AccrueStatusID + "' ";

                data = repo.GetPaged(whereClause, "ID ASC", rowSkip, pageSize);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwtrxUploadDataAccrueStaging { ErrorMessage = ex.Message });
            }

            return data;
        }
        private List<vwtrxUploadDataAccrueStaging> pGetHistoryUploadList(int? Month, int? Week, int? AccrueStatusID, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwtrxUploadDataAccrueStaging> data = new List<vwtrxUploadDataAccrueStaging>();
            try
            {

                var repo = new vwtrxUploadDataAccrueStagingRepository(context);

                string whereClause = "1=1 AND IsDelete = 0";
                if (Week > 0 || Week != null)
                {
                    whereClause += " AND Week = '" + Week + "' ";
                }
                if (Month > 0 || Month != null)
                {
                    whereClause += " AND Month = '" + Month + "' ";
                }
                if (Month > 0 || Month != null)
                {
                    whereClause += " AND Month = '" + Month + "' ";
                }
                whereClause += " AND AccrueStatusID = '" + AccrueStatusID + "' ";

                data = repo.GetPaged(whereClause, "ID ASC", rowSkip, pageSize);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwtrxUploadDataAccrueStaging { ErrorMessage = ex.Message });
            }

            return data;
        }
        private trxUploadDataAccrue pDeleteAccrue(string UserID, int ID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxUploadDataAccrueRepository(context);
            var uow = context.CreateUnitOfWork();
            trxUploadDataAccrue param = new trxUploadDataAccrue();
            try
            {
                //param = Repo.GetByPK(ID);
                Repo.DeleteByPK(ID);

                uow.SaveChanges();
                return new trxUploadDataAccrue();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxUploadDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "pDeleteAccrue", "Delete", UserID));
            }
            finally
            {
                context.Dispose();

            }
        }
        private trxUploadDataAccrueStaging pDeleteAccrueHistory(string UserID, List<string> ID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxUploadDataAccrueStagingRepository(context);
            var uow = context.CreateUnitOfWork();
            trxUploadDataAccrueStaging param = new trxUploadDataAccrueStaging();
            try
            {
                string strWhereClause = "";

                string listID = string.Join("','", ID);
                strWhereClause = "ID IN ('" + listID + "')";

                Repo.UpdateByFilter(strWhereClause);

                uow.SaveChanges();
                return new trxUploadDataAccrueStaging();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxUploadDataAccrueStaging((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "pDeleteAccrueHistory", "Delete", UserID));
            }
            finally
            {
                context.Dispose();

            }
        }
        private trxUploadDataAccrueStaging pDeleteAccrueHistoryByParams(string UserID, vwtrxUploadDataAccrueStaging param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxUploadDataAccrueStagingRepository(context);
            var uow = context.CreateUnitOfWork();
            try
            {
                string whereClause = "1=1 AND IsDelete = 0";
                if (param.Week > 0 || param.Week != null)
                {
                    whereClause += " AND D.Week = '" + param.Week + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.MonthYear))
                {
                    whereClause += " AND D.Month = '" + int.Parse(DateTime.Parse(param.MonthYear).ToString("MM")) + "' ";
                    whereClause += " AND D.Year = '" + int.Parse(DateTime.Parse(param.MonthYear).ToString("yyyy")) + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    whereClause += " AND SONumber = '" + param.SONumber + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteID))
                {
                    whereClause += " AND SiteID = '" + param.SiteID + "' ";
                }

                whereClause += " AND AccrueStatusID IS NULL ";

                Repo.UpdateDeleteByFilter(whereClause);

                uow.SaveChanges();
                return new trxUploadDataAccrueStaging();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxUploadDataAccrueStaging((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "pDeleteAccrueHistory", "Delete", UserID));
            }
            finally
            {
                context.Dispose();

            }
        }
        private trxUploadDataAccrueStaging pSubmitUploadDataAccrue(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxUploadDataAccrueStagingRepository(context);
            var uow = context.CreateUnitOfWork();

            trxUploadDataAccrueStaging param = new trxUploadDataAccrueStaging();

            try
            {
                Repo.Create(UserID);

                uow.SaveChanges();
                return new trxUploadDataAccrueStaging();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxUploadDataAccrueStaging((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "pSubmitUploadDataAccrue", "Submit", UserID));
            }
            finally
            {
                context.Dispose();

            }
        }
        private int pGetValidateAccrueListCount(vwtrxUploadDataAccrue param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {

                var repo = new vwtrxUploadDataAccrueRepository(context);

                string whereClause = "1=1";

                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    whereClause += " AND SONumber = '" + param.SONumber + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteID))
                {
                    whereClause += " AND SiteID = '" + param.SiteID + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.Remarks))
                {
                    whereClause += " AND Remarks LIKE '%" + param.Remarks + "%' ";
                }

                whereClause += "AND IsActive = 1";

                return repo.GetCount(whereClause);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }

        }
        private int pGetDataAccrueStagingListCount(vwtrxUploadDataAccrueStaging param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {

                var repo = new vwtrxUploadDataAccrueStagingRepository(context);

                string whereClause = "1=1 AND IsDelete = 0";
                if (param.Week > 0 || param.Week != null)
                {
                    whereClause += " AND Week = '" + param.Week + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.MonthYear))
                {
                    whereClause += " AND Month = '" + int.Parse(DateTime.Parse(param.MonthYear).ToString("MM")) + "' ";
                    whereClause += " AND Year = '" + int.Parse(DateTime.Parse(param.MonthYear).ToString("yyyy")) + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    whereClause += " AND SONumber = '" + param.SONumber + "' ";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteID))
                {
                    whereClause += " AND SiteID = '" + param.SiteID + "' ";
                }

                whereClause += " AND AccrueStatusID = '" + param.AccrueStatusID + "' ";

                return repo.GetCount(whereClause);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }

        }
        public List<mstAccrueStatus> GetStatusAccrueToList(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new mstAccrueStatusRepository(context);
            List<mstAccrueStatus> list = new List<mstAccrueStatus>();

            try
            {
                string strWhereClause = "IsActive = 1";
                list = Repo.GetList(strWhereClause, "");

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new mstAccrueStatus((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetStatusAccrueToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        public int CountDataValid()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {

                var repo = new vwtrxUploadDataAccrueRepository(context);

                string whereClause = "1=1";
                whereClause += "AND IsActive = 1 AND IsValid = 1";

                return repo.GetCount(whereClause);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }

        }
        public int CountDataNotValid()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {

                var repo = new vwtrxUploadDataAccrueRepository(context);

                string whereClause = "1=1";
                whereClause += "AND IsValid = 0";

                return repo.GetCount(whereClause);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }

        }
        public List<string> GetHistoryUploadListId(string UserID, vwtrxUploadDataAccrueStaging data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwtrxUploadDataAccrueStaging> list = new List<vwtrxUploadDataAccrueStaging>();
            List<string> ListID = new List<string>();
            try
            {

                var repo = new vwtrxUploadDataAccrueStagingRepository(context);

                string whereClause = "1=1 AND IsDelete = 0";
                if (data.Week > 0 || data.Week != null)
                {
                    whereClause += " AND Week = '" + data.Week + "' ";
                }
                if (!string.IsNullOrWhiteSpace(data.MonthYear))
                {
                    whereClause += " AND Month = '" + int.Parse(DateTime.Parse(data.MonthYear).ToString("MM")) + "' ";
                    whereClause += " AND Year = '" + int.Parse(DateTime.Parse(data.MonthYear).ToString("yyyy")) + "' ";
                }
                if (!string.IsNullOrWhiteSpace(data.SONumber))
                {
                    whereClause += " AND SONumber = '" + data.SONumber + "' ";
                }
                if (!string.IsNullOrWhiteSpace(data.SiteID))
                {
                    whereClause += " AND SiteID = '" + data.SiteID + "' ";
                }

                whereClause += " AND AccrueStatusID = '" + data.AccrueStatusID + "' ";

                list = repo.GetList(whereClause, "");
                ListID = list.Select(l => l.ID.ToString()).ToList();
                return ListID;
            }
            catch (Exception ex)
            {
                list.Add(new vwtrxUploadDataAccrueStaging((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "UploadAccrueService", "GetHistoryUploadListId", UserID)));
                return ListID;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
