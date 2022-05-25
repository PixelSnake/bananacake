using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCake.Parser.Errors
{
    public class ResultValue<T> : BaseResultValue
    {
        public new T Value => (T)base.Value;

        public ResultValue(T value) : base(value) { }
    }
}
