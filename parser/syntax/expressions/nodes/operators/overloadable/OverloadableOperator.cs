using System.Linq;

namespace BCake.Parser.Syntax.Expressions.Nodes.Operators {
    public abstract class OverloadableOperator : Operator {
        public Types.Type Target { get; protected set; }
        public Expression _targetNode { get; protected set; }
        protected Types.FunctionType _operatorFunction;
        public override Types.Type ReturnType {
            get => _operatorFunction.ReturnType;
        }

        /// <summary>
        /// checks if the left hand side type of the operator defines an overload (for the
        /// derived operator class with the name given in the Operator attribute)
        /// </summary>
        protected override Expression ParseLeft(Scopes.Scope scope, Token[] tokens, Scopes.Scope typeSource) {
            Types.Type symbol;

            _targetNode = Expression.Parse(scope, tokens, typeSource);

            switch (_targetNode.Root) {
                case SymbolNode s: symbol = s.Symbol; break;
                case OperatorAccess o: symbol = o.ReturnSymbol; break;
                default: symbol = _targetNode.ReturnType; break;
            }

            var opMeta = Operator.GetOperatorMetadata(this.GetType());
            var opName = $"!operator_{ opMeta.OverloadableName }";
            _operatorFunction = _targetNode.ReturnType.Scope.GetSymbol(opName, true) as Types.FunctionType;
            if (_operatorFunction == null) throw new Exceptions.OperatorNotDefinedException(this.GetType(), DefiningToken, _targetNode.ReturnType);

            return _targetNode;
        }

        /// <summary>
        /// checks if the overload found in ParseLeft takes the right hand side type
        /// as a parameter. If yes, remembers the fitting overload in _operatorFunction.
        /// (if there is no overload, this function will never be called)
        /// </summary>
        protected override Expression ParseRight(Scopes.Scope scope, Token[] tokens, Scopes.Scope typeSource) {
            var right = Expression.Parse(scope, tokens);

            // no need to check _operatorFunction for null because that happens in ParseLeft
            Types.Type[] argumentTypes;

            // if the operator is implemented by a native function it might expect this to be passed as the first argument
            if (_operatorFunction.ExpectsThisArg)
                argumentTypes = new Types.Type[] { Left.ReturnType, right.ReturnType };
            else
                argumentTypes = new Types.Type[] { right.ReturnType };

            var overload = _operatorFunction.GetMatchingOverload(argumentTypes);
            
            if (overload == null) {
                throw new Exceptions.InvalidArgumentsException(
                    DefiningToken,
                    _operatorFunction,
                    new Functions.ArgumentsNode.Argument[] { new Functions.ArgumentsNode.Argument(right) }
                );
            }

            _operatorFunction = overload;
            return right;
        }

        public OperatorInvoke ToOperatorInvoke(Scopes.Scope scope) {
            return OperatorInvoke.FromOverloadableOperator(scope, this, _operatorFunction, _targetNode);
        }
    }
}