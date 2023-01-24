using BCake.Parser.Errors;
using System.Collections.Generic;
using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Syntax.Expressions.Nodes.Operators {
    [Operator(
        Symbol = "new",
        Left = OperatorAttribute.ParameterType.None
    )]
    public class OperatorNew : Operator, IRValue {
        public FunctionType ParentFunction { get; protected set; }

        public OperatorNew() {}

        public override IEnumerable<Result> OnCreated(Token token, Scopes.Scope scope)
        {
            yield return Result.True;
        }
    }
}