using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.beans
{
    public class CourseBean
    {
        public string course_id { get; set; }
        public string course_name { get; set; }
        public string duration_months { get; set; }
        public string is_active { get; set; }
        public string description { get; set; }
        public string course_map_id { get; set; }
        public string course_type { get; set; }
        public string message { get; set; }
        public bool is_opr_success { get; set; }
        public string parent_course_id { get; set; }
        public string fees { get; set; }
        public string royalty { get; set; }
        public string theory_hrs { get; set; }
        public string practical_hrs { get; set; }
        public string code { get; set; }
    }
}
