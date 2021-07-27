using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class GradeSheetBean
    {
        public string form_id { get; set; }
        public string course_id{get;set;}
        public string marks_total{get;set;}
        public string marks_obtain{get;set;}
        public string create_date { get; set; }
        public string modify_date { get; set; }
        public string name { get; set; }
        public string enroll_no { get; set; }
        public string grade{get;set;}
        public string parent_course_id { get; set; }
        public string message { get; set; }
        public bool is_opr_success { get; set; }
        public string form_date { get; set; }
        public string percentage { get; set; }
    }
}
