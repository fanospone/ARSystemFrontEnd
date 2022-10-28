using ARSystem.Domain.Models;
using ARSystem.Service;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("TaskToDoRANew")]
    public class TaskToDoRANewController : BaseController

    {
        // GET: DashboardRANew
        public ActionResult Index()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            return View();
        }

        [HttpGet]
        public void ExportData(string ToDoName)
        {
            var taskToDoList = new List<dwhTaskToDoRANewDetail>();
            DataTable table = new DataTable();
            try
            {
                var service = new TaskToDoRANewService();
                taskToDoList = service.GetToDoListDetail(ToDoName);
                taskToDoList.Add(new dwhTaskToDoRANewDetail
                {
                    CustomerId = "Total",
                    Stip1 = taskToDoList.Sum(x => x.Stip1),
                    Stip267 = taskToDoList.Sum(x => x.Stip267),
                });

                string[] fieldList = new string[] {
                    "CustomerId",
                    "Stip1",
                    "Stip267"
                 };

                var reader = FastMember.ObjectReader.Create(taskToDoList.Select(i => new
                {
                    i.CustomerId,
                    i.Stip1,
                    i.Stip267

                }), fieldList);
                table.Load(reader);


            }
            catch (Exception)
            {

            }

            ExportToExcelHelper.Export(ToDoName, table);
        }

        [HttpGet,Route("ExportCreateBaps")]
        public async Task ExportCreateBapsData()
        {
            var dataList = new List<vwRATaskTodoCreateBAPS>();
            DataTable table = new DataTable();
            try
            {
                var service = new BapsValidationService();
                dataList = await service.RATaskTodoCreateBAPSList();
               

                string[] fieldList = new string[] {
                    "SoNumber",
                    "SiteID",
                    "SiteName"  ,
                    "BAPSNumber",
                    "CompanyID",
                    "CustomerID",
                    "SIRO",
                    "Product",
                    "RegionName",
                    "BapsType",
                    "STIPCode",
                    "ActivityName"
                 };

                var reader = FastMember.ObjectReader.Create(dataList.Select(i => new
                {
                    i.SoNumber,
                    i.SiteID,
                    i.SiteName,
                    i.BAPSNumber,
                    i.CompanyID,
                    i.CustomerID,
                    i.SIRO,
                    i.Product,
                    i.RegionName,
                    i.BapsType,
                    i.STIPCode,
                    i.ActivityName

                }), fieldList);
                table.Load(reader);


            }
            catch (Exception)
            {

            }

            ExportToExcelHelper.Export("CreateBaps", table);
        }
    }
}