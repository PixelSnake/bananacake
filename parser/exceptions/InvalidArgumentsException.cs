using BCake.Parser.Syntax.Expressions.Nodes.Functions;
using BCake.Parser.Syntax.Types;
using System.Collections.Generic;
using System.Linq;

namespace BCake.Parser.Exceptions {
    public class InvalidArgumentsException : Error {
        public InvalidArgumentsException(
            Token token,
            FunctionType function,
            ArgumentsNode.Argument[] provided
        )
            : base(
                $"No matching overload for function {function.FullName}:\n\tgiven {provided.Length}: ({FormatArgumentList(provided)})\n\n{FormatAllOverloads(function)}",
                token
            )
        {}

        private static string FormatParamList(FunctionType.ParameterType[] ps) {
            return "(" + string.Join(", ", ps.Select(p => p.Type.FullName)) + ")";
        }

        private static string FormatAllOverloads(FunctionType function)
        {
            var overloads = new List<string>();

            foreach (var overload in function.Overloads.Prepend(function))
            {
                overloads.Add($"\t\t{FormatParamList(overload.Parameters)}");
            }

            return "\tAvailable overloads are:\n" + overloads.JoinString("\n");
        }

        private static string FormatArgumentList(BCake.Parser.Syntax.Expressions.Nodes.Functions.ArgumentsNode.Argument[] args) {
            return string.Join(", ", args.Select(a => a.Expression.ReturnType.FullName));
        }
    }
}