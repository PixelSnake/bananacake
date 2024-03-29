using System.Linq;
using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Exceptions {
    public class InvalidOperatorDefinitionException : Error {
        public InvalidOperatorDefinitionException(Token token, string message)
                : base(
                    $"Invalid operator definition" + (message != null
                        ? $"\n\t{message}"
                        : "")
                    ,
                    token
                )
        {}
    }
}