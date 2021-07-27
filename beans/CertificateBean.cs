using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class CertificateBean
    {
        
        public string student_name{get;set;}
        public string reg_no { get; set; }
        public string course_name { get; set; }
      
        public string period_from { get; set; }
        public string period_to { get; set; }
        public string exam_date { get; set; }
        
        public string grade { get; set; }
        public string form_id { get; set; }
        public string gaurdian { get; set; }
        public string fran_city { get; set; }
        public string photoPath { get; set; }

        public string percentage { get; set; }
        public string serial_no { get; set; }
        public string course_id { get; set; }
        public string parent_course_id { get; set; }


    }
}
