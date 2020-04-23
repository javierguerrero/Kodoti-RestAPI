using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Layer
{
    public abstract class ResponseBaseHelper
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ResponseHelper<T> : ResponseBaseHelper
    {
        public T Result { get; set; }
    }

    public class ResponseHelper : ResponseBaseHelper
    {

    }
}
