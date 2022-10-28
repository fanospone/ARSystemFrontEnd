using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;
using ARSystemFrontEnd.Helper;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/manualBook")]
    public class ApiManualBookController : ApiController
    {
        private readonly ManualBookService manualBookService;
        public ApiManualBookController()
        {
            manualBookService = new ManualBookService();
        }

        [HttpPost, Route("view")]
        public IHttpActionResult GetManualBookView(PostManualBook post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                int intTotalRecord = 0;

                List<vwProjectManualBook> manualBookList = new List<vwProjectManualBook>();

                var manualBook = new vwProjectManualBook();
                manualBook.ProjectName = post.strProjectName;
                manualBook.ProjectDescription = post.strProjectDescription;

                intTotalRecord = manualBookService.GetManualBookCount(userCredential.UserID, manualBook);

                string strOrderBy = "";
                if (post.order != null)
                    strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                manualBookList = manualBookService.GetManualBookList(userCredential.UserID, manualBook, strOrderBy, post.start, post.length).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = manualBookList });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}