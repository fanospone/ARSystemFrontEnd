using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Data.SqlClient;

namespace ARSystemFrontEnd.Providers
{
    public class UserManager
    {
        public static SecureAccessService.vmUserLogin User
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return ((ARPrincipal)(HttpContext.Current.User)).User;
                }
                else if (HttpContext.Current.Items.Contains("UserLoginARSystem"))
                {
                    return (SecureAccessService.vmUserLogin)HttpContext.Current.Items["UserLoginARSystem"];
                }
                else
                {
                    return null;
                }
            }
        }

        public static SecureAccessService.vmUserLogin AuthenticateUser(string strUserID, string strPassword)
        {
            SecureAccessService.vmUserLogin userLogin = new SecureAccessService.vmUserLogin();

            using (var client = new SecureAccessService.UserServiceClient())
            {
                SecureAccessService.vmLogin login = new SecureAccessService.vmLogin();
                login.UserID = strUserID;
                login.Password = strPassword;
                login.IP = Helper.Helper.GetIPAddress();
                login.Application = Helper.Helper.GetApplicationName();
                userLogin = client.Login(login);
            }

            userLogin.Application = Helper.Helper.GetApplicationName();
            return userLogin;
        }

        public static bool ValidateUser(SecureAccessService.vmLogin login, HttpResponseBase response)
        {
            bool result = false;

            if (Membership.ValidateUser(login.UserID, login.Password))
            {
                var serializer = new JavaScriptSerializer();
                string userData = serializer.Serialize(UserManager.User);

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        login.UserID,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(Helper.Helper.GetIdleTime()),
                        true,
                        userData,
                        FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(ticket);
                response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                result = true;
            }

            return result;
        }

        public static void Logout(HttpSessionStateBase session, HttpResponseBase response)
        {
            session.Abandon();

            using (var client = new SecureAccessService.UserServiceClient())
            {
                client.Logout(UserManager.User.UserToken);
            }

            FormsAuthentication.SignOut();

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            response.Cookies.Add(cookie);
        }
    }
}