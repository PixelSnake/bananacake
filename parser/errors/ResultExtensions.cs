using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCake.Parser.Errors
{
    public static class ResultExtensions
    {
        public static IEnumerable<Error> Errors(this IEnumerable<Result> res) => res
            .Where(r => r.IsError)
            .Cast<Error>();
        public static bool HasErrors(this IEnumerable<Result> res) => res
            .Errors()
            .Any();
        public static T Value<T>(this IEnumerable<Result> res) => res
            .Where(r => !r.IsError)
            .Cast<BaseResultValue>()
            .Select(v => (T)v.Value)
            .FirstOrDefault();
    }
}
