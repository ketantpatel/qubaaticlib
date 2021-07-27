using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class AnswerSheetBean
    {
        public string question_json { get; set; }
        public string ans_id { get; set; }
        public string answer { get; set; }
        public string correct_answer { get; set; }
        public string form_id { get; set; }
        public string stud_id { get; set; }
        public string create_date { get; set; }
        public string modify_date { get; set; }
        public string is_checked { get; set; }
        public string is_correct { get; set; }
        public string marks { get; set; }
        public string question_id { get; set; }
        public string exam_id { get; set; }
        public string submit_date { get; set; }
        public string user_id { get; set; }
    }
}
