using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class ExamMarkRowBean
    {
        public string inquiry_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string course_name { get; set; }
        public string middle_name { get; set; }
        public string enroll_no { get; set; }
        public string form_id { get; set; }
        public string form_date { get; set; }
        public string marks { get; set; }
        public string theory_marks { get; set; }
        public string practical_marks { get; set; }
    }
}
