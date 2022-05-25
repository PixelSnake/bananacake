using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BCake.Parser.Syntax.Expressions.Nodes;

namespace BCake.Parser.Syntax.Expressions.Nodes {
    public class SymbolNode : Node, ILValue, IRValue {
        public static readonly string rxIdentifier = @"^[A-Za-z_][A-Za-z_0-9]*(\.[A-Za-z_][A-Za-z_0-9]*)*$";
        public Types.Type Symbol { get; protected set; }
        public override Types.Type ReturnType {
            get {
                switch (Symbol) {
                    case Types.LocalVariableType t: return t.Type;
                    case Types.MemberVariableType t: return t.Type;
                    case Types.FunctionType t: return t.ReturnType;
                    case Types.FunctionType.ParameterType t: return t.Type;
                    case Types.ClassType t: return t;
                    case Types.PrimitiveType t: return t;
                    case Namespace t: return t;
                }
                return null; // todo what now? does not make much sense
            }
        }

        public SymbolNode(Token token, Types.Type symbol) : base(token) {
            Symbol = symbol;
        }

        /// <summary>
        /// Tests if a given list of tokens is a syntactically correct identifier.
        /// Does not test whether the identifier exists.
        /// </summary>
        public static bool CouldBeIdentifier(Token[] tokens) {
            return CouldBeIdentifier(tokens, out var _);
        }

        /// <summary>
        /// Tests if a given list of tokens is a syntactically correct identifier.
        /// Does not test whether the identifier exists.
        /// </summary>
        /// <param name="simpleMatch">Returns the initial regex match that is successful for types without generic type arguments.</param>
        public static bool CouldBeIdentifier(Token[] tokens, out Match simpleMatch) {
            var tokensString = string.Join("", tokens.SelectValues());

            simpleMatch = Regex.Match(tokensString, rxIdentifier);
            if (simpleMatch.Success) return true;

            // it's a string literal
            if (tokensString.StartsWith("\"") && tokensString.EndsWith("\""))
            {
                return false;
            }

            // we might have type args here, need to do further testing
            if (tokensString.Contains("<") && tokensString.Contains(">")) {
                var firstAngleBracketPos = tokens.SelectValues().IndexOf("<");
                var endAngleBracketPos = ParserHelper.FindClosingScope(tokens, firstAngleBracketPos);
                var innerTokens = tokens.TakeRange(firstAngleBracketPos + 1, endAngleBracketPos).ToArray();

                var listItemStart = 0;
                while (listItemStart < innerTokens.Length) {
                    var listItemEnd = ParserHelper.FindListItemEnd(innerTokens, listItemStart);
                    if (listItemEnd < 0) listItemEnd = innerTokens.Length;

                    var listItemTokens = innerTokens.TakeRange(listItemStart, listItemEnd);
                    if (!CouldBeIdentifier(listItemTokens.ToArray())) return false;

                    listItemStart = listItemEnd + 1;
                }
            }

            return true;
        }

        public static SymbolNode Parse(Scopes.Scope scope, Token token) {
            var symbol = GetSymbol(scope, token);
            if (symbol == null) return null;
            return new SymbolNode(token, symbol);
        }

        public static Types.Type GetSymbol(Scopes.Scope scope, Token token) {
            var simpleSymbol = scope.GetSymbol(token.Value);
            if (simpleSymbol != null) return simpleSymbol;

            var compositeTypeTokens = new List<Token>();
            var parts = token.Value.Split(".");

            if (parts.Length <= 1) throw new Exceptions.UndefinedSymbolException(token, token.Value, scope);

            foreach (var p in parts) {
                compositeTypeTokens.Add(new Token {
                    Column = token.Column,
                    Line = token.Line,
                    FilePath = token.FilePath,
                    Value = p
                });
                compositeTypeTokens.Add(new Token {
                    Column = token.Column,
                    Line = token.Line,
                    FilePath = token.FilePath,
                    Value = "."
                });
            }
            compositeTypeTokens.RemoveAt(compositeTypeTokens.Count - 1);

            var symbolExpression = Expression.Parse(scope, compositeTypeTokens.ToArray());

            return new Types.CompositeType(
                scope,
                symbolExpression.Root as Operators.OperatorAccess
            );
        }
    }
}