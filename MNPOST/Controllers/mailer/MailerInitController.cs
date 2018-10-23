using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MNPOST.Models;
using System.Web.Mvc;
using MNPOSTCOMMON;
using System.IO;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace MNPOST.Controllers.mailer
{
    public class MailerInitController : MailerController
    {



        [HttpGet]
        public ActionResult Init()
        {

            ViewBag.Customers = db.BS_Customers.Where(p=> p.IsActive == true).Select(item => new
            {
                code = item.CustomerCode,
                name = item.CustomerName,
                phone = item.Mobile,
                provinceId = item.ProvinceID,
                address = item.Address,
                districtId = item.DistrictID,
                wardId = item.WardID
            }).ToList();

            // dịch vu

            ViewBag.MailerTypes = db.BS_ServiceTypes.Select(p => new CommonData()
            {
                code = p.ServiceID,
                name = p.ServiceName
            }).ToList();

            // hinh thuc thanh toan
            ViewBag.Payments = db.CDatas.Where(p => p.CType == "MAILERPAY").Select(p => new CommonData() { code = p.Code, name = p.Name }).ToList();


            // danh sach phu phi
            ViewBag.Services = db.BS_Services.Select(p => new ItemPriceCommon()
            {
                code = p.ServiceID,
                name = p.ServiceName,
                price = p.Price,
                choose = false

            }).ToList(); ;

            // buu cuc
            ViewBag.PostOffices = EmployeeInfo.postOffices;

            // tinh thanh
            ViewBag.Provinces = GetProvinceDatas("", "province");


            return View();
        }



        #region
        [HttpPost]
        public ActionResult InsertByExcel(HttpPostedFileBase files, string senderID, string senderAddress, string senderName, string senderPhone, string senderProvince, string senderDistrict, string senderWard, string postId)
        {
            List<MailerIdentity> mailers = new List<MailerIdentity>();
            var result = new ResultInfo()
            {
                error = 0,
                msg = "Đã tải",
                data = mailers
            };
            string path = "";
            try
            {

                // check sender
                var checkSender = db.BS_Customers.Where(p => p.CustomerCode == senderID).FirstOrDefault();

                if (checkSender == null)
                    throw new Exception("Sai thông tin người gửi");

                var checkSendProvince = db.BS_Provinces.Find(senderProvince);

                if (checkSendProvince == null)
                    throw new Exception("Sai thông tin tỉnh thành");

                var checkSendDistrict = db.BS_Districts.Find(senderDistrict);
                if (checkSendDistrict == null)
                    throw new Exception("Sai thông tin quận huyện ");

                var checkSendWard = db.BS_Wards.Find(senderWard);
                if (checkSendWard == null)
                    throw new Exception("Sai thông tin phường xã ");

                if (files == null || files.ContentLength <= 0)
                    throw new Exception("Thiếu file Excel");

                string extension = System.IO.Path.GetExtension(files.FileName);

                if (extension.Equals(".xlsx") || extension.Equals(".xls"))
                {
                    string fileSave = "mailersupload" + DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
                    path = Server.MapPath("~/Temps/" + fileSave);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    files.SaveAs(path);
                    FileInfo newFile = new FileInfo(path);
                    var package = new ExcelPackage(newFile);

                    ExcelWorksheet sheet = package.Workbook.Worksheets[1];

                    int totalRows = sheet.Dimension.End.Row;
                    int totalCols = sheet.Dimension.End.Column;

                    // 
                    int mailerCodeIdx = -1;
                    int receiverIdx = -1;
                    int receiPhoneIdx = -1;
                    int receiAddressIdx = -1;
                    int receiProvinceIdx = -1;
                    int receiDistrictIdx = -1;
                    int receiWardIdx = -1;
                    int mailerTypeIdx = -1;
                    int payTypeIdx = -1;
                    int codIdx = -1;
                    int merchandiseIdx = -1;
                    int weigthIdx = -1;
                    int quantityIdx = -1;
                    int lengthIdx = -1;
                    int widthIdx = -1;
                    int heightIdx = -1;
                    int notesIdx = -1;
                    int desIdx = -1;
                    int otherPriceIdx = -1;

                    // lay index col tren excel
                    for (int i = 0; i < totalCols; i++)
                    {
                        var colValue = Convert.ToString(sheet.Cells[1, i + 1].Value).Trim();

                        Regex regex = new Regex(@"\((.*?)\)");
                        Match match = regex.Match(colValue);

                        if (match.Success)
                        {
                            string key = match.Groups[1].Value;

                            switch (key)
                            {
                                case "1":
                                    mailerCodeIdx = i + 1;
                                    break;
                                case "2":
                                    receiverIdx = i + 1;
                                    break;
                                case "3":
                                    receiPhoneIdx = i + 1;
                                    break;
                                case "4":
                                    receiAddressIdx = i + 1;
                                    break;
                                case "5":
                                    receiProvinceIdx = i + 1;
                                    break;
                                case "6":
                                    receiDistrictIdx = i + 1;
                                    break;
                                case "7":
                                    receiWardIdx = i + 1;
                                    break;
                                case "8":
                                    mailerTypeIdx = i + 1;
                                    break;
                                case "9":
                                    payTypeIdx = i + 1;
                                    break;
                                case "10":
                                    codIdx = i + 1;
                                    break;
                                case "11":
                                    merchandiseIdx = i + 1;
                                    break;
                                case "12":
                                    weigthIdx = i + 1;
                                    break;
                                case "13":
                                    quantityIdx = i + 1;
                                    break;
                                case "14":
                                    lengthIdx = i + 1;
                                    break;
                                case "15":
                                    widthIdx = i + 1;
                                    break;
                                case "16":
                                    heightIdx = i + 1;
                                    break;
                                case "17":
                                    notesIdx = i + 1;
                                    break;
                                case "18":
                                    desIdx = i + 1;
                                    break;
                                case "19":
                                    otherPriceIdx = i + 1;
                                    break;
                            }

                        }
                    }

                    // check cac gia tri can
                    if (receiverIdx == -1 || receiAddressIdx == -1 || receiPhoneIdx == -1 || receiDistrictIdx == -1 || receiProvinceIdx == -1 || mailerTypeIdx == 1 ||
                        merchandiseIdx == -1 || weigthIdx == -1)
                        throw new Exception("Thiếu các cột cần thiết");

                    //

                    for (int i = 2; i <= totalRows; i++)
                    {
                        string mailerId = mailerCodeIdx == -1 ? GeneralMailerCode("") : Convert.ToString(sheet.Cells[i, mailerCodeIdx].Value);

                        //
                        string receiverPhone = Convert.ToString(sheet.Cells[i, receiPhoneIdx].Value);
                        if (String.IsNullOrEmpty(receiverPhone))
                            throw new Exception("Dòng " + (i) + " cột " + receiPhoneIdx + " : thiếu thông tin");

                        //
                        string receiver = Convert.ToString(sheet.Cells[i, receiverIdx].Value);
                        if (String.IsNullOrEmpty(receiver))
                            throw new Exception("Dòng " + (i) + " cột " + receiverIdx + " : thiếu thông tin");
                        //
                        string receiverAddress = Convert.ToString(sheet.Cells[i, receiAddressIdx].Value);
                        if (String.IsNullOrEmpty(receiverAddress))
                            throw new Exception("Dòng " + (i) + " cột " + receiAddressIdx + " : thiếu thông tin");
                        // 
                        string receiverProvince = Convert.ToString(sheet.Cells[i, receiProvinceIdx].Value);
                        var checkProvince = db.BS_Provinces.Find(receiverProvince);
                        if (checkProvince == null)
                            throw new Exception("Dòng " + (i) + " cột " + receiProvinceIdx + " : sai thông tin");

                        //
                        string receiverDistrict = Convert.ToString(sheet.Cells[i, receiDistrictIdx].Value);
                        var checkDistrict = db.BS_Districts.Find(receiverDistrict);
                        if (checkDistrict == null)
                            throw new Exception("Dòng " + (i) + " cột " + receiDistrictIdx + " : sai thông tin");

                        //
                        string receiverWard = receiWardIdx == -1 ? "" : Convert.ToString(sheet.Cells[i, receiWardIdx].Value);
                        if (receiWardIdx != -1)
                        {
                            var checkWard = db.BS_Wards.Find(receiverWard);
                            receiverWard = checkWard == null ? "" : receiverWard;
                        }

                        //
                        string mailerType = Convert.ToString(sheet.Cells[i, mailerTypeIdx].Value);
                        var checkMailerType = db.BS_ServiceTypes.Find(mailerType);
                        if (checkMailerType == null)
                            throw new Exception("Dòng " + (i) + " cột " + mailerTypeIdx + " : sai thông tin");

                        //
                        var mailerPay = payTypeIdx == -1 ? "NGTT" : Convert.ToString(sheet.Cells[i, payTypeIdx].Value);
                        if (payTypeIdx != -1)
                        {
                            var checkMailerPay = db.CDatas.Where(p => p.Code == mailerPay && p.CType == "MAILERPAY").FirstOrDefault();
                            mailerPay = checkMailerPay == null ? "NGTT" : checkMailerPay.Code;
                        }

                        // COD
                        var codValue = sheet.Cells[i, codIdx].Value;
                        decimal cod = 0;
                        if (codValue != null)
                        {
                            var isCodeNumber = codIdx == -1 ? false : Regex.IsMatch(codValue.ToString(), @"^\d+$");
                            cod = isCodeNumber ? Convert.ToDecimal(codValue) : 0;
                        }

                        // hang hoa
                        var merchandisType = Convert.ToString(sheet.Cells[i, merchandiseIdx].Value);
                        var checkMerchandisType = db.CDatas.Where(p => p.Code == merchandisType && p.CType == "GOODTYPE").FirstOrDefault();
                        if (checkMerchandisType == null)
                            throw new Exception("Dòng " + (i) + " cột " + merchandiseIdx + " : sai thông tin");

                        // trong luong
                        var weightValue = sheet.Cells[i, weigthIdx].Value;
                        double weight = 0;
                        if (weightValue == null)
                            throw new Exception("Dòng " + (i) + " cột " + weigthIdx + " : sai thông tin");
                        else
                        {
                            var isWeightNumber = Regex.IsMatch(weightValue.ToString(), @"^\d+$");
                            weight = isWeightNumber ? Convert.ToDouble(sheet.Cells[i, weigthIdx].Value) : 0;
                        }

                        // so luong
                        var quantityValue = sheet.Cells[i, quantityIdx].Value;
                        var isQuantityNumber = quantityIdx == -1 ? false : Regex.IsMatch(quantityValue == null?"0":quantityValue.ToString(), @"^\d+$");
                        var quantity = isQuantityNumber ? Convert.ToInt32(quantityValue) : 0;


                        // dai
                        var lengthValue = sheet.Cells[i, lengthIdx].Value;
                        var isLengthNumber = lengthIdx == -1 ? false : Regex.IsMatch(lengthValue == null?"0":lengthValue.ToString(), @"^\d+$");
                        var length = isLengthNumber ? Convert.ToDouble(lengthValue) : 0;

                        // rong
                        var widthValue = sheet.Cells[i, widthIdx].Value;
                        var isWidthNumber = widthIdx == -1 ? false : Regex.IsMatch(widthValue == null?"0":widthValue.ToString(), @"^\d+$");
                        var width = isWidthNumber ? Convert.ToDouble(widthValue) : 0;

                        //cao
                        var heightValue = sheet.Cells[i, heightIdx].Value;
                        var isHeightNumber = heightIdx == -1 ? false : Regex.IsMatch(heightValue == null? "0":heightValue.ToString(), @"^\d+$");
                        var height = isHeightNumber ? Convert.ToDouble(heightValue) : 0;

                        //
                        string notes = notesIdx == -1 ? "" : Convert.ToString(sheet.Cells[i, notesIdx].Value);

                        //
                        string describe = desIdx == -1 ? "" : Convert.ToString(sheet.Cells[i, desIdx].Value);

                        // phu phi
                        var otherPriceValue = sheet.Cells[i, otherPriceIdx].Value;
                        var isOtherPirce = otherPriceIdx == -1 ? false : Regex.IsMatch(otherPriceValue==null?"0":otherPriceValue.ToString(), @"^\d+$");
                        var otherPrice = isHeightNumber ? Convert.ToDecimal(otherPriceValue) : 0;

                        
                        var price = db.CalPrice(weight, senderID, senderProvince,mailerType, postId, DateTime.Now.ToString("yyyy-MM-dd")).FirstOrDefault();
                        var codPrice = 0;

                        otherPrice += codPrice;

                        mailers.Add(new MailerIdentity()
                        {
                            MailerID = mailerId,
                            COD = cod,
                            CODPrice = codPrice,
                            HeightSize = height,
                            LengthSize = length,
                            MailerDescription = describe,
                            MailerTypeID = mailerType,
                            MerchandiseID = merchandisType,
                            MerchandiseValue = cod,
                            Notes = notes,
                            PaymentMethodID = mailerPay,
                            PriceDefault = price,
                            PriceMain = price,
                            PriceService = otherPrice,
                            Quantity = quantity,
                            RecieverAddress = receiverAddress,
                            RecieverDistrictID = receiverDistrict,
                            RecieverName = receiver,
                            RecieverPhone = receiverPhone,
                            RecieverProvinceID = receiverProvince,
                            RecieverWardID = receiverWard,
                            Weight = weight,
                            WidthSize = width,
                            SenderID = senderID,
                            SenderAddress = senderAddress,
                            SenderDistrictID = senderDistrict,
                            SenderName = senderName,
                            SenderPhone = senderPhone,
                            SenderProvinceID = senderProvince,
                            SenderWardID = senderWard
                        });

                    }

                    // xoa file temp
                    package.Dispose();
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                }

                result.data = mailers;
            }
            catch (Exception e)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                result.error = 1;
                result.msg = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion



        [HttpPost]
        public ActionResult InsertMailers(List<MailerIdentity> mailers, string postId)
        {

            if (mailers == null)
                return Json(new { error = 1, msg = "Hoàn thành" }, JsonRequestBehavior.AllowGet);

            if (mailers.Count() > 100)
                return Json(new { error = 1, msg = "Để đảm bảo hệ thống chỉ update 100/1 lần" }, JsonRequestBehavior.AllowGet);

            var checkPost = db.BS_PostOffices.Find(postId);

            if (checkPost == null)
                return Json(new { error = 1, msg = "chọn bưu cục" }, JsonRequestBehavior.AllowGet);

            List<MailerIdentity> insertFail = new List<MailerIdentity>();

            foreach (var item in mailers)
            {
                // checkMailer
                if (String.IsNullOrEmpty(item.MailerID))
                {
                    insertFail.Add(item);
                    continue;
                }

                var checkExist = db.MM_Mailers.Where(p => p.MailerID == item.MailerID).FirstOrDefault();

                if (checkExist != null)
                {
                    insertFail.Add(item);
                    continue;
                }

                // theem
                var mailerIns = new MM_Mailers()
                {
                    MailerID = item.MailerID,
                    AcceptTime = DateTime.Now,
                    AcceptDate = DateTime.Now,
                    COD = item.COD,
                    CreationDate = DateTime.Now,
                    CurrentStatusID = 0,
                    HeightSize = item.HeightSize,
                    Weight = item.Weight,
                    LengthSize = item.LengthSize,
                    WidthSize = item.WidthSize,
                    Quantity = item.Quantity,
                    PostOfficeAcceptID = postId,
                    CurrentPostOfficeID = postId,
                    EmployeeAcceptID = EmployeeInfo.employeeId,
                    MailerDescription = item.MailerDescription,
                    MailerTypeID = item.MailerTypeID,
                    MerchandiseValue = item.MerchandiseValue,
                    MerchandiseID = item.MerchandiseID,
                    PriceDefault = item.PriceDefault,
                    Price = item.PriceDefault,
                    PriceService = item.PriceService,
                    Notes = item.Notes,
                    PaymentMethodID = item.PaymentMethodID,
                    RecieverAddress = item.RecieverAddress,
                    RecieverName = item.RecieverName,
                    RecieverPhone = item.RecieverPhone,
                    RecieverDistrictID = item.RecieverDistrictID,
                    RecieverWardID = item.RecieverWardID,
                    RecieverProvinceID = item.RecieverProvinceID,
                    SenderID = item.SenderID,
                    SenderAddress = item.SenderAddress,
                    SenderDistrictID = item.SenderDistrictID,
                    SenderName = item.SenderName,
                    SenderPhone = item.SenderPhone,
                    SenderProvinceID = item.SenderProvinceID,
                    SenderWardID = item.SenderWardID
                };

                // 
                db.MM_Mailers.Add(mailerIns);
                db.SaveChanges();

                // luu tracking
                HandleHistory.AddTracking(0, item.MailerID, postId, "Nhận thông tin đơn hàng");
            }


            return Json(new { error = 0, data = insertFail }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetProvinces(string parentId, string type)
        {
            return Json(GetProvinceDatas(parentId, type), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDistrictAndWard(string provinceId, string districtId)
        {
            return Json(new { districts = GetProvinceDatas(provinceId, "district"), wards = GetProvinceDatas(districtId, "ward") }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GeneralCode(string postId)
        {
            var code = GeneralMailerCode(postId);

            return Json(new { error = 0, code = code }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProvinceFromAddress(string province, string district, string ward)
        {
            var findProvince = db.BS_Provinces.Where(p => p.ProvinceName == province).FirstOrDefault();

            if (findProvince == null)
                return Json(new { provinceId = "", districtId = "", wardId = "" }, JsonRequestBehavior.AllowGet);


            var findDistrict = db.BS_Districts.Where(p => p.ProvinceID == findProvince.ProvinceID && p.DistrictName == district).FirstOrDefault();

            if (findDistrict == null)
                return Json(new { provinceId = findProvince.ProvinceID, districtId = "", wardId = "" }, JsonRequestBehavior.AllowGet);


            var findWard = db.BS_Wards.Where(p => p.DistrictID == findDistrict.DistrictID && p.WardName == ward).FirstOrDefault();

            if (findWard == null)
                return Json(new { provinceId = findProvince.ProvinceID, districtId = findDistrict.DistrictID, wardId = "" }, JsonRequestBehavior.AllowGet);


            return Json(new { provinceId = findProvince.ProvinceID, districtId = findDistrict.DistrictID, wardId = findWard.WardID }, JsonRequestBehavior.AllowGet);
        }



    }
}