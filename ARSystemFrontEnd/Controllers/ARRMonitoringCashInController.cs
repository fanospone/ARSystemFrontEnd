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
using System.Threading.Tasks;
using ARSystem.Service;

namespace ARSystemFrontEnd.Controllers
{
    [Route("{action=Index}")]
    [RoutePrefix("MonitoringCashIn")]
    public class ARRMonitoringCashInController : BaseController
    {
        [Authorize]
        [Route("Quarterly")]
        public ActionResult Quarterly()
        {
            string actionTokenView = "eb7fa593-608a-4b91-9132-2af3b6a598e2";
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
        [Route("Monthly")]
        public ActionResult Monthly()
        {
            string actionTokenView = "e14d52b9-7674-41d6-89a7-49deba6ab6be";


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
        [Route("Weekly")]
        public ActionResult Weekly()
        {
            string actionTokenView = "49da2c69-22de-4028-aa39-e3910bbdbcae";
           

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
        [Route("WeeklyApproval")]
        public ActionResult WeeklyApproval()
        {
            string actionTokenView = "fc128a2b-613f-4ff8-a576-97ee937ccbce";


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
        [Route("MonthlyApproval")]
        public ActionResult MonthlyApproval()
        {
            string actionTokenView = "3be4e167-6b13-4d1d-a474-ead5761bad96";
            

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
        [Route("QuarterlyApproval")]
        public ActionResult QuarterlyApproval()
        {
            string actionTokenView = "eb7fa593-608a-4b91-9132-2af3b6a598e2";


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
    }
        
}