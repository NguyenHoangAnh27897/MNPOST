﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;
namespace MNPOST.Controllers.mailer
{
    public class MailerDeliveryController : MailerController
    {
        // GET: MailerDelivery
        [HttpGet]
        public ActionResult Show()
        {
            ViewBag.PostOffices = EmployeeInfo.postOffices;
            ViewBag.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.FromDate = DateTime.Now.ToString("dd/MM/yyyy");
            return View();
        }




        [HttpPost]
        public ActionResult GetMailerDelivery(int? page, string search, string fromDate, string toDate, string optionSeach, string postId)
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            if (!CheckPostOffice(postId))
                return Json(new { error = 1, msg = "Không phải bưu cục" }, JsonRequestBehavior.AllowGet);

            if (String.IsNullOrEmpty(fromDate) || String.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString("dd/MM/yyyy");
                toDate = DateTime.Now.ToString("dd/MM/yyyy");
            }

            DateTime paserFromDate = DateTime.Now;
            DateTime paserToDate = DateTime.Now;

            try
            {
                paserFromDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                paserToDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
            }
            catch
            {
                paserFromDate = DateTime.Now;
                paserToDate = DateTime.Now;
            }

            var data = db.MAILER_GET_ALL_DELIVERY(paserFromDate.ToString("yyyy-MM-dd"), paserToDate.ToString("yyyy-MM-dd")).Select(p => new MailerDeliveryIdentity()
            {
                DocumentDate = p.DocumentDate.Value.ToString("dd/MM/yyyy HH:mm"),
                DocumentID = p.DocumentID,
                DocumentCode = p.DocumentCode,
                EmployeeID = p.EmployeeID,
                EmployeeName = p.EmployeeName,
                Notes = p.Notes,
                NumberPlate = p.NumberPlate,
                PostOfficeId = postId,
                Quantity = p.Quantity,
                RouteID = p.RouteID,
                StatusID = p.StatusID,
                Weight = p.Weight
            }).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                page = pageNumber,
                pageSize = pageSize,
                toltalSize = data.Count(),
                data = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetDataHandle(string postId)
        {
            var data = GetEmployeeByPost(postId);

            var licensePlates = new List<CommonData>();

            licensePlates.Add(new CommonData()
            {
                code = "10180",
                name = "Xe chạy chuyến 3320"
            });

            return Json(new { employees = data, licensePlates = licensePlates }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult create(string employeeId, string deliveryDate, string licensePlate, string notes, string postId)
        {

            if (!CheckPostOffice(postId))
                return Json(new ResultInfo() { error = 0, msg = "Sai bưu cục" }, JsonRequestBehavior.AllowGet);
            DateTime date = DateTime.Now;
            try
            {
                date = DateTime.ParseExact(deliveryDate, "dd/MM/yyyy", null);
            }
            catch
            {
                date = DateTime.Now;
            }

            var insDocument = new MM_MailerDelivery()
            {
                DocumentID = Guid.NewGuid().ToString(),
                DocumentCode = postId + DateTime.Now.ToString("ddMMyyyyHH:mm"),
                CreateDate = DateTime.Now,
                DocumentDate = date,
                EmployeeID = employeeId,
                Notes = notes,
                NumberPlate = licensePlate,
                Quantity = 1,
                StatusID = 0,
                Weight = 0
            };

            db.MM_MailerDelivery.Add(insDocument);

            db.SaveChanges();

            return Json(new ResultInfo()
            {
                error = 0,
                msg = "",
                data = new MailerDeliveryIdentity()
                {
                    DocumentDate = insDocument.DocumentDate.Value.ToString("dd/MM/yyyy HH:mm"),
                    DocumentID = insDocument.DocumentID,
                    DocumentCode = insDocument.DocumentCode,
                    EmployeeID = insDocument.EmployeeID,
                    EmployeeName = insDocument.BS_Employees.EmployeeName,
                    Notes = insDocument.Notes,
                    NumberPlate = insDocument.NumberPlate,
                    PostOfficeId = postId,
                    Quantity = insDocument.Quantity,
                    RouteID = insDocument.RouteID,
                    StatusID = insDocument.StatusID,
                    Weight = insDocument.Weight
                }
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetDeliveryMailerDetail(string documentID)
        {

            var data = db.MAILERDELIVERY_GETMAILER(documentID).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult edit(MM_MailerDelivery mailer)
        {
            if (String.IsNullOrEmpty(mailer.DocumentID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_MailerDelivery.Find(mailer.DocumentID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.DocumentDate = mailer.DocumentDate;
            check.EmployeeID = mailer.EmployeeID;
            check.Notes = mailer.Notes;
            check.Quantity = mailer.Quantity;
            check.LastEditDate = mailer.LastEditDate;
            check.StatusID = mailer.StatusID;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string DocumentID)
        {
            if (String.IsNullOrEmpty(DocumentID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_MailerDelivery.Find(DocumentID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);
            var detail = db.MM_MailerDeliveryDetail.Find(DocumentID);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.Entry(detail).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}