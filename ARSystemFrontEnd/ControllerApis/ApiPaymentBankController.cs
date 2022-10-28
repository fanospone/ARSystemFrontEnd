using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;

namespace ARSystemFrontEnd.Controllers
{
    [RoutePrefix("api/MstPaymentBank")]
    public class ApiPaymentBankController : ApiController
    {
        [HttpPost, Route("")]
        public IHttpActionResult CreatePaymentBank(ARSystemService.mstPaymentBank paymentBank)
        {
            try
            {
                using (var client = new ARSystemService.ImstPaymentBankServiceClient())
                {
                    paymentBank = client.CreatePaymentBank(UserManager.User.UserToken, paymentBank);
                }

                return Ok(paymentBank);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdatePaymentBank(int id, ARSystemService.mstPaymentBank paymentBank)
        {
            try
            {
                using (var client = new ARSystemService.ImstPaymentBankServiceClient())
                {
                    paymentBank = client.UpdatePaymentBank(UserManager.User.UserToken, id, paymentBank);
                }

                return Ok(paymentBank);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("grid")]
        public IHttpActionResult GetPaymentBankToGrid(PostPaymentBankView post)
        {
            try
            {
                int intTotalRecord = 0;

                List<ARSystemService.mstPaymentBank> PaymentBanks = new List<ARSystemService.mstPaymentBank>();
                using (var client = new ARSystemService.ImstPaymentBankServiceClient())
                {
                    intTotalRecord = client.GetPaymentBankCount(UserManager.User.UserToken, post.bankGroupId, post.companyId, post.accountNo);

                    string strOrderBy = "";
                    if (post.order != null)
                        if (post.columns[post.order[0].column].data != "0")
                            strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();

                    PaymentBanks = client.GetPaymentBankToList(UserManager.User.UserToken, post.bankGroupId, post.companyId, post.accountNo, strOrderBy, post.start, post.length).ToList();
                }

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = PaymentBanks });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}