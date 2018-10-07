﻿using MNPOST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;

namespace MNPOST.Controllers.mailer
{
    public class MailerImportController : MailerController
    {
        // GET: MailerImport
        public ActionResult Show()
        {
            ViewBag.PostOffices = EmployeeInfo.postOffices;
            var customers = db.BS_Customers.Select(p => new {
                name = p.CustomerName,
                code = p.CustomerCode,
                address = p.Address,
                phone = p.Phone
            }).ToList();

  
            ViewBag.AllCustomer = customers;

            return View();
        }


        [HttpGet]
        public ActionResult GetEmployee(string postId)
        {
            var data = db.BS_Employees.Where(p => p.PostOfficeID == postId).Select(p => new CommonData()
            {
                code = p.EmployeeID,
                name = p.EmployeeName

            }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateTakeMailer(string postId, string cusCode, string cusName, string cusAddress, string cusPhone, string content, string employeeId, List<string> mailers)
        {

            var takeMaileInfo = new MM_TakeMailers()
            {
                DocumentID = Guid.NewGuid().ToString(),
                Content = content,
                CreateTime = DateTime.Now,
                CustomerAddress = cusAddress,
                CustomerID = cusCode,
                CustomerName = cusName,
                CustomerPhone = cusPhone,
                UserCreate = EmployeeInfo.user,
                EmployeeID = employeeId,
                StatusID = 7,
                PostID = postId,
                DocumentCode = DateTime.Now.ToString("ddMMyyyyHHmmss")

            };

            db.MM_TakeMailers.Add(takeMaileInfo);
            db.SaveChanges();


            foreach(var item in mailers)
            {
                var findMailer = db.MM_Mailers.Find(item);

                if(findMailer != null)
                {
                    var deltail = new MM_TakeDetails()
                    {
                        DocumentID = takeMaileInfo.DocumentID,
                        MailerID = item,
                        StatusID = 7,
                        TimeTake = null
                    };

                    db.MM_TakeDetails.Add(deltail);
                    db.SaveChanges();


                    findMailer.CurrentStatusID = 7;
                    findMailer.LastUpdateDate = DateTime.Now;
                    db.Entry(findMailer).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }

            }


            return Json(new ResultInfo()
            {
                error = 0
            }, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public ActionResult GetTakeMailers (string postId)
        {
            var data = db.TAKEMAILER_GETLIST(postId, 7).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
       

        [HttpGet]
        public ActionResult GetData(string postId)
        {

            var data = db.MAILER_GET_NOT_INVENTORY("%" + postId + "%").ToList();

            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddMailers(List<string> mailers, string postId)
        {
            List<string> listAdds = new List<string>();
            foreach (var item in mailers)
            {
                var find = db.MM_Mailers.Where(p => p.MailerID == item && p.PostOfficeAcceptID == postId).FirstOrDefault();

                if (find != null && find.CurrentStatusID == 0)
                {
                    find.CurrentStatusID = 2; // nhap kho
                    find.LastUpdateDate = DateTime.Now;
                    db.Entry(find).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    listAdds.Add(item);
                }
            }


            return Json(new ResultInfo()
            {
                error = 0,
                data = listAdds

            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ShowTakeDetail (string documentID)
        {

            var data = db.TAKEMAILER_GETDETAILs(documentID).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public ActionResult UpdateTakeDetails(string documentID, List<string> mailers)
        {

            var checkDocument = db.MM_TakeMailers.Find(documentID);

            if(checkDocument == null)
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Sai thông tin"

                }, JsonRequestBehavior.AllowGet);
            }

            foreach(var item in mailers)
            {

                var checkMailer = db.MM_Mailers.Find(item);

                if (checkMailer == null)
                    continue;

                var findDetail = db.MM_TakeDetails.Where(p => p.DocumentID == documentID && p.MailerID == item).FirstOrDefault();

                if (findDetail == null)
                    continue;

                findDetail.StatusID = 8;
                findDetail.TimeTake = DateTime.Now;
                db.Entry(findDetail).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                checkMailer.CurrentStatusID = 8;
                checkMailer.LastUpdateDate = DateTime.Now;
                db.Entry(checkMailer).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            var checkCount = db.TAKEMAILER_GETDETAILs(documentID).ToList();

            if (checkCount.Count() == 0)
            {
                checkDocument.StatusID = 8;
                db.Entry(checkDocument).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }


            return Json(new ResultInfo()
            {
                error = 0

            }, JsonRequestBehavior.AllowGet);
        }

    }
}