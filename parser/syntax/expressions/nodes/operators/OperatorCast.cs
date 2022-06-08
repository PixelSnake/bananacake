using BCake.Parser.Errors;
using BCake.Parser.Exceptions;
using BCake.Parser.Syntax.Types;
using System.Collections.Generic;

namespace BCake.Parser.Syntax.Expressions.Nodes.Operators {
    [Operator(
        Symbol = "as",
        Direction = OperatorAttribute.EvaluationDirection.RightToLeft,
        CheckReturnTypes = false
    )]
    public class OperatorCast : Operator, IRValue {
        public override Type ReturnType {
            get => (Right?.Root as SymbolNode).Symbol;
        }

        public override IEnumerable<Result> OnCreated(Token token, Scopes.Scope scope) {
            yield return ResultSense.FalseDominates;

            var hasUnexpectedTokensRes = GuardUnexpectedTokens();
            if (hasUnexpectedTokensRes.HasErrors())
            {
                yield return hasUnexpectedTokensRes.Pack();
                yield break;
            }

            var leftSymbol = (Left?.Root as SymbolNode).Symbol;
            var rightSymbol = (Right?.Root as SymbolNode).Symbol;

            Type leftType = null;
            switch (leftSymbol) {
                case MemberVariableType t: leftType = t.Type; break;
                case LocalVariableType t: leftType = t.Type; break;
                case FunctionType.ParameterType t: leftType = t.Type; break;
                case ClassType t: leftType = t; break;
            }

            if (leftType == null)
            {
                yield return new InvalidCastError(leftSymbol, rightSymbol, DefiningToken);
                yield break;
            }

            var res = Validate(leftSymbol, leftType, rightSymbol);
            foreach (var err in res.Errors())
                yield return err;
        }

        private IEnumerable<Result> GuardUnexpectedTokens()
        {
            yield return ResultSense.FalseDominates;

            if (!(Left?.Root is SymbolNode))
            {
                yield return new UnexpectedTokenError(DefiningToken);
                yield return Result.False;
            }
            if (!(Right?.Root is SymbolNode))
            {
                yield return new UnexpectedTokenError(DefiningToken);
                yield return Result.False;
            }
        }

        /// <param name="symbol">The symbol that is being casted from <see cref="from"/> to <see cref="to"/>.</param>
        private IEnumerable<Result> Validate(Type symbol, Type from, Type to)
        {
            yield return ResultSense.FalseDominates;

            var casterFunction = from.Scope.GetSymbol($"!as_{ to.Name }", true);
            if (casterFunction == null)
            {
                // if there is some kind of inheritance relationship between the two this is not a compile time error
                if (from is InheritableType inhFrom && to is InheritableType inhTo && (inhFrom.IsDescendantOf(inhTo) || inhTo.IsDescendantOf(inhFrom)))
                    yield break;

                yield return new InvalidCastError(symbol, to, DefiningToken);
            }
        }
    }
}