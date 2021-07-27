using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class LessonDataBean
    {
        public string data_id { get; set; }
        public string lesson_id { get; set; }
        public string level_id { get; set; }
        public string pattern_id { get; set; }
        public string label_id { get; set; }
        public string data_value { get; set; }
        public string create_date { get; set; }
        public string modify_date { get; set; }
        public string teach_id { get; set; }
        public string batch_id { get; set; }
        public string stud_id { get; set; }

        public string sw_1_minute { get; set; }
        public string jd_called { get; set; }
        public string jd_scored { get; set; }
        public string fc_showed { get; set; }
        public string fc_scored { get; set; }
        public string fmp_total { get; set; }
        public string fmp_correct { get; set; }
        public string os_called { get; set; }
        public string os_correct { get; set; }
        public string vs_total { get; set; }
        public string vs_correct { get; set; }
        public bool is_success { get; set; }
        public string message { get; set; }
    }
}
