using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace stdlibgen
{
    internal static class NativeFunctionTypeGenerator
    {
        public static void Generate(string filePath, JObject header, string code)
        {
            var text = @"
namespace BCake.Std {
    public class %%NAME : NativeFunctionType {
        public static NativeFunctionType Implementation = new %%NAME(%%TYPEARG1, true);

        public override bool ExpectsThisArg => %%THISARG;

        private %%NAME(Type typearg1, bool initOverloads = false) : base(
            %%SCOPE,
            %%RETURNS,
            ""%%NAME"",
            new ParameterType[] {
                 new ParameterType(null, typearg1, ""p1""),
            },
            initOverloads ? new NativeFunctionType[]
            %%OVERLOADS : null
        ) {}

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments) {
            %%CODE
        }
    }
}
";

            var overloadBuilder = new StringBuilder("{\r\n");
            foreach (var typearg in header["typeArgs"].Skip(1))
            {
                overloadBuilder.Append($"                new {header["name"]}({typearg}),\r\n");
            }
            overloadBuilder.Append("            }");

            text = text.Replace("%%NAME", header["name"].ToString());
            text = text.Replace("%%TYPEARG1", header["typeArgs"][0].ToString());
            text = text.Replace("%%SCOPE", header["scope"].ToString());
            text = text.Replace("%%RETURNS", header["returns"].ToString());
            text = text.Replace("%%THISARG", header["thisArg"].ToString().ToLower());
            text = text.Replace("%%OVERLOADS", overloadBuilder.ToString());
            text = text.Replace("%%CODE", string.Join('\n', code.Split('\n').Select(l => $"            {l}")));

            var usingText = string.Join("\r\n", header["using"].Select(u => $"using {u};"));
            text = usingText + "\r\n" + text;

            File.WriteAllText(filePath, text);
        }
    }
}
