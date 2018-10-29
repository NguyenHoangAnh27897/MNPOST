﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MNPOSTCOMMON;
using MNPOSTWEBSITEMODEL;

namespace MNPOSTAPI.Models
{
   
    public class CustomerInfoResult : ResultInfo
    {
        public BS_Customers customer { get; set; }
        
    }


    public class AddCustomerRequest : RequestInfo
    {
        public BS_Customers customer { get; set; }
    }

    public class AddCustomerFromWebsiteRequest : RequestInfo
    {
        public MNPOSTWEBSITEMODEL.AspNetUser customer { get; set; }
    }

}