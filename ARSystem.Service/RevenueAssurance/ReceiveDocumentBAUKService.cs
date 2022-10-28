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
    public class ReceiveDocumentBAUKService
    {
        public List<vwRAReceiveDocumentBAUK> GetReceiveDocumentBAUK(vwRAReceiveDocumentBAUK param, string UserID, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            return pGetReceiveDocumentBAUKList(param, UserID, strOrderBy, intRowSkip, intPageSize);
        }
        public int GetReceiveDocumentBAUKCount(vwRAReceiveDocumentBAUK param, string UserID)
        {
            return pGetReceiveDocumentBAUKCount(param, UserID);
        }
        public List<trxReceiveDocumentBAUK> GetHistory(string UserID, string vSonumber)
        {
            return pGetHistory(UserID, vSonumber);
        }
        public List<vwRAReceiveDocumentBAUKHistory> GetReceiveDocumentBAUKHistory(vwRAReceiveDocumentBAUK param, string UserID)
        {
            return pGetReceiveDocumentBAUKHistory(param, UserID);
        }
        public List<vwRAReceiveDocumentBAUK> GetCheckList(string UserID, List<string> list, string Action)
        {
            return pGetCheckList(UserID, list, Action);
        }
        public List<vwPICReceiveDocumentBAUK> GetPICReceiveDocumentBAUK(string UserID)
        {
            return pGetPICReceiveDocumentBAUK(UserID);
        }
        public List<vwCustomerReceiveBAUK> GetCustomerReceiveBAUK(string UserID)
        {
            return pGetCustomerReceiveBAUK(UserID);
        }
        public List<vwRAReceiveDocumentBAUK> GetReceiveDocumentBySonumb(string UserID, string SONumber)
        {
            return pGetReceiveDocumentBySonumb(UserID, SONumber);
        }
        public List<trxReceiveDocumentBAUK> SaveReceiveDocBulky(string UserID, List<trxReceiveDocumentBAUK> ModelSaveBulky)
        {
            return pSaveReceiveDocBulky(UserID, ModelSaveBulky);
        }
        public trxReceiveDocumentBAUK Savetrx(string UserID, trxReceiveDocumentBAUK model)
        {
            return pSavetrx(UserID, model);
        }

        #region private
        private List<vwRAReceiveDocumentBAUK> pGetReceiveDocumentBAUKList(vwRAReceiveDocumentBAUK param, string UserID, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRAReceiveDocumentBAUKRepository(context);
           
            List<vwRAReceiveDocumentBAUK> list = new List<vwRAReceiveDocumentBAUK>();

            try
            {
                string strWhereClause = MapParameter(param);
                list = repo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwRAReceiveDocumentBAUK((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReceiveDocumentBAUKService", "pGetReceiveDocumentBAUKList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        private List<vwRAReceiveDocumentBAUKHistory> pGetReceiveDocumentBAUKHistory(vwRAReceiveDocumentBAUK param, string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repohistory = new vwRAReceiveDocumentBAUKHistoryRepository(context);
            List<vwRAReceiveDocumentBAUKHistory> listhistory = new List<vwRAReceiveDocumentBAUKHistory>();

            try
            {
                string strWhereClause = MapParameter(param);
                listhistory = repohistory.GetList(strWhereClause, "");

                return listhistory;
            }
            catch (Exception ex)
            {
                listhistory.Add(new vwRAReceiveDocumentBAUKHistory((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReceiveDocumentBAUKService", "pGetReceiveDocumentBAUKHistory", UserID)));
                return listhistory;
            }
            finally
            {
                context.Dispose();
            }
        }
        private int pGetReceiveDocumentBAUKCount(vwRAReceiveDocumentBAUK param, string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRAReceiveDocumentBAUKRepository(context);

            try
            {
                string strWhereClause = MapParameter(param);
                //strWhereClause += " AND StatusDoc NOT IN ('Complete')";
                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "ReceiveDocumentBAUKService", "pGetReceiveDocumentBAUKCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
        private List<trxReceiveDocumentBAUK> pGetHistory(string UserID, string vSONumber)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new trxReceiveDocumentBAUKRepository(context);
            List<trxReceiveDocumentBAUK> list = new List<trxReceiveDocumentBAUK>();

            try
            {
                string strWhereClause = "1=1";
                if (!string.IsNullOrWhiteSpace(vSONumber))
                {
                    strWhereClause += " AND SONumber = '" + vSONumber + "' ";
                }
                list = repo.GetList(strWhereClause);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new trxReceiveDocumentBAUK((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReceiveDocumentBAUKService", "pGetReceiveDocumentBAUKList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        private List<vwRAReceiveDocumentBAUK> pGetCheckList(string UserID, List<string> list, string Action)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwRAReceiveDocumentBAUKRepository(context);
            List<vwRAReceiveDocumentBAUK> CheckList = new List<vwRAReceiveDocumentBAUK>();

            try
            {
                string strWhereClause = "";
                if (list.Count() != 0)
                {
                    string listSONumber = string.Empty;
                    foreach (string SeqID in list)
                    {
                        if (listSONumber == string.Empty)
                            listSONumber = "'" + SeqID.ToString() + "'";
                        else
                            listSONumber += ",'" + SeqID.ToString() + "'";
                    }
                    strWhereClause += "SONumber IN (" + listSONumber + ")";
                }
                if (Action == "CheckNext")
                {
                    CheckList = repo.GetListCheckNext(strWhereClause, "", Action);
                }
                else if (Action == "NotComplete")
                {
                    CheckList = repo.GetListNotComplete(strWhereClause, "", Action);
                }
                else
                {
                    CheckList = repo.GetListComplete(strWhereClause, "", Action);
                }
                
                return CheckList;
            }
            catch (Exception ex)
            {
                CheckList.Add(new vwRAReceiveDocumentBAUK((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReceiveDocumentBAUKService", "pGetCheckList", UserID)));
                return CheckList;
            }
            finally
            {
                context.Dispose();
            }
        }
        private List<vwPICReceiveDocumentBAUK> pGetPICReceiveDocumentBAUK(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repository = new vwPICReceiveDocumentBAUKRepository(context);
            List<vwPICReceiveDocumentBAUK> list = new List<vwPICReceiveDocumentBAUK>();

            try
            {
                string strWhereClause = string.Empty;
                list = repository.GetList(strWhereClause);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwPICReceiveDocumentBAUK((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReceiveDocumentBAUKService", "pGetPICReceiveDocumentBAUK", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        private List<vwCustomerReceiveBAUK> pGetCustomerReceiveBAUK(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repository = new vwCustomerReceiveBAUKRepository(context);
            List<vwCustomerReceiveBAUK> list = new List<vwCustomerReceiveBAUK>();

            try
            {
                string strWhereClause = string.Empty;
                list = repository.GetList(strWhereClause);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwCustomerReceiveBAUK((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReceiveDocumentBAUKService", "pGetCustomerReceiveBAUK", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        private List<vwRAReceiveDocumentBAUK> pGetReceiveDocumentBySonumb(string UserID, string SONumber)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repository = new vwRAReceiveDocumentBAUKRepository(context);
            List<vwRAReceiveDocumentBAUK> list = new List<vwRAReceiveDocumentBAUK>();

            try
            {
                string strWhereClause = "SONumber = '" + SONumber + "' ";
                list = repository.GetList(strWhereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwRAReceiveDocumentBAUK((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReceiveDocumentBAUKService", "pGetReceiveDocumentBySonumb", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        private List<trxReceiveDocumentBAUK> pSaveReceiveDocBulky(string UserID, List<trxReceiveDocumentBAUK> ModelSaveBulky)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();
            var repo = new trxReceiveDocumentBAUKRepository(context);
            var trxReceiveDocument = new List<trxReceiveDocumentBAUK>();

            try
            {
                trxReceiveDocument = repo.CreateBulky(ModelSaveBulky);
                uow.SaveChanges();
                return trxReceiveDocument;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                trxReceiveDocument.Add(new trxReceiveDocumentBAUK((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReceiveDocumentBAUKService", "pSaveReceiveDocBulky", UserID)));
                return trxReceiveDocument;
            }
            finally
            {
                context.Dispose();
            }
        }
        private trxReceiveDocumentBAUK pSavetrx(string UserID, trxReceiveDocumentBAUK Model)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();
            var repo = new trxReceiveDocumentBAUKRepository(context);
            var trxReceiveDocument = new trxReceiveDocumentBAUK();

            try
            {
                trxReceiveDocument = repo.Create(Model);
                uow.SaveChanges();
                return trxReceiveDocument;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxReceiveDocumentBAUK((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReceiveDocumentBAUKService", "pSavetrx", UserID));

            }
            finally
            {
                context.Dispose();
            }
        }
        private static string MapParameter(vwRAReceiveDocumentBAUK param)
        {
            string strWhereClause = "1=1";
            if (!string.IsNullOrWhiteSpace(param.SONumber))
            {
                strWhereClause += " AND SONumber = '" + param.SONumber + "' ";
            }

            if (!string.IsNullOrWhiteSpace(param.SiteID))
            {
                strWhereClause += " AND SiteID = '" + param.SiteID + "' ";
            }

            if (!string.IsNullOrWhiteSpace(param.SiteName))
            {
                strWhereClause += " AND SiteName = '" + param.SiteName + "' ";
            }

            if (!string.IsNullOrEmpty(param.StartSubmit.ToString()) && !string.IsNullOrEmpty(param.EndSubmit.ToString()))
            {
                strWhereClause += " AND CAST(BaukSubmitBySystem as date) between CAST('" + String.Format("{0:yyyy/MM/dd}", param.StartSubmit) + "' as date) AND CAST('" + String.Format("{0:yyyy/MM/dd}", param.EndSubmit) + "' as date) ";
            }
            //this param is set to -1 if empty from ui
            //if (param.StipSiro != -1)
            //{
            //    strWhereClause += " AND StipSiro LIKE '%" + param.StipSiro + "%' ";
            //}
            if (!string.IsNullOrWhiteSpace(param.CustomerID))
            {
                strWhereClause += " AND CustomerID = '" + param.CustomerID + "' ";
            }

            if (!string.IsNullOrWhiteSpace(param.CompanyID))
            {
                strWhereClause += " AND CompanyID = '" + param.CompanyID + "' ";
            }

            if (!string.IsNullOrWhiteSpace(param.StatusDoc) || param.StatusDoc != null)
            {
                strWhereClause += " AND StatusDoc = '" + param.StatusDoc + "' ";
            }
            else
            {
                strWhereClause += "AND StatusDoc = 'Not Yet' ";
            }

            if (param.ProductID != 0 && param.ProductID != null)
            {
                strWhereClause += " AND ProductID = '" + param.ProductID + "' ";
            }

            if (param.STIPID != 0 && param.STIPID != null)
            {
                strWhereClause += " AND STIPID = '" + param.STIPID + "' ";
            }

            return strWhereClause;
        }
        #endregion
    }
}
