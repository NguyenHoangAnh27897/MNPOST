using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MNPOSTAPI.Models;
using System.Web.Script.Serialization;
namespace MNPOSTAPI.Controllers.web
{
    public class MailerController : WebBaseController
    {
        [HttpGet]
        public MailerInfoResultbyID GetMailerbyID(string id, string customerid)
        {
            try
            {
                MailerInfoResultbyID result = new MailerInfoResultbyID()
                {
                    error = 0,
                    msg = "400-OK",
                    mailer = db.MM_Mailers.Where(s => s.MailerID == id && s.SenderID == customerid).FirstOrDefault()
                };
                return result;
            }catch
            {
                MailerInfoResultbyID result = new MailerInfoResultbyID()
                {
                    error = 1,
                    msg = "Khong ket noi he thong"
                };
                return result;
            }
            
        }
        public MailerInfoResult GetMailerbyCustomerIDandDate(string customerid,DateTime fromdate,DateTime todate)
        {
            try
            {
                MailerInfoResult result = new MailerInfoResult()
                {
                    error = 0,
                    msg = "400-OK",
                    mailer = db.MM_Mailers.Where(p => p.SenderID == customerid && p.AcceptDate >= fromdate.Date && p.AcceptDate <= todate.Date).ToList()
                };
                return result;
            }catch
            {
                MailerInfoResult result = new MailerInfoResult()
                {
                    error = 1,
                    msg = "Khong ket noi he thong"
                };
                return result;
            }          
        }
        [HttpGet]  
        public MailerInfoResult GetMailerbyCustomerID(string customerid)
        {
            try
            {
                MailerInfoResult result = new MailerInfoResult()
                {
                    error = 0,
                    msg = "400-OK",
                    mailer = db.MM_Mailers.Where(p => p.SenderID == customerid).ToList()
                };
                return result;
            }
            catch
            {
                MailerInfoResult result = new MailerInfoResult()
                {
                    error = 1,
                    msg = "Khong ket noi he thong"
                };
                return result;
            }
        }

        [HttpPost]
        public ResultInfo DeleteCustomerByCustomerID()
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
                var paser = jsonserializer.Deserialize<AddMailerRequest>(requestContent);

                var data = paser.mailer;
                var checkmailer = db.MM_Mailers.Find(data.MailerID);
                if (data == null)
                    throw new Exception("Sai du lieu gui len");

                db.MM_Mailers.Remove(checkmailer); ;
                db.SaveChanges();

            }
            catch (Exception e)
            {
                result.error = 1;
                result.msg = e.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultInfo AddMailer()
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
                var paser = jsonserializer.Deserialize<AddMailerRequest>(requestContent);

                var data = paser.mailer;

                if (data == null)
                    throw new Exception("Sai du lieu gui len");

                data.CurrentStatusID = 0;
                data.CreationDate = DateTime.Now;
                data.LastUpdateDate = DateTime.Now;

                db.MM_Mailers.Add(data);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                result.error = 1;
                result.msg = e.Message;
            }
            return result;
        }
        public ResultInfo UpdateMailer()
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
                var paser = jsonserializer.Deserialize<AddMailerRequest>(requestContent);

                var data = paser.mailer;
                var checkmailer = db.MM_Mailers.Find(data.MailerID);
                if (data == null)
                    throw new Exception("Sai du lieu gui len");
                checkmailer.PostOfficeAcceptID = data.PostOfficeAcceptID;
                checkmailer.SenderID = data.SenderID;
                checkmailer.SenderName = data.SenderName;
                checkmailer.SenderAddress = data.SenderAddress;
                checkmailer.SenderWardID = data.SenderWardID;
                checkmailer.SenderDistrictID = data.SenderDistrictID;
                checkmailer.SenderProvinceID = data.SenderProvinceID;
                checkmailer.SenderPhone = data.SenderPhone;
                checkmailer.RecieverName = data.RecieverName;
                checkmailer.RecieverAddress = data.RecieverAddress;
                checkmailer.RecieverWardID = data.RecieverWardID;
                checkmailer.RecieverDistrictID = data.RecieverDistrictID;
                checkmailer.RecieverProvinceID = data.RecieverProvinceID;
                checkmailer.RecieverPhone = data.RecieverPhone;
                checkmailer.EmployeeAcceptID = data.EmployeeAcceptID;
                checkmailer.AcceptDate = data.AcceptDate;
                checkmailer.AcceptTime = data.AcceptTime;
                checkmailer.MailerTypeID = data.MailerTypeID;
                checkmailer.Quantity = data.Quantity;
                checkmailer.Weight = data.Weight;
                checkmailer.ReWeight = data.ReWeight;
                checkmailer.PriceDefault = data.PriceDefault;
                checkmailer.Price = data.Price;
                checkmailer.PriceService = data.PriceService;
                checkmailer.Discount = data.Discount;
                checkmailer.BefVATAmount = data.BefVATAmount;
                checkmailer.VATPercent = data.VATPercent;
                checkmailer.VATAmount = data.VATAmount;
                checkmailer.Amount = data.Amount;
                checkmailer.AmountBefDiscount = data.AmountBefDiscount;
                checkmailer.PaymentMethodID = data.PaymentMethodID;
                checkmailer.MailerDescription = data.MailerDescription;
                checkmailer.ThirdpartyDocID = data.ThirdpartyDocID;
                checkmailer.ThirdpartyCost = data.ThirdpartyCost;
                checkmailer.ThirdpartyPaymentMethodID = data.ThirdpartyPaymentMethodID;
                checkmailer.CurrentStatusID = data.CurrentStatusID;
                checkmailer.CurrentPostOfficeID = data.CurrentPostOfficeID;
                checkmailer.PriceType = data.PriceType;//loai bang gia
                checkmailer.PriceIncludeVAT = data.PriceIncludeVAT;
                checkmailer.CommissionAmt = data.CommissionAmt;
                checkmailer.CommissionPercent = data.CommissionPercent;
                checkmailer.CostAmt = data.CostAmt;
                checkmailer.SalesClosingDate = data.SalesClosingDate;
                checkmailer.DiscountPercent = data.DiscountPercent;
                checkmailer.LastUpdateDate = DateTime.Now;

                db.Entry(checkmailer).State = System.Data.Entity.EntityState.Modified;
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
