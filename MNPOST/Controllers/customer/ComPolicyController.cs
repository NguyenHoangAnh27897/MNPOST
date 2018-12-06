﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOST.Models;
using MNPOSTCOMMON;
using System.Globalization;
using System.Linq;

namespace MNPOST.Controllers.customer
{
    public class ComPolicyController : BaseController
    {
        // GET: ComPolicy
        public ActionResult Show()
        {
            ViewBag.allPost = db.BS_PostOffices.ToList();
            ViewBag.allService = db.BS_ServiceTypes.ToList();
            ViewBag.allCustomer = db.BS_CustomerGroups.ToList();
            ViewBag.allSV = db.BS_Services.ToList();
            return View();
        }
        [HttpGet]
        public ActionResult getComissionPolicy(string search = "")
        {
            var data = db.MM_ComissionPolicys.ToList();
            
            // var data;
            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CheckDV(string discountid)
        {
            var chiTiet = db.MM_ComissionService.Where(p => p.ComissionID == discountid).Select(p => new { ServiceID = p.ServiceID }).ToList();
            return Json(chiTiet, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CheckPT(string discountid)
        {
            var chiTiet = db.MM_ComissionPolicyMethod.Where(p => p.ComissionID == discountid).Select(p => new { ServiceID = p.ServiceID }).ToList();
            return Json(chiTiet, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CheckKH(string discountid)
        {
            var chiTiet = db.MM_ComissionPolicyCustomer.Where(p => p.ComissionID == discountid).Select(p => new { CustomerID = p.CustomerID }).ToList();
            return Json(chiTiet, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getDinhMuc(string discountid)
        {
            var chiTiet = db.MM_ComissionDinhMuc.Where(p => p.ComissionID == discountid).ToList();
            return Json(chiTiet, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetDiscountPolicyDinhMuc(string discountid)
        {
            var data = db.MM_ComissionDinhMuc.Where(p => p.ComissionID == discountid).Select(p =>
                new
                {
                    BatDau = p.ValueBegin,
                    KetThuc = p.ValueEnd,
                    TiLe = p.ComissionPercent
                }).ToList();
            // var data;
            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetDiscountPolicyDichVu(string discountid)
        {
            var data = db.MM_ComissionService.Where(p => p.ComissionID == discountid).ToList();
            // var data;
            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetDiscountPolicyKhachHang(string discountid)
        {
            var data = db.MM_ComissionPolicyCustomer.Where(p => p.ComissionID == discountid).ToList();
            // var data;
            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getMaxId(string postid)
        {
            var maxid = db.MM_ComissionPolicys.Where(p => p.PostOfficeID == postid).OrderByDescending(x => x.ComissionID).FirstOrDefault();
            string maxndg = string.Empty;
            if (maxid != null)
            {
                maxndg = string.Format(postid + "{0}", (Convert.ToUInt32(maxid.ComissionID.Substring(4)) + 1).ToString("D4"));
            }
            else
            {
                maxndg = postid + "0001";
            }
            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                data = maxndg
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult create(string tuNgay, string denNgay, string ngayTao, IdentityComissionPolicy csck, HTCoDinh htcd, List<HTDinhMuc> htdm, List<string> dv, List<string> cus, List<string> pt)
        {

            if (String.IsNullOrEmpty(tuNgay))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            // 20/12/2018
            string tngay = tuNgay.Substring(0, 2);
            string tthang = tuNgay.Substring(3, 2);
            string tnam = tuNgay.Substring(6, 4);

            string dngay = denNgay.Substring(0, 2);
            string dthang = denNgay.Substring(3, 2);
            string dnam = denNgay.Substring(6, 4);

            string cngay = ngayTao.Substring(0, 2);
            string cthang = ngayTao.Substring(3, 2);
            string cnam = ngayTao.Substring(6, 4);

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            //format sever M/d/yyyy
            DateTime dateFrom;
            DateTime dateTo;
            DateTime dateTao;
            if (sysFormat == "M/d/yyyy")
            {
                dateFrom = DateTime.Parse(tthang + '/' + tngay + '/' + tnam);
                dateTo = DateTime.Parse(dthang + '/' + dngay + '/' + dnam);
                dateTao = DateTime.Parse(cthang + '/' + cngay + '/' + cnam);
            }
            else
            {
                dateFrom = DateTime.Parse(tuNgay);
                dateTo = DateTime.Parse(denNgay);
                dateTao = DateTime.Parse(ngayTao);
            }


            var check = db.MM_ComissionPolicys.Where(p => p.ComissionID == csck.ComissionID).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            MM_ComissionPolicys cs = new MM_ComissionPolicys();
            cs.ComissionID = csck.ComissionID;
            cs.PostOfficeID = csck.PostOfficeID;
            cs.CreateDate = dateTao;
            cs.StartDate = dateFrom;
            cs.EndDate = dateTo;
            cs.EditDate = DateTime.Now;
            cs.StatusID = csck.StatusID;
            cs.PermanentCal = bool.Parse(csck.PermanentCal);
            cs.AllCustomer = bool.Parse(csck.AllCustomer);
            cs.AllService = bool.Parse(csck.AllService);
            cs.UserCreate = "LOIVV";
            cs.Description = csck.Description;
            cs.AllMethod = csck.AllMethod;
            db.MM_ComissionPolicys.Add(cs);
            // db.SaveChanges();
            //kiem tra các truong hop lưu bảng riêng
            //co dinh hoac dinh muc
            if (csck.PermanentCal == "true")
            {

                cs.LimitValue = csck.LimitValue;
                cs.CommissionPercent = csck.ComissionPercent;

            }
            else
            {
                foreach (var item in htdm)
                {
                    MM_ComissionDinhMuc dm = new MM_ComissionDinhMuc();
                    dm.ComissionID = csck.ComissionID;
                    dm.ValueBegin = item.BatDau;
                    dm.ValueEnd = item.KetThuc;
                    dm.ComissionPercent = item.TiLe;
                    db.MM_ComissionDinhMuc.Add(dm);
                }

            }
            //tat ca dich vu
            if (csck.AllService == "false")
            {
                foreach (var item in dv)
                {
                    MM_ComissionService sv = new MM_ComissionService();
                    sv.ComissionID = csck.ComissionID;
                    sv.ServiceID = item;
                    db.MM_ComissionService.Add(sv);
                    //db.SaveChanges();
                }
            }
            //tat ca khách hàng
            if (csck.AllCustomer == "false")
            {
                foreach (var item in cus)
                {
                    MM_ComissionPolicyCustomer kh = new MM_ComissionPolicyCustomer();
                    kh.ComissionID = csck.ComissionID;
                    kh.CustomerID = item;
                    db.MM_ComissionPolicyCustomer.Add(kh);
                }
            }
            if (csck.AllMethod == 0 || csck.AllMethod == 2)
            {
                foreach (var item in pt)
                {
                    MM_ComissionPolicyMethod mt = new MM_ComissionPolicyMethod();
                    mt.ComissionID = csck.ComissionID;
                    mt.ServiceID = item;
                    db.MM_ComissionPolicyMethod.Add(mt);
                }
            }
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = sysFormat }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult edit(string tuNgay, string denNgay, string ngayTao, IdentityComissionPolicy csck, HTCoDinh htcd, List<HTDinhMuc> htdm, List<string> dv, List<string> cus, List<string> pt)
        {
            if (String.IsNullOrEmpty(tuNgay))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            // 20/12/2018
            string tngay = tuNgay.Substring(0, 2);
            string tthang = tuNgay.Substring(3, 2);
            string tnam = tuNgay.Substring(6, 4);

            string dngay = denNgay.Substring(0, 2);
            string dthang = denNgay.Substring(3, 2);
            string dnam = denNgay.Substring(6, 4);

            string cngay = ngayTao.Substring(0, 2);
            string cthang = ngayTao.Substring(3, 2);
            string cnam = ngayTao.Substring(6, 4);

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            //format sever M/d/yyyy
            DateTime dateFrom;
            DateTime dateTo;
            DateTime dateTao;
            if (sysFormat == "M/d/yyyy")
            {
                dateFrom = DateTime.Parse(tthang + '/' + tngay + '/' + tnam);
                dateTo = DateTime.Parse(dthang + '/' + dngay + '/' + dnam);
                dateTao = DateTime.Parse(cthang + '/' + cngay + '/' + cnam);
            }
            else
            {
                dateFrom = DateTime.Parse(tuNgay);
                dateTo = DateTime.Parse(denNgay);
                dateTao = DateTime.Parse(ngayTao);
            }


            var check = db.MM_ComissionPolicys.Where(p => p.ComissionID == csck.ComissionID).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.MM_ComissionDinhMuc.RemoveRange(db.MM_ComissionDinhMuc.Where(p => p.ComissionID == csck.ComissionID));
            db.MM_ComissionService.RemoveRange(db.MM_ComissionService.Where(p => p.ComissionID == csck.ComissionID));
            db.MM_ComissionPolicyCustomer.RemoveRange(db.MM_ComissionPolicyCustomer.Where(p => p.ComissionID == csck.ComissionID));
            db.MM_ComissionPolicyMethod.RemoveRange(db.MM_ComissionPolicyMethod.Where(p => p.ComissionID == csck.ComissionID));

            check.CreateDate = dateTao;
            check.StartDate = dateFrom;
            check.EndDate = dateTo;
            check.EditDate = DateTime.Now;
            check.StatusID = csck.StatusID;
            check.PermanentCal = bool.Parse(csck.PermanentCal);
            check.AllCustomer = bool.Parse(csck.AllCustomer);
            check.AllService = bool.Parse(csck.AllService);
            check.UserCreate = "LOIVV";
            check.Description = csck.Description;
            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            check.AllMethod = csck.AllMethod;
            // db.SaveChanges();
            //kiem tra các truong hop lưu bảng riêng
            //co dinh hoac dinh muc
            if (csck.PermanentCal == "true")
            {
                check.LimitValue = csck.LimitValue;
                check.CommissionPercent = csck.ComissionPercent;

            }
            else
            {

                foreach (var item in htdm)
                {
                    MM_ComissionDinhMuc dm = new MM_ComissionDinhMuc();
                    dm.ComissionID = csck.ComissionID;
                    dm.ValueBegin = item.BatDau;
                    dm.ValueEnd = item.KetThuc;
                    dm.ComissionPercent = item.TiLe;
                    db.MM_ComissionDinhMuc.Add(dm);
                }

            }
            //tat ca dich vu
            if (csck.AllService == "false")
            {

                foreach (var item in dv)
                {
                    MM_ComissionService sv = new MM_ComissionService();
                    sv.ComissionID = csck.ComissionID;
                    sv.ServiceID = item;
                    db.MM_ComissionService.Add(sv);
                    //db.SaveChanges();
                }
            }
            //tat ca khách hàng
            if (csck.AllCustomer == "false")
            {

                foreach (var item in cus)
                {
                    MM_ComissionPolicyCustomer kh = new MM_ComissionPolicyCustomer();
                    kh.ComissionID = csck.ComissionID;
                    kh.CustomerID = item;
                    db.MM_ComissionPolicyCustomer.Add(kh);
                }
            }
            if (csck.AllMethod == 0 || csck.AllMethod == 2)
            {
                foreach (var item in pt)
                {
                    MM_ComissionPolicyMethod mt = new MM_ComissionPolicyMethod();
                    mt.ComissionID = csck.ComissionID;
                    mt.ServiceID = item;
                    db.MM_ComissionPolicyMethod.Add(mt);
                }
            }
            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = csck }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult delete(string discountid)
        {
            if (String.IsNullOrEmpty(discountid))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_ComissionPolicys.Where(p => p.ComissionID == discountid).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);
            db.MM_ComissionPolicyCustomer.RemoveRange(db.MM_ComissionPolicyCustomer.Where(p => p.ComissionID == discountid));
            db.MM_ComissionDinhMuc.RemoveRange(db.MM_ComissionDinhMuc.Where(p => p.ComissionID == discountid));
            db.MM_ComissionService.RemoveRange(db.MM_ComissionService.Where(p => p.ComissionID == discountid));
            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}