using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARSystem.Domain.Models;

namespace ARSystemFrontEnd.Models
{
    public class PostNotificationData : DatatableAjaxModel
    {
        public string IsRead { get; set; }
        public string UserID { get; set; }
        public string Action { get; set; }
        public long Id { get; set; }

        public Notification DataNotif { get; set; }
    }
}