using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystem.Service
{
    public class Constants
    {
        public static Dictionary<string, string> BapsTypes = new Dictionary<string, string>() {
            { "ELECT" , "Electricty" },
            { "INF" , "Inflation" },
            { "POWER" , "Power" },
            { "PWRUP" , "Power Up" },
            { "TOWER" , "Tower" },
        };

        public static Dictionary<string, string> VerificationStatus = new Dictionary<string, string>() {
            { "WAITING" , "P" },
            { "APPROVED" , "A" },
            { "REJECTED" , "R" },
            { "NEW" , "N" },
            { "WAITINGAPP" , "W" },
        };

        public struct Parameter
        {
            public static string PPH23 = "PPH23";
            public static string PPF = "PPF";
            public static string PPN = "PPN";
        }
        public static Dictionary<string, string> PaidStatus = new Dictionary<string, string>() {
            { "NEW" , "0" },
            { "FULL" , "1" },
            { "PARTIAL" , "2" },
            { "OVERPAID" , "3" }
        };

        public enum PaymentCode : int
        {
            DBT = 1,
            PAM = 2,
            PNT = 3,
            PPE = 4,
            PPH = 5,
            RTE = 6,
            RTG = 7,
            PAT = 8,
            PPF = 9
        }

        public struct Jobs
        {
            public static string JobBapsReceive = "Receive BAPS Big Data";
            public static string JobBapsConfirm = "Confirm BAPS Big Data";
            public static string JobCreateInvoiceTower = "Create Big Invoice Tower";
        }

        public static string TBGSysServiceToken = "bbac92c7-ff22-45f6-8298-dfcfb1df68ad";
        

        public enum UserGroup : int
        {
            ARData = 1,
            ARCollection = 2,
            Electricity = 3
        }

        public enum EmailID : int
        {
            RejectBAPS = 1,
            StartReceiveBulky = 2,
            EndReceiveBulky = 3,
            StartConfirmBulky = 4,
            EndConfirmBulky = 5,
            StartCreateBulky = 6,
            EndCreateBulky = 7,
            StartReturnBulky = 8,
            EndReturnBulky = 9,
            StartRejectBulky = 10,
            EndRejectBulky = 11,
        }

        public static List<string> AREmail = new List<string>(new string[] {
            //public static string ARDataGroup = "ar.invoice@tower-bersama.com;";
            //public static string RAGroup = "ra";
            "information@internal.m-tbig.com;",
            "information@internal.m-tbig.com;"
        });

        public enum PICATypeIDARData : int
        {
            Internal = 4,
            ExtMarketing = 3,
            ExtRANew = 5,
            ExtRARecurringTSEL = 6,
            ExtRARecurringNonTSEL = 7,
            CNInternal = 8,
            CNExternal = 9,
            ReturnProcessInternal = 10,
            ReturnProcessExternal =11
        }

        public static List<string> _Operator = new List<string>
        {  "'TSEL'"
            , "'ISAT'"
            , "'HCPT'"
            , "'MITEL'"
            , "'NTS'"
            , "'XL'"
            , "'PAB'"
            , "'TELKOM'"
        };

        public enum RAActivity : int
        {
            HOLD = 0,
            RECONCILE_NOT_YET = 1,
            RECONCILE_PROCESSED = 2,
            RECONCILE_DONE = 3,
            BOQ_PROCESSED = 4,
            BOQ_DONE = 5,
            BOQ_REJECT = 6,
            PO_INPUT = 7,
            PO_DONE = 8,
            BAPS_INPUT = 9,
            BAPS_DONE = 10,
            BAPS_RETURN = 11,
            RTI = 12
        }

        public enum RADocument : int
        {
            BOQ = 1,
            PO = 2,
            BAPSRECURRING = 3,
            RTI = 4,
            RECON_REGIONAL = 5
        }

        public static Dictionary<string, string> CustomerID = new Dictionary<string, string>() {
            { "TELKOMSEL","TSEL"  },
            { "INDOSAT", "ISAT"  },
            { "EXCELCOMINDO", "XL" },
            { "HCPT" , "HCPT" },
            { "MITEL" , "MITEL" },
            { "PAB" , "PAB" },
            { "TELKOM" , "TELKOM" },
        };

        public static class CompanyCode
        {
            public const string PKP = "PKP";
        }

        /// <summary>
        /// Enum for department type in Input Target Page
        /// </summary>
        public static class RADepartmentTypeEnum
        {
            public const string TSEL = "TSEL";
            public const string NonTSEL = "NonTSEL";
            public const string NewProduct = "New Product";
            public const string BAPS = "BAPS";
        }
        /// <summary>
        /// Enum for department code that used in page input target
        /// </summary>
        public static class RADepartmentCodeTabEnum
        {
            public const string NewBaps = "DE290";
            public const string TSEL = "DE000434";
            public const string NonTSEL = "DE000435";
            public const string NewProduct = "DE000421";
        }
    }
}