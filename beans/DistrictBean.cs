using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class DistrictBean
    {
        public string dist_id { get; set; }
        public string dist_name { get; set; }
    }
    public class StateBean 
    {
        public string state_id { get; set; }
        public string state_name { get; set; }

        public string code { get; set; }

        public string country_id { get; set; }

        public bool isSuccess { get; set; }
    }
    public class AreaBean
    {
        public string area_id { get; set; }
        public string area_name { get; set; }

        public string area_code { get; set; }

        public string state_id { get; set; }

        public bool isSuccess { get; set; }
    }
    public class CountryBean
    {
        public string country_id { get; set; }
        public string country_name { get; set; }
        public string country_code { get; set; }
    }
}
