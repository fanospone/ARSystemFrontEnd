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
    public class MstRAUserGroupService
    {
        public string GetUserGroup (string userID)
        {
            return pGetUserGroup(userID);
        }

        private string pGetUserGroup(string userID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new mstRAUserGroupRepository(context);
            mstRAUserGroup data = new mstRAUserGroup();
            try
            {
                data = repo.GetByPK(userID);
                return data.UserGroup;
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
