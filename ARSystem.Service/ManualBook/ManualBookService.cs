using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;

namespace ARSystem.Service
{
    public class ManualBookService
    {
        public int GetManualBookCount(string userID, vwProjectManualBook manualBook)
        {
            var context = new DbContext(Helper.GetConnection("TeamMate"));
            var manualBookRepo = new vwProjectManualBookRepository(context);
            
            try
            {
                string strWhereClause = pGetManualBookWhereClause(manualBook);
                return manualBookRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "ManualBookService", "GetManualBookCount", userID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwProjectManualBook> GetManualBookList(string userID, vwProjectManualBook manualBook, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("TeamMate"));
            var manualBookRepo = new vwProjectManualBookRepository(context);
            List<vwProjectManualBook> manualBookList = new List<vwProjectManualBook>();

            try
            {
                string strWhereClause = pGetManualBookWhereClause(manualBook);

                if (intPageSize > 0)
                    manualBookList = manualBookRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    manualBookList = manualBookRepo.GetList(strWhereClause, strOrderBy);

                return manualBookList;
            }
            catch (Exception ex)
            {
                manualBookList.Add(new vwProjectManualBook((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ManualBookService", "GetManualBookList", userID)));
                return manualBookList;
            }
            finally
            {
                context.Dispose();
            }
        }

        public static vwProjectManualBook GetSingleManualBook(vwProjectManualBook manualBook)
        {
            var context = new DbContext(Helper.GetConnection("TeamMate"));
            vwProjectManualBookRepository manualBookRepo = new vwProjectManualBookRepository(context);

            try
            {
                manualBook = manualBookRepo.GetList("ProjectID =" + manualBook.ProjectID).FirstOrDefault();
                return manualBook;
            }
            catch (Exception ex)
            {
                manualBook = new vwProjectManualBook((int)Helper.ErrorType.Error, ex.Message.ToString());
                return manualBook;
            }
            finally
            {
                context.Dispose();
            }
        }

        private string pGetManualBookWhereClause(vwProjectManualBook manualBook)
        {
            string strWhereClause = "1=1 AND FileName IS NOT NULL AND ApplicationID=3"; //3 = ARSystem
            if (!string.IsNullOrWhiteSpace(manualBook.ProjectName))
            {
                strWhereClause += " AND ProjectName LIKE '%" + manualBook.ProjectName.Trim() + "%'";
            }
            if (!string.IsNullOrWhiteSpace(manualBook.ProjectDescription))
            {
                strWhereClause += " AND ProjectDescription LIKE '%" + manualBook.ProjectDescription.Trim() + "%'";
            }

            return strWhereClause;
        }
    }
}
