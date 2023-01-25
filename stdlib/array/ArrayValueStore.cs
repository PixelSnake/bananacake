using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = BCake.Parser.Syntax.Types.Type;

namespace BCake.Std.Array
{
    internal static class ArrayValueStore
    {
        internal static Dictionary<int, object[]> Arrays = new();
        internal static Dictionary<int, Type> Types = new();
    }
}
