using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class MarketingActiviesBean
    {
        public string mk_id { get;set;}
        public string title { get;set;}
        public string create_date { get;set;}
        public string modify_date { get;set;}
        public string frn_id { get; set; }
        public string usr_id { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public bool isSuccess { get; set; }
      
    }
}
