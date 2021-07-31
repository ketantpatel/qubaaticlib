using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class TeacherBean
    {
        public string full_name { get; set; }
        public string teach_id { get; set; }
        public string frn_code { get; set; }
        public string frn_name { get; set; }
        public string frn_share { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string profile_desc { get; set; }
        public string is_left { get; set; }
        public string left_date { get; set; }
        public string is_active { get; set; }
        public string create_date { get; set; }
        public string modify_date { get; set; }
        public string user_id { get; set; }
        public string state_id { get; set; }
        public string country_id { get; set; }

        public bool is_success { get; set; }
        public int? last_level_trained { get; set; }
        public string last_level_trained_date { get; set; }
        public int? partipation_level { get; set; }
        public string partipation_level_Date { get; set; }
        public string code { get; set; }
        public string birthdate { get; set; }
        public string education_qualification { get; set; }
        public string teaching_experience { get; set; }
        public string residential_address { get; set; }
        public string location { get; set; }
        public string city { get; set; }
        public int? level_id { get; set; }
        public string class_taken { get; set; }
        public int? refresh_training_level { get; set; }
        public int? frn_id { get; set; }
        public bool IsExistedInTraining { get; set; }
        public string frn_master_code { get; set; }
        public string frn_master_name { get; set; }
        public string last_level_name { get; set; }
        public string previous_level_name { get; set; }
    }
}
