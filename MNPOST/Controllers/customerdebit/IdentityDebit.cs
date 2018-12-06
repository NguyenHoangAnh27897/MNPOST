using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNPOST.Controllers.customerdebit
{
    public class IdentityDebit
    {
       public string  MailerID {get;set;}
       public decimal  Amount{get;set;}
       public decimal   Price{get;set;}
       public decimal  PriceService {get;set;}
       public double  Discount {get;set;}
       public double  DiscountPercent {get;set;}
       public double VATPercent {get;set;}
       public decimal BfVATAmount {get;set;}
       public decimal VATAmount {get;set;}
       public decimal AfVATAmount {get;set;}
       public DateTime AcceptDate {get;set;}
       public int Quantity {get;set;}
       public double Weight {get;set;}
    }
}