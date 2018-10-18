using MNPOSTAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

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

        [HttpGet]
        public ResultInfo GetDeliveryEmployeeHistory(string employeeId, string date)
        {
            return presenter.GetListHistoryDelivery(employeeId, date);
        }

        [HttpGet]
        public ResultInfo GetReturnReasons()
        {

            return presenter.GetListReturnReason();
        }

        [HttpPost]
        public ResultInfo UpdateDelivery()
        {

            var result = new ResultInfo()
            {
                error = 0,
                msg = "success"
            };

            try
            {
          
                var requestContent = Request.Content.ReadAsStringAsync().Result;
                var jsonserializer = new JavaScriptSerializer();
                var paser = jsonserializer.Deserialize<UpdateDeliveryReceive>(requestContent);

                var user = User.Identity.Name;

                result = presenter.UpdateDelivery(paser, user);

            }
            catch (Exception e)
            {
                result.error = 1;
                result.msg = e.Message;
            }
            return result;
        }

    }
}
