
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


namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("ISPInvoiceInformation")]
    public class ISPInvoiceInformationController : BaseController
    {
        private readonly ISPInvoiceInformationService _ISPService;
        public ISPInvoiceInformationController()
        {
            _ISPService = new ISPInvoiceInformationService();
        }

        [Authorize]
        [Route("")]
        // GET: RevenueSystem
        public ActionResult Index()
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            return View();
        }


        [Route("InvoiceInformation")]
        public ActionResult InvoiceInformation()
        {

            string actionTokenView = "CD560FB0-4F5B-4694-A311-4796BB7A91CB";
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
        [Route("InvoiceInformationDetail/{SONumber?}")]
        public ActionResult InvoiceInformationDetail (string SONumber)
        {
            if (UserManager.User.IsPasswordExpired)
                return RedirectToAction("PasswordExpired", "Login");

            ViewBag.SONumber = SONumber;
            return View();
        }

        [Route("ExporInvoiceInformationList")]
        public void ExporInvoiceInformationList()
        {
            var post = new PostISPInvoiceInformation()
            {

                slCompany = Request.QueryString["slCompany"].ToString(),
                slCustomer = Request.QueryString["slCustomer"].ToString(),
                slStipCode = Request.QueryString["slStipCode"].ToString(),
                fSONumber = Request.QueryString["fSONumber"].ToString(),
                fSiteID = Request.QueryString["fSiteID"].ToString(),
                fSiteName = Request.QueryString["fSiteName"].ToString(),
                fSiteIDOpr = Request.QueryString["fSiteIDOpr"].ToString(),
                fSiteNameOpr = Request.QueryString["fSiteNameOpr"].ToString()

            };
  
            string[] arrInvoiceList = new string[] {
                        "SONumber",
                        "SiteID",
                        "SiteName",
                        "CustomerSiteID",
                        "CustomerSiteName",
                        "Company",
                        "CustomerName",
                        "STIPDescription"
                    };

            string strWhereClause = "1=1";
            if (!string.IsNullOrWhiteSpace(post.slCompany))
            {
                strWhereClause += $" AND CompanyID Like '%{post.slCompany.Trim()}%'";
            }
            if (!string.IsNullOrWhiteSpace(post.slStipCode))
            {
                strWhereClause += $" AND STIPCode Like '%{post.slStipCode}%'";
            }
            if (!string.IsNullOrWhiteSpace(post.slCustomer))
            {
                strWhereClause += $" AND OperatorID Like '%{post.slCustomer}%'";
            }
            if (!string.IsNullOrWhiteSpace(post.fSONumber))
            {
                strWhereClause += $" AND SONumber Like '%{post.fSONumber}%'";
            }
            if (!string.IsNullOrWhiteSpace(post.fSiteID))
            {
                strWhereClause += $" AND SiteID Like '%{post.fSiteID}%'";
            }
            if (!string.IsNullOrWhiteSpace(post.fSiteName))
            {
                strWhereClause += $" AND SiteName Like '%{post.fSiteName}%'";
            }
            if (!string.IsNullOrWhiteSpace(post.fSiteIDOpr))
            {
                strWhereClause += $" AND CustomerSiteID Like '%{post.fSiteIDOpr}%'";
            }
            if (!string.IsNullOrWhiteSpace(post.fSiteNameOpr))
            {
                strWhereClause += $" AND CustomerSiteName Like '%{post.fSiteNameOpr}%'";
            }
           
           
            var datalist = _ISPService.GetISPInvoiceInformationList(strWhereClause, post.start, 0).ToList();
            var reader = FastMember.ObjectReader.Create(datalist, arrInvoiceList);
            DataTable table = new DataTable();
            table.Load(reader);

            //Export to Excel: filename Max 31 Char
            ExportToExcelHelper.Export("InvoiceInformationList", table);
        }

    }

}