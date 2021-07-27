using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class ResultBean
    {
        public string full_name { get; set; }
        public string enroll_no { get; set; }
        public string start_time { get; set; }
        public string expire_time { get; set; }
        public string terminate_time { get; set; }
        public string total_questions { get; set; }
        public string total_attempted { get; set; }
        public string form_id { get; set; }
        public string total_correct { get; set; }
        public string score { get; set; }
        public string weighatge { get; set; }
    }
}
