using BCake.Parser.Syntax;
using BCake.Parser.Syntax.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCake.Std.IStringCast
{
    public class IStringCast : NativeInterfaceType
    {
        public static IStringCast Implementation = new IStringCast();

        public IStringCast() : base(Namespace.Global.Scope, "IStringCast") { }

        public override void ParseInner()
        {
            Scope.Declare(CastToString.Implementation);
        }
    }
}
