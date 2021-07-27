using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class VerificationBean
    {
        public string StudentName { get; set; }
        public string EnrollNo { get; set; }
        public string Grade{get;set;}
        public string JoinDate { get; set; }
        public string CourseName { get; set; }
        public string Duration { get; set; }
        public bool isFail { get; set; }
        public string message { get; set; }
    }
}
