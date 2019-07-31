using System;
using System.Collections.Generic;
using System.Text;

namespace EntitySpaces.ProfilerApplication
{
    public class esParameters : List<esParameter>
    {

    }

    public class esParameter
    {
        public string Name { get; set; }
        public string Direction { get; set; }
        public string ParamType { get; set; }
        public string BeforeValue { get; set; }
        public string AfterValue { get; set; }
    }
}
