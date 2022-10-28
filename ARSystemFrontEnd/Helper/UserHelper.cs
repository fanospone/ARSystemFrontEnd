using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;

namespace ARSystemFrontEnd.Helper
{
    public static class UserHelper
    {
        
        public static string GetUserARPosition(string UserId)
        {
            var context = new DbContext(ARSystem.Service.Helper.GetConnection("ARSystem"));
            var vwARUserRepo = new vwDataARUserRepository(context);
            
            List<vwDataARUser> DataARUser = new List<vwDataARUser>();
            DataARUser = vwARUserRepo.GetList("UserId='" + UserId + "' AND Position IN ('DEPT HEAD','AR DATA','AR COLLECTION','AR MONITORING')", ""); //DEPT HEAD = Dept.Head AR Data, AR MONITORING  = Dept.Head AR Collection
            if (DataARUser.Count() > 0)
                return DataARUser[0].Position;
            else
                return "NON AR";
        }

        public static List<vwARUserRole> GetUserRoles(string UserId)
        {
            var context = new DbContext(ARSystem.Service.Helper.GetConnection("ARSystem"));
            var repo = new vwARUserRoleRepository(context);
            List<vwARUserRole> roles = new List<vwARUserRole>();

            roles = repo.GetList("UserId='" + UserId + "'", "");

            return roles;
        }

        public static string GetUserRAPosition(string UserId)
        {
            var context = new DbContext(ARSystem.Service.Helper.GetConnection("ARSystem"));
            var vwARUserRepo = new vwDataARUserRepository(context);

            List<vwDataARUser> DataARUser = new List<vwDataARUser>();
            DataARUser = vwARUserRepo.GetList("UserId='" + UserId + "' AND Position IN ('DEPT HEAD','AR DATA','AR COLLECTION','AR MONITORING')", ""); //DEPT HEAD = Dept.Head AR Data, AR MONITORING  = Dept.Head AR Collection
            if (DataARUser.Count() > 0)
                return DataARUser[0].Position;
            else
                return "NON AR";
        }
    }

}