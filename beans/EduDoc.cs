using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.beans
{
    public class EduDoc
    {
        public string doc_id { get; set; }
        public string stud_id { get; set; }
        public string course_id { get; set; }
        public string grade_marks { get; set; }
        public string passing_year { get; set; }
        public string school { get; set; }
        public string create_date { get; set; }
        public string modify_date { get; set; }
        public string created_by { get; set; }
        public string modify_by { get; set; }
        public string university { get; set; }
        public string file_name { get; set; }
        public string message { get; set; }
        public bool is_opr_success { get; set; }
        public string course_name { get; set; }
        public string status { get; set; }
        public string frn_id { get; set; }
    }
}
