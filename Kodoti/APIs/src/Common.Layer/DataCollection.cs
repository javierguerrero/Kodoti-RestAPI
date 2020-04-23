using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Layer
{
    public class DataCollection <T>
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
