using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Syntax.Expressions.Nodes.Functions {
    public class ArgumentsNode : Node, IRValue {
        public Argument[] Arguments { get; protected set; }
        public Expression FunctionNode { get; protected set; }
        /* ArgumentsNode needs to have the same return type as the function itself for typechecking */
        public override Types.Type ReturnType { get => FunctionNode.ReturnType; }

        public ArgumentsNode(Token token, Expression functionNode, Argument[] arguments) : base(token) {
            Arguments = arguments;
            FunctionNode = functionNode;
        }

        public static ArgumentsNode Parse(Expression functionNode, Scopes.Scope scope, Token[] tokens) {
            var pos = 0;
            var arguments = new List<Argument>();

            while (true) {
                var paramEnd = ParserHelper.FindListItemEnd(tokens, pos);
                if (paramEnd == -1) paramEnd = tokens.Length;
                var paramTokens = tokens.Skip(pos).Take(paramEnd - pos).ToArray();
                if (paramTokens.Length < 1) break;

                var paramExpr = Expression.Parse(scope, paramTokens);
                arguments.Add(new Argument(paramExpr));

                pos += paramTokens.Length + 1;
            }

            return new ArgumentsNode(tokens.FirstOrDefault(), functionNode, arguments.ToArray());
        }

        public class Argument {
            public Expression Expression { get; protected set; }
            public bool IsThisArg { get; protected set; }

            public Argument(Expression expression, bool isThisArg = false) {
                Expression = expression;
                IsThisArg = isThisArg;
            }
        }
    }
}