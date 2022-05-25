using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace BCake.Parser.Exceptions {
    public class InvalidTypeArgumentListException : TokenException {
        public InvalidTypeArgumentListException(BCake.Parser.Token token, BCake.Parser.Syntax.Types.ClassType classType)
            : base($"Invalid type argument list - the type { classType.Name } is not generic", token)
        {}

        public InvalidTypeArgumentListException(
            BCake.Parser.Token token,
            BCake.Parser.Syntax.Types.GenericClassType classType,
            IEnumerable<BCake.Parser.Syntax.Types.Type> providedTypes)
            : base($"Invalid type argument list - the type(s) <{ string.Join(",", providedTypes.Select(t => t.Name)) }> do not match the type parameters of { classType.Name }<{ string.Join(",", classType.GenericTypeParameters.Select(p => p.Name)) }>", token)
        {}
    }
}