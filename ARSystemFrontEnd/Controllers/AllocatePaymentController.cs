using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Diagnostics;
using ARSystemFrontEnd.Helper;

using ARSystemFrontEnd.Providers;
using ARSystem.Service.ARSystem;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("AllocatePayment")]
    public class AllocatePaymentController : BaseController
    {
        private AllocatePaymentService _services;

        public AllocatePaymentController()
        {
            _services = new AllocatePaymentService();
        }

        // GET: AllocatePayment
        public ActionResult Index()
        {
            string actionTokenView = "7DA930A7-F884-4A77-9D2A-E0A734455ABE";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
            return View();
        }

        [Route("Summary/Export")]
        public void ExportSummary(vmAllocatePayment param)
        {
            var list = new Datatable<vwtrxAllocatePayment>();

            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            list = _services.GetDataAllocatePayment(userCredential.UserID, param);

            DataTable table = new DataTable();

            string[] fields = new string[] { "PaidDate", "Type", "Company", "Customer", "Amount", "Description", "CreatedDate", "Unsettle", "Status" };

            try
            {
                var reader = FastMember.ObjectReader.Create(list.List.Select(i => new
                {
                    PaidDate = i.PaidDate,
                    Type = i.Type,
                    Company = i.CompanyID,
                    Customer = i.OperatorID,
                    Amount = i.Amount,
                    Description = i.Description,
                    CreatedDate = i.CreatedDate,
                    Unsettle = i.Unsettle,
                    Status = i.Status
                }), fields);
                table.Load(reader);

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
            }
            //Export to Excel
            ExportToExcelHelper.Export("AllocatePaymentSummary", table);
        }
    }
}