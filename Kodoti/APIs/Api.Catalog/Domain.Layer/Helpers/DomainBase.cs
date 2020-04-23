using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Layer.Helpers
{
    public abstract class DomainBase
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
