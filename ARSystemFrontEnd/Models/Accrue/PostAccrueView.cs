using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ARSystemFrontEnd.Models
{
    public class PostAccrueView : DatatableAjaxModel
    {
        public PostAccrueView()
        {
            vwAccrueList = new ARSystem.Domain.Models.vwAccrueList();
            vwtrxDataAccrue = new ARSystem.Domain.Models.vwtrxDataAccrue();
            vwAccrueFinalConfirm = new ARSystem.Domain.Models.vwAccrueFinalConfirm();
            vwmstAccrueSettingAutoConfirm = new ARSystem.Domain.Models.vwmstAccrueSettingAutoConfirm();
            ListID = new List<string>();
            remarks = string.Empty;
            department = string.Empty;
            paramAllData = string.Empty;
            File = null;
            

        }
        public virtual ARSystem.Domain.Models.vwAccrueList vwAccrueList { get; set; }
        public virtual ARSystem.Domain.Models.vwtrxDataAccrue vwtrxDataAccrue { get; set; }
        public virtual ARSystem.Domain.Models.vwAccrueFinalConfirm vwAccrueFinalConfirm { get; set; }
        public virtual ARSystem.Domain.Models.vwmstAccrueSettingAutoConfirm vwmstAccrueSettingAutoConfirm { get; set; }        
        public virtual List<string> ListID { get; set; }
        public virtual string remarks { get; set; }
        public virtual string department { get; set; }
        public virtual string Type { get; set; }
        public virtual string date { get; set; }
        public virtual string week { get; set; }
        public virtual string autoConfirmDate { get; set; }
        public virtual string paramAllData { get; set; }
        public virtual HttpPostedFile File { get; set; }
    }
}