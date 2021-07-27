using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class CenterReportEntryBean
    {
        public string id { get; set; }
        public string batch_no { get; set; }
        public string day1 { get; set; }
        public string day1_time { get; set; }
        public string day2 { get; set; }
        public string day2_time { get; set; }
        public string totalStudents { get; set; }
        public string currentlevel { get; set; }
        public string start_date { get; set; }
        public string compeledlession { get; set; }
        public string teach_id { get; set; }
        public string create_date { get; set; }
        public string modify_date { get; set; }
        public string frn_id { get; set; }
        public string created_by { get; set; }
        public bool isSuccess { get; set; }
        public string teacher_name { get; set; }
        public string batch_name { get; set; }
        public string level_name { get; set; }


    }
}
