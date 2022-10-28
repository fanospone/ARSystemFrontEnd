using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

namespace ARSystem.Service
{
    public class LogHelper
    {
        public static int GetLogWeek(DateTime date)
        {
            int logWeek = 1;

            var context = new DbContext(Helper.GetConnection("ARSystem"));
            mstArLogDateKpiRepository logDateRepo = new mstArLogDateKpiRepository(context);
            string where = "LogDate = '" + date.ToString("yyyy-MM-dd") + "'";
            int logDateCount = logDateRepo.GetCount(where);

            if(logDateCount > 0)
            {
                List<mstArLogDateKpi> logDates = logDateRepo.GetList(where, "mstArLogDateKpiId DESC");
                mstArLogDateKpi logDate = logDates.FirstOrDefault();
                if(logDate != null)
                {
                    logWeek = logDate.LogWeek;
                }
            }

            return logWeek;
        }
    }
}