using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class FeeBean
    {
        public string fee_id { get; set; }
        public string fee_amount { get; set; }
        public string fee_amount_min { get; set; }
        public string fee_amount_max { get; set; }
        public string fee_max { get; set; }
        public string fee_min { get; set; }
        public string fee_type { get; set; }
        public string fee_type_id { get; set; }
        public string is_active { get; set; }
        public string frn_id { get; set; }
        public string tax { get; set; }
        public string tax_old { get; set; }
        public string fee_old_amout { get; set; }
        public bool is_db_success { get; set; }
    }
}
