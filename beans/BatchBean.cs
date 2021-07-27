using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class BatchBean
    {
        public string batch_id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string start_date { get; set; }
        public string frn_id { get; set; }
        public string db_start_date { get; set; }
        public string db_end_date { get; set; }
        public string end_date { get; set; }
        public string message { get; set; }
        public string total_students { get; set; }
        public bool isSuccess { get; set; }
        public string start_from_time { get; set; }
        public string start_to_time { get; set; }
        public string end_from_time { get; set; }
        public string end_to_time { get; set; }
        public string teach_id { get; set; }
        public string frn_name { get; set; }
    }
}
