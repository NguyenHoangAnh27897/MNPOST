using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MNPOSTAPI.Models;
using MNPOSTCOMMON;

namespace MNPOSTAPI.Controllers.mobile.mailer
{
    public class MailerPresenter 
    {

        MNPOSTEntities db = new MNPOSTEntities();

        public ResponseInfo GetListDelivery(string employeeId)
        {
            var data = db.MAILER_DELIVERY_GETMAILER_EMPLOYEE(employeeId).ToList();

            return new ResponseInfo()
            {
                error = 0,
                data= data
            };
        }
    }
}