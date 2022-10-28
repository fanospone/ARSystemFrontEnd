using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Copyright
            string strYear = "2017";
            if (DateTime.Now.Year.ToString() != strYear)
            {
                strYear += "-" + DateTime.Now.Year.ToString();
            }
            ViewBag.CopyrightYear = strYear;

            // User
            ViewBag.UserLogin = UserManager.User;
            
            // Menu
            List<SecureAccessService.vwUserMenu> userMenu = new List<SecureAccessService.vwUserMenu>();
            using (var client = new SecureAccessService.AccessServiceClient())
            {
                userMenu = client.GetMenuToListFilteredByUser(UserManager.User.UserToken, Helper.Helper.GetApplicationID()).ToList();
            }

            ViewBag.MenuList = userMenu;

            //Hit Counter
            Helper.Helper.HitLog(filterContext.RouteData);
        }
    }
}