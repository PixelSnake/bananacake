namespace BCake.Parser.Syntax.Expressions.Nodes.Operators {
    [Operator(
        Symbol = "=",
        Left = OperatorAttribute.ParameterType.LValue,
        TypeSlope = OperatorAttribute.TypeSlopeDirection.ToLeft
    )]
    public class OperatorAssign : Operator, IRValue {
        public OperatorAssign() {}
    }
}