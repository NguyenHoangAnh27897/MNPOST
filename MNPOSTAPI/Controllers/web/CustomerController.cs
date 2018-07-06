using MNPOSTAPI.Models;
using MNPOSTCOMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MNPOSTAPI.Controllers.web
{
    public class CustomerController : WebBaseController
    {
       
        [HttpGet]
    
        public CustomerInfoResult GetCustomerInfo (string id)
        {
            CustomerInfoResult result = new CustomerInfoResult()
            {
                error = 0,
                msg = "400-OK",
                customer = db.BS_Customers.Find(id)
            };

            return result;
            
        }

        [HttpPost]
        public ResultInfo AddCustomer()
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
                var paser = jsonserializer.Deserialize<AddCustomerRequest>(requestContent);

                var data = paser.customer;

                if (data == null)
                    throw new Exception("Sai du lieu gui len");

                data.CustomerID = Guid.NewGuid().ToString();
                data.CustomerCode = generalCusCode();
                data.CreateDate = DateTime.Now;
                data.LastEditDate = DateTime.Now;
                data.CreationDate = DateTime.Now;

                db.BS_Customers.Add(data);
                db.SaveChanges();

                result.msg = data.CustomerID;
            }
            catch (Exception e)
            {
                result.error = 1;
                result.msg = e.Message;
            }
            return result;
        }
        public ResultInfo UpdateCustomer()
        {
            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "Cap nhat thanh cong"
            };

            try
            {
                var requestContent = Request.Content.ReadAsStringAsync().Result;

                var jsonserializer = new JavaScriptSerializer();
                var paser = jsonserializer.Deserialize<AddCustomerRequest>(requestContent);

                var data = paser.customer;
                var checkcustomer = db.BS_Customers.Find(data.CustomerID);
                if (data == null)
                    throw new Exception("Sai du lieu gui len");
                checkcustomer.CustomerName = data.CustomerName;
                checkcustomer.CustomerType = data.CustomerType;
                checkcustomer.Address = data.Address;
                checkcustomer.DistrictID = data.DistrictID;
                checkcustomer.ProvinceID = data.PostOfficeID;
                checkcustomer.CountryID = data.CountryID;
                checkcustomer.FaxNo = data.FaxNo;
                checkcustomer.Email = data.Email;
                checkcustomer.Phone = data.Phone;
                checkcustomer.CompanyPhone = data.CompanyPhone;
                checkcustomer.Mobile = data.Mobile;
                checkcustomer.PersonalInfo = data.PersonalInfo;
                checkcustomer.BankAccount = data.BankAccount;
                checkcustomer.BankName = data.BankName;
                checkcustomer.TaxCode = data.TaxCode;
                checkcustomer.LastEditDate = DateTime.Now;

                db.Entry(checkcustomer).State = System.Data.Entity.EntityState.Modified;               
                db.SaveChanges();

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
