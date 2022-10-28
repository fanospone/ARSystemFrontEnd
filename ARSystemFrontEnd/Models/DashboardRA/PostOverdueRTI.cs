using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARSystemFrontEnd.Models;
using ARSystemFrontEnd.Providers;
using ARSystem.Service;
using ARSystem.Domain.Models;

namespace ARSystemFrontEnd.Models
{
    public class PostOverdueRTI : DatatableAjaxModel
    {


        public string DataType { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public string CustomerID { get; set; }
    }


    public class PostRTILeadTime : DatatableAjaxModel
    {


        public string LeadTime { get; set; }
        public int Year { get; set; }
        public string CustomerID { get; set; }
    }

    public class PostRTILeadTimeByStatus : DatatableAjaxModel
    {


        public string CustomerID { get; set; }
        public int Year { get; set; }
        public string currentStatus { get; set; }
    }

    public class PostIntID : DatatableAjaxModel
    {
        
        public int ID { get; set; }
    }

    public class PostRTIAchievement : DatatableAjaxModel
    {


        public string CustomerID { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }

    }

    public class PostTarget: DatatableAjaxModel
    {
        public TrxRAAmountTarget post { get; set; }
        public List<TrxRAAmountTargetDetail> postDetail { get; set; }
    }

    public class PostTargetReady : DatatableAjaxModel
    {
        public TrxRAAmountTarget post { get; set; }
    }
}