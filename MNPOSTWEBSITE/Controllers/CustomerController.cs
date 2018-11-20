using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MNPOSTWEBSITE.Models;
using MNPOSTWEBSITE.Controllers;
using MNPOSTWEBSITEMODEL;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MNPOSTWEBSITE.Controllers
{
    public class CustomerController : ApiController
    {
        MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEEntities();
        [HttpPost]
        public async Task<ResultInfo> AddCustomer()
        {
            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "Them moi thanh cong"
            };

            try
            {
                var requestContent = Request.Content.ReadAsStringAsync().Result;

                var jsonserializer = new JavaScriptSerializer();
                var paser = jsonserializer.Deserialize<AddCustomerFromWebsiteRequest>(requestContent);

                var data = paser.customer;

                if (data == null)
                    throw new Exception("Sai du lieu gui len");

                AccountController acc = new AccountController();
                RegisterViewModel rg = new RegisterViewModel();
                rg.UserName = data.UserName;
                rg.Password = data.PasswordHash;

                await acc.RegisterFromWeb(rg, data.FullName, data.Phone);
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
