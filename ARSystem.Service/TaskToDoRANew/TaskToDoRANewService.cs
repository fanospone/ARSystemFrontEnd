using System;
using System.Collections.Generic;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

namespace ARSystem.Service
{

    public class TaskToDoRANewService
    {
        public   List<dwhTaskToDoRANew> GetToDoList()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new dwhTaskToDoRANewRepository(context);
            var list = new List<dwhTaskToDoRANew>();

            try
            {
                list = repo.GetList("","Sort ASC");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new dwhTaskToDoRANew((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "TaskToDoRANewService", "GetToDoList", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<dwhTaskToDoRANewDetail> GetToDoListDetail(string TodoName)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new dwhTaskToDoRANewDetailRepository(context);
            var list = new List<dwhTaskToDoRANewDetail>();

            try
            {
                list = repo.GetList("TaskToDoName = '"+ TodoName + "'");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new dwhTaskToDoRANewDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "TaskToDoRANewService", "GetToDoListDetail", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
