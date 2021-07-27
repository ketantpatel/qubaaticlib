using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class SeminarSetupBean
    {
        public string seminar_setup_id { get; set; }
        public string name { get; set; }
        public string seminarDate { get; set; }
        public string created_by { get; set; }
        public string CreatedDate { get; set; }
        public bool isSuccess { get; set; }
        public string frn_id { get; set; }
    }
}
