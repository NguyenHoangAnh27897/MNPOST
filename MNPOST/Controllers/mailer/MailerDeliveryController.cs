using System;
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
            List<CommonData> allProvince = GetProvinceDatas("", "province");
            ViewBag.Provinces = allProvince;
            ViewBag.ReturnReasons = db.BS_ReturnReasons.Select(p => new { name = p.ReasonName, code = p.ReasonID }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult GetReportEmployeeDelivery(string postId, List<string> employees)
        {
            var allDatas = db.MAILER_DELIVERY__EMPLOYEE(postId).ToList();

            if (employees == null)
                employees = new List<string>();

            if(employees.Count() > 0)
            {
                allDatas = allDatas.Where(p => employees.Contains(p.EmployeeID)).ToList();
            }

            return Json(new ResultInfo()
            {
                error = 0,
                data = allDatas
            }, JsonRequestBehavior.AllowGet);
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

            var data = db.MAILER_GET_ALL_DELIVERY(paserFromDate.ToString("yyyy-MM-dd"), paserToDate.ToString("yyyy-MM-dd"), postId).ToList();

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
        public ActionResult AddMailer(string mailerId, string documentId)
        {
            var mailer = db.MM_Mailers.Find(mailerId);
            if (mailer == null)
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Sai thông tin"
                }, JsonRequestBehavior.AllowGet);
            }

            if (mailer.CurrentStatusID != 2 && mailer.CurrentStatusID != 6)
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Mã hàng không thể phân phát"
                }, JsonRequestBehavior.AllowGet);
            }

            var delivery = db.MM_MailerDelivery.Find(documentId);

            if (delivery == null || delivery.StatusID != 0)
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Không thể phân phát"
                }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var data = db.MAILERDELIVERY_GETMAILER_BY_ID(documentId, mailerId).FirstOrDefault();

                if (data != null)
                {
                    return Json(new ResultInfo()
                    {
                        error = 1,
                        msg = "Mã đã tồn tại"
                    }, JsonRequestBehavior.AllowGet);
                }

                var insData = new MM_MailerDeliveryDetail()
                {
                    DocumentID = documentId,
                    MailerID = mailerId,
                    CreationDate = DateTime.Now,
                    DeliveryStatus = 3,
                };

                db.MM_MailerDeliveryDetail.Add(insData);

                db.SaveChanges();

                mailer.CurrentStatusID = 3;
                mailer.LastUpdateDate = DateTime.Now;
                db.Entry(mailer).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                delivery.Quantity = delivery.Quantity + 1;
                delivery.Weight = delivery.Weight + mailer.Weight;

                db.Entry(delivery).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


                // luu tracking
                var employee = db.BS_Employees.Where(p => p.EmployeeID == delivery.EmployeeID).FirstOrDefault();
                HandleHistory.AddTracking(3, mailerId, mailer.CurrentPostOfficeID, "Nhân viên " + employee.EmployeeName + "(" + employee.Phone + ") , đang đi phát hàng");

                data = db.MAILERDELIVERY_GETMAILER_BY_ID(documentId, mailerId).FirstOrDefault();

                return Json(new ResultInfo()
                {
                    error = 0,
                    msg = "",
                    data = data
                }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Lỗi cập nhật"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult AddListMailer(List<string> mailers, string documentId)
        {
            var delivery = db.MM_MailerDelivery.Find(documentId);
            var employee = db.BS_Employees.Where(p => p.EmployeeID == delivery.EmployeeID).FirstOrDefault();
            if (delivery == null || delivery.StatusID != 0)
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Không thể phân phát"
                }, JsonRequestBehavior.AllowGet);
            }
            foreach (var item in mailers)
            {
                var mailer = db.MM_Mailers.Find(item);
                if (mailer == null)
                    continue;

                if (mailer.CurrentStatusID != 2 && mailer.CurrentStatusID != 6)
                    continue;

                var data = db.MAILERDELIVERY_GETMAILER_BY_ID(documentId, item).FirstOrDefault();
                if (data != null)
                    continue;

                var insData = new MM_MailerDeliveryDetail()
                {
                    DocumentID = documentId,
                    MailerID = item,
                    CreationDate = DateTime.Now,
                    DeliveryStatus = 3,
                };

                db.MM_MailerDeliveryDetail.Add(insData);

                db.SaveChanges();

                mailer.CurrentStatusID = 3;
                mailer.LastUpdateDate = DateTime.Now;
                db.Entry(mailer).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

              
                HandleHistory.AddTracking(3, item, mailer.CurrentPostOfficeID, "Nhân viên " + employee.EmployeeName + "(" + employee.Phone + ") , đang đi phát hàng");

                delivery.Quantity = delivery.Quantity + 1;
                delivery.Weight = delivery.Weight + mailer.Weight;

            }

            db.Entry(delivery).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo()
            {
                error = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);
        }

        // huy mailer moi phan phat 
        // mailer trang thai la 3 - dang phat
        [HttpPost]
        public ActionResult DetroyMailerDelivery(string mailerId, string documentId)
        {
            var data = db.MM_MailerDeliveryDetail.Where(p => p.DocumentID == documentId && p.MailerID == mailerId).FirstOrDefault();
            var mailer = db.MM_Mailers.Find(mailerId);
            var delivery = db.MM_MailerDelivery.Find(documentId);

            if (data == null || mailer == null || delivery == null)
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Sai thông tin"
                }, JsonRequestBehavior.AllowGet);

          
            if(mailer.CurrentStatusID != 3)
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Đơn đã được phát"
                }, JsonRequestBehavior.AllowGet);

            // remove in tracking
            var tracking = db.MM_Tracking.Where(p => p.StatusID == 3 && p.MailerID == mailer.MailerID).OrderByDescending(p=> p.CreateTime).FirstOrDefault();

            if(tracking != null)
            {
                db.MM_Tracking.Remove(tracking);
                db.SaveChanges();
            }

            //
            db.MM_MailerDeliveryDetail.Remove(data);
            
            mailer.CurrentStatusID = 2;
            mailer.LastUpdateDate = DateTime.Now;

            delivery.Quantity = delivery.Quantity - 1;
            delivery.Weight = delivery.Weight - mailer.Weight;

            db.Entry(mailer).State = System.Data.Entity.EntityState.Modified;
            db.Entry(delivery).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();

            return Json(new ResultInfo()
            {
                error = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(string employeeId, string deliveryDate, string licensePlate, string notes, string postId)
        {

            if (!CheckPostOffice(postId))
                return Json(new ResultInfo() { error = 0, msg = "Sai bưu cục" }, JsonRequestBehavior.AllowGet);
            DateTime date = DateTime.Now;
            try
            {
                date = DateTime.ParseExact(deliveryDate, "dd/M/yyyy", null);
            }
            catch
            {
                date = DateTime.Now;
            }

            var checkEmployyee = db.BS_Employees.Find(employeeId);
            if (checkEmployyee == null)
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Sai nhân viên"
                }, JsonRequestBehavior.AllowGet);

            var insDocument = new MM_MailerDelivery()
            {
                DocumentID = Guid.NewGuid().ToString(),
                DocumentCode = postId + DateTime.Now.ToString("ddMMyyyyHHmmss"),
                CreateDate = DateTime.Now,
                DocumentDate = date,
                EmployeeID = employeeId,
                Notes = notes,
                NumberPlate = licensePlate,
                Quantity = 0,
                StatusID = 0,
                Weight = 0,
                PostID = postId
            };

            db.MM_MailerDelivery.Add(insDocument);

            db.SaveChanges();

            return Json(new ResultInfo()
            {
                error = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetDeliveryMailerDetail(string documentID)
        {
            var data = db.MAILERDELIVERY_GETMAILER(documentID).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        // UPDATE DELIVER 
        [HttpGet]
        public ActionResult GetDeliveryMailerDetailNotUpdate(string documentID)
        {
            var data = db.MAILERDELIVERY_GETMAILER(documentID).Where(p => p.DeliveryStatus == 3).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetMailerForReUpdate(string mailerID)
        {

            var findLastDelivery = db.MM_MailerDeliveryDetail.Where(p => p.MailerID == mailerID).OrderByDescending(p => p.CreationDate).FirstOrDefault();

            if (findLastDelivery == null)
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Mã chưa được phân phát"
                }, JsonRequestBehavior.AllowGet);

            }

            // kiem tra đã chốt công nợ chưa


            // lấy thông tin

            var data = db.MAILERDELIVERY_GETMAILER_BY_ID(findLastDelivery.DocumentID, findLastDelivery.MailerID).FirstOrDefault();

            if(data.CurrentStatusID != 3)
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Mã không thể cập nhật"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new ResultInfo()
            {
                error = 0,
                msg = "",
                data = data
            }, JsonRequestBehavior.AllowGet);

        }

        // lay tuyen tu dong
        [HttpGet]
        public ActionResult AutoGetRouteEmployees(string postId)
        {
            var employees = db.BS_Employees.Where(p => p.PostOfficeID == postId).ToList();
            List<EmployeeAutoRouteInfo> routeAutoes = new List<EmployeeAutoRouteInfo>();

            var countMailers = db.MM_Mailers.Where(p => p.CurrentPostOfficeID == postId && (p.CurrentStatusID == 2 || p.CurrentStatusID == 6)).ToList();

            foreach (var item in employees)
            {
                var listMailer = db.ROUTE_GETMAILER_BYEMPLOYEEID(item.EmployeeID, postId).ToList();
                var data = new EmployeeAutoRouteInfo()
                {
                    EmployeeID = item.EmployeeID,
                    EmployeeName = item.EmployeeName,
                    Mailers = new List<MailerIdentity>()
                };

                foreach (var mailer in listMailer)
                {
                    if (mailer.IsDetail == true)
                    {
                        // check phuong
                        var wardCheck = db.BS_RouteDetails.Where(p => p.RouteID == mailer.RouteID && p.WardID == mailer.RecieverWardID).FirstOrDefault();

                        if (wardCheck != null)
                        {
                            data.Mailers.Add(new MailerIdentity()
                            {
                                MailerID = mailer.MailerID,
                                COD = mailer.COD,
                                SenderName = mailer.SenderName,
                                SenderAddress = mailer.SenderAddress,
                                RecieverAddress = mailer.RecieverAddress,
                                RecieverProvinceID = mailer.RecieverProvinceID,
                                RecieverDistrictID = mailer.RecieverDistrictID,
                                RecieverWardID = mailer.RecieverWardID,
                                RecieverPhone = mailer.RecieverPhone,
                                CurrentStatusID = mailer.CurrentStatusID,
                                MailerTypeID = mailer.MailerTypeID
                            });
                        }

                    }
                    else
                    {
                        data.Mailers.Add(new MailerIdentity()
                        {
                            MailerID = mailer.MailerID,
                            COD = mailer.COD,
                            SenderName = mailer.SenderName,
                            SenderAddress = mailer.SenderAddress,
                            RecieverAddress = mailer.RecieverAddress,
                            RecieverProvinceID = mailer.RecieverProvinceID,
                            RecieverDistrictID = mailer.RecieverDistrictID,
                            RecieverWardID = mailer.RecieverWardID,
                            RecieverPhone = mailer.RecieverPhone,
                            CurrentStatusID = mailer.CurrentStatusID,
                            MailerTypeID = mailer.MailerTypeID
                        });
                    }
                }

                routeAutoes.Add(data);
            }

            return Json(new { routes = routeAutoes, coutMailer = countMailers.Count() }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetAutoMailerFromEmployeeRoute(string postId, string employeeId)
        {
            var employee = db.BS_Employees.Find(employeeId);

            var countMailers = db.MM_Mailers.Where(p => p.CurrentPostOfficeID == postId && (p.CurrentStatusID == 2 || p.CurrentStatusID == 6)).ToList();

            var listMailer = db.ROUTE_GETMAILER_BYEMPLOYEEID(employee.EmployeeID, postId).ToList();
            var data = new EmployeeAutoRouteInfo()
            {
                EmployeeID = employee.EmployeeID,
                EmployeeName = employee.EmployeeName,
                Mailers = new List<MailerIdentity>()
            };

            foreach (var mailer in listMailer)
            {
                if (mailer.IsDetail == true)
                {
                    // check phuong
                    var wardCheck = db.BS_RouteDetails.Where(p => p.RouteID == mailer.RouteID && p.WardID == mailer.RecieverWardID).FirstOrDefault();

                    if (wardCheck != null)
                    {
                        data.Mailers.Add(new MailerIdentity()
                        {
                            MailerID = mailer.MailerID,
                            COD = mailer.COD,
                            SenderName = mailer.SenderName,
                            SenderAddress = mailer.SenderAddress,
                            RecieverAddress = mailer.RecieverAddress,
                            RecieverProvinceID = mailer.RecieverProvinceID,
                            RecieverDistrictID = mailer.RecieverDistrictID,
                            RecieverWardID = mailer.RecieverWardID,
                            RecieverPhone = mailer.RecieverPhone,
                            CurrentStatusID = mailer.CurrentStatusID,
                            MailerTypeID = mailer.MailerTypeID
                        });
                    }

                }
                else
                {
                    data.Mailers.Add(new MailerIdentity()
                    {
                        MailerID = mailer.MailerID,
                        COD = mailer.COD,
                        SenderName = mailer.SenderName,
                        SenderAddress = mailer.SenderAddress,
                        RecieverAddress = mailer.RecieverAddress,
                        RecieverProvinceID = mailer.RecieverProvinceID,
                        RecieverDistrictID = mailer.RecieverDistrictID,
                        RecieverWardID = mailer.RecieverWardID,
                        RecieverPhone = mailer.RecieverPhone,
                        CurrentStatusID = mailer.CurrentStatusID,
                        MailerTypeID = mailer.MailerTypeID
                    });
                }
            }

            return Json(new { routes = data, countMailer = countMailers.Count() }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetMailerForEmployee(string postId, string province, string district)
        {
            var mailers = db.MM_Mailers.Where(p => p.CurrentPostOfficeID == postId && (p.CurrentStatusID == 2 || p.CurrentStatusID == 6) && p.RecieverProvinceID.Contains(province) && p.RecieverDistrictID.Contains(district)).ToList();
            var data = new List<MailerIdentity>();
            foreach (var mailer in mailers)
            {
                data.Add(new MailerIdentity()
                {
                    MailerID = mailer.MailerID,
                    COD = mailer.COD,
                    SenderName = mailer.SenderName,
                    SenderAddress = mailer.SenderAddress,
                    RecieverAddress = mailer.RecieverAddress,
                    RecieverProvinceID = mailer.RecieverProvinceID,
                    RecieverDistrictID = mailer.RecieverDistrictID,
                    RecieverWardID = mailer.RecieverWardID,
                    RecieverPhone = mailer.RecieverPhone,
                    CurrentStatusID = mailer.CurrentStatusID,
                    MailerTypeID = mailer.MailerTypeID
                });
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateFromRoutes(List<EmployeeAutoRouteInfo> routes, string postId, string deliveryDate)
        {
            DateTime date = DateTime.Now;
            try
            {
                date = DateTime.ParseExact(deliveryDate, "dd/MM/yyyy", null);
            }
            catch
            {
                date = DateTime.Now;
            }

            List<MailerDeliveryIdentity> result = new List<MailerDeliveryIdentity>();

            foreach (var item in routes)
            {
                if (item.Mailers.Count() == 0)
                    continue;

                var insDocument = new MM_MailerDelivery()
                {
                    DocumentID = Guid.NewGuid().ToString(),
                    DocumentCode = postId + DateTime.Now.ToString("ddMMyyyyHHmmss"),
                    CreateDate = DateTime.Now,
                    DocumentDate = date,
                    EmployeeID = item.EmployeeID,
                    Notes = "Tạo từ chia tuyến phát",
                    NumberPlate = "",
                    Quantity = 0,
                    StatusID = 0,
                    Weight = 0
                };

                db.MM_MailerDelivery.Add(insDocument);

                db.SaveChanges();

                int allQuantity = 0;
                double? allWeight = 0;

                // add to detail
                foreach (var mailer in item.Mailers)
                {
                    var checkMailer = db.MM_Mailers.Where(p => p.MailerID == mailer.MailerID && (p.CurrentStatusID == 2 || p.CurrentStatusID == 6)).FirstOrDefault();

                    if (checkMailer != null)
                    {
                        var insData = new MM_MailerDeliveryDetail()
                        {
                            DocumentID = insDocument.DocumentID,
                            MailerID = checkMailer.MailerID,
                            CreationDate = DateTime.Now,
                            DeliveryStatus = 3,
                        };

                        db.MM_MailerDeliveryDetail.Add(insData);

                        checkMailer.CurrentStatusID = 3;
                        checkMailer.LastUpdateDate = DateTime.Now;
                        db.Entry(checkMailer).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();

                        allQuantity++;
                        allWeight = allWeight + checkMailer.Weight;
                    }
                }

                var deliveryDocument = db.MM_MailerDelivery.Find(insDocument.DocumentID);

                deliveryDocument.Quantity = allQuantity;
                deliveryDocument.Weight = allWeight;
                db.Entry(deliveryDocument).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                result.Add(new MailerDeliveryIdentity()
                {
                    DocumentDate = deliveryDocument.DocumentDate.Value.ToString("dd/MM/yyyy HH:mm"),
                    DocumentID = deliveryDocument.DocumentID,
                    DocumentCode = deliveryDocument.DocumentCode,
                    EmployeeID = deliveryDocument.EmployeeID,
                    EmployeeName = deliveryDocument.BS_Employees.EmployeeName,
                    Notes = deliveryDocument.Notes,
                    NumberPlate = deliveryDocument.NumberPlate,
                    PostOfficeId = postId,
                    Quantity = deliveryDocument.Quantity,
                    RouteID = deliveryDocument.RouteID,
                    StatusID = deliveryDocument.StatusID,
                    Weight = deliveryDocument.Weight
                });
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        // update phat
        [HttpPost]
        public ActionResult ConfirmDeliveyMailer(List<MailerDeliveryConfirmInfo> detail)
        {
            List<string> documents = new List<string>();
            foreach (var item in detail)
            {
                var findDetail = db.MM_MailerDeliveryDetail.Where(p => p.DocumentID == item.DocumentID && p.MailerID == item.MailerID).FirstOrDefault();

                if (findDetail != null)
                {
                    var mailerInfo = db.MM_Mailers.Find(findDetail.MailerID);

                    var findDocument = db.MM_MailerDelivery.Where(p => p.DocumentID == item.DocumentID).FirstOrDefault();

                    if (findDocument == null)
                        continue;

                    findDetail.DeliveryStatus = item.DeliveryStatus;
                    mailerInfo.CurrentStatusID = item.DeliveryStatus;

                    var deliveryDate = DateTime.ParseExact(item.DeliveryDate + " " + item.DeliveryTime, "dd/M/yyyy HH:mm", null);

                    if (deliveryDate == null)
                        deliveryDate = DateTime.Now;

                    if (item.DeliveryStatus == 5)
                    {
                        var findReason = db.BS_ReturnReasons.Where(p => p.ReasonID == item.ReturnReasonID).FirstOrDefault();

                        findDetail.DeliveryTo = "";
                        findDetail.DeliveryNotes = findReason.ReasonName;
                        findDetail.ReturnReasonID = item.ReturnReasonID;
                        findDetail.DeliveryDate = deliveryDate;


                        mailerInfo.DeliveryTo = "";
                        mailerInfo.DeliveryDate = deliveryDate;
                        mailerInfo.DeliveryNotes = findReason.ReasonName;

                        HandleHistory.AddTracking(5, item.MailerID, mailerInfo.CurrentPostOfficeID, "Trả lại hàng, vì lý do " + findReason.ReasonName);
                    }
                    else if (item.DeliveryStatus == 6)
                    {
                        findDetail.DeliveryTo = "";
                        findDetail.DeliveryNotes = item.DeliveryNotes;
                        findDetail.DeliveryDate = deliveryDate;

                        mailerInfo.DeliveryTo = "";
                        mailerInfo.DeliveryDate = deliveryDate;
                        mailerInfo.DeliveryNotes = item.DeliveryNotes;

                        HandleHistory.AddTracking(6, item.MailerID, mailerInfo.CurrentPostOfficeID, "Chưa phát được vì " + item.DeliveryNotes);
                    }
                    else if (item.DeliveryStatus == 4)
                    {
                        findDetail.DeliveryTo = item.DeliveryTo;
                        findDetail.ReturnReasonID = null;
                        findDetail.DeliveryNotes = "Đã phát";
                        findDetail.DeliveryDate = deliveryDate;

                        mailerInfo.DeliveryTo = item.DeliveryTo;
                        mailerInfo.DeliveryDate = deliveryDate;
                        mailerInfo.DeliveryNotes = "Đã phát";

                        HandleHistory.AddTracking(4, item.MailerID, mailerInfo.CurrentPostOfficeID, "Ngày phát " + deliveryDate.ToString("dd/MM/yyyy") + " lúc " + deliveryDate.ToString("HH:mm") + ", người nhận: " + item.DeliveryTo);
                        // save nhung don co thu tien COD
                        if (mailerInfo.COD > 0)
                        {
                            var saveCoDDebit = new EmpployeeDebitCOD()
                            {
                                Id = Guid.NewGuid().ToString(),
                                AccountantConfirm = 0,
                                COD = Convert.ToDouble(mailerInfo.COD),
                                ConfirmDate = DateTime.Now,
                                CreateDate = DateTime.Now,
                                DocumentID = item.DocumentID,
                                EmployeeID = findDocument.EmployeeID,
                                MailerID = mailerInfo.MailerID
                            };

                            db.EmpployeeDebitCODs.Add(saveCoDDebit);
                        }
                    }


                    db.Entry(mailerInfo).State = System.Data.Entity.EntityState.Modified;
                    db.Entry(findDetail).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    if (!documents.Contains(findDetail.DocumentID))
                    {
                        documents.Add(findDetail.DocumentID);
                    }
                }
            }

            foreach (var item in documents)
            {
                UpdateDeliveryStatus(item);
            }

            return Json(new ResultInfo()
            {
                error = 0,
                msg = ""
            }, JsonRequestBehavior.AllowGet);
        }

        public void UpdateDeliveryStatus(string documentId)
        {
            var find = db.MM_MailerDelivery.Find(documentId);

            var findDetail = db.MM_MailerDeliveryDetail.Where(p => p.DocumentID == documentId).ToList();

            var countSucess = 0;

            foreach (var item in findDetail)
            {
                if (item.DeliveryStatus == 4 || item.DeliveryStatus == 5)
                {
                    countSucess++;
                }
            }

            if (countSucess == findDetail.Count())
            {
                find.StatusID = 3;
            }
            else
            {
                find.StatusID = 2;
            }

            db.Entry(find).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

    }
}