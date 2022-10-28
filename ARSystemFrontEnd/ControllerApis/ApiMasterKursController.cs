using ARSystemFrontEnd.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/Kurs")]
    public class ApiMasterKursController : ApiController
    {
        [HttpPost, Route("")]
        public IHttpActionResult GetMstKursToList(PostKursView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.mstKurs> list = new List<ARSystemService.mstKurs>();
                using (var client = new ARSystemService.ImstKursServiceClient())
                {
                    intTotalRecord = client.GetKursCount(UserManager.User.UserToken, post.kursDate);

                    string strOrderBy = "KursDate DESC";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    list = client.GetMstKursToList(UserManager.User.UserToken, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("Save")]
        public IHttpActionResult SaveMstKurs(PostKursView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.mstKurs> list = new List<ARSystemService.mstKurs>();
                ARSystemService.mstKurs kursReturn = new ARSystemService.mstKurs();
                using (var client = new ARSystemService.ImstKursServiceClient())
                {
                    intTotalRecord = client.GetKursCount(UserManager.User.UserToken, post.kursDate);


                    ARSystemService.mstKurs kurs = new ARSystemService.mstKurs();
                    kurs.KursDate = post.kursDate ?? new DateTime(0);
                    kurs.KursTengahBI = post.kursTengahBI;
                    kurs.KursPajak = post.kursPajak;

                    if (intTotalRecord == 0)
                    {
                        //Save New Kurs
                        kursReturn= client.CreateKurs(UserManager.User.UserToken, kurs);
                    }
                    else
                    {
                        //Update Existing Kurs
                        kursReturn = client.UpdateKurs(UserManager.User.UserToken, kurs);
                    }
                }

                list.Add(kursReturn);
                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}