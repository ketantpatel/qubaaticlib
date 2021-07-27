using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDACLib.beans
{
    public class MCQUserBean 
    {
        public string user_id { get; set; }
        public string username { get; set; }
        public string user_type { get; set; }
        public string password { get; set; }
        public string photo { get; set; }
        public string enq_id { get; set; }
        public string form_no { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string user_ip { get; set; }
        public DateTime user_date { get; set; }
        public string program_name { get; set; }
        public string send_email { get; set; }
        public string flag { get; set; }
        public string is_sms { get; set; }
        public string message { get; set; }
        public string group { get; set; }
        public string lickey { get; set; }
    }
}