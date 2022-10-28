using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;
using TBGFramework.Workflow;

namespace ARSystem.Service
{
    public class WorkFlowService
    {


        public List<vwWfPrDefActivity> GetActivity(vwWfPrDefActivity param)
        {
            return pGetActivity(param);
        }


        private List<vwWfPrDefActivity> pGetActivity(vwWfPrDefActivity param)
        {
            var context = new DbContext(Helper.GetConnection("TBGFlow"));
            var repo = new vwWfPrDefActivityRepository(context);
            var result = new List<vwWfPrDefActivity>();
            try
            {
                string whereClause = "";

                if (!string.IsNullOrEmpty(param.Activity))
                    whereClause += "Activity ='" + param.Activity + "' AND ";

                if (!string.IsNullOrEmpty(param.Code))
                    whereClause += "Code ='" + param.Code + "' AND ";

                if (param.JobID !=null && param.JobID !=0)
                    whereClause += "JobID =" + param.JobID + " AND ";

                if (param.ActivityID != null && param.ActivityID != 0)
                    whereClause += "ActivityID =" + param.ActivityID + " AND ";

                if (param.ProcessID != null && param.ProcessID != 0)
                    whereClause += "ProcessID =" + param.ProcessID + " AND ";
               

                whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
                
                result = repo.GetList(whereClause, "SortOrder");
            }
            catch (Exception ex)
            {
                result.Add(new vwWfPrDefActivity { ErrorMessage = ex.Message });
            }
            finally
            {
                context.Dispose();
            }

            return result;
        }
    }
}
