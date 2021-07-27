using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class RoyaltyReportBean
    {
        public string Receipt_NO { get; set; }
        public string Receipt_Date { get; set; }
        public string Student_Code { get; set; }
        public string Student_Name { get; set; }
        public string Kit_Fees { get; set; }
        public string Module_Fees { get; set; }
        public string GST { get; set; }
        public string Other_fee { get; set; }
        public string Total_amount { get; set; }
        public string Royalti_payable { get; set; }
        public string Mention_module { get; set; }
        public string frn_code { get; set; }
        public string frn_name { get; set; }
        public string state { get; set; }
    }
}
