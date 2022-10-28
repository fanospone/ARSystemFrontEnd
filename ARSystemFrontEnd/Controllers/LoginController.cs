using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ARSystemFrontEnd.Providers;
using ARSystemFrontEnd.SecureAccessService;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("Login")]
    public class LoginController : Controller
    {
        [AllowAnonymous]
        [Route("")]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                string strYear = "2017";
                if (DateTime.Now.Year.ToString() != strYear)
                {
                    strYear += "-" + DateTime.Now.Year.ToString();
                }
                ViewBag.CopyrightYear = strYear;

                return View();
            }
        }

        [AllowAnonymous]
        [Route("Login")]
        public ActionResult Login(SecureAccessService.vmLogin login)
        {
            login.UserID = login.UserID.Trim();
            login.IP = Helper.Helper.GetIPAddress(); 
            login.Application = Helper.Helper.GetApplicationName();
            
            SecureAccessService.vmUserLogin userLogin = new SecureAccessService.vmUserLogin();

            using (var client = new SecureAccessService.UserServiceClient())
            {
                userLogin = client.LoginValidation(login);
                if (!string.IsNullOrWhiteSpace(userLogin.ErrorMessage))
                {
                    return Json(userLogin);
                }
                else
                {
                    if (UserManager.ValidateUser(login, Response))
                    {
						//cookie for redirect to legacy system
                        HttpCookie myCookiePassword = new HttpCookie("pid");
                        HttpCookie myCookieId = new HttpCookie("uid");
                        myCookiePassword.Value = Encrypt(login.Password);
                        Response.Cookies.Add(myCookiePassword);
                        myCookieId.Value = Encrypt(login.UserID);
                        Response.Cookies.Add(myCookieId);
                        //end
                        return Json(userLogin);
                    }
                    else
                    {
                        userLogin.ErrorMessage = "Username or Password is incorrect!";
                        return Json(userLogin);
                    }
                }
            }
        }

        [Route("PasswordExpired")]
        public ActionResult PasswordExpired()
        {
            string strYear = "2017";
            if (DateTime.Now.Year.ToString() != strYear)
            {
                strYear += "-" + DateTime.Now.Year.ToString();
            }
            ViewBag.CopyrightYear = strYear;

            SecureAccessService.mstUser user = new SecureAccessService.mstUser();
            using (var client = new SecureAccessService.UserServiceClient())
            {
                user = client.GetUserSingle(UserManager.User.UserToken);
            }

            ViewBag.UserID = user.UserID;

            return View("");
        }

        [Route("Logout")]
        public ActionResult Logout()
        {
            DeleleteCookies();
            UserManager.Logout(Session, Response);
            //return RedirectToAction("Index", "Login");
            return RedirectToAction("Redirect", "Login", new
            {
                returnUrl = "Logout",
                actLogout = "legacy"
            });
        }

        [Route("OTPRequestForceLogin")]
        [AllowAnonymous]
        public ActionResult OTPRequestForceLogin(trxOTP otp)
        {
            using (var client = new UserServiceClient())
            {
                otp.Application = Helper.Helper.GetApplicationName();
                otp.ApplicationID = Helper.Helper.GetApplicationID();
                otp.OTPTypeID = 1;
                otp.IP = Helper.Helper.GetIPAddress();
                return Json(client.OTPForceLogin(otp));
            }
        }

        [Route("OTPConfirmForceLogin")]
        [AllowAnonymous]
        public ActionResult OTPConfirmForceLogin(trxOTP otp)
        {
            using (var client = new UserServiceClient())
            {
                otp.Application = Helper.Helper.GetApplicationName();
                otp.ApplicationID = Helper.Helper.GetApplicationID();
                otp.OTPTypeID = 1;
                otp.IP = Helper.Helper.GetIPAddress();
                return Json(client.OTPCConfirmForceLogin(otp));
            }
        }

        [AllowAnonymous]
        [Route("Redirect")]
        public ActionResult Redirect(string uid, string pid, string returnUrl, string actLogout)
        {
            string strYear = "2017";
            if (DateTime.Now.Year.ToString() != strYear)
            {
                strYear += "-" + DateTime.Now.Year.ToString();
            }
            ViewBag.CopyrightYear = strYear;

            if (returnUrl == "Logout" && actLogout == "new") //log out from legacy
            {
                DeleleteCookies();
                if (User.Identity.IsAuthenticated)
                {
                    UserManager.Logout(Session, Response);
                }
                ViewBag.actLogout = actLogout;
                ViewBag.LegacyUrl = System.Configuration.ConfigurationManager.AppSettings["TBGSysV1URL"].ToString();
                return View("~/Views/Legacy/Index.cshtml");
            }
            else if (returnUrl == "Logout" && actLogout == "legacy") //log out from legacy
            {
                ViewBag.actLogout = actLogout;
                ViewBag.LegacyUrl = System.Configuration.ConfigurationManager.AppSettings["TBGSysV1URL"].ToString();
                return View("~/Views/Legacy/Index.cshtml");
            }
            else if (User.Identity.IsAuthenticated)
            {
                ViewBag.IsAuthenticated = 1;
                ViewBag.Link = returnUrl;
            }
            else
            {
                if (!String.IsNullOrEmpty(uid) && !String.IsNullOrEmpty(pid))
                {
                    ViewBag.UserPassword = Decrypt(pid);
                    ViewBag.UserID = Decrypt(uid);
                    ViewBag.Link = returnUrl;
                }
            }
            return View("~/Views/Login/Index.cshtml");
        }

        #region Private
        private static string Encrypt(string strToEncrypt)
        {
            string strEncrypted = "";
            byte[] data;
            data = System.Text.ASCIIEncoding.ASCII.GetBytes(strToEncrypt);
            strEncrypted = System.Convert.ToBase64String(data);
            return strEncrypted;
        }
        private static string Decrypt(string strToDecrypt)
        {
            string strDecrypted = "";
            byte[] data;
            data = System.Convert.FromBase64String(strToDecrypt);
            strDecrypted = System.Text.ASCIIEncoding.ASCII.GetString(data);
            return strDecrypted;
        }

        private void DeleleteCookies()
        {
            HttpCookie myCookiePassword = new HttpCookie("pid");
            HttpCookie myCookieId = new HttpCookie("uid");
            myCookiePassword.Expires = DateTime.Now.AddDays(-1D);
            myCookieId.Expires = DateTime.Now.AddDays(-1D); ;
            Response.Cookies.Add(myCookiePassword);
            Response.Cookies.Add(myCookieId);

        }
        #endregion
    }
}