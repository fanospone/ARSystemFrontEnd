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
    public class MstRAAmountTargetService
    {
        public List<MstRAAmountTarget> GetList(MstRAAmountTarget param, int rowSkip, int pageSize)
        {
            return pGetList(WhereClause(param), rowSkip, pageSize);
        }
        public int GetCount(MstRAAmountTarget param)
        {
            return pGetCount(WhereClause(param));
        }
        private string WhereClause(MstRAAmountTarget param)
        {
            string whereClause = "";
            if (!string.IsNullOrEmpty(param.CustomerID))
                whereClause += "CustomerID ='" + param.CustomerID + "' AND ";
            if (param.Year!=0)
                whereClause += "Year ='" + param.Year + "' AND ";

            whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            return whereClause;
        }
        private List<MstRAAmountTarget> pGetList(string whereClause, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new MstRAAmountTargetRepository(context);
            List<MstRAAmountTarget> list = new List<MstRAAmountTarget>();
            try
            {

                if (pageSize > 0)
                    list = repo.GetPaged(whereClause, "", rowSkip, pageSize);
                else
                    list = repo.GetList(whereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new MstRAAmountTarget((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataLeadeTime", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        private int pGetCount(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new MstRAAmountTargetRepository(context);
            try
            {
                return repo.GetCount(whereClause);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
