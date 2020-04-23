using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Layer
{
    public class QueryFilter
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public QueryFilterType Type { get; set; }
    }

    public enum QueryFilterType
    {
        eq,
        NotEq
    }
}
