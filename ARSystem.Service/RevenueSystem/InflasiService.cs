using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.ViewModels.RevenueSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using System.Web;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;

namespace ARSystem.Service.RevenueSystem
{
    public class InflasiService : BaseService
    {
        protected readonly DbContext _context;
        private mstRevSysInflationRateRepository _mstIR;
        private mstRevSysSonumbInflasiRepository _mstSonumbInflasi;
        private mstRevSysKursRepository _mstKurs;
        private vwRevSysSoInflasiSonumbListRepository _vwSonumb;
        private vwRevSysSonumbInflasiListRepository _vwSoInflasi;
        private OleDbConnection Econ;

        public InflasiService()
        {
            _context = this.SetContext();
            _mstIR = new mstRevSysInflationRateRepository(_context);
            _mstSonumbInflasi = new mstRevSysSonumbInflasiRepository(_context);
            _mstKurs = new mstRevSysKursRepository(_context);
            _vwSonumb = new vwRevSysSoInflasiSonumbListRepository(_context);
            _vwSoInflasi = new vwRevSysSonumbInflasiListRepository(_context);
            Econ = new OleDbConnection();
        }
        public List<mstRevSysInflationRate> GetInflationGridList(string strToken, GridInflasiParam post)
        {
            List<mstRevSysInflationRate> dataList = new List<mstRevSysInflationRate>();

            try
            {
                return _mstIR.GetList();
            }
            catch (Exception ex)
            {
                dataList.Add(new mstRevSysInflationRate((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InflasiService", "GetInflationGridList", "")));
                return dataList;
            }
            finally
            {
                this.Dispose();
            }
        }
        public List<vwRevSysSonumbInflasiList> GetSonumbInflasiGridList(string strToken, GridInflasiParam post)
        {
            List<vwRevSysSonumbInflasiList> dataList = new List<vwRevSysSonumbInflasiList>();

            try
            {
                return _vwSoInflasi.GetList();
            }
            catch (Exception ex)
            {
                dataList.Add(new vwRevSysSonumbInflasiList((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InflasiService", "GetSonumbGridList", "")));
                return dataList;
            }
            finally
            {
                this.Dispose();
            }
        }
        public List<vwRevSysSoInflasiSonumbList> GetSonumbGridList(string strToken, GridInflasiParam post)
        {
            List<vwRevSysSoInflasiSonumbList> dataList = new List<vwRevSysSoInflasiSonumbList>();

            try
            {
                return _vwSonumb.GetList();
            }
            catch (Exception ex)
            {
                dataList.Add(new vwRevSysSoInflasiSonumbList((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InflasiService", "GetSonumbGridList", "")));
                return dataList;
            }
            finally
            {
                this.Dispose();
            }
        }
        public mstRevSysInflationRate SubmitIR(string strToken, mstRevSysInflationRate post, vmUserCredential userCredential, HttpPostedFile attachment)
        {
            var uow = _context.CreateUnitOfWork();
            try
            {
                var IRSave = new mstRevSysInflationRate();
                if (post.ID == 0)
                {
                    if (attachment != null)
                    {
                        IRSave.FileName = attachment.FileName;
                        IRSave.ContentType = attachment.ContentType;
                        IRSave.FilePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString() + attachment.FileName);
                        IRSave.FileExtension = Path.GetExtension(attachment.FileName);

                        CreateNewFolder(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString()));
                        attachment.SaveAs(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString() + attachment.FileName));
                    }

                    IRSave.Year = post.Year;
                    IRSave.Percentage = post.Percentage;
                    IRSave.CreatedDate = Helper.GetDateTimeNow();
                    IRSave.CreatedBy = userCredential.UserID;

                    _mstIR.Create(IRSave);
                }
                else
                {
                    var IRUpd = _mstIR.GetByPK(post.ID);
                    if (attachment != null)
                    {
                        IRUpd.FileName = attachment.FileName;
                        IRUpd.ContentType = attachment.ContentType;
                        IRUpd.FilePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString() + attachment.FileName);
                        IRUpd.FileExtension = Path.GetExtension(attachment.FileName);

                        CreateNewFolder(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString()));
                        attachment.SaveAs(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString() + attachment.FileName));
                    }

                    IRUpd.Year = post.Year;
                    IRUpd.Percentage = post.Percentage;
                    IRUpd.UpdatedDate = Helper.GetDateTimeNow();
                    IRUpd.UpdatedBy = userCredential.UserID;

                    _mstIR.Update(IRUpd);
                }

                uow.SaveChanges();
                return post;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstRevSysInflationRate((int)Helper.ErrorType.Error, Helper.logError(ex.Message + ";" + ex.StackTrace.ToString(), "InflasiService", "SubmitIR", userCredential.UserID));

            }
        }
        //private void connection()
        //{
        //    string sqlconn = ConfigurationManager.ConnectionStrings["SqlCom"].ConnectionString;
        //    con = new SqlConnection(sqlconn);

        //}
        public mstRevSysSonumbInflasi SubmitSI(string strToken, mstRevSysSonumbInflasi post, HttpPostedFile attachment, List<mstRevSysSonumbInflasi> dataExcel, vmUserCredential userCredential)
        {
            var uow = _context.CreateUnitOfWork();
            try
            {
                if (post.ID != 0 && (post.UpdatedBy == null || post.UpdatedBy == ""))
                {
                    var EditSI = _mstSonumbInflasi.GetByPK(post.ID);
                    EditSI.AmountRental = post.AmountRental;
                    EditSI.AmountService = post.AmountService;
                    EditSI.AmountInflation = post.AmountInflation;
                    EditSI.InflationRate = post.InflationRate;
                    EditSI.UpdatedDate = Helper.GetDateTimeNow();
                    _mstSonumbInflasi.Update(EditSI);
                }
                else if (post.UpdatedBy == "Delete")
                {
                    _mstSonumbInflasi.DeleteByPK(post.ID);
                }
                else
                {
                    if (attachment != null)
                    {
                        _mstSonumbInflasi.CreateBulky(dataExcel);
                    }
                    for (int i = 0; i < post.SonumbList.Count; i++) // Loop through List with for
                    {
                        mstRevSysSonumbInflasi objList = new mstRevSysSonumbInflasi();
                        objList.Sonumb = post.SonumbList[i];
                        objList.AmountRental = post.AmountRental;
                        objList.AmountService = post.AmountService;
                        objList.AmountInflation = post.AmountInflation;
                        objList.InflationRate = post.InflationRate;
                        objList.CreatedDate = Helper.GetDateTimeNow();
                        objList.CreatedBy = userCredential.UserID;
                        _mstSonumbInflasi.Create(objList);
                    }
                }
                uow.SaveChanges();
                return post;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstRevSysSonumbInflasi((int)Helper.ErrorType.Error, Helper.logError(ex.Message + ";" + ex.StackTrace.ToString(), "InflasiService", "SubmitSI", ""));

            }
        }
        public List<mstRevSysKurs> GetKursGridList(string strToken, GridInflasiParam post)
        {
            List<mstRevSysKurs> dataList = new List<mstRevSysKurs>();

            try
            {
                return _mstKurs.GetList();
            }
            catch (Exception ex)
            {
                dataList.Add(new mstRevSysKurs((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InflasiService", "GetKursGridList", "")));
                return dataList;
            }
            finally
            {
                this.Dispose();
            }
        }
        public mstRevSysKurs SubmitKurs(string strToken, mstRevSysKurs post, vmUserCredential userCredential, HttpPostedFile attachment)
        {
            var uow = _context.CreateUnitOfWork();
            try
            {
                var KursSave = new mstRevSysKurs();
                if (post.Mode == "Save")
                {
                    if (attachment != null)
                    {
                        KursSave.FileName = attachment.FileName;
                        KursSave.ContentType = attachment.ContentType;
                        KursSave.FilePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString() + attachment.FileName);
                        KursSave.FileExtension = Path.GetExtension(attachment.FileName);

                        CreateNewFolder(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString()));
                        attachment.SaveAs(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString() + attachment.FileName));
                    }

                    KursSave.Currency = post.Currency;
                    KursSave.StartDate = post.StartDate;
                    KursSave.EndDate = post.EndDate;
                    KursSave.Kurs = post.Kurs;
                    KursSave.CreatedDate = Helper.GetDateTimeNow();
                    KursSave.CreatedBy = userCredential.UserID;

                    _mstKurs.Create(KursSave);
                }
                else if (post.Mode == "Delete")
                {
                    string whrDel = "StartDate ='" + post.StartDate + "' AND EndDate ='" + post.EndDate + "'";
                    _mstKurs.DeleteByFilter(whrDel);
                }
                else
                {
                    var KursUpd = _mstKurs.GetList("StartDate ='" + post.UpdatedStartDate + "' AND EndDate ='" + post.UpdatedEndDate + "'").FirstOrDefault();
                    if (attachment != null)
                    {
                        KursUpd.FileName = attachment.FileName;
                        KursUpd.ContentType = attachment.ContentType;
                        KursUpd.FilePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString() + attachment.FileName);
                        KursUpd.FileExtension = Path.GetExtension(attachment.FileName);

                        CreateNewFolder(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString()));
                        attachment.SaveAs(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DocPath"].ToString() + attachment.FileName));
                    }

                    KursUpd.StartDate = post.StartDate;
                    KursUpd.EndDate = post.EndDate;
                    KursUpd.UpdatedStartDate = post.UpdatedStartDate;
                    KursUpd.UpdatedEndDate = post.UpdatedEndDate;
                    KursUpd.Kurs = post.Kurs;
                    KursUpd.UpdatedDate = Helper.GetDateTimeNow();
                    KursUpd.UpdatedBy = userCredential.UserID;


                    _mstKurs.Update(KursUpd);
                }

                uow.SaveChanges();
                return post;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstRevSysKurs((int)Helper.ErrorType.Error, Helper.logError(ex.Message + ";" + ex.StackTrace.ToString(), "InflasiService", "SubmitKurs", userCredential.UserID));

            }
        }
        public List<mstRevSysSonumbInflasi> GetMstSonumbInflasiList()
        {
            List<mstRevSysSonumbInflasi> dataList = new List<mstRevSysSonumbInflasi>();

            try
            {
                return _mstSonumbInflasi.GetList();
            }
            catch (Exception ex)
            {
                dataList.Add(new mstRevSysSonumbInflasi((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "InflasiService", "GetMstSonumbInflasiList", "")));
                return dataList;
            }
            finally
            {
                this.Dispose();
            }
        }
        private static void CreateNewFolder(string strPath)
        {
            if (!Directory.Exists(strPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(strPath);
            }
        }
    }
}
