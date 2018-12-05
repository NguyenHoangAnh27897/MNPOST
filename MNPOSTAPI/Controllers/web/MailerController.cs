using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MNPOSTAPI.Models;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using MNPOSTCOMMON;

namespace MNPOSTAPI.Controllers.web
{
    public class MailerController : WebBaseController
    {

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
                var paser = jsonserializer.Deserialize<MailerIdentity>(requestContent);

                var findCus = db.BS_Customers.Where(p => p.CustomerCode == paser.SenderID).FirstOrDefault();

                if (findCus == null)
                    throw new Exception("Sai thông tin");

                if (String.IsNullOrEmpty(findCus.Address) || String.IsNullOrEmpty(findCus.ProvinceID) || String.IsNullOrEmpty(findCus.DistrictID) || String.IsNullOrEmpty(findCus.CustomerName))
                    throw new Exception("Cập nhật lại thông tin cá nhân");

                MailerHandleCommon mailerHandle = new MailerHandleCommon(db);
                var code = mailerHandle.GeneralMailerCode(findCus.PostOfficeID);
                var price = db.CalPrice(paser.Weight, findCus.CustomerID, paser.RecieverProvinceID, paser.MailerTypeID, findCus.PostOfficeID, DateTime.Now.ToString("yyyy-MM-dd")).FirstOrDefault();
                var codPrice = 0;


                // theem
                var mailerIns = new MM_Mailers()
                {
                    MailerID = code,
                    AcceptTime = DateTime.Now,
                    AcceptDate = DateTime.Now,
                    COD = paser.COD,
                    CreationDate = DateTime.Now,
                    CurrentStatusID = 0,
                    HeightSize = paser.HeightSize,
                    Weight = paser.Weight,
                    LengthSize = paser.LengthSize,
                    WidthSize = paser.WidthSize,
                    Quantity = paser.Quantity,
                    PostOfficeAcceptID = findCus.PostOfficeID,
                    CurrentPostOfficeID = findCus.PostOfficeID,
                    EmployeeAcceptID = "",
                    MailerDescription = paser.MailerDescription,
                    MailerTypeID = paser.MailerTypeID,
                    MerchandiseValue = paser.MerchandiseValue,
                    MerchandiseID = paser.MerchandiseID,
                    PriceDefault = price,
                    Price = price,
                    PriceService = paser.PriceService,
                    Amount = price + codPrice,
                    PriceCoD = codPrice,
                    Notes = paser.Notes,
                    PaymentMethodID = paser.PaymentMethodID,
                    RecieverAddress = paser.RecieverAddress,
                    RecieverName = paser.RecieverName,
                    RecieverPhone = paser.RecieverPhone,
                    RecieverDistrictID = paser.RecieverDistrictID,
                    RecieverWardID = paser.RecieverWardID,
                    RecieverProvinceID = paser.RecieverProvinceID,
                    SenderID = findCus.CustomerCode,
                    SenderAddress = findCus.Address,
                    SenderDistrictID = findCus.DistrictID,
                    SenderName = findCus.CustomerName,
                    SenderPhone = findCus.Phone,
                    SenderProvinceID = findCus.ProvinceID,
                    SenderWardID = findCus.WardID,
                    PaidCoD = 0,
                    CreateType = 1
                };

                // 
                db.MM_Mailers.Add(mailerIns);
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
        public ResultInfo GetMailers()
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
                var paser = jsonserializer.Deserialize<MailerShowRequest>(requestContent);

                int pageSize = 50;

                int pageNumber = (paser.page ?? 1);


                DateTime paserFromDate = DateTime.Now;
                DateTime paserToDate = DateTime.Now;

                try
                {
                    paserFromDate = DateTime.ParseExact(paser.fromDate, "dd/MM/yyyy", null);
                    paserToDate = DateTime.ParseExact(paser.toDate, "dd/MM/yyyy", null);
                }
                catch
                {
                    paserFromDate = DateTime.Now;
                    paserToDate = DateTime.Now;
                }

                var findCus = db.BS_Customers.Where(p => p.CustomerCode == paser.customerId).FirstOrDefault();
                if (findCus == null)
                    throw new Exception("sai thông tin");

                var data = db.MAILER_GETALL(paserFromDate.ToString("yyyy-MM-dd"), paserToDate.ToString("yyyy-MM-dd"), "%" + findCus.PostOfficeID + "%", "%" + paser.search + "%").Where(p => p.SenderID.Contains(paser.customerId)).ToList();

                if (paser.status != -1)
                {
                    data = data.Where(p => p.CurrentStatusID == paser.status).ToList();
                }

                result = new ResultWithPaging()
                {
                    error = 0,
                    msg = "",
                    page = pageNumber,
                    pageSize = pageSize,
                    toltalSize = data.Count(),
                    data = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
                };

            }
            catch (Exception e)
            {
                result.error = 1;
                result.msg = e.Message;
            }
            return result;

        }

        [HttpGet]
        public ResultInfo FindMailer(string mailerId)
        {
            var mailer = db.MAILER_GETINFO_BYID(mailerId).FirstOrDefault();

            if (mailer == null)
            {
                return new ResultInfo()
                {
                    error = 1,
                    msg = "Không tìm thấy"
                };
            }
            else
            {
                var data = db.MAILER_GETTRACKING(mailerId).ToList();
                var images = db.MailerImages.Where(p => p.MailerID == mailerId).Select(p => new
                {
                    url = p.PathImage,
                    time = p.CreateTime.Value.ToString("dd/MM/yyyy HH:mm")
                });
                return new ResponseInfo()
                {
                    error = 0,
                    data = new
                    {
                        mailer = mailer,
                        tracks = data
                    }

                };
            }
        }


        [HttpPost]
        public ResultInfo CancelMailers()
        {
            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "Đã thực hiện"
            };
            try
            {
                var requestContent = Request.Content.ReadAsStringAsync().Result;

                var jsonserializer = new JavaScriptSerializer();
                var paser = jsonserializer.Deserialize<CancelMailerRequest>(requestContent);
                var findMailer = db.MM_Mailers.Find(paser.mailerId);

                if (findMailer != null && findMailer.CurrentStatusID == 0)
                {
                    // moi tao moi dc huy
                    findMailer.CurrentStatusID = 10;
                    findMailer.LastUpdateDate = DateTime.Now;
                    findMailer.StatusNotes = paser.reason;
                    db.Entry(findMailer).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

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
