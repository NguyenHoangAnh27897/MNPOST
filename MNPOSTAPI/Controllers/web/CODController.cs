using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MNPOSTAPI.Models;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
namespace MNPOSTAPI.Controllers.web
{
    public class CODController : WebBaseController
    {
        // GET: COD
        //lay ra danh sách cac bang ke cod da chi tra cho khách hàng
        [HttpGet]
        public CODInfoResult GetCODDebitvoucher( string customerid,string fromdate,string todate)
        {
            var cusid = new SqlParameter("@customerid", customerid);
            var fdate = new SqlParameter("@fromdate", fromdate);
            var tdate = new SqlParameter("@todate", todate);

            try
            {
                CODInfoResult result = new CODInfoResult()
                {
                    error = 0,
                    msg = "400-OK",
                    codinfo = db.Database.SqlQuery<CODEntity>("AC_CODDEBITVOUCHER_BYCUSTOMERID @customerid, @fromdate,@todate", cusid, fdate,tdate).ToList()
            };
                return result;
            }
            catch(Exception ex)
            {
                CODInfoResult result = new CODInfoResult()
                {
                    error = 1,
                    msg = "Khong ket noi he thong"
                };
                return result;
            }

        }
        [HttpGet]
        public CODDetailInfoResult GetCODDebitvoucherDetail(string documentid)
        {
            //string documentid ="BCQ30001";
            var docid = new SqlParameter("@documentid", documentid);

            try
            {
                CODDetailInfoResult result = new CODDetailInfoResult()
                {
                    error = 0,
                    msg = "400-OK",
                    coddetailinfo = db.Database.SqlQuery<CODDetail>("AC_CODDEBITVOUCHERDETAILS_BYDOCUMENTID @documentid", docid).ToList()
                };
                return result;
            }
            catch (Exception ex)
            {
                CODDetailInfoResult result = new CODDetailInfoResult()
                {
                    error = 1,
                    msg = "Khong ket noi he thong"
                };
                return result;
            }

        }
        [HttpGet]
        public CODTotalInfoResult GetCODTotal(string customerid)
        {
            //string documentid ="BCQ30001";
            var cusid = new SqlParameter("@customerid", customerid);

            try
            {
                CODTotalInfoResult result = new CODTotalInfoResult()
                {
                    error = 0,
                    msg = "400-OK",
                    codtotalinfo = db.Database.SqlQuery<CODTotal>("CODCUSTOMER_TRANSFER @customerid", cusid).ToList()
                };
                return result;
            }
            catch (Exception ex)
            {
                CODTotalInfoResult result = new CODTotalInfoResult()
                {
                    error = 1,
                    msg = "Khong ket noi he thong"
                };
                return result;
            }

        }

        //tinh cuoc
        [HttpGet]
        public CalPriceResult CalculatePrice(double Weight, string CustomerID, string ProvinceID, string ServiceTypeID, string PostOfficeID, string Ngay)
        {
            var weight = new SqlParameter("@Weight", Weight);
            var cusid = new SqlParameter("@CustomerID", CustomerID);
            var proid = new SqlParameter("@ProvinceID", ProvinceID);
            var serviceid = new SqlParameter("@ServiceTypeID", ServiceTypeID);
            var postid = new SqlParameter("@PostOfficeID", PostOfficeID);
            var ngay = new SqlParameter("@Ngay", Ngay);

            try
            {
                CalPriceResult result = new CalPriceResult()
                {
                    error = 0,
                    msg = "400-OK",
                    calprice = db.Database.SqlQuery<CalPrice>("CalPrice @Weight,@CustomerID,@ProvinceID,@ServiceTypeID,@PostOfficeID,@Ngay", weight, cusid, proid, serviceid, postid, ngay).FirstOrDefault()
                };
                return result;
            }
            catch (Exception ex)
            {
                CalPriceResult result = new CalPriceResult()
                {
                    error = 1,
                    msg = "Khong ket noi he thong"
                };
                return result;
            }
        }
    }
}