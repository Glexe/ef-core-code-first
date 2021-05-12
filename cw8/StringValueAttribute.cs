using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw8
{
    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; private set; }
        public StringValueAttribute(string value)
        {
            StringValue = value;
        }
    }
}
