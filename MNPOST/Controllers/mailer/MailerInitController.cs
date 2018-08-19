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
            var allCus = db.BS_Customers.ToList();

            List<CustomerInfoResult> cusResult = new List<CustomerInfoResult>();

            foreach (var item in allCus)
            {
                cusResult.Add(new CustomerInfoResult()
                {
                    code = item.CustomerCode,
                    name = item.CustomerName,
                    phone = item.Phone,
                    provinceId = item.ProvinceID,
                    address = item.Address,
                    districtId = item.DistrictID,
                    wardId = item.WardID
                });
            }

            ViewBag.Customers = cusResult;

            //
            List<CommonData> allMailerType = db.BS_ServiceTypes.Select(p => new CommonData()
            {
                code = p.ServiceID,
                name = p.ServiceName
            }).ToList();

            ViewBag.MailerTypes = allMailerType;

            //
            List<CommonData> allPayment = new List<CommonData>();

            allPayment.Add(new CommonData()
            {
                code = "NGTT",
                name = "Người gửi thanh toán"
            });

            allPayment.Add(new CommonData()
            {
                code = "NNTT",
                name = "Người nhận thanh toán"
            });

            ViewBag.Payments = allPayment;


            //
            List<ItemPriceCommon> allServices = db.BS_Services.Select(p => new ItemPriceCommon()
            {
                code = p.ServiceID,
                name = p.ServiceName,
                price = p.Price,
                choose = false

            }).ToList();

            ViewBag.Services = allServices;

            //
            ViewBag.PostOffices = EmployeeInfo.postOffices;

            // danh sach tinh thanh
            List<CommonData> allProvince = GetProvinceDatas("", "province");
            ViewBag.Provinces = allProvince;


            return View();
        }


        #region
        [HttpPost]
        public ActionResult InsertByExcel(HttpPostedFileBase files, string name)
        {
            List<MailerIdentity> mailers = new List<MailerIdentity>();
            var result = new ResultInfo()
            {
                error = 0,
                msg = "Đã tải",
                data = mailers
            };
           
            try
            {
                if (files == null || files.ContentLength <= 0)
                    throw new Exception("Thiếu file Excel");

                string extension = System.IO.Path.GetExtension(files.FileName);

                if (extension.Equals(".xlsx") || extension.Equals(".xls"))
                {
                    string fileSave = "mailersupload" + DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
                    string path = Server.MapPath("~/Temps/" + fileSave);
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
                    int servicePriceIdx = -1;

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
                                    servicePriceIdx = i + 1;
                                    break;
                            }

                        }
                    }

                    // check cac gia tri can
                    if (receiverIdx == -1 || receiAddressIdx == -1 || receiPhoneIdx == -1 || receiDistrictIdx == -1 || receiProvinceIdx == -1 || mailerTypeIdx == 1 ||
                        merchandiseIdx == -1 || weigthIdx == -1)
                        throw new Exception("Thiếu các cột cần thiết");

                    //
                    
                    for (int i = 0; i < totalRows; i++)
                    {
                        string mailerId = mailerCodeIdx == -1 ? GeneralMailerCode("") : Convert.ToString(sheet.Cells[i + 2, mailerCodeIdx].Value);

                        //
                        string receiver = Convert.ToString(sheet.Cells[i + 2, receiverIdx].Value);
                        if (String.IsNullOrEmpty(receiver))
                            throw new Exception("Dòng " + (i + 2) + " cột " + receiverIdx + " : thiếu thông tin");
                        //
                        string receiverAddress = Convert.ToString(sheet.Cells[i + 2, receiAddressIdx].Value);
                        if (String.IsNullOrEmpty(receiverAddress))
                            throw new Exception("Dòng " + (i + 2) + " cột " + receiAddressIdx + " : thiếu thông tin");
                        // 
                        string receiverProvince = Convert.ToString(sheet.Cells[i + 2, receiProvinceIdx].Value);
                        var checkProvince = db.BS_Provinces.Find(receiverProvince);
                        if(checkProvince == null)
                            throw new Exception("Dòng " + (i + 2) + " cột " + receiProvinceIdx + " : sai thông tin");

                        //
                        string receiverDistrict = Convert.ToString(sheet.Cells[i + 2, receiDistrictIdx].Value);
                        var checkDistrict = db.BS_Districts.Find(receiverDistrict);
                        if (checkDistrict == null)
                            throw new Exception("Dòng " + (i + 2) + " cột " + receiDistrictIdx + " : sai thông tin");

                        //
                        string receiverWard = Convert.ToString(sheet.Cells[i + 2, receiWardIdx].Value);
                        var checkWard = db.BS_Wards.Find(receiverWard);
                        if (checkWard == null)
                            throw new Exception("Dòng " + (i + 2) + " cột " + receiWardIdx + " : sai thông tin");

                        //
                        string mailerType = Convert.ToString(sheet.Cells[i + 2, mailerTypeIdx].Value);
                        var checkMailerType = db.BS_ServiceTypes.Find(mailerType);
                        if (checkMailerType == null)
                            throw new Exception("Dòng " + (i + 2) + " cột " + mailerTypeIdx + " : sai thông tin");

                    }


                }


            }
            catch (Exception e)
            {
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
                    CurrentStatusID = 1,
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
        public ActionResult GeneralCode(string cusId)
        {
            var code = GeneralMailerCode(cusId);

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