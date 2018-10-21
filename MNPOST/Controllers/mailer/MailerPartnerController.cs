﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;

namespace MNPOST.Controllers.mailer
{
    public class MailerPartnerController : MailerController
    {

        MNPOSTEntities db = new MNPOSTEntities();
        // GET: MailerPartner
        public ActionResult Show()
        {
            ViewBag.PostOffices = EmployeeInfo.postOffices;
            ViewBag.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.FromDate = DateTime.Now.ToString("dd/MM/yyyy");
            var partner = db.BS_Partners.Select(p => new CommonData()
            {
                code = p.PartnerID,
                name = p.ParterName,

            }).ToList();

            ViewBag.Partners = partner;

            return View();
        }

        [HttpPost]
        public ActionResult GetMailerPartner(int? page, string fromDate, string toDate, string postId)
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

            var data = db.MAILER_PARTNER_GETALL(paserFromDate.ToString("yyyy-MM-dd"), paserToDate.ToString("yyyy-MM-dd"), postId).ToList();

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

        [HttpPost]
        public ActionResult CreateMailerPartner(string partnerId, string notes, string postId)
        {
            var insterInfo = new MM_MailerPartner()
            {
                CreateTime = DateTime.Now,
                DocumentCode = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                DocumentID = Guid.NewGuid().ToString(),
                Notes = notes,
                PartnerID = partnerId,
                PostOfficeID = postId,
                StatusID = 0
            };

            db.MM_MailerPartner.Add(insterInfo);
            db.SaveChanges();

            return Json(new ResultInfo()
            {
                error = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMailerPartnerDetail(string documentId)
        {
            var data = db.MAILER_PARTNER_GETDETAIL(documentId).ToList();

            return Json(new ResultInfo()
            {
                data = data,
                error = 0,
                msg = ""

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddMailer (string documentId, string mailerId )
        {
            var checkDocument = db.MM_MailerPartner.Find(documentId);

            if(checkDocument == null || checkDocument.StatusID == 1)
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Không thể cập nhật"
                }, JsonRequestBehavior.AllowGet);
            }

            var mailer = db.MM_Mailers.Find(mailerId);

            if(mailer == null || mailer.CurrentStatusID != 2)
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Sai mã hoặc chưa nhập kho"
                }, JsonRequestBehavior.AllowGet);
            }

            var detail = new MM_MailerPartnerDetail()
            {
                DocumentID = documentId,
                MailerID = mailer.MailerID,
                OrderCosst = 0,
                OrderReference= "",
                StatusID = 0
            };

            db.MM_MailerPartnerDetail.Add(detail);
            db.SaveChanges();

            return Json(new ResultInfo()
            {
                error = 0,
                msg = "",
                data = db.MAILER_PARTNER_GETDETAIL_BY_MAILERID(documentId, mailer.MailerID).FirstOrDefault()
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult UpdateDetails(List<MailerPartnerDetailUpdate> mailers, string documentId)
        {
            foreach(var item in mailers)
            {
                var find = db.MM_MailerPartnerDetail.Where(p => p.DocumentID == documentId && p.MailerID == item.MailerID).FirstOrDefault();

                if(find != null)
                {
                    find.OrderCosst = item.OrderCosst;
                    find.OrderReference = item.OrderReference;
                    db.Entry(find).State = System.Data.Entity.EntityState.Modified;
                }
            }

            db.SaveChanges();

            return Json(new ResultInfo()
            {
                error = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDetail(string documentId, string mailerId)
        {
            var find = db.MM_MailerPartnerDetail.Where(p => p.DocumentID == documentId && p.MailerID == mailerId).FirstOrDefault();

            if(find != null)
            {
                db.MM_MailerPartnerDetail.Remove(find);
                db.SaveChanges();
            }

            return Json(new ResultInfo()
            {
                error = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SendPartner(string documentId)
        {
            var checkDocument = db.MM_MailerPartner.Find(documentId);

            if(checkDocument == null)
            {
                return Json(new ResultInfo()
                {
                    error =1,
                    msg = "Sai thông tin"
                }, JsonRequestBehavior.AllowGet);
            }

            var partner = db.BS_Partners.Find(checkDocument.PartnerID);

            if(partner == null)
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Sai thông tin"
                }, JsonRequestBehavior.AllowGet);
            }
            
            //
            switch(partner.PartnerCode)
            {
                case "VIETTEL":
                    break;
                default:
                    break;
            }

            return Json(new ResultInfo()
            {
                error = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);
        }
    }
}