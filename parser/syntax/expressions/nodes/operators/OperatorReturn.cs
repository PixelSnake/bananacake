using BCake.Parser.Errors;
using System.Collections.Generic;

namespace BCake.Parser.Syntax.Expressions.Nodes.Operators {
    [Operator(
        Symbol = "return",
        Left = OperatorAttribute.ParameterType.None
    )]
    public class OperatorReturn : Operator, IRValue {
        public Types.FunctionType ParentFunction { get; protected set; }

        public OperatorReturn() {}

        public override IEnumerable<Result> OnCreated(Token token, Scopes.Scope scope) {
            yield return ResultSense.FalseDominates;

            ParentFunction = scope.GetClosestFunction();
            if (ParentFunction == null)
            {
                yield return new InvalidReturnError(token);
                yield break;
            }

            CheckRightReturnType(ParentFunction.ReturnType);
        }
    }
}