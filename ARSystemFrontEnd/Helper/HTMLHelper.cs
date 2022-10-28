using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ARSystemFrontEnd.Helper
{
    public static class HTMLHelper
    {
        public static MvcHtmlString MenuList(this HtmlHelper helper, List<SecureAccessService.vwUserMenu> menus)
        {
            string url = HttpContext.Current.Request.Url.AbsolutePath;
            List<SecureAccessService.vwUserMenu> menuActives = new List<SecureAccessService.vwUserMenu>();
            SecureAccessService.vwUserMenu menu = menus.Where(i => i.Url == url).SingleOrDefault();

            //Recursive to Get Menu Active
            if (menu != null)
            {
                menuActives = GetMenuActive(menus, menuActives, Convert.ToInt64(menu.MenuParentID));
                menuActives.Add(menu);
            }

            //Generate Menu
            /*TagBuilder liTag = new TagBuilder("li");
            liTag.AddCssClass("nav-item start");
            if (url == "/Dashboard")
                liTag.AddCssClass("active");

            TagBuilder aTag = new TagBuilder("a");
            aTag.AddCssClass("nav-link");
            aTag.MergeAttribute("href", "/Dashboard");

            TagBuilder iTag = new TagBuilder("i");
            iTag.AddCssClass("fa fa-home");

            TagBuilder spanTag = new TagBuilder("span");
            spanTag.AddCssClass("title");
            spanTag.InnerHtml = "Dashboard";

            aTag.InnerHtml = iTag.ToString() + spanTag.ToString();
            liTag.InnerHtml = aTag.ToString();*/

            //return MvcHtmlString.Create(liTag.ToString() + GenerateMenu(menus, menuActives, 0, true));
            return MvcHtmlString.Create("" + GenerateMenu(menus, menuActives, 0, true));
        }

        public static MvcHtmlString BreadCrumb(this HtmlHelper helper, List<SecureAccessService.vwUserMenu> menus)
        {
            string url = HttpContext.Current.Request.Url.AbsolutePath;
            List<SecureAccessService.vwUserMenu> menuActives = new List<SecureAccessService.vwUserMenu>();
            SecureAccessService.vwUserMenu menu = menus.Where(i => i.Url == url).SingleOrDefault();

            //Recursive to Get Menu Active
            if (menu != null)
            {
                menuActives = GetMenuActive(menus, menuActives, Convert.ToInt64(menu.MenuParentID));
                menuActives.Add(menu);
            }

            //Generate BreadCrumb
            string strBreadCrumb = "";
            if (url == "/Dashboard")
            {
                TagBuilder liTag = new TagBuilder("li");
                TagBuilder aTag = new TagBuilder("a");
                aTag.MergeAttribute("href", "/Dashboard");
                aTag.InnerHtml = "Dashboard";

                liTag.InnerHtml = aTag.ToString();
                strBreadCrumb = liTag.ToString();
            }
            else if (menuActives.Count() > 0)
            {
                SecureAccessService.vwUserMenu menuActiveLast = menuActives.Last();
                foreach (SecureAccessService.vwUserMenu menuActive in menuActives)
                {
                    TagBuilder liTag = new TagBuilder("li");

                    if (menuActive.Equals(menuActiveLast))
                    {
                        TagBuilder spanTag = new TagBuilder("span");
                        spanTag.AddCssClass("active");
                        spanTag.InnerHtml = menuActive.Menu;
                        liTag.InnerHtml = spanTag.ToString();
                    }
                    else
                    {
                        TagBuilder aTag = new TagBuilder("a");
                        if (!string.IsNullOrWhiteSpace(menuActive.Url))
                            aTag.MergeAttribute("href", menuActive.Url);
                        aTag.InnerHtml = menuActive.Menu;

                        TagBuilder iTag = new TagBuilder("i");
                        iTag.AddCssClass("fa fa-circle");

                        liTag.InnerHtml = aTag.ToString() + iTag.ToString();
                    }

                    strBreadCrumb += liTag.ToString();
                }
            }

            return MvcHtmlString.Create(strBreadCrumb);
        }

        #region "Private"

        private static List<SecureAccessService.vwUserMenu> GetMenuActive(List<SecureAccessService.vwUserMenu> menus, List<SecureAccessService.vwUserMenu> menuActives, long lngParentID)
        {
            if (lngParentID > 0)
            {
                SecureAccessService.vwUserMenu menu = menus.Where(i => i.mstMenuID == lngParentID).SingleOrDefault();
                if (menu != null)
                {
                    if (menu.MenuParentID > 0)
                        menuActives = GetMenuActive(menus, menuActives, Convert.ToInt64(menu.MenuParentID));
                    menuActives.Add(menu);
                }
            }

            return menuActives;
        }

        private static string GenerateMenu(List<SecureAccessService.vwUserMenu> menus, List<SecureAccessService.vwUserMenu> menuActives, long lngParentID, bool bolIsFirst)
        {
            string strMenuHolder = "";
            int intCount = menus.Where(i => i.MenuParentID == lngParentID).Count();

            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("sub-menu");

            foreach (SecureAccessService.vwUserMenu menu in menus.Where(i => i.MenuParentID == lngParentID).OrderBy(i => i.Sort).ToList())
            {
                string strMenuPart = "";

                TagBuilder liTag = new TagBuilder("li");
                liTag.AddCssClass("nav-item");

                if (menuActives.Where(i => i.mstMenuID == menu.mstMenuID).Count() > 0)
                {
                    liTag.AddCssClass("active");
                    liTag.AddCssClass("open");
                }
                TagBuilder aTag = new TagBuilder("a");
                if (!string.IsNullOrWhiteSpace(menu.Url))
                    aTag.MergeAttribute("href", menu.Url);
                else
                    aTag.MergeAttribute("href", "javascript:;");
                aTag.AddCssClass("nav-link");

                if (!string.IsNullOrWhiteSpace(menu.FaIcon))
                {
                    TagBuilder iTag = new TagBuilder("i");
                    iTag.AddCssClass("fa " + menu.FaIcon);
                    strMenuPart += iTag.ToString() + " ";
                }

                TagBuilder spanTag = new TagBuilder("span");
                spanTag.AddCssClass("title");
                spanTag.InnerHtml = menu.Menu;
                strMenuPart += spanTag.ToString();
                if (menus.Where(i => i.MenuParentID == menu.mstMenuID).Count() > 0)
                {
                    TagBuilder spanTagArrow = new TagBuilder("span");
                    spanTagArrow.AddCssClass("arrow");
                    strMenuPart += spanTagArrow.ToString();

                }
                //start of modify
                var cek = 0;
                foreach (var item in menus.Where(i => i.MenuParentID == menu.mstMenuID))
                {
                    if (item.MenuStatus == "New")
                    {
                        cek += 1;
                    }
                }
                if (cek > 0)
                {
                    TagBuilder spanTagBubble = new TagBuilder("span");
                    spanTagBubble.AddCssClass("badge badge-danger");
                    spanTagBubble.InnerHtml = "New";
                    strMenuPart += spanTagBubble.ToString();
                }
                //end of modify

                if (menu.MenuStatus == "New")
                {
                    TagBuilder spanTagBubble = new TagBuilder("span");
                    spanTagBubble.AddCssClass("badge badge-danger");
                    spanTagBubble.InnerHtml = "New";
                    strMenuPart += spanTagBubble.ToString();
                }
                if (menu.MenuStatus == "Update")
                {
                    TagBuilder spanTagBubble = new TagBuilder("span");
                    spanTagBubble.AddCssClass("badge badge-danger");
                    spanTagBubble.InnerHtml = "Update";
                    strMenuPart += spanTagBubble.ToString();
                }

                aTag.InnerHtml = strMenuPart;
                liTag.InnerHtml = aTag.ToString();

                if (menus.Where(i => i.MenuParentID == menu.mstMenuID).Count() > 0)
                    liTag.InnerHtml += GenerateMenu(menus, menuActives, Convert.ToInt64(menu.mstMenuID), false).ToString();

                strMenuHolder += liTag.ToString();
            }

            if (!bolIsFirst)
            {
                ulTag.InnerHtml = strMenuHolder;
                return ulTag.ToString();
            }
            else
                return strMenuHolder;
        }

        #endregion
    }
}