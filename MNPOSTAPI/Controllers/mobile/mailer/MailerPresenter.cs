using System;
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
                    findDetail.ConfirmDate = null;

                   
                    mailerInfo.DeliveryTo = "";
                    mailerInfo.DeliveryDate = null;
                    mailerInfo.DeliveryNotes = findReason.ReasonName;
                }
                else if (info.StatusID == 6)
                {
                    findDetail.DeliveryTo = "";
                    findDetail.ConfirmDate = null;
                    findDetail.DeliveryNotes = info.Note;

                    mailerInfo.DeliveryTo = "";
                    mailerInfo.DeliveryDate = null;
                    mailerInfo.DeliveryNotes = info.Note;

                }
                else if (info.StatusID == 4)
                {
                    findDetail.DeliveryTo = info.Reciever;
                    findDetail.ReturnReasonID = null;
                    findDetail.DeliveryNotes = "Đã phát";
                    findDetail.ConfirmDate = deliverDate;

                    mailerInfo.DeliveryTo = info.Reciever;
                    mailerInfo.DeliveryDate = deliverDate;
                    mailerInfo.DeliveryNotes = "Đã phát";


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

    }
}