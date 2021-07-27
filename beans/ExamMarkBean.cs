using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class ExamMarkBean
    {
        public string course_id { get; set; }
        public string parent_course_id { get; set; }
        public string subject_id { get; set; }
        public string marks { get; set; }
        public string stud_id { get; set; }
        public string create_date { get; set; }
        
        public string modify_date { get; set; }
        public string frn_id { get; set; }
        public string enroll_no { get; set; }
        public string exam_type { get; set; }

        public bool is_success { get; set; }
        public string course_code { get; set; }
    }
}
