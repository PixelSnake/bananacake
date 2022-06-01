using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCake.Parser.Errors
{
    /// <summary>
    /// Contains information on how the results of a method should be interpreted
    /// 
    /// Example: A method can yield return a ResultSense object to specify the result sense of the method
    /// </summary>
    public class ResultSense : Result
    {
        /// <summary>
        /// One false result will make the overall result equal false
        /// </summary>
        public static readonly ResultSense FalseDominates = new ResultSense(ResultDominance.FalseDominates, true);
        /// <summary>
        /// One true result will make the overall result equal true
        /// </summary>
        public static readonly ResultSense TrueDominates = new ResultSense(ResultDominance.TrueDominates, false);
        /// <summary>
        /// The first result will dictate the overall result
        /// </summary>
        public static readonly ResultSense First = new ResultSense(ResultDominance.First, false);
        /// <summary>
        /// The last result will dictate the overall result
        /// </summary>
        public static readonly ResultSense Last = new ResultSense(ResultDominance.Last, false);

        public static ResultSense Default => FalseDominates;

        public readonly ResultDominance Sense;
        public readonly bool DefaultValue;

        private ResultSense(ResultDominance sense, bool defaultValue)
        {
            Sense = sense;
            DefaultValue = defaultValue;
        }

        public enum ResultDominance
        {
            FalseDominates = 0,
            TrueDominates,
            First,
            Last,
        }
    }
}
