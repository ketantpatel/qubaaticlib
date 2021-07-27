using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.beans
{
    public class AskEducation
    {
        public AskEducation(string title, string value)
        {
            edu_title = title;
            edu_id = value;
        }
        public string edu_id { get; set; }
        public string edu_title { get; set; }
    }
}
