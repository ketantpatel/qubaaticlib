using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class SourceBean
    {
        public SourceBean(string title, string value)
        {
            source_id = value;
            this.title = title;
        }
        public string source_id { get; set; }
        public string title { get; set; }
    }
}
