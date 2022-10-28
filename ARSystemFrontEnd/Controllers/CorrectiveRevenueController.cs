
using ARSystem.Domain.Models;
using ARSystem.Service;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Providers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("CorrectiveRevenue")]
    public class CorrectiveRevenueController : BaseController
    {
        // GET: UploadCorrectiveData
        public ActionResult Index()
        {
            string actionTokenView = "57DD8765-A208-450A-95D1-F9C568A22060";

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


        [Route("DownloadExcelTemplate")]
        public ActionResult DownloadExcelTemplate()
        {
            string actionTokenView = "57DD8765-A208-450A-95D1-F9C568A22060";

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

            string fileName = "Upload Corrective Revenue.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/templates/" + fileName));
            return File(bytes, contentType, fileName);
        }

        [HttpGet, Route("ExportDataGenerated")]

        public void ExportDataGenerated()
        {

            string actionTokenView = "57DD8765-A208-450A-95D1-F9C568A22060";
            DataTable table = new DataTable();
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    var result = new List<ArCorrectiveRevenueFinalTemp>();
                    var service = new CorrectiveRevenueService();
                    vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                    result = service.GetDataGenerated(userCredential.UserID);

                    string[] fieldList = new string[] {
                "Number",
                "SONumber",
                "Site_Id",
                "Site_Name",
                "Total_Adjusment",
                "Remark_Adjusment",
                "Net_Revenue",
                "Normal_Revenue",
                "Balance_Amount",
            };


                    try
                    {
                        var reader = FastMember.ObjectReader.Create(result.Select(i => new
                        {
                            Number = i.RowIndex,
                            SONumber = i.SoNumber,
                            Site_Id = i.SiteId,
                            Site_Name = i.SiteName,
                            Total_Adjusment = i.TotalAdjusment,
                            Remark_Adjusment = i.RemarkAdjusment,
                            Net_Revenue = i.NetRevenue,
                            Normal_Revenue = i.NormalRevenue,
                            Balance_Amount = i.BalanceAccrue,
                        }), fieldList);
                        table.Load(reader);

                    }
                    catch (System.Exception)
                    {

                    }
                }

            }

            //Export to Excel
            ExportToExcelHelper.Export("CorrectiveRevenue", table);
        }

    }
}