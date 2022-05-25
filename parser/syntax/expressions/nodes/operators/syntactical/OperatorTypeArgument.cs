using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BCake.Parser.Exceptions;
using BCake.Parser.Syntax.Scopes;

namespace BCake.Parser.Syntax.Expressions.Nodes.Operators.Syntactical {
    [Operator(
        Symbol = "<",
        CheckReturnTypes = false
    )]
    public class OperatorTypeArgument : Operator {
        public Types.ConcreteClassType ConcreteClassType;
        public Types.Type[] TypeArguments;
        public override Types.Type ReturnType => ConcreteClassType;

        protected override Expression ParseLeft(Scope scope, Token[] tokens, Scope typeSource) {
            var left = Expression.Parse(scope, tokens, typeSource);
            var classType = left.ReturnType as Types.ClassType;
            if (classType == null) throw new UnexpectedTokenException(tokens[0]); 

            var genericClassType = classType as Types.GenericClassType;
            if (genericClassType == null) throw new InvalidTypeArgumentListException(tokens[0], classType); 

            return left;
        }

        protected override Expression ParseRight(Scope scope, Token[] tokens, Scope typeSource) {
            TypeArguments = ParseGenericTypeArgs(tokens.SkipEnd(1).ToArray(), scope, typeSource).ToArray();

            var genericClassType = Left.ReturnType as Types.GenericClassType;
            Types.ConcreteClassType.GuardTypeArguments(tokens[0], genericClassType, TypeArguments);
            ConcreteClassType = new Types.ConcreteClassType(genericClassType, TypeArguments);
            
            return null;
        }

        [OperatorParsePreflight]
        public static bool ParsePreflight(OperatorParseInfo info) {
            if (info.Tokens.Length < 1) return false;
            return info.TokensRight.Last().Value == ">";
        }

        public static IEnumerable<Types.Type> ParseGenericTypeArgs(Token[] tokens, Scopes.Scope scope, Scopes.Scope typeSource) {
            var listItemStart = 0;
            while (true) {
                var listItemEnd = ParserHelper.FindListItemEnd(tokens, listItemStart);
                Token[] listItemTokens;

                if (listItemEnd < 0) {
                    listItemTokens = tokens.Skip(listItemStart).Take(tokens.Length - listItemStart).ToArray();
                } else {
                    listItemTokens = tokens.Skip(listItemStart).Take(listItemEnd - listItemStart).ToArray();
                }

                var typeArg = Expression.Parse(scope, listItemTokens, typeSource);
                // TODO we should check to only allow TYPES here, not expressions that resolve to a type
                yield return typeArg.ReturnType;

                if (listItemEnd < 0) break;

                listItemStart = listItemEnd + 1;
            }
        }
    }
}