namespace BCake.Parser.Syntax.Expressions.Nodes.Operators.Comparison {
    /// <summary>
    /// Operator smaller needs to suppress errors that ocurr on parsing, because
    /// generic type argument lists incorrectly trigger parsing of this operator.
    /// Parsing may therefore fail without immediately throwing an exception.
    /// </summary>
    [Operator(Symbol = "<", SuppressErrors = true)]
    public class OperatorSmaller : ComparisonOperator, IRValue {}
}