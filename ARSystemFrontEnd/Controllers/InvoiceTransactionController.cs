using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using Microsoft.Reporting.WebForms;
using ClosedXML.Excel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using ARSystem.Service;
using ARSystem.Domain.Models;

using ARSystem.Service.ARSystem.Invoice;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels;
using ARSystem.Service.ARSystem;
using ARSystem.Domain.Models.HTBGDWH01.TBGARSystem;
using System.Globalization;
using ARSystem.Service.RevenueAssurance;
using System.Threading.Tasks;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("InvoiceTransaction")]
    public class InvoiceTransactionController : BaseController
    {
        private trxChecklistInvoiceTowerService _CheckListservices;
        private trxARPaymentInvoiceTowerService _ARPaymentservice;
        private ApprovalMonitoringARService _ApprovalMonitoringSevice;
        private MasterService _master;
        private readonly HistoryCNInvoiceService _historyCNService;
        private readonly vwRTIDataDoneService _vwRTIDataDoneService;

        public InvoiceTransactionController()
        {
            _CheckListservices = new trxChecklistInvoiceTowerService();
            _ARPaymentservice = new trxARPaymentInvoiceTowerService();
            _ApprovalMonitoringSevice = new ApprovalMonitoringARService();
            _master = new MasterService();
            _historyCNService = new HistoryCNInvoiceService();
            _vwRTIDataDoneService = new vwRTIDataDoneService();
        }

        #region Routes

        [Authorize]
        [Route("TrxBapsConfirm")]
        public ActionResult TrxBapsConfirm()
        {
            string actionTokenView = "b6ef5db8-3025-480f-9932-ffeef95cb013";
            string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxBapsConfirms")]
        public ActionResult TrxBapsConfirm(string renderType = null, string mOperator = null)
        {
            string actionTokenView = "b6ef5db8-3025-480f-9932-ffeef95cb013";
            string actionTokenProcess = "b01dea72-9b52-46d9-86d4-9c1db21e2e8b";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    ViewBag.RenderType = !string.IsNullOrEmpty(renderType) ? renderType : "";
                    ViewBag.Operator = !string.IsNullOrEmpty(mOperator) ? mOperator : "";
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("TrxCreateInvoiceTower")]
        public ActionResult TrxCreateInvoiceTower()
        {
            string actionTokenView = "2fa0107f-ed90-4059-a920-48dafd0b58bc";
            string actionTokenProcess = "f9f9711a-5a54-4b54-94a7-80fea609432a";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    using (var clientPPH = new ARSystemService.ImstDataSourceServiceClient())
                    {
                        ViewBag.PPHValue = clientPPH.GetPPHPercentage(UserManager.User.UserToken).PPHValue;
                        ViewBag.PPFValue = clientPPH.GetPPFPercentage(UserManager.User.UserToken).PPFValue;
                        ViewBag.PPNValue = _master.GetPPNPercentage(userCredential.UserID).PPNValue;
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
        [Route("TrxPostingInvoiceTower")]
        public ActionResult TrxPostingInvoiceTower()
        {
            string actionTokenView = "9c94d3b1-bd13-4e33-9b91-49a695d0bcff";
            string actionTokenProcess = "c134b02d-bd8a-4605-bbb4-ae6bcca0af5f";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxCreateInvoiceOtherProduct")]
        public ActionResult TrxCreateInvoiceOtherProduct()
        {
            string actionTokenView = "8fb60acb-cf05-4f95-9d10-9bf52f0280e8";
            string actionTokenProcess = "82597fb3-1fda-4d6e-9ac2-f3a313c89b15";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxCreateInvoiceBuilding")]
        public ActionResult TrxCreateInvoiceBuilding()
        {
            string actionTokenView = "34430cad-49bd-4730-928a-f4e763db0b52";
            string actionTokenProcess = "1867fb27-ed61-4f6b-8f04-032f560d8ee5";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxCreateInvoiceNonBuilding")]
        public ActionResult TrxCreateInvoiceNonBuilding()
        {
            string actionTokenView = "ED57757F-A62F-4B7B-8044-105FE14A46DD";
            string actionTokenProcess = "A58BA157-6FE6-436C-8D0A-7C72DAE32CCD";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxPostingInvoiceBuilding")]
        public ActionResult TrxPostingInvoiceBuilding()
        {
            string actionTokenView = "e2baf02c-20c0-4b9c-86c3-059fef452b73";
            // string actionTokenProcess = "fc1fe066-f108-4bec-b2e4-458021e9cb68";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    // ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("TrxPostingInvoiceOtherProduct")]
        public ActionResult TrxPostingInvoiceOtherProduct()
        {
            string actionTokenView = "bf62992f-a471-4af8-afdb-cdd93f50cb47";
            string actionTokenProcess = "68b4b203-03a9-4f41-9557-6bac5115f957";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxCreateInvoiceTowerRemainingAmount")]
        public ActionResult TrxCreateInvoiceTowerRemainingAmount()
        {
            string actionTokenView = "4bc4ccf7-a008-4c8b-ab7b-7552d12aafdd";
            string actionTokenProcess = "5eddc9c6-d980-4841-8c32-34e49faa7168";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    using (var clientPPH = new ARSystemService.ImstDataSourceServiceClient())
                    {
                        ViewBag.PPHValue = clientPPH.GetPPHPercentage(UserManager.User.UserToken).PPHValue;
                        ViewBag.PPFValue = clientPPH.GetPPFPercentage(UserManager.User.UserToken).PPFValue;
                        ViewBag.PPNValue = _master.GetPPNPercentage(userCredential.UserID).PPNValue;
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
        [Route("TrxPrintInvoiceTower")]
        public ActionResult TrxPrintInvoiceTower()
        {
            string actionTokenView = "78d3f80c-e0cb-441f-ab2f-ffdf9a1db978";
            string actionTokenProcess = "e475f8fe-64b5-4b0c-897a-1595701c5b17";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxPrintInvoiceBuilding")]
        public ActionResult TrxPrintInvoiceBuilding()
        {
            string actionTokenView = "d14f2260-30a6-4542-9109-97d9aad33906";
            string actionTokenProcess = "6a2cae1b-6945-4760-a86f-259d77d99617";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxChecklistInvoiceTower")]
        public ActionResult TrxChecklistInvoiceTower()
        {
            string actionTokenView = "a75473fd-e114-4458-94a4-c2ff65525dfb";
            string actionTokenProcess = "049af01f-9429-402f-a26a-bb8ca23c4668";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);

                    using (var userClient = new ARSystemService.UserServiceClient())
                    {
                        ViewBag.Role = userClient.GetARUserPosition(UserManager.User.UserToken).Result;
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
        [Route("TrxChecklistInvoiceBuilding")]
        public ActionResult TrxChecklistInvoiceBuilding()
        {
            string actionTokenView = "8217aec6-9fc1-4890-97e7-94266478d3bc";
            string actionTokenProcess = "8217aec6-9fc1-4890-97e7-94266478d3bc";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);

                    using (var userClient = new ARSystemService.UserServiceClient())
                    {
                        ViewBag.Role = userClient.GetARUserPosition(UserManager.User.UserToken).Result;
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
        [Route("TrxCNInvoiceBuilding")]
        public ActionResult TrxCNInvoiceBuilding()
        {
            string actionTokenView = "7b2827da-1333-4a98-9e25-279a39bcc413";
            string actionTokenProcess = "83687efa-e338-4e1d-8bf9-c4b2970d917c";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);

                    using (var userClient = new ARSystemService.UserServiceClient())
                    {
                        ViewBag.Role = userClient.GetARUserPosition(UserManager.User.UserToken).Result;
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
        [Route("TrxCNInvoiceTower")]
        public ActionResult TrxCNInvoiceTower()
        {
            string actionTokenView = "3ed75352-7bcc-4c9f-a8b4-35a90695a35d";
            string actionTokenProcess = "ff4adb00-c6dc-41f2-abb1-195a010e4530";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);

                    using (var userClient = new ARSystemService.UserServiceClient())
                    {
                        ViewBag.Role = userClient.GetARUserPosition(UserManager.User.UserToken).Result;
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
        [Route("TrxManageMergedInvoiceOnlyTower")]
        public ActionResult TrxManageMergedInvoiceOnlyTower()
        {
            string actionTokenView = "93020eed-2c51-491e-ad80-0867b5cf004d";
            string actionTokenProcess = "370e1788-3b72-43af-9248-86efc972c569";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxManageMergedInvoiceOnlyBuilding")]
        public ActionResult TrxManageMergedInvoiceOnlyBuilding()
        {
            string actionTokenView = "d4867327-0edb-46dc-a4c0-b3aca6259017";
            string actionTokenProcess = "6071fba9-dcec-42c2-b3e5-289c44eecd85";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxManageMergedInvoiceDetailTower")]
        public ActionResult TrxManageMergedInvoiceDetailTower()
        {
            string actionTokenView = "36bb5465-ddb4-4962-be91-1c9fb6902d7a";
            string actionTokenProcess = "ec04820e-fc62-4e43-ba2a-89e66a57c455";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxManageMergedInvoiceDetailBuilding")]
        public ActionResult TrxManageMergedInvoiceDetailBuilding()
        {
            string actionTokenView = "6775dd41-6fbc-4d05-8034-bcd74e43f429";
            string actionTokenProcess = "7ae087ac-88fb-4017-b33f-b1f0824337fc";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxReportInvoiceTower")]
        public ActionResult TrxReportInvoiceTower()
        {
            string actionTokenView = "6625c854-103e-431b-9eb3-a22e1c4aeaaf";
            string actionTokenProcess = "c5ba37d1-8db7-4395-a3e1-95de007a0b93";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxReportInvoiceBuilding")]
        public ActionResult TrxReportInvoiceBuilding()
        {
            string actionTokenView = "046d5f90-3634-4f0e-bdf6-98d56846a5b2";
            string actionTokenProcess = "d14180d5-9439-4d81-855e-81f7c2700995";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxCollectionReportInvoice")]
        public ActionResult TrxCollectionReportInvoice()
        {
            string actionTokenView = "00dd6d67-3f06-47ee-ad2b-cff30f0d0c4e";
            string actionTokenProcess = "cc306e81-e213-4558-9605-d752d73c9382";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    //string ReportServer = System.Configuration.ConfigurationManager.AppSettings["ReportServer"];
                    //string ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"];
                    //string UserName = System.Configuration.ConfigurationManager.AppSettings["ReportUserName"];
                    //string Password = System.Configuration.ConfigurationManager.AppSettings["ReportPassword"];
                    //string Domain = System.Configuration.ConfigurationManager.AppSettings["Domain"];

                    //ReportViewer reportViewer = new ReportViewer();
                    //reportViewer.ProcessingMode = ProcessingMode.Remote;
                    //reportViewer.SizeToReportContent = true;
                    //reportViewer.Width = System.Web.UI.WebControls.Unit.Percentage(900);
                    //reportViewer.Height = System.Web.UI.WebControls.Unit.Percentage(900);
                    //reportViewer.CssClass = "reports";
                    //reportViewer.ServerReport.ReportServerUrl = new Uri(ReportServer);
                    //reportViewer.ServerReport.ReportPath = ReportPath;
                    //reportViewer.ServerReport.ReportServerCredentials = new CustomReportCredentials(UserName, Password, Domain);
                    //ViewBag.ReportViewer = reportViewer;
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        //[Authorize]
        //[Route("TrxCollectionReportInvoiceTower")]
        //public ActionResult TrxCollectionReportInvoiceTower()
        //{
        //    string actionTokenView = "00dd6d67-3f06-47ee-ad2b-cff30f0d0c4e";
        //    string actionTokenProcess = "cc306e81-e213-4558-9605-d752d73c9382";

        //    using (var client = new SecureAccessService.UserServiceClient())
        //    {
        //        if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
        //        {
        //            ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Logout", "Login");
        //        }
        //    }

        //    return View();
        //}

        [Authorize]
        [Route("TrxARProcessInvoiceTower")]
        public ActionResult TrxARProcessInvoiceTower()
        {
            string actionTokenView = "46a48c0d-d5a3-4e22-a6b2-46e236da2e7f";
            string actionTokenProcess = "31f45aec-5a86-4089-b2db-5626a19377f1";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    ViewBag.DocPath = Helper.Helper.GetDocPath();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("TrxARProcessInvoiceBuilding")]
        public ActionResult TrxARProcessInvoiceBuilding()
        {
            string actionTokenView = "dcadbff1-91aa-498c-9faf-7c3361d4b155";
            string actionTokenProcess = "538b0b24-ac60-46a4-9759-8e5418f5a2dc";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    ViewBag.DocPath = Helper.Helper.GetDocPath();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("TrxARPaymentInvoiceTower")]
        public ActionResult TrxARPaymentInvoiceTower()
        {
            string actionTokenView = "1951cebe-f009-4261-9fe7-628f19e1104a";
            string actionTokenProcess = "d727ba68-ded5-4878-abca-60b02e9eb422";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    using (var clientPPH = new ARSystemService.ImstDataSourceServiceClient())
                    {
                        ViewBag.PPHValue = clientPPH.GetPPHPercentage(UserManager.User.UserToken).PPHValue;
                        ViewBag.PPFValue = clientPPH.GetPPFPercentage(UserManager.User.UserToken).PPFValue;
                        ViewBag.DocPath = Helper.Helper.GetDocPath();
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
        [Route("TrxARPaymentInvoiceBuilding")]
        public ActionResult TrxARPaymentInvoiceBuilding()
        {
            string actionTokenView = "5ce69f43-bf97-48a1-9d3b-f786c76c59bd";
            string actionTokenProcess = "be810687-04f5-47fc-9888-4474bda60f28";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    using (var clientPPH = new ARSystemService.ImstDataSourceServiceClient())
                    {
                        ViewBag.PPHValue = clientPPH.GetPPHPercentage(UserManager.User.UserToken).PPHValue;
                        ViewBag.PPFValue = clientPPH.GetPPFPercentage(UserManager.User.UserToken).PPFValue;
                        ViewBag.DocPath = Helper.Helper.GetDocPath();
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
        [Route("TrxApprovalARMonitoringTower")]
        public ActionResult TrxApprovalARMonitoringTower()
        {
            string actionTokenView = "c3e4f1cf-44ad-43fc-b658-4c0d33fb5b41";
            string actionTokenProcess = "2ea01744-6545-45a0-8fde-5f73a6b29fa9";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxApprovalARMonitoringBuilding")]
        public ActionResult TrxApprovalARMonitoringBuilding()
        {
            string actionTokenView = "851e09ff-18d0-4089-adfb-d3e5b97ec9ff";
            string actionTokenProcess = "705f9fb7-3e16-4830-88c5-c1962488da1e";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxBapsBigData")]
        public ActionResult TrxBapsBigData()
        {
            string actionTokenView = "d77eeb7c-78e3-427f-95f5-0f28acd09e01";
            string actionTokenProcess = "a3e5e19b-e9aa-4ef2-8a4b-2a2c9089eb00";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxCreateLargeInvoiceTower")]
        public ActionResult TrxCreateLargeInvoiceTower()
        {
            string actionTokenView = "388b6ee6-a130-41db-8d33-424d0da08fee";
            string actionTokenProcess = "d5cf9ed0-3bb1-46b7-a9b7-48757f2d4bd7";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    using (var clientPPH = new ARSystemService.ImstDataSourceServiceClient())
                    {
                        ViewBag.PPHValue = clientPPH.GetPPHPercentage(UserManager.User.UserToken).PPHValue;
                        ViewBag.PPFValue = clientPPH.GetPPFPercentage(UserManager.User.UserToken).PPFValue;
                        ViewBag.PPNValue = _master.GetPPNPercentage(userCredential.UserID).PPNValue;
                    }
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View("TrxCreateSuperInvoiceTower");
        }

        [Authorize]
        [Route("TrxARProcessInvoiceTowerHistory")]
        public ActionResult TrxARProcessInvoiceTowerHistory()
        {
            string actionTokenView = "5b8a9807-4e31-4ac8-9f7e-60b8f79d554f";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.DocPath = Helper.Helper.GetDocPath();
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("TrxARProcessInvoiceBuildingHistory")]
        public ActionResult TrxARProcessInvoiceBuildingHistory()
        {
            string actionTokenView = "baf1b24a-3136-4063-bdbb-3958f9dad032";

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

        [Authorize]
        [Route("TrxApprovalCNInvoiceBuilding")]
        public ActionResult TrxApprovalCNInvoiceBuilding()
        {
            string actionTokenView = "f2a62faf-cc6e-4e24-b9a0-76fa7c9b5949";
            string actionTokenProcess = "d4b47d04-c7f3-4fe2-a76f-4a52e73ce3fe";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);

                    using (var userClient = new ARSystemService.UserServiceClient())
                    {
                        ViewBag.Role = userClient.GetARUserPosition(UserManager.User.UserToken).Result;
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
        [Route("TrxApprovalCNInvoiceTower")]
        public ActionResult TrxApprovalCNInvoiceTower()
        {
            string actionTokenView = "0ad18458-0661-4e38-8fc1-1d8d5b0f0c00";
            string actionTokenProcess = "338ed179-a911-4afe-9d9b-62c4b3a6a637";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);

                    using (var userClient = new ARSystemService.UserServiceClient())
                    {
                        ViewBag.Role = userClient.GetARUserPosition(UserManager.User.UserToken).Result;
                    }
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        //ReCommit
        [Authorize]
        [Route("TrxReportInvoiceCompile")]
        public ActionResult TrxReportInvoiceCompile()
        {
            string actionTokenView = "b5145570-ac66-43fa-b63e-05ef8b5f9573";

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

        [Authorize]
        [Route("TrxReserveInvoiceNumber")]
        public ActionResult TrxReserveInvoiceNumber()
        {
            string actionTokenView = "89328729-92b0-401b-b2f6-d754db3dbdfc";
            string actionTokenProcess = "62e1a773-4257-4346-a5b6-0d49d884cf24";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
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
        [Route("TrxHistoryCNInvoiceARCollection")]
        public ActionResult TrxHistoryCNInvoiceARCollection()
        {
            string actionTokenView = "e8ca7f35-3ea3-4d84-a7c2-02c89469a98e";

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

        [Authorize]
        [Route("TrxHistoryCNInvoiceARData")]
        public ActionResult TrxHistoryCNInvoiceARData()
        {
            string actionTokenView = "8b7e9180-b831-471d-8453-26f2739b63eb";

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
        [Authorize]
        [Route("ChangePPHFinal")]
        public ActionResult ChangePPHFinal()
        {
            string actionTokenView = "3054c376-2075-42d6-9024-4bb5b2396765";
            string actionTokenEdit = "1e34a978-a0a0-44ed-a0d5-885d971f2432";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowEdit = client.CheckUserAccess(UserManager.User.UserToken, actionTokenEdit);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        #region Invoice Manual
        [Authorize]
        [Route("TrxUploadInvoiceManual")]
        public ActionResult TrxUploadInvoiceManual()
        {
            string actionTokenView = "97380846-4b42-4318-afed-59707d39626a";
            string actionTokenAdd = "5743e4e7-4788-4a8f-b502-86bfd2bb1c13";
            string actionTokenDelete = "051593a5-0b3a-4dc1-9f1b-c9abae5cfa01";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowAdd = client.CheckUserAccess(UserManager.User.UserToken, actionTokenAdd);
                    ViewBag.AllowDelete = client.CheckUserAccess(UserManager.User.UserToken, actionTokenDelete);

                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Authorize]
        [Route("TrxBapsInvoiceManualConfirm")]
        public ActionResult TrxBapsInvoiceManualConfirm()
        {
            string actionTokenView = "a710eb2e-34cd-4647-861f-3ffd99c5067c";
            string actionTokenProcess = "bee896a5-9786-4d45-9832-583a16925791";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Route("TrxUploadInvoiceManual/ImportExcel")]
        public ActionResult ImportInvoiceManual()
        {
            try
            {
                HttpPostedFileBase files = Request.Files[0]; //Read the first Posted Excel File  

                ISheet sheet; //Create the ISheet object to read the sheet cell values  
                string filename = Path.GetFileName(Server.MapPath(files.FileName)); //get the uploaded file name  
                var fileExt = Path.GetExtension(filename); //get the extension of uploaded excel file  
                if (fileExt == ".xls")
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(files.InputStream); //HSSWorkBook object will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(files.InputStream); //XSSFWorkBook will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook   
                }
                List<ARSystemService.trxInvoiceManualTemp> data = new List<ARSystemService.trxInvoiceManualTemp>();

                List<ARSystemService.mstValidasiInvoiceManual> validateMandatory = new List<ARSystemService.mstValidasiInvoiceManual>();

                using (var client2 = new ARSystemService.ItrxInvoiceManualTempServiceClient())
                {
                    validateMandatory = client2.GetValidateList(UserManager.User.UserToken, "", 0, 0).ToList();
                }

                List<string> newMandatory = new List<string>();
                foreach (ARSystemService.mstValidasiInvoiceManual item in validateMandatory)
                {
                    string result = item.FieldName.ToString().ToLower().Trim();
                    newMandatory.Add(result);
                }
                ARSystemService.trxInvoiceManualTemp temp;
                string sheetName = string.Empty;
                string soNumber = string.Empty;
                bool rowNull = true;
                int startIndex = 1;
                IRow row;
                ICell cell;
                string validateMandatorySONumber = "";
                List<string> hold = new List<string>();
                for (int i = startIndex; i <= sheet.LastRowNum; i++) //Loop the records upto filled row, starts from content row (row 3)
                {
                    if (sheet.GetRow(i) != null) //null is when the row only contains empty cells   
                    {
                        temp = new ARSystemService.trxInvoiceManualTemp();
                        row = sheet.GetRow(i);
                        if (row != null)
                        {
                            cell = row.GetCell(0);
                            if (cell != null && cell.ToString() != "")
                                temp.SONumber = sheet.GetRow(i).GetCell(0).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("sonumber"))
                                {
                                    if (hold.Contains("sonumber"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("sonumber");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " SONumber";
                                        else
                                            validateMandatorySONumber += ", SONumber";
                                    }
                                }
                            }

                            cell = row.GetCell(1);
                            if (cell != null && cell.ToString() != "")
                                temp.SiteIDOpr = sheet.GetRow(i).GetCell(1).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("siteidopr"))
                                {
                                    if (hold.Contains("siteidopr"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("siteidopr");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " SiteIDOpr";
                                        else
                                            validateMandatorySONumber += ", SiteIDOpr";
                                    }
                                }
                            }

                            cell = row.GetCell(2);
                            if (cell != null && cell.ToString() != "")
                                temp.SiteNameOpr = sheet.GetRow(i).GetCell(2).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("sitenameopr"))
                                {
                                    if (hold.Contains("sitenameopr"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("sitenameopr");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " SiteNameOpr";
                                        else
                                            validateMandatorySONumber += ", SiteNameOpr";
                                    }
                                }
                            }

                            cell = row.GetCell(3);
                            if (cell != null && cell.ToString() != "")
                                temp.InitialPONumber = sheet.GetRow(i).GetCell(3).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("initialponumber"))
                                {
                                    if (hold.Contains("initialponumber"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("initialponumber");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " PONumber";
                                        else
                                            validateMandatorySONumber += ", PONumber";
                                    }
                                }
                            }
                            cell = row.GetCell(4);
                            if (cell != null && cell.ToString() != "")
                                temp.InvoiceStartDate = DateTime.Parse(sheet.GetRow(i).GetCell(4).ToString().Trim());
                            else
                            {
                                if (newMandatory.Contains("invoicestartdate"))
                                {
                                    if (hold.Contains("invoicestartdate"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("invoicestartdate");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " InvoiceStartDate";
                                        else
                                            validateMandatorySONumber += ", InvoiceStartDate";
                                    }
                                }
                            }
                            cell = row.GetCell(5);
                            if (cell != null && cell.ToString() != "")
                                temp.InvoiceEndDate = DateTime.Parse(sheet.GetRow(i).GetCell(5).ToString().Trim());
                            else
                            {
                                if (newMandatory.Contains("invoiceenddate"))
                                {
                                    if (hold.Contains("invoiceenddate"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("invoiceenddate");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " InvoiceEndDate";
                                        else
                                            validateMandatorySONumber += ", InvoiceEndDate";
                                    }
                                }
                            }
                            cell = row.GetCell(6);
                            if (cell != null && cell.ToString() != "")
                                temp.BaseLeasePrice = Convert.ToDecimal(sheet.GetRow(i).GetCell(6).ToString().Trim());
                            else
                            {
                                if (newMandatory.Contains("baseleaseprice"))
                                {
                                    if (hold.Contains("baseleaseprice"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("baseleaseprice");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " BaseLeasePrice";
                                        else
                                            validateMandatorySONumber += ", BaseLeasePrice";
                                    }
                                }
                            }
                            cell = row.GetCell(7);
                            if (cell != null && cell.ToString() != "")
                                temp.OMPrice = Convert.ToDecimal(sheet.GetRow(i).GetCell(7).ToString().Trim());
                            else
                            {
                                if (newMandatory.Contains("omprice"))
                                {
                                    if (hold.Contains("omprice"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("omprice");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " OMPrice";
                                        else
                                            validateMandatorySONumber += ", OMPrice";
                                    }
                                }
                            }
                            cell = row.GetCell(8);
                            if (cell != null && cell.ToString() != "")
                                temp.InvoiceAmount = Convert.ToDecimal(sheet.GetRow(i).GetCell(8).ToString().Trim());
                            else
                            {
                                if (newMandatory.Contains("invoiceamount"))
                                {
                                    if (hold.Contains("invoiceamount"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("invoiceamount");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " InvoiceAmount";
                                        else
                                            validateMandatorySONumber += ", InvoiceAmount";
                                    }
                                }
                            }
                            cell = row.GetCell(9);
                            if (cell != null && cell.ToString() != "")
                                temp.StipSiro = Convert.ToInt32(sheet.GetRow(i).GetCell(9).ToString().Trim());
                            else
                            {
                                if (newMandatory.Contains("stipsiro"))
                                {
                                    if (hold.Contains("stipsiro"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("stipsiro");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " StipSiro";
                                        else
                                            validateMandatorySONumber += ", StipSiro";
                                    }
                                }
                            }
                            cell = row.GetCell(10);
                            if (cell != null && cell.ToString() != "")
                                temp.CompanyID = sheet.GetRow(i).GetCell(10).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("companyid"))
                                {
                                    if (hold.Contains("companyid"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("companyid");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " CompanyID";
                                        else
                                            validateMandatorySONumber += ", CompanyID";
                                    }
                                }
                            }
                            cell = row.GetCell(11);
                            if (cell != null && cell.ToString() != "")
                                temp.PriceCurrency = sheet.GetRow(i).GetCell(11).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("pricecurrency"))
                                {
                                    if (hold.Contains("pricecurrency"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("pricecurrency");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " PriceCurrency";
                                        else
                                            validateMandatorySONumber += ", PriceCurrency";
                                    }
                                }
                            }
                            cell = row.GetCell(12);
                            if (cell != null && cell.ToString() != "")
                                temp.LossPNN = sheet.GetRow(i).GetCell(12).ToString().Trim() == "Loss" ? true : false;
                            else
                            {
                                if (newMandatory.Contains("islossppn"))
                                {
                                    if (hold.Contains("islossppn"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("islossppn");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " LossPPN";
                                        else
                                            validateMandatorySONumber += ", LossPPN";
                                    }
                                }
                            }
                            cell = row.GetCell(13);
                            if (cell != null && cell.ToString() != "")
                                temp.BapsNO = sheet.GetRow(i).GetCell(13).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("bapsno"))
                                {
                                    if (hold.Contains("bapsno"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("bapsno");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " BapsNO";
                                        else
                                            validateMandatorySONumber += ", BapsNO";
                                    }
                                }
                            }
                            cell = row.GetCell(14);
                            if (cell != null && cell.ToString() != "")
                                temp.BapsType = sheet.GetRow(i).GetCell(14).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("bapstype"))
                                {
                                    if (hold.Contains("bapstype"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("bapstype");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " BapsType";
                                        else
                                            validateMandatorySONumber += ", BapsType";
                                    }
                                }
                            }
                            cell = row.GetCell(15);
                            if (cell != null && cell.ToString() != "")
                                temp.BapsPeriod = sheet.GetRow(i).GetCell(15).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("bapsperiod"))
                                {
                                    if (hold.Contains("bapsperiod"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("bapsperiod");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " BapsPeriod";
                                        else
                                            validateMandatorySONumber += ", BapsPeriod";
                                    }
                                }
                            }
                            cell = row.GetCell(16);
                            if (cell != null && cell.ToString() != "")
                                temp.OperatorName = sheet.GetRow(i).GetCell(16).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("operatorname"))
                                {
                                    if (hold.Contains("operatorname"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("operatorname");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " OperatorName";
                                        else
                                            validateMandatorySONumber += ", OperatorName";
                                    }
                                }
                            }
                            cell = row.GetCell(17);
                            if (cell != null && cell.ToString() != "")
                                temp.StartLeaseDate = DateTime.Parse(sheet.GetRow(i).GetCell(17).ToString().Trim());
                            else
                            {
                                if (newMandatory.Contains("startleasedate"))
                                {
                                    if (hold.Contains("startleasedate"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("startleasedate");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " StartLeaseDate";
                                        else
                                            validateMandatorySONumber += ", StartLeaseDate";
                                    }
                                }
                            }
                            cell = row.GetCell(18);
                            if (cell != null && cell.ToString() != "")
                                temp.EndLeaseDate = DateTime.Parse(sheet.GetRow(i).GetCell(18).ToString().Trim());
                            else
                            {
                                if (newMandatory.Contains("endleasedate"))
                                {
                                    if (hold.Contains("endleasedate"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("endleasedate");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " EndLeaseDate";
                                        else
                                            validateMandatorySONumber += ", EndLeaseDate";
                                    }
                                }
                            }
                            cell = row.GetCell(19);
                            if (cell != null && cell.ToString() != "")
                                temp.InvoiceTerm = sheet.GetRow(i).GetCell(19).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("invoiceterm"))
                                {
                                    if (hold.Contains("invoiceterm"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("invoiceterm");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " InvoiceTerm";
                                        else
                                            validateMandatorySONumber += ", InvoiceTerm";
                                    }
                                }
                            }
                            cell = row.GetCell(20);
                            if (cell != null && cell.ToString() != "")
                                temp.MLANumber = sheet.GetRow(i).GetCell(20).ToString().Trim();
                            else
                            {
                                if (newMandatory.Contains("mlanumber"))
                                {
                                    if (hold.Contains("mlanumber"))
                                    {
                                    }
                                    else
                                    {
                                        hold.Add("mlanumber");
                                        if (validateMandatorySONumber == "")
                                            validateMandatorySONumber += " MLANumber";
                                        else
                                            validateMandatorySONumber += ", MLANumber";
                                    }
                                }
                            }

                            //if (!string.IsNullOrEmpty(temp.InvNo) && !string.IsNullOrEmpty(temp.TaxInvoiceNo)) // && !string.IsNullOrEmpty(temp.strTaxInvoiceDate)
                            data.Add(temp);
                            rowNull = false;
                        }
                        else
                        {
                            rowNull = true;
                        }
                    }
                }

                List<ARSystemService.trxInvoiceManualTemp> validatedData;
                using (var client = new ARSystemService.ItrxInvoiceManualTempServiceClient())
                {
                    if (UserManager.User.CompanyCode == "PKP")
                    {
                        data = data.Where(w => w.CompanyID == UserManager.User.CompanyCode.Trim()).ToList();
                    }
                    validatedData = client.CreateBulkyInvoiceManual(UserManager.User.UserToken, data.ToArray(), validateMandatorySONumber, rowNull).ToList();
                }

                return Json(validatedData);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        #endregion
        #region REPORT ARCBS
        [Authorize]
        [Route("TrxReportARCBS")]
        public ActionResult TrxReportARCBS()
        {
            string actionTokenView = "0b90a49d-737a-443a-8e24-e1482977faa5";
            string actionTokenApprove = "b8dbf784-e523-4d09-b373-243e70b1bfb1";
            string actionTokenConfirm = "b5f4a992-6df8-43ee-9ebb-6ff36aa7eea8";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowApprove = client.CheckUserAccess(UserManager.User.UserToken, actionTokenApprove);
                    ViewBag.AllowConfirm = client.CheckUserAccess(UserManager.User.UserToken, actionTokenConfirm);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        #endregion
        #region Invoice Non Revenue
        [Authorize]
        [Route("TrxCreateInvoiceNonRevenue")]
        public ActionResult TrxCreateInvoiceNonRevenue()
        {
            string actionTokenView = "745671E8-2570-4633-B3B8-C214AE86791D";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                    using (var clientPPH = new ARSystemService.ImstDataSourceServiceClient())
                    {
                        ViewBag.PPHValue = clientPPH.GetPPHPercentage(UserManager.User.UserToken).PPHValue;
                        ViewBag.PPFValue = clientPPH.GetPPFPercentage(UserManager.User.UserToken).PPFValue;
                        ViewBag.PPNValue = _master.GetPPNPercentage(userCredential.UserID).PPNValue;
                    }
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }
        #endregion
        #region New menu approval monitoring AR
        [Authorize]
        [Route("ApprovalMonitoringAR")]
        public ActionResult ApprovalMonitoringAR()
        {
            string actionTokenView = "A0FA6A88-8472-4BEA-B608-5E2B5ADD3841";

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
        #endregion


        [Authorize]
        [Route("HandoverDocRTI")]
        public ActionResult HandoverDocumentRTI()
        {
            string actionTokenView = "498255be-ea42-4a7e-9e5e-c882b0d4ffa5";

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

        [Authorize]
        [Route("HandoverDocsRTI")]
        public ActionResult HandoverDocumentRTI(string renderType = null, string mOperator = null, string date = "")
        {
            string actionTokenView = "498255be-ea42-4a7e-9e5e-c882b0d4ffa5";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.UserCompanyCode = UserManager.User.CompanyCode;//Add User Company Login
                    ViewBag.RenderType = !string.IsNullOrEmpty(renderType) ? renderType : "";
                    ViewBag.Operator = !string.IsNullOrEmpty(mOperator) ? mOperator : "";
                    ViewBag.Date = !string.IsNullOrEmpty(date) ? date : "";
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
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
            string strStartDate = Request.QueryString["strStartDate"].ToString();
            string strEndDate = Request.QueryString["strEndDate"].ToString();
            string strOrderBy = "";
            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(strStartDate) && !string.IsNullOrEmpty(strEndDate))
            {
                startDate = DateTime.ParseExact(strStartDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                endDate = DateTime.ParseExact(strEndDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            }

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
            List<vwRTIDataDone> listRTIData = new List<vwRTIDataDone>();
            int intTotalRecord = 0;

            listRTIData = _vwRTIDataDoneService.GetDataDone(UserManager.User.UserToken, CompanyID, CustomerID, strBAPSNumber.ToList(), strPONumber.ToList(), strSONumber.ToList(), Year, Quartal, BapsType, PowerType, strOrderBy, 0, 0, startDate, endDate).ToList();
            //intTotalRecord = _vwRTIDataDoneService.GetCountDone(UserManager.User.UserToken, CompanyID, CustomerID, strBAPSNumber.ToList(), strPONumber.ToList(), strSONumber.ToList(), Year, Quartal, BapsType, PowerType, startDate, endDate);
            intTotalRecord = listRTIData.Count();
            //listRTIData.AddRange(listRTIData);

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
                , "BAPSConfirmDate"
                , "CreateInvoiceDate"
                , "PostingInvoiceDate"
                , "BaseLeasePrice"
                , "ServicePrice"
                , "DeductionAmount"
                , "InflationAmount"
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
                i.BAPSConfirmDate
                ,
                i.CreateInvoiceDate
                ,
                i.PostingInvoiceDate
                ,
                i.BaseLeasePrice
                ,
                i.ServicePrice
                ,
                i.DeductionAmount
                ,
                i.InflationAmount
                ,
                i.TotalPaymentRupiah
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("trxBAPSInput", table);
        }

        [Route("Input/ExportBapsDone")]
        public void ExportToExcelBapsDone()
        {
            var param = new vwRABapsDone();
            var service = new BapsDoneService();
            var dataList = new List<vwRABapsDone>();
            int intTotalRecord = 0;

            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"].ToString();
            string strCustomerID = Request.QueryString["strCustomerID"].ToString();
            string strProductId = Request.QueryString["strProductId"].ToString();
            string strBAPSType = Request.QueryString["strTowerType"].ToString();
            string mstRAActivityID = Request.QueryString["mstRAActivityID"].ToString();
            string strStartDate = Request.QueryString["strStartDate"].ToString();
            string strEndDate = Request.QueryString["strEndDate"].ToString();

            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(strStartDate) && !string.IsNullOrEmpty(strEndDate))
            {
                startDate = DateTime.ParseExact(strStartDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                endDate = DateTime.ParseExact(strEndDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            }

            param.CustomerID = strCustomerID;
            //NEWBAPS_DONE
            param.ActivityID = Convert.ToInt32(mstRAActivityID);
            param.ProductID = int.Parse(string.IsNullOrEmpty(strProductId) ? "0" : strProductId);
            param.CompanyID = strCompanyId;
            param.TowerTypeID = string.IsNullOrEmpty(strBAPSType) ? "TOWER" : strBAPSType;
            param.StartDate = startDate;
            param.EndDate = endDate;

            dataList = service.BapsDoneTreeViewList(param, 0, 0, TableViewTypes.GRID).Result;
            intTotalRecord = dataList.Count();

            DataTable table = new DataTable();

            string[] fieldList = new string[] {
                        "DoneDate",
                        "BAPSConfirmDate",
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

            var reader = FastMember.ObjectReader.Create(dataList.Select(i => new
            {
                i.DoneDate
              ,
                i.BAPSConfirmDate,
                i.SoNumber
              ,
                i.TowerTypeID
              ,
                i.CustomerID
              ,
                i.SiteID
              ,
                i.SiteName
              ,
                i.CustomerSiteID
              ,
                i.CustomerSiteName
              ,
                i.CompanyID
              ,
                i.CompanyName
              ,
                i.SIRO
              ,
                i.STIPNumber
              ,
                i.StipCode
              ,
                i.Product
              ,
                i.MLANumber
              ,
                i.MLADate
              ,
                i.BaukNumber
              ,
                i.BaukDate
              ,
                i.PoAmount
              ,
                i.PoDate
            }), fieldList);
            table.Load(reader);

            //Export to Excel
             ExportToExcelHelper.Export("NewBaps", table);
        }

        #endregion

        #region Export to Excel
        #region Invoice Manual
        [Route("TrxUploadInvoiceManual/Export")]
        public void InvoiceManualExport()
        {
            //Parameter
            string strSONumber = Request.QueryString["SONumber"];
            string strDateFrom = Request.QueryString["dateFrom"];
            string strDateTo = Request.QueryString["dateTo"];



            //Call Service
            List<ARSystemService.vwtrxInvoiceManualTemp> invoiceManual = new List<ARSystemService.vwtrxInvoiceManualTemp>();
            using (var client = new ARSystemService.ItrxInvoiceManualTempServiceClient())
            {
                int intTotalRecord = client.GetInvoiceManualCount(UserManager.User.UserToken, strSONumber, strDateFrom, strDateTo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwtrxInvoiceManualTemp> invoiceManualHolder = new List<ARSystemService.vwtrxInvoiceManualTemp>();

                for (int i = 0; i <= intBatch; i++)
                {
                    invoiceManualHolder = client.GetInvoiceManualList(UserManager.User.UserToken, strSONumber, strDateFrom, strDateTo, null, 50 * i, 50).ToList();
                    invoiceManual.AddRange(invoiceManualHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"SONumber", "SiteIDOpr","SiteNameOpr",
                "InitialPONumber","InvoiceStartDate", "InvoiceEndDate","BaseLeasePrice","OMPrice", "StipSiro",
                "CompanyID","PriceCurrency","LossPNN", "BapsNO","BapsType", "OperatorName",
                "StartLeaseDate", "EndLeaseDate","InvoiceTerm", "MLANumber"
            };
            var reader = FastMember.ObjectReader.Create(invoiceManual.Select(i => new
            {
                i.SONumber,
                i.SiteIDOpr,
                i.SiteNameOpr,
                i.InitialPONumber,
                i.InvoiceStartDate,
                i.InvoiceEndDate,
                i.BaseLeasePrice,
                i.OMPrice,
                i.StipSiro,
                i.CompanyID,
                i.PriceCurrency,
                i.LossPNN,
                i.BapsNO,
                i.BapsType,
                i.OperatorName,
                i.StartLeaseDate,
                i.EndLeaseDate,
                i.InvoiceTerm,
                i.MLANumber
            }), ColumsShow);
            table.Load(reader);

            try
            {
                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(table, "InvoiceManual");

                System.Web.HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=InvoiceManual.xlsx");

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
            catch (Exception ex)
            {

            }
        }
        [Route("TrxBapsReceiveInvoiceManual/Export")]
        public void GetTrxBapsReceiveInvoiceManualToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strStatusBAPS = Request.QueryString["strStatusBAPS"];
            string strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strCurrency = Request.QueryString["strCurrency"];
            string strPONumber = Request.QueryString["strPONumber"];
            string strBAPSNumber = Request.QueryString["strBAPSNumber"];
            string strSONumber = Request.QueryString["strSONumber"];
            string strBapsType = Request.QueryString["strBapsType"];
            string strisReceive = Request.QueryString["isReceive"];
            string strSiteIdOld = Request.QueryString["strSiteIdOld"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            int isReceive = int.Parse(strisReceive);

            //Call Service
            List<ARSystemService.vwtrxInvoiceManualTemp> listBAPSData = new List<ARSystemService.vwtrxInvoiceManualTemp>();
            using (var client = new ARSystemService.ItrxInvoiceManualTempServiceClient())
            {
                int intTotalRecord = client.GetBAPSReceiveCount(UserManager.User.UserToken, strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwtrxInvoiceManualTemp> listBAPSDataHolder = new List<ARSystemService.vwtrxInvoiceManualTemp>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listBAPSDataHolder = client.GetBAPSReceiveList(UserManager.User.UserToken, strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod, null, 50 * i, 50).ToList();
                    listBAPSData.AddRange(listBAPSDataHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"SONumber", "SiteIDOpr","SiteNameOpr",
                "CompanyInvoice","OperatorInvoice", "PONumber","StartDateReceivable","EndDateReceivable", "BapsNO",
                "Basic","Maintenance","AmountInvoice", "StartDate","EndDate", "AmountPPN",
                "AmountLossPPN", "CompanyID","Operator", "InvoiceType","InvoiceTerm","BapsType","PowerType","Currency","PPHType","IsLossPPN"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
            {
                i.SONumber,
                i.SiteIDOpr,
                i.SiteNameOpr,
                CompanyInvoice = i.CompanyID,
                OperatorInvoice = i.OperatorName,
                PONumber = i.InitialPONumber,
                StartDateReceivable = i.InvoiceStartDate,
                EndDateReceivable = i.InvoiceEndDate,
                i.BapsNO,
                Basic = i.BaseLeasePrice,
                Maintenance = i.OMPrice,
                AmountInvoice = i.InvoiceAmount,
                StartDate = i.StartLeaseDate,
                EndDate = i.EndLeaseDate,
                AmountPPN = i.AmountPPN,
                AmountLossPPN = i.AmountLossPPN,
                CompanyID = i.CompanyID,
                Operator = i.OperatorName,
                InvoiceType = i.InvoiceType,
                InvoiceTerm = i.InvoiceTerm,
                BapsType = i.BapsType,
                PowerType = i.PowerType,
                Currency = i.PriceCurrency,
                PPHType = i.PPHType,
                IsLossPPN = i.LossPNN

            }), ColumsShow);
            table.Load(reader);

            try
            {
                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(table, "BAPSReceiveInvoiceManual");

                System.Web.HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=BAPSReceiveInvoiceManual.xlsx");

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
            catch (Exception ex)
            {

            }
        }
        [Route("TrxBapsConfirmInvoiceManual/Export")]
        public void GetTrxBapsConfirmInvoiceManualToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strStatusBAPS = Request.QueryString["strStatusBAPS"];
            string strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strCurrency = Request.QueryString["strCurrency"];
            string strPONumber = Request.QueryString["strPONumber"];
            string strBAPSNumber = Request.QueryString["strBAPSNumber"];
            string strSONumber = Request.QueryString["strSONumber"];
            string strBapsType = Request.QueryString["strBapsType"];
            string strisReceive = Request.QueryString["isReceive"];
            string strSiteIdOld = Request.QueryString["strSiteIdOld"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            int isReceive = int.Parse(strisReceive);

            //Call Service
            List<ARSystemService.vwtrxInvoiceManual> listBAPSData = new List<ARSystemService.vwtrxInvoiceManual>();
            using (var client = new ARSystemService.ItrxInvoiceManualServiceClient())
            {
                int intTotalRecord = client.GetBAPSConfirmInvoiceManualCount(UserManager.User.UserToken, strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwtrxInvoiceManual> listBAPSDataHolder = new List<ARSystemService.vwtrxInvoiceManual>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listBAPSDataHolder = client.GetBAPSConfirmInvoiceManualList(UserManager.User.UserToken, strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod, null, 50 * i, 50).ToList();
                    listBAPSData.AddRange(listBAPSDataHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"SONumber", "SiteIDOpr","SiteNameOpr",
                "CompanyInvoice","OperatorInvoice", "PONumber","StartDateReceivable","EndDateReceivable", "BapsNO",
                "Basic","Maintenance","AmountInvoice", "StartDate","EndDate", "AmountPPN",
                "AmountLossPPN", "CompanyID","Operator", "InvoiceType","InvoiceTerm","BapsType","PowerType","Currency","PPHType","IsLossPPN"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
            {
                i.SONumber,
                i.SiteIDOpr,
                i.SiteNameOpr,
                CompanyInvoice = i.CompanyID,
                OperatorInvoice = i.OperatorName,
                PONumber = i.InitialPONumber,
                StartDateReceivable = i.InvoiceStartDate,
                EndDateReceivable = i.InvoiceEndDate,
                i.BapsNO,
                Basic = i.BaseLeasePrice,
                Maintenance = i.OMPrice,
                AmountInvoice = i.InvoiceAmount,
                StartDate = i.StartLeaseDate,
                EndDate = i.EndLeaseDate,
                AmountPPN = i.AmountPPN,
                AmountLossPPN = i.AmountLossPPN,
                CompanyID = i.CompanyID,
                Operator = i.OperatorName,
                InvoiceType = i.InvoiceType,
                InvoiceTerm = i.InvoiceTerm,
                BapsType = i.BapsType,
                PowerType = i.PowerType,
                Currency = i.PriceCurrency,
                PPHType = i.PPHType,
                IsLossPPN = i.LossPNN

            }), ColumsShow);
            table.Load(reader);

            try
            {
                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(table, "BAPSConfirmInvoiceManual");

                System.Web.HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=BAPSConfirmInvoiceManual.xlsx");

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
            catch (Exception ex)
            {

            }
        }
        [Route("TrxBapsRejectInvoiceManual/Export")]
        public void GetTrxBapsRejectInvoiceManualToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strStatusBAPS = Request.QueryString["strStatusBAPS"];
            string strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strCurrency = Request.QueryString["strCurrency"];
            string strPONumber = Request.QueryString["strPONumber"];
            string strBAPSNumber = Request.QueryString["strBAPSNumber"];
            string strSONumber = Request.QueryString["strSONumber"];
            string strBapsType = Request.QueryString["strBapsType"];
            string strisReceive = Request.QueryString["isReceive"];
            string strSiteIdOld = Request.QueryString["strSiteIdOld"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];


            //Call Service
            List<ARSystemService.vwInvoiceManualReject> listBAPSData = new List<ARSystemService.vwInvoiceManualReject>();
            using (var client = new ARSystemService.ItrxInvoiceManualServiceClient())
            {
                int intTotalRecord = client.GetBAPSRejectInvoiceManualCount(UserManager.User.UserToken, strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwInvoiceManualReject> listBAPSDataHolder = new List<ARSystemService.vwInvoiceManualReject>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listBAPSDataHolder = client.GetBAPSRejectInvoiceManualList(UserManager.User.UserToken, strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod, null, 50 * i, 50).ToList();
                    listBAPSData.AddRange(listBAPSDataHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"SONumber", "Pica","RejectRemark", "SiteIDOpr","SiteNameOpr",
                "CompanyInvoice","OperatorInvoice", "PONumber","StartDateReceivable","EndDateReceivable", "BapsNO",
                "Basic","Maintenance","AmountInvoice", "StartDate","EndDate", "AmountPPN",
                "AmountLossPPN", "CompanyID","Operator", "InvoiceType","InvoiceTerm","BapsType","PowerType","Currency","PPHType","IsLossPPN"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
            {
                i.SONumber,
                Pica = i.Description,
                RejectRemark = i.RejectRemarks,
                i.SiteIDOpr,
                i.SiteNameOpr,
                CompanyInvoice = i.CompanyID,
                OperatorInvoice = i.OperatorName,
                PONumber = i.InitialPONumber,
                StartDateReceivable = i.InvoiceStartDate,
                EndDateReceivable = i.InvoiceEndDate,
                i.BapsNO,
                Basic = i.BaseLeasePrice,
                Maintenance = i.OMPrice,
                AmountInvoice = i.InvoiceAmount,
                StartDate = i.StartLeaseDate,
                EndDate = i.EndLeaseDate,
                AmountPPN = i.AmountPPN,
                AmountLossPPN = i.AmountLossPPN,
                CompanyID = i.CompanyID,
                Operator = i.OperatorName,
                InvoiceType = i.InvoiceType,
                InvoiceTerm = i.InvoiceTerm,
                BapsType = i.BapsType,
                PowerType = i.PowerType,
                Currency = i.PriceCurrency,
                PPHType = i.PPHType,
                IsLossPPN = i.LossPNN
            }), ColumsShow);
            table.Load(reader);

            try
            {
                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(table, "BAPSRejectInvoiceManual");

                System.Web.HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=BAPSRejectInvoiceManual.xlsx");

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region REPORT ARCBS
        //[Route("ReportARCBS/ExportHistory")]
        //public void ExportHistory()
        //{
        //    //Parameter
        //    string date = Request.QueryString["Date"];

        //    //Call Service
        //    List<ARSystemService.vwlogHistoryARCBS> ARCBS = new List<ARSystemService.vwlogHistoryARCBS>();
        //    using (var client = new ARSystemService.ItrxReportARCBSServiceClient())
        //    {
        //        int intTotalRecord = client.GetHistoryLogARCBSCount(UserManager.User.UserToken, date);
        //        int intBatch = intTotalRecord / 50;
        //        List<ARSystemService.vwlogHistoryARCBS> Holder = new List<ARSystemService.vwlogHistoryARCBS>();

        //        for (int i = 0; i <= intBatch; i++)
        //        {
        //            Holder = client.GetHistoryLogARCBSList(UserManager.User.UserToken, date, null, 50 * i, 50).ToList();
        //            ARCBS.AddRange(Holder);
        //        }
        //    }

        //    //Convert to DataTable
        //    DataTable table = new DataTable();
        //    string[] ColumsShow = new string[] {"Date", "Action", "ApproverName","Reason"
        //    };
        //    var reader = FastMember.ObjectReader.Create(ARCBS.Select(i => new
        //    {
        //        Date = i.CreatedDate,
        //        Action = i.Description,
        //        ApproverName = i.Name,
        //        Reason = i.Remarks
        //    }), ColumsShow);
        //    table.Load(reader);

        //    try
        //    {
        //        ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
        //        wbook.Worksheets.Add(table, "HistoryReportARCBS");

        //        System.Web.HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
        //        httpResponse.Clear();
        //        httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        httpResponse.AddHeader("content-disposition", "attachment;filename=HistoryReportARCBS.xlsx");

        //        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
        //        {
        //            wbook.SaveAs(memoryStream);
        //            memoryStream.WriteTo(httpResponse.OutputStream);
        //            memoryStream.Close();
        //        }

        //        httpResponse.End();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //[Route("ReportARCBS/ExportDetail")]
        //public void GetReportARCBSDetailExport()
        //{
        //    //Parameter
        //    string date = Request.QueryString["Date"];
        //    string week = Request.QueryString["Week"];
        //    bool isAccounting = Convert.ToBoolean(Request.QueryString["isAccounting"]);


        //    //Call Service
        //    List<ARSystemService.TempReportARCBS_Summary> ARCBS = new List<ARSystemService.TempReportARCBS_Summary>();
        //    using (var client = new ARSystemService.ItrxReportARCBSServiceClient())
        //    {
        //        int intTotalRecord = client.GetReportARCBSSummaryCount(UserManager.User.UserToken, date, week, isAccounting);
        //        int intBatch = intTotalRecord / 50;
        //        List<ARSystemService.TempReportARCBS_Summary> HolderSummary = new List<ARSystemService.TempReportARCBS_Summary>();

        //        //for (int i = 0; i <= intBatch; i++)
        //        //{
        //        HolderSummary = client.GetReportARCBSSummaryList(UserManager.User.UserToken, date, week, isAccounting, null, 0, intTotalRecord).ToList();
        //        ARCBS.AddRange(HolderSummary);
        //        //}
        //    }

        //    //Convert to DataTable
        //    DataTable tableSummary = new DataTable();
        //    string[] ColumsSummary = new string[] {"Operator", "WK1_Revenue","WK1_NonRevenue",
        //        "WK2_Revenue","WK2_NonRevenue","WK3_Revenue","WK3_NonRevenue","WK4_Revenue","WK4_NonRevenue",
        //        "WK5_Revenue","WK5_NonRevenue","WK6_Revenue","WK6_NonRevenue"
        //    };
        //    var reader = FastMember.ObjectReader.Create(ARCBS.Select(i => new
        //    {
        //        Operator = i.OperatorReport,
        //        WK1_Revenue = i.Revenue_1,
        //        WK1_NonRevenue = i.NonRevenue_1,
        //        WK2_Revenue = i.Revenue_2,
        //        WK2_NonRevenue = i.NonRevenue_2,
        //        WK3_Revenue = i.Revenue_3,
        //        WK3_NonRevenue = i.NonRevenue_3,
        //        WK4_Revenue = i.Revenue_4,
        //        WK4_NonRevenue = i.NonRevenue_4,
        //        WK5_Revenue = i.Revenue_5,
        //        WK5_NonRevenue = i.NonRevenue_5,
        //        WK6_Revenue = i.Revenue_6,
        //        WK6_NonRevenue = i.NonRevenue_6,
        //    }), ColumsSummary);
        //    tableSummary.Load(reader);
        //    var sumWK1_Revenue = tableSummary.Compute("Sum(WK1_Revenue)", string.Empty);
        //    var sumWK1_NonRevenue = tableSummary.Compute("Sum(WK1_NonRevenue)", string.Empty);
        //    var sumWK2_Revenue = tableSummary.Compute("Sum(WK2_Revenue)", string.Empty);
        //    var sumWK2_NonRevenue = tableSummary.Compute("Sum(WK2_NonRevenue)", string.Empty);
        //    var sumWK3_Revenue = tableSummary.Compute("Sum(WK3_Revenue)", string.Empty);
        //    var sumWK3_NonRevenue = tableSummary.Compute("Sum(WK3_NonRevenue)", string.Empty);
        //    var sumWK4_Revenue = tableSummary.Compute("Sum(WK4_Revenue)", string.Empty);
        //    var sumWK4_NonRevenue = tableSummary.Compute("Sum(WK4_NonRevenue)", string.Empty);
        //    var sumWK5_Revenue = tableSummary.Compute("Sum(WK5_Revenue)", string.Empty);
        //    var sumWK5_NonRevenue = tableSummary.Compute("Sum(WK5_NonRevenue)", string.Empty);
        //    var sumWK6_Revenue = tableSummary.Compute("Sum(WK6_Revenue)", string.Empty);
        //    var sumWK6_NonRevenue = tableSummary.Compute("Sum(WK6_NonRevenue)", string.Empty);
        //    tableSummary.Rows.Add("Grand Total", sumWK1_Revenue, sumWK1_NonRevenue, sumWK2_Revenue, sumWK2_NonRevenue, sumWK3_Revenue, sumWK3_NonRevenue, sumWK4_Revenue, sumWK4_NonRevenue,
        //        sumWK5_Revenue, sumWK5_NonRevenue, sumWK6_Revenue, sumWK6_NonRevenue);

        //    #region Get Detail Accounting OR AR
        //    List<ARSystemService.vwReportARCBSDetail> ARCBSDetail = new List<ARSystemService.vwReportARCBSDetail>();
        //    using (var client = new ARSystemService.ItrxReportARCBSServiceClient())
        //    {
        //        int intTotalRecord = client.GetReportARCBSDetailCount(UserManager.User.UserToken, date, week, "", "", isAccounting);
        //        int intBatch = intTotalRecord / 50;
        //        List<ARSystemService.vwReportARCBSDetail> HolderDetail = new List<ARSystemService.vwReportARCBSDetail>();

        //        //for (int i = 0; i <= intBatch; i++)
        //        //{
        //        HolderDetail = client.GetReportARCBSDetail(UserManager.User.UserToken, date, week, "", "", isAccounting, null, 0, intTotalRecord).ToList();
        //        ARCBSDetail.AddRange(HolderDetail);
        //        //}
        //    }

        //    DataTable tableDetail = new DataTable();
        //    if (isAccounting == true)
        //    {
        //        string[] ColumsDetailACC = new string[] {"SONumber", "Invoice","Operator",
        //        "SiteIDOperator","Company","StartSewa","EndSewa","BapsNo","SiteNameOperator",
        //        "TenantType","InvoiceNumber","NoMergedInvoice","InvoiceDate",

        //        "StartPeriodeTagih","EndPeriodeTagih","Basic","Maintenance",
        //        "AmountInvoice","AmountDiscount","AmountPinalty","SiteType",
        //        "PPN","Loss","Claim","FakturPajak","FPDate",

        //         "JenisPPH","CNDate","CategoryPicaCN","PicaRemarks","ClosingPeriodWeekly",
        //          "ClosingPeriodMonthly","RevType","InvoiceType","KursSMI"
        //    };
        //        var readerDetailACC = FastMember.ObjectReader.Create(ARCBSDetail.Select(i => new
        //        {
        //            SONumber = i.SONumber,
        //            Invoice = i.InvManualDesc,
        //            Operator = i.Operator,
        //            SiteIDOperator = i.SiteIDOperator,
        //            Company = i.CompanyID,
        //            StartSewa = i.StartSewa,
        //            EndSewa = i.EndSewa,
        //            BapsNo = i.BapsNo,
        //            SiteNameOperator = i.SiteName,
        //            TenantType = i.TenantType,
        //            InvoiceNumber = i.InvNo,
        //            NoMergedInvoice = i.NoMergedInv,
        //            InvoiceDate = i.InvDate,
        //            StartPeriodeTagih = i.StartInvDate,
        //            EndPeriodeTagih = i.EndInvDate,
        //            Basic = i.BasicAmount,
        //            Maintenance = i.MaintenanceAmount,
        //            AmountInvoice = i.InvAmount,
        //            AmountDiscount = i.AmountDiscount,
        //            AmountPinalty = i.AmountPenalty,
        //            SiteType = i.SiteType,
        //            PPN = i.IsLossPPNDesc,
        //            Loss = i.AmountPPNLoss,
        //            Claim = i.AmountPPNClaim,
        //            FakturPajak = i.TaxInvoice,
        //            FPDate = i.FPJDate,
        //            JenisPPH = i.PPHType,
        //            CNDate = i.CNDate,
        //            CategoryPicaCN = i.CNCategoryPica,
        //            PicaRemarks = i.CNRemarks,
        //            ClosingPeriodWeekly = i.ClosingPeriodWeeklyDesc,
        //            ClosingPeriodMonthly = i.ClosingPeriodMonthly,
        //            RevType = i.RevenueType,
        //            InvoiceType = i.InvoiceCNType,
        //            KursSMI = i.KursSMI


        //        }), ColumsDetailACC);
        //        tableDetail.Load(readerDetailACC);
        //    }
        //    else
        //    {
        //        string[] ColumsDetailAR = new string[] {"SONumber", "Invoice","Operator",
        //        "SiteIDOperator","Company","StartSewa","EndSewa","BapsNo","SiteNameOperator",
        //        "TenantType","InvoiceNumber","NoMergedInvoice","InvoiceDate",

        //        "StartPeriodeTagih","EndPeriodeTagih","Basic","Maintenance",
        //        "AmountInvoice","AmountDiscount","AmountPinalty","SiteType",
        //        "PPN","Loss","Claim","FakturPajak","FPDate",

        //         "JenisPPH","CNDate","CategoryPicaCN","PicaRemarks","ClosingPeriodWeekly",
        //          "ClosingPeriodMonthly","RevType","InvoiceType","KursSMI",
        //            "ReceiveDate",
        //           "ConfirmDate","CreatedInvoice","PostingInvoice","ChecklistInvoice","Verificator","Inputer","Finishing","ARDatabase"
        //    };
        //        var readerDetailAR = FastMember.ObjectReader.Create(ARCBSDetail.Select(i => new
        //        {
        //            SONumber = i.SONumber,
        //            Invoice = i.InvManualDesc,
        //            Operator = i.Operator,
        //            SiteIDOperator = i.SiteIDOperator,
        //            Company = i.CompanyID,
        //            StartSewa = i.StartSewa,
        //            EndSewa = i.EndSewa,
        //            BapsNo = i.BapsNo,
        //            SiteNameOperator = i.SiteName,
        //            TenantType = i.TenantType,
        //            InvoiceNumber = i.InvNo,
        //            NoMergedInvoice = i.NoMergedInv,
        //            InvoiceDate = i.InvDate,
        //            StartPeriodeTagih = i.StartInvDate,
        //            EndPeriodeTagih = i.EndInvDate,
        //            Basic = i.BasicAmount,
        //            Maintenance = i.MaintenanceAmount,
        //            AmountInvoice = i.InvAmount,
        //            AmountDiscount = i.AmountDiscount,
        //            AmountPinalty = i.AmountPenalty,
        //            SiteType = i.SiteType,
        //            PPN = i.IsLossPPNDesc,
        //            Loss = i.AmountPPNLoss,
        //            Claim = i.AmountPPNClaim,
        //            FakturPajak = i.TaxInvoice,
        //            FPDate = i.FPJDate,
        //            JenisPPH = i.PPHType,
        //            CNDate = i.CNDate,
        //            CategoryPicaCN = i.CNCategoryPica,
        //            PicaRemarks = i.CNRemarks,
        //            ClosingPeriodWeekly = i.ClosingPeriodWeeklyDesc,
        //            ClosingPeriodMonthly = i.ClosingPeriodMonthly,
        //            RevType = i.RevenueType,
        //            InvoiceType = i.InvoiceCNType,
        //            KursSMI = i.KursSMI
        //            ,
        //            ReceiveDate = i.ReceiveDate,
        //            ConfirmDate = i.ConfirmDate,
        //            CreatedInvoice = i.CreateInvDate,
        //            PostingInvoice = i.PostingDate,
        //            ChecklistInvoice = i.ChecklistDate,
        //            Verificator = i.LeadTimeVerificator,
        //            Inputer = i.LeadTimeInputer,
        //            Finishing = i.LeadTimeFinishing,
        //            ARDatabase = i.LeadTimeARData

        //        }), ColumsDetailAR);
        //        tableDetail.Load(readerDetailAR);
        //    }
        //    #endregion


        //    try
        //    {
        //        ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
        //        wbook.Worksheets.Add(tableSummary, "ARCBS_Summary");
        //        wbook.Worksheets.Add(tableDetail, "ARCBS_Detail");

        //        System.Web.HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
        //        httpResponse.Clear();
        //        httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        httpResponse.AddHeader("content-disposition", "attachment;filename=ReportARCBS.xlsx");

        //        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
        //        {
        //            wbook.SaveAs(memoryStream);
        //            memoryStream.WriteTo(httpResponse.OutputStream);
        //            memoryStream.Close();
        //        }

        //        httpResponse.End();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //[Route("ReportARCBS/ExportARDetail")]
        //public void GetReportARCBSARDetailExport()
        //{
        //    //Parameter
        //    string date = Request.QueryString["Date"];
        //    string week = Request.QueryString["Week"];
        //    bool isAccounting = Convert.ToBoolean(Request.QueryString["isAccounting"]);
        //    string Operator = Request.QueryString["operatorReport"].Replace("$", "&");
        //    string RevenueType = Request.QueryString["RevenueType"];

        //    #region Get Detail Accounting OR AR
        //    List<ARSystemService.vwReportARCBSDetail> ARCBSDetail = new List<ARSystemService.vwReportARCBSDetail>();
        //    using (var client = new ARSystemService.ItrxReportARCBSServiceClient())
        //    {
        //        int intTotalRecord = client.GetReportARCBSDetailCount(UserManager.User.UserToken, date, week, Operator, RevenueType, isAccounting);
        //        int intBatch = intTotalRecord / 50;
        //        List<ARSystemService.vwReportARCBSDetail> HolderDetail = new List<ARSystemService.vwReportARCBSDetail>();

        //        //for (int i = 0; i <= intBatch; i++)
        //        //{
        //        HolderDetail = client.GetReportARCBSDetail(UserManager.User.UserToken, date, week, Operator, RevenueType, isAccounting, null, 0, intTotalRecord).ToList();
        //        ARCBSDetail.AddRange(HolderDetail);
        //        //}
        //    }

        //    DataTable tableDetail = new DataTable();
        //    if (isAccounting == true)
        //    {
        //        string[] ColumsDetailACC = new string[] {"SONumber", "Invoice","Operator",
        //        "SiteIDOperator","Company","StartSewa","EndSewa","BapsNo","SiteNameOperator",
        //        "TenantType","InvoiceNumber","NoMergedInvoice","InvoiceDate",

        //        "StartPeriodeTagih","EndPeriodeTagih","Basic","Maintenance",
        //        "AmountInvoice","AmountDiscount","AmountPinalty","SiteType",
        //        "PPN","Loss","Claim","FakturPajak","FPDate",

        //         "JenisPPH","CNDate","CategoryPicaCN","PicaRemarks","ClosingPeriodWeekly",
        //          "ClosingPeriodMonthly","RevType","InvoiceType","KursSMI"
        //    };
        //        var readerDetailACC = FastMember.ObjectReader.Create(ARCBSDetail.Select(i => new
        //        {
        //            SONumber = i.SONumber,
        //            Invoice = i.InvManualDesc,
        //            Operator = i.Operator,
        //            SiteIDOperator = i.SiteIDOperator,
        //            Company = i.CompanyID,
        //            StartSewa = i.StartSewa,
        //            EndSewa = i.EndSewa,
        //            BapsNo = i.BapsNo,
        //            SiteNameOperator = i.SiteName,
        //            TenantType = i.TenantType,
        //            InvoiceNumber = i.InvNo,
        //            NoMergedInvoice = i.NoMergedInv,
        //            InvoiceDate = i.InvDate,
        //            StartPeriodeTagih = i.StartInvDate,
        //            EndPeriodeTagih = i.EndInvDate,
        //            Basic = i.BasicAmount,
        //            Maintenance = i.MaintenanceAmount,
        //            AmountInvoice = i.InvAmount,
        //            AmountDiscount = i.AmountDiscount,
        //            AmountPinalty = i.AmountPenalty,
        //            SiteType = i.SiteType,
        //            PPN = i.IsLossPPNDesc,
        //            Loss = i.AmountPPNLoss,
        //            Claim = i.AmountPPNClaim,
        //            FakturPajak = i.TaxInvoice,
        //            FPDate = i.FPJDate,
        //            JenisPPH = i.PPHType,
        //            CNDate = i.CNDate,
        //            CategoryPicaCN = i.CNCategoryPica,
        //            PicaRemarks = i.CNRemarks,
        //            ClosingPeriodWeekly = i.ClosingPeriodWeeklyDesc,
        //            ClosingPeriodMonthly = i.ClosingPeriodMonthly,
        //            RevType = i.RevenueType,
        //            InvoiceType = i.InvoiceCNType,
        //            KursSMI = i.KursSMI


        //        }), ColumsDetailACC);
        //        tableDetail.Load(readerDetailACC);
        //    }
        //    else
        //    {
        //        string[] ColumsDetailAR = new string[] {"SONumber", "Invoice","Operator",
        //        "SiteIDOperator","Company","StartSewa","EndSewa","BapsNo","SiteNameOperator",
        //        "TenantType","InvoiceNumber","NoMergedInvoice","InvoiceDate",

        //        "StartPeriodeTagih","EndPeriodeTagih","Basic","Maintenance",
        //        "AmountInvoice","AmountDiscount","AmountPinalty","SiteType",
        //        "PPN","Loss","Claim","FakturPajak","FPDate",

        //         "JenisPPH","CNDate","CategoryPicaCN","PicaRemarks","ClosingPeriodWeekly",
        //          "ClosingPeriodMonthly","RevType","InvoiceType","KursSMI",
        //            "ReceiveDate",
        //           "ConfirmDate","CreatedInvoice","PostingInvoice","ChecklistInvoice","Verificator","Inputer","Finishing","ARDatabase"
        //    };
        //        var readerDetailAR = FastMember.ObjectReader.Create(ARCBSDetail.Select(i => new
        //        {
        //            SONumber = i.SONumber,
        //            Invoice = i.InvManualDesc,
        //            Operator = i.Operator,
        //            SiteIDOperator = i.SiteIDOperator,
        //            Company = i.CompanyID,
        //            StartSewa = i.StartSewa,
        //            EndSewa = i.EndSewa,
        //            BapsNo = i.BapsNo,
        //            SiteNameOperator = i.SiteName,
        //            TenantType = i.TenantType,
        //            InvoiceNumber = i.InvNo,
        //            NoMergedInvoice = i.NoMergedInv,
        //            InvoiceDate = i.InvDate,
        //            StartPeriodeTagih = i.StartInvDate,
        //            EndPeriodeTagih = i.EndInvDate,
        //            Basic = i.BasicAmount,
        //            Maintenance = i.MaintenanceAmount,
        //            AmountInvoice = i.InvAmount,
        //            AmountDiscount = i.AmountDiscount,
        //            AmountPinalty = i.AmountPenalty,
        //            SiteType = i.SiteType,
        //            PPN = i.IsLossPPNDesc,
        //            Loss = i.AmountPPNLoss,
        //            Claim = i.AmountPPNClaim,
        //            FakturPajak = i.TaxInvoice,
        //            FPDate = i.FPJDate,
        //            JenisPPH = i.PPHType,
        //            CNDate = i.CNDate,
        //            CategoryPicaCN = i.CNCategoryPica,
        //            PicaRemarks = i.CNRemarks,
        //            ClosingPeriodWeekly = i.ClosingPeriodWeeklyDesc,
        //            ClosingPeriodMonthly = i.ClosingPeriodMonthly,
        //            RevType = i.RevenueType,
        //            InvoiceType = i.InvoiceCNType,
        //            KursSMI = i.KursSMI
        //            ,
        //            ReceiveDate = i.ReceiveDate,
        //            ConfirmDate = i.ConfirmDate,
        //            CreatedInvoice = i.CreateInvDate,
        //            PostingInvoice = i.PostingDate,
        //            ChecklistInvoice = i.ChecklistDate,
        //            Verificator = i.LeadTimeVerificator,
        //            Inputer = i.LeadTimeInputer,
        //            Finishing = i.LeadTimeFinishing,
        //            ARDatabase = i.LeadTimeARData

        //        }), ColumsDetailAR);
        //        tableDetail.Load(readerDetailAR);
        //    }
        //    #endregion


        //    try
        //    {
        //        ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
        //        //wbook.Worksheets.Add(tableSummary, "ARCBS_Summary");
        //        wbook.Worksheets.Add(tableDetail, "ARCBS_Detail");

        //        System.Web.HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
        //        httpResponse.Clear();
        //        httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        httpResponse.AddHeader("content-disposition", "attachment;filename=ReportARCBS.xlsx");

        //        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
        //        {
        //            wbook.SaveAs(memoryStream);
        //            memoryStream.WriteTo(httpResponse.OutputStream);
        //            memoryStream.Close();
        //        }

        //        httpResponse.End();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion

        [Route("TrxBapsConfirm/FreezeExport")]
        public void GetFreezeToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strStatusBAPS = Request.QueryString["strStatusBAPS"];
            string strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strCurrency = Request.QueryString["strCurrency"];
            string strPONumber = Request.QueryString["strPONumber"];
            string strBAPSNumber = Request.QueryString["strBAPSNumber"];
            string strSONumber = Request.QueryString["strSONumber"];
            string strBapsType = Request.QueryString["strBapsType"];
            int strisFreeze = Convert.ToInt32(Request.QueryString["isFreeze"]);
            string strSiteIdOld = Request.QueryString["strSiteIdOld"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];

            string strCreatedBy = Request.QueryString["strCreatedBy"];
            string fileName = "BAPSFreeze";
            if (strisFreeze == 0)
                fileName = "BAPSPOConfirm";
            //Call Service

            List<vwDataBAPSConfirm> listBAPSData = new List<vwDataBAPSConfirm>();
            var client = new trxBAPSDataService();
            int intTotalRecord = 0;

            intTotalRecord = client.GetTrxBapsConfirmNewFlowCount("", strCompanyId, strOperator, strBapsType, strPeriodInvoice, strInvoiceType, strSONumber, strPONumber, strBAPSNumber, strSiteIdOld, strStartPeriod, strEndPeriod, strisFreeze);
            listBAPSData = client.GetTrxBapsConfirmNewFlowToList("", strCompanyId, strOperator, strBapsType, strPeriodInvoice, strInvoiceType, strSONumber, strPONumber, strBAPSNumber, strSiteIdOld, strStartPeriod, strEndPeriod, strisFreeze, "", 0, 0);

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"SONumber", "SiteIdOpr","SiteId",
                "Sitename","CompanyInvoice", "OperatorInvoice","PoNumber","StartDateReceivable", "EndDateReceivable",
                "BapsNo","Basic","Maintenance", "AmountInvoice","StartDate", "EndDate",
                "AmountPPN", "AmountLossPPN","CompanyId", "Operator", "InvoiceType", "InvoiceTerm", "BapsType", "PowerType", "Currency","PPHType","LossPNN","Status"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
            {
                i.SONumber,
                i.SiteIdOpr,
                SiteId = i.SiteIdOld,
                Sitename = i.SiteName,
                i.CompanyInvoice,
                OperatorInvoice = i.Operator,
                i.PoNumber,
                StartDateReceivable = i.StartDateInvoice,
                EndDateReceivable = i.EndDateInvoice,
                i.BapsNo,
                Basic = i.AmountRental,
                Maintenance = i.AmountService,
                AmountInvoice = i.InvoiceAmount,
                i.StartDate,
                i.EndDate,
                i.AmountPPN,
                i.AmountLossPPN,
                i.CompanyId,
                Operator = i.OperatorAsset,
                InvoiceType = i.PeriodInvoice,
                InvoiceTerm = i.InvoiceTypeDesc,
                i.BapsType,
                i.PowerType,
                i.Currency,
                i.PPHType,
                LossPNN = i.IsLossPPN == true ? "Loss" : "Claim"
            }), ColumsShow);
            table.Load(reader);

            ExportToExcelHelper.Export(fileName, table);
        }

        [Route("TrxBapsConfirm/POConfirmExport")]
        public void GetPOConfirmToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strStatusBAPS = Request.QueryString["strStatusBAPS"];
            string strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strCurrency = Request.QueryString["strCurrency"];
            string strPONumber = Request.QueryString["strPONumber"];
            string strBAPSNumber = Request.QueryString["strBAPSNumber"];
            string strSONumber = Request.QueryString["strSONumber"];
            string strBapsType = Request.QueryString["strBapsType"];
            int strisFreeze = Convert.ToInt32(Request.QueryString["isFreeze"]);
            string strSiteIdOld = Request.QueryString["strSiteIdOld"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string strStatusDismantle = Request.QueryString["strStatusDismantle"];

            string strCreatedBy = Request.QueryString["strCreatedBy"];
            string fileName = "BAPSPOConfirm";

            //Call Service
            List<vwBAPSData> listBAPSData = new List<vwBAPSData>();

            var client = new trxBAPSDataService();
            int intTotalRecord = 0;

            intTotalRecord = client.GetTrxBapsDataCount("", strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod, 0, "", strStatusDismantle);
            listBAPSData = client.GetTrxBAPSDataToList("", strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod, 0, "", strStatusDismantle, "", 0, 0);

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"SONumber", "SiteIdOpr","SiteId",
                "Sitename","CompanyInvoice", "OperatorInvoice","PoNumber","StartDateReceivable", "EndDateReceivable",
                "BapsNo","Basic","Maintenance", "AmountInvoice","StartDate", "EndDate",
                "AmountPPN", "AmountLossPPN","CompanyId", "Operator", "InvoiceType", "InvoiceTerm", "BapsType", "PowerType", "Currency","PPHType","LossPNN",
                "BapsConfirmDate","POConfirmDate","Status","Remarks","status"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
            {
                i.SONumber,
                i.SiteIdOpr,
                SiteId = i.SiteIdOld,
                Sitename = i.SiteName,
                i.CompanyInvoice,
                OperatorInvoice = i.Operator,
                i.PoNumber,
                StartDateReceivable = i.StartDateInvoice,
                EndDateReceivable = i.EndDateInvoice,
                i.BapsNo,
                Basic = i.AmountRental,
                Maintenance = i.AmountService,
                AmountInvoice = i.InvoiceAmount,
                i.StartDate,
                i.EndDate,
                i.AmountPPN,
                i.AmountLossPPN,
                i.CompanyId,
                Operator = i.OperatorAsset,
                InvoiceType = i.PeriodInvoice,
                InvoiceTerm = i.InvoiceTypeDesc,
                i.BapsType,
                i.PowerType,
                i.Currency,
                i.PPHType,
                LossPNN = i.IsLossPPN == true ? "Loss" : "Claim",
                BapsConfirmDate = i.BapsConfirmDate,
                POConfirmDate = i.POConfirmDate,
                Status = i.StatusPOConfirm,
                Remarks = i.RemarksRejectPO,
                status = i.StatusTrx
            }), ColumsShow);
            table.Load(reader);

            ExportToExcelHelper.Export(fileName, table);
        }

        [Route("TrxBapsReceive/Export")]
        public void GetTrxBapsReceiveToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strStatusBAPS = Request.QueryString["strStatusBAPS"];
            string strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strCurrency = Request.QueryString["strCurrency"];
            string strPONumber = Request.QueryString["strPONumber"];
            string strBAPSNumber = Request.QueryString["strBAPSNumber"];
            string strSONumber = Request.QueryString["strSONumber"];
            string strBapsType = Request.QueryString["strBapsType"];
            string strisReceive = Request.QueryString["isReceive"];
            string strSiteIdOld = Request.QueryString["strSiteIdOld"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            int isReceive = int.Parse(strisReceive);
            string strCreatedBy = Request.QueryString["strCreatedBy"];
            string strStatusDismantle = Request.QueryString["strStatusDismantle"];
            //Call Service
            List<vwBAPSData> listBAPSData = new List<vwBAPSData>();
            var client = new trxBAPSDataService();

            int intTotalRecord = client.GetTrxBapsDataCount("", strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod, isReceive, strCreatedBy, strStatusDismantle);

            listBAPSData = client.GetTrxBAPSDataToList("", strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod, isReceive, strCreatedBy, strStatusDismantle, null, 0, 0).ToList();

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"SONumber", "SiteIdOpr","SiteId",
                "Sitename","CompanyInvoice", "OperatorInvoice","PoNumber","StartDateReceivable", "EndDateReceivable",
                "BapsNo","Basic","Maintenance", "AmountInvoice","StartDate", "EndDate",
                "AmountPPN", "AmountLossPPN","CompanyId", "Operator", "InvoiceType", "InvoiceTerm", "BapsType", "PowerType", "Currency","PPHType","LossPNN","Status","DismantleDate"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
            {
                i.SONumber,
                i.SiteIdOpr,
                SiteId = i.SiteIdOld,
                Sitename = i.SiteName,
                i.CompanyInvoice,
                OperatorInvoice = i.Operator,
                i.PoNumber,
                StartDateReceivable = i.StartDateInvoice,
                EndDateReceivable = i.EndDateInvoice,
                i.BapsNo,
                Basic = i.AmountRental,
                Maintenance = i.AmountService,
                AmountInvoice = i.InvoiceAmount,
                i.StartDate,
                i.EndDate,
                i.AmountPPN,
                i.AmountLossPPN,
                i.CompanyId,
                Operator = i.OperatorAsset,
                InvoiceType = i.PeriodInvoice,
                InvoiceTerm = i.InvoiceTypeDesc,
                i.BapsType,
                i.PowerType,
                i.Currency,
                i.PPHType,
                LossPNN = i.IsLossPPN == true ? "Loss" : "Claim",
                Status = i.StatusTrx,
                i.DismantleDate
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            if (isReceive == 1)
                #region 'EXPORT TO EXCEL WITH CUSTOM BACKGROUND'
                try
                {
                    ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                    IXLWorksheet ws = wbook.Worksheets.Add("Sheet 1");

                    // From a DataTable
                    var dataTable = table;
                    IXLTable tableWithData = ws.Cell(1, 1).InsertTable(dataTable.AsEnumerable());
                    for (int c = 1; c <= dataTable.Columns.Count; c++)
                    {
                        ws.Column(c).AdjustToContents();
                    }

                    for (int i = 0; i < listBAPSData.Count; i++)
                    {
                        if (listBAPSData[i].IsLossPPN == true)
                        {
                            if (listBAPSData[i].AmountPPN != listBAPSData[i].AmountLossPPN)
                            {
                                ws.Row(i + 2).Style.Fill.BackgroundColor = XLColor.Red;
                            }
                        }
                    }


                    HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
                    httpResponse.Clear();
                    httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    httpResponse.AddHeader("content-disposition", "attachment;filename=" + "TrxBapsReceive" + ".xlsx");

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wbook.SaveAs(memoryStream);
                        memoryStream.WriteTo(httpResponse.OutputStream);
                        memoryStream.Close();
                    }

                    httpResponse.End();
                }
                catch (Exception err)
                {

                }
            #endregion
            else
                ExportToExcelHelper.Export("TrxBapsConfirm", table);
        }

        [Route("TrxBapsConfirm/Export")]
        public void GetTrxBapsConfirmToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strStatusBAPS = Request.QueryString["strStatusBAPS"];
            string strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strCurrency = Request.QueryString["strCurrency"];
            string strPONumber = Request.QueryString["strPONumber"];
            string strBAPSNumber = Request.QueryString["strBAPSNumber"];
            string strSONumber = Request.QueryString["strSONumber"];
            string strBapsType = Request.QueryString["strBapsType"];
            string strisReceive = Request.QueryString["isReceive"];
            string strSiteIdOld = Request.QueryString["strSiteIdOld"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            int isReceive = int.Parse(strisReceive);
            string strCreatedBy = Request.QueryString["strCreatedBy"];
            string strStatusDismantle = Request.QueryString["strStatusDismantle"];
            //Call Service
            List<vwBAPSData> listBAPSData = new List<vwBAPSData>();
            var client = new trxBAPSDataService();

            int intTotalRecord = client.GetTrxBapsDataCount("", strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod, isReceive, strCreatedBy, strStatusDismantle);

            listBAPSData = client.GetTrxBAPSDataToList("", strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod, isReceive, strCreatedBy, strStatusDismantle, null, 0, 0).ToList();


            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"SONumber", "SiteIdOpr","SiteId",
                "Sitename","CompanyInvoice", "OperatorInvoice","PoNumber","StartDateReceivable", "EndDateReceivable",
                "BapsNo","Basic","Maintenance", "AmountInvoice","StartDate", "EndDate",
                "AmountPPN", "AmountLossPPN","CompanyId", "Operator", "InvoiceType", "InvoiceTerm", "BapsType", "PowerType", "Currency","PPHType","LossPNN",
                "BapsConfirmDate","POConfirmDate","Status"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
            {
                i.SONumber,
                i.SiteIdOpr,
                SiteId = i.SiteIdOld,
                Sitename = i.SiteName,
                i.CompanyInvoice,
                OperatorInvoice = i.Operator,
                i.PoNumber,
                StartDateReceivable = i.StartDateInvoice,
                EndDateReceivable = i.EndDateInvoice,
                i.BapsNo,
                Basic = i.AmountRental,
                Maintenance = i.AmountService,
                AmountInvoice = i.InvoiceAmount,
                i.StartDate,
                i.EndDate,
                i.AmountPPN,
                i.AmountLossPPN,
                i.CompanyId,
                Operator = i.OperatorAsset,
                InvoiceType = i.PeriodInvoice,
                InvoiceTerm = i.InvoiceTypeDesc,
                i.BapsType,
                i.PowerType,
                i.Currency,
                i.PPHType,
                LossPNN = i.IsLossPPN == true ? "Loss" : "Claim",
                BapsConfirmDate = i.BapsConfirmDate,
                POConfirmDate = i.POConfirmDate,
                Status = i.StatusTrx
            }), ColumsShow);
            table.Load(reader);


            ExportToExcelHelper.Export("TrxBapsConfirm", table);
        }

        [Route("TrxBapsReject/Export")]
        public void GetTrxBapsRejectToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strStatusBAPS = Request.QueryString["strStatusBAPS"];
            string strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strCurrency = Request.QueryString["strCurrency"];
            string strPONumber = Request.QueryString["strPONumber"];
            string strBAPSNumber = Request.QueryString["strBAPSNumber"];
            string strSONumber = Request.QueryString["strSONumber"];
            string strBapsType = Request.QueryString["strBapsType"];
            string strSiteIdOld = Request.QueryString["strSiteIdOld"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];

            //Call Service
            List<vwBAPSDataReject> listBAPSData = new List<vwBAPSDataReject>();
            var client = new trxBAPSDataService();

            int intTotalRecord = client.GetTrxBapsRejectCount("", strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod);

            listBAPSData = client.GetTrxBAPSRejectToList("", strCompanyId, strOperator, strStatusBAPS, strPeriodInvoice, strInvoiceType, strCurrency, strPONumber, strBAPSNumber, strSONumber, strBapsType, strSiteIdOld, strStartPeriod, strEndPeriod, null, 0, 0).ToList();

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"SONumber","PICA","RejectRemark", "SiteIdOpr","SiteId",
                "Sitename","CompanyInvoice", "OperatorInvoice","PoNumber","StartDateReceivable", "EndDateReceivable",
                "BapsNo","Basic","Maintenance", "AmountInvoice","StartDate", "EndDate",
                "AmountPPN", "AmountLossPPN","CompanyId", "Operator", "InvoiceType", "InvoiceTerm", "BapsType", "PowerType", "Currency","PPHType","LossPNN"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
            {
                i.SONumber,
                PICA = i.Description,
                RejectRemark = i.RejectRemarks,
                i.SiteIdOpr,
                SiteId = i.SiteIdOld,
                Sitename = i.SiteName,
                i.CompanyInvoice,
                OperatorInvoice = i.Operator,
                i.PoNumber,
                StartDateReceivable = i.StartDateInvoice,
                EndDateReceivable = i.EndDateInvoice,
                i.BapsNo,
                Basic = i.AmountRental,
                Maintenance = i.AmountService,
                AmountInvoice = i.InvoiceAmount,
                i.StartDate,
                i.EndDate,
                i.AmountPPN,
                i.AmountLossPPN,
                i.CompanyId,
                Operator = i.OperatorAsset,
                InvoiceType = i.PeriodInvoice,
                InvoiceTerm = i.InvoiceTypeDesc,
                i.BapsType,
                i.PowerType,
                i.Currency,
                i.PPHType,
                LossPNN = i.IsLossPPN == true ? "Loss" : "Claim"
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxBapsReject", table);
        }

        [Route("TrxCreateInvoiceTower/Export")]
        public void GetTrxCreateInvoiceTowerToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strBapsType = Request.QueryString["strBapsType"];
            string strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strRegional = Request.QueryString["strRegional"];
            string strSONumber = Request.QueryString["strSONumber"];
            string intmstInvoiceStatusId = Request.QueryString["intmstInvoiceStatusId"];
            string strPONumber = Request.QueryString["strPONumber"];
            string strBAPSNumber = Request.QueryString["strBAPSNumber"];
            string strSiteIdOld = Request.QueryString["strSiteIdOld"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string invoiceManual = Request.QueryString["invoiceManual"];
            //Call Service
            List<ARSystemService.vwDataBAPSConfirm> listBAPSData = new List<ARSystemService.vwDataBAPSConfirm>();
            using (var client = new ARSystemService.ItrxCreateInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetTrxCreateInvoiceTowerCount(UserManager.User.UserToken, strCompanyId, strOperator, strBapsType, strPeriodInvoice, strInvoiceType, strRegional, strSONumber, int.Parse(intmstInvoiceStatusId), strPONumber, strBAPSNumber, strSiteIdOld, strStartPeriod, strEndPeriod, invoiceManual == null ? -1 : int.Parse(invoiceManual));
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwDataBAPSConfirm> listBAPSDataConfirmHolder = new List<ARSystemService.vwDataBAPSConfirm>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listBAPSDataConfirmHolder = client.GetTrxCreateInvoiceTowerToList(UserManager.User.UserToken, strCompanyId, strOperator, strBapsType, strPeriodInvoice, strInvoiceType, strRegional, strSONumber, int.Parse(intmstInvoiceStatusId), strPONumber, strBAPSNumber, strSiteIdOld, strStartPeriod, strEndPeriod, invoiceManual == null ? -1 : int.Parse(invoiceManual), null, 50 * i, 50).ToList();
                    listBAPSData.AddRange(listBAPSDataConfirmHolder);
                }
            }
            string UserRole = "";
            using (var client = new ARSystemService.UserServiceClient())
            {
                UserRole = client.GetARUserPosition(UserManager.User.UserToken).Result;
            }
            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { };
            if (UserRole == "DEPT HEAD")
            {
                ColumsShow = new string[] { "SONumber", "SiteIdOpr","SiteId",
                "Sitename","CompanyInvoice", "OperatorInvoice","PoNumber","StartDateReceivable", "EndDateReceivable",
                 "BapsNo","Basic","Maintenance", "AmountInvoice","Status","ReturnRemarks",   "StartDate", "EndDate", "AmountPPN", "AmountLossPPN",
                "CompanyId", "Operator", "InvoiceType", "InvoiceTerm", "BapsType", "PowerType", "Currency","PPHType"};

                var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
                {
                    i.SONumber,
                    i.SiteIdOpr,
                    SiteId = i.SiteIdOld,
                    Sitename = i.SiteName,
                    i.CompanyInvoice,
                    OperatorInvoice = i.Operator,
                    i.PoNumber,
                    StartDateReceivable = i.StartDateInvoice,
                    EndDateReceivable = i.EndDateInvoice,
                    i.BapsNo,
                    Basic = i.AmountRental,
                    Maintenance = i.AmountService,
                    AmountInvoice = i.InvoiceAmount,
                    Status = i.InvStatus,
                    i.ReturnRemarks,
                    i.StartDate,
                    i.EndDate,
                    i.AmountPPN,
                    i.AmountLossPPN,
                    i.CompanyId,
                    Operator = i.OperatorAsset,
                    InvoiceType = i.PeriodInvoice,
                    InvoiceTerm = i.InvoiceTypeDesc,
                    i.BapsType,
                    i.PowerType,
                    i.Currency,
                    i.PPHType
                }), ColumsShow);

                table.Load(reader);
            }
            else
            {
                ColumsShow = new string[] { "SONumber","SiteIdOpr","SiteId","Sitename","CompanyInvoice", "OperatorInvoice",
                 "PoNumber","StartDateReceivable", "EndDateReceivable","BapsNo","Basic","Maintenance",
                 "AmountInvoice",  "StartDate", "EndDate","AmountPPN", "AmountLossPPN",
                "CompanyId", "Operator", "InvoiceType", "InvoiceTerm", "BapsType", "PowerType", "Currency","PPHType"};
                var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
                {
                    i.SONumber,
                    i.SiteIdOpr,
                    SiteId = i.SiteIdOld,
                    Sitename = i.SiteName,
                    i.CompanyInvoice,
                    OperatorInvoice = i.Operator,
                    i.PoNumber,
                    StartDateReceivable = i.StartDateInvoice,
                    EndDateReceivable = i.EndDateInvoice,
                    i.BapsNo,
                    Basic = i.AmountRental,
                    Maintenance = i.AmountService,
                    AmountInvoice = i.InvoiceAmount,
                    i.StartDate,
                    i.EndDate,
                    i.AmountPPN,
                    i.AmountLossPPN,
                    i.CompanyId,
                    Operator = i.OperatorAsset,
                    InvoiceType = i.PeriodInvoice,
                    InvoiceTerm = i.InvoiceTypeDesc,
                    i.BapsType,
                    i.PowerType,
                    i.Currency,
                    i.PPHType
                }), ColumsShow);

                table.Load(reader);
            }



            //Export to Excel
            ExportToExcelHelper.Export("TrxCreateInvoiceTower", table);
        }

        [Route("TrxCreateInvoiceOtherProduct/Export")]
        public void GetTrxCreateInvoiceOtherProductToExport()
        {
            //Parameter
            string invoiceNumber = Request.QueryString["invoiceNumber"];

            //Call Service
            List<ARSystemService.vwInvoiceOtherProductDetail> list = new List<ARSystemService.vwInvoiceOtherProductDetail>();
            using (var client = new ARSystemService.ItrxInvoiceOtherProductDetailServiceClient())
            {
                int intTotalRecord = client.GetInvoiceOtherProductDetailCount(UserManager.User.UserToken, invoiceNumber);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwInvoiceOtherProductDetail> listHolder = new List<ARSystemService.vwInvoiceOtherProductDetail>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetTrxInvoiceOtherProductDetailToList(UserManager.User.UserToken, invoiceNumber, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "InvNo",
                "ProductType",
                "ProductName",
                "CustomerName",
                "InvoiceStartDate",
                "InvoiceEndDate",
                "Currency",
                "InvTotalAmount"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("OtherProductInvoices", table);
        }

        [Route("TrxPostingInvoiceTower/Export")]
        public void GetTrxPostingInvoiceTowerToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string intmstInvoiceStatusId = Request.QueryString["intmstInvoiceStatusId"];
            string invNo = Request.QueryString["invNo"];
            string invManual = Request.QueryString["invoiceManual"];
            //Call Service
            List<ARSystemService.vwDataCreatedInvoiceTower> listDataCreatedInvoice = new List<ARSystemService.vwDataCreatedInvoiceTower>();

            using (var client = new ARSystemService.ItrxPostingInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetTrxPostingInvoiceTowerCount(UserManager.User.UserToken, strCompanyId, strOperator, strInvoiceType, int.Parse(intmstInvoiceStatusId), invNo, int.Parse(invManual));
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwDataCreatedInvoiceTower> listDataCreatedInvoiceHolder = new List<ARSystemService.vwDataCreatedInvoiceTower>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataCreatedInvoiceHolder = client.GetTrxPostingInvoiceTowerToList(UserManager.User.UserToken, strCompanyId, strOperator, strInvoiceType, int.Parse(intmstInvoiceStatusId), invNo, int.Parse(invManual), null, 50 * i, 50).ToList();
                    listDataCreatedInvoice.AddRange(listDataCreatedInvoiceHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"NoInvoiceTemporary",
                "InvoiceDate", "CreateInvoice", "TermInvoice", "Company", "CompanyInvoice", "Operator",
                "OperatorInvoice", "Amount", "Discount", "PPN", "Penalty", "Currency","InvoiceStatus"
            };
            var reader = FastMember.ObjectReader.Create(listDataCreatedInvoice.Select(i => new
            {
                NoInvoiceTemporary = i.InvTemp,
                InvoiceDate = i.InvDate,
                i.CreateInvoice,
                TermInvoice = i.TermPeriod,
                i.Company,
                i.CompanyInvoice,
                i.Operator,
                i.OperatorInvoice,
                i.Amount,
                i.Discount,
                PPN = i.InvTotalAPPN,
                Penalty = i.InvTotalPenalty,
                i.Currency,
                InvoiceStatus = i.InvStatus
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxPostingInvoiceTower", table);
        }

        [Route("TrxCreateInvoiceBuilding/Export")]
        public void GetTrxCreateInvoiceBuildingToExport()
        {
            //Parameter
            string companyName = Request.QueryString["companyName"];
            string invoiceTypeId = Request.QueryString["invoiceTypeId"];
            string invNo = Request.QueryString["invNo"];
            string invCateg = Request.QueryString["invCateg"];

            //Call Service
            List<ARSystemService.vwInvoiceBuildingDetail> list = new List<ARSystemService.vwInvoiceBuildingDetail>();
            using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
            {
                int intTotalRecord = client.GetInvoiceBuildingDetailCount(UserManager.User.UserToken, companyName, invoiceTypeId, -1, invNo, invCateg);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwInvoiceBuildingDetail> listHolder = new List<ARSystemService.vwInvoiceBuildingDetail>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetTrxInvoiceBuildingDetailToList(UserManager.User.UserToken, companyName, invoiceTypeId, -1, invNo, invCateg, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "InvTemp",
                "CompanyName",
                "Area",
                "MeterPrice",
                "StartPeriod",
                "EndPeriod",
                "TermPeriod",
                "InvSumADPP",
                "InvTotalAPPN"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("BuildingInvoices", table);
        }

        [Route("TrxCreateInvoiceTowerRemainingAmount/Export")]
        public void GetTrxCreateInvoiceTowerRemainingAmountToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strBapsType = Request.QueryString["strBapsType"];
            string strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strRegional = Request.QueryString["strRegional"];
            string InvoiceCategoryId = Request.QueryString["InvoiceCategoryId"];
            string strSONumber = Request.QueryString["strSONumber"];
            string intmstInvoiceStatusId = Request.QueryString["intmstInvoiceStatusId"];
            string strPONumber = Request.QueryString["strPONumber"];
            string strBAPSNumber = Request.QueryString["strBAPSNumber"];
            string strSiteIdOld = Request.QueryString["strSiteIdOld"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            //Call Service
            List<ARSystemService.vwDataBAPSRemainingAmount> listBAPSData = new List<ARSystemService.vwDataBAPSRemainingAmount>();
            using (var client = new ARSystemService.ItrxCreateInvoiceTowerRemainingAmountServiceClient())
            {
                int intTotalRecord = client.GetTrxCreateInvoiceTowerRemainingCount(UserManager.User.UserToken, strCompanyId, strOperator, strBapsType, strPeriodInvoice, strInvoiceType, strRegional, int.Parse(InvoiceCategoryId), strSONumber, int.Parse(intmstInvoiceStatusId), strPONumber, strBAPSNumber, strSiteIdOld, strStartPeriod, strEndPeriod);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwDataBAPSRemainingAmount> listBAPSDataConfirmHolder = new List<ARSystemService.vwDataBAPSRemainingAmount>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listBAPSDataConfirmHolder = client.GetTrxCreateInvoiceTowerRemaningAmountToList(UserManager.User.UserToken, strCompanyId, strOperator, strBapsType, strPeriodInvoice, strInvoiceType, strRegional, int.Parse(InvoiceCategoryId), strSONumber, int.Parse(intmstInvoiceStatusId), strPONumber, strBAPSNumber, strSiteIdOld, strStartPeriod, strEndPeriod, null, 50 * i, 50).ToList();
                    listBAPSData.AddRange(listBAPSDataConfirmHolder);
                }
            }
            string UserRole = "";
            using (var client = new ARSystemService.UserServiceClient())
            {
                UserRole = client.GetARUserPosition(UserManager.User.UserToken).Result;
            }
            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] { };
            if (UserRole == "DEPT HEAD")
            {
                ColumsShow = new string[] { "SONumber","SiteIdOpr","SiteId",
                "Sitename","CompanyInvoice", "OperatorInvoice","PoNumber","StartDateReceivable", "EndDateReceivable",
                "BapsNo","Basic","Maintenance", "AmountInvoice","Status","ReturnRemarks","StartDate", "EndDate","AmountPPN", "AmountLossPPN",
                "CompanyId", "Operator", "InvoiceType", "InvoiceTerm", "BapsType", "PowerType", "Currency","PPHType"};

                var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
                {
                    i.SONumber,
                    i.SiteIdOpr,
                    SiteId = i.SiteIdOld,
                    Sitename = i.SiteName,
                    i.CompanyInvoice,
                    OperatorInvoice = i.Operator,
                    i.PoNumber,
                    StartDateReceivable = i.StartDateInvoice,
                    EndDateReceivable = i.EndDateInvoice,
                    i.BapsNo,
                    Basic = i.AmountRental,
                    Maintenance = i.AmountService,
                    AmountInvoice = i.InvoiceAmount,
                    Status = i.InvStatus,
                    i.ReturnRemarks,
                    i.StartDate,
                    i.EndDate,
                    i.AmountPPN,
                    i.AmountLossPPN,
                    i.CompanyId,
                    Operator = i.OperatorAsset,
                    InvoiceType = i.PeriodInvoice,
                    InvoiceTerm = i.InvoiceTypeDesc,
                    i.BapsType,
                    i.PowerType,
                    i.Currency,
                    i.PPHType
                }), ColumsShow);

                table.Load(reader);
            }
            else
            {
                ColumsShow = new string[] { "SONumber","SiteIdOpr","SiteId","Sitename","CompanyInvoice", "OperatorInvoice",
                    "PoNumber","StartDateReceivable", "EndDateReceivable","BapsNo","Basic","Maintenance", "AmountInvoice",
                   "StartDate", "EndDate","AmountPPN", "AmountLossPPN",
                    "CompanyId", "Operator", "InvoiceType", "InvoiceTerm", "BapsType", "PowerType", "Currency","PPHType"};
                var reader = FastMember.ObjectReader.Create(listBAPSData.Select(i => new
                {
                    i.SONumber,
                    i.SiteIdOpr,
                    SiteId = i.SiteIdOld,
                    Sitename = i.SiteName,
                    i.CompanyInvoice,
                    OperatorInvoice = i.Operator,
                    i.PoNumber,
                    StartDateReceivable = i.StartDateInvoice,
                    EndDateReceivable = i.EndDateInvoice,
                    i.BapsNo,
                    Basic = i.AmountRental,
                    Maintenance = i.AmountService,
                    AmountInvoice = i.InvoiceAmount,
                    i.StartDate,
                    i.EndDate,
                    i.AmountPPN,
                    i.AmountLossPPN,
                    i.CompanyId,
                    Operator = i.OperatorAsset,
                    InvoiceType = i.PeriodInvoice,
                    InvoiceTerm = i.InvoiceTypeDesc,
                    i.BapsType,
                    i.PowerType,
                    i.Currency,
                    i.PPHType
                }), ColumsShow);

                table.Load(reader);
            }



            //Export to Excel
            ExportToExcelHelper.Export("TrxCreateInvoiceTowerRemaining", table);
        }

        [Route("TrxPrintInvoiceTower/Export")]
        public void GetTrxPrintInvoiceTowerToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string intmstInvoiceStatusId = Request.QueryString["intmstInvoiceStatusId"];
            string invNo = Request.QueryString["invNo"];
            string invoiceManual = Request.QueryString["invoiceManual"];

            //Call Service
            List<ARSystemService.vwDataPostedInvoiceTower> listDataPostedInvoice = new List<ARSystemService.vwDataPostedInvoiceTower>();

            using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetTrxPrintInvoiceTowerCount(UserManager.User.UserToken, strCompanyId, strOperator, strInvoiceType, strStartPeriod, strEndPeriod, int.Parse(intmstInvoiceStatusId), invNo, int.Parse(invoiceManual));
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwDataPostedInvoiceTower> listDataPostedInvoiceHolder = new List<ARSystemService.vwDataPostedInvoiceTower>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataPostedInvoiceHolder = client.GetTrxPrintnvoiceTowerToList(UserManager.User.UserToken, strCompanyId, strOperator, strInvoiceType, strStartPeriod, strEndPeriod, int.Parse(intmstInvoiceStatusId), invNo, int.Parse(invoiceManual), null, 50 * i, 50).ToList();
                    listDataPostedInvoice.AddRange(listDataPostedInvoiceHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"StatusPrint",
                "NoInvoice", "InvoiceDate", "TermInvoice", "Company", "Operator",
                "Amount", "Discount", "PPN", "Penalty", "Currency","PaidStatus","CNStatus","ChecklistStatus","InvoiceCategory","PPHType","PrintUsers"
            };
            var reader = FastMember.ObjectReader.Create(listDataPostedInvoice.Select(i => new
            {
                StatusPrint = i.PrintStatus,
                NoInvoice = i.InvNo,
                InvoiceDate = i.InvPrintDate,
                TermInvoice = i.InvTypeDesc,
                Company = i.InvCompanyId,
                Operator = i.InvOperatorID,
                Amount = i.InvTotalAmount,
                Discount = i.Currency,
                PPN = i.InvTotalAPPN,
                Penalty = i.InvTotalPenalty,
                i.Currency,
                PaidStatus = i.InvPaidStatus,
                i.CNStatus,
                ChecklistStatus = i.ChecklistStatus,
                InvoiceCategory = i.InvoiceCategory,
                i.PPHType,
                i.PrintUsers
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxPrintInvoiceTower", table);

        }

        [Route("TrxPrintInvoiceBuilding/Export")]
        public void GetTrxPrintInvoiceBuildingToExport()
        {
            //Parameter
            string companyName = Request.QueryString["companyName"];
            string invoiceTypeId = Request.QueryString["invoiceTypeId"];
            int invoiceStatusId = int.Parse(Request.QueryString["invoiceStatusId"]);
            string invNo = Request.QueryString["invNo"];

            DateTime? startPeriod = DateTime.MinValue;
            DateTime? endPeriod = DateTime.MaxValue;

            if (!string.IsNullOrEmpty(Request.QueryString["startPeriod"]))
                startPeriod = DateTime.Parse(Request.QueryString["startPeriod"]);

            if (!string.IsNullOrEmpty(Request.QueryString["endPeriod"]))
                endPeriod = DateTime.Parse(Request.QueryString["endPeriod"]);
            //Call Service
            List<ARSystemService.vwPrintInvoiceBuilding> list = new List<ARSystemService.vwPrintInvoiceBuilding>();

            using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
            {
                int intTotalRecord = client.GetPrintInvoiceBuildingCount(UserManager.User.UserToken, companyName, invoiceTypeId, startPeriod, endPeriod, invoiceStatusId, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwPrintInvoiceBuilding> listHolder = new List<ARSystemService.vwPrintInvoiceBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetPrintInvoiceBuildingToList(UserManager.User.UserToken, companyName, invoiceTypeId, startPeriod, endPeriod, invoiceStatusId, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] fieldList = new string[] {
                "PrintStatus",
                "InvNo",
                "InvPrintDate",
                "Term",
                "CompanyTBG",
                "Company",
                "InvTotalAmount",
                "Discount",
                "InvTotalAPPN",
                "InvTotalPenalty",
                "Currency",
                "StartPeriod",
                "EndPeriod",
                "InvPaidStatus",
                "ChecklistStatus",
                "PrintUsers"
            };
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxPrintInvoiceBuilding", table);

        }

        [Route("TrxChecklistInvoiceBuilding/Export")]
        public void GetTrxChecklistInvoiceBuildingToExport()
        {
            //Parameter
            string companyName = Request.QueryString["companyName"];
            string invoiceTypeID = Request.QueryString["invoiceTypeID"];
            DateTime? postingDateFrom = DateTime.MinValue;
            DateTime? postingDateTo = DateTime.MaxValue;
            string status = Request.QueryString["status"];
            string invNo = Request.QueryString["invNo"];

            if (!string.IsNullOrEmpty(Request.QueryString["postingDateFrom"]))
                postingDateFrom = DateTime.Parse(Request.QueryString["postingDateFrom"]);

            if (!string.IsNullOrEmpty(Request.QueryString["postingDateTo"]))
                postingDateTo = DateTime.Parse(Request.QueryString["postingDateTo"]);

            //Call Service
            List<ARSystemService.vmChecklistInvoiceBuilding> list = new List<ARSystemService.vmChecklistInvoiceBuilding>();
            using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
            {
                int intTotalRecord = client.GetChecklistInvoiceBuildingCount(UserManager.User.UserToken, companyName, invoiceTypeID, status, postingDateFrom.Value, postingDateTo.Value, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vmChecklistInvoiceBuilding> listHolder = new List<ARSystemService.vmChecklistInvoiceBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetChecklistInvoiceBuildingToList(UserManager.User.UserToken, companyName, invoiceTypeID, status, postingDateFrom.Value, postingDateTo.Value, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "InvNo",
                "InvTemp",
                "Company",
                "CompanyType",
                "StartPeriod",
                "EndPeriod",
                "InvPrintDate",
                "PostedBy",
                "PostedDate",
                "Term",
                "Currency",
                "InvTotalAmount",
                "InvTotalAPPN",
                "Discount",
                "InvTotalPenalty",
                "TaxInvoiceNo"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ChecklistBuildingInvoices", table);
        }

        [Route("TrxChecklistInvoiceTower/Export")]
        public void GetTrxChecklistInvoiceTowerToExport()
        {
            //Parameter
            string companyId = Request.QueryString["companyName"];
            string invoiceTypeID = Request.QueryString["invoiceTypeID"];
            string operatorId = Request.QueryString["operatorId"];
            DateTime? postingDateFrom = DateTime.MinValue;
            DateTime? postingDateTo = DateTime.MaxValue;
            string status = Request.QueryString["status"];
            string invNo = Request.QueryString["invNo"];

            if (!string.IsNullOrEmpty(Request.QueryString["postingDateFrom"]))
                postingDateFrom = DateTime.Parse(Request.QueryString["postingDateFrom"]);

            if (!string.IsNullOrEmpty(Request.QueryString["postingDateTo"]))
                postingDateTo = DateTime.Parse(Request.QueryString["postingDateTo"]);

            //Call Service
            vwChecklistInvoiceTower param = new vwChecklistInvoiceTower();
            param.InvCompanyId = companyId;
            param.InvoiceTypeId = invoiceTypeID;
            param.InvOperatorID = operatorId;
            param.Status = status;
            param.InvNo = invNo;

            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            List<vwChecklistInvoiceTower> list = new List<vwChecklistInvoiceTower>();
            list = _CheckListservices.GetDataCheckList(userCredential.UserID, param, postingDateFrom.Value, postingDateTo.Value).ToList();

            //Convert to DataTable
            string[] fieldList = new string[] {
                "InvNo",
                "InvPrintDate",
                "InvTemp",
                "PostedBy",
                "PostedDate",
                "Term",
                "VerificationStatus",
                "Company",
                "Operator",
                "InvTotalAmount",
                "InvTotalAPPN",
                "Currency",
                "TaxInvoiceNo",
                "Remark",
                "VerificationDate",
                "ChecklistARData"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ChecklistTowerInvoices", table);
        }

        //[Route("TrxCNInvoiceBuilding/Export")]
        //public void GetTrxCNInvoiceBuildingToExport()
        //{
        //    //Parameter
        //    string companyName = Request.QueryString["companyName"];
        //    string invoiceTypeID = Request.QueryString["invoiceTypeID"];
        //    string invCompanyId = Request.QueryString["invCompanyId"];
        //    string invNo = Request.QueryString["invNo"];

        //    //Call Service
        //    List<ARSystemService.vmCNInvoiceBuilding> list = new List<ARSystemService.vmCNInvoiceBuilding>();
        //    using (var client = new ARSystemService.ItrxCNInvoiceBuildingServiceClient())
        //    {
        //        int intTotalRecord = client.GetCNInvoiceBuildingCount(UserManager.User.UserToken, invoiceTypeID, companyName, invCompanyId, invNo);
        //        int intBatch = intTotalRecord / 50;
        //        List<ARSystemService.vmCNInvoiceBuilding> listHolder = new List<ARSystemService.vmCNInvoiceBuilding>();

        //        for (int i = 0; i <= intBatch; i++)
        //        {
        //            listHolder = client.GetCNInvoiceBuildingToList(UserManager.User.UserToken, invoiceTypeID, companyName, invCompanyId, invNo, null, 50 * i, 50).ToList();
        //            list.AddRange(listHolder);
        //        }
        //    }

        //    //Convert to DataTable
        //    string[] fieldList = new string[] {
        //        "InvNo",
        //        "InvTemp",
        //        "Company",
        //        "CompanyType",
        //        "InvPrintDate",
        //        "PostedBy",
        //        "PostingDate",
        //        "Term",
        //        "Currency",
        //        "InvTotalAmount",
        //        "InvTotalAPPN",
        //        "Discount",
        //        "InvTotalPenalty",
        //        "TaxInvoiceNo"
        //    };
        //    DataTable table = new DataTable();
        //    var reader = FastMember.ObjectReader.Create(list, fieldList);
        //    table.Load(reader);

        //    //Export to Excel
        //    ExportToExcelHelper.Export("CNBuildingInvoices", table);
        //}

        //[Route("TrxCNInvoiceTower/Export")]
        //public void GetTrxCNInvoiceTowerToExport()
        //{
        //    //Parameter
        //    string companyId = Request.QueryString["companyId"];
        //    string invoiceTypeID = Request.QueryString["invoiceTypeID"];
        //    string operatorId = Request.QueryString["operatorId"];
        //    string invNo = Request.QueryString["invNo"];

        //    //Call Service
        //    List<ARSystemService.vmCNInvoiceTower> list = new List<ARSystemService.vmCNInvoiceTower>();
        //    using (var client = new ARSystemService.ItrxCNInvoiceTowerServiceClient())
        //    {
        //        int intTotalRecord = client.GetCNInvoiceTowerCount(UserManager.User.UserToken, invoiceTypeID, companyId, operatorId, invNo);
        //        int intBatch = intTotalRecord / 50;
        //        List<ARSystemService.vmCNInvoiceTower> listHolder = new List<ARSystemService.vmCNInvoiceTower>();

        //        for (int i = 0; i <= intBatch; i++)
        //        {
        //            listHolder = client.GetCNInvoiceTowerToList(UserManager.User.UserToken, invoiceTypeID, companyId, operatorId, invNo, null, 50 * i, 50).ToList();
        //            list.AddRange(listHolder);
        //        }
        //    }

        //    //Convert to DataTable
        //    string[] fieldList = new string[] {
        //        "InvNo",
        //        "InvTemp",
        //        "CompanyTBG",
        //        "OperatorDesc",
        //        "PostedBy",
        //        "PostingDate",
        //        "Term",
        //        "Currency",
        //        "InvSumADPP",
        //        "InvTotalAPPN",
        //        "Discount",
        //        "InvTotalPenalty",
        //        "InvPrintDate"
        //    };
        //    DataTable table = new DataTable();
        //    var reader = FastMember.ObjectReader.Create(list, fieldList);
        //    table.Load(reader);

        //    //Export to Excel
        //    ExportToExcelHelper.Export("CNTowerInvoices", table);
        //}

        [Route("TrxPrintInvoiceBuildingDetailCalculation/Export")]
        public void GetTrxPrintInvoiceBuildingDetailCalculationToExport()
        {
            //Parameter
            string strHeaderId = Request.QueryString["HeaderId"];
            int[] HeaderId = strHeaderId.Split('|').Select(Int32.Parse).ToArray();

            //Call Service
            List<ARSystemService.vwPrintInvoiceBuilding> listDataDetailCalculationPostedInvoice = new List<ARSystemService.vwPrintInvoiceBuilding>();

            using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
            {
                int intTotalRecord = client.GetInvoiceBuildingDetailListDataCount(UserManager.User.UserToken, HeaderId);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwPrintInvoiceBuilding> listHolder = new List<ARSystemService.vwPrintInvoiceBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetInvoiceBuildingDetailListDataToList(UserManager.User.UserToken, HeaderId, null, 50 * i, 50).ToList();
                    listDataDetailCalculationPostedInvoice.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {
                "InvNo", "InvPrintDate", "Term", "CompanyTBG", "Company",
                "InvTotalAmount", "Discount", "InvTotalAPPN", "InvTotalPenalty", "Currency", "StartPeriod", "EndPeriod", "InvPaidStatus","PPHType"
            };
            var reader = FastMember.ObjectReader.Create(listDataDetailCalculationPostedInvoice.Select(i => new
            {
                InvNo = i.InvNo,
                InvPrintDate = i.InvPrintDate,
                Term = i.Term,
                CompanyTBG = i.CompanyTBG,
                Company = i.CompanyType + " " + i.Company,
                InvTotalAmount = i.InvTotalAmount,
                Discount = i.Discount,
                InvTotalAPPN = i.InvTotalAPPN,
                InvTotalPenalty = i.InvTotalPenalty,
                Currency = i.Currency,
                StartPeriod = i.StartPeriod,
                EndPeriod = i.EndPeriod,
                InvPaidStatus = (!string.IsNullOrEmpty(i.InvPaidStatus) ? "Paid" : "Unpaid"),
                PPHType = i.PPHType
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("DetailCalculation", table);

        }

        [Route("TrxPrintInvoiceTowerDetailCalculation/Export")]
        public void GetTrxPrintInvoiceTowerDetailCalculationToExport()
        {
            //Parameter
            string strHeaderId = Request.QueryString["HeaderId"];
            string strCategoryId = Request.QueryString["CategoryId"];
            int[] HeaderId = strHeaderId.Split('|').Select(Int32.Parse).ToArray();
            int[] CategoryId = strCategoryId.Split('|').Select(Int32.Parse).ToArray();
            //Call Service
            List<ARSystemService.vwDataPostedInvoiceTowerDetailCalculation> listDataDetailCalculationPostedInvoice = new List<ARSystemService.vwDataPostedInvoiceTowerDetailCalculation>();

            using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
            {
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = HeaderId;
                vm.CategoryId = CategoryId;

                int intTotalRecord = client.GetInvoiceDetailListDataCount(UserManager.User.UserToken, vm);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwDataPostedInvoiceTowerDetailCalculation> listDataDetailPostedInvoiceHolder = new List<ARSystemService.vwDataPostedInvoiceTowerDetailCalculation>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataDetailPostedInvoiceHolder = client.GetInvoiceDetailListDataToList(UserManager.User.UserToken, vm, null, 50 * i, 50).ToList();
                    listDataDetailCalculationPostedInvoice.AddRange(listDataDetailPostedInvoiceHolder);
                }
            }
            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {
                "Company", "Operator", "SONumber","SiteIDOperator","StartSewa",
                "EndSewa","PONumber","SiteNameOperator","Type","StartTagih","EndTagih","PricePerMonth","TotalPO",
                "TarifPemotonganPajak","InvoiceNumber"
            };
            var reader = FastMember.ObjectReader.Create(listDataDetailCalculationPostedInvoice.Select(i => new
            {

                Company = i.InvCompanyId,
                Operator = i.InvOperatorID,
                SONumber = i.SONumber,
                SiteIDOperator = i.SiteIdOpr,
                StartSewa = string.IsNullOrEmpty(i.StartDateRent.ToString()) ? "" : DateTime.Parse(i.StartDateRent.ToString()).ToString("dd-MMM-yyyy"),
                EndSewa = string.IsNullOrEmpty(i.EndDateRent.ToString()) ? "" : DateTime.Parse(i.EndDateRent.ToString()).ToString("dd-MMM-yyyy"),
                PONumber = i.PoNumber,
                SiteNameOperator = i.SiteNameOpr,
                Type = i.JobType,
                StartTagih = string.IsNullOrEmpty(i.StartDateperiod.ToString()) ? "" : DateTime.Parse(i.StartDateperiod.ToString()).ToString("dd-MMM-yyyy"),
                EndTagih = string.IsNullOrEmpty(i.EndDatePeriod.ToString()) ? "" : DateTime.Parse(i.EndDatePeriod.ToString()).ToString("dd-MMM-yyyy"),
                PricePerMonth = i.PricePerMonth,
                TotalPO = i.TotalPO,
                TarifPemotonganPajak = i.PPHType,
                InvoiceNumber = i.InvNo


            }), ColumsShow);
            table.Load(reader);

            try
            {
                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                IXLWorksheet ws = wbook.Worksheets.Add("Sheet 1");


                // From a DataTable
                var dataTable = table;
                decimal SUMTotalPO = 0;
                IXLTable tableWithData = ws.Cell(1, 1).InsertTable(dataTable.AsEnumerable());
                for (int i = 1; i <= table.Columns.Count; i++)
                {
                    ws.Column(i).AdjustToContents();
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    SUMTotalPO += decimal.Parse(table.Rows[i]["TotalPO"].ToString());
                }
                var TotalLabelIndex = table.Rows.Count + 2;

                ws.Cell(TotalLabelIndex, 12).Value = "TOTAL";
                ws.Cell(TotalLabelIndex, 12).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                ws.Cell(TotalLabelIndex, 12).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                ws.Cell(TotalLabelIndex, 12).Style.Border.RightBorder = XLBorderStyleValues.Thick;
                ws.Cell(TotalLabelIndex, 12).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                ws.Cell(TotalLabelIndex, 12).Style.Font.Bold = true;
                ws.Cell(TotalLabelIndex, 13).Value = SUMTotalPO;
                ws.Cell(TotalLabelIndex, 13).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                ws.Cell(TotalLabelIndex, 13).Style.Border.TopBorder = XLBorderStyleValues.Thick;
                ws.Cell(TotalLabelIndex, 13).Style.Border.RightBorder = XLBorderStyleValues.Thick;
                ws.Cell(TotalLabelIndex, 13).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                ws.Cell(TotalLabelIndex, 13).Style.Font.Bold = true;


                HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=DetailCalculation.xlsx");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
            catch (Exception ex)
            {

            }

            //Export to Excel
            //ExportToExcelHelper.Export("DetailCalculation", table);

        }

        [Route("TrxManageMergedInvoiceOnlyTower/Export")]
        public void TrxManageMergedInvoiceOnlyTowerToExport()
        {
            string operatorId = Request.QueryString["operatorId"];
            string companyId = Request.QueryString["companyId"];
            string invNo = Request.QueryString["invNo"];

            List<ARSystemService.vwMergedInvoiceOnlyTower> list = new List<ARSystemService.vwMergedInvoiceOnlyTower>();

            using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
            {
                int intTotalRecord = client.GetMergedInvoiceTowerOnlyCount(UserManager.User.UserToken, companyId, operatorId, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwMergedInvoiceOnlyTower> listHolder = new List<ARSystemService.vwMergedInvoiceOnlyTower>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMergedInvoiceTowerOnlyToList(UserManager.User.UserToken, companyId, operatorId, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            string[] fieldList = new string[] {
                "InvNo", "Company", "Operator", "InvTotalAmount", "Discount", "InvTotalAPPN", "InvTotalPenalty", "Currency","PrintUsers"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            ExportToExcelHelper.Export("MergedInvoiceTower", table);
        }

        [Route("TrxManageMergedInvoiceDetailTower/Export")]
        public void TrxManageMergedInvoiceDetailTowerToExport()
        {
            string operatorId = Request.QueryString["operatorId"];
            string companyId = Request.QueryString["companyId"];
            string invNo = Request.QueryString["invNo"];

            List<ARSystemService.vwMergedInvoiceDetailTower> list = new List<ARSystemService.vwMergedInvoiceDetailTower>();

            using (var client = new ARSystemService.ItrxManageMergedInvoiceDetailTowerServiceClient())
            {
                int intTotalRecord = client.GetMergedInvoiceTowerDetailCount(UserManager.User.UserToken, companyId, operatorId, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwMergedInvoiceDetailTower> listHolder = new List<ARSystemService.vwMergedInvoiceDetailTower>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMergedInvoiceTowerDetailToList(UserManager.User.UserToken, companyId, operatorId, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            string[] fieldList = new string[] {
                "InvNo", "Company", "Operator", "InvTotalAmount", "Discount", "InvTotalAPPN", "InvTotalPenalty", "Currency"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            ExportToExcelHelper.Export("MergedInvoiceTower", table);
        }

        [Route("TrxManageMergedInvoiceOnlyBuilding/Export")]
        public void TrxManageMergedInvoiceOnlyBuildingToExport()
        {
            string customerName = Request.QueryString["customerName"];
            string companyId = Request.QueryString["companyId"];
            string invNo = Request.QueryString["invNo"];

            List<ARSystemService.vwMergedInvoiceOnlyBuilding> list = new List<ARSystemService.vwMergedInvoiceOnlyBuilding>();

            using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
            {
                int intTotalRecord = client.GetMergedInvoiceBuildingOnlyCount(UserManager.User.UserToken, companyId, customerName, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwMergedInvoiceOnlyBuilding> listHolder = new List<ARSystemService.vwMergedInvoiceOnlyBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMergedInvoiceBuildingOnlyToList(UserManager.User.UserToken, companyId, customerName, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            string[] fieldList = new string[] {
                "InvNo", "Company", "CustomerName", "Company", "InvTotalAmount", "Discount", "InvTotalAPPN", "InvTotalPenalty", "Currency","PrintUsers"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            ExportToExcelHelper.Export("MergedInvoiceBuilding", table);
        }

        [Route("TrxManageMergedInvoiceDetailBuilding/Export")]
        public void TrxManageMergedInvoiceDetailBuildingToExport()
        {
            string customerName = Request.QueryString["customerName"];
            string companyId = Request.QueryString["companyId"];
            string invNo = Request.QueryString["invNo"];

            List<ARSystemService.vwMergedInvoiceDetailBuilding> list = new List<ARSystemService.vwMergedInvoiceDetailBuilding>();

            using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
            {
                int intTotalRecord = client.GetMergedInvoiceBuildingDetailCount(UserManager.User.UserToken, companyId, customerName, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwMergedInvoiceDetailBuilding> listHolder = new List<ARSystemService.vwMergedInvoiceDetailBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetMergedInvoiceBuildingDetailToList(UserManager.User.UserToken, companyId, customerName, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            string[] fieldList = new string[] {
                "InvNo", "InvParentNo", "Company", "CustomerName", "InvTotalAmount", "Discount", "InvTotalAPPN", "InvTotalPenalty", "Currency"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            ExportToExcelHelper.Export("MergedInvoiceBuildingDetail", table);
        }

        [Route("TrxReportInvoiceTowerToAX/Export")]
        public void TrxReportInvoiceTowerToAXExport()
        {
            //Parameter
            string strHeaderId = Request.QueryString["HeaderId"];
            string strCategoryId = Request.QueryString["CategoryId"];
            string strisCNInvoice = Request.QueryString["isCNInvoice"];
            int[] HeaderId = strHeaderId.Split('|').Select(Int32.Parse).ToArray();
            int[] CategoryId = strCategoryId.Split('|').Select(Int32.Parse).ToArray();
            int[] isCNInvoice = strisCNInvoice.Split('|').Select(Int32.Parse).ToArray();

            //Call Service
            List<ARSystemService.vwReportInvoiceTowerByInvoice> listDataReportInvoice = new List<ARSystemService.vwReportInvoiceTowerByInvoice>();

            using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
            {
                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = HeaderId;
                vm.CategoryId = CategoryId;
                vm.isCNInvoice = isCNInvoice;
                listDataReportInvoice = client.GetReportInvoiceListDataToList(UserManager.User.UserToken, vm).ToList();
            }


            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"TransactionDate",
                "Voucher", "AccountType", "Operator", "TransactionText", "Debit",
                "Credit","Currency", "Xrate", "DocNumber", "SONumber", "DocDate","DueDate","InvoiceID",
                 "PostingProfile", "OffsetAccount", "TaxGroup","TaxItemGroup","FPJNumber","FPJDate"
            };
            var reader = FastMember.ObjectReader.Create(listDataReportInvoice.Select(i => new
            {
                TransactionDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                Voucher = i.InvTemp,
                AccountType = i.AccountType,
                Operator = i.InvOperatorID,
                TransactionText = i.InvSubject,
                Debit = i.InvSumADPP,
                Credit = i.Credit,
                i.Currency,
                i.Xrate,
                i.DocNumber,
                i.SONumber,
                DocDate = i.InvPrintDate,
                i.DueDate,
                InvoiceID = i.InvNo,
                i.PostingProfile,
                OffsetAccount = i.OffSetAccount,
                i.TaxGroup,
                i.TaxItemGroup,
                FPJNumber = i.TaxInvoiceNo,
                FPJDate = string.IsNullOrEmpty(i.FPJDate.ToString()) ? "" : DateTime.Parse(i.FPJDate.ToString()).ToString("dd-MMM-yyyy")
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("InvoiceToAX", table);

        }

        [Route("TrxReportInvoiceTower/Export")]
        public void TrxReportInvoiceTowerToExport()
        {
            //Parameter
            int intYearPosting = int.Parse(Request.QueryString["intYearPosting"]);
            int intMonthPosting = int.Parse(Request.QueryString["intMonthPosting"]);
            int intWeekPosting = int.Parse(Request.QueryString["intWeekPosting"]);
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string invNo = Request.QueryString["invNo"];
            string strCompanyCode = Request.QueryString["strCompanyCode"];
            //Call Service
            List<ARSystemService.vwReportInvoiceTowerByInvoice> listDataPostedInvoice = new List<ARSystemService.vwReportInvoiceTowerByInvoice>();

            using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetTrxReportInvoiceTowerCount(UserManager.User.UserToken, strStartPeriod, strEndPeriod,strCompanyCode, intYearPosting, intMonthPosting, intWeekPosting, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwReportInvoiceTowerByInvoice> listDataPostedInvoiceHolder = new List<ARSystemService.vwReportInvoiceTowerByInvoice>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataPostedInvoiceHolder = client.GetTrxReportInvoiceTowerToList(UserManager.User.UserToken, strStartPeriod, strEndPeriod,strCompanyCode, null, intYearPosting, intMonthPosting, intWeekPosting, invNo, 50 * i, 50).ToList();
                    listDataPostedInvoice.AddRange(listDataPostedInvoiceHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvoiceID",
                "TransactionText", "Debit", "CompanyInvoice", "CompanyReal", "TransactionDate",
                "Voucher", "AccountType", "InvoiceCategory", "ElectricityCategory","Operator",
                "Credit","Currency","Xrate","DocNumber","DocDate","DueDate","PostingProfile","OffsetAccount",
                "TaxGroup","TaxItemGroup","FPJNumber","FPJDate","CreatedDate","OperatorRegion","Address","Status"
            };
            var reader = FastMember.ObjectReader.Create(listDataPostedInvoice.Select(i => new
            {
                InvoiceID = i.InvNo,
                TransactionText = i.InvSubject,
                Debit = i.InvSumADPP,
                CompanyInvoice = i.InvCompanyId,
                CompanyReal = i.CompanyIdAx,
                TransactionDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                Voucher = i.InvTemp,
                i.AccountType,
                i.InvoiceCategory,
                i.ElectricityCategory,
                Operator = i.InvOperatorID,
                i.Credit,
                i.Currency,
                i.Xrate,
                i.DocNumber,
                DocDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                i.DueDate,
                i.PostingProfile,
                OffsetAccount = i.OffSetAccount,
                i.TaxGroup,
                i.TaxItemGroup,
                FPJNumber = i.TaxInvoiceNo,
                FPJDate = string.IsNullOrEmpty(i.FPJDate.ToString()) ? "" : DateTime.Parse(i.FPJDate.ToString()).ToString("dd-MMM-yyyy"),
                CreatedDate = i.PostingDate,
                i.OperatorRegion,
                i.Address,
                i.Status
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxReportInvoiceTower", table);

        }

        [Route("TrxReportInvoiceTowerSONumber/Export")]
        public void TrxReportInvoiceTowerBySONumberToExport()
        {
            //Parameter
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string invNo = Request.QueryString["invNo"];
            string strCompanyCode = Request.QueryString["strCompanyCode"];
            //Call Service
            List<ARSystemService.vwReportInvoiceTowerBySoNumber> listDataInvoiceBySONumber = new List<ARSystemService.vwReportInvoiceTowerBySoNumber>();

            using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetTrxReportInvoiceTowerBySONumberCount(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyCode, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwReportInvoiceTowerBySoNumber> listDataInvoiceBySONumberHolder = new List<ARSystemService.vwReportInvoiceTowerBySoNumber>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataInvoiceBySONumberHolder = client.GetTrxReportInvoiceTowerBySONumberToList(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyCode,invNo, null, 50 * i, 50).ToList();
                    listDataInvoiceBySONumber.AddRange(listDataInvoiceBySONumberHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"SONumber",
                "SiteID", "SiteName", "InvoiceID", "TransactionText", "StartPeriod",
                "EndPeriod", "Debit", "CompanyInvoice", "CompanyReal","Voucher",
                "Type","CategoryInvoice","Operator","Credit","Currency","SLDDate","BAPSReceiveDate","BAPSDate",
                "CreateDate","InvoiceDate","DueDate","PostingProfile","OffsetAccount","TaxGroup","TaxItemGroup","FPJNumber",
                "FPJDate","BAPSConfirmDate","BAPSPostingDate","BAPSPrintDate","BAPSReceiptDate","LeadTimeBAPSConfirm",
                "LeadTimeVerificator","LeadTimeInputer","LeadTimeFinishing","LeadTimeARDataDept"
            };
            var reader = FastMember.ObjectReader.Create(listDataInvoiceBySONumber.Select(i => new
            {
                i.SONumber,
                SiteID = i.SiteIdOld,
                i.SiteName,
                InvoiceID = i.InvNo,
                TransactionText = i.InvSubject,
                StartPeriod = DateTime.Parse(i.StartDatePeriod.ToString()).ToString("dd-MMM-yyyy"),
                EndPeriod = DateTime.Parse(i.EndDatePeriod.ToString()).ToString("dd-MMM-yyyy"),
                Debit = i.InvSumADPP,
                CompanyInvoice = i.InvCompanyId,
                CompanyReal = i.CompanyIdAx,
                Voucher = i.InvTemp,
                Type = i.Description,
                CategoryInvoice = i.InvoiceCategory,
                Operator = i.InvOperatorID,
                i.Credit,
                i.Currency,
                SLDDate = "",//DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),//change to sld
                BAPSReceiveDate = DateTime.Parse(i.BapsReceiveDate.ToString()).ToString("dd-MMM-yyyy"),
                BAPSDate = DateTime.Parse(i.BapsDone.ToString()).ToString("dd-MMM-yyyy"),
                CreateDate = DateTime.Parse(i.CreatedDate.ToString()).ToString("dd-MMM-yyyy"),
                InvoiceDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                DueDate = DateTime.Parse(i.DueDate.ToString()).ToString("dd-MMM-yyyy"),
                PostingProfile = i.PostingProfile,
                OffsetAccount = i.OffSetAccount,
                i.TaxGroup,
                i.TaxItemGroup,
                FPJNumber = i.TaxInvoiceNo,
                FPJDate = string.IsNullOrEmpty(i.FPJDate.ToString()) ? "" : DateTime.Parse(i.FPJDate.ToString()).ToString("dd-MMM-yyyy"),
                BAPSConfirmDate = DateTime.Parse(i.BapsConfirmDate.ToString()).ToString("dd-MMM-yyyy"),
                BAPSPostingDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),// is it really invoice date? existing is
                BAPSPrintDate = string.IsNullOrEmpty(i.InvFirstPrintDate.ToString()) ? "" : DateTime.Parse(i.InvFirstPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                BAPSReceiptDate = string.IsNullOrEmpty(i.InvReceiptDate.ToString()) ? "" : DateTime.Parse(i.InvReceiptDate.ToString()).ToString("dd-MMM-yyyy"),
                LeadTimeBAPSConfirm = i.LeadTimeVerificator, //existing lead time verficator
                LeadTimeVerificator = i.LeadTimeVerificator,
                LeadTimeInputer = i.LeadTimeInputer,
                LeadTimeFinishing = i.LeadTimeFinishing, //LeadTimeFinishing
                LeadTimeARDataDept = i.LeadTimeARData //LeadTimeAR
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxReportInvoiceTowerBySONumber", table);

        }

        [Route("TrxReportInvoiceBuilding/Export")]
        public void TrxReportInvoiceBuildingToExport()
        {
            //Parameter
            int year = 0;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["year"]))
                year = int.Parse(Request.QueryString["year"]);

            int month = 0;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["month"]))
                month = int.Parse(Request.QueryString["month"]);

            int week = 0;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["week"]))
                week = int.Parse(Request.QueryString["week"]);

            DateTime invPrintDateFrom = DateTime.MinValue;
            DateTime invPrintDateTo = DateTime.MaxValue;

            string strInvPrintDateFrom = Request.QueryString["invPrintDateFrom"];
            string strInvPrintDateTo = Request.QueryString["invPrintDateTo"];
            string invNo = Request.QueryString["invNo"];

            if (!string.IsNullOrWhiteSpace(strInvPrintDateFrom))
                invPrintDateFrom = DateTime.Parse(strInvPrintDateFrom);

            if (!string.IsNullOrWhiteSpace(strInvPrintDateTo))
                invPrintDateTo = DateTime.Parse(strInvPrintDateTo);

            //Call Service
            List<ARSystemService.vwReportInvoiceBuilding> list = new List<ARSystemService.vwReportInvoiceBuilding>();

            using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
            {
                int intTotalRecord = client.GetReportInvoiceBuildingCount(UserManager.User.UserToken, invPrintDateFrom, invPrintDateTo, year, month, week, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwReportInvoiceBuilding> listHolder = new List<ARSystemService.vwReportInvoiceBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetReportInvoiceBuildingToList(UserManager.User.UserToken, invPrintDateFrom, invPrintDateTo, year, month, week, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvoiceNo",
                "DescriptionInvoice", "InvoiceDate", "PostingDate", "CompanyTBG", "CustomerName",
                "Area", "PricePerMeter", "Term", "Currency","DPPAmount",
                "Discount","PPN","Penalty"
            };
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                InvoiceNo = i.InvNo,
                DescriptionInvoice = i.InvSubject,
                InvoiceDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                PostingDate = DateTime.Parse(i.PostingDate.ToString()).ToString("dd-MMM-yyyy"),
                CompanyTBG = i.Company,
                CustomerName = i.CustomerName,
                Area = i.Area,
                PricePerMeter = i.MeterPrice,
                Term = i.TermPeriod,
                Currency = i.Currency,
                DPPAmount = i.InvSumADPP,
                Discount = i.Discount,
                PPN = i.InvTotalAPPN,
                Penalty = i.InvTotalPenalty
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxReportInvoiceBuilding", table);

        }

        [Route("TrxARProcessInvoiceTower/Export")]
        public void TrxARProcessInvoiceTowerToExport()
        {
            //Parameter
            string term = Request.QueryString["invoiceTypeId"];
            string invOperatorId = Request.QueryString["invOperatorId"];
            string invCompanyId = Request.QueryString["invCompanyId"];
            string invNo = Request.QueryString["invNo"];
            string StatusReceipt = Request.QueryString["StatusReceipt"];

            //Call Service
            List<ARSystemService.vmARProcessInvoiceTower> list = new List<ARSystemService.vmARProcessInvoiceTower>();
            using (var client = new ARSystemService.ItrxARProcessInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetARProcessInvoiceTowerCount(UserManager.User.UserToken, term, invOperatorId, invCompanyId, invNo, StatusReceipt);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vmARProcessInvoiceTower> listHolder = new List<ARSystemService.vmARProcessInvoiceTower>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetARProcessInvoiceTowerToList(UserManager.User.UserToken, term, invOperatorId, invCompanyId, invNo, StatusReceipt, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"NoInvoice","InvoiceDate","NoInvoiceTemp","InvoiceType","Company","OperatorID",
                "Amount","Discount","PPN","Penalty","Currency","ReceiptDate","StatusReceipt","InvoiceStatus","StatusProgress","Status",
                "DownloadFile","ChecklistDocDate","PPHType","AmountLossPPN"
            };
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                NoInvoice = i.InvNo,
                InvoiceDate = string.IsNullOrEmpty(i.InvPrintDate.ToString()) ? "" : DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                NoInvoiceTemp = i.InvTemp,
                InvoiceType = i.Term,
                Company = i.InvCompanyId,
                OperatorID = i.InvOperatorID,
                Amount = i.InvSumADPP,
                Discount = i.Discount,
                PPN = i.InvTotalAPPN,
                Penalty = i.InvTotalPenalty,
                Currency = i.Currency,
                ReceiptDate = string.IsNullOrEmpty(i.InvReceiptDate.ToString()) ? "" : DateTime.Parse(i.InvReceiptDate.ToString()).ToString("dd-MMM-yyyy"),
                StatusReceipt = i.StatusReceipt,
                InvoiceStatus = "Reguler",
                StatusProgress = "",
                Status = "",
                DownloadFile = "",
                ChecklistDocDate = string.IsNullOrEmpty(i.ChecklistDate.ToString()) ? "" : DateTime.Parse(i.ChecklistDate.ToString()).ToString("dd-MMM-yyyy"),
                PPHType = i.PPHType,
                AmountLossPPN = i.InvAmountLossPPN

            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ARProcessInvoiceTower", table);
        }

        [Route("TrxARProcessInvoiceTowerHistory/Export")]
        public void TrxARProcessInvoiceTowerHistoryToExport()
        {
            //Parameter
            string term = Request.QueryString["invoiceTypeId"];
            string invOperatorId = Request.QueryString["invOperatorId"];
            string invCompanyId = Request.QueryString["invCompanyId"];
            string invNo = Request.QueryString["invNo"];
            string strReceiptDateFrom = Request.QueryString["receiptDateFrom"];
            string strReceiptDateTo = Request.QueryString["receiptDateTo"];
            DateTime? receiptDateFrom = null;
            DateTime? receiptDateTo = null;

            if (!string.IsNullOrEmpty(strReceiptDateFrom))
                receiptDateFrom = DateTime.Parse(strReceiptDateFrom);

            if (!string.IsNullOrEmpty(strReceiptDateTo))
                receiptDateTo = DateTime.Parse(strReceiptDateTo);

            //Call Service
            List<ARSystemService.vwARProcessInvoiceTowerHistory> list = new List<ARSystemService.vwARProcessInvoiceTowerHistory>();
            using (var client = new ARSystemService.ItrxARProcessInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetARProcessInvoiceTowerHistoryCount(UserManager.User.UserToken, term, invOperatorId, invCompanyId, invNo, receiptDateFrom, receiptDateTo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwARProcessInvoiceTowerHistory> listHolder = new List<ARSystemService.vwARProcessInvoiceTowerHistory>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetARProcessInvoiceTowerHistoryToList(UserManager.User.UserToken, term, invOperatorId, invCompanyId, invNo, receiptDateFrom, receiptDateTo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "InvNo",
                "Company",
                "Operator",
                "ReceiptDate",
                "Remark",
                "ARProcessPenalty",
                "CreatedDate",
                "CreatedBy"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ARProcessInvoiceTowerHistory", table);
        }

        [Route("TrxARProcessInvoiceBuilding/Export")]
        public void TrxARProcessInvoiceBuildingToExport()
        {
            //Parameter
            string term = Request.QueryString["invoiceTypeId"];
            string customerName = Request.QueryString["customerName"];
            string invCompanyId = Request.QueryString["invCompanyId"];
            string invNo = Request.QueryString["invNo"];
            string StatusReceipt = Request.QueryString["StatusReceipt"];

            //Call Service
            List<ARSystemService.vmARProcessInvoiceBuilding> list = new List<ARSystemService.vmARProcessInvoiceBuilding>();
            using (var client = new ARSystemService.ItrxARProcessInvoiceBuildingServiceClient())
            {
                int intTotalRecord = client.GetARProcessInvoiceBuildingCount(UserManager.User.UserToken, term, invCompanyId, customerName, invNo, StatusReceipt);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vmARProcessInvoiceBuilding> listHolder = new List<ARSystemService.vmARProcessInvoiceBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetARProcessInvoiceBuildingToList(UserManager.User.UserToken, term, invCompanyId, customerName, invNo, StatusReceipt, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"NoInvoice","InvoiceDate","NoInvoiceTemp","InvoiceType","Company","OperatorID",
                "Amount","Discount","PPN","Penalty","Currency","ReceiptDate","StatusReceipt","InvoiceStatus","StatusProgress","Status",
                "DownloadFile","ChecklistDocDate","PPHType"
            };
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                NoInvoice = i.InvNo,
                InvoiceDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                NoInvoiceTemp = i.InvTemp,
                InvoiceType = i.Term,
                Company = i.InvCompanyId,
                OperatorID = i.InvOperatorID,
                Amount = i.InvSumADPP,
                Discount = i.Discount,
                PPN = i.InvTotalAPPN,
                Penalty = i.InvTotalPenalty,
                Currency = i.Currency,
                ReceiptDate = DateTime.Parse(i.InvReceiptDate.ToString()).ToString("dd-MMM-yyyy"),
                StatusReceipt = i.StatusReceipt,
                InvoiceStatus = "Reguler",
                StatusProgress = "",
                Status = "",
                DownloadFile = "",
                ChecklistDocDate = DateTime.Parse(i.ChecklistDate.ToString()).ToString("dd-MMM-yyyy"),
                PPHType = i.PPHType

            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ARProcessInvoiceBuilding", table);
        }

        [Route("TrxARProcessInvoiceBuildingHistory/Export")]
        public void TrxARProcessInvoiceBuildingHistoryToExport()
        {
            //Parameter
            string term = Request.QueryString["invoiceTypeId"];
            string invOperatorId = Request.QueryString["invOperatorId"];
            string invCompanyId = Request.QueryString["invCompanyId"];
            string invNo = Request.QueryString["invNo"];
            string strReceiptDateFrom = Request.QueryString["receiptDateFrom"];
            string strReceiptDateTo = Request.QueryString["receiptDateTo"];
            DateTime? receiptDateFrom = null;
            DateTime? receiptDateTo = null;

            if (!string.IsNullOrEmpty(strReceiptDateFrom))
                receiptDateFrom = DateTime.Parse(strReceiptDateFrom);

            if (!string.IsNullOrEmpty(strReceiptDateTo))
                receiptDateTo = DateTime.Parse(strReceiptDateTo);

            //Call Service
            List<ARSystemService.vwARProcessInvoiceBuildingHistory> list = new List<ARSystemService.vwARProcessInvoiceBuildingHistory>();
            using (var client = new ARSystemService.ItrxARProcessInvoiceBuildingServiceClient())
            {
                int intTotalRecord = client.GetARProcessInvoiceBuildingHistoryCount(UserManager.User.UserToken, term, invOperatorId, invCompanyId, invNo, receiptDateFrom, receiptDateTo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwARProcessInvoiceBuildingHistory> listHolder = new List<ARSystemService.vwARProcessInvoiceBuildingHistory>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetARProcessInvoiceBuildingHistoryToList(UserManager.User.UserToken, term, invOperatorId, invCompanyId, invNo, receiptDateFrom, receiptDateTo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "InvNo",
                "Company",
                "CustomerName",
                "ReceiptDate",
                "Remark",
                "ARProcessPenalty",
                "CreatedDate",
                "CreatedBy"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ARProcessInvoiceBuildingHistory", table);
        }

        [Route("TrxARPaymentInvoiceTowerToExport/Export")]
        public void TrxARPaymentInvoiceTowerToExport()
        {
            //Parameter
            string term = Request.QueryString["invoiceTypeId"];
            string Operator = Request.QueryString["Operator"];
            string invCompanyId = Request.QueryString["invCompanyId"];
            string invNo = Request.QueryString["invNo"];

            //Call Service
            List<ARSystemService.vmARPaymentInvoiceTower> list = new List<ARSystemService.vmARPaymentInvoiceTower>();
            using (var client = new ARSystemService.ItrxARPaymentInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetARPaymentInvoiceTowerCount(UserManager.User.UserToken, term, Operator, invCompanyId, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vmARPaymentInvoiceTower> listHolder = new List<ARSystemService.vmARPaymentInvoiceTower>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetARPaymentInvoiceTowerToList(UserManager.User.UserToken, term, Operator, invCompanyId, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvoiceNo",
                "InvoiceDate", "InvoiceTemp", "TermInvoice", "CompanyName", "OperatorID",
                "ReceiptDate", "AgingDays", "PICInternal", "PaidStatus","AmountTotal",
                "Currency","ChecklistDate","PPHType","AmountLossPPN","AmountPenalty"
            };
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                InvoiceNo = i.InvNo,
                InvoiceDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                InvoiceTemp = i.InvTemp,
                TermInvoice = i.Term,
                CompanyName = i.Company,
                OperatorID = i.InvOperatorID,
                ReceiptDate = (i.InvReceiptDate != null ? DateTime.Parse(i.InvReceiptDate.ToString()).ToString("dd-MMM-yyyy") : ""),
                i.AgingDays,
                PICInternal = i.InvInternalPIC,
                i.PaidStatus,
                AmountTotal = i.InvTotalAmount,
                i.Currency,
                ChecklistDate = (i.ChecklistDate != null ? DateTime.Parse(i.ChecklistDate.ToString()).ToString("dd-MMM-yyyy") : ""),
                PPHType = i.PPHType,
                AmountLossPPN = i.InvAmountLossPPN,
                AmountPenalty = i.InvTotalPenalty
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ARPaymentInvoiceTower", table);
        }

        [Route("TrxARPaymentInvoiceBuilding/Export")]
        public void TrxARPaymentInvoiceBuildingToExport()
        {
            //Parameter
            string term = Request.QueryString["invoiceTypeId"];
            string customerName = Request.QueryString["customerName"];
            string invCompanyId = Request.QueryString["invCompanyId"];
            string invNo = Request.QueryString["invNo"];

            //Call Service
            List<ARSystemService.vmARPaymentInvoiceBuilding> list = new List<ARSystemService.vmARPaymentInvoiceBuilding>();
            using (var client = new ARSystemService.ItrxARPaymentInvoiceBuildingServiceClient())
            {
                int intTotalRecord = client.GetARPaymentInvoiceBuildingCount(UserManager.User.UserToken, term, invCompanyId, customerName, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vmARPaymentInvoiceBuilding> listHolder = new List<ARSystemService.vmARPaymentInvoiceBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetARPaymentInvoiceBuildingToList(UserManager.User.UserToken, term, invCompanyId, customerName, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }


            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvoiceNo","InvoiceDate","InvoiceTemp","Term","CompanyName","CustomerName",
                "ReceiptDate","AgingDays","PICInternal","PaidStatus","AmountLossPPN","AmountPenalty","AmountTotal","Currency","ChecklistDate","PPHType"
            };
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                InvoiceNo = i.InvNo,
                InvoiceDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                InvoiceTemp = i.InvTemp,
                Term = i.Term,
                CompanyName = i.InvCompanyId,
                CustomerName = i.CustomerName,
                ReceiptDate = DateTime.Parse(i.InvReceiptDate.ToString()).ToString("dd-MMM-yyyy"),
                AgingDays = i.AgingDays,
                PICInternal = i.InvInternalPIC,
                PaidStatus = i.PaidStatus,
                AmountLossPPN = i.InvAmountLossPPN,
                AmountPenalty = i.InvTotalPenalty,
                AmountTotal = i.InvTotalAmount,
                Currency = i.Currency,
                ChecklistDate = DateTime.Parse(i.ChecklistDate.ToString()).ToString("dd-MMM-yyyy"),
                PPHType = i.PPHType

            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ARPaymentInvoiceBuilding", table);
        }

        [Route("TrxApprovalARMonitoringTowerInvoice/Export")]
        public void TrxApprovalARMonitoringTowerInvoiceToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string invNo = Request.QueryString["invNo"];

            //Call Service
            List<ARSystemService.vwApprovalARMonitoringInvoiceTower> listDataInvoice = new List<ARSystemService.vwApprovalARMonitoringInvoiceTower>();

            using (var client = new ARSystemService.ItrxApprovalARMonitoringTowerServiceClient())
            {
                int intTotalRecord = client.GetTrxApprovalARMonitoringInvoiceTowerCount(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyId, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwApprovalARMonitoringInvoiceTower> listDataInvoiceHolder = new List<ARSystemService.vwApprovalARMonitoringInvoiceTower>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataInvoiceHolder = client.GetTrxApprovalARMonitoringInvoiceTowerToList(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyId, invNo, null, 50 * i, 50).ToList();
                    listDataInvoice.AddRange(listDataInvoiceHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvoiceNo",
                "InvoiceTempNo", "PaidDate", "Company", "Operator", "Amount"
            };
            var reader = FastMember.ObjectReader.Create(listDataInvoice.Select(i => new
            {
                InvoiceNo = i.InvNo,
                InvoiceTempNo = i.InvTemp,
                PaidDate = DateTime.Parse(i.PaymentDate.ToString()).ToString("dd-MMM-yyyy"),
                Company = i.InvCompanyId,
                Operator = i.InvOperatorID,
                Amount = i.AmountPaid
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxApprovalARMonitoringTower", table);

        }

        [Route("TrxApprovalARMonitoringTowerCollection/Export")]
        public void TrxApprovalARMonitoringTowerCollectionToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string strStatusGenerate = Request.QueryString["strStatusGenerate"];
            string strInvNo = Request.QueryString["strInvNo"];

            //Call Service
            List<ARSystemService.vwApprovalARMonitoringCollectionTower> listDataInvoice = new List<ARSystemService.vwApprovalARMonitoringCollectionTower>();

            using (var client = new ARSystemService.ItrxApprovalARMonitoringTowerServiceClient())
            {
                int intTotalRecord = client.GetTrxApprovalARMonitoringCollectionTowerCount(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyId, strInvNo, strStatusGenerate);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwApprovalARMonitoringCollectionTower> listDataInvoiceHolder = new List<ARSystemService.vwApprovalARMonitoringCollectionTower>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataInvoiceHolder = client.GetTrxApprovalARMonitoringCollectionTowerToList(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyId, strInvNo, strStatusGenerate, null, 50 * i, 50).ToList();
                    listDataInvoice.AddRange(listDataInvoiceHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"CompanyAX","CompanyID", "TransactionDate", "Voucher", "AccountType", "AccountNumber",
                "TransactionText", "Debit", "Credit", "Notes", "Currency","Xrate","SONumber","DocNumber","DocDate","DueDate",
                "InvoiceID","PostingProfile","OffsetAccount","TaxGroup","TaxItem","FPJNumber","FPJDate","CreatedDate","TaxCode",
                "OffsetAccountType","JournalID","StatusGenerate"
            };
            var reader = FastMember.ObjectReader.Create(listDataInvoice.Select(i => new
            {
                CompanyAX = i.CompanyIdAx,
                CompanyID = i.CompanyId,
                TransactionDate = DateTime.Parse(i.TransDate.ToString()).ToString("dd-MMM-yyyy"),
                Voucher = i.VoucherNumber,
                AccountType = i.AccountType,
                AccountNumber = i.AccountNum,
                TransactionText = i.TransactionText,
                i.Debit,
                i.Credit,
                i.Notes,
                i.Currency,
                i.Xrate,
                i.SONumber,
                i.DocNumber,
                DocDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                DueDate = DateTime.Parse(i.DueDate.ToString()).ToString("dd-MMM-yyyy"),
                InvoiceID = i.InvNo,
                i.PostingProfile,
                OffsetAccount = i.OffSetAccount,
                i.TaxGroup,
                TaxItem = i.TaxGroup,
                FPJNumber = i.TaxInvoiceNo,
                FPJDate = string.IsNullOrEmpty(i.FPJDate.ToString()) ? "" : DateTime.Parse(i.FPJDate.ToString()).ToString("dd-MMM-yyyy"),
                CreatedDate = DateTime.Parse(i.CreatedDate.ToString()).ToString("dd-MMM-yyyy"),
                TaxCode = i.TaxGroup,
                OffsetAccountType = i.OffSetAccountType,
                JournalID = i.JournalId,
                StatusGenerate = i.StatusGenerate
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxApprovalARMonitoringTower", table);

        }

        [Route("TrxApprovalARMonitoringBuildingInvoice/Export")]
        public void TrxApprovalARMonitoringBuildingInvoiceToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string invNo = Request.QueryString["invNo"];

            //Call Service
            List<ARSystemService.vwApprovalARMonitoringInvoiceBuilding> listDataInvoice = new List<ARSystemService.vwApprovalARMonitoringInvoiceBuilding>();

            using (var client = new ARSystemService.ItrxApprovalARMonitoringBuildingServiceClient())
            {
                int intTotalRecord = client.GetTrxApprovalARMonitoringInvoiceBuildingCount(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyId, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwApprovalARMonitoringInvoiceBuilding> listDataInvoiceHolder = new List<ARSystemService.vwApprovalARMonitoringInvoiceBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataInvoiceHolder = client.GetTrxApprovalARMonitoringInvoiceBuildingToList(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyId, invNo, null, 50 * i, 50).ToList();
                    listDataInvoice.AddRange(listDataInvoiceHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvoiceNo",
                "InvoiceTempNo", "PaidDate", "Company", "Customer", "Amount"
            };
            var reader = FastMember.ObjectReader.Create(listDataInvoice.Select(i => new
            {
                InvoiceNo = i.InvNo,
                InvoiceTempNo = i.InvTemp,
                PaidDate = DateTime.Parse(i.PaymentDate.ToString()).ToString("dd-MMM-yyyy"),
                Company = i.InvCompanyId,
                Customer = i.CustomerName,
                Amount = i.AmountPaid
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxApprovalARMonitoringBuilding", table);

        }

        [Route("TrxApprovalARMonitoringBuildingCollection/Export")]
        public void TrxApprovalARMonitoringBuildingCollectionToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string strStatusGenerate = Request.QueryString["strStatusGenerate"];
            string strInvNo = Request.QueryString["strInvNo"];

            //Call Service
            List<ARSystemService.vwApprovalARMonitoringCollectionBuilding> listDataInvoice = new List<ARSystemService.vwApprovalARMonitoringCollectionBuilding>();

            using (var client = new ARSystemService.ItrxApprovalARMonitoringBuildingServiceClient())
            {
                int intTotalRecord = client.GetTrxApprovalARMonitoringCollectionBuildingCount(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyId, strInvNo, strStatusGenerate);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwApprovalARMonitoringCollectionBuilding> listDataInvoiceHolder = new List<ARSystemService.vwApprovalARMonitoringCollectionBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataInvoiceHolder = client.GetTrxApprovalARMonitoringCollectionBuildingToList(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyId, strInvNo, strStatusGenerate, null, 50 * i, 50).ToList();
                    listDataInvoice.AddRange(listDataInvoiceHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"CompanyAX","CompanyID", "TransactionDate", "Voucher", "AccountType", "AccountNumber",
                "TransactionText", "Debit", "Credit", "Notes", "Currency","Xrate","SONumber","DocNumber","DocDate","DueDate",
                "InvoiceID","PostingProfile","OffsetAccount","TaxGroup","TaxItem","FPJNumber","FPJDate","CreatedDate","TaxCode",
                "OffsetAccountType","JournalID","StatusGenerate"
            };
            var reader = FastMember.ObjectReader.Create(listDataInvoice.Select(i => new
            {
                CompanyAX = i.CompanyIdAx,
                CompanyID = i.CompanyId,
                TransactionDate = DateTime.Parse(i.TransDate.ToString()).ToString("dd-MMM-yyyy"),
                Voucher = i.VoucherNumber,
                AccountType = i.AccountType,
                AccountNumber = i.AccountNum,
                TransactionText = i.TransactionText,
                i.Debit,
                i.Credit,
                i.Notes,
                i.Currency,
                i.Xrate,
                i.SONumber,
                i.DocNumber,
                DocDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                DueDate = DateTime.Parse(i.DueDate.ToString()).ToString("dd-MMM-yyyy"),
                InvoiceID = i.InvNo,
                i.PostingProfile,
                OffsetAccount = i.OffSetAccount,
                i.TaxGroup,
                TaxItem = i.TaxGroup,
                FPJNumber = i.TaxInvoiceNo,
                FPJDate = string.IsNullOrEmpty(i.FPJDate.ToString()) ? "" : DateTime.Parse(i.FPJDate.ToString()).ToString("dd-MMM-yyyy"),
                CreatedDate = DateTime.Parse(i.CreatedDate.ToString()).ToString("dd-MMM-yyyy"),
                TaxCode = i.TaxGroup,
                OffsetAccountType = i.OffSetAccountType,
                JournalID = i.JournalId,
                StatusGenerate = i.StatusGenerate
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxApprovalARMonitoringBuilding", table);

        }

        [Route("TrxCollectionReport/Export")]
        public void TrxCollectionReportToExport()
        {
            //Parameter
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string strStartPaidDate = Request.QueryString["strStartPaidDate"];
            string strEndPaidDate = Request.QueryString["strEndPaidDate"];
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strPaidStatus = Request.QueryString["strPaidStatus"];
            int intInvoiceCategory = int.Parse(Request.QueryString["intInvoiceCategory"]);
            int intCustomerId = int.Parse(Request.QueryString["intCustomerId"]);
            string InvNo = Request.QueryString["InvNo"];

            //Call Service
            List<ARSystemService.vwCollectionReportInvoice> list = new List<ARSystemService.vwCollectionReportInvoice>();
            using (var client = new ARSystemService.ItrxCollectionReportInvoiceServiceClient())
            {
                int intTotalRecord = client.GetTrxCollectionReportInvoiceCount(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strStartPaidDate, strEndPaidDate, strCompanyId, strOperator, strPaidStatus, intInvoiceCategory, intCustomerId, InvNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwCollectionReportInvoice> listHolder = new List<ARSystemService.vwCollectionReportInvoice>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetTrxCollectionReportInvoiceToList(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strStartPaidDate, strEndPaidDate, strCompanyId, strOperator, strPaidStatus, intInvoiceCategory, intCustomerId, InvNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }
            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvoiceNo","InvoiceDate", "NoInvoiceTemp", "ReceiptDate", "PaidDate", "TermInvoice",
                "Company", "Operator", "CustomerName", "AmountDPP", "VAT","PenaltyAfterTax","AmountPPH23","AmountPPHFinal","AmountTotal","PartialPaid",
                "AmountInvoice","PaidAmount","Rounding","DR","RTGS","Penalty","PPNExpired","WAPU","PaidStatus",
                "Status","ChecklistDocDate","InvoiceCategory","LastPosition"
            };
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                InvoiceNo = i.InvNo,
                InvoiceDate = string.IsNullOrEmpty(i.InvPrintDate.ToString()) ? "" : DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                NoInvoiceTemp = i.InvTemp,
                ReceiptDate = string.IsNullOrEmpty(i.InvReceiptDate.ToString()) ? "" : DateTime.Parse(i.InvReceiptDate.ToString()).ToString("dd-MMM-yyyy"),
                PaidDate = string.IsNullOrEmpty(i.InvPaidDate.ToString()) ? "" : DateTime.Parse(i.InvPaidDate.ToString()).ToString("dd-MMM-yyyy"),
                TermInvoice = i.Description,
                Company = i.InvCompanyId,
                Operator = i.InvOperatorID,
                CustomerName = i.CustomerName,
                AmountDPP = i.InvSumADPP,
                VAT = i.InvTotalAPPN,
                PenaltyAfterTax = i.PAT,
                AmountPPH23 = i.PPH,
                AmountPPHFinal = i.PPF,
                AmountTotal = i.InvTotalAmount,
                PartialPaid = i.PartialPaid,
                AmountInvoice = i.InvSumADPP,
                PaidAmount = i.PAM,
                Rounding = i.RND,
                DR = i.DBT,
                RTGS = i.RTG,
                Penalty = i.PNT,
                PPNExpired = i.PPE,
                WAPU = i.WAPU,
                PaidStatus = i.PaidStatus,
                Status = i.StatusAx,
                ChecklistDocDate = string.IsNullOrEmpty(i.VerificationDate.ToString()) ? "" : DateTime.Parse(i.VerificationDate.ToString()).ToString("dd-MMM-yyyy"),
                InvoiceCategory = (i.mstInvoiceCategoryId) == 2 ? "Building" : "TOWER",
                LastPosition = i.LastPosition
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("CollectionReport", table);
        }

        //[Route("TrxApprovalARMonitoringTowerToExcel/Export")]
        //public void TrxApprovalARMonitoringTowerToExcel()
        //{
        //    //Parameter
        //    string strHeaderId = Request.QueryString["strHeaderId"];
        //    string strCategoryId = Request.QueryString["strCategoryId"];
        //    string strBatchNumber = Request.QueryString["strBatchNumber"];

        //    //Call Service
        //    List<ARSystemService.vwApprovalARMonitoringCollectionTower> listDataInvoice = new List<ARSystemService.vwApprovalARMonitoringCollectionTower>();

        //    using (var client = new ARSystemService.ItrxApprovalARMonitoringTowerServiceClient())
        //    {
        //        listDataInvoice = client.GetChecklistedARMonitoringCollectionTowerToList(UserManager.User.UserToken, strHeaderId, strCategoryId, strBatchNumber).ToList();
        //    }

        //    //Convert to DataTable
        //    DataTable table = new DataTable();
        //    string[] ColumsShow = new string[] {"journalId","dayFPJ", "monthFPJ", "yearFPJ", "InvoiceID", "itemId",
        //        "offsetAccount", "taxGroup", "taxItemGroup", "fpjNumber","voucher","AccountNum","transactionText","amountCurDebit","currencyCode",
        //        "soNumb","docNumb","dayDueDate","monthDueDate","yearDueDate","xRate","amountCurCredit","departement","costCenter",
        //        "purpose","dayTrans","monthTrans","yearTrans","transferBy","agsTypeAR","company"
        //    };
        //    var reader = FastMember.ObjectReader.Create(listDataInvoice.Select(i => new
        //    {
        //        journalId = i.JournalId,
        //        dayFPJ = string.IsNullOrEmpty(i.FPJDate.ToString()) ? 0 : DateTime.Parse(i.FPJDate.ToString()).Day,
        //        monthFPJ = string.IsNullOrEmpty(i.FPJDate.ToString()) ? 0 : DateTime.Parse(i.FPJDate.ToString()).Month,
        //        yearFPJ = string.IsNullOrEmpty(i.FPJDate.ToString()) ? 0 : DateTime.Parse(i.FPJDate.ToString()).Year,
        //        InvoiceID = i.InvNo,
        //        itemId = "",
        //        offsetAccount = i.OffSetAccount,
        //        taxGroup = i.TaxGroup,
        //        taxItemGroup = i.TaxGroup,
        //        fpjNumber = i.TaxInvoiceNo,
        //        voucher = i.VoucherNumber,
        //        AccountNum = i.AccountNum,
        //        transactionText = i.TransactionText,
        //        amountCurDebit = double.Parse(i.Debit.ToString()),
        //        currencyCode = i.Currency,
        //        soNumb = i.SONumber,
        //        docNumb = i.DocNumber,
        //        dayDueDate = DateTime.Parse(i.DueDate.ToString()).Day,
        //        monthDueDate = DateTime.Parse(i.DueDate.ToString()).Month,
        //        yearDueDate = DateTime.Parse(i.DueDate.ToString()).Year,
        //        xRate = double.Parse(i.Xrate.ToString()),
        //        amountCurCredit = double.Parse(i.Credit.ToString()),
        //        departement = "",
        //        costCenter = "",
        //        purpose = "",
        //        dayTrans = DateTime.Parse(i.TransDate.ToString()).Day,
        //        monthTrans = DateTime.Parse(i.TransDate.ToString()).Month,
        //        yearTrans = DateTime.Parse(i.TransDate.ToString()).Year,
        //        transferBy = "",
        //        agsTypeAR = i.TaxGroup == "PPN" ? 1 : 2,
        //        company = i.CompanyIdAx
        //    }), ColumsShow);
        //    table.Load(reader);

        //    //Export to Excel
        //    ExportToExcelHelper.Export("ARCollectionTowerToAX", table);

        //}

        //[Route("TrxApprovalARMonitoringBuildingToExcel/Export")]
        //public void TrxApprovalARMonitoringBuildingToExcel()
        //{
        //    //Parameter
        //    string strHeaderId = Request.QueryString["strHeaderId"];
        //    string strCategoryId = Request.QueryString["strCategoryId"];
        //    string strBatchNumber = Request.QueryString["strBatchNumber"];

        //    //Call Service
        //    List<ARSystemService.vwApprovalARMonitoringCollectionBuilding> listDataInvoice = new List<ARSystemService.vwApprovalARMonitoringCollectionBuilding>();

        //    using (var client = new ARSystemService.ItrxApprovalARMonitoringBuildingServiceClient())
        //    {
        //        listDataInvoice = client.GetChecklistedARMonitoringCollectionBuildingToList(UserManager.User.UserToken, strHeaderId, strCategoryId, strBatchNumber).ToList();
        //    }

        //    //Convert to DataTable
        //    DataTable table = new DataTable();
        //    string[] ColumsShow = new string[] {"journalId","dayFPJ", "monthFPJ", "yearFPJ", "InvoiceID", "itemId",
        //        "offsetAccount", "taxGroup", "taxItemGroup", "fpjNumber","voucher","AccountNum","transactionText","amountCurDebit","currencyCode",
        //        "soNumb","docNumb","dayDueDate","monthDueDate","yearDueDate","xRate","amountCurCredit","departement","costCenter",
        //        "purpose","dayTrans","monthTrans","yearTrans","transferBy","agsTypeAR","company"
        //    };
        //    var reader = FastMember.ObjectReader.Create(listDataInvoice.Select(i => new
        //    {
        //        journalId = i.JournalId,
        //        dayFPJ = string.IsNullOrEmpty(i.FPJDate.ToString()) ? 0 : DateTime.Parse(i.FPJDate.ToString()).Day,
        //        monthFPJ = string.IsNullOrEmpty(i.FPJDate.ToString()) ? 0 : DateTime.Parse(i.FPJDate.ToString()).Month,
        //        yearFPJ = string.IsNullOrEmpty(i.FPJDate.ToString()) ? 0 : DateTime.Parse(i.FPJDate.ToString()).Year,
        //        InvoiceID = i.InvNo,
        //        itemId = "",
        //        offsetAccount = i.OffSetAccount,
        //        taxGroup = i.TaxGroup,
        //        taxItemGroup = i.TaxGroup,
        //        fpjNumber = i.TaxInvoiceNo,
        //        voucher = i.VoucherNumber,
        //        AccountNum = i.AccountNum,
        //        transactionText = i.TransactionText,
        //        amountCurDebit = double.Parse(i.Debit.ToString()),
        //        currencyCode = i.Currency,
        //        soNumb = i.SONumber,
        //        docNumb = i.DocNumber,
        //        dayDueDate = DateTime.Parse(i.DueDate.ToString()).Day,
        //        monthDueDate = DateTime.Parse(i.DueDate.ToString()).Month,
        //        yearDueDate = DateTime.Parse(i.DueDate.ToString()).Year,
        //        xRate = double.Parse(i.Xrate.ToString()),
        //        amountCurCredit = double.Parse(i.Credit.ToString()),
        //        departement = "",
        //        costCenter = "",
        //        purpose = "",
        //        dayTrans = DateTime.Parse(i.TransDate.ToString()).Day,
        //        monthTrans = DateTime.Parse(i.TransDate.ToString()).Month,
        //        yearTrans = DateTime.Parse(i.TransDate.ToString()).Year,
        //        transferBy = "",
        //        agsTypeAR = i.TaxGroup == "PPN" ? 1 : 2,
        //        company = i.CompanyIdAx
        //    }), ColumsShow);
        //    table.Load(reader);

        //    //Export to Excel
        //    ExportToExcelHelper.Export("ARCollectionBuildingToAX", table);

        //}

        [Route("TrxApprovalCNInvoiceBuilding/Export")]
        public void GetTrxApprovalCNInvoiceBuildingToExport()
        {
            //Parameter
            string companyName = Request.QueryString["companyName"];
            string invoiceTypeID = Request.QueryString["invoiceTypeID"];
            string invCompanyId = Request.QueryString["invCompanyId"];
            string invNo = Request.QueryString["invNo"];

            //Call Service
            List<ARSystemService.vwCNInvoiceBuilding> list = new List<ARSystemService.vwCNInvoiceBuilding>();
            using (var client = new ARSystemService.ItrxApprovalCNInvoiceBuildingServiceClient())
            {
                int intTotalRecord = client.GetApprovalCNInvoiceBuildingCount(UserManager.User.UserToken, invoiceTypeID, companyName, invCompanyId, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwCNInvoiceBuilding> listHolder = new List<ARSystemService.vwCNInvoiceBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetApprovalCNInvoiceBuildingToList(UserManager.User.UserToken, invoiceTypeID, companyName, invCompanyId, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "InvNo",
                "InvTemp",
                "Company",
                "CompanyType",
                "InvPrintDate",
                "PostedBy",
                "PostingDate",
                "Term",
                "Currency",
                "InvTotalAmount",
                "InvTotalAPPN",
                "Discount",
                "InvTotalPenalty",
                "TaxInvoiceNo"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ApprovalCNBuildingInvoices", table);
        }

        [Route("TrxApprovalCNInvoiceTower/Export")]
        public void GetTrxApprovalCNInvoiceTowerToExport()
        {
            //Parameter
            string companyId = Request.QueryString["companyId"];
            string invoiceTypeID = Request.QueryString["invoiceTypeID"];
            string operatorId = Request.QueryString["operatorId"];
            string invNo = Request.QueryString["invNo"];

            //Call Service
            List<ARSystemService.vwCNInvoiceTower> list = new List<ARSystemService.vwCNInvoiceTower>();
            using (var client = new ARSystemService.ItrxApprovalCNInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetApprovalCNInvoiceTowerCount(UserManager.User.UserToken, invoiceTypeID, companyId, operatorId, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwCNInvoiceTower> listHolder = new List<ARSystemService.vwCNInvoiceTower>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetApprovalCNInvoiceTowerToList(UserManager.User.UserToken, invoiceTypeID, companyId, operatorId, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            string[] fieldList = new string[] {
                "InvNo",
                "InvTemp",
                "PICACollection",
                "Remark",
                "PICAARData",
                "Company",
                "Operator",
                "PostedBy",
                "PostingDate",
                "Term",
                "Currency",
                "InvSumADPP",
                "InvTotalAPPN",
                "Discount",
                "InvTotalPenalty",
                "InvPrintDate"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                i.InvNo,
                InvTemp = i.InvTemp,
                PICACollection = i.PICADetail,
                Remark = i.Remark,
                PICAARData = i.PicaDetailSec,
                Company = i.InvCompanyId,
                Operator = i.InvOperatorID,
                i.PostedBy,
                i.PostingDate,
                i.Term,
                i.Currency,
                i.InvSumADPP,
                i.InvTotalAPPN,
                i.Discount,
                i.InvTotalPenalty,
                i.InvPrintDate
            }), fieldList);
            table.Load(reader);



            //Export to Excel
            ExportToExcelHelper.Export("ApprovalCNTowerInvoices", table);
        }

        [Route("TrxRePrintApprovalInvoiceTower/Export")]
        public void GetTrxRePrintInvoiceTowerToExport()
        {
            //Parameter
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strInvoiceType = Request.QueryString["strInvoiceType"];
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string intmstInvoiceStatusId = Request.QueryString["intmstInvoiceStatusId"];
            string invNo = Request.QueryString["invNo"];
            //Call Service
            List<ARSystemService.vwDeptHeadReprintApprovalInvoiceTower> listDataPostedInvoice = new List<ARSystemService.vwDeptHeadReprintApprovalInvoiceTower>();

            using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetApprovalReprintTowerCount(UserManager.User.UserToken, strCompanyId, strOperator, strInvoiceType, strStartPeriod, strEndPeriod, int.Parse(intmstInvoiceStatusId), invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwDeptHeadReprintApprovalInvoiceTower> listDataPostedInvoiceHolder = new List<ARSystemService.vwDeptHeadReprintApprovalInvoiceTower>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataPostedInvoiceHolder = client.GetApprovalReprintTowertoList(UserManager.User.UserToken, strCompanyId, strOperator, strInvoiceType, strStartPeriod, strEndPeriod, int.Parse(intmstInvoiceStatusId), invNo, null, 50 * i, 50).ToList();
                    listDataPostedInvoice.AddRange(listDataPostedInvoiceHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {
                "NoInvoice", "Requestor", "PICAReprint", "PICARemarks", "InvoiceDate",
                "Term", "Company", "Operator", "Currency", "RequestCounter","ApproveCounter","RejectCounter"
            };
            var reader = FastMember.ObjectReader.Create(listDataPostedInvoice.Select(i => new
            {
                NoInvoice = i.InvNo,
                Requestor = i.FullName,
                i.PICAReprint,
                i.PICARemarks,
                InvoiceDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                Term = i.InvTypeDesc,
                Company = i.InvCompanyId,
                Operator = i.InvOperatorID,
                i.Currency,
                i.RequestCounter,
                i.ApproveCounter,
                i.RejectCounter
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxRePrintInvoiceTower", table);

        }
        [Route("TrxRePrintInvoiceBuilding/Export")]
        public void GetTrxRePrintInvoiceBuildingToExport()
        {
            //Parameter
            string companyName = Request.QueryString["companyName"];
            string invoiceTypeId = Request.QueryString["invoiceTypeId"];
            int invoiceStatusId = int.Parse(Request.QueryString["invoiceStatusId"]);
            string invNo = Request.QueryString["invNo"];

            DateTime? startPeriod = DateTime.MinValue;
            DateTime? endPeriod = DateTime.MaxValue;

            if (!string.IsNullOrEmpty(Request.QueryString["startPeriod"]))
                startPeriod = DateTime.Parse(Request.QueryString["startPeriod"]);

            if (!string.IsNullOrEmpty(Request.QueryString["endPeriod"]))
                endPeriod = DateTime.Parse(Request.QueryString["endPeriod"]);
            //Call Service
            List<ARSystemService.vwDeptHeadReprintApprovalInvoiceBuilding> list = new List<ARSystemService.vwDeptHeadReprintApprovalInvoiceBuilding>();

            using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
            {
                int intTotalRecord = client.GetApprovalReprintBuildingCount(UserManager.User.UserToken, companyName, invoiceTypeId, startPeriod, endPeriod, invoiceStatusId, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwDeptHeadReprintApprovalInvoiceBuilding> listHolder = new List<ARSystemService.vwDeptHeadReprintApprovalInvoiceBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetApprovalReprintBuildingtoList(UserManager.User.UserToken, companyName, invoiceTypeId, startPeriod, endPeriod, invoiceStatusId, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable(); string[] ColumsShow = new string[] {
                "NoInvoice", "Requestor", "PICAReprint", "PICARemarks", "InvoiceDate",
                "Term", "Company", "Customer", "Currency", "RequestCounter","ApproveCounter","RejectCounter"
            };
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                NoInvoice = i.InvNo,
                Requestor = i.FullName,
                i.PICAReprint,
                i.PICARemarks,
                InvoiceDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                Term = i.Term,
                Company = i.InvCompanyId,
                Customer = i.Company,
                i.Currency,
                i.RequestCounter,
                i.ApproveCounter,
                i.RejectCounter
            }), ColumsShow);

            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxRePrintInvoiceBuilding", table);

        }

        [Route("TrxManageMergedRePrintInvoiceTower/Export")]
        public void TrxManageMergedRePrintInvoiceTowerToExport()
        {
            string operatorId = Request.QueryString["operatorId"];
            string companyId = Request.QueryString["companyId"];
            string invNo = Request.QueryString["invNo"];

            List<ARSystemService.vwDeptHeadReprintAppInvMergedTower> list = new List<ARSystemService.vwDeptHeadReprintAppInvMergedTower>();

            using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
            {
                int intTotalRecord = client.GetApprovalReprintMergedTowerCount(UserManager.User.UserToken, companyId, operatorId, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwDeptHeadReprintAppInvMergedTower> listHolder = new List<ARSystemService.vwDeptHeadReprintAppInvMergedTower>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetApprovalReprintMergedTowertoList(UserManager.User.UserToken, companyId, operatorId, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            string[] fieldList = new string[] {
                "InvNo", "FullName", "PICAReprint", "PICARemarks", "Company", "Operator", "InvSumADPP", "Discount","InvTotalAPPN","InvTotalPenalty"
                ,"Currency","RequestCounter","ApproveCounter","RejectCounter"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);
            table.Load(reader);

            ExportToExcelHelper.Export("ReprintMergedInvoiceTower", table);
        }

        [Route("TrxManageMergedRePrintInvoiceBuilding/Export")]
        public void TrxManageMergedRePrintInvoiceBuildingToExport()
        {
            string customerName = Request.QueryString["customerName"];
            string companyId = Request.QueryString["companyId"];
            string invNo = Request.QueryString["invNo"];

            List<ARSystemService.vwDeptHeadReprintAppInvMergedBuilding> list = new List<ARSystemService.vwDeptHeadReprintAppInvMergedBuilding>();

            using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
            {
                int intTotalRecord = client.GetApprovalReprintMergedBuildingCount(UserManager.User.UserToken, companyId, customerName, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwDeptHeadReprintAppInvMergedBuilding> listHolder = new List<ARSystemService.vwDeptHeadReprintAppInvMergedBuilding>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetApprovalReprintMergedBuildingtoList(UserManager.User.UserToken, companyId, customerName, invNo, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }

            string[] fieldList = new string[] {
                "InvNo", "FullName", "PICAReprint", "PICARemarks", "Company", "CustomerName", "InvSumADPP", "Discount","InvTotalAPPN","InvTotalPenalty"
                ,"Currency","RequestCounter","ApproveCounter","RejectCounter"
            };
            DataTable table = new DataTable();
            var reader = FastMember.ObjectReader.Create(list, fieldList);

            table.Load(reader);

            ExportToExcelHelper.Export("MergedInvoiceBuilding", table);
        }

        [Route("TrxReportInvoiceCompile/Export")]
        public void TrxReportInvoiceCompileToExport()
        {
            //Parameter
            int intYearPosting = int.Parse(Request.QueryString["intYearPosting"]);
            int intMonthPosting = int.Parse(Request.QueryString["intMonthPosting"]);
            int intWeekPosting = int.Parse(Request.QueryString["intWeekPosting"]);
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string invNo = Request.QueryString["invNo"];
            string strCompanyCode =Request.QueryString["InvCompanyId"];
            //Call Service
            List<ARSystemService.vwReportInvoiceCompileByInvoice> listDataPostedInvoice = new List<ARSystemService.vwReportInvoiceCompileByInvoice>();

            using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetTrxReportInvoiceTowerCount(UserManager.User.UserToken, strStartPeriod, strEndPeriod,strCompanyCode, intYearPosting, intMonthPosting, intWeekPosting, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwReportInvoiceCompileByInvoice> listDataPostedInvoiceHolder = new List<ARSystemService.vwReportInvoiceCompileByInvoice>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataPostedInvoiceHolder = client.GetTrxReportInvoiceCompileToList(UserManager.User.UserToken, strStartPeriod, strEndPeriod, null, intYearPosting, intMonthPosting, intWeekPosting, invNo, 50 * i, 50).ToList();
                    listDataPostedInvoice.AddRange(listDataPostedInvoiceHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvoiceID",
                "TransactionText", "Debit", "CompanyInvoice", "CompanyReal", "TransactionDate",
                "Voucher", "AccountType", "InvoiceCategory", "ElectricityCategory","Operator",
                "Credit","Currency","Xrate","DocNumber","DocDate","DueDate","PostingProfile","OffsetAccount",
                "TaxGroup","TaxItemGroup","FPJNumber","FPJDate","CreatedDate","OperatorRegion","Address"
            };
            var reader = FastMember.ObjectReader.Create(listDataPostedInvoice.Select(i => new
            {
                InvoiceID = i.InvNo,
                TransactionText = i.InvSubject,
                Debit = i.InvSumADPP,
                CompanyInvoice = i.InvCompanyId,
                CompanyReal = i.CompanyIdAx,
                TransactionDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                Voucher = i.InvTemp,
                i.AccountType,
                i.InvoiceCategory,
                i.ElectricityCategory,
                Operator = i.InvOperatorID,
                i.Credit,
                i.Currency,
                i.Xrate,
                i.DocNumber,
                DocDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                i.DueDate,
                i.PostingProfile,
                OffsetAccount = i.OffSetAccount,
                i.TaxGroup,
                i.TaxItemGroup,
                FPJNumber = i.TaxInvoiceNo,
                FPJDate = string.IsNullOrEmpty(i.FPJDate.ToString()) ? "" : DateTime.Parse(i.FPJDate.ToString()).ToString("dd-MMM-yyyy"),
                CreatedDate = i.PostingDate,
                i.OperatorRegion,
                i.Address
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxReportInvoiceCompile", table);

        }

        [Route("TrxReportInvoiceCompileSONumber/Export")]
        public void TrxReportInvoiceCompileBySONumberToExport()
        {
            //Parameter
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string invNo = Request.QueryString["invNo"];
            //Call Service
            List<ARSystemService.vwReportInvoiceCompileBySoNumber> listDataInvoiceBySONumber = new List<ARSystemService.vwReportInvoiceCompileBySoNumber>();

            using (var client = new ARSystemService.ItrxReportInvoiceTowerServiceClient())
            {
                int intTotalRecord = client.GetTrxReportInvoiceCompileBySONumberCount(UserManager.User.UserToken, strStartPeriod, strEndPeriod, invNo);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwReportInvoiceCompileBySoNumber> listDataInvoiceBySONumberHolder = new List<ARSystemService.vwReportInvoiceCompileBySoNumber>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataInvoiceBySONumberHolder = client.GetTrxReportInvoiceCompileBySONumberToList(UserManager.User.UserToken, strStartPeriod, strEndPeriod, invNo, null, 50 * i, 50).ToList();
                    listDataInvoiceBySONumber.AddRange(listDataInvoiceBySONumberHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"SONumber",
                "SiteID", "SiteName", "InvoiceID", "TransactionText", "StartPeriod",
                "EndPeriod", "Debit", "CompanyInvoice", "CompanyReal","Voucher",
                "Type","CategoryInvoice","Operator","Credit","Currency",
                "SLDDate","BAPSReceiveDate","BAPSDate",
                "CreateDate","InvoiceDate","DueDate","PostingProfile","OffsetAccount","TaxGroup","TaxItemGroup","FPJNumber",
                "FPJDate","BAPSConfirmDate","BAPSPostingDate","BAPSPrintDate","BAPSReceiptDate","LeadTimeBAPSConfirm",
                "LeadTimeVerificator","LeadTimeInputer","LeadTimeFinishing","LeadTimeARDataDept"
            };
            var reader = FastMember.ObjectReader.Create(listDataInvoiceBySONumber.Select(i => new
            {
                i.SONumber,
                SiteID = i.SiteIdOld,
                i.SiteName,
                InvoiceID = i.InvNo,
                TransactionText = i.InvSubject,
                StartPeriod = DateTime.Parse(i.StartDatePeriod.ToString()).ToString("dd-MMM-yyyy"),
                EndPeriod = DateTime.Parse(i.EndDatePeriod.ToString()).ToString("dd-MMM-yyyy"),
                Debit = i.InvSumADPP,
                CompanyInvoice = i.InvCompanyId,
                CompanyReal = i.CompanyIdAx,
                Voucher = i.InvTemp,
                Type = i.Description,
                CategoryInvoice = i.InvoiceCategory,
                Operator = i.InvOperatorID,
                i.Credit,
                i.Currency,
                SLDDate = "",//DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),//change to sld
                BAPSReceiveDate = DateTime.Parse(i.BapsReceiveDate.ToString()).ToString("dd-MMM-yyyy"),
                BAPSDate = DateTime.Parse(i.BapsDone.ToString()).ToString("dd-MMM-yyyy"),
                CreateDate = DateTime.Parse(i.CreatedDate.ToString()).ToString("dd-MMM-yyyy"),
                InvoiceDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                DueDate = DateTime.Parse(i.DueDate.ToString()).ToString("dd-MMM-yyyy"),
                PostingProfile = i.PostingProfile,
                OffsetAccount = i.OffSetAccount,
                i.TaxGroup,
                i.TaxItemGroup,
                FPJNumber = i.TaxInvoiceNo,
                FPJDate = string.IsNullOrEmpty(i.FPJDate.ToString()) ? "" : DateTime.Parse(i.FPJDate.ToString()).ToString("dd-MMM-yyyy"),
                BAPSConfirmDate = DateTime.Parse(i.BapsConfirmDate.ToString()).ToString("dd-MMM-yyyy"),
                BAPSPostingDate = DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),// is it really invoice date? existing is
                BAPSPrintDate = string.IsNullOrEmpty(i.InvFirstPrintDate.ToString()) ? "" : DateTime.Parse(i.InvFirstPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                BAPSReceiptDate = string.IsNullOrEmpty(i.InvReceiptDate.ToString()) ? "" : DateTime.Parse(i.InvReceiptDate.ToString()).ToString("dd-MMM-yyyy"),
                LeadTimeBAPSConfirm = i.LeadTimeVerificator, //existing lead time verficator
                LeadTimeVerificator = i.LeadTimeVerificator,
                LeadTimeInputer = i.LeadTimeInputer,
                LeadTimeFinishing = i.LeadTimeFinishing, //LeadTimeFinishing
                LeadTimeARDataDept = i.LeadTimeARData //LeadTimeAR
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxReportInvoiceTowerBySONumber", table);

        }

        [Route("TrxReserveInvoiceNumberToExport/Export")]
        public void TrxRequestInvoiceNumberToExport()
        {
            //Parameter
            string strOperator = Request.QueryString["OperatorId"];
            string strCompany = Request.QueryString["CompanyId"];
            string StartDateRequest = Request.QueryString["StartDateRequest"];
            string EndDateRequest = Request.QueryString["EndDateRequest"];
            //Call Service
            List<ARSystemService.vmReserveInvoiceNumber> listData = new List<ARSystemService.vmReserveInvoiceNumber>();

            using (var client = new ARSystemService.ItrxReserveInvoiceNumberClient())
            {
                int intTotalRecord = client.GetReserveInvoiceNumberCount(UserManager.User.UserToken, strOperator, strCompany, StartDateRequest, EndDateRequest);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vmReserveInvoiceNumber> listDataHolder = new List<ARSystemService.vmReserveInvoiceNumber>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataHolder = client.GetReserveInvoiceNumberToList(UserManager.User.UserToken, strOperator, strCompany, StartDateRequest, EndDateRequest, null, 50 * i, 50).ToList();
                    listData.AddRange(listDataHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"Company",
                "Operator", "AmountReserved", "Remarks", "Requestor", "RequestDate"
            };
            var reader = FastMember.ObjectReader.Create(listData.Select(i => new
            {
                Company = i.CompanyId,
                Operator = i.OperatorId,
                AmountReserved = i.AmountReserve,
                Remarks = i.Remarks,
                Requestor = i.CreatedBy,
                RequestDate = DateTime.Parse(i.CreatedDate.ToString()).ToString("dd-MMM-yyyy"),

            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxReserveInvoiceNumber", table);

        }

        [Route("TrxReplaceInvoiceNumberToExport/Export")]
        public void TrxReplaceInvoiceNumberToExport()
        {
            //Parameter
            string strOperator = Request.QueryString["OperatorId"];
            string strCompany = Request.QueryString["CompanyId"];
            string ReservedNo = Request.QueryString["ReservedNo"];
            string Year = Request.QueryString["Year"];
            //Call Service
            List<ARSystemService.vwReserveInvoiceNumberDetail> listData = new List<ARSystemService.vwReserveInvoiceNumberDetail>();

            using (var client = new ARSystemService.ItrxReserveInvoiceNumberClient())
            {
                int intTotalRecord = client.GetReplaceInvoiceNumberCount(UserManager.User.UserToken, strOperator, strCompany, ReservedNo, Year);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwReserveInvoiceNumberDetail> listDataHolder = new List<ARSystemService.vwReserveInvoiceNumberDetail>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listDataHolder = client.GetReplaceInvoiceNumberToList(UserManager.User.UserToken, strOperator, strCompany, ReservedNo, Year, null, 50 * i, 50).ToList();
                    listData.AddRange(listDataHolder);
                }
            }

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvoiceNumber","Company",
                "Operator", "Year", "PairedNumber"
            };
            var reader = FastMember.ObjectReader.Create(listData.Select(i => new
            {
                InvoiceNumber = i.InvNo,
                Company = i.CompanyId,
                Operator = i.OperatorId,
                Year = i.Year,
                PairedNumber = i.PairedNo

            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("TrxReplaceInvoiceNumber", table);

        }

        [Route("TrxHistoryCNInvoiceARCollectionReport/Export")]
        public void TrxHistoryCNInvoiceARCollectionReport()
        {
            //Parameter
            string strStartPeriod = Request.QueryString["strStartPeriod"];
            string strEndPeriod = Request.QueryString["strEndPeriod"];
            string strCompanyId = Request.QueryString["strCompanyId"];
            string strOperator = Request.QueryString["strOperator"];
            string strCNStatus = Request.QueryString["strCNStatus"];
            string InvNo = Request.QueryString["InvNo"];
            string InvoiceTypeId = Request.QueryString["InvoiceTypeId"];

            //Call Service
            List<ARSystemService.vwHistoryCNInvoiceARCollection> list = new List<ARSystemService.vwHistoryCNInvoiceARCollection>();
            using (var client = new ARSystemService.ItrxHistoryCNInvoiceARCollectionServiceClient())
            {
                int intTotalRecord = client.GetHistoryCNInvoiceARCollectionCount(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyId, strOperator, InvNo, strCNStatus, InvoiceTypeId);
                int intBatch = intTotalRecord / 50;
                List<ARSystemService.vwHistoryCNInvoiceARCollection> listHolder = new List<ARSystemService.vwHistoryCNInvoiceARCollection>();

                for (int i = 0; i <= intBatch; i++)
                {
                    listHolder = client.GetHistoryCNInvoiceARCollectionToList(UserManager.User.UserToken, strStartPeriod, strEndPeriod, strCompanyId, strOperator, InvNo, strCNStatus, InvoiceTypeId, null, 50 * i, 50).ToList();
                    list.AddRange(listHolder);
                }
            }
            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvoiceNo","ParentInvoice", "NoInvoiceTemp","AmountDPP","AmountPPN"
                ,"InvoiceDate","RequestDate","RequestedBy","ApprovalDate","ApprovedBy","TermInvoice","Company","Operator","Status","PICA","PICARemarks"
            };
            var reader = FastMember.ObjectReader.Create(list.Select(i => new
            {
                InvoiceNo = i.InvNo,
                ParentInvoice = i.InvParentNo,
                NoInvoiceTemp = i.InvTemp,
                AmountDPP = i.InvSumADPP,
                AmountPPN = i.InvTotalAPPN,
                InvoiceDate = string.IsNullOrEmpty(i.InvPrintDate.ToString()) ? "" : DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                RequestDate = string.IsNullOrEmpty(i.RequestedDate.ToString()) ? "" : DateTime.Parse(i.RequestedDate.ToString()).ToString("dd-MMM-yyyy"),
                RequestedBy = i.RequestedBy,
                ApprovalDate = string.IsNullOrEmpty(i.ApprovedDate.ToString()) ? "" : DateTime.Parse(i.ApprovedDate.ToString()).ToString("dd-MMM-yyyy"),
                ApprovedBy = i.ApprovedBy,
                TermInvoice = i.Description,
                Company = i.InvCompanyId,
                Operator = i.InvOperatorID,
                Status = i.isCNApproved,
                PICA = i.PicaDetailRequestor,
                PICARemarks = i.Remark
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("HistoryCNInvoiceARCollection", table);
        }

        [Route("TrxHistoryCNInvoiceARDataReport/Export")]
        public void TrxHistoryCNInvoiceARDataReport(string strStartPeriod, string strEndPeriod, string strCompanyId, string strOperator, string strCNStatus, string strRpStatus, string InvNo, string InvoiceTypeId, int ProccessType)
        {
            var token = UserManager.User.UserToken;
            vmUserCredential userCredential = UserService.CheckUserToken(token);
            //Parameterstring TaxNo = string.Empty;
            var list = new List<vwHistoryCNInvoiceARData>();
            var result = _historyCNService.GetHistoryCNInvoiceARDataToList(token, userCredential, strStartPeriod, strEndPeriod, strCompanyId, strOperator, InvNo, null, strCNStatus, InvoiceTypeId, ProccessType, strRpStatus, null, null, null).ToList();
            list.AddRange(result);
            DataTable table = new DataTable();
            string[] fieldList;
            if (ProccessType == 1)
            {
                fieldList = new string[] {
                    "SONumber","InvoiceNo","ParentInvoice", "NoInvoiceTemp","AmountDPP","AmountPPN"
                    ,"InvoiceDate","RequestDate","RequestedBy","ApprovalDate","ApprovedBy","TermInvoice","Company","Operator","CNInfo","CNFrom","PICA","PICARemarks"
                };

                var reader = FastMember.ObjectReader.Create(list.Select(i => new
                {
                    SONumber = i.SONumber,
                    InvoiceNo = i.InvNo,
                    ParentInvoice = i.InvParentNo,
                    NoInvoiceTemp = i.InvTemp,
                    AmountDPP = i.InvSumADPP,
                    AmountPPN = i.InvTotalAPPN,
                    InvoiceDate = string.IsNullOrEmpty(i.InvPrintDate.ToString()) ? "" : DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                    RequestDate = string.IsNullOrEmpty(i.RequestedDate.ToString()) ? "" : DateTime.Parse(i.RequestedDate.ToString()).ToString("dd-MMM-yyyy"),
                    RequestedBy = i.RequestedBy,
                    ApprovalDate = string.IsNullOrEmpty(i.ApprovedDate.ToString()) ? "" : DateTime.Parse(i.ApprovedDate.ToString()).ToString("dd-MMM-yyyy"),
                    ApprovedBy = i.ApprovedBy,
                    TermInvoice = i.Description,
                    Company = i.InvCompanyId,
                    Operator = i.InvOperatorID,
                    CNInfo = i.CNInfo,
                    CNFrom = i.CNFrom,
                    PICA = i.PicaTypeRequestor + " - " + i.PicaDetailRequestor,
                    PICARemarks = i.Remark
                }), fieldList);
                table.Load(reader);
            }
            else
            {
                fieldList = new string[] {
                   "InvoiceNo","ParentInvoice", "NoInvoiceTemp","AmountDPP","AmountPPN"
                    ,"InvoiceDate","RequestDate","RequestedBy","ApprovalDate","ApprovedBy","TermInvoice","Company","Operator","CNInfo","CNFrom","PICA","PICARemarks","ReplacementStatus", "ReplaceDate", "ReplaceInvoice"
                };
                var reader = FastMember.ObjectReader.Create(list.Select(i => new
                {
                    SONumber = i.SONumber,
                    InvoiceNo = i.InvNo,
                    ParentInvoice = i.InvParentNo,
                    NoInvoiceTemp = i.InvTemp,
                    AmountDPP = i.InvSumADPP,
                    AmountPPN = i.InvTotalAPPN,
                    InvoiceDate = string.IsNullOrEmpty(i.InvPrintDate.ToString()) ? "" : DateTime.Parse(i.InvPrintDate.ToString()).ToString("dd-MMM-yyyy"),
                    RequestDate = string.IsNullOrEmpty(i.RequestedDate.ToString()) ? "" : DateTime.Parse(i.RequestedDate.ToString()).ToString("dd-MMM-yyyy"),
                    RequestedBy = i.RequestedBy,
                    ApprovalDate = string.IsNullOrEmpty(i.ApprovedDate.ToString()) ? "" : DateTime.Parse(i.ApprovedDate.ToString()).ToString("dd-MMM-yyyy"),
                    ApprovedBy = i.ApprovedBy,
                    TermInvoice = i.Description,
                    Company = i.InvCompanyId,
                    Operator = i.InvOperatorID,
                    CNInfo = i.CNInfo,
                    CNFrom = i.CNFrom,
                    PICA = i.PicaTypeRequestor + " - " + i.PicaDetailRequestor,
                    PICARemarks = i.Remark,
                    ReplacementStatus = i.ReplacementStatus,
                    ReplaceDate = i.ReplaceDate,
                    ReplaceInvoice = i.ReplaceInvoice
                }), fieldList);
                table.Load(reader);
            }
                
            ExportToExcelHelper.Export("HistoryCNInvoiceARData", table);
        }

        [Route("TrxInvoiceManual/DownloadExcel")]
        public ActionResult DownloadInvoiceManualExcel()
        {
            string fileName = "TemplateUpload_InvoiceManual.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/templates/" + fileName));
            return File(bytes, contentType, fileName);
        }

        [Route("stgDocumentPayment/Export")]
        public void stgDocumentPayment(vmInvoiceMatchingAR post)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            var result = _ARPaymentservice.GetDocumentPaymentSAP(userCredential.UserID, "", "", null, null).ToList();

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"DocumentPayment",
                "Companycode", "Tanggaluangmasuk", "Totalpayment", "Currency", "Namabank",
                "Nomorrekening", "Keterangan"
            };
            var reader = FastMember.ObjectReader.Create(result.Select(i => new
            {
                DocumentPayment = i.Documentpayment,
                Companycode = i.Companycode,
                Tanggaluangmasuk = i.Tanggaluangmasuk,
                Totalpayment = i.Totalpayment,
                Currency = i.Currency,
                Namabank = i.Namabank,
                Nomorrekening = i.Nomorrekening,
                Keterangan = i.Keterangan
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("StatusPenerimaanPembayaran", table);
        }

        [Route("ReportInvoice/Export")]
        public void ExportReportInvoice(vmInvoiceMatchingAR param)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            var result = _ApprovalMonitoringSevice.GetDataInvoiceMatchingAR(userCredential.UserID, param);

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"InvoiceNumber", "PaidDate", "Company", "Operator",
                "PaidAmount", "DocumentPayment", "Ket"
            };
            var reader = FastMember.ObjectReader.Create(result.List.Select(i => new
            {
                InvoiceNumber = i.InvoiceNumber,
                PaidDate = i.InvPaidDate,
                Company = i.CompanyCodeInvoice,
                Operator = i.Customer,
                PaidAmount = i.PaidAmount,
                DocumentPayment = i.DocumentPayment,
                Ket = i.Keterangan
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ReportInvoice", table);
        }

        [Route("ReportCollection/Export")]
        public void ExportReportCollection(vmInvoiceMatchingAR param)
        {
            ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            var result = _ApprovalMonitoringSevice.GetDataCollectionMatchingAR(userCredential.UserID, param);

            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {"DocumentHeaderText", "InsertDate", "InsertTime", "CompanyCodeInvoice", "Customer",
                "CompanyCodePayment", "Documentpayment", "Tanggaluangmasuk", "Currency", "Totalpayment", "NilaiInvoice", "PaidAmount",
                "PPHAmount", "Rounding", "WAPU", "RTGS", "Penalty", "PPNExpired", "PaymentType", "Status"
            };
            var reader = FastMember.ObjectReader.Create(result.List.Select(i => new
            {
                DocumentHeaderText = i.DocumentHeaderText,
                InsertDate = i.InsertDate,
                InsertTime = i.InsertTime,
                CompanyCodeInvoice = i.CompanyCodeInvoice,
                Customer = i.Customer,
                CompanyCodePayment = i.CompanyCodePayment,
                Documentpayment = i.Documentpayment,
                Tanggaluangmasuk = i.Tanggaluangmasuk,
                Currency = i.Currency,
                Totalpayment = i.Totalpayment,
                NilaiInvoice = i.NilaiInvoice,
                PaidAmount = i.PaidAmount,
                PPHAmount = i.PPHAmount,
                Rounding = i.Rounding,
                WAPU = i.WAPU,
                RTGS = i.RTGS,
                Penalty = i.Penalty,
                PPNExpired = i.PPNExpired,
                PaymentType = i.PaymentType,
                Status = i.Status
            }), ColumsShow);
            table.Load(reader);

            //Export to Excel
            ExportToExcelHelper.Export("ReportCollection", table);
        }
        #endregion

        #region Print Invoice
        [Route("TrxPrintInvoiceTower/Print")]
        public ActionResult TrxPrintInvoiceTowerPrint()
        {
            //Parameter
            string strHeaderId = Request.QueryString["HeaderId"];
            string strCategoryId = Request.QueryString["CategoryId"];
            int[] HeaderId = strHeaderId.Split('|').Select(Int32.Parse).ToArray();
            int[] CategoryId = strCategoryId.Split('|').Select(Int32.Parse).ToArray();
            // int PicaReprintID = Request.QueryString["PicaReprintID"].ToString() == null || Request.QueryString["PicaReprintID"].ToString() == "" ? 0 : int.Parse(Request.QueryString["PicaReprintID"].ToString());

            //Call Service
            string terbilang = "";
            List<ARSystemService.vwDataPostedInvoiceTower> listDataPostedInvoice = new List<ARSystemService.vwDataPostedInvoiceTower>();
            List<PostTrxPrintInvoiceTowerHTMLData> listDataHTML = new List<PostTrxPrintInvoiceTowerHTMLData>();
            PostTrxPrintInvoiceTowerHTMLData DataHTML;
            using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                ARSystemService.vmGetInvoicePostedList vm = new ARSystemService.vmGetInvoicePostedList();
                vm.HeaderId = HeaderId;
                vm.CategoryId = CategoryId;
                listDataPostedInvoice = client.GetInvoiceListDataToList(UserManager.User.UserToken, vm).ToList();
                using (var clientPPH = new ARSystemService.ImstDataSourceServiceClient())
                {
                    ViewBag.PPHValue = clientPPH.GetPPHPercentage(UserManager.User.UserToken).PPHValue;
                    ViewBag.PPFValue = clientPPH.GetPPFPercentage(UserManager.User.UserToken).PPFValue;
                    ViewBag.PPNText = _master.GetPPNPercentage(userCredential.UserID).PPNText;
                }
                foreach (ARSystemService.vwDataPostedInvoiceTower vw in listDataPostedInvoice)
                {
                    decimal? amount = vw.InvSumADPP + vw.InvTotalAPPN - vw.InvTotalPenalty;

                    terbilang = client.GetTerbilang(UserManager.User.UserToken, amount.Value, vw.Currency);
                    listDataPostedInvoice.Where(x => x.trxInvoiceHeaderID == vw.trxInvoiceHeaderID).Select(a => { a.Terbilang = terbilang; return a; }).ToList();
                }

            }
            //string picaReprint = "";
            //if (PicaReprintID != 0)
            //{
            //    List<ARSystemService.mstPICAReprint> list = new List<ARSystemService.mstPICAReprint>();

            //    using (var client = new ARSystemService.ImstDataSourceServiceClient())
            //    {
            //        ARSystemService.mstPICAReprint pica = new ARSystemService.mstPICAReprint();
            //        list = client.GetDataPICAReprint(UserManager.User.UserToken).ToList();
            //        pica = list.Where(x => x.PICAReprintID == PicaReprintID && x.PICAReprintID == 2).FirstOrDefault();
            //        picaReprint = pica.PICAReprint == null ? "" : " -" + pica.PICAReprint;
            //    }
            //}
            foreach (ARSystemService.vwDataPostedInvoiceTower vw in listDataPostedInvoice)
            {
                DataHTML = new PostTrxPrintInvoiceTowerHTMLData();
                DataHTML.trxInvoiceHeaderID = vw.trxInvoiceHeaderID;
                DataHTML.Company = vw.Company;
                DataHTML.PrintCount = vw.PrintCount;
                DataHTML.InvNo = vw.InvNo;
                DataHTML.InvPrintDate = Helper.Helper.ConvertDateTimeToIndDate(vw.InvPrintDate.Value);
                DataHTML.OperatorDesc = vw.OperatorDesc;
                DataHTML.Address = vw.Address;
                DataHTML.Address3 = vw.Address3;
                DataHTML.Section = vw.Section;
                DataHTML.Currency = vw.Currency;
                DataHTML.InvSumADPP = vw.InvSumADPP;
                DataHTML.InvTotalDiscount = vw.InvTotalDiscount;
                DataHTML.Terbilang = vw.Terbilang;
                DataHTML.InvTotalAPPN = vw.InvTotalAPPN;
                DataHTML.InvTotalPenalty = vw.InvTotalPenalty;
                DataHTML.InvTotalAmount = vw.InvTotalAmount;
                DataHTML.IsPPHFinal = vw.IsPPHFinal;
                DataHTML.BankName = vw.BankName;
                DataHTML.AccNo = vw.AccNo;
                DataHTML.FullName = vw.FullName;
                DataHTML.Position = vw.Position;
                DataHTML.InvSubject = vw.InvSubject;
                DataHTML.InvAdditionalNote = vw.InvAdditionalNote;
                DataHTML.IsPPH = vw.IsPPH;
                DataHTML.BapsType = vw.BapsType;
                DataHTML.ReprintString = vw.ReprintString;
                //Add by ASE
                DataHTML.CompanyInvoice = vw.CompanyInvoice;
                DataHTML.CompanyInvoiceID = vw.CompanyInvoiceID;
                DataHTML.BankNameCompanyInvoice = vw.BankNameCompanyInvoice;
                DataHTML.AccNoCompanyInvoice = vw.AccNoCompanyInvoice;
                //End Add
                listDataHTML.Add(DataHTML);
            }
            return new Rotativa.ViewAsPdf("~/Views/PrintInvoice/PrintInvoiceTower.cshtml", listDataHTML)
            {
                FileName = "INVOICE_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
            };
        }

        [Route("TrxPrintInvoiceBuilding/Print")]
        public ActionResult TrxPrintInvoiceBuildingPrint()
        {
            //Parameter
            string strHeaderId = Request.QueryString["HeaderId"];
            int[] HeaderId = strHeaderId.Split('|').Select(Int32.Parse).ToArray();
            int PicaReprintID = Request.QueryString["PicaReprintID"] == null || Request.QueryString["PicaReprintID"] == "" ? 0 : int.Parse(Request.QueryString["PicaReprintID"]);

            //Call Service
            string terbilang = "";
            List<ARSystemService.vwPrintInvoiceBuilding> listDataPostedInvoice = new List<ARSystemService.vwPrintInvoiceBuilding>();
            List<PostTrxPrintInvoiceBuildingHTMLData> listDataHTML = new List<PostTrxPrintInvoiceBuildingHTMLData>();
            PostTrxPrintInvoiceBuildingHTMLData DataHTML;
            using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                listDataPostedInvoice = client.GetInvoiceBuildingListDataToList(UserManager.User.UserToken, HeaderId).ToList();
                using (var clientPPH = new ARSystemService.ImstDataSourceServiceClient())
                {
                    //ViewBag.PPHValue = clientPPH.GetPPHPercentage(UserManager.User.UserToken).PPHValue;
                    ViewBag.PPFValue = clientPPH.GetPPFPercentage(UserManager.User.UserToken).PPFValue;
                    ViewBag.PPNText = _master.GetPPNPercentage(userCredential.UserID).PPNText;

                }
                using (var client2 = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
                {
                    foreach (ARSystemService.vwPrintInvoiceBuilding vw in listDataPostedInvoice)
                    {
                        decimal? amount = vw.InvTotalAmount;
                        terbilang = client2.GetTerbilang(UserManager.User.UserToken, amount.Value, vw.Currency);
                        listDataPostedInvoice.Where(x => x.trxInvoiceHeaderID == vw.trxInvoiceHeaderID).Select(a => { a.Terbilang = terbilang; return a; }).ToList();
                    }
                }

            }
            string picaReprint = "";
            if (PicaReprintID != 0)
            {
                List<ARSystemService.mstPICAReprint> list = new List<ARSystemService.mstPICAReprint>();

                using (var client = new ARSystemService.ImstDataSourceServiceClient())
                {
                    ARSystemService.mstPICAReprint pica = new ARSystemService.mstPICAReprint();
                    list = client.GetDataPICAReprint(UserManager.User.UserToken).ToList();
                    pica = list.Where(x => x.PICAReprintID == PicaReprintID && x.PICAReprintID == 2).FirstOrDefault();
                    picaReprint = pica.PICAReprint == null ? "" : " -" + pica.PICAReprint;
                }
            }

            foreach (ARSystemService.vwPrintInvoiceBuilding vw in listDataPostedInvoice)
            {

                DataHTML = new PostTrxPrintInvoiceBuildingHTMLData();
                DataHTML.trxInvoiceHeaderID = vw.trxInvoiceHeaderID;
                DataHTML.CompanyTBG = vw.CompanyTBG;
                DataHTML.PrintCount = vw.PrintCount;
                DataHTML.InvNo = vw.InvNo;
                DataHTML.InvPrintDate = Helper.Helper.ConvertDateTimeToIndDate(vw.InvPrintDate.Value);
                DataHTML.Company = vw.Company;
                DataHTML.BillingAddress = vw.BillingAddress;
                DataHTML.RegencyName = vw.RegencyName;
                DataHTML.ProvinceName = vw.ProvinceName;
                DataHTML.Section = vw.Section;
                DataHTML.InvSubject = vw.InvSubject;
                DataHTML.Currency = vw.Currency;
                DataHTML.InvSumADPP = vw.InvSumADPP;
                DataHTML.Discount = vw.Discount;
                DataHTML.Terbilang = vw.Terbilang;
                DataHTML.InvTotalAPPN = vw.InvTotalAPPN;
                DataHTML.InvTotalPenalty = vw.InvTotalPenalty;
                DataHTML.InvTotalAmount = vw.InvTotalAmount;
                DataHTML.IsPPHFinal = vw.IsPPHFinal;
                DataHTML.BankName = vw.BankName;
                DataHTML.AccNo = vw.AccNo;
                DataHTML.FullName = vw.FullName;
                DataHTML.Position = vw.Position;
                DataHTML.ReprintString = vw.ReprintString + picaReprint;

                DataHTML.PPHValue = vw.PPHValue;
                listDataHTML.Add(DataHTML);
            }
            return new Rotativa.ViewAsPdf("~/Views/PrintInvoice/PrintInvoiceBuilding.cshtml", listDataHTML)
            {
                FileName = "INVOICE_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };
        }

        [Route("TrxPrintMergedInvoiceTower/Print")]
        public ActionResult TrxPrintMergedInvoiceTowerPrint()
        {
            //Parameter
            string strHeaderId = Request.QueryString["HeaderId"];
            int[] HeaderId = strHeaderId.Split('|').Select(Int32.Parse).ToArray();

            //Call Service
            List<ARSystemService.vwPrintMergedInvoiceOnlyTower> listDataPostedInvoice = new List<ARSystemService.vwPrintMergedInvoiceOnlyTower>();
            List<PostTrxManageMergedInvoiceOnlyTowerHTMLData> listDataHTML = new List<PostTrxManageMergedInvoiceOnlyTowerHTMLData>();
            PostTrxManageMergedInvoiceOnlyTowerHTMLData DataHTML;
            using (var client = new ARSystemService.ItrxManageMergedInvoiceOnlyTowerServiceClient())
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                listDataPostedInvoice = client.GetMergedInvoiceTowerListDataToList(UserManager.User.UserToken, HeaderId).ToList();
                using (var clientPPH = new ARSystemService.ImstDataSourceServiceClient())
                {
                    ViewBag.PPHValue = clientPPH.GetPPHPercentage(UserManager.User.UserToken).PPHValue;
                    ViewBag.PPFValue = clientPPH.GetPPFPercentage(UserManager.User.UserToken).PPFValue;
                    ViewBag.PPNText = _master.GetPPNPercentage(userCredential.UserID).PPNText;
                }
            }
            foreach (ARSystemService.vwPrintMergedInvoiceOnlyTower vw in listDataPostedInvoice)
            {
                DataHTML = new PostTrxManageMergedInvoiceOnlyTowerHTMLData();
                DataHTML.trxInvoiceHeaderID = vw.trxInvoiceHeaderID;
                DataHTML.Company = vw.Company;
                DataHTML.PrintCount = vw.PrintCount;
                DataHTML.InvNo = vw.InvNo;
                DataHTML.InvPrintDate = Helper.Helper.ConvertDateTimeToIndDate(vw.InvPrintDate.Value);
                DataHTML.OperatorDesc = vw.OperatorDesc;
                DataHTML.Address = vw.Address;
                DataHTML.Address3 = vw.Address3;
                DataHTML.Section = vw.Section;
                DataHTML.InvSubject = vw.InvSubject;
                DataHTML.Currency = vw.Currency;
                DataHTML.InvSumADPP = vw.InvSumADPP;
                DataHTML.Discount = vw.Discount;
                DataHTML.Terbilang = vw.Terbilang;
                DataHTML.InvTotalAPPN = vw.InvTotalAPPN;
                DataHTML.InvTotalPenalty = vw.InvTotalPenalty;
                DataHTML.InvTotalAmount = vw.InvTotalAmount;
                DataHTML.IsPPHFinal = vw.IsPPHFinal;
                DataHTML.BankName = vw.BankName;
                DataHTML.AccNo = vw.AccNo;
                DataHTML.FullName = vw.FullName;
                DataHTML.Position = vw.Position;
                DataHTML.InvAdditionalNote = vw.InvAdditionalNote;
                DataHTML.ReprintString = vw.ReprintString;

                listDataHTML.Add(DataHTML);
            }
            return new Rotativa.ViewAsPdf("~/Views/PrintInvoice/PrintMergedInvoiceTower.cshtml", listDataHTML)
            {
                FileName = "INVOICE_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };
        }

        [Route("TrxPrintMergedInvoiceBuilding/Print")]
        public ActionResult TrxPrintMergedInvoiceBuildingPrint()
        {
            //Parameter
            string strHeaderId = Request.QueryString["HeaderId"];
            int[] HeaderId = strHeaderId.Split('|').Select(Int32.Parse).ToArray();

            //Call Service
            List<ARSystemService.vwPrintMergedInvoiceOnlyBuilding> listDataPostedInvoice = new List<ARSystemService.vwPrintMergedInvoiceOnlyBuilding>();
            List<PostTrxManageMergedInvoiceOnlyBuildingHTMLData> listDataHTML = new List<PostTrxManageMergedInvoiceOnlyBuildingHTMLData>();
            PostTrxManageMergedInvoiceOnlyBuildingHTMLData DataHTML;
            using (var client = new ARSystemService.ItrxInvoiceBuildingDetailServiceClient())
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                listDataPostedInvoice = client.GetMergedInvoiceBuildingListDataToList(UserManager.User.UserToken, HeaderId).ToList();
                using (var clientPPH = new ARSystemService.ImstDataSourceServiceClient())
                {
                    ViewBag.PPHValue = clientPPH.GetPPHPercentage(UserManager.User.UserToken).PPHValue;
                    ViewBag.PPFValue = clientPPH.GetPPFPercentage(UserManager.User.UserToken).PPFValue;
                    ViewBag.PPNText = _master.GetPPNPercentage(userCredential.UserID).PPNText;
                }
            }
            foreach (ARSystemService.vwPrintMergedInvoiceOnlyBuilding vw in listDataPostedInvoice)
            {
                DataHTML = new PostTrxManageMergedInvoiceOnlyBuildingHTMLData();
                DataHTML.trxInvoiceHeaderID = vw.trxInvoiceHeaderID;
                DataHTML.Company = vw.Company;
                DataHTML.PrintCount = vw.PrintCount;
                DataHTML.InvNo = vw.InvNo;
                DataHTML.InvPrintDate = Helper.Helper.ConvertDateTimeToIndDate(vw.InvPrintDate.Value);
                DataHTML.CustomerName = vw.CustomerName;
                DataHTML.BillingAddress = vw.BillingAddress;
                DataHTML.RegencyName = vw.RegencyName;
                DataHTML.ProvinceName = vw.ProvinceName;
                DataHTML.Section = vw.Section;
                DataHTML.InvSubject = vw.InvSubject;
                DataHTML.Currency = vw.Currency;
                DataHTML.InvSumADPP = vw.InvSumADPP;
                DataHTML.Discount = vw.Discount;
                DataHTML.Terbilang = vw.Terbilang;
                DataHTML.InvTotalAPPN = vw.InvTotalAPPN;
                DataHTML.InvTotalPenalty = vw.InvTotalPenalty;
                DataHTML.InvTotalAmount = vw.InvTotalAmount;
                DataHTML.IsPPHFinal = vw.IsPPHFinal;
                DataHTML.BankName = vw.BankName;
                DataHTML.AccNo = vw.AccNo;
                DataHTML.FullName = vw.FullName;
                DataHTML.Position = vw.Position;
                DataHTML.ReprintString = vw.ReprintString;

                listDataHTML.Add(DataHTML);
            }
            return new Rotativa.ViewAsPdf("~/Views/PrintInvoice/PrintMergedInvoiceBuilding.cshtml", listDataHTML)
            {
                FileName = "INVOICE_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };
        }

        [Route("TrxPrintInvoiceDynamic/Print")]
        public ActionResult Print()
        {
            //Parameter

            string strCustomerID = Request.QueryString["CustomerID"];
            string strHeaderId = Request.QueryString["HeaderId"];
            int[] HeaderId = strHeaderId.Split('|').Select(Int32.Parse).ToArray();
            string htmlElements = "";
            ViewBag.HtmlElements = "";
            ARSystemService.mstARGeneratorPDF data = new ARSystemService.mstARGeneratorPDF();
            using (var client = new ARSystemService.ItrxPrintInvoiceTowerServiceClient())
            {
                for (int i = 0; i < HeaderId.Count(); i++)
                {
                    data = client.GetHTMLElements(UserManager.User.UserToken, HeaderId[i], strCustomerID);

                    if (!string.IsNullOrEmpty(data.LogoPathLeft))
                        htmlElements = data.HtmlString.Replace("ImagePath", Server.MapPath(data.LogoPathLeft));

                    if (!string.IsNullOrEmpty(data.LogoPathRight))
                        htmlElements = htmlElements.Replace("PaymentImgPath", Server.MapPath(data.LogoPathRight));

                    ViewBag.HtmlElements += i != HeaderId.Count() - 1 ? "<div style='page-break-after:always'>" : "<div>";
                    ViewBag.HtmlElements += htmlElements;
                    ViewBag.HtmlElements += "</div>";
                }

            }

            return new Rotativa.ViewAsPdf("~/Views/PrintInvoice/TrxPrintInvoiceDynamic.cshtml", ViewBag.HtmlElements)
            {
                FileName = strCustomerID + '_' + DateTime.Now.ToString("yyyyMMddhhmm") + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(Convert.ToInt16(data.MarginTop), Convert.ToInt16(data.MarginRight), Convert.ToInt16(data.MarginBottom), Convert.ToInt16(data.MarginLeft))
            };
        }
        #endregion

        #region Print Memo and Note CN Invoice

        #region Invoice Building

        [Route("TrxPrintCNInvoiceBuilding/Print")]
        public ActionResult TrxPrintCNInvoiceBuilding()
        {
            //Parameter
            int trxCNInvoiceHeaderID = int.Parse(Request.QueryString["trxCNInvoiceHeaderID"]);
            bool isApproved = Request.QueryString["isApproved"] == "1";

            //Call Service
            ARSystemService.vmPrintCNInvoiceBuilding print = new ARSystemService.vmPrintCNInvoiceBuilding();

            using (var client = new ARSystemService.ItrxApprovalCNInvoiceBuildingServiceClient())
            {
                print = client.PrintApprovalCNNoteInvoiceBuilding(UserManager.User.UserToken, trxCNInvoiceHeaderID, isApproved);
            }

            return new Rotativa.ViewAsPdf("~/Views/CN/Building/Note.cshtml", print)
            {
                FileName = "NOTE_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };
        }

        #endregion

        #region Invoice Tower

        [Route("TrxPrintCNInvoiceTower/Print")]
        public ActionResult TrxPrintCNInvoiceTower()
        {
            //Parameter
            int trxCNInvoiceHeaderID = int.Parse(Request.QueryString["trxCNInvoiceHeaderID"]);
            int mstInvoiceCategoryId = int.Parse(Request.QueryString["mstInvoiceCategoryId"]);
            bool isApproved = (Request.QueryString["isApproved"] == "1");

            //Call Service
            ARSystemService.vmPrintCNInvoiceTower print = new ARSystemService.vmPrintCNInvoiceTower();

            using (var client = new ARSystemService.ItrxApprovalCNInvoiceTowerServiceClient())
            {
                print = client.PrintApprovalCNNoteInvoiceTower(UserManager.User.UserToken, trxCNInvoiceHeaderID, mstInvoiceCategoryId, isApproved);
            }

            return new Rotativa.ViewAsPdf("~/Views/CN/Tower/Note.cshtml", print)
            {
                FileName = "NOTE_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };
        }

        #endregion

        #endregion

        #region AR Electricity
        [Authorize]
        [Route("TrxElectricityConfirm")]
        public ActionResult TrxElectricityConfirm()
        {
            string actionTokenView = "E373501F-E6F6-4B97-B9FA-43941ACF6B47";
            string actionTokenProcess = "ABF12120-ED7D-4457-911E-44C80481263A";
            string actionTokenCancel = "F8DAD8F3-3227-4F04-9435-66E66737C0FA";

            using (var client = new SecureAccessService.UserServiceClient())
            {
                if (client.CheckUserAccess(UserManager.User.UserToken, actionTokenView))
                {
                    ViewBag.AllowProcess = client.CheckUserAccess(UserManager.User.UserToken, actionTokenProcess);
                    ViewBag.AllowCancel = client.CheckUserAccess(UserManager.User.UserToken, actionTokenCancel);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }

            return View();
        }

        [Route("ExportTrxElectricity")]
        public void ExportTrxElectricity()
        {
            //Parameter
            PostTrxElectricityData post = new PostTrxElectricityData();
            post.strCompanyId = Request.QueryString["strCompanyId"];
            post.strOperator = Request.QueryString["strOperator"];
            post.strStatus = Request.QueryString["strStatus"];
            post.strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            post.strAccountName = Request.QueryString["strAccountName"];
            post.strCurrency = Request.QueryString["strCurrency"];
            post.strPONumber = Request.QueryString["strPONumber"];
            post.strAccountNumber = Request.QueryString["strAccountNumber"];
            post.strSONumber = Request.QueryString["strSONumber"];
            post.strBankName = Request.QueryString["strBankName"];
            post.isReceive = Int32.Parse(Request.QueryString["isReceive"]);
            post.strSiteIdOld = Request.QueryString["strSiteIdOld"];
            post.strSiteIDOpr = Request.QueryString["strSiteIDOpr"];
            post.strSiteName = Request.QueryString["strSiteName"];
            post.strVoucherNumber = Request.QueryString["strVoucherNumber"];
            post.isReject = int.Parse(Request.QueryString["isReject"]);
            post.strDescription = Request.QueryString["strDescription"];
            post.strRejectRemarks = Request.QueryString["strRejectRemarks"];
            post.strPICA = Request.QueryString["strPICA"];
            post.strYearPeriod = Request.QueryString["strYearPeriod"];
            post.strStartPeriod = Request.QueryString["strStartPeriod"];
            post.strEndPeriod = Request.QueryString["strEndPeriod"];
            post.strRegion = Request.QueryString["strRegion"];

            //Call Service
            List<vwElectricityData> listBAPSData = new List<vwElectricityData>();
            var client = new ElectricityService();
            string Parameters = MappingParameterElectricity(post);

            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            int intTotalRecord = client.GetDataCount(userCredential.UserID, Parameters);
            int intBatch = intTotalRecord / 50;
            List<vwElectricityData> listBAPSDataHolder = new List<vwElectricityData>();

            for (int i = 0; i <= intBatch; i++)
            {
                listBAPSDataHolder = client.GetDataToList(userCredential.UserID, Parameters, null, 50 * i, 50).ToList();
                listBAPSData.AddRange(listBAPSDataHolder);
            }


            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {
                "SONumber", "SiteIdOpr","SiteIdOld",
                "SiteName","CompanyInvoice", "Operator",
                "StartDateInvoice", "EndDateInvoice","PoNumber",
                "PPHType","IsLossPPN","TRANSDATE",
                "BankAccountNumber","BankAccountName","BankName",
                "ReferenceNumber","AmountInvoicePeriod", "VOUCHER",
                "ExpenseAdvance", "Currency","AmountExpense", "ExpDescription"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData, ColumsShow);
            table.Load(reader);

            table.Columns["SiteIdOpr"].ColumnName = "Site ID Opr";
            table.Columns["SONumber"].ColumnName = "SO Number";
            table.Columns["SiteIdOld"].ColumnName = "Site ID";
            table.Columns["SiteName"].ColumnName = "Site Name";
            table.Columns["Operator"].ColumnName = "Operator Invoice";
            table.Columns["CompanyInvoice"].ColumnName = "Company Invoice";
            table.Columns["StartDateInvoice"].ColumnName = "Start Date";
            table.Columns["EndDateInvoice"].ColumnName = "End Date";
            table.Columns["PPHType"].ColumnName = "PPH Type";
            table.Columns["IsLossPPN"].ColumnName = "Is Loss PPN";
            table.Columns["TRANSDATE"].ColumnName = "Transfer Date";
            table.Columns["BankAccountNumber"].ColumnName = "Account Number";
            table.Columns["BankAccountName"].ColumnName = "Account Name";
            table.Columns["BankName"].ColumnName = "Bank Name";
            table.Columns["ReferenceNumber"].ColumnName = "Payment Reference";
            table.Columns["AmountInvoicePeriod"].ColumnName = "Amount Total";
            table.Columns["VOUCHER"].ColumnName = "Voucher No";
            table.Columns["ExpenseAdvance"].ColumnName = "Document Number";
            table.Columns["AmountExpense"].ColumnName = "Transaction Amount";
            table.Columns["ExpDescription"].ColumnName = "Description";

            //Export to Excel
            var datenow = DateTime.Now.ToShortDateString().Replace("/", "_");
            ExportToExcelHelper.Export("TrxElectricity" + datenow, table);
        }

        [Route("ExportRejectTrxElectricity")]
        public void ExportRejectTrxElectricity()
        {
            //Parameter
            PostTrxElectricityData post = new PostTrxElectricityData();
            post.strCompanyId = Request.QueryString["strCompanyId"];
            post.strOperator = Request.QueryString["strOperator"];
            post.strStatus = Request.QueryString["strStatus"];
            post.strPeriodInvoice = Request.QueryString["strPeriodInvoice"];
            post.strAccountName = Request.QueryString["strAccountName"];
            post.strCurrency = Request.QueryString["strCurrency"];
            post.strPONumber = Request.QueryString["strPONumber"];
            post.strAccountNumber = Request.QueryString["strAccountNumber"];
            post.strSONumber = Request.QueryString["strSONumber"];
            post.strBankName = Request.QueryString["strBankName"];
            post.isReceive = Int32.Parse(Request.QueryString["isReceive"]);
            post.strSiteIdOld = Request.QueryString["strSiteIdOld"];
            post.strSiteIDOpr = Request.QueryString["strSiteIDOpr"];
            post.strSiteName = Request.QueryString["strSiteName"];
            post.strVoucherNumber = Request.QueryString["strVoucherNumber"];
            post.isReject = int.Parse(Request.QueryString["isReject"]);
            post.strDescription = Request.QueryString["strDescription"];
            post.strRejectRemarks = Request.QueryString["strRejectRemarks"];
            post.strPICA = Request.QueryString["strPICA"];
            post.strYearPeriod = Request.QueryString["strYearPeriod"];

            //Call Service
            List<vwElectricityDataReject> listBAPSData = new List<vwElectricityDataReject>();
            var client = new ElectricityService();
            string Parameters = MappingParameterElectricity(post);

            vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

            int intTotalRecord = client.GetTrxElectricityRejectCount(userCredential.UserID, Parameters);
            int intBatch = intTotalRecord / 50;
            List<vwElectricityDataReject> listBAPSDataHolder = new List<vwElectricityDataReject>();

            for (int i = 0; i <= intBatch; i++)
            {
                listBAPSDataHolder = client.GetTrxElectricityRejectToList(userCredential.UserID, Parameters, null, 50 * i, 50).ToList();
                listBAPSData.AddRange(listBAPSDataHolder);
            }


            //Convert to DataTable
            DataTable table = new DataTable();
            string[] ColumsShow = new string[] {
                "CreatedDate","SONumber","Description","RejectRemarks", "SiteIdOpr","SiteIdOld",
                "SiteName","CompanyInvoice", "Operator","BapsPeriod","YearPeriod","PoNumber",
                "InvoiceAmount","PICAStatus"
            };
            var reader = FastMember.ObjectReader.Create(listBAPSData, ColumsShow);
            table.Load(reader);

            table.Columns["SiteIdOpr"].ColumnName = "Site ID Opr";
            table.Columns["SONumber"].ColumnName = "SO Number";
            table.Columns["SiteIdOld"].ColumnName = "Site ID";
            table.Columns["SiteName"].ColumnName = "Site Name";
            table.Columns["Operator"].ColumnName = "Operator Invoice";
            table.Columns["CompanyInvoice"].ColumnName = "Company Invoice";
            table.Columns["CreatedDate"].ColumnName = "Reject Date";
            table.Columns["BapsPeriod"].ColumnName = "Period";
            table.Columns["YearPeriod"].ColumnName = "Year";
            table.Columns["PoNumber"].ColumnName = "Exp Number";
            table.Columns["InvoiceAmount"].ColumnName = "Amount Invoice";
            table.Columns["PICAStatus"].ColumnName = "Status";

            //Export to Excel
            var datenow = DateTime.Now.ToShortDateString().Replace("/", "_");
            ExportToExcelHelper.Export("RejectTrxElectricity" + datenow, table);
        }
        private string MappingParameterElectricity(PostTrxElectricityData post)
        {
            string strWhereClause = "";

            if (post.isReject > 0)
            {
                strWhereClause = "mstInvoiceStatusId IN (0,18) AND "; //TAB BAPS REJECT 
            }
            else
            {
                if (post.isReceive == 1)
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.NotProcessed + " AND "; //TAB BAPS RECEIVE 
                else if (post.isReceive > 1)
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.StateBAPSConfirm + " AND InvoiceDate IS NOT NULL AND "; //TAB BAPS CONFIRM & DONE INV 
                else
                    strWhereClause = "mstInvoiceStatusId = " + (int)StatusHelper.InvoiceStatus.StateBAPSReceive + " AND "; //TAB BAPS CONFIRM 
            }


            if (!string.IsNullOrWhiteSpace(post.strCompanyId))
            {
                strWhereClause += "CompanyId LIKE '%" + post.strCompanyId.TrimStart().TrimEnd() + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strOperator))
            {
                strWhereClause += "Operator LIKE '%" + post.strOperator.TrimStart().TrimEnd() + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteName))
            {
                strWhereClause += "SiteName LIKE '%" + post.strSiteName + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strAccountNumber))
            {
                strWhereClause += "AccountNumber LIKE '%" + post.strAccountNumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strAccountName))
            {
                strWhereClause += "AccountName LIKE '%" + post.strAccountName + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strBankName))
            {
                strWhereClause += "BankName LIKE '%" + post.strBankName + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPONumber))
            {
                strWhereClause += "PoNumber LIKE '%" + post.strPONumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteIDOpr))
            {
                strWhereClause += "SiteIDOpr LIKE '%" + post.strSiteIDOpr + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSONumber))
            {
                strWhereClause += "SONumber LIKE '%" + post.strSONumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strVoucherNumber))
            {
                strWhereClause += "Voucher LIKE '%" + post.strVoucherNumber + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strSiteIdOld))
            {
                strWhereClause += "SiteIdOld LIKE '%" + post.strSiteIdOld + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strDescription))
            {
                strWhereClause += "ExpDescription LIKE '%" + post.strDescription + "%' AND ";
            }

            if (!string.IsNullOrWhiteSpace(post.strStatus))
            {
                strWhereClause += "PICAStatus LIKE '%" + post.strStatus + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPICA))
            {
                strWhereClause += "Description LIKE '%" + post.strPICA + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strRejectRemarks))
            {
                strWhereClause += "RejectRemarks LIKE '%" + post.strRejectRemarks + "%' AND ";
            }
            if (!string.IsNullOrWhiteSpace(post.strPeriodInvoice))
            {
                strWhereClause += "BapsPeriod LIKE '%" + post.strPeriodInvoice + "%' AND ";
            }

            if (!string.IsNullOrWhiteSpace(post.strRegion))
            {
                strWhereClause += "Regional LIKE '%" + post.strRegion.TrimStart().TrimEnd() + "%' AND ";
            }

            if (!string.IsNullOrWhiteSpace(post.strStartPeriod))
            {
                strWhereClause += ("YearPeriod BETWEEN " + post.strStartPeriod + " AND " + post.strEndPeriod + " AND ");
            }

            if (!string.IsNullOrWhiteSpace(post.strYearPeriod))
            {
                strWhereClause += "YearPeriod LIKE '%" + post.strYearPeriod + "%' AND ";
            }

            strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

            return strWhereClause;
        }

        #endregion
    }
}