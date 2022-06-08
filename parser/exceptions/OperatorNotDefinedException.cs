using BCake.Parser.Syntax.Types;
using BCake.Parser.Syntax.Expressions.Nodes.Operators;

namespace BCake.Parser.Exceptions {
    public class OperatorNotDefinedException : Error {
        public OperatorNotDefinedException(System.Type opType, Token token, Type type)
            : base($"Invalid use of operator \"{ Operator.GetOperatorMetadata(opType).Symbol }\" - no overload defined on type { type.FullName }", token)
        {}
    }
}