using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class ExamBean
    {
        public string exam_id{get;set;}
        public string from_time{get;set;}
        public string to_time{get;set;}
        public string exam_type{get;set;}
        public string weightage { get; set; }
        public string passing_marks { get; set; }
        public string title{get;set;}
        public string choice_type{get;set;}
        public string total_questions{get;set;}
        public string create_date{get;set;}
        public string exam_level { get; set; }
        public string modify_date { get; set; }
        public string hours { get; set; }
        public string form_id { get; set; }
        public string is_terminate { get; set; }
        public string exam_time { get; set; }
        public string end_time { get; set; }
        public List<QuestionBankBean> questions { get; set; }
        public bool is_expire { get; set; }
        public string status { get; set; }
        public string each_question_marks { get; set; }
    }
}
