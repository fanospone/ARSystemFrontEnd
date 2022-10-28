using ARSystem.Service.ARSystem;
using ARSystemFrontEnd.Domain.Models;
using ARSystemFrontEnd.Helper;
using ARSystemFrontEnd.Models.TrxInputForecast;
using ARSystemFrontEnd.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ARSystemFrontEnd.ControllerApis
{
    [RoutePrefix("api/ApiInputForecastCashIn")]
    public class ApiInputForecastCashInController : ApiController
    {
        private readonly InputForecastCashInService _inputForecastCashInService;
        public ApiInputForecastCashInController()
        {
            _inputForecastCashInService = new InputForecastCashInService();
        }
        public string forecastTypeFcVsAct = "ForecastVsActual";
        public string forecastTypeFcVsFc = "ForecastVsForecast";
        public string submitForApproval = "Submit For Approval";
        public string waitingForApproval = "Waiting For Action Approval";
        public string actionBySectionHead = "Section Head";
        public string actionByDeptHead = "Department Head";
        public string approved = "Approved";
        public string rejected = "Rejected";
        #region Forecast Vs Actual

        [HttpPost, Route("loadForecastVsActual")]
        public IHttpActionResult LoadForecastVsActual(PostTrxInputForecast post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<uspARCashInForecastVsActual> list = new List<uspARCashInForecastVsActual>();

                var forecast = new uspARCashInForecastVsActual();
                forecast.Year = post.Year.GetValueOrDefault();
                forecast.Quarter = post.Quarter.GetValueOrDefault();
                forecast.Month = post.Month.GetValueOrDefault();
                forecast.OperatorID = post.OperatorID;
                intTotalRecord = _inputForecastCashInService.GetForecastVsActualCount(userCredential.UserID, forecast);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();


                list = _inputForecastCashInService.GetForecastVsActualList(userCredential.UserID, forecast, strOrderBy, post.start, post.length).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("loadSummaryActualForecastVsActual")]
        public IHttpActionResult LoadSummaryActualForecastVsActual(PostTrxInputForecast post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<trxARCashInActualDetails> list = new List<trxARCashInActualDetails>();

                var forecast = new trxARCashInActualDetails();
                forecast.Year = post.Year.GetValueOrDefault();
                forecast.Quarter = post.Quarter.GetValueOrDefault();
                forecast.MonthInQuarter = post.MonthWithinQuarter.GetValueOrDefault();
                forecast.InvoiceOperatorID = post.OperatorID;
                intTotalRecord = _inputForecastCashInService.GetActualSummaryDetailCount(userCredential.UserID, forecast);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();


                list = _inputForecastCashInService.GetActualSummaryDetailList(userCredential.UserID, forecast, strOrderBy, post.start, post.length).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("submitForecastVsActual")] 
        public IHttpActionResult SubmitForecastVsActual(PostTrxInputForecast post)
        {
            int currentQuarter = Convert.ToInt32(Math.Floor((decimal)((DateTime.Today.Month - 1) + 2) / 3));
            int currentQuarterForecast = Convert.ToInt32(Math.Floor((decimal)((post.Month) + 2) / 3));
            int monthInQuarterInputPica = 0;
            int monthInQuarterInputForecast = 0;
            int startMonthPica = currentQuarter * 3 - 2;
            int startMonthForecast = currentQuarter * 3 - 2;
            for (int i = 0; i < 3; i++)
            {
                if (DateTime.Now.Month - 1 == (startMonthPica + i))
                {
                    monthInQuarterInputPica = i + 1;
                }
                if (post.Month == (startMonthForecast + i))
                {
                    monthInQuarterInputForecast = i + 1;
                }
            }

            try
            {
                List<uspARCashInForecastVsActual> forecastData = new List<uspARCashInForecastVsActual>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                    forecastData.Add(new uspARCashInForecastVsActual(userCredential.ErrorType, userCredential.ErrorMessage));
                else
                {
                    forecastData.Add(new uspARCashInForecastVsActual()
                    {
                        OperatorID = post.OperatorID,
                        Month = post.Month,
                        Year = post.Year,
                        Quarter = post.Quarter,
                        ActualM1 = post.ActualM1,
                        ActualM2 = post.ActualM2,
                        ActualM3 = post.ActualM3,
                        FCMarketingM1 = post.FCMarketingM1,
                        FCMarketingM2 = post.FCMarketingM2,
                        FCMarketingM3 = post.FCMarketingM3,
                        FCFinanceM1 = post.FCFinanceM1,
                        FCFinanceM2 = post.FCFinanceM2,
                        FCFinanceM3 = post.FCFinanceM3,
                        FCRevenueAssuranceM1 = post.FCRevenueAssuranceM1,
                        FCRevenueAssuranceM2 = post.FCRevenueAssuranceM2,
                        FCRevenueAssuranceM3 = post.FCRevenueAssuranceM3,
                        PiCaM1 = post.PiCaM1,
                        PiCaM2 = post.PiCaM2,
                        PiCaM3 = post.PiCaM3,
                        VarianceM1 = post.VarianceM1,
                        VarianceM2 = post.VarianceM2,
                        VarianceM3 = post.VarianceM3,
                        MonthWithinQuarter = post.MonthWithinQuarter,
                        CreatedBy = userCredential.UserID
                    });

                    forecastData = _inputForecastCashInService.UpdateInputForecastVsActual(userCredential.UserID, forecastData);
                }


                return Ok(forecastData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("getForecastVsActualByOperator")]
        public IHttpActionResult GetForecastVsActualByOperator(PostTrxInputForecast post)
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var model = new uspARCashInForecastVsActual()
                {
                    OperatorID = post.OperatorID,
                    Year = post.Year,
                    Quarter = post.Quarter
                };
                var res = _inputForecastCashInService.GetByOperatorInPeriod(userCredential.UserID, model);
                int currentQuarter = Convert.ToInt32(Math.Floor((decimal)(DateTime.Today.Month + 2) / 3));
                int numberOfMonthWithinQuarter = 0;
                int startMonth = currentQuarter * 3 - 2;
                for (int i = 0; i < 3; i++)
                {
                    if (DateTime.Now.Month == (startMonth + i))
                    {
                        numberOfMonthWithinQuarter = i + 1;
                    }
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("sendApprovalRequestForecastVsActual")]
        public IHttpActionResult SendApprovalRequestForecastVsActual(PostTrxInputForecast post)
        {
            try
            {
                List<uspARCashInForecastVsForecast> forecastData = new List<uspARCashInForecastVsForecast>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                    forecastData.Add(new uspARCashInForecastVsForecast(userCredential.ErrorType, userCredential.ErrorMessage));
                else
                {
                    int currentQuarter = Convert.ToInt32(Math.Floor((decimal)(DateTime.Today.Month + 2) / 3));

                    var data = new trxARCashInForecastApprovalLog()
                    {
                        ApprovalType = submitForApproval,
                        ForecastQuarter = currentQuarter,
                        ForecastMonth =  DateTime.Today.Month,
                        ForecastYear = DateTime.Today.Year,
                        ForecastType = forecastTypeFcVsAct,
                    };
                    var res = _inputForecastCashInService.CreateApprovalLog(userCredential.UserID, data);
                }
                return Ok(forecastData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("submitApprovalForecastVsActual")]
        public IHttpActionResult SubmitApprovalForecastVsActual(PostTrxInputForecast post)
        {
            try
            {
                List<uspARCashInForecastVsForecast> forecastData = new List<uspARCashInForecastVsForecast>();
                var approvalPeriod = DateTime.Today.AddMonths(-1);

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                    forecastData.Add(new uspARCashInForecastVsForecast(userCredential.ErrorType, userCredential.ErrorMessage));
                else
                {
                    int currentQuarter = Convert.ToInt32(Math.Floor((decimal)(approvalPeriod.Month + 2) / 3));
                    var data = new trxARCashInForecastApprovalLog()
                    {
                        ApprovalType = post.ApprovalType,
                        ForecastQuarter = currentQuarter,
                        ForecastMonth = approvalPeriod.Month,
                        ForecastYear = approvalPeriod.Year,
                        ForecastType = forecastTypeFcVsAct,
                        Action = post.ApprovalAction,
                        Remarks = post.ApprovalRemarks,
                        ProcessOID = post.ProcessOID,
                        CreatedBy = String.Format("{0}-{1}", userCredential.UserID, UserManager.User.UserFullName)
                    };
                    var res = _inputForecastCashInService.CreateApprovalLog(userCredential.UserID, data);
                }
                return Ok(forecastData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("checkPiCaValidation")]
        public IHttpActionResult CheckPiCaValidation(PostTrxInputForecast post)
        {
            try
            {
                List<uspARCashInForecastVsActual> forecastData = new List<uspARCashInForecastVsActual>();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var forecast = new uspARCashInForecastVsActual();
                forecast.Year = post.Year.GetValueOrDefault();
                forecast.Quarter = post.Quarter.GetValueOrDefault();
                forecast.Month = post.Month.GetValueOrDefault();

                if (userCredential.ErrorType > 0)
                    forecastData.Add(new uspARCashInForecastVsActual(userCredential.ErrorType, userCredential.ErrorMessage));

                forecastData = _inputForecastCashInService.CheckPiCaValidation(userCredential.UserID, forecast, String.Empty, post.start, post.length).ToList();

                return Ok(forecastData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        #endregion

        #region Forecast Vs Forecast

        [HttpPost, Route("loadForecastVsForecast")]
        public IHttpActionResult LoadForecastVsForecast(PostTrxInputForecast post)
        {
            try
            {
                int intTotalRecord = 0;
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                List<uspARCashInForecastVsForecast> list = new List<uspARCashInForecastVsForecast>();

                var forecast = new uspARCashInForecastVsForecast();
                forecast.Year = post.Year.GetValueOrDefault();
                forecast.Quarter = post.Quarter.GetValueOrDefault();
                //forecast.Month = post.Month.GetValueOrDefault();
                forecast.OperatorID = post.OperatorID;
                intTotalRecord = _inputForecastCashInService.GetForecastVsForecastCount(userCredential.UserID, forecast);

                string strOrderBy = "";
                if (post.order != null)
                    if (post.columns[post.order[0].column].data != "0")
                        strOrderBy = post.columns[post.order[0].column].data + " " + post.order[0].dir.ToLower();


                list = _inputForecastCashInService.GetForecastVsForecastList(userCredential.UserID, forecast, strOrderBy, post.start, post.length).ToList();

                return Ok(new { draw = post.draw, recordsTotal = intTotalRecord, recordsFiltered = intTotalRecord, data = list });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("submitForecastVsForecast")]
        public IHttpActionResult SubmitForecastVsForecast(PostTrxInputForecast post)
        {
            try
            {
                List<uspARCashInForecastVsForecast> forecastData = new List<uspARCashInForecastVsForecast>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                    forecastData.Add(new uspARCashInForecastVsForecast(userCredential.ErrorType, userCredential.ErrorMessage));
                else
                {
                    //for (int i = 1; i<= 3; i++)
                    //{
                        forecastData.Add(new uspARCashInForecastVsForecast()
                        {
                            OperatorID = post.OperatorID,
                            Year = post.Year,
                            Quarter = post.Quarter,
                            //Month = (post.Quarter * 3) - (3 - i),
                            //MonthWithinQuarter = i,
                            //TotalActualM1Idr = post.TotalActualM1Idr,
                            //TotalActualM2Idr = post.TotalActualM2Idr,
                            //TotalActualM3Idr = post.TotalActualM3Idr,
                            //TotalForecastM1Idr = post.TotalForecastM1Idr,
                            //TotalForecastM2Idr = post.TotalForecastM2Idr,
                            //TotalForecastM3Idr = post.TotalForecastM3Idr,
                            //TotalActualM1Usd = post.TotalActualM1Usd,
                            //TotalActualM2Usd = post.TotalActualM2Usd,
                            //TotalActualM3Usd = post.TotalActualM3Usd,
                            //TotalForecastM1Usd = post.TotalForecastM1Usd,
                            //TotalForecastM2Usd = post.TotalForecastM2Usd,
                            //TotalForecastM3Usd = post.TotalForecastM3Usd,
                            
                            Remarks = post.Remarks,
                            VarianceIdr = post.VarianceIdr,
                            VarianceUsd = post.VarianceUsd,
                            TotalCurrentPeriodIdr = post.TotalCurrentPeriodIdr,
                            TotalCurrentPeriodUsd = post.TotalCurrentPeriodUsd,

                            TotalLastPeriodIdr = post.TotalCurrentPeriodIdr,
                            TotalLastPeriodUsd = post.TotalCurrentPeriodUsd,

                            CreatedBy = userCredential.UserID
                        });
                    //}
                    forecastData = _inputForecastCashInService.UpdateInputForecastVsForecast(userCredential.UserID, forecastData);
                }
                return Ok(forecastData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost, Route("getForecastVsForecastByOperator")]
        public IHttpActionResult GetForecastVsForecastByOperator(PostTrxInputForecast post) 
        {
            try
            {
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var model = new uspARCashInForecastVsActual()
                {
                    OperatorID = post.OperatorID,
                    Year = post.Year,
                    Quarter = post.Quarter
                };
                var res = _inputForecastCashInService.GetByOperatorInPeriod(userCredential.UserID, model);
                int currentQuarter = Convert.ToInt32(Math.Floor((decimal)(DateTime.Today.Month + 2) / 3));
                int numberOfMonthWithinQuarter = 0;
                int startMonth = currentQuarter * 3 - 2;
                for (int i = 0; i < 3; i++)
                {
                    if (DateTime.Now.Month == (startMonth + i))
                    {
                        numberOfMonthWithinQuarter = i + 1;
                    }
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("sendApprovalRequestForecastVsForecast")]
        public IHttpActionResult SendApprovalRequestForecastVsForecast(PostTrxInputForecast post)
        {
            try
            {
                List<uspARCashInForecastVsForecast> forecastData = new List<uspARCashInForecastVsForecast>();

                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                int currentQuarter = Convert.ToInt32(Math.Floor((decimal)(DateTime.Today.Month + 2) / 3));
                if (userCredential.ErrorType > 0)
                    forecastData.Add(new uspARCashInForecastVsForecast(userCredential.ErrorType, userCredential.ErrorMessage));
                else
                {
                    var data = new trxARCashInForecastApprovalLog()
                    {
                        ApprovalType = submitForApproval,
                        ForecastQuarter = currentQuarter,
                        ForecastMonth = DateTime.Today.Month,
                        ForecastYear = DateTime.Today.Year,
                        ForecastType = forecastTypeFcVsFc,
                    };
                    var res = _inputForecastCashInService.CreateApprovalLog(userCredential.UserID, data);
                }
                return Ok(forecastData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("submitApprovalForecastVsForecast")]
        public IHttpActionResult SubmitApprovalForecastVsForecast(PostTrxInputForecast post)
        {
            try
            {
                List<uspARCashInForecastVsForecast> forecastData = new List<uspARCashInForecastVsForecast>();
                var approvalPeriod = DateTime.Today.AddMonths(-1);
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);

                if (userCredential.ErrorType > 0)
                    forecastData.Add(new uspARCashInForecastVsForecast(userCredential.ErrorType, userCredential.ErrorMessage));
                else
                {
                int currentQuarter = Convert.ToInt32(Math.Floor((decimal)(approvalPeriod.Month + 2) / 3));
                    var data = new trxARCashInForecastApprovalLog()
                    {
                        ApprovalType = post.ApprovalType,
                        ForecastQuarter = currentQuarter,
                        ForecastMonth = approvalPeriod.Month,
                        ForecastYear = approvalPeriod.Year,
                        ForecastType = forecastTypeFcVsFc,
                        Action = post.ApprovalAction,
                        Remarks = post.ApprovalRemarks,
                        ProcessOID = post.ProcessOID,

                        CreatedBy = String.Format("{0}-{1}", userCredential.UserID, UserManager.User.UserFullName)
                    };
                    var res = _inputForecastCashInService.CreateApprovalLog(userCredential.UserID, data);

                }
                return Ok(forecastData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        [HttpPost, Route("checkPiCaValidationForecastVsForecast")]
        public IHttpActionResult CheckPiCaValidationForecastVsForecast(PostTrxInputForecast post)
        {
            try
            {
                List<uspARCashInForecastVsForecast> forecastData = new List<uspARCashInForecastVsForecast>();
                ARSystem.Domain.Models.vmUserCredential userCredential = UserService.CheckUserToken(UserManager.User.UserToken);
                var forecast = new uspARCashInForecastVsForecast();
                forecast.Year = post.Year.GetValueOrDefault();
                forecast.Quarter = post.Quarter.GetValueOrDefault();


                if (userCredential.ErrorType > 0)
                    forecastData.Add(new uspARCashInForecastVsForecast(userCredential.ErrorType, userCredential.ErrorMessage));

                forecastData = _inputForecastCashInService.CheckPiCaValidationForecastVsForecast(userCredential.UserID, forecast, String.Empty, post.start, post.length).ToList();

                return Ok(forecastData);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        #endregion
    }
}
