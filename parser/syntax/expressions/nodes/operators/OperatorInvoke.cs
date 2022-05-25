using System.Linq;
using BCake.Parser.Exceptions;
using BCake.Parser.Syntax.Expressions.Nodes.Functions;
using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Syntax.Expressions.Nodes.Operators {
    [Operator(
        Symbol = "(",
        CheckReturnTypes = false,
        Direction = OperatorAttribute.EvaluationDirection.RightToLeft
    )]
    public class OperatorInvoke : Operator, IRValue {
        public FunctionType Function { get; protected set; }
        protected Expression _functionNode;

        protected override Expression ParseLeft(Scopes.Scope scope, Token[] tokens, Scopes.Scope typeSource) {
            Type symbol;

            _functionNode = Expression.Parse(scope, tokens, typeSource);

            switch (_functionNode.Root) {
                case SymbolNode s: symbol = s.Symbol; break;
                case OperatorAccess o: symbol = o.ReturnSymbol; break;
                case Syntactical.OperatorTypeArgument t: symbol = t.ConcreteClassType; break;
                default: symbol = _functionNode.ReturnType; break;
            }

            if (!(symbol is FunctionType || symbol is ClassType))
            {
                if (symbol is InterfaceType) throw new InvalidConstructorCallException(symbol, _functionNode.DefiningToken);

                throw new System.Exception("TODO invalid call");
            }
            Function = symbol as FunctionType;

            // if the function is null, this must be a constructor call
            if (Function == null) {
                var classType = symbol as ClassType;

                Function = classType?.Scope.GetSymbol("!constructor", true) as FunctionType;
                if (Function == null && classType is ConcreteClassType) {
                    var concreteClassType = classType as ConcreteClassType;
                    Function = concreteClassType.GenericType.Scope.GetSymbol("!constructor", true) as FunctionType;
                }

                if (Function != null && classType is GenericClassType and not ConcreteClassType)
                {
                    throw new Exceptions.MissingTypeArgumentsException(tokens.First(), symbol);
                }

                // This means that the class has no constructor, so we use a default empty constructor instead
                if (Function == null)
                {
                    // we will declare an empty default constructor for later
                    if (classType is GenericClassType and not ConcreteClassType)
                        throw new MissingTypeArgumentsException(tokens.First(), symbol);

                    Function = classType.GenerateDefaultConstructor();
                    Function.ParseInner();
                    return new Expression(null, classType.Scope, new SymbolNode(null, Function));
                }

                if (Function.Access != Access.@public && !scope.IsChildOf(classType.Scope)) {
                    throw new Exceptions.AccessViolationException(_functionNode.DefiningToken, Function, scope);
                }

                symbol = Function;
            }

            return _functionNode;
        }

        protected override Expression ParseRight(Scopes.Scope scope, Token[] tokens, Scopes.Scope typeSource) {
            var argListClose = ParserHelper.FindClosingScope(
                tokens.Prepend(new Token { Value = "(" }).ToArray(),
                0
            ) - 1;
            var argList = tokens.Take(argListClose).ToArray();

            var arguments = ArgumentsNode.Parse(_functionNode, scope, argList);
            var overload = Function.GetMatchingOverload(arguments.Arguments);
            if (overload == null) {
                if (argList.Length > 0) throw new Exceptions.InvalidArgumentsException(argList.FirstOrDefault(), Function, arguments.Arguments);
                else throw new Exceptions.InvalidArgumentsException(tokens[0], Function, arguments.Arguments);
            }

            Function = overload;

            return new Expression(
                argList.FirstOrDefault() ?? _functionNode.DefiningToken,
                scope,
                arguments
            );
        }

        [OperatorParsePreflight]
        public static bool ParsePreflight(OperatorParseInfo info) {
            var tokens = info.Tokens;

            if (tokens.Length <= 2) return false;

            if (!SymbolNode.CouldBeIdentifier(info.TokensLeft)) {
                return false;
            }

            return true;
        }

        public static OperatorInvoke FromOverloadableOperator(
            Scopes.Scope scope,
            OverloadableOperator op,
            FunctionType operatorFunction,
            Expression operatorFunctionExpression
        ) {
            var opInvoke = new OperatorInvoke();
            opInvoke.Function = operatorFunction;

            // opInvoke.Left = new Expression(
            //     op.DefiningToken,
            //     scope,
            //     new ValueN
            // );

            var leftScope = operatorFunctionExpression.ReturnType.Scope;
            if (op.Left.Root is SymbolNode) {
                var symbolNode = op.Left.Root as SymbolNode;
                if (symbolNode.ReturnType is PrimitiveType) leftScope = symbolNode.ReturnType.Scope;
                else leftScope = symbolNode.Symbol.Scope;
            }

            Node left;
            if (op.Left.ReturnType is PrimitiveType) {
                left = Operator.Parse(
                    leftScope,
                    leftScope,
                    typeof(OperatorAccess),
                    op.Left.DefiningToken,
                    new Token[] { new Token { Value = op.Left.ReturnType.Name } },
                    new Token[] { new Token { Value = operatorFunction.Name } }
                );
            }
            else {
                left = Operator.Parse(
                    leftScope,
                    leftScope,
                    typeof(OperatorAccess),
                    op.Left.DefiningToken,
                    new Token[] { op.Left.DefiningToken },
                    new Token[] { new Token { Value = operatorFunction.Name } }
                );
            }

            opInvoke.Left = new Expression(
                op.DefiningToken,
                leftScope,
                left
            );
            opInvoke.Right = new Expression(
                op.DefiningToken,
                scope,
                new ArgumentsNode(
                    op.DefiningToken,
                    null,
                    new ArgumentsNode.Argument[] {
                        new ArgumentsNode.Argument(op.Left, true), // provide the left hand side for native implementations
                        new ArgumentsNode.Argument(op.Right)
                    }
                )
            );

            return opInvoke;
        }
    }
}