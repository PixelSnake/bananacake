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
    public class %%CLASSNAME : NativeFunctionType {
        public static readonly NativeFunctionType Implementation = new %%CLASSNAME(%%CONSTRCALL);

        public override bool ExpectsThisArg => %%THISARG;

        private %%CLASSNAME(%%CONSTRARGS) : base(
            %%SCOPE,
            %%RETURNS,
            ""%%NAME"",
            new ParameterType[] {
%%PARAMS
            }%%OVERLOADS
        ) {}

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments) {
            %%CODE
        }
    }
}
";

            var className = (header["classname"] ?? header["name"]).ToString();

            if (header["typeArgs"] != null)
            {
                var overloadBuilder = new StringBuilder("{\r\n");
                foreach (var typearg in header["typeArgs"].Skip(1))
                {
                    overloadBuilder.Append($"                new {className}({typearg}),\r\n");
                }
                overloadBuilder.Append("            }");

                text = text.Replace("%%OVERLOADS", $",\r\n            initOverloads ? new NativeFunctionType[] {overloadBuilder} : null");
            }
            else
            {
                text = text.Replace("%%OVERLOADS", "");
            }

            if (header["typeArgs"] != null)
            {
                text = text.Replace("%%CONSTRARGS", "Type typearg1, bool initOverloads = false");
                text = text.Replace("%%CONSTRCALL", $"{header["typeArgs"][0].ToString()}, true");
            }
            else
            {
                text = text.Replace("%%CONSTRARGS", "");
                text = text.Replace("%%CONSTRCALL", "");
            }

            var paramsBuilder = new StringBuilder();
            var paramIdx = 0;
            foreach (var param in header["params"])
            {
                paramsBuilder.Append($"                new ParameterType(Token.NativeCode(), {param}, \"p{paramIdx++}\")\r\n");
            }

            text = text.Replace("%%PARAMS", paramsBuilder.ToString());
            text = text.Replace("%%CLASSNAME", className);
            text = text.Replace("%%NAME", header["name"].ToString());
            text = text.Replace("%%SCOPE", header["scope"].ToString());
            text = text.Replace("%%RETURNS", header["returns"].ToString());
            text = text.Replace("%%THISARG", header["thisArg"].ToString().ToLower());
            text = text.Replace("%%CODE", string.Join('\n', code.Split('\n').Select(l => $"            {l}")));

            var usingText = string.Join("\r\n", header["using"].Select(u => $"using {u};"));
            text = usingText + "\r\n" + text;

            File.WriteAllText(filePath, text);
        }
    }
}
