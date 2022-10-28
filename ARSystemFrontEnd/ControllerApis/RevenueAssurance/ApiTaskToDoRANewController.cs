using System;
using System.Collections.Generic;
using System.Web.Http;
using ARSystem.Domain.Models;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;


namespace ARSystemFrontEnd.ControllerApis.RevenueAssurance
{
    [RoutePrefix("api/TaskToDoRANew")]
    public class ApiTaskToDoRANewController : ApiController
    {
        [HttpGet, Route("getToDoList")]
        public IHttpActionResult GetToDoList()
        {
            var taskToDoList = new List<dwhTaskToDoRANew>();
            try
            {
                var service = new TaskToDoRANewService();
                taskToDoList = service.GetToDoList();
                return Ok(taskToDoList);
            }
            catch (Exception ex)
            {
                return Ok(taskToDoList);
            }
        }

        [HttpGet, Route("getToDoListDetail")]
        public IHttpActionResult GetToDoListDetail(string ToDoName)
        {
            var taskToDoList = new List<dwhTaskToDoRANewDetail>();
            try
            {
                var service = new TaskToDoRANewService();
                taskToDoList = service.GetToDoListDetail(ToDoName);
                return Ok(taskToDoList);
            }
            catch (Exception ex)
            {
                return Ok(taskToDoList);
            }
        }
      

    }

    public class TaskToDoRANew
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int count { get; set; }
    }

    public class TaskToDoRANewDetail
    {
        public string CustomerName { get; set; }
        public int Stip1 { get; set; }
        public int Stip267 { get; set; }
    }
}