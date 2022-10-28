using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ARSystemFrontEnd.Providers;
using System.Data;
using ARSystemFrontEnd.Helper;
using ARSystem.Service;
using ARSystem.Domain.Models;
using Newtonsoft.Json;

using ARSystem.Domain.Models.ViewModels;
using ARSystem.Service.ARSystem;
using System.Diagnostics;
using ARSystem.Domain.Models.ViewModels.Datatable;
using ARSystem.Service.ARSystem.Dashboard;
using ARSystemFrontEnd.Models;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using ARSystem.Domain.Models.HTBGDWH01.TBGARSystem;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("Dashboard")]
    public class DashboardController : BaseController
    {
        private readonly DashboardBAUKService dashBAUKService;
        private MonitoringAgingExecutiveService _agingExecutiveService;
        private InvoiceProductionService _invoiceProduction;
        private readonly MonitoringCNInvoiceService _monitoringCNService;

        public DashboardController()
        {
            dashBAUKService = new DashboardBAUKService();
            _agingExecutiveService = new MonitoringAgingExecutiveService();
            _invoiceProduction = new InvoiceProductionService();
            _monitoringCNService = new MonitoringCNInvoiceService();
        }

        [Authorize]
        [Route("")]
        public ActionResult Index()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            string role = PageAuth(userCredential.UserRoleID);
            if (!string.IsNullOrEmpty(role))
                return Redirect(role);

            return View();
        }

        private string PageAuth(int roleId)
        {
            MstRoleGroupService service = new MstRoleGroupService();
            string role = service.GetDashboardUrl(roleId);


            return role;
        }
        [Authorize]
        [Route("Auth")]
        public ActionResult Authentivicationd()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");


            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            if (userCredential.ErrorType > 0)
                return RedirectToAction("PasswordExpired", "Login");

            string role = PageAuth(userCredential.UserRoleID);
            if (string.IsNullOrEmpty(role))
                role = "/Dashboard";


            return Redirect(role);

        }

        [Authorize]
        [Route("LeadTimePIC")]
        public ActionResult LeadTimePIC()
        {
            string actionTokenView = "eb4d384c-bd84-4773-b4e2-8d8adaae073c";

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

        [Authorize]
        [Route("KPISummary")]
        public ActionResult KPISummary()
        {
            string actionTokenView = "4be2f004-9470-4643-9982-a1fe50387042";

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

        [Authorize]
        [Route("InvoiceTowerSummary")]
        public ActionResult InvoiceTowerSummary()
        {
            string actionTokenView = "28623d70-6113-4533-8695-b997858f69f7";

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

        [Authorize]
        [Route("InvoiceBuildingSummary")]
        public ActionResult InvoiceBuildingSummary()
        {
            string actionTokenView = "f3871150-f8d8-423b-9744-3df879972e1e";

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

        [Authorize]
        [Route("ARRatio")]
        public ActionResult ARRatio()
        {
            string actionTokenView = "c2ef5763-47bb-4322-858e-67b18c386335";

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

        [Authorize]
        [Route("ARMonitoringAging")]
        public ActionResult ARMonitoringAging()
        {
            string actionTokenView = "e886fbd7-3034-48cd-9bba-928e66e7b6dc";

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

        [Authorize]
        [Route("ReconcileHistory")]
        public ActionResult ReconcileHistory()
        {
            string actionTokenView = "36286c45-7544-4f11-8b79-60de39500180";

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

        [Authorize]
        [Route("BAUK")]
        public ActionResult BAUK()
        {
            string actionTokenView = "639DEC6F-AFC8-46B0-96D4-DB4B9BBE9DF9";

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

            return View("~/Views/DashboardBAUK/Index.cshtml");
        }

        [Authorize]
        [Route("InvoiceProduction")]
        public ActionResult InvoiceProduction()
        {
            string actionTokenView = "384b195f-ec69-4bb6-8e99-3b5e99c50f02";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        #region Export to Excel

        [Route("LeadTimePIC/Export")]
        public void GetLeadTimePICToExport()
        {
            //Parameter
            int year = int.Parse(Request.QueryString["year"]);
            int month = int.Parse(Request.QueryString["month"]);

            //Call Service
            List<ARSystemService.vmDashboardLeadTimePIC> list = new List<ARSystemService.vmDashboardLeadTimePIC>();
            using (var client = new ARSystemService.DashboardServiceClient())
            {
                List<ARSystemService.vmDashboardLeadTimePIC> listHolder = new List<ARSystemService.vmDashboardLeadTimePIC>();
                if (month > 0)
                    listHolder = client.GetLeadTimePICPerWeek(UserManager.User.UserToken, year, month).ToList();
                else
                    listHolder = client.GetLeadTimePICPerMonth(UserManager.User.UserToken, year).ToList();
                list.AddRange(listHolder);
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "Param3",
                "VerificatorCurr",
                "VerificatorOD",
                "VerificatorAVG",
                "InputerCurr",
                "InputerOD",
                "InputerAVG",
                "FinishingCurr",
                "FinishingOD",
                "FinishingAVG",
                "ARDataCurr",
                "ARDataOD",
                "ARDataAVG"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("Lead Time PIC Dashboard", table);
        }

        [Route("KPISummary/Export")]
        public void GetKPISummaryToExport()
        {
            //Parameter
            int year = int.Parse(Request.QueryString["year"]);
            int month = int.Parse(Request.QueryString["month"]);

            //Call Service
            using (var client = new ARSystemService.DashboardServiceClient())
            {
                if (month > 0)
                {
                    List<ARSystemService.vmDashboardKPISummaryPerWeek> list = new List<ARSystemService.vmDashboardKPISummaryPerWeek>();
                    List<ARSystemService.vmDashboardKPISummaryPerWeek> listHolder = client.GetKPISummaryPerWeek(UserManager.User.UserToken, year, month).ToList();
                    list.AddRange(listHolder);

                    string[] fieldList = new string[] {
                        "No",
                        "KPIDesc",
                        "TargetKPI",
                        "LeadTimeWeek1",
                        "LeadTimeWeek2",
                        "LeadTimeWeek3",
                        "LeadTimeWeek4",
                        "LeadTimeWeek5",
                        "LeadTimeWeek6",
                        "BobotKPI"
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(list, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("KPI Summary Per Week Dashboard", table);
                }
                else
                {
                    List<ARSystemService.vmDashboardKPISummaryPerMonth> list = new List<ARSystemService.vmDashboardKPISummaryPerMonth>();
                    List<ARSystemService.vmDashboardKPISummaryPerMonth> listHolder = client.GetKPISummaryPerMonth(UserManager.User.UserToken, year).ToList();
                    list.AddRange(listHolder);

                    //Convert to DataTable
                    string[] fieldList = new string[] {
                        "No",
                        "KPIDesc",
                        "TargetKPI",
                        "LeadTimeJan",
                        "LeadTimeFeb",
                        "LeadTimeMar",
                        "LeadTimeApr",
                        "LeadTimeMay",
                        "LeadTimeJun",
                        "LeadTimeJul",
                        "LeadTimeAug",
                        "LeadTimeSep",
                        "LeadTimeOct",
                        "LeadTimeNov",
                        "LeadTimeDec",
                        "BobotKPI"
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(list, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("KPI Summary Per Month Dashboard", table);
                }
            }
        }

        [Route("InvoiceTowerSummary/Export")]
        public void GetInvoiceTowerSummaryToExport()
        {
            //Parameter
            int year = int.Parse(Request.QueryString["year"]);
            int month = int.Parse(Request.QueryString["month"]);
            string currency = Request.QueryString["currency"];

            //Call Service
            using (var client = new ARSystemService.DashboardServiceClient())
            {
                if (month > 0)
                {
                    List<ARSystemService.vmDashboardInvoiceTowerSummaryPerWeek> list = new List<ARSystemService.vmDashboardInvoiceTowerSummaryPerWeek>();
                    List<ARSystemService.vmDashboardInvoiceTowerSummaryPerWeek> listHolder = client.GetInvoiceTowerSummaryPerWeek(UserManager.User.UserToken, year, month, currency).ToList();
                    list.AddRange(listHolder);

                    string[] fieldList = new string[] {
                        "Category",
                        "Week1Curr",
                        "Week1OD",
                        "Week2Curr",
                        "Week2OD",
                        "Week3Curr",
                        "Week3OD",
                        "Week4Curr",
                        "Week4OD",
                        "Week5Curr",
                        "Week5OD",
                        "Week6Curr",
                        "Week6OD"
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(list, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("DashboardInvoiceMonthly", table);
                }
                else
                {
                    List<ARSystemService.vmDashboardInvoiceTowerSummaryPerMonth> list = new List<ARSystemService.vmDashboardInvoiceTowerSummaryPerMonth>();
                    List<ARSystemService.vmDashboardInvoiceTowerSummaryPerMonth> listHolder = client.GetInvoiceTowerSummaryPerMonth(UserManager.User.UserToken, year, currency).ToList();
                    list.AddRange(listHolder);

                    //Convert to DataTable
                    string[] fieldList = new string[] {
                        "Category",
                        "JanCurr",
                        "JanOD",
                        "FebCurr",
                        "FebOD",
                        "MarCurr",
                        "MarOD",
                        "AprCurr",
                        "AprOD",
                        "MayCurr",
                        "MayOD",
                        "JunCurr",
                        "JunOD",
                        "JulCurr",
                        "JulOD",
                        "AugCurr",
                        "AugOD",
                        "SepCurr",
                        "SepOD",
                        "OctCurr",
                        "OctOD",
                        "NovCurr",
                        "NovOD",
                        "DecCurr",
                        "DecOD",
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(list, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("DashboardInvoiceWeekly", table);
                }
            }
        }

        [Route("InvoiceBuildingSummary/Export")]
        public void GetInvoiceBuildingSummaryToExport()
        {
            //Parameter
            int year = int.Parse(Request.QueryString["year"]);
            int month = int.Parse(Request.QueryString["month"]);

            //Call Service
            using (var client = new ARSystemService.DashboardServiceClient())
            {
                if (month > 0)
                {
                    List<ARSystemService.vmDashboardInvoiceBuildingSummaryPerWeek> list = new List<ARSystemService.vmDashboardInvoiceBuildingSummaryPerWeek>();
                    List<ARSystemService.vmDashboardInvoiceBuildingSummaryPerWeek> listHolder = client.GetInvoiceBuildingSummaryPerWeek(UserManager.User.UserToken, year, month).ToList();
                    list.AddRange(listHolder);

                    string[] fieldList = new string[] {
                        "Category",
                        "Week1Curr",
                        "Week1OD",
                        "Week2Curr",
                        "Week2OD",
                        "Week3Curr",
                        "Week3OD",
                        "Week4Curr",
                        "Week4OD",
                        "Week5Curr",
                        "Week5OD",
                        "Week6Curr",
                        "Week6OD"
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(list, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("InvoiceBuildingDashboard", table);
                }
                else
                {
                    List<ARSystemService.vmDashboardInvoiceBuildingSummaryPerMonth> list = new List<ARSystemService.vmDashboardInvoiceBuildingSummaryPerMonth>();
                    List<ARSystemService.vmDashboardInvoiceBuildingSummaryPerMonth> listHolder = client.GetInvoiceBuildingSummaryPerMonth(UserManager.User.UserToken, year).ToList();
                    list.AddRange(listHolder);

                    //Convert to DataTable
                    string[] fieldList = new string[] {
                        "Category",
                        "JanCurr",
                        "JanOD",
                        "FebCurr",
                        "FebOD",
                        "MarCurr",
                        "MarOD",
                        "AprCurr",
                        "AprOD",
                        "MayCurr",
                        "MayOD",
                        "JunCurr",
                        "JunOD",
                        "JulCurr",
                        "JulOD",
                        "AugCurr",
                        "AugOD",
                        "SepCurr",
                        "SepOD",
                        "OctCurr",
                        "OctOD",
                        "NovCurr",
                        "NovOD",
                        "DecCurr",
                        "DecOD",
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(list, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("InvoiceBuildingDashboard", table);
                }
            }
        }

        [Route("ARRatio/Export")]
        public void GetARRatioToExport()
        {
            //Parameter
            int year = int.Parse(Request.QueryString["year"]);
            int month = int.Parse(Request.QueryString["month"]);

            //Call Service
            using (var client = new ARSystemService.DashboardServiceClient())
            {
                if (month > 0)
                {
                    List<ARSystemService.vmDashboardARRatioPerWeek> list = new List<ARSystemService.vmDashboardARRatioPerWeek>();
                    List<ARSystemService.vmDashboardARRatioPerWeek> listHolder = client.GetARRatioPerWeek(UserManager.User.UserToken, year, month).ToList();
                    list.AddRange(listHolder);

                    string[] fieldList = new string[] {
                        "Category",
                        "AmountWeek1",
                        "AmountWeek2",
                        "AmountWeek3",
                        "AmountWeek4",
                        "AmountWeek5",
                        "AmountWeek6"
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(list, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("ARRatioDashboardMonth_" + month, table);
                }
                else
                {
                    List<ARSystemService.vmDashboardARRatioPerMonth> list = new List<ARSystemService.vmDashboardARRatioPerMonth>();
                    List<ARSystemService.vmDashboardARRatioPerMonth> listHolder = client.GetARRatioPerMonth(UserManager.User.UserToken, year).ToList();
                    list.AddRange(listHolder);

                    //Convert to DataTable
                    string[] fieldList = new string[] {
                        "Category",
                        "AmountJan",
                        "AmountFeb",
                        "AmountMar",
                        "AmountApr",
                        "AmountMay",
                        "AmountJun",
                        "AmountJul",
                        "AmountAug",
                        "AmountSep",
                        "AmountOct",
                        "AmountNov",
                        "AmountDec"
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(list, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("ARRatioDashboardYear_" + year, table);
                }
            }
        }

        [Route("ARMonitoringAging/Summary/Export")]
        public void GetARMonitoringAgingSummaryToExport(Models.PostDashboardARMonitoringAgingView post)
        {
            //Parameter
            string endDate = "";
            string companyId = post.CompanyId;
            if (post.EndDate.Contains("-"))
                endDate = post.EndDate.Substring(1);
            else
                endDate = post.EndDate;
            string operatorId = post.OperatorId;
            string invoiceType = post.InvoiceType;
            string amountType = post.AmountType;

            //Call Service
            using (var client = new ARSystemService.DashboardServiceClient())
            {
                List<ARSystemService.vmDashboardARMonitoringAgingSummary> list = new List<ARSystemService.vmDashboardARMonitoringAgingSummary>();
                List<ARSystemService.vmDashboardARMonitoringAgingSummary> listHolder = client.GetARMonitoringAgingSummary(UserManager.User.UserToken, companyId, endDate, operatorId, invoiceType, amountType, post.vPKP).ToList();
                list.AddRange(listHolder);

                string[] fieldList = new string[] {
                        "Operator",
                        "CountNoReceipt",
                        "AmountNoReceipt",
                        "CountCurrent",
                        "AmountCurrent",
                        "CountAging1_30",
                        "AmountAging1_30",
                        //"CountAging16_30",
                        //"AmountAging16_30",
                        "CountAging31_60",
                        "AmountAging31_60",
                        "CountAging61_90",
                        "AmountAging61_90",
                        "CountAgingOver90",
                        "AmountAgingOver90",
                        "CountTotal",
                        "AmountTotal",
                        "PercentageOverDue",
                        "InvoiceOverDue"
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(list, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("ARMonitoringAgingSummary", table);
            }
        }

        [Route("ARMonitoringAging/Detail/Export")]
        public void GetARMonitoringAgingDetailToExport(Models.PostDashboardARMonitoringAgingView post)
        {
            //Parameter
            string endDate = "";
            string companyId = post.CompanyId;
            if (post.EndDate.Contains("-"))
                endDate = post.EndDate.Substring(1);
            else
                endDate = post.EndDate;
            string operatorId = post.OperatorId;
            string invoiceType = post.InvoiceType;
            string amountType = post.AmountType;
            string status = post.Status;

            //Call Service
            using (var client = new ARSystemService.DashboardServiceClient())
            {
                List<ARSystemService.vmDashboardARMonitoringAgingDetail> list = new List<ARSystemService.vmDashboardARMonitoringAgingDetail>();
                List<ARSystemService.vmDashboardARMonitoringAgingDetail> listHolder = client.GetARMonitoringAgingDetail(UserManager.User.UserToken, companyId, endDate, operatorId, invoiceType, amountType, status, post.vPKP).ToList();
                list.AddRange(listHolder);

                string[] fieldList = new string[] {
                    "Company",
                    "Operator",
                    "InvoiceNo",
                    "InvoiceDate",
                    "ReceiptDate",
                    "DueDate",
                    "InvoiceType",
                    "OverDue",
                    "DPP",
                    "VAT",
                    "WAPU",
                    "SKB",
                    "Penalty",
                    "OutstandingInvoiceGross",
                    "PICInternal",
                    "PICExternal",
                    "Subject"
                };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(list, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("ARMonitoringAgingDetail", table);
            }
        }

        [Route("DetailLeadTime/Export")]
        public void GetInvoiceTowerDetailLeadTimeToExport()
        {
            //Parameter
            int year = int.Parse(Request.QueryString["year"]);
            int month = int.Parse(Request.QueryString["month"]);
            int week = int.Parse(Request.QueryString["week"]);
            string pic = Request.QueryString["pic"];
            string leadTime = Request.QueryString["leadTime"];

            //Call Service
            using (var client = new ARSystemService.DashboardServiceClient())
            {
                if (month > 0)
                {
                    List<ARSystemService.vmDashboardInvoiceTowerDetailLeadTime> list = new List<ARSystemService.vmDashboardInvoiceTowerDetailLeadTime>();
                    List<ARSystemService.vmDashboardInvoiceTowerDetailLeadTime> listHolder = client.GetDetailLeadTime(UserManager.User.UserToken, year, month, week, pic, leadTime).ToList();
                    list.AddRange(listHolder);

                    string[] fieldList = new string[] {
                        "Company",
                        "Operator",
                        "SoNumber",
                        "CollectionPeriod",
                        "InvoiceNumber",
                        "Activity",
                        "LogActivity",
                        "LogUser"
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(list, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("DetailLeadTime", table);
                }
            }
        }

        [Route("DetailSoNumber/Export")]
        public void GetInvoiceTowerDetailSoNumberToExport()
        {
            //Parameter
            int year = int.Parse(Request.QueryString["year"]);
            int month = int.Parse(Request.QueryString["month"]);
            int week = int.Parse(Request.QueryString["week"]);
            string leadTime = Request.QueryString["leadTime"];
            string currency = Request.QueryString["currency"];

            //Call Service
            using (var client = new ARSystemService.DashboardServiceClient())
            {
                if (month > 0)
                {
                    List<ARSystemService.vmDashboardInvoiceTowerDetailSoNumber> list = new List<ARSystemService.vmDashboardInvoiceTowerDetailSoNumber>();
                    List<ARSystemService.vmDashboardInvoiceTowerDetailSoNumber> listHolder = client.GetDetailSONumber(UserManager.User.UserToken, year, month, week, leadTime, currency).ToList();
                    list.AddRange(listHolder);

                    string[] fieldList = new string[] {
                        "SoNumber",
                        "Operator",
                        "Company",
                        "TenantType",
                        "Start",
                        "End",
                        "EGF",
                        "RevPerMonth",
                        "AmountInv"
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(list, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("DetailSONumber", table);
                }
            }
        }

        [Route("DetailBuilding/Export")]
        public void GetInvoiceBuildingDetail()
        {
            //Parameter
            int year = int.Parse(Request.QueryString["year"]);
            int month = int.Parse(Request.QueryString["month"]);
            int week = int.Parse(Request.QueryString["week"]);
            string leadTime = Request.QueryString["leadTime"];

            //Call Service
            using (var client = new ARSystemService.DashboardServiceClient())
            {
                if (month > 0)
                {
                    List<ARSystemService.vmDashboardInvoiceBuildingDetail> list = new List<ARSystemService.vmDashboardInvoiceBuildingDetail>();
                    List<ARSystemService.vmDashboardInvoiceBuildingDetail> listHolder = client.GetInvoiceBuildingDetail(UserManager.User.UserToken, year, month, week, leadTime).ToList();
                    list.AddRange(listHolder);

                    string[] fieldList = new string[] {
                        "InvNo",
                        "Company",
                        "CompanyType",
                        "TermPeriod",
                        "Area",
                        "PricePerMeter",
                        "PricePerMonth",
                        "TotalPrice",
                        "PPN",
                        "Discount",
                        "Penalty",
                        "TotalAmount"
                    };

                    DataTable table = new DataTable();
                    var reader = FastMember.ObjectReader.Create(list, fieldList);
                    table.Load(reader);

                    //Export to Excel
                    ExportToExcelHelper.Export("DetailInvoiceBuilding", table);
                }
            }
        }

        // Modification Or Added By Ibnu Setiawan 21. January 2019

        [Route("ReconcileHistory/Export")]
        public void GetReconcileHistory()
        {
            //Parameter
            Models.PostTrxBapsDataView post = new Models.PostTrxBapsDataView();
            post.strCompanyId = Request.QueryString["strCompanyId"];
            post.strOperator = Request.QueryString["strOperator"];
            post.strBAPSNumber = Request.QueryString["strBAPSNumber"];
            post.strBapsType = Request.QueryString["strBapsType"];
            post.strCreatedBy = Request.QueryString["strCreatedBy"];
            post.strInvoiceType = Request.QueryString["strInvoiceType"];
            post.strPONumber = Request.QueryString["strPONumber"];
            post.strSONumber = Request.QueryString["strSONumber"];
            post.strSiteIdOld = Request.QueryString["strSiteIdOld"];
            post.strStartPeriod = Request.QueryString["strStartPeriod"];
            post.strEndPeriod = Request.QueryString["strEndPeriod"];
            string strWhereClause = GetWhereClause(post);

            //Call Service
            using (var client = new ARSystemService.DashboardServiceClient())
            {
                List<ARSystemService.vwReconcileHistory> list = new List<ARSystemService.vwReconcileHistory>();
                List<ARSystemService.vwReconcileHistory> data = client.ReconcileHistory(UserManager.User.UserToken, strWhereClause, "").ToList();
                list.AddRange(data);

                string[] fieldList = new string[] {
                        "SoNumber",
                        "SiteID",
                        "SiteName",
                        "CompanyInvoice",
                        "CustomerInvoice",
                        "Term",
                        "StartInvoiceDate",
                        "EndInvoiceDate",
                        "BaseLeasePrice",
                        "ServicePrice",
                        "InflationAmount",
                        "AdditionalAmount",
                        "DeductionAmount",
                        "PenaltySlaAmount",
                        "BOQNumber",
                        "PONumber",
                        "BAPSNumber",
                        "ActivityName"
                    };

                DataTable table = new DataTable();
                var reader = FastMember.ObjectReader.Create(list, fieldList);
                table.Load(reader);

                //Export to Excel
                ExportToExcelHelper.Export("ReconcileHistory" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString(), table);
            }
        }

        [Route("BAUK/Activity/Export")]
        public void ExportBAUKActivity()
        {
            var filter = new vmDashboardBAUKFilter();

            filter.Month = Convert.ToInt32(Request.QueryString["intMonth"]);
            filter.Year = Convert.ToInt32(Request.QueryString["intYear"]);
            filter.GroupBy = Request.QueryString["strGroupBy"];
            filter.strCompany = Request.QueryString["strCompany"].Replace("_", "','") != "" ? "'" + Request.QueryString["strCompany"].Replace("_", "','") + "'" : Request.QueryString["strCompany"].Replace("_", "','");
            filter.strCustomer = Request.QueryString["strCustomer"].Replace("_", "','") != "" ? "'" + Request.QueryString["strCustomer"].Replace("_", "','") + "'" : Request.QueryString["strCustomer"].Replace("_", "','");
            filter.strSTIP = Request.QueryString["strSTIP"].Replace("_", ",");
            filter.strProduct = Request.QueryString["strProduct"].Replace("_", ",");
            filter.strMonth = Request.QueryString["strMonth"].Replace("_", ",");
            filter.AmountMode = Convert.ToBoolean(Request.QueryString["bitAmount"]);

            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            List<vmDashboardBAUKActivity> dataToExport = dashBAUKService.GetDashboardActivity(userCredential.UserID, filter);

            var totRow = new vmDashboardBAUKActivity();

            foreach (var row in dataToExport)
            {
                totRow.AmountBAUKApproved += row.AmountBAUKApproved;
                totRow.AmountBAUKRejected += row.AmountBAUKRejected;
                totRow.AmountBAUKSubmitted += row.AmountBAUKSubmitted;
                totRow.AmountRFIDone += row.AmountRFIDone;
                totRow.AmountTotal += row.AmountTotal;
                totRow.BAUKApproved += row.BAUKApproved;
                totRow.BAUKRejected += row.BAUKRejected;
                totRow.BAUKSubmitted += row.BAUKSubmitted;
                totRow.RFIDone += row.RFIDone;
                totRow.Total += row.Total;
                totRow.PercentTotal += row.PercentTotal;
            }
            totRow.GroupSum = "Total";

            dataToExport.Add(totRow);

            foreach (var row in dataToExport)
            {
                row.PercentTotal = Decimal.Round(row.PercentTotal,2);
            }

            DataTable table = new DataTable();
            if (filter.AmountMode == false)
            {
                var reader = FastMember.ObjectReader.Create(dataToExport, "GroupSum", "RFIDone", "BAUKSubmitted", "BAUKApproved", "BAUKRejected", "Total", "PercentTotal");
                table.Load(reader);

                table.Columns["GroupSum"].ColumnName = filter.GroupBy;
                table.Columns["RFIDone"].ColumnName = "RFI Done";
                table.Columns["BAUKSubmitted"].ColumnName = "BAUK Submitted";
                table.Columns["BAUKApproved"].ColumnName = "BAUK Approved";
                table.Columns["BAUKRejected"].ColumnName = "BAUK Rejected";
                table.Columns["Total"].ColumnName = "Total BAUK Activity";
                table.Columns["PercentTotal"].ColumnName = "BAUK Percentage (%)";
                table.AcceptChanges();
            }
            else
            {
                var reader = FastMember.ObjectReader.Create(dataToExport, "GroupSum", "AmountRFIDone", "AmountBAUKSubmitted", "AmountBAUKApproved", "AmountBAUKRejected", "AmountTotal", "PercentTotal");
                table.Load(reader);

                table.Columns["GroupSum"].ColumnName = filter.GroupBy;
                table.Columns["AmountRFIDone"].ColumnName = "RFI Done (Rp)";
                table.Columns["AmountBAUKSubmitted"].ColumnName = "BAUK Submitted (Rp)";
                table.Columns["AmountBAUKApproved"].ColumnName = "BAUK Approved (Rp)";
                table.Columns["AmountBAUKRejected"].ColumnName = "BAUK Rejected (Rp)";
                table.Columns["AmountTotal"].ColumnName = "Total BAUK Activity (Rp)";
                table.Columns["PercentTotal"].ColumnName = "BAUK Percentage (%)";
                table.AcceptChanges();
            }

            //Export to Excel
            ExportToExcelHelper.Export("BAUK Analysis - Activity", table);
        }

        [Route("BAUK/Forecast/Export")]
        public void ExportBAUKForecast()
        {
            var filter = new vmDashboardBAUKFilter();

            filter.Month = Convert.ToInt32(Request.QueryString["intMonth"]);
            filter.Year = Convert.ToInt32(Request.QueryString["intYear"]);
            filter.GroupBy = Request.QueryString["strGroupBy"];
            filter.strCompany = Request.QueryString["strCompany"].Replace("_", "','") != "" ? "'" + Request.QueryString["strCompany"].Replace("_", "','") + "'" : Request.QueryString["strCompany"].Replace("_", "','");
            filter.strCustomer = Request.QueryString["strCustomer"].Replace("_", "','") != "" ? "'" + Request.QueryString["strCustomer"].Replace("_", "','") + "'" : Request.QueryString["strCustomer"].Replace("_", "','");
            filter.strSTIP = Request.QueryString["strSTIP"].Replace("_", ",");
            filter.strProduct = Request.QueryString["strProduct"].Replace("_", ",");
            filter.AmountMode = Convert.ToBoolean(Request.QueryString["bitAmount"]);

            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            List<vmDashboardBAUKForecast> dataToExport = dashBAUKService.GetDashboardForecast(userCredential.UserID, filter);

            var totRow = new vmDashboardBAUKForecast();

            foreach (var row in dataToExport)
            {
                totRow.AmountTotOutstanding += row.AmountTotOutstanding;
                totRow.AmountTotW1 += row.AmountTotW1;
                totRow.AmountTotW2 += row.AmountTotW2;
                totRow.AmountTotW3 += row.AmountTotW3;
                totRow.AmountTotW4 += row.AmountTotW4;
                totRow.AmountTotW5 += row.AmountTotW5;
                totRow.AmountTotM1 += row.AmountTotM1;
                totRow.AmountTotM2 += row.AmountTotM2;
                totRow.AmountTotM3 += row.AmountTotM3;
                totRow.AmountTotM4 += row.AmountTotM4;
                totRow.AmountTotM5 += row.AmountTotM5;
                totRow.AmountTotNA += row.AmountTotNA;
                totRow.TotOutstanding += row.TotOutstanding;
                totRow.TotW1 += row.TotW1;
                totRow.TotW2 += row.TotW2;
                totRow.TotW3 += row.TotW3;
                totRow.TotW4 += row.TotW4;
                totRow.TotW5 += row.TotW5;
                totRow.TotM1 += row.TotM1;
                totRow.TotM2 += row.TotM2;
                totRow.TotM3 += row.TotM3;
                totRow.TotM4 += row.TotM4;
                totRow.TotM5 += row.TotM5;
                totRow.TotNA += row.TotNA;
            }
            totRow.GroupSum = "Total";

            dataToExport.Add(totRow);

            string[] monthList = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            DateTime dtFilter = new DateTime(filter.Year, filter.Month, 1);

            DataTable table = new DataTable();
            if (filter.AmountMode == false)
            {
                var reader = FastMember.ObjectReader.Create(dataToExport, "GroupSum", "TotOutstanding", "TotW1", "TotW2",
                    "TotW3", "TotW4", "TotW5", "TotM1", "TotM2", "TotM3", "TotM4", "TotM5", "TotNA");
                table.Load(reader);

                table.Columns["GroupSum"].ColumnName = filter.GroupBy;
                table.Columns["TotOutstanding"].ColumnName = "Total BAUK Forecasted";
                table.Columns["TotW1"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(-1).Month - 1] + "-" + dtFilter.AddMonths(-1).Year + " Week 1";
                table.Columns["TotW2"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(-1).Month - 1] + "-" + dtFilter.AddMonths(-1).Year + " Week 2";
                table.Columns["TotW3"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(-1).Month - 1] + "-" + dtFilter.AddMonths(-1).Year + " Week 3";
                table.Columns["TotW4"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(-1).Month - 1] + "-" + dtFilter.AddMonths(-1).Year + " Week 4";
                table.Columns["TotW5"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(-1).Month - 1] + "-" + dtFilter.AddMonths(-1).Year + " Week 5";
                table.Columns["TotM1"].ColumnName = "Forecast " + monthList[dtFilter.Month - 1] + "-" + dtFilter.Year;
                table.Columns["TotM2"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(1).Month - 1] + "-" + dtFilter.AddMonths(1).Year;
                table.Columns["TotM3"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(2).Month - 1] + "-" + dtFilter.AddMonths(2).Year;
                table.Columns["TotM4"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(3).Month - 1] + "-" + dtFilter.AddMonths(3).Year;
                table.Columns["TotM5"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(4).Month - 1] + "-" + dtFilter.AddMonths(4).Year;
                table.Columns["TotNA"].ColumnName = "Forecast > " + monthList[dtFilter.AddMonths(4).Month - 1] + "-" + dtFilter.AddMonths(4).Year;
                table.AcceptChanges();
            }
            else
            {
                var reader = FastMember.ObjectReader.Create(dataToExport, "GroupSum", "AmountTotOutstanding", "AmountTotW1", "AmountTotW2",
                     "AmountTotW3", "AmountTotW4", "AmountTotW5", "AmountTotM1", "AmountTotM2", "AmountTotM3", "AmountTotM4", "AmountTotM5", "AmountTotNA");
                table.Load(reader);

                table.Columns["GroupSum"].ColumnName = filter.GroupBy;
                table.Columns["AmountTotOutstanding"].ColumnName = "Total BAUK Forecasted (Rp)";
                table.Columns["AmountTotW1"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(-1).Month - 1] + "-" + dtFilter.AddMonths(-1).Year + " Week 1 (Rp)";
                table.Columns["AmountTotW2"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(-1).Month - 1] + "-" + dtFilter.AddMonths(-1).Year + " Week 2 (Rp)";
                table.Columns["AmountTotW3"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(-1).Month - 1] + "-" + dtFilter.AddMonths(-1).Year + " Week 3 (Rp)";
                table.Columns["AmountTotW4"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(-1).Month - 1] + "-" + dtFilter.AddMonths(-1).Year + " Week 4 (Rp)";
                table.Columns["AmountTotW5"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(-1).Month - 1] + "-" + dtFilter.AddMonths(-1).Year + " Week 5 (Rp)";
                table.Columns["AmountTotM1"].ColumnName = "Forecast " + monthList[dtFilter.Month - 1] + "-" + dtFilter.Year + " (Rp)";
                table.Columns["AmountTotM2"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(1).Month - 1] + "-" + dtFilter.AddMonths(1).Year + " (Rp)";
                table.Columns["AmountTotM3"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(2).Month - 1] + "-" + dtFilter.AddMonths(2).Year + " (Rp)";
                table.Columns["AmountTotM4"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(3).Month - 1] + "-" + dtFilter.AddMonths(3).Year + " (Rp)";
                table.Columns["AmountTotM5"].ColumnName = "Forecast " + monthList[dtFilter.AddMonths(4).Month - 1] + "-" + dtFilter.AddMonths(4).Year + " (Rp)";
                table.Columns["AmountTotNA"].ColumnName = "Forecast > " + monthList[dtFilter.AddMonths(4).Month - 1] + "-" + dtFilter.AddMonths(4).Year + " (Rp)";
                table.AcceptChanges();
            }

            //Export to Excel
            ExportToExcelHelper.Export("BAUK Analysis - Forecast", table);
        }

        [Route("BAUK/Achievement/Export")]
        public void ExportBAUKAchievement()
        {
            var filter = new vmDashboardBAUKFilter();

            filter.Year = Convert.ToInt32(Request.QueryString["intYear"]);
            filter.GroupBy = Request.QueryString["strGroupBy"];
            filter.strCompany = Request.QueryString["strCompany"].Replace("_", "','") != "" ? "'" + Request.QueryString["strCompany"].Replace("_", "','") + "'" : Request.QueryString["strCompany"].Replace("_", "','");
            filter.strCustomer = Request.QueryString["strCustomer"].Replace("_", "','") != "" ? "'" + Request.QueryString["strCustomer"].Replace("_", "','") + "'" : Request.QueryString["strCustomer"].Replace("_", "','");
            filter.strSTIP = Request.QueryString["strSTIP"].Replace("_", ",");
            filter.strProduct = Request.QueryString["strProduct"].Replace("_", ",");

            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            List<vmDashboardBAUKAchievement> dataToExport = dashBAUKService.GetDashboardAchievement(userCredential.UserID, filter);

            var totRow = new vmDashboardBAUKAchievement();

            foreach (var row in dataToExport)
            {
                totRow.TotM1 += row.TotM1;
                totRow.TotM2 += row.TotM2;
                totRow.TotM3 += row.TotM3;
                totRow.TotM4 += row.TotM4;
                totRow.TotM5 += row.TotM5;
                totRow.TotM6 += row.TotM6;
                totRow.TotM7 += row.TotM7;
                totRow.TotM8 += row.TotM8;
                totRow.TotM9 += row.TotM9;
                totRow.TotM10 += row.TotM10;
                totRow.TotM11 += row.TotM11;
                totRow.TotM12 += row.TotM12;
                totRow.TotLTM1 += row.TotLTM1;
                totRow.TotLTM2 += row.TotLTM2;
                totRow.TotLTM3 += row.TotLTM3;
                totRow.TotLTM4 += row.TotLTM4;
                totRow.TotLTM5 += row.TotLTM5;
                totRow.TotLTM6 += row.TotLTM6;
                totRow.TotLTM7 += row.TotLTM7;
                totRow.TotLTM8 += row.TotLTM8;
                totRow.TotLTM9 += row.TotLTM9;
                totRow.TotLTM10 += row.TotLTM10;
                totRow.TotLTM11 += row.TotLTM11;
                totRow.TotLTM12 += row.TotLTM12;
            }
            totRow.GroupSum = "Total";

            dataToExport.Add(totRow);

            foreach (var row in dataToExport)
            {
                row.TotLTM1 = row.TotM1 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM1) / Convert.ToDecimal(row.TotM1));
                row.TotLTM2 = row.TotM2 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM2) / Convert.ToDecimal(row.TotM2));
                row.TotLTM3 = row.TotM3 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM3) / Convert.ToDecimal(row.TotM3));
                row.TotLTM4 = row.TotM4 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM4) / Convert.ToDecimal(row.TotM4));
                row.TotLTM5 = row.TotM5 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM5) / Convert.ToDecimal(row.TotM5));
                row.TotLTM6 = row.TotM6 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM6) / Convert.ToDecimal(row.TotM6));
                row.TotLTM7 = row.TotM7 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM7) / Convert.ToDecimal(row.TotM7));
                row.TotLTM8 = row.TotM8 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM8) / Convert.ToDecimal(row.TotM8));
                row.TotLTM9 = row.TotM9 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM9) / Convert.ToDecimal(row.TotM9));
                row.TotLTM10 = row.TotM10 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM10) / Convert.ToDecimal(row.TotM10));
                row.TotLTM11 = row.TotM11 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM11) / Convert.ToDecimal(row.TotM11));
                row.TotLTM12 = row.TotM12 == 0 ? 0 : Convert.ToInt32(Convert.ToDecimal(row.TotLTM12) / Convert.ToDecimal(row.TotM12));
            }

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataToExport, "GroupSum", "TotM1", "TotLTM1", "TotM2", "TotLTM2", "TotM3", "TotLTM3", "TotM4", "TotLTM4",
                    "TotM5", "TotLTM5", "TotM6", "TotLTM6", "TotM7", "TotLTM7", "TotM8", "TotLTM8", "TotM9", "TotLTM9", "TotM10", "TotLTM10", "TotM11", "TotLTM11", "TotM12", "TotLTM12");
            table.Load(reader);

            table.Columns["GroupSum"].ColumnName = filter.GroupBy;
            table.Columns["TotM1"].ColumnName = "BAUK Jan " + filter.Year;
            table.Columns["TotLTM1"].ColumnName = "LT(Avg) Jan " + filter.Year;
            table.Columns["TotM2"].ColumnName = "BAUK Feb " + filter.Year;
            table.Columns["TotLTM2"].ColumnName = "LT(Avg) Feb " + filter.Year;
            table.Columns["TotM3"].ColumnName = "BAUK Mar " + filter.Year;
            table.Columns["TotLTM3"].ColumnName = "LT(Avg) Mar " + filter.Year;
            table.Columns["TotM4"].ColumnName = "BAUK Apr " + filter.Year;
            table.Columns["TotLTM4"].ColumnName = "LT(Avg) Apr " + filter.Year;
            table.Columns["TotM5"].ColumnName = "BAUK May " + filter.Year;
            table.Columns["TotLTM5"].ColumnName = "LT(Avg) May " + filter.Year;
            table.Columns["TotM6"].ColumnName = "BAUK Jun " + filter.Year;
            table.Columns["TotLTM6"].ColumnName = "LT(Avg) Jun " + filter.Year;
            table.Columns["TotM7"].ColumnName = "BAUK Jul " + filter.Year;
            table.Columns["TotLTM7"].ColumnName = "LT(Avg) Jul " + filter.Year;
            table.Columns["TotM8"].ColumnName = "BAUK Aug " + filter.Year;
            table.Columns["TotLTM8"].ColumnName = "LT(Avg) Aug " + filter.Year;
            table.Columns["TotM9"].ColumnName = "BAUK Sep " + filter.Year;
            table.Columns["TotLTM9"].ColumnName = "LT(Avg) Sep " + filter.Year;
            table.Columns["TotM10"].ColumnName = "BAUK Oct " + filter.Year;
            table.Columns["TotLTM10"].ColumnName = "LT(Avg) Oct " + filter.Year;
            table.Columns["TotM11"].ColumnName = "BAUK Nov " + filter.Year;
            table.Columns["TotLTM11"].ColumnName = "LT(Avg) Nov " + filter.Year;
            table.Columns["TotM12"].ColumnName = "BAUK Dec " + filter.Year;
            table.Columns["TotLTM12"].ColumnName = "LT(Avg) Dec " + filter.Year;
            table.AcceptChanges();

            //Export to Excel
            ExportToExcelHelper.Export("BAUK Analysis - Achievement", table);
        }

        [Route("BAUK/Reject/Export")]
        public void ExportBAUKReject()
        {
            var filter = new vmDashboardBAUKFilter();

            filter.Month = Convert.ToInt32(Request.QueryString["intMonth"]);
            filter.Year = Convert.ToInt32(Request.QueryString["intYear"]);
            filter.GroupBy = Request.QueryString["strGroupBy"];
            filter.strCompany = Request.QueryString["strCompany"].Replace("_", "','") != "" ? "'" + Request.QueryString["strCompany"].Replace("_", "','") + "'" : Request.QueryString["strCompany"].Replace("_", "','");
            filter.strCustomer = Request.QueryString["strCustomer"].Replace("_", "','") != "" ? "'" + Request.QueryString["strCustomer"].Replace("_", "','") + "'" : Request.QueryString["strCustomer"].Replace("_", "','");
            filter.strSTIP = Request.QueryString["strSTIP"].Replace("_", ",");
            filter.strMonth = Request.QueryString["strMonth"].Replace("_", ",");
            filter.strProduct = Request.QueryString["strProduct"].Replace("_", ",");

            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            List<vmDashboardBAUKReject> dataToExport = dashBAUKService.GetDashboardReject(userCredential.UserID, filter);

            var totRow = new vmDashboardBAUKReject();

            foreach (var row in dataToExport)
            {
                totRow.TotReject += row.TotReject;
                totRow.TotImproper += row.TotImproper;
                totRow.TotUncompleted += row.TotUncompleted;
                totRow.TotWrong += row.TotWrong;
                totRow.TotOther += row.TotOther;
                totRow.PercentTotal += row.PercentTotal;

                row.PercentTotal = Decimal.Round(row.PercentTotal, 2);
            }
            totRow.GroupSum = "Total";

            dataToExport.Add(totRow);

            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataToExport, "GroupSum", "TotReject", "TotImproper", "TotUncompleted", "TotWrong", "TotOther", "PercentTotal");
            table.Load(reader);

            table.Columns["GroupSum"].ColumnName = filter.GroupBy;
            table.Columns["TotReject"].ColumnName = "Total BAUK Rejected";
            table.Columns["TotImproper"].ColumnName = "BAUK Rejected - Improper Doc";
            table.Columns["TotUncompleted"].ColumnName = "BAUK Rejected - Uncompleted Doc";
            table.Columns["TotWrong"].ColumnName = "BAUK Rejected - Wrong Doc";
            table.Columns["TotOther"].ColumnName = "BAUK Rejected - Other Reason";
            table.Columns["PercentTotal"].ColumnName = "BAUK Rejected Percentage (%)";
            table.AcceptChanges();

            //Export to Excel
            ExportToExcelHelper.Export("BAUK Analysis - Reject", table);
        }

        [Route("BAUK/Detail/Export")]
        public void ExportBAUKDetail()
        {
            var post = new vmDashboardBAUKDetail();

            var TabMode = "";

            if (Request.Cookies["ExportParams"] != null)
            {
                var jsonParam = Request.Cookies["ExportParams"].Value.Replace("%22", "\"").Replace("%2C", ",").Replace("%20", " ");
                dynamic param = JsonConvert.DeserializeObject(jsonParam);

                post.strCompany = param.CompanyIDs == "''" ? "" : param.CompanyIDs;
                post.strCustomer = param.CustomerIDs == "''" ? "" : param.CustomerIDs;
                post.strSTIP = param.STIPIDs;
                post.strProduct = param.ProductIDs;
                post.strMonth = param.Months;
                post.Month = param.Month;
                post.Year = param.Year;
                post.SelectedData = param.SelectedData;
                TabMode = param.TabMode;
            }

            //Call Service
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            var list = new List<vmDashboardBAUKDetail>();

            if (TabMode == "Activity")
            {
                list = dashBAUKService.GetDashboardActivityDetail(userCredential.UserID, post).ToList();
            }
            else if (TabMode == "Forecast")
            {
                list = dashBAUKService.GetDashboardForecastDetail(userCredential.UserID, post).ToList();
            }
            else if (TabMode == "Achievement")
            {
                list = dashBAUKService.GetDashboardAchievementDetail(userCredential.UserID, post).ToList();
            }

            string[] fields = new string[] { "SONumber", "CustomerName", "SiteID", "SiteName", "CustomerSiteID", "CustomerSiteName", "Company",
                "STIPCode", "Product", "RegionName", "ProvinceName", "ResidenceName", "STIPAmount", "LeadTime", "BAUKFirstSubmitDate", "BAUKLastSubmitDate",
                "BAUKApprovalDate", "BAUKForecastDate", "BAUKStatus", "RFIDoneDate" };

            //Convert to DataTable
            var table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fields);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("Dashboard BAUK Detail", table);
        }

        private string GetWhereClause(Models.PostTrxBapsDataView post)
        {
            string strWhereClause = "";
            if (!string.IsNullOrWhiteSpace(post.strCompanyId))
            {
                strWhereClause += "CompanyInvoice = '" + post.strCompanyId + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strOperator))
            {
                strWhereClause += "CustomerInvoice = '" + post.strOperator + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strStatusBAPS))
            {
                strWhereClause += "Status = '" + post.strStatusBAPS + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPeriodInvoice))
            {
                strWhereClause += "PeriodInvoice = '" + post.strPeriodInvoice + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strInvoiceType))
            {
                strWhereClause += "InvoiceType = '" + post.strInvoiceType + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strCurrency))
            {
                strWhereClause += "Currency = '" + post.strCurrency + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPONumber))
            {
                strWhereClause += "PONumber LIKE '%" + post.strPONumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strBAPSNumber))
            {
                strWhereClause += "BAPSNumber LIKE '%" + post.strBAPSNumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSONumber))
            {
                strWhereClause += "SoNumber LIKE '%" + post.strSONumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strBapsType))
            {
                strWhereClause += "TowerTypeID LIKE '%" + post.strBapsType + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteIdOld))
            {
                strWhereClause += "SiteID LIKE '%" + post.strSiteIdOld + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strStartPeriod))
            {
                strWhereClause += "CONVERT(VARCHAR, StartInvoiceDate, 106) LIKE '%" + post.strStartPeriod.Replace('-', ' ') + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strEndPeriod))
            {
                strWhereClause += "CONVERT(VARCHAR, EndInvoiceDate, 106) LIKE '%" + post.strEndPeriod.Replace('-', ' ') + "%' AND ";
                //strWhereClause += "EndDateInvoice <= '" + DateTime.Parse(strEndPeriod).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd") + "' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strCreatedBy))
            {
                strWhereClause += "CreatedBy LIKE '%" + post.strCreatedBy + "%' AND ";
            }
            strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

            return strWhereClause;
        }
        #endregion

        #region Dynamic Dashboard By MTR
        [Authorize]
        [Route("DynamicDashboard")]
        public ActionResult DynamicDashboard()
        {
            string actionTokenView = "B1FAA0AF-8F62-4A9B-BF57-FBD4CD6F4CA6";

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
        [Authorize]
        [Route("DashboardToPDF")]
        public ActionResult DashboardToPDF(string pageSize, string pageOrientation)
        {

            return new Rotativa.ViewAsPdf("~/Views/Dashboard/PrintDashboard.cshtml")
            {
                FileName = DateTime.Now.ToString("yyyyMMddhhmm") + ".pdf",
                PageSize = pageSize == "A4" ? Rotativa.Options.Size.A4 : Rotativa.Options.Size.Letter,
                PageOrientation = pageOrientation == "Landscape" ? Rotativa.Options.Orientation.Landscape : Rotativa.Options.Orientation.Portrait

            };
        }

        [Authorize]
        [Route("DataSourceDashboard")]

        public ActionResult DataSourceDashboard()
        {
            string actionTokenView = "45A3E284-45B0-4C5D-BE10-B4993396E0BF";


            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    //ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        #endregion

        #region Monitoring Aging Executive
        [Authorize]
        [Route("MonitoringAgingExecutive")]
        public ActionResult MonitoringAgingExecutive()
        {
            string actionTokenView = "15897D4D-5D64-4B46-8EEF-C893CFE0FA31";

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

        [Route("MonitoringAgingExecutive/Detail/Export")]
        public void ExportMonitoringAgingExecutiveDetail(vmMonitoringAgingExecutive param)
        {
            var list = new List<vmMonitoringAgingExecutiveSummary>();

            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            list = _agingExecutiveService.GetDetail(userCredential.UserID, param).ToList();

            string[] fields = new string[] { "InvNo", "CompanyID", "OperatorID", "BucketAging", "aOutstandingGross", "aOutstandingNett", "AmountBankOut", "PAM", "InvoiceType", "PowerType",
                                            "TenantType", "STIPCode", "ARCApproveDate", "PaidDate", "PaidStatus"};

            //Convert to DataTable
            var table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fields);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("AgingExecutiveDetail", table);
        }

        [Route("MonitoringAgingExecutive/Export")]
        public void ExportMonitoringAgingExecutive(vmMonitoringAgingExecutive param)
        {
            var list = new List<vmMonitoringAgingExecutiveSummary>();

            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            list = _agingExecutiveService.GetSummary(userCredential.UserID, param).ToList();

            DataTable table = new DataTable();

            if (param.vCategory == 1)
            {
                string[] fields = new string[] { "Operator", "Current", "AmountOD1_30", "AmountOD1_60", "AmountODAbove90", "TotalOS", "PercetageODPerOpt", "PercentageODAllOpt" };

                try
                {
                    var reader = FastMember.ObjectReader.Create(list.Select(i => new
                    {
                        Operator = i.OperatorID,
                        Current = i.AmountCurrent,
                        AmountOD1_30 = i.AmountOD_30,
                        AmountOD1_60 = i.AmountOD_60,
                        AmountODAbove90 = i.AmountOD_90,
                        TotalOS = i.TotalOS,
                        PercetageODPerOpt = i.PercetageODPerOpt,
                        PercentageODAllOpt = i.PercentageODAllOpt
                    }), fields);
                    table.Load(reader);

                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                //Export to Excel
                ExportToExcelHelper.Export("AgingExecutiveDetailAllCategory", table);
            } 
            else if (param.vCategory == 2)
            {
                string[] fields = new string[] { "Operator", "Current", "AmountODAbove30", "TotalOS", "PercetageODPerOpt", "PercentageODAllOpt" };

                try
                {
                    var reader = FastMember.ObjectReader.Create(list.Select(i => new
                    {
                        Operator = i.OperatorID,
                        Current = i.AmountCurrent,
                        AmountODAbove30 = i.AmountOD_30,
                        TotalOS = i.TotalOS,
                        PercetageODPerOpt = i.PercetageODPerOpt,
                        PercentageODAllOpt = i.PercentageODAllOpt
                    }), fields);
                    table.Load(reader);

                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                //Export to Excel
                ExportToExcelHelper.Export("AgingExecutiveDetail30DCategory", table);
            }
            else
            {
                string[] fields = new string[] { "Operator", "Current", "AmountODAbove60", "TotalOS", "PercetageODPerOpt", "PercentageODAllOpt" };

                try
                {
                    var reader = FastMember.ObjectReader.Create(list.Select(i => new
                    {
                        Operator = i.OperatorID,
                        Current = i.AmountCurrent,
                        AmountODAbove60 = i.AmountOD_90,
                        TotalOS = i.TotalOS,
                        PercetageODPerOpt = i.PercetageODPerOpt,
                        PercentageODAllOpt = i.PercentageODAllOpt
                    }), fields);
                    table.Load(reader);

                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                //Export to Excel
                ExportToExcelHelper.Export("AgingExecutiveDetail60DCategory", table);
            }

        }
        #endregion

        #region Invoice Production
        [Route("InvoiceProduction/Header/Export")]
        public void ExportHeader(vmInvoiceProductionPost param)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            Datatable<vmInvoiceProduction> result = _invoiceProduction.GetHeaderList(userCredential.UserID, param);

            string[] fields = new string[] { "InvoiceNumber", "SubjectInvoice", "AmountInvoice", "Operator", "Company", "TypeInvoice", "CreateInvoice", "PostingInvoice", "SubmitDocInvoice",
                                            "ApproveDocInvoice", "ReceiptDocInvoice", "TypePayment", "PaymentDate", "DocumentPayment", "DocumentPaymentIntegration" };

            //Convert to DataTable
            var table = new DataTable();
            var reader = FastMember.ObjectReader.Create(result.List.Select(i => new
            {
                i.InvoiceNumber,
                i.SubjectInvoice,
                i.AmountInvoice,
                i.Operator ,
                i.Company ,
                i.TypeInvoice,
                CreateInvoice = string.IsNullOrEmpty(i.CreateInvoice.ToString()) ? "" : DateTime.Parse(i.CreateInvoice.ToString()).ToString("dd-MMM-yyyy"),
                PostingInvoice = string.IsNullOrEmpty(i.PostingInvoice.ToString()) ? "" : DateTime.Parse(i.PostingInvoice.ToString()).ToString("dd-MMM-yyyy"),
                SubmitDocInvoice = string.IsNullOrEmpty(i.SubmitDocInvoice.ToString()) ? "" : DateTime.Parse(i.SubmitDocInvoice.ToString()).ToString("dd-MMM-yyyy"),
                ApproveDocInvoice = string.IsNullOrEmpty(i.ApproveDocInvoice.ToString()) ? "" : DateTime.Parse(i.ApproveDocInvoice.ToString()).ToString("dd-MMM-yyyy"),
                ReceiptDocInvoice = string.IsNullOrEmpty(i.ReceiptDocInvoice.ToString()) ? "" : DateTime.Parse(i.ReceiptDocInvoice.ToString()).ToString("dd-MMM-yyyy"),
                i.TypePayment,
                PaymentDate = string.IsNullOrEmpty(i.PaymentDate.ToString()) ? "" : DateTime.Parse(i.PaymentDate.ToString()).ToString("dd-MMM-yyyy"),
                i.DocumentPayment,
                DocumentPaymentIntegration = string.IsNullOrEmpty(i.DocumentPaymentIntegration.ToString()) ? "" : DateTime.Parse(i.DocumentPaymentIntegration.ToString()).ToString("dd-MMM-yyyy")
            }), fields);
            table.Load(reader);

            string vType = "";
            if (param.vType == "LT1")
                vType = "LTCreateInv";
            else if (param.vType == "LT2")
                vType = "LTReceiptARR";
            else if (param.vType == "LT3")
                vType = "LTSubmitOpr";
            else
                vType = param.vType;

            string vGroup = "";
            if (param.vGroup == 1)
                vGroup = "OutStanding";
            else
                vGroup = "Production";

            //Export to Excel
            ExportToExcelHelper.Export(vType + vGroup + "Header", table);
        }

        [Route("InvoiceProduction/Detail/Export")]
        public void ExportDetail(vmInvoiceProductionPost param)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            Datatable<vmInvoiceProduction> result = _invoiceProduction.GetDetailList(userCredential.UserID, param);

            string[] fields = new string[] { "SONumber", "SiteID", "SiteName", "SiteIDOpr", "SiteNameOpr", "Operator", "Company", "BapsType", "StartPeriodInvoice",
                                            "EndPeriodInvoice", "AmountInvoice", "ReceiveDate", "ConfirmDate", "InvoiceNumber", "CreateInvoiceDate",
                                            "PostingDate", "PrintDate", "SubmitChecklistDate", "ApproveChecklistDate" };

            //Convert to DataTable
            var table = new DataTable();
            var reader = FastMember.ObjectReader.Create(result.List.Select(i => new
            {
                i.SONumber,
                i.SiteID,
                i.SiteName,
                i.SiteIDOpr,
                i.SiteNameOpr,
                i.Operator,
                i.Company,
                i.BapsType,
                StartPeriodInvoice = string.IsNullOrEmpty(i.StartPeriodInvoice.ToString()) ? "" : DateTime.Parse(i.StartPeriodInvoice.ToString()).ToString("dd-MMM-yyyy"),
                EndPeriodInvoice = string.IsNullOrEmpty(i.EndPeriodInvoice.ToString()) ? "" : DateTime.Parse(i.EndPeriodInvoice.ToString()).ToString("dd-MMM-yyyy"),
                i.AmountInvoice,
                ReceiveDate = string.IsNullOrEmpty(i.ReceiveDate.ToString()) ? "" : DateTime.Parse(i.ReceiveDate.ToString()).ToString("dd-MMM-yyyy"),
                ConfirmDate = string.IsNullOrEmpty(i.ConfirmDate.ToString()) ? "" : DateTime.Parse(i.ConfirmDate.ToString()).ToString("dd-MMM-yyyy"),
                i.InvoiceNumber,
                CreateInvoiceDate = string.IsNullOrEmpty(i.CreateInvoiceDate.ToString()) ? "" : DateTime.Parse(i.CreateInvoiceDate.ToString()).ToString("dd-MMM-yyyy"),
                PostingDate = string.IsNullOrEmpty(i.PostingDate.ToString()) ? "" : DateTime.Parse(i.PostingDate.ToString()).ToString("dd-MMM-yyyy"),
                PrintDate = string.IsNullOrEmpty(i.PrintDate.ToString()) ? "" : DateTime.Parse(i.PrintDate.ToString()).ToString("dd-MMM-yyyy"),
                SubmitChecklistDate = string.IsNullOrEmpty(i.SubmitChecklistDate.ToString()) ? "" : DateTime.Parse(i.SubmitChecklistDate.ToString()).ToString("dd-MMM-yyyy"),
                ApproveChecklistDate = string.IsNullOrEmpty(i.ApproveChecklistDate.ToString()) ? "" : DateTime.Parse(i.ApproveChecklistDate.ToString()).ToString("dd-MMM-yyyy")
            }), fields);
            table.Load(reader);

            string vType = "";
            if (param.vType == "LT1")
                vType = "LTCreateInv";
            else if (param.vType == "LT2")
                vType = "LTReceiptARR";
            else if (param.vType == "LT3")
                vType = "LTSubmitOpr";
            else
                vType = param.vType;

            string vGroup = "";
            if (param.vGroup == 1)
                vGroup = "OutStanding";
            else
                vGroup = "Production";

            //Export to Excel
            ExportToExcelHelper.Export(vType + vGroup + "Detail", table);
        }

        [Route("InvoiceProduction/All/Export")]
        public void ExportAll(vmInvoiceProductionPost param)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            Datatable<vmInvoiceProduction> result = _invoiceProduction.GetAllData(userCredential.UserID, param);

            string[] fields = new string[] { "SONumber", "SiteID", "SiteName", "SiteIDOpr", "SiteNameOpr", "BapsType", "StartPeriodInvoice",
                                            "EndPeriodInvoice", "AmountInvoice", "Operator", "Company", "ReceiveDate", "ConfirmDate", "InvoiceNumber", "CreateInvoiceDate",
                                            "PostingDate", "PrintDate", "SubmitChecklistDate", "ApproveChecklistDate", "ReceiptDocInvoice", "TypePayment", "PaymentDate",
                                            "DocumentPayment", "DocumentPaymentIntegration" };

            //Convert to DataTable
            var table = new DataTable();
            var reader = FastMember.ObjectReader.Create(result.List.Select(i => new
            {
                i.SONumber,
                i.SiteID,
                i.SiteName,
                i.SiteIDOpr,
                i.SiteNameOpr,
                i.BapsType,
                StartPeriodInvoice = string.IsNullOrEmpty(i.StartPeriodInvoice.ToString()) ? "" : DateTime.Parse(i.StartPeriodInvoice.ToString()).ToString("dd-MMM-yyyy"),
                EndPeriodInvoice = string.IsNullOrEmpty(i.EndPeriodInvoice.ToString()) ? "" : DateTime.Parse(i.EndPeriodInvoice.ToString()).ToString("dd-MMM-yyyy"),
                i.AmountInvoice,
                i.Operator ,
                i.Company ,
                ReceiveDate = string.IsNullOrEmpty(i.ReceiveDate.ToString()) ? "" : DateTime.Parse(i.ReceiveDate.ToString()).ToString("dd-MMM-yyyy"),
                ConfirmDate = string.IsNullOrEmpty(i.ConfirmDate.ToString()) ? "" : DateTime.Parse(i.ConfirmDate.ToString()).ToString("dd-MMM-yyyy"),
                i.InvoiceNumber,
                CreateInvoiceDate = string.IsNullOrEmpty(i.CreateInvoiceDate.ToString()) ? "" : DateTime.Parse(i.CreateInvoiceDate.ToString()).ToString("dd-MMM-yyyy"),
                PostingDate = string.IsNullOrEmpty(i.PostingDate.ToString()) ? "" : DateTime.Parse(i.PostingDate.ToString()).ToString("dd-MMM-yyyy"),
                PrintDate = string.IsNullOrEmpty(i.PrintDate.ToString()) ? "" : DateTime.Parse(i.PrintDate.ToString()).ToString("dd-MMM-yyyy"),
                SubmitChecklistDate = string.IsNullOrEmpty(i.SubmitChecklistDate.ToString()) ? "" : DateTime.Parse(i.SubmitChecklistDate.ToString()).ToString("dd-MMM-yyyy"),
                ApproveChecklistDate = string.IsNullOrEmpty(i.ApproveChecklistDate.ToString()) ? "" : DateTime.Parse(i.ApproveChecklistDate.ToString()).ToString("dd-MMM-yyyy"),
                ReceiptDocInvoice = string.IsNullOrEmpty(i.ReceiptDocInvoice.ToString()) ? "" : DateTime.Parse(i.ReceiptDocInvoice.ToString()).ToString("dd-MMM-yyyy"),
                i.TypePayment,
                PaymentDate = string.IsNullOrEmpty(i.PaymentDate.ToString()) ? "" : DateTime.Parse(i.PaymentDate.ToString()).ToString("dd-MMM-yyyy"),
                i.DocumentPayment,
                DocumentPaymentIntegration = string.IsNullOrEmpty(i.DocumentPaymentIntegration.ToString()) ? "" : DateTime.Parse(i.DocumentPaymentIntegration.ToString()).ToString("dd-MMM-yyyy")
            }), fields);
            table.Load(reader);

            string vGroup = "";
            if (param.vGroup == 1)
                vGroup = "OutStanding";
            else
                vGroup = "Production";

            //Export to Excel
            ExportToExcelHelper.ExportExcelMultiSheet("Export" + vGroup + "All", table);
        }
        #endregion


        #region DASHBOARD INPUT TARGET
        [Authorize]
        [Route("InputTarget")]
        public ActionResult InputTarget()
        {
            string actionTokenView = "45b5d476-315b-4b70-ac9a-0dc3add058ab";

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

            return View("DashboardInputTarget");
        }
        [Authorize]
        [Route("UpdateInputTargetRatio")]
        public ActionResult UpdateInputTargetRatio()
        {
            string actionTokenView = "c8d2e27e-e740-40f7-9143-08bd4c68289c";

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

            return View("UpdateInputTargetRatio");
        }

        [Route("DashboardInputTarget/Export")]
        public void ExportDashboardInputTarget()
        {
            //Year: year,
            //Month: month,
            //DepartmentCode: departmentCode,
            //CompanyInvoiceID: $('#slSearchCompany').val(),
            //CustomerID: $('#slSearchCustomer').val(),
            //Parameter
            vwDashboardInputTargetDetail post = new vwDashboardInputTargetDetail();
            post.Year = Request.QueryString["Year"] == "" ? 0 : Convert.ToInt32(Request.QueryString["Year"]);
            post.Month = Request.QueryString["Month"] == "" ? 0 : Convert.ToInt32(Request.QueryString["Month"]);
            post.DepartmentCode = Request.QueryString["DepartmentCode"];
            post.CompanyInvoiceID = Request.QueryString["CompanyInvoiceID"];
            post.CustomerID = Request.QueryString["CustomerID"];



            //Call Service
            List<vwDashboardInputTargetDetail> detailInputTarget = new List<vwDashboardInputTargetDetail>();

            var client = new DashboardInputTargetService();
            int intTotalRecord = 0;

            intTotalRecord = client.GetCountInputTargetDetail(UserManager.User.UserToken, post);


            detailInputTarget = client.GetListInputTargetDetail(UserManager.User.UserToken, post, "", 0, 0);


            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { "SONumber", "SiteID", "SiteName", "CustomerSiteID", "CustomerSiteName",
                "CustomerID", "CompanyInvoiceID", "RegionalName", "StartInvoiceDate", "EndInvoiceDate",
            "AmountIDR", "AmountUSD", "DepartmentName" };
            var reader = FastMember.ObjectReader.Create(detailInputTarget.Select(i => new
            {
                i.SONumber,
                i.SiteID,
                i.SiteName,
                i.CustomerSiteID,
                i.CustomerSiteName,
                i.CustomerID,
                i.CompanyInvoiceID,
                i.RegionalName,
                i.StartInvoiceDate,
                i.EndInvoiceDate,
                i.AmountIDR,
                i.AmountUSD,
                i.DepartmentName
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            //ExportToExcelHelper.Export("ListInputTargetDetails", table);
            ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
            var ws = wbook.Worksheets.Add(table, "ListInputTargetDetails");
            ws.Column(11).Style.NumberFormat.Format = "#,##0";
            ws.Column(12).Style.NumberFormat.Format = "#,##0";
            HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=" + "ListInputTargetDetails" + ".xlsx");

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
        }
        [Route("DashboardInputTargetHeader/Export")]
        public ActionResult ExportDashboardInputTargetHeader()
        {
            PostDashboardInputTarget post = new PostDashboardInputTarget();
            post.Year = Request.QueryString["Year"] == "" ? 0 : Convert.ToInt32(Request.QueryString["Year"]);
            post.CustomerID = Request.QueryString["CustomerID"];
            post.CompanyInvoiceID = Request.QueryString["CompanyInvoiceID"];


            var client = new DashboardInputTargetService();

            //Call Service
            List<vwDashboardInputTarget> inputTarget = new List<vwDashboardInputTarget>();
            inputTarget = client.GetInputTargetDashboard(UserManager.User.UserToken, post.CompanyInvoiceID, post.CustomerID, post.Year.GetValueOrDefault());

            byte[] downloadBytes;
            using (var package = DashboardInputTargetHeaderTemlpate(inputTarget))
            {
                downloadBytes = package.GetAsByteArray();
            }
            string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(downloadBytes, XlsxContentType, "TBG - List Dashboard Input Target.xlsx");
        }
        [NonAction]
        private ExcelPackage DashboardInputTargetHeaderTemlpate(List<vwDashboardInputTarget> inputTarget)
        {
            List<string> detailMonthcolumns = new List<string>()
            {
                 "Target" ,
                 "Optimist" ,
                 "Most Likely" ,
                 "Pesimist" 
            };
            List<string> columns = new List<string>()
            {
                 "Department" ,
                 "Jan" ,
                 "Feb" ,
                 "Mar" ,
                 "Apr" ,
                 "May" ,
                 "Jun" ,
                 "Jul" ,
                 "Aug" ,
                 "Sep" ,
                 "Oct" ,
                 "Nov" ,
                 "Dec" 
            };   
            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "List Dashboard Input Target";
            package.Workbook.Properties.Author = "Tower Bersama Group";
            package.Workbook.Properties.Subject = "ListInputTarget";
            package.Workbook.Properties.Keywords = "TBG - ListInputTarget";

            var worksheet = package.Workbook.Worksheets.Add("ListInputTarget");
            int col = 1; int row = 1;
            worksheet.Cells[1, 1, 2, 1].Merge = true;
            worksheet.Cells[1, 1, 2, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            for (int i = 0; i < columns.Count(); i++)
            {
                var colS = col;
                worksheet.Cells[row, col].Value = columns[i];
                worksheet.Cells[row , col].Style.Font.Bold = true;
                worksheet.Cells[row , col].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[row , col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row , col].Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                if (columns[i] != "Department")
                {
                    for(int j =0; j < detailMonthcolumns.Count(); j++)
                    {
                        worksheet.Cells[row + 1, col].Value = detailMonthcolumns[j];
                        worksheet.Column(col).Width = 11;
                        worksheet.Cells[row + 1, col].Style.Font.Bold = true;
                        worksheet.Cells[row + 1, col].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[row + 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[row + 1, col].Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                        col = col + 1;

                    }
                    worksheet.Cells[row, colS, row, col-1].Merge = true;
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
            worksheet.Cells["B3:AW8"].Style.Numberformat.Format = "#,##0";
            worksheet.Column(1).Width = 28;

            foreach (var target in inputTarget)
            {
                worksheet.Cells[row, 1].Value = target.DepartmentName;
                worksheet.Cells[row, 1].Style.WrapText = true;
                worksheet.Cells[row, 2].Value =  (target.Jan_AmountIDR    > 0) ? target.Jan_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 3].Value =  (target.Jan_OptimistIDR  > 0) ? target.Jan_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 4].Value =  (target.Jan_MostLikelyIDR> 0) ? target.Jan_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 5].Value =  (target.Jan_PessimistIDR > 0) ? target.Jan_PessimistIDR  /1000000 : 0;
                worksheet.Cells[row, 6].Value =  (target.Feb_AmountIDR    > 0) ? target.Feb_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 7].Value =  (target.Feb_OptimistIDR  > 0) ? target.Feb_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 8].Value =  (target.Feb_MostLikelyIDR> 0) ? target.Feb_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 9].Value =  (target.Feb_PessimistIDR > 0) ? target.Feb_PessimistIDR  /1000000 : 0;
                worksheet.Cells[row, 10].Value = (target.Mar_AmountIDR    > 0) ? target.Mar_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 11].Value = (target.Mar_OptimistIDR  > 0) ? target.Mar_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 12].Value = (target.Mar_MostLikelyIDR> 0) ? target.Mar_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 13].Value = (target.Mar_PessimistIDR > 0) ? target.Mar_PessimistIDR  /1000000 : 0;
                worksheet.Cells[row, 14].Value = (target.Apr_AmountIDR    > 0) ? target.Apr_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 15].Value = (target.Apr_OptimistIDR  > 0) ? target.Apr_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 16].Value = (target.Apr_MostLikelyIDR> 0) ? target.Apr_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 17].Value = (target.Apr_PessimistIDR > 0) ? target.Apr_PessimistIDR  /1000000 : 0;
                worksheet.Cells[row, 18].Value = (target.May_AmountIDR    > 0) ? target.May_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 19].Value = (target.May_OptimistIDR  > 0) ? target.May_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 20].Value = (target.May_MostLikelyIDR> 0) ? target.May_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 21].Value = (target.May_PessimistIDR > 0) ? target.May_PessimistIDR  /1000000 : 0;
                worksheet.Cells[row, 22].Value = (target.Jun_AmountIDR    > 0) ? target.Jun_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 23].Value = (target.Jun_OptimistIDR  > 0) ? target.Jun_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 24].Value = (target.Jun_MostLikelyIDR> 0) ? target.Jun_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 25].Value = (target.Jun_PessimistIDR > 0) ? target.Jun_PessimistIDR  /1000000 : 0;
                worksheet.Cells[row, 26].Value = (target.Jul_AmountIDR    > 0) ? target.Jul_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 27].Value = (target.Jul_OptimistIDR  > 0) ? target.Jul_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 28].Value = (target.Jul_MostLikelyIDR> 0) ? target.Jul_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 29].Value = (target.Jul_PessimistIDR > 0) ? target.Jul_PessimistIDR  /1000000 : 0;
                worksheet.Cells[row, 30].Value = (target.Aug_AmountIDR    > 0) ? target.Aug_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 31].Value = (target.Aug_OptimistIDR  > 0) ? target.Aug_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 32].Value = (target.Aug_MostLikelyIDR> 0) ? target.Aug_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 33].Value = (target.Aug_PessimistIDR > 0) ? target.Aug_PessimistIDR  /1000000 : 0;
                worksheet.Cells[row, 34].Value = (target.Sep_AmountIDR    > 0) ? target.Sep_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 35].Value = (target.Sep_OptimistIDR  > 0) ? target.Sep_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 36].Value = (target.Sep_MostLikelyIDR> 0) ? target.Sep_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 37].Value = (target.Sep_PessimistIDR > 0) ? target.Sep_PessimistIDR  /1000000 : 0;
                worksheet.Cells[row, 38].Value = (target.Oct_AmountIDR    > 0) ? target.Oct_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 39].Value = (target.Oct_OptimistIDR  > 0) ? target.Oct_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 40].Value = (target.Oct_MostLikelyIDR> 0) ? target.Oct_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 41].Value = (target.Oct_PessimistIDR > 0) ? target.Oct_PessimistIDR  /1000000 : 0;
                worksheet.Cells[row, 42].Value = (target.Nov_AmountIDR    > 0) ? target.Nov_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 43].Value = (target.Nov_OptimistIDR  > 0) ? target.Nov_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 44].Value = (target.Nov_MostLikelyIDR> 0) ? target.Nov_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 45].Value = (target.Nov_PessimistIDR > 0) ? target.Nov_PessimistIDR  /1000000 : 0;
                worksheet.Cells[row, 46].Value = (target.Dec_AmountIDR    > 0) ? target.Dec_AmountIDR     /1000000 : 0;
                worksheet.Cells[row, 47].Value = (target.Dec_OptimistIDR  > 0) ? target.Dec_OptimistIDR   /1000000 : 0;
                worksheet.Cells[row, 48].Value = (target.Dec_MostLikelyIDR> 0) ? target.Dec_MostLikelyIDR /1000000 : 0;
                worksheet.Cells[row, 49].Value = (target.Dec_PessimistIDR > 0) ? target.Dec_PessimistIDR  /1000000 : 0;
                row = row + 1;
            }
            worksheet.Cells[row, 1].Value = "Total";
            worksheet.Cells[row, 2].Value =  inputTarget.Sum(m => m.Jan_AmountIDR.GetValueOrDefault(0)) / 1000000;
            worksheet.Cells[row, 3].Value =  inputTarget.Sum(m => m.Jan_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 4].Value =  inputTarget.Sum(m => m.Jan_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 5].Value =  inputTarget.Sum(m => m.Jan_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 6].Value =  inputTarget.Sum(m => m.Feb_AmountIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 7].Value =  inputTarget.Sum(m => m.Feb_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 8].Value =  inputTarget.Sum(m => m.Feb_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 9].Value =  inputTarget.Sum(m => m.Feb_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 10].Value = inputTarget.Sum(m => m.Mar_AmountIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 11].Value = inputTarget.Sum(m => m.Mar_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 12].Value = inputTarget.Sum(m => m.Mar_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 13].Value = inputTarget.Sum(m => m.Mar_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 14].Value = inputTarget.Sum(m => m.Apr_AmountIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 15].Value = inputTarget.Sum(m => m.Apr_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 16].Value = inputTarget.Sum(m => m.Apr_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 17].Value = inputTarget.Sum(m => m.Apr_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 18].Value = inputTarget.Sum(m => m.May_AmountIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 19].Value = inputTarget.Sum(m => m.May_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 20].Value = inputTarget.Sum(m => m.May_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 21].Value = inputTarget.Sum(m => m.May_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 22].Value = inputTarget.Sum(m => m.Jun_AmountIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 23].Value = inputTarget.Sum(m => m.Jun_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 24].Value = inputTarget.Sum(m => m.Jun_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 25].Value = inputTarget.Sum(m => m.Jun_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 26].Value = inputTarget.Sum(m => m.Jul_AmountIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 27].Value = inputTarget.Sum(m => m.Jul_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 28].Value = inputTarget.Sum(m => m.Jul_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 29].Value = inputTarget.Sum(m => m.Jul_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 30].Value = inputTarget.Sum(m => m.Aug_AmountIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 31].Value = inputTarget.Sum(m => m.Aug_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 32].Value = inputTarget.Sum(m => m.Aug_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 33].Value = inputTarget.Sum(m => m.Aug_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 34].Value = inputTarget.Sum(m => m.Sep_AmountIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 35].Value = inputTarget.Sum(m => m.Sep_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 36].Value = inputTarget.Sum(m => m.Sep_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 37].Value = inputTarget.Sum(m => m.Sep_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 38].Value = inputTarget.Sum(m => m.Oct_AmountIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 39].Value = inputTarget.Sum(m => m.Oct_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 40].Value = inputTarget.Sum(m => m.Oct_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 41].Value = inputTarget.Sum(m => m.Oct_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 42].Value = inputTarget.Sum(m => m.Nov_AmountIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 43].Value = inputTarget.Sum(m => m.Nov_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 44].Value = inputTarget.Sum(m => m.Nov_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 45].Value = inputTarget.Sum(m => m.Nov_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 46].Value = inputTarget.Sum(m => m.Dec_AmountIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 47].Value = inputTarget.Sum(m => m.Dec_OptimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 48].Value = inputTarget.Sum(m => m.Dec_MostLikelyIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells[row, 49].Value = inputTarget.Sum(m => m.Dec_PessimistIDR.GetValueOrDefault(0))/ 1000000;
            worksheet.Cells["A2:AW2"].AutoFilter = true;
            worksheet.Cells["A7:AW7"].Style.Font.Bold = true;

            return package;
        }

        #endregion

        #region Monitoring CN Invoice
        [Authorize]
        [Route("MonitoringCNInvoiceList")]
        //[PageAuthorize(ActionTokenHelper.ColoPrimingView)]
        public ActionResult MonitoringCNInvoiceList()
        {
            //ViewBag.AllowCreate = _userServiceClient.CheckUserAccessNew(UserManager.User.UserToken, ActionTokenHelper.ColoPrimingCreate);
            string actionTokenView = "0907a0f1-b33f-4676-b30b-a06e08e3da37";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    //ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    //ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }
            //return View("~/Views/Dashboard/MonitoringCNInvoice/MasterPeriodeList.cshtml");
            return View();
        }

        [Route("List/ExportMonitoringCN")]
        public void ExportMonitoringCN(string CustomerID, string CompanyID, string vPKP)
        {
            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            string[] fieldList = new string[] {

                        "CustomerName",
                        "OD_13",
                        "OD_46",
                        "OD_79",
                        "OD_9s",
                        "GrandTotal"
                    };

            string strWhereClause = $"1=1";
            if (!string.IsNullOrEmpty(CustomerID))
            {
                strWhereClause += $" AND CustomerID = '{CustomerID}'";
            }
            if (!string.IsNullOrEmpty(CompanyID))
            {
                strWhereClause += $" AND CompanyID = '{CompanyID}'";
            }
            if (!string.IsNullOrEmpty(vPKP))
            {
                strWhereClause += $" AND CompanyID != 'pkp'";
            }
            string strOrderBy = "CustomerName asc";

            var dataList = new List<vwDashBoardMonitoringCNInvoice>();
            var result = _monitoringCNService.GetMonitoringCNInvoiceList(UserManager.User.UserToken, userCredential, strWhereClause, strOrderBy);
            dataList.AddRange(result);
            DataTable table = new DataTable();
            
            var reader = FastMember.ObjectReader.Create(dataList, fieldList);
            table.Load(reader);
            table.Columns[0].ColumnName = "Customer Name";
            table.Columns[1].ColumnName = "OD 1-3 days";
            table.Columns[2].ColumnName = "OD 4-6 days";
            table.Columns[3].ColumnName = "OD 7-9 days";
            table.Columns[4].ColumnName = "OD > 9 days";
            table.Columns[5].ColumnName = "Grand Total";
            ExportToExcelHelper.Export("MonitoringCNInvoiceList", table);
        }

        [Route("List/ExportMonitoringCNDetail")]
        public void ExportMonitoringCNDetail(string CustomerID, string CompanyID, string vPKP, string Range)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
            string[] fieldList = new string[] {

                        "InvNumber",
                        "NoFaktur",
                        "CompanyInvoice",
                        "CustomerID",
                        "Subject",
                        "AmountInvoice"
                    };

            string strWhereClause = $"1=1";
            if (!string.IsNullOrEmpty(CustomerID))
            {
                strWhereClause += $" AND CustomerID = '{CustomerID}'";
            }
            if (!string.IsNullOrEmpty(CompanyID))
            {
                strWhereClause += $" AND CompanyID = '{CompanyID}'";
            }
            if (!string.IsNullOrEmpty(vPKP))
            {
                strWhereClause += $" AND CompanyID != 'pkp'";
            }
            if (!string.IsNullOrEmpty(Range))
            {
                if (Range == "13") strWhereClause += $" AND OD_13 = '1'";
                if (Range == "46") strWhereClause += $" AND OD_46 = '1'";
                if (Range == "79") strWhereClause += $" AND OD_79 = '1'";
                if (Range == "9s") strWhereClause += $" AND OD_9s = '1'";
            }
            string strOrderBy = "CustomerName asc";

            var dataList = new List<vwDashBoardMonitoringCNInvoiceDetail>();
            var result = _monitoringCNService.GetMonitoringCNInvoiceListDetail(UserManager.User.UserToken, userCredential, strWhereClause, strOrderBy);
            dataList.AddRange(result);
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(dataList, fieldList);
            table.Load(reader);
            table.Columns[0].ColumnName = "Invoice Number";
            table.Columns[1].ColumnName = "No. Faktur";
            table.Columns[2].ColumnName = "Company Invoice";
            table.Columns[3].ColumnName = "Customer Invoice";
            table.Columns[4].ColumnName = "Subject";
            table.Columns[5].ColumnName = "Amount Invoice";
            ExportToExcelHelper.Export("MonitoringCNInvoiceListDetail", table);
        }
        #endregion
    }
}