using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCake.Parser.Errors
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Packs the results of a nested method to a multi-result that will be unpacked during further processing
        /// </summary>
        public static MultiResult Pack(this IEnumerable<Result> res) => new MultiResult(res);

        public static IEnumerable<Result> Unpack(this IEnumerable<Result> res)
        {
            foreach (var r in res)
            {
                if (r is MultiResult multi)
                {
                    foreach (var _r in multi.Results.Unpack())
                        yield return _r;
                }
                else
                {
                    yield return r;
                }
            }
        }

        /// <summary>
        /// Returns the first ResultSense object in the result enumerable. Nested result senses are ignored
        /// </summary>
        public static ResultSense Sense(this IEnumerable<Result> res) {
            var senses = res.Where(r => r is ResultSense).Cast<ResultSense>().ToArray();
            if (senses.Length == 0) return ResultSense.Default;
            if (senses.Length == 1) return senses.First();
            throw new Exception("Multiple result senses returned");
        }

        /// <summary>
        /// Returns all results except for any ResultSense objects
        /// </summary>
        public static IEnumerable<Result> WithoutSense(this IEnumerable<Result> res)
            => res.Where(r => r is not ResultSense).Unpack();
        
        /// <summary>
        /// The first bool result
        /// </summary>
        public static bool FirstBool(this IEnumerable<Result> res)
            => (res.Unpack().First(r => r is ResultValue<bool>) as ResultValue<bool>).Value;

        /// <summary>
        /// All errors
        /// </summary>
        public static IEnumerable<Error> Errors(this IEnumerable<Result> res)
            => res.Unpack().Where(r => r.IsError)
            .Cast<Error>();
        
        /// <summary>
        /// All errors
        /// </summary>
        public static IEnumerable<BaseResultValue> Values(this IEnumerable<Result> res)
            => res.Unpack().Where(r => r is BaseResultValue)
            .Cast<BaseResultValue>();

        /// <summary>
        /// Whether or not the method has returned any errors
        /// </summary>
        public static bool HasErrors(this IEnumerable<Result> res)
            => res.Unpack().Errors()
            .Any();

        /// <summary>
        /// Specifies a callback method for when an error occurrs
        /// </summary>
        public static IEnumerable<Result> Catch(this IEnumerable<Result> res, Action<Error[]> onErrors)
        {
            var errors = res.Unpack().Errors().ToArray();

            if (errors.Length > 0)
                onErrors(errors);

            return res;
        }

        /// <summary>
        /// If the method returns one or more errors, they are automatically thrown as an exception
        /// </summary>
        public static IEnumerable<Result> Throw(this IEnumerable<Result> res) => res.Unpack().Catch(errs => throw Error.Multiple(errs).ToException());

        /// <summary>
        /// The first or default value of type <see cref="T"/> returned
        /// </summary>
        public static T Value<T>(this IEnumerable<Result> res)
        {
            if (typeof(T) == typeof(bool)) throw new Exception("Result sense matters! Use BoolValue instead");

            return res
                .Unpack()
                .Where(r => !r.IsError)
                .Cast<BaseResultValue>()
                .Select(v => (T)v.Value)
                .FirstOrDefault();
        }

        /// <summary>
        /// The overall boolean result, according to the given <see cref="ResultSense"/>
        /// </summary>
        public static bool BoolValue(this IEnumerable<Result> res)
        {
            var values = res
                .Unpack()
                .Where(r => r is ResultValue<bool>)
                .Cast<ResultValue<bool>>()
                .ToArray();

            var sense = res.Sense();

            if (values.Length == 0) return sense.DefaultValue;
            if (values.Length == 1) return values.First().Value;
            
            switch (sense.Sense)
            {
                case ResultSense.ResultDominance.FalseDominates:
                    return values.Any(r => r.Value == false) ? false : sense.DefaultValue;
                case ResultSense.ResultDominance.TrueDominates:
                    return values.Any(r => r.Value == true) ? true : sense.DefaultValue;
                case ResultSense.ResultDominance.First:
                    return values.Where(r => r is ResultValue<bool>).Cast<ResultValue<bool>>().Select(r => r.Value).FirstOr(sense.DefaultValue);
                case ResultSense.ResultDominance.Last:
                    return values.Where(r => r is ResultValue<bool>).Cast<ResultValue<bool>>().Select(r => r.Value).Reverse().FirstOr(sense.DefaultValue);
                default:
                    throw new Exception("Invalid result sense");
            }
        }
    }
}
