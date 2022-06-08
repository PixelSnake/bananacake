using System.Collections.Generic;
using BCake.Parser.Errors;
using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Syntax.Expressions.Nodes.Operators {
    [Operator(
        Symbol = ".",
        Direction = OperatorAttribute.EvaluationDirection.RightToLeft,
        CheckReturnTypes = false
    )]
    public class OperatorAccess : Operator, IRValue {
        public Type SymbolToAccess { get; protected set; }
        public Type MemberToAccess { get; protected set; }

        public override Type ReturnType {
            get {
                return Right.ReturnType;
            }
        }

        public Type ReturnSymbol {
            get {
                switch (Right.Root) {
                    case SymbolNode n: return n.Symbol;
                    default: return Right.ReturnType;
                }
            }
        }

        protected override Expression ParseRight(Scopes.Scope scope, Token[] tokens, Scopes.Scope typeSource) {
            if (Left == null) throw new System.Exception("Left hand side of access operator must not be null when parsing right hand side");

            var symbol = Expression.Parse(scope, tokens, Left.ReturnType.Scope);

            return symbol;
        }

        public override IEnumerable<Result> OnCreated(Token token, Scopes.Scope scope) {
            yield return ResultSense.FalseDominates;

            var leftSymbol = Left.Root as SymbolNode;
            if (leftSymbol == null) yield break;

            // we need to treat the left hand side specifically, if it is not a type
            switch (leftSymbol.Symbol) {
                case ClassType: yield break;

                default:
                    SymbolToAccess = leftSymbol.Symbol;
                    MemberToAccess = (Right.Root as SymbolNode).Symbol;

                    var leftType = leftSymbol.ReturnType;
                    var isSameType = leftType.FullName == scope.GetClosestType()?.FullName;

                    var canAccess = MemberToAccess.Access == Access.@public || scope.IsChildOf(MemberToAccess.Scope) || isSameType;
                    if (!canAccess) throw new Exceptions.AccessViolationException(Right.DefiningToken, MemberToAccess, scope);
                    break;
            }
        }
    }
}