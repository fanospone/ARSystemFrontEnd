using System;
using System.Web.Mvc;
using System.Web;
using ARSystemFrontEnd.Providers;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using ARSystemFrontEnd.Models;
using System.Linq;
using FastMember;
using ARSystemFrontEnd.Helper;

using ARSystem.Domain.Models;
using ARSystem.Service;
using System.Threading.Tasks;
using ARSystem.Domain.Models.ViewModels;
using ARSystem.Service.RevenueAssurance;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Globalization;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("RevenueAssurance")]
    public class RevenueAssuranceController : BaseController
    {
        private readonly ReceiveDocumentBAUKService _revenueAssuranceService;
        private readonly TemplateService _ReportTemplateService;
        private readonly HistoryRejectInvoiceService _historyRejectInvoiceService;
        private readonly SummaryRejectionService _summaryRejectionService;

        public RevenueAssuranceController()
        {
            _revenueAssuranceService = new ReceiveDocumentBAUKService();
            _ReportTemplateService = new TemplateService();
            _historyRejectInvoiceService = new HistoryRejectInvoiceService();
            _summaryRejectionService = new SummaryRejectionService();
        }

        #region Routes

        // Modification Or Added By Ibnu Setiawan 16. May 2019 335F1E09-648E-47C6-9473-A04453FE2D13
        [Authorize]
        [Route("RelocBAPS")]
        public ActionResult RelocBAPS()
        {
            string actionTokenView = "3654D04F-84B4-4715-BF29-86AC17103531";
            string actionTokenProcess = "053CDA8A-9124-4DA6-A7CA-135C593269C6";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }	

        [Authorize]
        [Route("ListReconcile")]
        public ActionResult ListReconcile()
        {
            string actionTokenView = "01951206-2C8D-4D6C-AA44-736BDBF29D71";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("CheckingDocument")]
        public ActionResult CheckingDocument(bool taskTodo = false)
        {
            string actionTokenView = "17B039D8-6431-47B4-BD54-F11194899C8C";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.TaskTodo = taskTodo;
                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
        }

        [Authorize]
        [Route("BAPSValidation")]
        public ActionResult BAPSValidation(bool taskTodo=false)
        {
            string actionTokenView = "fb832a1a-f8a5-4d20-a25d-cfcd936404dc";
            string actionTokenEdit = "51465d41-243d-4c9f-8f05-24924b1efe6a";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.TaskTodo = taskTodo;
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);

                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
        }

        [Authorize]
        [Route("BAPSBulkyValidation")]
        public ActionResult BAPSBulkyValidation(bool taskTodo = false)
        {
            string actionTokenView = "170796aa-c6ca-4b30-8898-2a3ff0e11087";
            string actionTokenEdit = "283e9e22-988f-42cf-af30-2b27ec801d30";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.TaskTodo = taskTodo;
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);

                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
        }

        [Authorize]
        [Route("NewBapsInput")]
        public ActionResult NewBapsInput(bool taskTodo= false, string type="apr")
        {
            string actionTokenView = "EDF5F3B5-A6C5-4437-9ADB-C9EC5E322217";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.TaskTodo = taskTodo;
                    ViewBag.TaskTodoType = type;
                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
        }

        [Authorize]
        [Route("NewBapsInputs")]
        public ActionResult NewBapsInput(string renderType = null, string mOperator = null, string date = "", bool taskTodo = false, string type = "apr")
        {
            string actionTokenView = "EDF5F3B5-A6C5-4437-9ADB-C9EC5E322217";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.TaskTodo = taskTodo;
                    ViewBag.TaskTodoType = type;
                    ViewBag.RenderType = !string.IsNullOrEmpty(renderType) ? renderType : "";
                    ViewBag.Operator = !string.IsNullOrEmpty(mOperator) ? mOperator : "";
                    ViewBag.Date = !string.IsNullOrEmpty(date) ? date : "";
                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
        }

        [Authorize]
        [Route("ReportTemplate")]
        public ActionResult ReportTemplate()
        {
            string actionTokenView = "d12ff46d-1225-447e-b690-0afd09e57ba6";
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
        }

        [Authorize]
        [Route("ReconcileDone")]
        public ActionResult ReconcileDone()
        {
            string actionTokenView = "01951206-2C8D-4D6C-CC44-736BDBF29D71";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("BOQInput")]
        public ActionResult BOQInput()
        {
            string actionTokenView = "39B26683-B816-4323-B934-1B780A0CD1FC";
            string actionTokenProcess = "CC844D71-00F3-4226-8CAD-A62DD2ED1A60";
            string actionTokenApproval = "33ecc0e9-bdc5-4dc9-9171-7ca0fead4504";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    ViewBag.AllowApproval = client.CheckUserAccess(UserManager.User.UserToken, actionTokenApproval);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("POInput")]
        public ActionResult POInput()
        {
            string actionTokenView = "01951206-2C8D-4D6C-FF44-736BDBF29D71";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.DocPath = Helper.Helper.GetDocPath();
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("TimeLineActivity")]
        public ActionResult TimeLineActivity()
        {
            string actionTokenView = "8460b461-b7f7-45cd-9e97-37c3f9bec55d";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("RTI")]
        public ActionResult RTI()
        {
            string actionTokenView = "00b53b36-c37e-4fe0-8f2e-293187eeea3d";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("ImportFromExcel")]
        public ActionResult ImportFromExcel(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

            }

            return Redirect("/RevenueAssurance/ListReconcile");
        }

        [Authorize]
        [Route("SubmitReconcile")]
        public ActionResult SubmitReconcile(HttpPostedFileBase postedFile1, HttpPostedFileBase postedFile2)
        {
            return Redirect("/RevenueAssurance/ListReconcile");
        }

        [Route("DownloadDocument")]
        public ActionResult DownloadDocument()
        {
            string Filepath = Request.QueryString["Filepath"].ToString().Trim();
            string Filename = Request.QueryString["Filename"].ToString().Trim();
            string ContentType = Request.QueryString["ContentType"].ToString().Trim();
            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath(Helper.Helper.GetDocPath() + Filepath));

            return File(bytes, ContentType, Filename);
        }

        [Authorize]
        [Route("BAPSInput")]
        public ActionResult BAPSInput()
        {
            string actionTokenView = "00C9F0C9-0531-45B1-9D75-73187A1AFF17";
            string actionTokenProcess = "6BFB5A89-8533-4B24-85F2-0160F36166BD";
            string actionTokenApproval = "dbac2854-42fe-4d80-bd79-33d9e74573f3";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    ViewBag.AllowApproval = client.CheckUserAccess(UserManager.User.UserToken, actionTokenApproval);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Route("ReportBOQ/Print")]
        public ActionResult ReportBOQPrint()
        {
            //Parameter
            string strBOQNumber = Request.QueryString["BOQNumber"];
            string ID = Request.QueryString["ID"];

            List<ARSystemService.vwBOQDataReport> listBOQDataReport = new List<ARSystemService.vwBOQDataReport>();

            using (var client = new ARSystemService.BOQDataServiceClient())
            {
                listBOQDataReport = client.GetBOQDatReportToList(UserManager.User.UserToken, ID).ToList();
            }

            PostTrxBOQReportHTMLData DataHTML = new PostTrxBOQReportHTMLData();

            DataHTML.listModel = listBOQDataReport;
            DataHTML.htmlHeader = GenerateHTMLHeader(DataHTML.listModel);
            DataHTML.htmlApproval = GenerateHTMLApproval(DataHTML.listModel);
            DataHTML.htmlString = GenerateHtmlBulky(DataHTML.htmlHeader, DataHTML.listModel, DataHTML.htmlApproval);
            if (int.Parse(System.Web.HttpContext.Current.Session["totalPage"].ToString()) > 1)
            {

                return new Rotativa.ViewAsPdf("~/Views/RevenueAssurance/PrintReportBOQ.cshtml", DataHTML)
                {
                    FileName = strBOQNumber + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".pdf",
                    PageSize = Rotativa.Options.Size.A3,
                    MinimumFontSize = 7,
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    CustomSwitches = "--footer-right \" [page] \"" + " --footer-font-size \"7\""
                };
            }
            else
                return new Rotativa.ViewAsPdf("~/Views/RevenueAssurance/PrintReportBOQ.cshtml", DataHTML)
                {
                    FileName = strBOQNumber + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".pdf",
                    PageSize = Rotativa.Options.Size.A3,
                    MinimumFontSize = 7,
                    PageOrientation = Rotativa.Options.Orientation.Landscape
                };

        }


        /*Edit by MTR*/
        [Authorize]
        [Route("ReconcileRejectNonTSEL")]
        public ActionResult ReconcileRejectNonTSEL()
        {
            string actionTokenView = "01951206-2C8D-4D6C-CC44-736BDBF29D71";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        /*Edit by MTR */

        [Authorize]
        [Route("UpdateInflation")]
        public ActionResult UpdateInflation()
        {
            string actionTokenView = "01951206-2C8D-4D6C-AA44-736BDBF29D81";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        private string GenerateHTMLHeader(List<ARSystemService.vwBOQDataReport> list)
        {
            string htmlHeader = "";
            try
            {
                string headerTable = "<table style='width: 100%; text-align: left; font-family: Calibri; font-size: 7px; line-height:1'>";
                headerTable += "<tr style = 'font-weight:bold; font-size:20px'><td>BOQ RECURRING " + list.FirstOrDefault().BatchID + "</td></tr>";
                headerTable += "<tr style = 'font-weight:bold; font-size:20px'><td> TOWER BERSAMA GROUP </td></tr>";
                headerTable += "<tr style = 'font-weight:bold; font-size:14px'><td><br/><br/>Kode " + list.FirstOrDefault().PrintID + "</td></tr>";
                headerTable += "</table><br />";

                htmlHeader = htmlHeader + headerTable;

                return htmlHeader;
            }
            catch (Exception ex)
            {
                return htmlHeader = ex.Message;
            }
        }

        private string GenerateHTMLApproval(List<ARSystemService.vwBOQDataReport> list)
        {
            string htmlApproval = "";
            try
            {
                //foreach (var item in list)
                //{
                string headerTable = "<table style='width: 100%; text-align: center; font-family: Calibri; font-size: 7px'>";
                headerTable += "<tr style = 'border-top:none; height: 100px!important'></tr>";
                headerTable += "<td colspan = '14' align = center style = 'font-weight:bold; font-size:14px'>" + list.FirstOrDefault().OprCompanyName + "</td>";
                headerTable += "<td colspan = '14' align = center style = 'font-weight:bold; font-size:14px'>" + list.FirstOrDefault().ComCompanyName + "</td>";
                headerTable += "<tr style = 'border-top:none; height: 100px!important'></tr>";
                headerTable += "<td colspan = '14' align = center style = 'font-weight:bold; font-size:14px'>" + list.FirstOrDefault().OprPICName + "</td>";
                headerTable += "<td colspan = '14' align = center style = 'font-weight:bold; font-size:14px'>" + list.FirstOrDefault().ComPICName + "</td>";
                headerTable += "<tr style = 'height: 10px!important'></tr>";
                headerTable += "<td colspan = '14' align = center style = 'font-size:14px'>" + list.FirstOrDefault().OprPosition + "</td>";
                headerTable += "<td colspan = '14' align = center style = 'font-size:14px'>" + list.FirstOrDefault().ComPosition + "</td>";
                headerTable += "</table><br />";

                htmlApproval = htmlApproval + headerTable;
                //}

                return htmlApproval;
            }
            catch (Exception ex)
            {
                return htmlApproval = ex.Message;
            }
        }

        private string GenerateHtmlBulky(string strHeader, List<ARSystemService.vwBOQDataReport> list, string strAppr)
        {
            string htmlString = "";
            try
            {

                // ==== html string ====//
                string htmlDiv = "<div>";
                string headerTable = "<table style='width:99%; border-collapse: collapse; font-family:Calibri; table-layout:fixed;word-wrap:break-word;'>";
                headerTable += "<tr>";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:30px; border:1px double black; font-size:14px' > No </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:150px; border:1px double black; font-size:14px' > Company Name </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:70px; border:1px double black; font-size:14px' > Area </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:70px; border:1px double black; font-size:14px' > Region </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:70px; border:1px double black; font-size:14px' > Tsel Site Id Asset </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:70px; border:1px double black; font-size:14px' > Previous Tsel baps site Id</ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:150px; border:1px double black; font-size:14px' > Tsel SiteName </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:150px; border:1px double black; font-size:14px' > Description </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > Long </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > Lat </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:150px !important; border:1px double black; font-size:14px' > Address </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > 1st Year PO </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:110px; border:1px double black; font-size:14px' > 1st Year PO Date</ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:150px; border:1px double black; font-size:14px' > Contract Number </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > 1st BAPS Date </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px;' > RFI Date </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:110px; border:1px double black; font-size:14px' > Start Lease Date</ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > End Lease Date</ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:70px; border:1px double black; font-size:14px' > Contract Periode </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > Basic Price </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > Amount / Month </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > Maint Price </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > Amount / Year </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > Amount Billed </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > Amount Deducated </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > Amount After Deducated</ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:70px; border:1px double black; font-size:14px' > Term </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > Start Renewal </ th >";
                headerTable += "<th style = 'text-align:center; background-color:#d3d3d3; border: 1px solid black; font-weight:bold; width:100px; border:1px double black; font-size:14px' > End Renewal </ th >";

                headerTable += "</tr>";
                //===  end header table ===//
                if (list.Count > 0)
                {
                    int firstRow = 0;
                    int endRow = 0;
                    int rowPerPage = 20;
                    int countData = list.Count();
                    decimal? sumAmountBilled = (from x in list select x.AmountBilled).Sum();
                    string totalAmountBilled = String.Format("{0:#,#.}", sumAmountBilled);
                    decimal? sumAmountDeduction = (from x in list select x.AmountDeduction).Sum();
                    string totalAmountDeduction = (sumAmountDeduction == 0) ? "0" : String.Format("{0:#,#.}", sumAmountDeduction);
                    decimal? sumAmountAfterDeduction = (from x in list select x.AmountAfterDeduction).Sum();
                    string totalAmountAfterDeduction = String.Format("{0:#,#.}", sumAmountAfterDeduction);
                    //(string.IsNullOrEmpty(vw.FirstBAPSDate.ToString())) ? "" : DateTime.Parse(vw.FirstBAPSDate.ToString()).ToString("dd-MMM-yy")
                    // === add grand total == //
                    string htmltotalAmount = "<tr><td colspan='20' style='text-align:right; padding-right:30px'></td>";
                    htmltotalAmount += "<td colspan='3' style='text-align:center; border: 1px solid black; font-weight:bold; font-size:14px; width:70px; border:1px double black'><br /><b>TOTAL</b></td>";
                    htmltotalAmount += "<td style='position:relative;text-align:center; border: 1px solid black; font-weight:bold; width:70px; border:1px double black'><br /><b>" + totalAmountBilled + "</b></td>";
                    htmltotalAmount += "<td style='position:relative;text-align:center; border: 1px solid black; font-weight:bold; width:70px; border:1px double black'><br /><b>" + totalAmountDeduction + "</b></td>";
                    htmltotalAmount += "<td style='position:relative;text-align:center; border: 1px solid black; font-weight:bold; width:70px; border:1px double black'><br /><b>" + totalAmountAfterDeduction + "</b></td>";
                    htmltotalAmount += "<td style='text-align:center;' colspan='3'></td></tr>";

                    //============================//
                    if (countData > rowPerPage)
                    {
                        int totalPage = countData / rowPerPage;

                        if ((totalPage * rowPerPage) < countData)
                        {
                            totalPage += 1;
                        }

                        System.Web.HttpContext.Current.Session["totalPage"] = totalPage;

                        firstRow = 1;
                        endRow = rowPerPage;

                        for (int i = 1; i <= totalPage; i++)
                        {
                            List<ARSystemService.vwBOQDataReport> listLq = new List<ARSystemService.vwBOQDataReport>();
                            listLq = (from a in list
                                      where a.No >= firstRow && a.No <= endRow
                                      select a).ToList();
                            if (i > 1)
                            {
                                htmlDiv = "<div style='page-break-before:always'>";
                            }

                            if (i == totalPage)
                            {
                                if (listLq.Count > 18)
                                {
                                    // === jika halaman terkakhir lebih dari > 15 maka ttd di halaman selanjuta====//
                                    htmlString = htmlString + htmlDiv + strHeader + headerTable;
                                    htmlString = htmlString + GenerateRowTableXLBulky(listLq) + htmltotalAmount;
                                    htmlString += "</table>";
                                    htmlString += "</div>";
                                    htmlString += htmlDiv;
                                    htmlString += strAppr;
                                    htmlString += "</div>";
                                }
                                else
                                {
                                    htmlString = htmlString + htmlDiv + strHeader + headerTable;
                                    htmlString = htmlString + GenerateRowTableXLBulky(listLq) + htmltotalAmount;
                                    htmlString += "</table><br />";
                                    htmlString += strAppr;
                                    htmlString += "</div>";
                                }
                            }
                            else
                            {
                                //if (i == 1)
                                //{
                                //    htmlString = strHeader + htmlString + htmlDiv + headerTable;
                                //    htmlString = htmlString + GenerateRowTableXLBulky(listLq);
                                //    htmlString += "</table>";
                                //    htmlString += "</div>";
                                //}
                                //else
                                //{
                                htmlString = htmlString + htmlDiv + strHeader + headerTable;
                                htmlString = htmlString + GenerateRowTableXLBulky(listLq);
                                htmlString += "</table>";
                                htmlString += "</div>";
                                //}
                            }

                            firstRow = firstRow + rowPerPage;
                            endRow = endRow + rowPerPage;
                        }

                    }
                    else
                    {
                        //  jika data kurang dari jumlah halaman per page //
                        System.Web.HttpContext.Current.Session["totalPage"] = 1;


                        htmlString = htmlString + htmlDiv + strHeader + headerTable;
                        htmlString = htmlString + GenerateRowTableXLBulky(list) + htmltotalAmount;
                        htmlString += "</table><br />";
                        htmlString += strAppr;
                        htmlString += "</div>";
                    }

                }
                else
                {
                    // == jika rows = 0 //
                    htmlString += "</table>";
                    htmlString += "</div>";
                }
                return htmlString;
            }
            catch (Exception ex)
            {
                return htmlString = ex.Message;
            }

        }

        private string GenerateRowTableXLBulky(List<ARSystemService.vwBOQDataReport> list)
        {
            string result = "";
            foreach (var item in list)
            {
                string rowTable = "";
                string poDate = "";
                string FirstbapsDate = "";
                string RFIDate = "";
                string bapsStartDate = "";
                string bapsEndDate = "";
                string StartInvoiceDate = "";
                string EndInvoiceDate = "";

                if (item.InitialPODate.ToString() != "")
                {
                    poDate = Convert.ToDateTime(item.InitialPODate.ToString()).ToString("dd-MMM-yy");
                }
                if (item.FirstBAPSDate.ToString() != "")
                {
                    FirstbapsDate = Convert.ToDateTime(item.FirstBAPSDate.ToString()).ToString("dd-MMM-yy");
                }
                if (item.RFIDate.ToString() != "")
                {
                    RFIDate = Convert.ToDateTime(item.RFIDate.ToString()).ToString("dd-MMM-yy");
                }
                if (item.bapsStartDate.ToString() != "")
                {
                    bapsStartDate = Convert.ToDateTime(item.bapsStartDate.ToString()).ToString("dd-MMM-yy");
                }
                if (item.bapsEndDate.ToString() != "")
                {
                    bapsEndDate = Convert.ToDateTime(item.bapsEndDate.ToString()).ToString("dd-MMM-yy");
                }
                if (item.StartInvoiceDate.ToString() != "")
                {
                    StartInvoiceDate = Convert.ToDateTime(item.StartInvoiceDate.ToString()).ToString("dd-MMM-yy");
                }
                if (item.EndInvoiceDate.ToString() != "")
                {
                    EndInvoiceDate = Convert.ToDateTime(item.EndInvoiceDate.ToString()).ToString("dd-MMM-yy");
                }

                rowTable += "<tr>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:30px;  border:1px double black; font-size:13px'>" + item.No + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:150px; border:1px double black; font-size:13px'>" + item.CompanyName + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:70px;  border:1px double black; font-size:13px'>" + item.Area + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:70px;  border:1px double black; font-size:13px'>" + item.RegionName + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:70px;  border:1px double black; font-size:13px'>" + item.SiteID + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:70px;  border:1px double black; font-size:13px'>" + item.PrevSiteID + "</td>";
                rowTable += "<td style='text-align:left; padding: 2px; border: 1px solid black; width:150px; border:1px double black; font-size:13px'>" + item.SiteName + "</td>";
                rowTable += "<td style='text-align:left; padding: 2px; border: 1px solid black; width:150px; border:1px double black; font-size:13px'>" + item.Decription + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + item.Longitude + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:150px;width:30px; border:1px double black; font-size:13px'>" + item.Latitude + "</td>";
                rowTable += "<td style='text-align:left; padding: 2px; border: 1px solid black; width:150px !important; border:1px double black; font-size:13px'>" + item.Address + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + item.InitialPONumber + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:110px; border:1px double black; font-size:13px'>" + poDate + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:150px; border:1px double black; font-size:13px'>" + item.Mla + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + FirstbapsDate + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + RFIDate + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:110px; border:1px double black; font-size:13px'>" + bapsStartDate + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + bapsEndDate + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:70px;  border:1px double black; font-size:13px'>" + item.ContractPeriod + "</td>";
                rowTable += "<td style='text-align:right; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + String.Format("{0:n0}", item.BaseLeasePrice) + "</td>";
                rowTable += "<td style='text-align:right; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + String.Format("{0:n0}", item.ServicePrice) + "</td>";
                rowTable += "<td style='text-align:right; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + String.Format("{0:n0}", item.AmountPerMonth) + "</td>";
                rowTable += "<td style='text-align:right; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + String.Format("{0:n0}", item.AmountPerYear) + "</td>";
                rowTable += "<td style='text-align:right; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + String.Format("{0:n0}", item.AmountBilled) + "</td>";
                rowTable += "<td style='text-align:right; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + String.Format("{0:n0}", item.AmountDeduction) + "</td>";
                rowTable += "<td style='text-align:right; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + String.Format("{0:n0}", item.AmountAfterDeduction) + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:70px;  border:1px double black; font-size:13px'>" + item.Term + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + StartInvoiceDate + "</td>";
                rowTable += "<td style='text-align:center; padding: 2px; border: 1px solid black; width:100px; border:1px double black; font-size:13px'>" + EndInvoiceDate + "</td>";
                rowTable += "</tr>";
                result = result + rowTable;

            }
            return result;
        }

        [Authorize]
        [Route("ReOpenPaymentDate")]
        public ActionResult ReOpenPaymentDate()
        {
            string actionTokenView = "3F5F5730-1626-4B1A-86CD-B45145900F2E";
            //string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ARSystemService.vmStringResult Result = new ARSystemService.vmStringResult();
                    using (var client2 = new ARSystemService.UserServiceClient())
                    {
                        Result = client2.GetARUserPosition(UserManager.User.UserToken);
                        if(Result.Result.ToString().ToUpper()=="DEPT HEAD")
                        {
                            ViewBag.SummaryEdit = true;
                            ViewBag.Request = false;
                        }else
                        {
                            ViewBag.SummaryEdit = false;
                            ViewBag.Request = true;
                        }
                    }

                    
                    
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
            
            return View();
        }

        [Authorize]
        [Route("BackStatusBAPSValidation")]
        public ActionResult BackStatusBAPSValidation()
        {
            string actionTokenView = "84cf4a34-624b-49fe-b0da-0dac8eec5ae6";
            string actionTokenProcess = "b5065beb-259c-47bd-a884-8f9e7d0dd19a";
            
            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("ReceiveDocumentBAUK")]
        public ActionResult ReceiveDocumentBAUK(bool taskTodo=false)
        {
            //string actionTokenView = "17B039D8-6431-47B4-BD54-F11194899C8C";
            //using (var client = new SecureAccessService.UserServiceClient())
            //{
            //    if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
            //    {
            //        return View();
            //    }
            //    else
            //    {
            //        return RedirectToAction("Logout", "Login");
            //    }
            //}
            ViewBag.TaskTodo = taskTodo;
            return View();
        }

        /*Add by NHF 17 Nov 21 (Reject Invoice)*/
        [Authorize]
        [Route("HistoryRejectInvoice")]
        public ActionResult HistoryRejectInvoice()
        {
            string actionTokenView = "83617E3A-F8A1-46B4-88BE-5625A2F41223";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("SummaryRejection")]
        public ActionResult SummaryRejection()
        {
            string actionTokenView = "7A164FE9-9FE5-4328-97FD-CCFECEB38EDF";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        /*End */

        #endregion

        #region Export To Excel

        // Modification Or Added By Ibnu Setiawan 22. February 2019
        [Route("POExport")]
        public void ExportToExcelInputPO()
        {
            //Parameter
            PostFilterReconcilePO post = new PostFilterReconcilePO();
            post.strCompanyId = Request.QueryString["strCompanyId"].ToString();
            post.strCustomerId = Request.QueryString["strCustomerId"].ToString();
            post.strStipID = Request.QueryString["strStipID"].ToString();
            post.strCurrency = Request.QueryString["strCurrency"].ToString();
            post.strSONumber = Request.QueryString["strSONumber"].ToString();
            post.strQuarterly = Request.QueryString["strQuarterly"].ToString();
            post.strYear = Request.QueryString["strYear"].ToString();
            post.strProduct = Request.QueryString["strProduct"].ToString();
            post.strBapsType = Request.QueryString["strBapsType"].ToString();
            post.strPowerType = Request.QueryString["strPowerType"].ToString();
            post.strSiteID = Request.QueryString["strSiteID"].ToString();
            post.strSiteName = Request.QueryString["strSiteName"].ToString();
            string strWhereClause = GetParam(post, " mstRAActivityID = 3 AND ");

            List<ARSystemService.vwRAData> reconciledata = new List<ARSystemService.vwRAData>();
            //Call Service
            using (var client = new ARSystemService.POInputServiceClient())
            {
                int intTotalRecord = 0;

                //intTotalRecord = client.GetReconcileDataCount(UserManager.User.UserToken, strCompanyId, strOperator, strRenewalYear, strRenewalYearSeq, strReconcileType, strCurrency, strRegional, strProvince, strDueDatePerMonth, Batch, 0);

                string strOrderBy = "";

                var data = client.GetReconcileToList(UserManager.User.UserToken, strWhereClause, post.strBapsType, strOrderBy, 0, Int32.MaxValue).ToList();
                reconciledata.AddRange(data);

                string[] fieldList = new string[] {
                        "SONumber","CompanyInvoice","SiteID","SiteName","CustomerSiteID","CustomerSiteName","CustomerID","Term","StartInvoiceDate","EndInvoiceDate",
                        "BaseLeasePrice","ServicePrice","InflationAmount","AdditionalAmount","AmountIDR"
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(reconciledata, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("PO_Input", table);
            }
        }

        private string GetParam(PostFilterReconcilePO post, string strWhereClause = "")
        {
            if (!string.IsNullOrWhiteSpace(post.strFilterID))
            {
                strWhereClause += "id in (" + post.strFilterID + ") AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strCompanyId))
            {
                strWhereClause += "CompanyInvoice = '" + post.strCompanyId + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strCustomerId))
            {
                strWhereClause += "CustomerInvoice = '" + post.strCustomerId + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPowerType))
            {
                strWhereClause += "PowerTypeID = " + post.strPowerType + " AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strProduct))
            {
                strWhereClause += "ProductID = " + post.strProduct + " AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strQuarterly))
            {
                strWhereClause += "Quartal = " + post.strQuarterly + " AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strCurrency))
            {
                strWhereClause += "BaseLeaseCurrency = '" + post.strCurrency + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteID))
            {
                strWhereClause += "SiteID LIKE '%" + post.strSiteID + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteName))
            {
                strWhereClause += "SiteName LIKE '%" + post.strSiteName + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSONumber))
            {
                strWhereClause += "SONumber LIKE '%" + post.strSONumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strBapsType))
            {
                strWhereClause += "BapsType LIKE '%" + post.strBapsType + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strStipID))
            {
                strWhereClause += "STIPID = " + post.strStipID + " AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strYear))
            {
                strWhereClause += "YEAR = " + post.strYear + " AND ";
            }

            strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

            return strWhereClause;
        }
        /*End OF Modif*/
        [Route("Input/Export")]
        public void ExportToExcelInputRaw()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strOperator = Request.QueryString["strOperator"].ToString();
            string strRenewalYear = Request.QueryString["strRenewalYear"].ToString();
            string strRenewalYearSeq = Request.QueryString["strRenewalYearSeq"].ToString();
            string strReconcileType = Request.QueryString["strReconcileType"].ToString();
            string strCurrency = Request.QueryString["strCurrency"].ToString();
            string strRegional = Request.QueryString["strRegional"].ToString();
            string strProvince = Request.QueryString["strProvince"].ToString();
            string strDueDatePerMonth = Request.QueryString["strDueDatePerMonth"].ToString();
            string Batch = Request.QueryString["Batch"].ToString();
            string TenantType = Request.QueryString["TenantType"].ToString();
            string SONumber = Request.QueryString["SONumber"].ToString();
            int length = int.Parse(Request.QueryString["length"]) - 1;
            int isRaw = int.Parse(Request.QueryString["isRaw"]);

            List<string> SONumberFilter = new List<string>();
            if (!string.IsNullOrEmpty(SONumber))
            {
                string[] str = SONumber.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    var item = str[i].ToString();
                    SONumberFilter.Add(item);
                }
            }

            List<ARSystemService.vwRAReconcile> reconciledata = new List<ARSystemService.vwRAReconcile>();
            //Call Service
            using (var client = new ARSystemService.ReconcileDataServiceClient())
            {
                int intTotalRecord = 0;

                //intTotalRecord = client.GetReconcileDataCount(UserManager.User.UserToken, strCompanyId, strOperator, strRenewalYear, strRenewalYearSeq, strReconcileType, strCurrency, strRegional, strProvince, strDueDatePerMonth, Batch, 0);

                string strOrderBy = "";

                var data = client.GetReconcileDataToList(UserManager.User.UserToken, strCompanyId, strOperator, strRenewalYear, strRenewalYearSeq, strReconcileType, strCurrency, strRegional, strProvince, strDueDatePerMonth, Batch, SONumberFilter.ToArray(), TenantType, strOrderBy, 0, intTotalRecord, isRaw).ToList();
                reconciledata.AddRange(data);

                string[] fieldList = new string[] {
                        "RowIndex","SONumber","SiteID","SiteName","CustomerSiteID","CustomerSiteName","CustomerID","RegionalName","ProvinceName","ResidenceName",
                        "PONumber","MLANumber","StartBapsDate","EndBapsDate","RFIDate","BaufDate","Term","BapsType","CustomerInvoice",
                        "CompanyInvoice","Company","StipSiro","InvoiceTypeName","Currency","StartInvoiceDate","EndInvoiceDate","BaseLeasePrice",
                        "ServicePrice","DeductionAmount","TotalPaymentRupiah"
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(reconciledata, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("Recurring Input", table);
            }
        }

        [Route("Upload/Export")]
        public void ExportToExcelUploadRegional()
        {
            //Parameter
            string strOperator = Request.QueryString["strOperator"].ToString();
            string strRegional = Request.QueryString["strRegional"].ToString();
            string RegionID = Request.QueryString["RegionID"].ToString();
            string Batch = Request.QueryString["Batch"].ToString();


            List<ARSystemService.vwRAReconcile> reconciledata = new List<ARSystemService.vwRAReconcile>();
            //Call Service
            using (var client = new ARSystemService.ReconcileDataServiceClient())
            {
                int intTotalRecord = 0;

                //intTotalRecord = client.GetReconcileDataCount(UserManager.User.UserToken, strCompanyId, strOperator, strRenewalYear, strRenewalYearSeq, strReconcileType, strCurrency, strRegional, strProvince, strDueDatePerMonth, Batch, 0);

                string strOrderBy = "";

                var data = client.GetReconcileSiteRegionalToList(UserManager.User.UserToken, "", strOperator, strRegional, RegionID, Batch, "", 0, 100000);
                //var data = client.GetReconcileDataToList(UserManager.User.UserToken, strCompanyId, strOperator, strRenewalYear, strRenewalYearSeq, strReconcileType, strCurrency, strRegional, strProvince, strDueDatePerMonth, Batch, SONumberFilter.ToArray(), TenantType, strOrderBy, 0, intTotalRecord, isRaw).ToList();
                reconciledata.AddRange(data);

                string[] fieldList = new string[] {
                        "SONumber",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "CompanyInvoice",
                        "CompanyInvoiceName",
                        "RegionName",
                        "CustomerRegionName",
                        "Term",
                        "StartInvoiceDate",
                        "EndInvoiceDate",
                        "StartBapsDate",
                        "EndBapsDate",
                        "BaseLeaseCurrency",
                        "BaseLeasePrice",
                        "ServiceCurrency",
                        "ServicePrice",
                        "DeductionCurrency",
                        "DeductionAmount",
                        "TotalAmount"
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(reconciledata, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("List Site", table);
            }
        }
        //public void ExportToExcelUploadRegional()
        //{
        //    //Parameter
        //    string strOperator = Request.QueryString["strOperator"].ToString();
        //    string strRegional = Request.QueryString["strRegional"].ToString();
        //    string RegionID = Request.QueryString["RegionID"].ToString();
        //    string Batch = Request.QueryString["Batch"].ToString();


        //    List<ARSystemService.vwRAReconcile> reconciledata = new List<ARSystemService.vwRAReconcile>();
        //    //Call Service
        //    using (var client = new ARSystemService.ReconcileDataServiceClient())
        //    {
        //        int intTotalRecord = 0;

        //        //intTotalRecord = client.GetReconcileDataCount(UserManager.User.UserToken, strCompanyId, strOperator, strRenewalYear, strRenewalYearSeq, strReconcileType, strCurrency, strRegional, strProvince, strDueDatePerMonth, Batch, 0);

        //        string strOrderBy = "";

        //        var data = client.GetReconcileSiteRegionalToList(UserManager.User.UserToken,"", strOperator, strRegional, RegionID, Batch, "",0,100000);
        //        //var data = client.GetReconcileDataToList(UserManager.User.UserToken, strCompanyId, strOperator, strRenewalYear, strRenewalYearSeq, strReconcileType, strCurrency, strRegional, strProvince, strDueDatePerMonth, Batch, SONumberFilter.ToArray(), TenantType, strOrderBy, 0, intTotalRecord, isRaw).ToList();
        //        reconciledata.AddRange(data);

        //        string[] fieldList = new string[] {
        //                "SONumber",
        //                "SiteID",
        //                "SiteName",
        //                "CustomerSiteID",
        //                "CustomerSiteName",
        //                "CompanyInvoice",
        //                "CompanyInvoiceName",
        //                "RegionName",
        //                "CustomerRegionName",
        //                "Term",
        //                "StartInvoiceDate",
        //                "EndInvoiceDate",
        //                "StartBapsDate",
        //                "EndBapsDate",
        //                "BaseLeaseCurrency",
        //                "BaseLeasePrice",
        //                "ServiceCurrency",
        //                "ServicePrice",
        //                "DeductionCurrency",
        //                "DeductionAmount",
        //                "TotalAmount"
        //            };

        //        DataTable table = new DataTable();
        //        var reader = FastMember.ObjectReader.Create(reconciledata, fieldList);
        //        table.Load(reader);

        //        //Export to Excel
        //        ExportToExcelHelper.Export("List Site", table);
        //    }
        //}
        /*
        [Route("Done/Export")]
        public void ExportToExcelDoneRaw()
        {
            //Parameter
            string strOperator = Request.QueryString["strOperator"].ToString();
            string strRenewalYear = Request.QueryString["strRenewalYear"].ToString();
            string strYear = Request.QueryString["strYear"].ToString();
            string strResidence = Request.QueryString["strResidence"].ToString();
            string strCurrency = Request.QueryString["strCurrency"].ToString();
            string strRegional = Request.QueryString["strRegional"].ToString();
            string strProvince = Request.QueryString["strProvince"].ToString();
            int length = int.Parse(Request.QueryString["length"]) -1;
            int isRaw = int.Parse(Request.QueryString["isRaw"]);

            List<ARSystemService.vwReconcileDone> reconciledata = new List<ARSystemService.vwReconcileDone>();
            //Call Service
            using (var client = new ARSystemService.ReconcileDoneServiceClient())
            {
                int intTotalRecord = 0;

                intTotalRecord = client.GetReconcileDoneCount(UserManager.User.UserToken, strOperator, strYear, strResidence, strRegional, strProvince, isRaw);


                string strOrderBy = "";

                if (intTotalRecord > 0)
                {
                    var data = client.GetReconcileDoneToList(UserManager.User.UserToken, strOperator, strYear, strResidence, strRegional, strProvince, strOrderBy, 0, length, isRaw).ToList();
                    reconciledata.AddRange(data);

                    string[] fieldList = new string[] {
                        "ReconProcessedId","Operator","RegionId","ProvinceName","RegionId","ResidenceName","ReconcileDate",
                        "Year","TotalRenewalTenant","Currency","TotalAmount","BAReconcile","BAOther","FinalDocument"
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(reconciledata, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("Recurring Done", table);
                }
            }
        }
        */
        [Route("trxBOQInput/Export")]
        public void GettrxBOQInputToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strYear = Request.QueryString["strYear"];
            string strArea = Request.QueryString["strArea"];
            string strRegional = Request.QueryString["strRegional"];
            string SONumber = Request.QueryString["strSOnumber"];
            var strSONumber = new List<string>();

            if (!string.IsNullOrEmpty(SONumber))
            {
                foreach (var item in SONumber.Split(','))
                {
                    strSONumber.Add(item);
                }
            }
            else
            {
                strSONumber.Add("0");
            }

            //Call Service
            List<ARSystemService.vwBOQData> listBOQData = new List<ARSystemService.vwBOQData>();
            using (var client = new ARSystemService.BOQDataServiceClient())
            {
                int intTotalRecord = client.GetBOQDataCount(UserManager.User.UserToken, strCompanyId, strOperator, strYear, strArea, strRegional, strSONumber.ToArray());
                //int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwBOQData> listBOQDataHolder = new List<ARSystemService.vwBOQData>();

                //for (int i = 0; i <= intBatch; i++)
                //{
                    listBOQDataHolder = client.GetBOQDataToList(UserManager.User.UserToken, strCompanyId, strOperator, strYear, strArea, strRegional, strSONumber.ToArray(), 0, intTotalRecord).ToList();
                    listBOQData.AddRange(listBOQDataHolder);
                //}
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            
            string[] ColumsShow = new string[] {"SoNumber"
                ,"SiteID"
                ,"SiteName"
                ,"Operator"
                ,"CustomerSiteID"
                ,"CustomerSiteName"
                //,"Customer Invoice ID"
                ,"CustomerInvoice"
                //,"Customer Asset ID"
                ,"CustomerAsset"
                //,"AreaID"
                ,"Area"
                //,"RegionID"
                ,"Region"
                ,"Province"
                ,"Residence"
                ,"StartLeaseDate"
                ,"EndLeaseDate"
                ,"Term"
                ,"InitPONumber"
                ,"MLANumber"
                //,"InvoiceTypeId"
                ,"InvoiceType"
                ,"InvoiceStartDate"
                ,"InvoiceEndDate"
                ,"BaseLeasePrice"
                ,"OMPrice"
                ,"AmountBilled"
                ,"AmountDeduction"
                ,"AmountAfterDeduction"
            };
            var reader = FastMember.ObjectReader.Create(listBOQData.Select(i => new
            {
                i.SoNumber
               ,
                i.SiteID
               ,
                i.SiteName
               ,
                i.Operator
               ,
                CustomerSiteID = i.SiteIDOperator
               ,
                CustomerSiteName = i.SiteNameOperator
               //,
               // i.CompanyInvID
               ,
                CustomerInvoice = i.CompanyInv
               //,
               // i.CompanyAssetID
               ,
                CustomerAsset = i.CompanyAsset
               //,
               // i.AreaID
               ,
                i.Area
               ,
               // i.RegionID
               //,
                i.Region
               ,
                i.Province
               ,
                i.Residence
               ,
                i.StartLeaseDate
               ,
                i.EndLeaseDate
               ,
                i.Term
               ,
                i.InitPONumber
               ,
                i.MLANumber
               //,
               // i.InvoiceTypeId
               ,
                i.InvoiceType
               ,
                i.InvoiceStartDate
               ,
                i.InvoiceEndDate
               ,
                i.BaseLeasePrice
               ,
                i.OMPrice
                ,
                 i.AmountBilled
                ,
                 i.AmountDeduction
                ,
                AmountAfterDeduction = i.TotalAmount

            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("trxBOQInput", table);
        }

        [Route("trxBOQProcess/Export")]
        public void GettrxBOQProcessToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string BOQNumber = Request.QueryString["strBOQNumber"];
            var strBOQNumber = new List<string>();
            //strSONumber.Add("0");

            if (!string.IsNullOrEmpty(BOQNumber))
            {
                foreach (var item in BOQNumber.Split(','))
                {
                    strBOQNumber.Add(item);
                }
            }
            else
            {
                strBOQNumber.Add("0");
            }

            //Call Service
            List<ARSystemService.vwBOQDataHeader> listBOQData = new List<ARSystemService.vwBOQDataHeader>();
            using (var client = new ARSystemService.BOQDataServiceClient())
            {
                int intTotalRecord = client.GetBOQDataProcessCount(UserManager.User.UserToken, strCompanyId, strBOQNumber.ToArray());
                //int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwBOQDataHeader> listBOQDataHolder = new List<ARSystemService.vwBOQDataHeader>();

                //for (int i = 0; i <= intBatch; i++)
                //{
                    listBOQDataHolder = client.GetBOQDataProcessToList(UserManager.User.UserToken, strCompanyId, strBOQNumber.ToArray(), "", 0, intTotalRecord).ToList();
                    listBOQData.AddRange(listBOQDataHolder);
                //}
            }

            //Convert to DataTable
            DataTable table = new DataTable();

            string[] ColumsShow = new string[]
            {   "BOQNumber"
                ,"Area"
                ,"Company"
                ,"TotalTenant"
                ,"TotalAmount"
                ,"CreatedDate"
                ,"CreatedBy"
            };
            var reader = FastMember.ObjectReader.Create(listBOQData.Select(i => new
            {
                i.BOQNumber
                ,
                i.Area
                ,
                i.Company
                ,
                i.TotalTenant
                ,
                i.TotalAmount
                ,
                i.CreatedDate
                ,
                i.CreatedBy
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("trxBOQProcess", table);
        }

        [Route("trxBOQDone/Export")]
        public void GettrxBOQDoneToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string BOQNumber = Request.QueryString["strBOQNumber"];
            var strBOQNumber = new List<string>();
            //strSONumber.Add("0");

            if (!string.IsNullOrEmpty(BOQNumber))
            {
                foreach (var item in BOQNumber.Split(','))
                {
                    strBOQNumber.Add(item);
                }
            }
            else
            {
                strBOQNumber.Add("0");
            }

            //Call Service
            List<ARSystemService.vwBOQDataDoneHeader> listBOQData = new List<ARSystemService.vwBOQDataDoneHeader>();
            using (var client = new ARSystemService.BOQDataServiceClient())
            {
                int intTotalRecord = client.GetBOQDataDoneCount(UserManager.User.UserToken, strCompanyId, strBOQNumber.ToArray());
                //int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwBOQDataDoneHeader> listBOQDataHolder = new List<ARSystemService.vwBOQDataDoneHeader>();

                //for (int i = 0; i <= intBatch; i++)
                //{
                    listBOQDataHolder = client.GetBOQDataDoneToList(UserManager.User.UserToken, strCompanyId, strBOQNumber.ToArray(), "", 0, intTotalRecord).ToList();
                    listBOQData.AddRange(listBOQDataHolder);
                //}
            }

            //Convert to DataTable
            DataTable table = new DataTable();

            string[] ColumsShow = new string[] {"BOQNumber"
                ,"Area"
                ,"Company"
                ,"TotalTenant"
                ,"TotalAmount"
                ,"CreatedDate"
                ,"CreatedBy"
                ,"ApprovedDate"
                ,"ApprovedBy"
            };
            var reader = FastMember.ObjectReader.Create(listBOQData.Select(i => new
            {
                i.BOQNumber
                ,
                i.Area
                ,
                i.Company
                ,
                i.TotalTenant
                ,
                i.TotalAmount
                ,
                i.CreatedDate
                ,
                i.CreatedBy
                ,
                i.ApprovedDate
                ,
                i.ApprovedBy
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("trxBOQDone", table);
        }

        [Route("CheckingDocument/Export")]
        public void CheckingDocumentExport()
        {
            //Parameter
            string strCustomerID = Request.QueryString["strCustomerID"].ToString();
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strProductId = Request.QueryString["strProductId"].ToString();
            string strSoNumber = Request.QueryString["strSoNumber"].ToString();
            string strSiteID = Request.QueryString["strSiteID"].ToString();
            string mstRAActivityID = Request.QueryString["mstRAActivityID"].ToString();

            List<ARSystemService.vmNewBapsData> model = new List<ARSystemService.vmNewBapsData>();
            //Call Service
            using (var client = new ARSystemService.NewBapsServiceClient())
            {
                var data = client.GetSoNumbList(UserManager.User.UserToken, strCompanyId, strCustomerID, strProductId, strSoNumber, strSiteID, "", mstRAActivityID, "", 0, 100000).ToList();

                model.AddRange(data);

                string[] fieldList = new string[] {
                        "SoNumber",
                        "CustomerID",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "CompanyID",
                        "CompanyName",
                        "SIRO",
                        "STIPNumber",
                        "Product",
                        "MLANumber",
                        "MLADate",
                        "BaukNumber",
                        "BaukDate"
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(model, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("List CheckDoc", table);
            }
        }

        [Route("BAPSValidation/Export")]
        public void BAPSValidationExport()
        {
            //Parameter
            string strCustomerID = Request.QueryString["strCustomerID"].ToString();
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strProductId = Request.QueryString["strProductId"].ToString();
            string strSoNumber = Request.QueryString["strSoNumber"].ToString();
            string strSiteID = Request.QueryString["strSiteID"].ToString();
            string mstRAActivityID = Request.QueryString["mstRAActivityID"].ToString();
            string strTenantType = Request.QueryString["strTenantType"].ToString();
            string strBaukDoneYear = Request.QueryString["strBaukDoneYear"].ToString();

            List<ARSystemService.vmNewBapsData> model = new List<ARSystemService.vmNewBapsData>();
            //Call Service
            using (var client = new ARSystemService.BAPSValidationClient())
            {
                var data = client.GetBapsDataValidationList(UserManager.User.UserToken, strCompanyId, strCustomerID, strProductId, strSoNumber, strSiteID, strTenantType, mstRAActivityID, int.Parse(strBaukDoneYear), "", 0, 100000).ToList();

                model.AddRange(data);

                string[] fieldList = new string[] {
                        "SoNumber",
                        "TowerTypeID",
                        "CustomerID",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "CompanyID",
                        "CompanyName",
                        "SIRO",
                        "STIPNumber",
                        "StipCode",
                        "Product",
                        "MLANumber",
                        "MLADate",
                        "BaukNumber",
                        "BaukDate",
                        "PoAmount",
                        "PoDate"
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(model, fieldList);
                table.Load(reader);

                //DataTable table = new DataTable();
                //using (var reader = ObjectReader.Create(data))
                //{
                //    table.Load(reader);
                //}

                //Export to Excel
                ExportToExcelHelper.Export("BAPSValidation", table);
            }
        }

        [Route("BAPSWaitingPo/Export")]
        public void BAPSWaitingPoExport()
        {
            //Parameter
            string strCustomerID = Request.QueryString["strCustomerID"].ToString();
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strProductId = Request.QueryString["strProductId"].ToString();
            string strSoNumber = Request.QueryString["strSoNumber"].ToString();
            string strSiteID = Request.QueryString["strSiteID"].ToString();
            string mstRAActivityID = Request.QueryString["mstRAActivityID"].ToString();
            string strTenantType = Request.QueryString["strTenantType"].ToString();

            List<ARSystemService.vmNewBapsData> model = new List<ARSystemService.vmNewBapsData>();
            //Call Service
            using (var client = new ARSystemService.BAPSValidationClient())
            {
                var data = client.GetBapsWaitingPoList(UserManager.User.UserToken, strCompanyId, strCustomerID, strProductId, strSoNumber, strSiteID, strTenantType, mstRAActivityID, "", 0, 1000000).ToList();

                model.AddRange(data);

                string[] fieldList = new string[] {
                        "SoNumber",
                        "TowerTypeID",
                        "CustomerID",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "CompanyID",
                        "CompanyName",
                        "SIRO",
                        "StipCode",
                        "Product",
                        "PONumber",
                        "PoAmount",
                        "PoDate"
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(model, fieldList);
                table.Load(reader);

                //DataTable table = new DataTable();
                //using (var reader = ObjectReader.Create(data))
                //{
                //    table.Load(reader);
                //}

                //Export to Excel
                ExportToExcelHelper.Export("BAPSWaitingPo", table);
            }
        }

        [Route("NewBaps/Export")]
        public void NewBapsExport()
        {
            //Parameter
            string strCustomerID = Request.QueryString["strCustomerID"].ToString();
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strProductId = Request.QueryString["strProductId"].ToString();
            string strSoNumber = Request.QueryString["strSoNumber"].ToString();
            string strSiteID = Request.QueryString["strSiteID"].ToString();
            string mstRAActivityID = Request.QueryString["mstRAActivityID"].ToString();
            string strTenantType = Request.QueryString["strTenantType"].ToString();

            List<ARSystemService.vmNewBapsData> model = new List<ARSystemService.vmNewBapsData>();
            //Call Service
            using (var client = new ARSystemService.NewBapsServiceClient())
            {
                var data = client.GetSoNumbList(UserManager.User.UserToken, strCompanyId, strCustomerID, strProductId, strSoNumber, strSiteID, strTenantType, mstRAActivityID, "", 0, 100000).ToList();

                model.AddRange(data);

                string[] fieldList = new string[] {
                        "SoNumber",
                        "TowerTypeID",
                        "CustomerID",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "CompanyID",
                        "CompanyName",
                        "SIRO",
                        "STIPNumber",
                        "StipCode",
                        "Product",
                        "MLANumber",
                        "MLADate",
                        "BaukNumber",
                        "BaukDate",
                        "PoAmount",
                        "PoDate"
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(model, fieldList);
                table.Load(reader);

                //DataTable table = new DataTable();
                //using (var reader = ObjectReader.Create(data))
                //{
                //    table.Load(reader);
                //}

                //Export to Excel
                ExportToExcelHelper.Export("NewBaps", table);
            }
        }

        [Route("ReconcileRejectNonTSEL/Export")]
        public void ReconcileRejectNonTSELExport()
        {
            //Parameter
            string strBAPSTypeID = Request.QueryString["strBAPSTypeID"].ToString();
            string strProductID = Request.QueryString["strProductID"].ToString();
            string strYear = Request.QueryString["strYear"].ToString();
            string strSTIPID = Request.QueryString["strSTIPID"].ToString();
            string strSONumber = Request.QueryString["strSONumber"].ToString();
            string strSiteID = Request.QueryString["strSiteID"].ToString();
            string strInvoiceTypeID = Request.QueryString["strInvoiceTypeID"].ToString();
            string strPowerTypeID = Request.QueryString["strPowerTypeID"].ToString();
            string strCompanyID = Request.QueryString["strCompanyID"].ToString();
            string strCustomerID = Request.QueryString["strCustomerID"].ToString();
            int intTotalRecord = 0;

            List<ARSystemService.vwRAReconcileRejectNonTSEL> modelList = new List<ARSystemService.vwRAReconcileRejectNonTSEL>();
            ARSystemService.vwRAReconcileRejectNonTSEL model = new ARSystemService.vwRAReconcileRejectNonTSEL();
            using (var client = new ARSystemService.ItrxReconcileRejectNonTSELServiceClient())
            {

                model.mstBapsTypeID = int.Parse(strBAPSTypeID);
                model.ProductID = int.Parse(strProductID);
                model.Year = int.Parse(strYear);
                model.STIPID = int.Parse(strSTIPID);
                model.SoNumber = strSONumber;
                model.SiteID = strSiteID;
                model.InvoiceTypeID = strInvoiceTypeID;
                model.PowerTypeID = int.Parse(strPowerTypeID);
                model.CompanyInvoiceId = strCompanyID;
                model.CustomerInvID = strCustomerID;

                modelList = client.GetListReconcileRejectNonTSEL(UserManager.User.UserToken, model, 0, 0).ToList();

                string[] fieldList = new string[] {
                    "RowIndex",
                    "SoNumber",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "Term",
                        "StartInvoiceDate",
                        "EndInvoiceDate",
                        "BaseLeasePrice",
                        "ServicePrice",
                        "AdditionalAmount",
                        "DeductionAmount",
                        "InflationAmount",
                        "AmountIDR",
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(modelList, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("Reconcile Reject", table);
            }
            //Call Service
        }

        [Route("Input/ExportRTI")]
        public void ExportToExcelInputRTI()
        {
            //Parameter

            string CompanyID = Request.QueryString["strCompanyId"].ToString();
            string CustomerID = Request.QueryString["strOperator"].ToString();

            string BAPSNumber = Request.QueryString["strBAPS"].ToString();
            var strBAPSNumber = new List<string>();

            if (!string.IsNullOrEmpty(BAPSNumber))
            {
                foreach (var item in BAPSNumber.Split(','))
                {
                    strBAPSNumber.Add(item);
                }
            }
            else
            {
                strBAPSNumber.Add("0");
            }

            string PONumber = Request.QueryString["strPO"].ToString();
            var strPONumber = new List<string>();

            if (!string.IsNullOrEmpty(PONumber))
            {
                foreach (var item in PONumber.Split(','))
                {
                    strPONumber.Add(item);
                }
            }
            else
            {
                strBAPSNumber.Add("0");
            }

            string SONumber = Request.QueryString["SONumber"].ToString();
            var strSONumber = new List<string>();

            if (!string.IsNullOrEmpty(SONumber))
            {
                foreach (var item in SONumber.Split(','))
                {
                    strSONumber.Add(item);
                }
            }
            else
            {
                strSONumber.Add("0");
            }

            //string PONumber = Request.QueryString["strPO"].ToString();
            //var strPONumber = new List<string>();

            //if (!string.IsNullOrEmpty(SONumber))
            //{
            //    foreach (var item in PONumber.Split(','))
            //    {
            //        strPONumber.Add(item);
            //    }
            //}
            //else
            //{
            //    strPONumber.Add("0");
            //}

            string Year = Request.QueryString["strYear"].ToString();
            string Quartal = Request.QueryString["strQuartal"].ToString();
            string BapsType = Request.QueryString["strBapsType"].ToString();
            string PowerType = Request.QueryString["strPowerType"].ToString();

            //Call Service
            List<ARSystemService.vwRTIData> listRTIData = new List<ARSystemService.vwRTIData>();
            using (var client = new ARSystemService.RTIServiceClient())
            {
                int intTotalRecord = client.GetCount(UserManager.User.UserToken, CompanyID, CustomerID, strBAPSNumber.ToArray(), strPONumber.ToArray(), strSONumber.ToArray(), Year, Quartal, BapsType, PowerType);

                //int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwRTIData> listRTIDataHolder = new List<ARSystemService.vwRTIData>();

                //for (int i = 0; i <= intBatch; i++)
                //{
                    listRTIDataHolder = client.GetData(UserManager.User.UserToken, CompanyID, CustomerID, strBAPSNumber.ToArray(), strPONumber.ToArray(), strSONumber.ToArray(), Year, Quartal, BapsType, PowerType, "", 0, intTotalRecord).ToList();
                    listRTIData.AddRange(listRTIDataHolder);
                //}
            }

            //Convert to DataTable
            DataTable table = new DataTable();


            string[] ColumsShow = new string[] {
                "SONumber"
                , "SiteID"
                , "SiteName"
                , "CustomerSiteID"
                , "CustomerSiteName"
                , "RegionName"
                , "ProvinceName"
                , "ResidenceName"
                , "PONumber"
                , "BAPSNumber"
                , "MLANumber"
                , "StartBapsDate"
                , "EndBapsDate"
                //, "RFIDate"
                //, "BaufDate"
                , "Term"
                , "BapsType"
                , "CustomerInvoice"
                , "CustomerID"
                , "CompanyInvoice"
                , "Company"
                , "StipSiro"
                , "InvoiceTypeName"
                , "BaseLeaseCurrency"
                , "ServiceCurrency"
                , "StartInvoiceDate"
                , "EndInvoiceDate"
                , "BaseLeasePrice"
                , "ServicePrice"
                , "DeductionAmount"
                , "TotalPaymentRupiah"
            };
            var reader = FastMember.ObjectReader.Create(listRTIData.Select(i => new
            {
                i.SONumber
                ,
                i.SiteID
                ,
                i.SiteName
                ,
                i.CustomerSiteID
                ,
                i.CustomerSiteName
                ,
                i.RegionName
                ,
                i.ProvinceName
                ,
                i.ResidenceName
                ,
                i.PONumber
                ,
                i.BAPSNumber
                ,
                i.MLANumber
                ,
                i.StartBapsDate
                ,
                i.EndBapsDate
                //, i.RFIDate
                //, i.BaufDate
                ,
                i.Term
                ,
                i.BapsType
                ,
                i.CustomerInvoice
                ,
                i.CustomerID
                ,
                i.CompanyInvoice
                ,
                i.Company
                ,
                i.StipSiro
                ,
                i.InvoiceTypeName
                ,
                i.BaseLeaseCurrency
                ,
                i.ServiceCurrency
                ,
                i.StartInvoiceDate
                ,
                i.EndInvoiceDate
                ,
                i.BaseLeasePrice
                ,
                i.ServicePrice
                ,
                i.DeductionAmount
                ,
                i.TotalPaymentRupiah
            }), ColumsShow);
            table.Load(reader);


            //Export to Excel
            ExportToExcelHelper.Export("trxBAPSInput", table);
        }

        [Route("Input/ExportRTIDone")]
        public void ExportToExcelInputRTIDone()
        {
            string CompanyID = Request.QueryString["strCompanyId"].ToString();
            string CustomerID = Request.QueryString["strOperator"].ToString();
            string BAPSNumber = Request.QueryString["strBAPS"].ToString();
            string Year = Request.QueryString["strYear"].ToString();
            string Quartal = Request.QueryString["strQuartal"].ToString();
            string BapsType = Request.QueryString["strBapsType"].ToString();
            string PowerType = Request.QueryString["strPowerType"].ToString();

            var strBAPSNumber = new List<string>();

            if (!string.IsNullOrEmpty(BAPSNumber))
            {
                foreach (var item in BAPSNumber.Split(','))
                {
                    strBAPSNumber.Add(item);
                }
            }
            else
            {
                strBAPSNumber.Add("0");
            }

            string PONumber = Request.QueryString["strPO"].ToString();
            var strPONumber = new List<string>();

            if (!string.IsNullOrEmpty(PONumber))
            {
                foreach (var item in PONumber.Split(','))
                {
                    strPONumber.Add(item);
                }
            }
            else
            {
                strPONumber.Add("0");
            }

            string SONumber = Request.QueryString["SONumber"].ToString();
            var strSONumber = new List<string>();

            if (!string.IsNullOrEmpty(SONumber))
            {
                foreach (var item in SONumber.Split(','))
                {
                    strSONumber.Add(item);
                }
            }
            else
            {
                strSONumber.Add("0");
            }

            //Call Service
            List<ARSystemService.vwRTIDataDone> listRTIData = new List<ARSystemService.vwRTIDataDone>();
            using (var client = new ARSystemService.RTIServiceClient())
            {
                int intTotalRecord = client.GetCount(UserManager.User.UserToken, CompanyID, CustomerID, strBAPSNumber.ToArray(), strPONumber.ToArray(), strSONumber.ToArray(), Year, Quartal, BapsType, PowerType);

                //int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwRTIDataDone> listRTIDataHolder = new List<ARSystemService.vwRTIDataDone>();

                //for (int i = 0; i <= intBatch; i++)
                //{
                listRTIDataHolder = client.GetDataDone(UserManager.User.UserToken, CompanyID, CustomerID, strBAPSNumber.ToArray(), strPONumber.ToArray(), strSONumber.ToArray(), Year, Quartal, BapsType, PowerType, "", 0, intTotalRecord).ToList();
                listRTIData.AddRange(listRTIDataHolder);

                //}
            }

            //Convert to DataTable
            DataTable table = new DataTable();

            string[] ColumsShow = new string[] {
                "SONumber"
                , "SiteID"
                , "SiteName"
                , "CustomerSiteID"
                , "CustomerSiteName"
                , "RegionName"
                , "ProvinceName"
                , "ResidenceName"
                , "PONumber"
                , "BAPSNumber"
                , "MLANumber"
                , "StartBapsDate"
                , "EndBapsDate"
                , "Term"
                , "BapsType"
                , "CustomerInvoice"
                , "CustomerID"
                , "CompanyInvoice"
                , "Company"
                , "StipSiro"
                , "InvoiceTypeName"
                , "BaseLeaseCurrency"
                , "ServiceCurrency"
                , "StartInvoiceDate"
                , "EndInvoiceDate"
                , "BaseLeasePrice"
                , "ServicePrice"
                , "DeductionAmount"
                , "TotalPaymentRupiah"
            };
            var reader = FastMember.ObjectReader.Create(listRTIData.Select(i => new
            {
                i.SONumber
                ,
                i.SiteID
                ,
                i.SiteName
                ,
                i.CustomerSiteID
                ,
                i.CustomerSiteName
                ,
                i.RegionName
                ,
                i.ProvinceName
                ,
                i.ResidenceName
                ,
                i.PONumber
                ,
                i.BAPSNumber
                ,
                i.MLANumber
                ,
                i.StartBapsDate
                ,
                i.EndBapsDate
                ,
                i.Term
                ,
                i.BapsType
                ,
                i.CustomerInvoice
                ,
                i.CustomerID
                ,
                i.CompanyInvoice
                ,
                i.Company
                ,
                i.StipSiro
                ,
                i.InvoiceTypeName
                ,
                i.BaseLeaseCurrency
                ,
                i.ServiceCurrency
                ,
                i.StartInvoiceDate
                ,
                i.EndInvoiceDate
                ,
                i.BaseLeasePrice
                ,
                i.ServicePrice
                ,
                i.DeductionAmount
                ,
                i.TotalPaymentRupiah
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("trxBAPSInput", table);
        }

        [Route("trxBAPSInput/Export")]
        public void GettrxBAPSInputToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string PONumber = Request.QueryString["strPOnumber"];
            var strPONumber = new List<string>();


            if (!string.IsNullOrEmpty(PONumber))
            {
                foreach (var item in PONumber.Split(','))
                {
                    strPONumber.Add(item);
                }
            }
            else
            {
                strPONumber.Add("0");
            }

            //Call Service
            List<ARSystemService.vwBAPSBulky> listBAPSData = new List<ARSystemService.vwBAPSBulky>();
            using (var client = new ARSystemService.BAPSBulkyServiceClient())
            {
                int intTotalRecord = client.GetBAPSBulkyCount(UserManager.User.UserToken, strCompanyId, strOperator, strPONumber.ToArray());
                //int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwBAPSBulky> listBAPSDataHolder = new List<ARSystemService.vwBAPSBulky>();

                //for (int i = 0; i <= intBatch; i++)
                //{
                    listBAPSDataHolder = client.GetBAPSBulkyToList(UserManager.User.UserToken, strCompanyId, strOperator, strPONumber.ToArray(), "", 0, intTotalRecord).ToList();
                    listBAPSData.AddRange(listBAPSDataHolder);
                //}
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            

            string[] ColumsShow = new string[]
            {
                "SONumber"
                ,"SiteID"
                ,"Term"
                ,"BAPSStartDate"
                ,"BAPSEndDate"
                ,"SiteName"
                ,"CustomerID"
                ,"Product"
                ,"CompanyName"
                ,"RegionName"
                ,"BAPSType"
                ,"BaseLeasePrice"
                ,"ServicePrice"
                ,"AdditionalAmount"
                ,"DeductionAmount"
                ,"PenaltySlaAmount"
                ,"AmountIDR"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
            {
                i.SONumber
                ,i.SiteID
                ,i.Term
                ,i.BAPSStartDate
                ,i.BAPSEndDate
                ,i.SiteName
                ,i.CustomerID
                ,i.Product
                ,i.CompanyName
                ,i.RegionName
                ,i.BAPSType
                ,i.BaseLeasePrice
                ,i.ServicePrice
                ,i.AdditionalAmount
                ,i.DeductionAmount
                ,i.PenaltySlaAmount
                ,i.AmountIDR

            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("trxBAPSInput", table);
        }

        [Route("trxBAPSDone/Export")]
        public void GettrxBAPSDoneToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string BAPSNumber = Request.QueryString["strBAPSNumber"];
            var strBAPSNumber = new List<string>();


            if (!string.IsNullOrEmpty(BAPSNumber))
            {
                foreach (var item in BAPSNumber.Split(','))
                {
                    strBAPSNumber.Add(item);
                }
            }
            else
            {
                strBAPSNumber.Add("0");
            }

            //Call Service
            List<ARSystemService.vwBAPSBulkyDoneHeader> listBAPSData = new List<ARSystemService.vwBAPSBulkyDoneHeader>();
            using (var client = new ARSystemService.BAPSBulkyServiceClient())
            {
                int intTotalRecord = client.GetBAPSBulkyDoneCount(UserManager.User.UserToken, strCompanyId, strBAPSNumber.ToArray());
                //int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwBAPSBulkyDoneHeader> listBAPSDataHolder = new List<ARSystemService.vwBAPSBulkyDoneHeader>();

                //for (int i = 0; i <= intBatch; i++)
                //{
                    listBAPSDataHolder = client.GetBAPSBulkyDoneToList(UserManager.User.UserToken, strCompanyId, strBAPSNumber.ToArray(), "", 0, intTotalRecord).ToList();
                    listBAPSData.AddRange(listBAPSDataHolder);
                //}
            }

            //Convert to DataTable
            DataTable table = new DataTable();

            string[] ColumsShow = new string[]
            {
                "BAPSNumber"
                ,"TotalTenant"
                ,"TotalAmount"
                ,"Remarks"
                ,"BAPSSignDate"
                ,"CreatedDate"
                ,"CreatedBy"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
            {
                i.BAPSNumber
               ,
                i.TotalTenant
               ,
                i.TotalAmount
               ,
                i.Remarks
               ,
                i.BAPSSignDate
               ,
                i.CreatedDate
               ,
                i.CreatedBy

            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("trxBAPSDone", table);
        }

        [Route("trxBAPSDoneDetail/Export")]
        public void GettrxBAPSDoneDetailToExport()
        {
            //Parameter
            string strBAPSId = Request.QueryString["strID"];

            //Call Service
            List<ARSystemService.vwBAPSBulkyDoneDetail> listBAPSData = new List<ARSystemService.vwBAPSBulkyDoneDetail>();
            using (var client = new ARSystemService.BAPSBulkyServiceClient())
            {
                int intTotalRecord = client.GetBAPSBulkyDoneDetailCount(UserManager.User.UserToken, strBAPSId);
                //int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwBAPSBulkyDoneDetail> listBAPSDataHolder = new List<ARSystemService.vwBAPSBulkyDoneDetail>();

                //for (int i = 0; i <= intBatch; i++)
                //{
                    listBAPSDataHolder = client.GetBAPSBulkyDoneDetailToList(UserManager.User.UserToken, strBAPSId, "", 0, intTotalRecord).ToList();
                    listBAPSData.AddRange(listBAPSDataHolder);
                //}
            }

            //Convert to DataTable
            DataTable table = new DataTable();

            string[] ColumsShow = new string[]
            {
                "SONumber"
                ,"SiteID"
                ,"SiteName"
                ,"Term"
                ,"bapsStartDate"
                ,"bapsEndDate"
                ,"CustomerID"
                ,"Product"
                ,"companyName"
                ,"RegionName"
                ,"BapsType"
                ,"BaseLeasePrice"
                ,"ServicePrice"
                ,"AdditionalAmount"
                ,"DeductionAmount"
                ,"PenaltySlaAmount"
                ,"AmountIDR"

            };
            var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
            {
                i.SONumber
               ,
                i.SiteID
               ,
                i.SiteName
               ,
                i.Term
               ,
                i.bapsStartDate
               ,
                i.bapsEndDate
               ,
                i.CustomerID
               ,
                i.Product
               ,
                i.companyName
               ,
                i.RegionName
               ,
                i.BapsType
               ,
                i.BaseLeasePrice
               ,
                i.ServicePrice
               ,
                i.AdditionalAmount
               ,
                i.DeductionAmount
               ,
                i.PenaltySlaAmount
               ,
                i.AmountIDR

            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("trxBAPSDoneDetail", table);
        }

        [Route("ReceiveDocumentBAUK/Export")]
        public void GetReceiveDocumentBAUKExport(PostReceiveDocumentBAUK post)
        {
            //Parameter
            var param = new vwRAReceiveDocumentBAUK();
            param.SONumber = post.vSONumber;
            param.SiteID = post.vSiteID;
            param.SiteName = post.vSiteName;
            param.StartSubmit = post.vStartSubmit;
            param.EndSubmit = post.vEndSubmit;
            param.CustomerID = post.vCustomerID;
            param.CompanyID = post.vCompanyID;
            param.StatusDoc = post.vStatusDoc;
            param.ProductID = post.vProductID;
            param.STIPID = post.vStip;

            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            var dataList = new List<vwRAReceiveDocumentBAUKHistory>();
            var result = _revenueAssuranceService.GetReceiveDocumentBAUKHistory(param, userCredential.UserID);
            dataList.AddRange(result);

            string[] fieldList = new string[]
            {
                "DeptSubmitBAUK"
                ,"BaukSubmitBySystem"
                ,"SONumber"
                ,"SiteID"
                ,"SiteName"
                ,"Product"
                ,"CustomerName"
                ,"CompanyID"
                ,"Company"
                ,"STIPCode"
                ,"StatusDoc"
            };

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataList, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("HistoryReceiveDocumentBAUK", table);
        }

        [Route("HistoryReject/Export")]
        public void ExportHistoryReject(vmHistoryRejectInvoice param)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            if (param.vSONumber == null)
            {
                param.vSONumber = new List<string>();
            }
            
            var result = _historyRejectInvoiceService.GetDataHistoryRejectInvoice(userCredential.UserID, param, param.vSONumber.ToList());

            string[] fields = new string[] { "SONumber", "SiteIdOld", "SiteName", "ReconcileType", "PowerType", "DepartmentName", "RejectYear", "RejectMonth", "CustomerSiteID",
                                            "CustomerSiteName", "CustomerInvoice", "CompanyInvoice", "InvNo", "StartDateInvoice", "EndDateInvoice", "AmountRental", "AmountService",
                                            "BaseLeaseCurrency", "ServiceCurrency", "AmountIDR", "AmountUSD", "Product", "BapsType", "StipSiro", "PicaType", "PicaMajor", "PicaDetail"};

            //Convert to DataTable
            var table = new DataTable();
            var reader = FastMember.ObjectReader.Create(result.List, fields);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("Rejection Invoice", table);
        }

        [Route("SummaryRejection/Summary/Export")]
        public ActionResult ExportSummary(vmSummaryRejectionPost param)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            var result = _summaryRejectionService.GetSummaryRejection(userCredential.UserID, param);

            byte[] downloadBytes;
            using (var package = SummaryRejectionHeader(result, param.vGroupBy))
            {
                downloadBytes = package.GetAsByteArray();
            }
            string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(downloadBytes, XlsxContentType, "TBG - List Dashboard Input Target.xlsx");

        }

        [NonAction]
        private ExcelPackage SummaryRejectionHeader(List<vmSummaryRejection> data, string vGroupBy)
        {
            List<string> detailHeader;

            if (vGroupBy == "1")
            {
                detailHeader = new List<string>()
                {
                     "# Of Sonumb Reject" ,
                     "Amount Of Sonumb Reject" ,
                     "# Of Sonumb Repetitive Reject"
                };
            } else if (vGroupBy == "2")
            {
                detailHeader = new List<string>()
                {
                     "# Of Baps Reject" ,
                     "Amount Of Baps Reject" ,
                     "# Of Baps Repetitive Reject"
                };
            } else
            {
                detailHeader = new List<string>()
                {
                     "# Of Invoice Reject" ,
                     "Amount Of Invoice Reject" ,
                     "# Of Invoice Repetitive Reject"
                };
            }

            List<string> columns = new List<string>()
            {
                 "Department" ,
                 "Reject By Finance" ,
                 "Reject By Operator"
            };

            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Summary Rejection";
            package.Workbook.Properties.Author = "Tower Bersama Group";
            package.Workbook.Properties.Subject = "SummaryRejection";
            package.Workbook.Properties.Keywords = "TBG - Summary Rejection";

            var worksheet = package.Workbook.Worksheets.Add("SummaryRejection");
            int col = 1; int row = 1;
            worksheet.Cells[1, 1, 2, 1].Merge = true;
            worksheet.Cells[1, 1, 2, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            for (int i = 0; i < columns.Count(); i++)
            {
                var colS = col;
                worksheet.Cells[row, col].Value = columns[i];
                worksheet.Cells[row, col].Style.Font.Bold = true;
                worksheet.Cells[row, col].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                if (columns[i] != "Department")
                {
                    for (int j = 0; j < detailHeader.Count(); j++)
                    {
                        worksheet.Cells[row + 1, col].Value = detailHeader[j];
                        worksheet.Column(col).Width = 11;
                        worksheet.Cells[row + 1, col].Style.Font.Bold = true;
                        worksheet.Cells[row + 1, col].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[row + 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[row + 1, col].Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                        col = col + 1;

                    }
                    worksheet.Cells[row, colS, row, col - 1].Merge = true;
                }
                else
                {
                    worksheet.Column(col).Width = 11;
                    worksheet.Cells[row + 1, col].Style.Font.Bold = true;
                    worksheet.Cells[row + 1, col].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[row + 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row + 1, col].Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                    col = col + 1;

                }
            }

            row = row + 2;
            //worksheet.Cells["B3:AW8"].Style.Numberformat.Format = "#,##0";
            worksheet.Column(1).Width = 28;

            foreach (var target in data)
            {
                worksheet.Cells[row, 1].Value = target.DepartmentName;
                worksheet.Cells[row, 1].Style.WrapText = true;
                worksheet.Cells[row, 2].Value = target.CountRejectFin;
                worksheet.Cells[row, 3].Value = target.AmountFin;
                worksheet.Cells[row, 4].Value = target.RepatitiveFin;
                worksheet.Cells[row, 5].Value = target.CountRejectOpr;
                worksheet.Cells[row, 6].Value = target.AmountOpr;
                worksheet.Cells[row, 7].Value = target.RepatitiveOpr;
                row = row + 1;
            }
            worksheet.Cells[row, 1].Value = "Total";
            worksheet.Cells[row, 2].Value = data.Sum(m => m.CountRejectFin);
            worksheet.Cells[row, 3].Value = data.Sum(m => m.AmountFin);
            worksheet.Cells[row, 4].Value = data.Sum(m => m.RepatitiveFin);
            worksheet.Cells[row, 5].Value = data.Sum(m => m.CountRejectOpr);
            worksheet.Cells[row, 6].Value = data.Sum(m => m.AmountOpr);
            worksheet.Cells[row, 7].Value = data.Sum(m => m.RepatitiveOpr);
            worksheet.Cells["A14:G14"].Style.Font.Bold = true;

            return package;
        }


        [Route("SummaryRejection/Detail/Export")]
        public void ExportDetail(vmSummaryRejectionPost param)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            List<vmSummaryRejection> data = new List<vmSummaryRejection>();

            string vName;
            if (param.vGroupBy == "1")
                vName = "DetailRejectBySonumb";
            else if (param.vGroupBy == "2")
                vName = "DetailRejectByBaps";
            else
                vName = "DetailRejectByInv";

            data = _summaryRejectionService.GetSummaryRejectionDetail(userCredential.UserID, param);
            string[] fields = new string[] { "SONumber", "SiteID", "SiteName", "CustomerSiteID", "CustomerSiteName", "CustomerInvoice", "CompanyInvoice", "RegionName", "BapsNo",
                                            "BapsType", "InvoiceNumber", "StartDateInvoice", "EndDateInvoice", "AmountInvoice", "AmountIDR", "AmountUSD", "DepartmentName", "RejectYear",
                                            "RejectMonth", "PicaType", "PicaMajor", "PicaDetail", "Remarks" };

            //Convert to DataTable
            var table = new DataTable();
            var reader = FastMember.ObjectReader.Create(data, fields);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export(vName, table);
        }

        #endregion

        #region Download
        [Route("Download")]
        public ActionResult DownloadFile()
        {
            string Filepath = Request.QueryString["FilePath"].ToString().Trim();
            string PONumber = Request.QueryString["PONumber"].ToString().Trim();
            string ContentType = Request.QueryString["ContentType"].ToString().Trim();
            string FileName = Request.QueryString["FileName"].ToString().Trim();
            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath(Helper.Helper.GetDocPath() + Filepath));
            //byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath(Filepath));

            return File(bytes, ContentType, FileName);
        }

        [Route("DownloadFile")]
        public ActionResult DownloadFiles()
        {
            string Filepath = Request.QueryString["FilePath"].ToString().Trim();
            string ContentType = Request.QueryString["ContentType"].ToString().Trim();
            string FileName = Request.QueryString["FileName"].ToString().Trim();
            byte[] bytes = System.IO.File.ReadAllBytes(Filepath + FileName);
            //byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath(Filepath));

            return File(bytes, ContentType, FileName);
        }
		
		[Route("DownloadFileProject")]
        public ActionResult DownloadFileProject()
        {
            string Filepath = Request.QueryString["FilePath"].ToString().Trim(); 
            string ContentType = Request.QueryString["ContentType"].ToString().Trim();
            string FileName = Request.QueryString["FileName"].ToString().Trim();
            string IsLegacy = Request.QueryString["IsLegacy"].ToString().Trim();
            var path = "";
            byte[] bytes;
            if (IsLegacy != "false")
            {
                path = Filepath;
                bytes = System.IO.File.ReadAllBytes(path);
            }
            else
            {
                //path = System.Web.HttpContext.Current.Server.MapPath(Helper.Helper.TbigSysDoc() + Filepath);
                bytes = System.IO.File.ReadAllBytes(Server.MapPath(Helper.Helper.TbigSysDoc() + Filepath));
            }
            
            //byte[] bytes = System.IO.File.ReadAllBytes(path);//(Filepath + FileName);
            //byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath(Filepath));

            return File(bytes, ContentType, FileName);
        }

        [Route("SummaryDocumentCheck/Export")]
        public async Task GetSummaryDocumentCheck()
        {
            //Parameter

            string strCustomerID = Request.QueryString["strCustomerID"].ToString();
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strProductId = Request.QueryString["strProductId"].ToString()=="" ? "0": Request.QueryString["strProductId"].ToString();
            string strSoNumber = Request.QueryString["strSoNumber"].ToString();
            string strSiteID = Request.QueryString["strSiteID"].ToString();
            string mstRAActivityID = Request.QueryString["mstRAActivityID"].ToString();

            var dataList = new List<DocumentCheckingSummary>();
            var service = new DocumentCheckingService();
            dataList = await service.GetSummaryDocumentCheck(strCustomerID, strSiteID, strSoNumber, strCompanyId, int.Parse(strProductId));
            

            string[] fieldList = new string[]
            {
                "NoSerahTerima"
                ,"NewORRevise"
                ,"SONumber"
                ,"STIP"
                ,"ProductName"
                ,"SiteID"
                ,"SiteName"
                ,"TenantType"
                ,"Operator"
                ,"DeptSubmiterBAUKSystem"
                ,"PICDeptSubmiterBAUKSystem"
                ,"PICNEBDeptReviewerHardCopyMandatoryBAUK"
                ,"DateReview"
                ,"StatusApproval"
                ,"RemarkSummary"
            };

           

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataList.Select(i=> new {
                NoSerahTerima = i.Number,
                NewORRevise = i.StatusBAUK,
                SONumber = i.SoNumber,
                STIP = i.StipCode,
                ProductName = i.Product,
                SiteID = i.SiteID,
                SiteName = i.SiteName,
                TenantType = i.TenantType,
                Operator = i.Customer,
                DeptSubmiterBAUKSystem = i.PICSubmitBAUK,
                PICDeptSubmiterBAUKSystem = i.DeptSubmitBAUK,
                PICNEBDeptReviewerHardCopyMandatoryBAUK = i.PICReveiewHardCopyBAUK,
                DateReview = i.DateReview,
                StatusApproval = i.ApprStatus,
                RemarkSummary = i.Remarks
            }), fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("SummaryVerifikasiBAUK", table);
        }

        [Route("SummaryDocumentCheckPerDoc/Export")]
        public async Task GetSummaryDocumentCheckPerDoc()
        {
            //Parameter

            string strCustomerID = Request.QueryString["strCustomerID"].ToString();
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strProductId = Request.QueryString["strProductId"].ToString() == "" ? "0" : Request.QueryString["strProductId"].ToString();
            string strSoNumber = Request.QueryString["strSoNumber"].ToString();
            string strSiteID = Request.QueryString["strSiteID"].ToString();
            

            var dataList = new List<DocumentCheckingSummaryPerDoc>();
            var service = new DocumentCheckingService();
            dataList = await service.GetSummaryDocumentCheckPerDoc(strCustomerID, strSiteID, strSoNumber, strCompanyId, int.Parse(strProductId));


            string[] fieldList = new string[]
            {
                "NoSerahTerima"
                ,"NewORRevise"
                ,"SONumber"
                ,"STIP"
                ,"ProductName"
                ,"SiteID"
                ,"SiteName"
                ,"TenantType"
                ,"Operator"
                ,"DeptSubmiterBAUKSystem"
                ,"PICDeptSubmiterBAUKSystem"
                ,"PICNEBDeptReviewerHardCopyMandatoryBAUK"
                ,"DateReview"
                ,"Mandatory"
                ,"DocItem"
                ,"StatusPerItem"
                ,"RemarkPerItem"
            };



            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataList.Select(i => new {
                NoSerahTerima = i.Number,
                NewORRevise = i.StatusBAUK,
                SONumber = i.SoNumber,
                STIP = i.StipCode,
                ProductName = i.Product,
                SiteID = i.SiteID,
                SiteName = i.SiteName,
                TenantType = i.TenantType,
                Operator = i.Customer,
                DeptSubmiterBAUKSystem = i.PICSubmitBAUK,
                PICDeptSubmiterBAUKSystem = i.DeptSubmitBAUK,
                PICNEBDeptReviewerHardCopyMandatoryBAUK = i.PICReveiewHardCopyBAUK,
                DateReview = i.DateReview,
                Mandatory  = i.Mandatory,
                DocItem = i.DocName,
                StatusPerItem = i.StatusPerItem,
                RemarkPerItem = i.RemarkPerITem
            }), fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("SummaryVerifikasiBAUKPerItem", table);
        }

        [Route("DownloadDocumentSuppoert")]
        public ActionResult DownloadDocumentSuppoert()
        {
            string FileName = Request.QueryString["fileName"].ToString().Trim();
            string CompanyId = Request.QueryString["companyId"].ToString().Trim();
            string SiteId = Request.QueryString["siteId"].ToString().Trim();
            var path = Server.MapPath(ConfigurationManager.AppSettings["DocSupportSite"].ToString() + @"\" + CompanyId+ @"\" + SiteId + @"\" + FileName);
            byte[] bytes;
            string contentType = Path.GetExtension(path);
            try
            {
                bytes = System.IO.File.ReadAllBytes(path);

            }
            catch (Exception)
            {
                contentType = ".pdf";
                bytes = null;
            }

            return File(bytes, contentType, FileName);
        }
        #endregion

        #region Template
        private string GenerateQRCode(string textQr, string fileName)
        {
            string pathDOc;
            string fileDoc;
            //=====================================================================//
            //pathDOc = Server.MapPath("~\\Content\\QRCode");
            //pathDOc = Helper.Helper.GetDocPath() + "\\RABAPSPrint\\QRCode";
            pathDOc = Server.MapPath(Helper.Helper.GetDocPath() + "RABAPSPrint\\QRCode");
            ThoughtWorks.QRCode.Codec.QRCodeEncoder qrCode = new ThoughtWorks.QRCode.Codec.QRCodeEncoder();
            System.Drawing.Image bitMap;
            //=====================================================================//
            qrCode.QRCodeEncodeMode = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCode.QRCodeScale = 7;
            qrCode.QRCodeVersion = 0;
            qrCode.QRCodeErrorCorrect = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ERROR_CORRECTION.Q;
            bitMap = qrCode.Encode(textQr);
            // =====================================================================//
            fileDoc = pathDOc + "\\" + fileName;
            if (Directory.Exists(pathDOc) == false)
            {
                Directory.CreateDirectory(pathDOc);
            }
            //  ==== save file ====== //

            if (System.IO.File.Exists(fileDoc) == false)
            {

                bitMap.Save(fileDoc);
            }
            //if (System.IO.File.Exists(fileDoc) == true)
            //{

            //    System.IO.File.Delete(fileDoc);
            //    bitMap.Save(fileDoc);
            //}
            //else
            //{
            //    bitMap.Save(fileDoc);
            //}
            bitMap.Dispose();

            return fileDoc;
        }

        [Authorize]
        [Route("ViewPrint")]
        public ActionResult ViewPrint(int ID)
        {
            ARSystem.Domain.Models.vwReportTemplate resultHtml = new ARSystem.Domain.Models.vwReportTemplate();

            resultHtml = _ReportTemplateService.GetReport().Where(w => w.ID == ID).FirstOrDefault();
            resultHtml.HtmlString = resultHtml.HtmlString == null ? "Template Not Found" : resultHtml.HtmlString;
            resultHtml.TextQrCode = resultHtml.TextQrCode == null ? "baps" : resultHtml.TextQrCode;

            string headerHtml = "";
            string footerHtml = "";
            string fileName = "";
            string htmlElements = "";

            headerHtml += "<!DOCTYPE html><html><body>";
            headerHtml += "<table style='width:100%'>";
            headerHtml += "<tr>";
            headerHtml += "<td> <img src='" + Server.MapPath(resultHtml.LogoPathLeft) + "' width='" + resultHtml.LogoLeftWidth + "px' height='" + resultHtml.LogoLeftHeight + "px' />";
            headerHtml += "</td>";
            headerHtml += "<td> <img src='" + Server.MapPath(resultHtml.LogoPathRight) + "'  width='" + resultHtml.LogoRightWidth + "px' height='" + resultHtml.LogoRightHeight + "px' style='float:right' />";
            headerHtml += "</td>";
            headerHtml += "</tr>";
            headerHtml += "</table>";
            headerHtml += "</body></html>";
            // ====== set footer page ================================//
            // === javascript for add page number == //
            string jsScript = "<script>function subst() { var vars = { };var x = document.location.search.substring(1).split('&');";
            jsScript += "for(var i in x) { var z = x[i].split('=', 2); vars[z[0]] = unescape(z[1]); }";
            jsScript += "var x =['frompage', 'topage', 'page', 'webpage', 'section', 'subsection', 'subsubsection'];";
            jsScript += "for(var i in x){";
            jsScript += "var y = document.getElementsByClassName(x[i]);";
            jsScript += "for (var j = 0; j < y.length; ++j) y[j].textContent = vars[x[i]];}}</script > ";
            //==================================================================================================//
            footerHtml += "<!DOCTYPE html><html><head>";
            footerHtml += jsScript + "</head><body onload='subst()'>";
            footerHtml += "<table style='width:100%'>";
            footerHtml += "<tr>";
            footerHtml += "<td valign='top' style='padding-right:10px;'>";
            footerHtml += resultHtml.FooterText;
            footerHtml += "</tr><tr>";
            footerHtml += "</td>";
            footerHtml += "<td style='width:" + resultHtml.QRCodeWidth + "px'> <img src='" + GenerateQRCode(resultHtml.TextQrCode, "baps_" + fileName + ".jpg") + "' width='" + resultHtml.QRCodeWidth + "px' height='" + resultHtml.QRCodeHeight + "px' style='float:right'/>";
            footerHtml += "</td>";
            footerHtml += "</tr>";
            footerHtml += "</table>";
            footerHtml += "</body></html>";
            //================================================================================================//
            htmlElements += resultHtml.HtmlString;

            BapsPrintPDF BapsPrintPDF = new BapsPrintPDF();
            BapsPrintPDF.htmlElements = htmlElements;

            string customeSwitches = "";
            string headerPath = Server.MapPath("~/Views/RANewBaps/Header.html");
            System.IO.File.WriteAllText(headerPath, headerHtml);
            string footerPath = Server.MapPath("~/Views/RANewBaps/Footer.html");
            System.IO.File.WriteAllText(footerPath, footerHtml);
            customeSwitches = string.Format("--print-media-type --header-html  \"{0}\" --header-spacing \"3\" --footer-html \"{1}\"", headerPath, footerPath);

            return new Rotativa.ViewAsPdf("~/Views/RANewBaps/PrintDetails.cshtml", BapsPrintPDF)
            {
                FileName = System.DateTime.Now.ToString("dd_MMM_yyy_HH_MM_SS") + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(Convert.ToInt16(resultHtml.MarginTop), Convert.ToInt16(resultHtml.MarginRight), Convert.ToInt16(resultHtml.MarginBottom), Convert.ToInt16(resultHtml.MarginLeft)),
                CustomSwitches = customeSwitches
            };
        }
        #endregion
    }
}