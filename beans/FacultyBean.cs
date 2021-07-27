using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class FacultyBean
    {
        public string user_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string user_type { get; set; }
        public string is_active { get; set; }
        public string create_date { get; set; }
        public string modify_date { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string parent_id { get; set; }
        public string role { get; set; }
        public string state_id { get; set; }
        public string country_id { get; set; }

        public string message { get; set; }
        public string is_success { get; set; }
    }
}
