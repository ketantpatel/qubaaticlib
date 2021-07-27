using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class SubjectBean  
    {
        public string subject_id { get; set; }
        public string name { get; set; }
        public string course_id { get; set; }
        public string create_date { get; set; } 
        public string message { get; set; }
        public bool is_opr_success { get; set; }
        public bool is_active { get; set; }
        public string practical_marks { get; set; }
        public string theory_marks { get; set; }
        public string code { get; set; }
    }
}
