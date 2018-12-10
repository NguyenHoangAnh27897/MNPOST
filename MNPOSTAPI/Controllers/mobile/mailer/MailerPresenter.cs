﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MNPOSTAPI.Models;
using MNPOSTCOMMON;

namespace MNPOSTAPI.Controllers.mobile.mailer
{
    public class MailerPresenter
    {

        MNPOSTEntities db = new MNPOSTEntities();

        MNHistory HandleHistory = new MNHistory();

        public ResponseInfo GetListDelivery(string employeeId)
        {
            var data = db.MAILER_DELIVERY_GETMAILER_EMPLOYEE(employeeId).ToList();

            return new ResponseInfo()
            {
                error = 0,
                data = data
            };
        }


        public ResultInfo GetListHistoryDelivery(string employeeId, string date)
        {
            try
            {

                var dateChoose = DateTime.ParseExact(date, "dd/M/yyyy", null);

                if (dateChoose == null)
                    throw new Exception("Sai thông tin");

                var data = db.MAILER_DELIVERY_GETMAILER_EMPLOYEE_BYDATE(employeeId, dateChoose.ToString("yyyy-MM-dd")).ToList();

                return new ResponseInfo()
                {
                    error = 0,
                    data = data
                };


            } catch(Exception e)
            {
                return new ResultInfo()
                {
                    error = 1,
                    msg = e.Message
                };
            }


        }

        // get danh sach ly do
        public ResponseInfo GetListReturnReason()
        {
            var data = db.BS_ReturnReasons.Where(p => p.IsActive == true).Select(p => new { code = p.ReasonID, name = p.ReasonName }).ToList();

            return new ResponseInfo()
            {
                error = 0,
                data = data
            };
        }


        public ResponseInfo GetReportDelivert(string employeeId, string fDate, string tDate)
        {
            var reportDelivery = db.DELIVERY_GETREPORT_EMPLOYEE(employeeId, fDate, tDate).Select(p => new
            {
                code = p.DeliveryStatus,
                data = p.CountMailer
            }).ToList();

            var reportCOD = db.EMPLOYEE_DEBIT_REPORT_BY_EMPLOYEEID(employeeId, fDate, tDate).Select(p => new
            {
                code = p.AccountantConfirm,
                data = p.MoneySum
            }).ToList();

            return new ResponseInfo()
            {
                error = 0,
                data = new
                {
                    RDelivery = reportDelivery,
                    RCOD = reportCOD
                }
            };

        }


        // cap nhat mailer
        public ResultInfo UpdateDelivery(UpdateDeliveryReceive info, string user)
        {

            var result = new ResultInfo()
            {
                error = 0,
                msg = "success"
            };

            try
            {
                var checkUser = db.BS_Employees.Where(p => p.UserLogin == user).FirstOrDefault();

                if (checkUser == null)
                    throw new Exception("Sai thông tin");

                // check Document of employee
                var document = db.MM_MailerDelivery.Where(p => p.DocumentID == info.DocumentID && p.EmployeeID == checkUser.EmployeeID).FirstOrDefault();

                if (document == null)
                    throw new Exception("Đơn này không được phân cho bạn phát");

                // find detail
                var findDetail = db.MM_MailerDeliveryDetail.Where(p => p.DocumentID == info.DocumentID && p.MailerID == info.MailerID).FirstOrDefault();

                if (findDetail == null)
                    throw new Exception("Sai thông tin");


                DateTime deliverDate = DateTime.ParseExact(info.DeliveryDate, "dd/M/yyyy HH:mm", null);

                if (deliverDate == null)
                    deliverDate = DateTime.Now;

                //
                var mailerInfo = db.MM_Mailers.Find(findDetail.MailerID);

                if (mailerInfo == null)
                    throw new Exception("Vận đơn sai");

                findDetail.DeliveryStatus = info.StatusID;
                mailerInfo.CurrentStatusID = info.StatusID;

                if (info.StatusID == 5)
                {
                    var findReason = db.BS_ReturnReasons.Where(p => p.ReasonID == info.ReturnReasonID).FirstOrDefault();

                    findDetail.DeliveryTo = "";
                    findDetail.DeliveryNotes = findReason.ReasonName;
                    findDetail.ReturnReasonID = info.ReturnReasonID;
                    findDetail.DeliveryDate = deliverDate;

                   
                    mailerInfo.DeliveryTo = "";
                    mailerInfo.DeliveryDate = deliverDate;
                    mailerInfo.DeliveryNotes = findReason.ReasonName;

                    HandleHistory.AddTracking(5, info.MailerID, mailerInfo.CurrentPostOfficeID, "Trả lại hàng, vì lý do " + findReason.ReasonName);

                }
                else if (info.StatusID == 6)
                {
                    findDetail.DeliveryTo = "";
                    findDetail.DeliveryDate = deliverDate;
                    findDetail.DeliveryNotes = info.Note;

                    mailerInfo.DeliveryTo = "";
                    mailerInfo.DeliveryDate = deliverDate;
                    mailerInfo.DeliveryNotes = info.Note;

                    HandleHistory.AddTracking(6, info.MailerID, mailerInfo.CurrentPostOfficeID, "Chưa phát được vì " + info.Note);

                }
                else if (info.StatusID == 4)
                {
                    findDetail.DeliveryTo = info.Reciever;
                    findDetail.ReturnReasonID = null;
                    findDetail.DeliveryNotes = "Đã phát";
                    findDetail.DeliveryDate = deliverDate;

                    mailerInfo.DeliveryTo = info.Reciever;
                    mailerInfo.DeliveryDate = deliverDate;
                    mailerInfo.DeliveryNotes = "Đã phát";

                    HandleHistory.AddTracking(4, info.MailerID, mailerInfo.CurrentPostOfficeID, "Ngày phát " + deliverDate.ToString("dd/MM/yyyy") + " lúc " + deliverDate.ToString("HH:mm") + ", người nhận: " + info.Reciever);

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
                            DocumentID = info.DocumentID,
                            EmployeeID = checkUser.EmployeeID,
                            MailerID = mailerInfo.MailerID
                        };

                        db.EmpployeeDebitCODs.Add(saveCoDDebit);
                    }
                }

                if(info.images != null)
                {
                    foreach(var image in info.images)
                    {
                        var saveImage = new MailerImage()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreateTime = DateTime.Now,
                            MailerID = info.MailerID,
                            PathImage = image,
                            UserSend = user
                        };

                        db.MailerImages.Add(saveImage);
                    }
                    db.SaveChanges();
                }

                db.Entry(mailerInfo).State = System.Data.Entity.EntityState.Modified;
                db.Entry(findDetail).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                UpdateDeliveryStatus(document.DocumentID);

            }
            catch (Exception e)
            {
                result.msg = e.Message;
                result.error = 1;
            }

            return result;

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


        // lay hang
        public ResultInfo GetTakeMailer(string user, string date, int statusId)
        {
            var checkUser = db.BS_Employees.Where(p => p.UserLogin == user).FirstOrDefault();

            if (checkUser == null)
            {
                return new ResultInfo()
                {
                    error = 1,
                    msg = "Sai thông tin"
                };

            }
       


            var data = db.TAKEMAILER_GETLIST_BY_EMPLOYEE(checkUser.EmployeeID, statusId, date).ToList();

            return new ResponseInfo()
            {
                error = 0,
                msg = "",
                data = data
            };

        }

        public ResultInfo GetTakeMailerDetail(string documentID)
        {
            var data = db.TAKEMAILER_GETDETAILs(documentID).ToList();

            return new ResponseInfo()
            {
                error = 0,
                msg = "",
                data = data
            };

        }

        public ResultInfo UpdateTakeMailer(string user, UpdateTakeMailerReceive info)
        {
            var result = new ResultInfo()
            {
                error = 0,
                msg = "success"
            };
            var checkUser = db.BS_Employees.Where(p => p.UserLogin == user).FirstOrDefault();

            if (checkUser == null)
            {
                return new ResultInfo()
                {
                    error = 1,
                    msg = "Sai thông tin"
                };

            }
            try
            {
                var checkDocument = db.MM_TakeMailers.Find(info.documentId);

                if (checkDocument == null)
                    throw new Exception("Sai thông tin");

                foreach (var item in info.mailers)
                {

                    var checkMailer = db.MM_Mailers.Find(item);

                    if (checkMailer == null)
                        continue;

                    var findDetail = db.MM_TakeDetails.Where(p => p.DocumentID == checkDocument.DocumentID && p.MailerID == item).FirstOrDefault();

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

                    HandleHistory.AddTracking(8, item, checkMailer.CurrentPostOfficeID, "Đã lấy hàng, đang giao về kho");

                }

                var checkCount = db.TAKEMAILER_GETDETAILs(checkDocument.DocumentID).Where(p=> p.CurrentStatusID == 7).ToList();

                if (checkCount.Count() == 0)
                {
                    checkDocument.StatusID = 8;
                    db.Entry(checkDocument).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                result.msg = e.Message;
                result.error = 1;
            }

            return result;

        }
    }
}