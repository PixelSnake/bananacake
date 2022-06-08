using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCake.Parser.Errors
{
    public class Result
    {
        public static ResultValue<bool> True => new ResultValue<bool>(true);
        public static ResultValue<bool> False => new ResultValue<bool>(false);

        public virtual bool IsError => false;
    }

    public class MultiResult : Result
    {
        public readonly IEnumerable<Result> Results;

        public MultiResult(IEnumerable<Result> results)
        {
            Results = results;
        }
    }
}
