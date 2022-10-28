using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
			MvcHandler.DisableMvcResponseHeader = true;
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                // Get the forms authentication ticket.
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var identity = new GenericIdentity(authTicket.Name, "Forms");
                var principal = new ARPrincipal(identity);

                // Get the custom user data encrypted in the ticket.
                string userData = ((FormsIdentity)(Context.User.Identity)).Ticket.UserData;

                // Deserialize the json data and set it on the custom principal.
                var serializer = new JavaScriptSerializer();
                UserLogin userLogin = (UserLogin)serializer.Deserialize(userData, typeof(UserLogin));

                SecureAccessService.vmUserLogin userLoginContext = new SecureAccessService.vmUserLogin();
                userLoginContext.UserToken = userLogin.UserToken;
                userLoginContext.UserFullName = userLogin.UserFullName;
                userLoginContext.UserRole = userLogin.UserRole;
                userLoginContext.Application = userLogin.Application;
                userLoginContext.IsPasswordExpired = userLogin.IsPasswordExpired;
                // Modification Or Added By Ibnu Setiawan 07. September 2020
                userLoginContext.CompanyCode = userLogin.CompanyCode;
                // End Modification Or Added By Ibnu Setiawan 07. September 2020
                principal.User = userLoginContext;

                // Set the context user.
                Context.User = principal;
            }
        }
		
		protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app != null && app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
            }
        }
    }
}
