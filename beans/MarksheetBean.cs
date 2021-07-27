using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class MarksheetBean
    {
        public string id_no{get;set;}
        public string student_name{get;set;}
        public string reg_no { get; set; }
        public string course_name { get; set; }
        public string course_code { get; set; }
        public string atc_name { get; set; }
        public string atc_code { get; set; }
        public string training_period { get; set; }
        public string exam_date { get; set; }
        public string marksheet_date { get; set; }
        public string percentage { get; set; }
        public string grade { get; set; }
        public string form_id { get; set; }
        public string total_in_words { get; set; }
        public string course_id { get; set; }
        public string month { get; set; }
        public string total_marks { get; set; }
        public string parent_course_id { get; set; }
        public string obtain_marks { get; set; }
        public string year { get; set; }
        
    }
}
