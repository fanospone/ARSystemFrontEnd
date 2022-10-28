using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Providers;
using ARSystemFrontEnd.SecureAccessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ARSystemFrontEnd.ControllerApis.General.DashboardNavigationMenu
{
    [RoutePrefix("api/dashboardNavigationMenu")]
    public class ApiDashboardNavigationMenuController : ApiController
    {
        [HttpGet, Route("getMenuList")]
        public IHttpActionResult GetMenuList(string keyword)
        {
            List<vwUserMenu> userMenu = new List<vwUserMenu>();
            List<vwApplicationUrl> appList = new List<vwApplicationUrl>();
            string html = "";
            //var applicationList = GetApplicationList();
            if (UserManager.User != null)
            {
                using (var client = new DashboardNavigationMenuServiceClient())
                {
                    appList = client.GetApplicationToListFilteredByUser(UserManager.User.UserToken).ToList();
                }
                foreach (var apl in appList)
                {
                    using (var client = new AccessServiceClient())
                    {
                        userMenu = client.GetMenuToListFilteredByUser(UserManager.User.UserToken, apl.ApplicationID).ToList();
                    }

                    html += GenerateSearchForm(userMenu, keyword, apl, false, true);
                }
            }

            return Ok(html);
        }

        [HttpGet, Route("getApplicationList")]
        public IHttpActionResult GetApplicationListByUser()
        {
            List<vwApplicationUrl> appList = new List<vwApplicationUrl>();
            if (UserManager.User != null)
            {
                using (var client = new DashboardNavigationMenuServiceClient())
                {
                    appList = client.GetApplicationToListFilteredByUser(UserManager.User.UserToken).ToList();
                }
            }

            return Ok(appList);
        }

        [HttpGet, Route("getFavoriteMenuList")]
        public IHttpActionResult GetFavoriteMenuList()
        {
            List<vwUserMenu> userMenu = new List<vwUserMenu>();
            List<vwApplicationUrl> appList = new List<vwApplicationUrl>();
            string html = "";

            if (UserManager.User != null)
            {
                using (var client = new DashboardNavigationMenuServiceClient())
                {
                    appList = client.GetApplicationToListFilteredByUser(UserManager.User.UserToken).ToList();

                    var favoriteList = client.GetFavoriteMenuList(UserManager.User.UserToken).ToList();
                    var distinctAppId = favoriteList.GroupBy(x => x.ApplicationID).Select(g => g.First()).ToList();
                    foreach (var a in distinctAppId)
                    {
                        //get menu list by app id
                        using (var accessClient = new AccessServiceClient())
                        {
                            userMenu = accessClient.GetMenuToListFilteredByUser(UserManager.User.UserToken, a.ApplicationID).ToList();
                        }
                        var applicationUrl = appList.Where(x => x.ApplicationID == a.ApplicationID).FirstOrDefault();
                        foreach (var favorite in favoriteList)
                        {
                            if (favorite.ApplicationID == a.ApplicationID)
                                html += GenerateFavoriteForm(userMenu, favorite, applicationUrl, true, false);

                        }

                    }
                }
            }

            return Ok(html);
        }

        [HttpPost, Route("createFavoriteMenu")]
        public IHttpActionResult CreateFavoriteMenu(trxFavoriteMenu request)
        {
            var result = new trxFavoriteMenu();
            if (UserManager.User != null)
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                using (var client = new DashboardNavigationMenuServiceClient())
                {
                    var data = client.GetFavoriteMenuList(UserManager.User.UserToken);
                    if (data.Where(x => x.mstMenuID == request.mstMenuID && x.UserID == userCredential.UserID).Count() == 0)
                        result = client.CreateFavoriteMenu(UserManager.User.UserToken, request);
                }
            }
            return Ok(result);
        }

        [HttpPost, Route("deleteFavoriteMenu")]
        public IHttpActionResult DeleteFavoriteMenu(trxFavoriteMenu request)
        {
            var result = new bool();
            if (UserManager.User != null)
            {
                using (var client = new DashboardNavigationMenuServiceClient())
                {
                    result = client.DeleteFavoriteMenu(UserManager.User.UserToken, request);
                }
            }
            return Ok(result);
        }


        #region Private
        private string GenerateSearchForm(List<vwUserMenu> menus, string keyword, vwApplicationUrl application, bool isFavorite, bool isSearch)
        {
            var filterMenu = menus.Where(x => x.Menu.ToUpper().Contains(keyword.ToUpper())).ToList();
            string form = "";
            var listMenus = new List<vwUserMenu>();
            foreach (var filter in filterMenu)
            {
                var listMenu = new List<vwUserMenu>();
                listMenu = GetChildMenu(menus, listMenu, filter);
                foreach (var child in listMenu)
                {
                    listMenus.Add(child);
                }
            }

            var childMenu = listMenus.GroupBy(x => x.mstMenuID).Select(g => g.First()).ToList();
            foreach (var child in childMenu)
            {
                string breadcrumb = BreadCrumb(menus, child, keyword, application, isSearch);
                using (var client = new DashboardNavigationMenuServiceClient())
                {
                    var favorite = client.GetFavoriteMenuList(UserManager.User.UserToken);
                    isFavorite = favorite.Where(x => x.mstMenuID == child.mstMenuID).ToList().Count > 0 ? true : false;
                }
                form += GenerateForm(breadcrumb, child, application, isFavorite, isSearch);
            }
            return form;
        }

        private string GenerateFavoriteForm(List<vwUserMenu> menus, trxFavoriteMenu favorite, vwApplicationUrl application, bool isFavorite, bool isSearch)
        {
            var filterMenu = menus.Where(x => x.mstMenuID == favorite.mstMenuID).FirstOrDefault();
            string form = "";

            var listMenu = new List<vwUserMenu>();
            listMenu = GetChildMenu(menus, listMenu, filterMenu);
            foreach (var child in listMenu)
            {
                string breadcrumb = BreadCrumb(menus, child, "", application, isSearch);
                form += GenerateForm(breadcrumb, child, application, isFavorite, isSearch);
            }
            return form;
        }


        private string BreadCrumb(List<vwUserMenu> menus, vwUserMenu child, string keyword, vwApplicationUrl application, bool isSearch)
        {
            List<vwUserMenu> menuActives = new List<vwUserMenu>();
            vwUserMenu menu = menus.Where(i => i.Url == child.Url).FirstOrDefault();

            //Recursive to Get Menu Active
            if (menu != null)
            {
                menuActives = GetMenuActive(menus, menuActives, Convert.ToInt64(menu.MenuParentID));
                menuActives.Add(menu);
            }

            //Generate BreadCrumb
            string strBreadCrumb = "";
            vwUserMenu menuActiveLast = menuActives.Last();
            foreach (vwUserMenu menuActive in menuActives)
            {
                System.Web.Mvc.TagBuilder liTag = new System.Web.Mvc.TagBuilder("li");
                System.Web.Mvc.TagBuilder spanTag = new System.Web.Mvc.TagBuilder("span");
                if (menuActive.Menu.ToUpper().Contains(keyword.ToUpper()) && isSearch)
                    spanTag.AddCssClass("keyword");
                spanTag.InnerHtml = menuActive.Menu;

                if (menuActive.Equals(menuActiveLast))
                {
                    liTag.InnerHtml = spanTag.ToString();
                }
                else
                {
                    System.Web.Mvc.TagBuilder iTag = new System.Web.Mvc.TagBuilder("i");
                    iTag.AddCssClass("fa fa-angle-right");
                    liTag.InnerHtml = spanTag.ToString() + iTag.ToString();
                }

                strBreadCrumb += liTag.ToString();
            }

            return ParentBreadcrumb(application.ApplicationName) + strBreadCrumb;
        }

        private string ParentBreadcrumb(string app)
        {
            System.Web.Mvc.TagBuilder liTag = new System.Web.Mvc.TagBuilder("li");
            System.Web.Mvc.TagBuilder spanTag = new System.Web.Mvc.TagBuilder("span");
            spanTag.InnerHtml = app;

            System.Web.Mvc.TagBuilder iTag = new System.Web.Mvc.TagBuilder("i");
            iTag.AddCssClass("fa fa-angle-right");

            liTag.InnerHtml = spanTag.ToString() + iTag.ToString();

            return liTag.ToString();
        }

        private List<vwUserMenu> GetChildMenu(List<vwUserMenu> menus, List<vwUserMenu> listMenu, vwUserMenu menu)
        {
            var menul = menus.Where(i => i.MenuParentID == menu.mstMenuID).ToList();
            if (menul.Count > 0)
            {
                foreach (var a in menul)
                {
                    listMenu = GetChildMenu(menus, listMenu, a);
                }
            }
            else
                listMenu.Add(menu);

            return listMenu;
        }

        private string GenerateForm(string breadcrumb, vwUserMenu menu, vwApplicationUrl application, bool isFavorite, bool isSearch)
        {
            string col = isFavorite && !isSearch ? "col-md-12" : "col-md-6";
            string favorite = isFavorite ? "favorite" : "";
            string attr = isFavorite ? "uncheck-fav" : "check-fav";
            string html = "<div class='" + col + "'><div class='mt-comment shadow-2'><div class='mt-comment-body'><div class='mt-comment-info'><div class='page-menus'><ul class='page-breadcrumb'>" + breadcrumb + "</ul></div>" +
                "<span class='mt-comment-date'><i class='fa fa-star fa-lg " + attr + " " + favorite + "' data-menu-id='" + menu.mstMenuID + "' data-app-id='" + menu.ApplicationID + "'></i></span></div><div class='mt-comment-text'><a class='list-menu' href='" + application.ApplicationURL + menu.Url + "' target='_blank'>" + menu.Menu + " </a></div></div></div></div>";
            return html;
        }

        private List<vwUserMenu> GetMenuActive(List<vwUserMenu> menus, List<vwUserMenu> menuActives, long lngParentID)
        {
            if (lngParentID > 0)
            {
                vwUserMenu menu = menus.Where(i => i.mstMenuID == lngParentID).SingleOrDefault();
                if (menu != null)
                {
                    if (menu.MenuParentID > 0)
                        menuActives = GetMenuActive(menus, menuActives, Convert.ToInt64(menu.MenuParentID));
                    menuActives.Add(menu);
                }
            }

            return menuActives;
        }
        #endregion
    }
}
