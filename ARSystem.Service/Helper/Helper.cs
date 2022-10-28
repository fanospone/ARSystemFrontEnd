using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Diagnostics;
using System.IO;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;

using System.Xml;
using System.Xml.Serialization;

namespace ARSystem.Service
{
    public static class Helper
    {
        /// <summary>
        /// Serialize Object to XML
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="dataToSerialize">Object</param>
        /// <returns></returns>
        public static string XmlSerializer<T>(T dataToSerialize)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            StringWriter sw = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(sw, settings))
            {
                var xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);

                xsSubmit.Serialize(writer, dataToSerialize, xmlns);
                return sw.ToString();
            }
        }

        /// <summary>
        /// Deserialize XML to Object
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="xmlText">xml</param>
        /// <returns></returns>
        public static T XMLDeserializer<T>(string xmlText)
        {
            var stringReader = new System.IO.StringReader(xmlText);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }
        /// <summary>
        /// Enum Error Type
        /// </summary>
        public enum ErrorType : int
        {
            None = 0,
            Validation = 1,
            Error = 2,
            Info = 3,
        };

        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <returns></returns>
        public static IDbConnectionFactory GetConnection(string strCon)
        {
            return new DbConnectionFactory(strCon);
        }

        /// <summary>
        /// Get DateTime Now
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTimeNow()
        {
            string strDtNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff");
            return DateTime.Parse(strDtNow);
        }

        /// <summary>
        /// Get Idle Time Application in Minutes
        /// </summary>
        /// <returns></returns>
        public static int GetIdletime()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["IdleTime"]);
        }

        /// <summary>
        /// Get Password Expired in Days
        /// </summary>
        /// <returns></returns>
        public static int GetPasswordExpired()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["PasswordExpired"]);
        }

        /// <summary>
        /// Hash string using MD5CryptoServiceProvider
        /// </summary>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public static string HashingMD5(string strInput)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            Byte[] bytes = md5.ComputeHash(new System.Text.UTF8Encoding().GetBytes(strInput));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Byte item in bytes)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="strError"></param>
        /// <param name="strService"></param>
        /// <param name="strMethod"></param>
        /// <returns></returns>
        public static string logError(string strError, string strService, string strMethod, string strUserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var errorRepo = new logErrorRepository(context);

            logError error = new logError();
            error.ErrorMessages = strError;
            error.Service = strService;
            error.Method = strMethod;
            error.CreatedDate = Helper.GetDateTimeNow();
            error.CreatedBy = strUserID;
            errorRepo.Create(error);

            return "Error on System. Please Call IT Help Desk!";
        }

        /// <summary>
        /// Generate Invoice Number, based on given Company TBG, Operator / Customer, and UserID
        /// </summary>
        /// <param name="company">Company TBG</param>
        /// <param name="operatorCode">Operator / Customer (for Invoice Building)</param>
        /// <param name="strUserID">User who creates the invoice number</param>
        /// <returns>Invoice Number</returns>

        public static string GetCurrentMethod()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);

            return stackFrame.GetMethod().Name;
        }

        /// <summary>
        /// Generate Invoice Number, based on given Company TBG, Operator / Customer, and UserID
        /// </summary>
        /// <param name="company">Company TBG</param>
        /// <param name="operatorCode">Operator / Customer (for Invoice Building)</param>
        /// <param name="strUserID">User who creates the invoice number</param>
        /// <returns>Invoice Number</returns>
        public static string GenerateInvoiceNumber(string company, string Operator, string strUserID, int mstInvoiceCategoryId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var logRepo = new logCounterInvoiceHeaderRepository(context);
            logCounterInvoiceHeader counter = new logCounterInvoiceHeader();
            string strWhereClause = string.Empty;
            string[] months = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };

            counter = new logCounterInvoiceHeader();
            if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower || mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.NonRevenue || mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower15 || mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Tower10)
            {
                strWhereClause = "CompanyID = '" + company + "' AND InvoiceYear = " + DateTime.Now.Year;
            }
            else if (mstInvoiceCategoryId == (int)StatusHelper.InvoiceCategory.Building)
            {
                strWhereClause = "CompanyID = '" + company + "' AND InvoiceYear = " + DateTime.Now.Year;
            }
            counter = logRepo.GetList(strWhereClause).FirstOrDefault();
            if (counter != null)
            {
                counter.Counter = counter.Counter + 1;
                counter.UpdatedBy = strUserID;
                counter.UpdatedDate = Helper.GetDateTimeNow();
                counter = logRepo.Update(counter);
            }
            else
            {
                counter = new logCounterInvoiceHeader();
                counter.CompanyID = company;
                counter.Counter = 1;
                counter.InvoiceYear = DateTime.Now.Year;
                counter.CreatedBy = strUserID;
                counter.CreatedDate = Helper.GetDateTimeNow();
                counter = logRepo.Create(counter);
            }
            return counter.Counter.ToString().PadLeft(4, '0') + "/" + company.Trim() + "/" + Operator.Trim() + "/" + months[DateTime.Now.Month - 1] + "/" + counter.InvoiceYear;
        }

        /// <summary>
        /// Generate CN Note Number, based on incremental number, companyID, and operatorID which passed as parameters
        /// </summary>
        /// <param name="trxCancelNoteFinanceID">ID of the inserted row</param>
        /// <param name="invCompanyID">Company ID in the Invoice</param>
        /// <param name="invOperatorID">Operator ID in the Invoice</param>
        /// <returns>CN Note Number</returns>
        public static string GenerateCNNoteNumber(int trxCancelNoteFinanceID, string invCompanyID, string invOperatorID)
        {
            string[] months = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
            return trxCancelNoteFinanceID + "/" + invCompanyID.Trim() + "/" + invOperatorID.Trim() + "/" + months[DateTime.Now.Month - 1] + "/" + DateTime.Now.Year;
        }

        /// <summary>
        /// Generate CN Note Number, based on incremental number, companyID, and operatorID which passed as parameters
        /// </summary>
        /// <param name="trxCancelNoteFinanceID">ID of the inserted row</param>
        /// <param name="invCompanyID">Company ID in the Invoice</param>
        /// <returns>CN Memo Number</returns>
        public static string GenerateCNMemoNumber(int trxCancelNoteFinanceID, string invCompanyID)
        {
            string[] months = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
            return trxCancelNoteFinanceID + "/Fin-AR/CN/" + invCompanyID.Trim() + "/" + months[DateTime.Now.Month - 1] + "/" + DateTime.Now.Year;
        }

        public static List<string> GenerateVoucherNumber(string company, string operatorCode, string strUserID)
        {
            string[] months = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };

            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var logVoucherNumberRepo = new logCounterVoucherNumberRepository(context);
            var vwRepo = new vwVoucherNumberCounterRepository(context);
            var mstRepo = new mstPaymentCodeRepository(context);
            List<string> listVoucher = new List<string>();
            List<mstPaymentCode> listPaymentCode = new List<mstPaymentCode>();
            logCounterVoucherNumber CounterVoucher = new logCounterVoucherNumber();
            string strWhereClause = "";
            try
            {
                listPaymentCode = mstRepo.GetList();

                foreach (mstPaymentCode mst in listPaymentCode)
                {
                    CounterVoucher = null;
                    strWhereClause = "OperatorId = '" + operatorCode + "' AND CompanyId='" + company + "' AND VoucherNumberYear='" + Helper.GetDateTimeNow().Year + "' AND mstPaymentCodeId=" + mst.mstPaymentCodeId;
                    if (logVoucherNumberRepo.GetList(strWhereClause).Count > 0)
                        CounterVoucher = logVoucherNumberRepo.GetList(strWhereClause)[0];
                    if (CounterVoucher != null)
                    {
                        CounterVoucher.Counter = CounterVoucher.Counter + 1;
                        CounterVoucher.UpdatedBy = strUserID;
                        CounterVoucher.UpdatedDate = Helper.GetDateTimeNow();
                        CounterVoucher = logVoucherNumberRepo.Update(CounterVoucher);
                    }
                    else
                    {
                        CounterVoucher = new logCounterVoucherNumber();
                        CounterVoucher.mstPaymentCodeId = mst.mstPaymentCodeId;
                        CounterVoucher.Counter = 1;
                        CounterVoucher.CompanyId = company;
                        CounterVoucher.OperatorId = operatorCode;
                        CounterVoucher.VoucherNumberYear = Helper.GetDateTimeNow().Year.ToString();
                        CounterVoucher.CreatedBy = strUserID;
                        CounterVoucher.CreatedDate = Helper.GetDateTimeNow();
                        CounterVoucher = logVoucherNumberRepo.Create(CounterVoucher);
                    }
                    listVoucher.Add(CounterVoucher.Counter.ToString().PadLeft(4, '0') + "/" + mst.Code.Trim() + "/" + company.Trim() + "/" + operatorCode.Trim() + "/" + months[DateTime.Now.Month - 1] + "/" + CounterVoucher.VoucherNumberYear);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listVoucher;
        }

        public static string ToSqlDate(this DateTime date)
        {
            return DateTime.Parse(date.ToString()).ToString("yyyy-MM-dd");
        }
    }

}