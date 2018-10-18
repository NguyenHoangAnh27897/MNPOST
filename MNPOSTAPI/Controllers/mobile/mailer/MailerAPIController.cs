using MNPOSTAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MNPOSTAPI.Controllers.mobile.mailer
{
    public class MailerAPIController : BaseMobileController
    {

        MailerPresenter presenter = new MailerPresenter();


        [HttpGet]
        public ResultInfo GetDeliveryByEmployee(string employeeId)
        {

            var result = presenter.GetListDelivery(employeeId);



            return result;

        }
    }
}
