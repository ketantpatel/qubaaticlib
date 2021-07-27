using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class PaperSessionBean
    {
        public string session_id { get; set; }
        public string start_date { get; set; }
        public string exam_time { get; set; }
        public string expire_time { get; set; }
        public string exam_id { get; set; }
        public string form_id { get; set; }
        public string modify_date { get; set; }
        public string expire_minute { get; set; }
    }
}
