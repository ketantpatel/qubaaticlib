using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class ExamMarksData
    {
        public int data_id { get; set; }
        public int teach_id { get; set; }
        public int batch_id { get; set; }
        public int form_id { get; set; }
        public int level_id { get; set; }
        public int frn_id { get; set; }
        public DateTime exam_date { get; set; }
        public float a { get; set; }
        public float b { get; set; }
        public float c { get; set; }
        public float d { get; set; }
        public float e { get; set; }
        public float f { get; set; }
        public float g { get; set; }
        public float h { get; set; }
        public float avg { get; set; }
        public DateTime create_date { get; set; }
        public DateTime modify_date { get; set; }
        public int create_by { get; set; }
        public int modify_by { get; set; }
    }
}
