using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class PaymentBean  
    {
        public string pay_id { get; set; }
        public string form_id { get; set; }
        public string frn_id { get; set; }
        public string amount { get; set; }
        public string crdr { get; set; }
        public string create_date { get; set; }
        public string created_by { get; set; }
        public string payment_date { get; set; }
        public string message { get; set; }
        public bool is_opr_success { get; set; }
        public string txn_id { get; set; }
        public string fee_type_id { get; set; }
        public string discount { get; set; }
        public string fee_type_title { get; set; }
        public string is_cancel { get; set; }
        public string tax_amount { get; set; }
        public string fee_receivable { get; set; }
        public string fee_type_amount { get; set; }
        public string payment_type { get; set; }
    }
}
