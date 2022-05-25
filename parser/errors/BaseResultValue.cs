using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCake.Parser.Errors
{
    public class BaseResultValue : Result
    {
        public object Value { get; }

        public BaseResultValue(object value)
        {
            Value = value;
        }
    }
}
