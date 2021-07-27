using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class ReceiptBean
    {
        public string receipt_date { get; set; }
        public string receipt_no { get; set; }
        public string receipt_to { get; set; }
        public string paid_amount { get; set; }
        public string fee_amount { get; set; }
        public string pending_amount { get; set; }
        public string received_by { get; set; }
        public string receipt_for { get; set; }
        public string payment_mode { get; set; }
    }
}
