using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.Models;

namespace ARSystem.Service
{
    public class MstRoleGroupService
    {
        public string GetDashboardUrl(int roleID)
        {
            return pGetDashboardUrl(roleID);
        }

        private string pGetDashboardUrl(int roleID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new mstRoleGroupDashboardRepository(context);
            mstRoleGroupDashboard data = new mstRoleGroupDashboard();
            try
            {
                data = repo.GetList(" RoleID = "+roleID+" AND IsActive=1 ").FirstOrDefault();
                return data.UrlAction;
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                context.Dispose();
            }
        }

    }
}
