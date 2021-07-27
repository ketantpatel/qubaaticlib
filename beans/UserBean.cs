using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.beans
{
    public class UserBean
    {
        public string user_id { get; set; }
        public string username { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string password { get; set; }
        public string user_type { get; set; }
        public bool isSwitchedAccount { get; set; }
        public string role { get; set; }
        public string is_active { get; set; }
        public string create_date { get; set; }
        public string modify_date { get; set; }
        public string db_name { get; set; }
        public string db_pass { get; set; }
        public string parent_frnachise_id { get; set; }
        public string rool_franchise_id { get; set; }
        public string db_user { get; set; }
        public string role_name { get; set; }
        public string db_ip { get; set; }
        public bool is_valid_user { get; set; }
        public string message { get; set; }
        public bool is_opr_success { get; set; }
        public string parent_id { get; set; }
        public string franchise_id { get; set; }
        public string frn_code { get; set; }
        public string franchise_type { get; set; }
        public string zone_id { get; set; }

        public string state_id { get; set; }

        public string country_id { get; set; }
        public string teach_id { get; set; }
        public string user_type_name { get; set; }
    }
}
