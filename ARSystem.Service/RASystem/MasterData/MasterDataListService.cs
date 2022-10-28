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
    public static class MasterDataListService
    {
        public static List<mstApprovalStatus> GetApprovalStatus()
        {
            return pGetApprovalStatus();
        }
        private static List<mstApprovalStatus> pGetApprovalStatus()
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new mstApprovalStatusRepository(context);
            List<mstApprovalStatus> list = new List<mstApprovalStatus>();
            try
            {

                list = repo.GetList(" statustype = 1 ");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new mstApprovalStatus((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "MasterDataListService", "GetApprovalStatus", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
