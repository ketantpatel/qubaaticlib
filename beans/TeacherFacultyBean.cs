using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class TeacherFacultyBean
    {
        public string allocate_id { get; set; }
        public string frn_id { get; set; }
        public string teach_id { get; set; }
        public string create_date { get; set; }
        public string status { get; set; }
        public string modify_date { get; set; }

        public bool is_checked { get; set; }

        public bool is_success { get; set; }

        public string frn_name { get; set; }
    }
}
