using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class DocumentBean
    {
        public string doc_id { get; set; }
        public string title { get; set; }
        public string is_verified { get; set; }
        public string verify_date { get; set; }
        public string frn_id { get; set; }
        public string form_id { get; set; }
        public string message { get; set; }
        public string doc_type { get; set; }
        public byte[] file { get; set; }
        public string filePath { get; set; }
        public bool is_opr_success { get; set; }
        public string name { get; set; }
        public string enroll_no { get; set; }
    }
}
