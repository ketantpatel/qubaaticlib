using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.beans
{
    public class Instructor_Training
    {
        public int it_id { get; set; }
        public string title { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string level_id { get; set; }
        public string level_name { get; set; }
        public string teach_id { get; set; }
        public string created_date { get; set; }
        public string user_id { get; set; }
        public string modified_date { get; set; }
        public bool is_success { get; set; }
        public string course_id { get; set; }
        public string status { get; set; }
        public string days { get; set; }
        public string week_days { get; set; }
        public string location { get; set; }
        public string country_id { get; set; }      
    }
}
