using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BCake.Parser.Syntax.Expressions.Nodes;
using BCake.Parser.Syntax.Expressions.Nodes.Operators;
using BCake.Parser.Syntax.Expressions.Nodes.Operators.Comparison;
using BCake.Parser.Syntax.Expressions.Nodes.Operators.Logical;
using BCake.Parser.Syntax.Expressions.Nodes.Operators.Syntactical;
using BCake.Parser.Exceptions;

namespace BCake.Parser.Syntax.Expressions {
    public class Expression {
        private static readonly string[] bracketsOpen = new string[] { "(", "[", "{", "<" };
        private static readonly string[] bracketsClose = new string[] { ")", "]", "}", ">" };
        private static readonly Type[] OperatorPrecedence = new Type[] {
            // functions
            typeof(OperatorReturn),

            // assigment
            typeof(OperatorAssign),

            // logical
            typeof(OperatorLogicalOr),
            
            // comparison
            typeof(OperatorGreater),
            typeof(OperatorGreaterEqual),
            typeof(OperatorSmaller),
            typeof(OperatorSmallerEqual),
            typeof(OperatorEqual),
            typeof(OperatorNotEqual),

            // arithmetic
            typeof(OperatorPlus),
            typeof(OperatorMinus),
            typeof(OperatorMultiply),
            typeof(OperatorDivide),

            typeof(OperatorCast),

            typeof(OperatorNew),
            typeof(OperatorIndex),
            typeof(OperatorInvoke),
            typeof(OperatorAccess),

            // syntactical
            typeof(OperatorTypeArgument),
        };
        private static readonly string[] OperatorSymbols = OperatorPrecedence
            .Select(op => Operator.GetOperatorMetadata(op).Symbol)
            .ToArray();
        public static readonly string[] OperatorOverloadableNames = OperatorPrecedence
            .Select(op => Operator.GetOperatorMetadata(op))
            .Where(meta => meta.OverloadableName != null)
            .Select(meta => meta.OverloadableName)
            .ToArray();

        public Token DefiningToken { get; protected set; }
        public Node Root { get; protected set; }
        public Scopes.Scope Scope { get; protected set; }
        public Types.Type ReturnType {
            get => Root.ReturnType;
        }

        public Expression(Token token, Scopes.Scope scope, Nodes.Node root) {
            DefiningToken = token;
            Root = root;
            Scope = scope;
        }

        public static Expression Parse(Scopes.Scope scope, Token[] tokens, Scopes.Scope typeSource = null) {
            if (typeSource == null) typeSource = scope;
            if (tokens.Length < 1) return null;

            var leftSideTokens = tokens.Take(tokens.Length - 1);
            var leftSideJoined = leftSideTokens.SelectValues().JoinString("");

            var couldBeComposite = couldBeCompositeType(tokens.Take(tokens.Length - 1), out var _);
            var couldBeGeneric = couldHaveTypeArgs(tokens.Take(tokens.Length - 1), out var _potentialCompositeTypeTokens, out var _potentialTypeArgsTokens);

            if (couldBeGeneric) {
                leftSideTokens = _potentialCompositeTypeTokens;
                leftSideJoined = leftSideTokens.SelectValues().JoinString("");
            }

            if (tokens.Length >= 2
                && (couldBeComposite || couldBeGeneric)
                // are these additional checks still necessary, now that we know exactly if its a type or not?
                && SymbolNode.CouldBeIdentifier(tokens.Last().InArray())
                && SymbolNode.CouldBeIdentifier(leftSideTokens.ToArray())
                && !OperatorSymbols.Contains(leftSideJoined)
            ) {
                var tLast = tokens.Last();
                Types.Type symbol;

                if (tokens.Length == 2) {
                    symbol = Types.CompositeType.Resolve(SymbolNode.GetSymbol(typeSource, tokens[0]));
                }
                else {
                    symbol = Expression.Parse(
                        scope,
                        tokens.Take(tokens.Length - 1).ToArray(),
                        typeSource
                    ).ReturnType;
                }

                if (symbol == null) throw new UndefinedSymbolException(tokens[0], tokens[0].Value, scope);

                if (symbol is Types.ClassType or Types.InterfaceType or Types.PrimitiveType) {
                    var newLocalVar = new Types.LocalVariableType(tLast, scope, symbol, tLast.Value);
                    scope.Declare(newLocalVar);
                    return new Expression(tLast, scope, SymbolNode.Parse(scope, tLast));
                }
            }

            // an expression within parentheses
            if (tokens.First().Value == "(" && tokens.Last().Value == ")")
            {
                if (ParserHelper.FindClosingScope(tokens, 0) == tokens.Length - 1)
                    return Expression.Parse(scope, tokens.Skip(1).SkipLast(1).ToArray(), typeSource);
            }

            Exception temporarilyIgnoredException = null;

            for (int i = 0; i < OperatorPrecedence.Length; ++i) {
                var op = OperatorPrecedence[i];
                var opMeta = Operator.GetOperatorMetadata(op) ?? throw new Exception("Invalid operator definition - no metadata provided");
                var reverse = opMeta.Direction == OperatorAttribute.EvaluationDirection.RightToLeft;

                var tempTokens = tokens;
                if (reverse) tempTokens = tempTokens.Reverse().ToArray();

                var bracketIndent = 0;
                var opPos = tempTokens
                    .Select((t, index) => {
                        var bracketIndentBefore = bracketIndent;
                        if (bracketsOpen.ToList().Contains(t.Value)) bracketIndent += reverse ? -1 : 1;
                        if (bracketsClose.ToList().Contains(t.Value)) bracketIndent += reverse ? 1 : -1;

                        return new {
                            t.Value,
                            index = index + 1,
                            bracketIndent = reverse ? bracketIndent : bracketIndentBefore
                        };
                    })
                    .TakeWhile(pair => pair.Value.Trim() != opMeta.Symbol || pair.bracketIndent != 0)
                    .Select(pair => pair.index)
                    .LastOrDefault();

                if (opPos >= tempTokens.Length) continue;
                if (reverse) opPos = tempTokens.Length - 1 - opPos;

                var tokensLeft = tokens.Take(opPos).ToArray();
                var tokensRight = tokens.Skip(opPos + 1).ToArray();

                // if (reverse) {
                //     var temp = tokensLeft;
                //     tokensLeft = tokensRight.Reverse().ToArray();
                //     tokensRight = temp.Reverse().ToArray();
                // }

                var handler = GetParsePreflight(op);
                if (handler != null) {
                    var info = new OperatorParseInfo {
                        OperatorPosition = opPos,
                        Tokens = tokens,
                        TokensLeft = tokensLeft,
                        TokensRight = tokensRight
                    };

                    // if the handler returns false, we continue with the next operator
                    if (!(bool)handler.Invoke(null, new object[] { info })) continue;
                }

                if (opMeta.Left == OperatorAttribute.ParameterType.None) {
                    if (opPos > 0) throw new UnexpectedTokenException(tempTokens[opPos]);
                    return new Expression(
                        tempTokens[0],
                        scope,
                        Operator.Parse(scope, typeSource, op, tempTokens[0], new Token[] { }, tempTokens.Skip(1).ToArray())
                    );
                }
                else {
                    Node parsedOperator = null;

                    try {
                        parsedOperator = Operator.Parse(scope, typeSource, op, tempTokens[0], tokensLeft, tokensRight);
                    } catch (Exception e) {
                        if (!opMeta.SuppressErrors) throw;
                        
                        temporarilyIgnoredException = e;
                        continue;
                    }

                    return new Expression(
                        tempTokens[0], // TODO this index isn't always 0!
                        scope,
                        parsedOperator
                    );
                }
            }

            if (tokens.Length == 1) {
                Nodes.Node node;
                var t = tokens[0];

                if ((node = Nodes.Value.ValueNode.Parse(t)) != null) return new Expression(t, scope, node);
                if ((node = SymbolNode.Parse(typeSource, t)) != null) return new Expression(t, scope, node);
                else throw new Exceptions.UndefinedSymbolException(t, t.Value, scope);
            }

            // if (temporarilyIgnoredException != null) throw temporarilyIgnoredException;

            // this exception might be deliberate. Some parsing operations fail before
            // backtracking and working out, so this doesn't always mean something has gone wrong
            throw new Exceptions.UnexpectedTokenException(tokens[0]);
        }

        /// <summary>
        /// Returns whether or not the given list of tokens might represent a composite type (e.g. A.B.C).
        /// Does not evaluate the type but loosely checks if the format fulfils basic requirements for further parsing.
        /// </summary>
        private static bool couldBeCompositeType(IEnumerable<Token> _tokens, out Token unexpectedToken) {
            var tokens = _tokens.ToArray();

            if (tokens.Length % 2 != 1) {
                unexpectedToken = tokens.LastOrDefault();
                return false;
            }

            var unexpectedTokens = tokens
                .Select((token, index) => new { token, index })
                .Where(pair =>
                    (pair.index % 2 == 1 && pair.token.Value != ".")
                    || (pair.index % 2 == 0 && !SymbolNode.CouldBeIdentifier(pair.token.InArray()))
                )
                .Select(pair => pair.token)
                .ToArray();
            
            if (unexpectedTokens.Length < 1) {
                unexpectedToken = null;
                return true;
            }

            unexpectedToken = unexpectedTokens.First();
            return false;
        }

        /// <summary>
        /// Returns whether or not the given list of tokens might contain a generic type argument list,
        /// and whether the type before that list could be a composite type.
        /// Does not evaluate the args list but loosely checks if the format fulfils basic requirements for further parsing.
        /// </summary>
        private static bool couldHaveTypeArgs(IEnumerable<Token> _tokens, out IEnumerable<Token> compositeType, out IEnumerable<Token> typeArgs) {
            var tokens = _tokens.ToArray();
            compositeType = null;
            typeArgs = null;

            var openingBracketCandidate = tokens
                .Select((token, index) => new { token, index = index as int? })
                .TakeWhile(pair => pair.token.Value != "<")
                .Select(pair => pair.index + 1)
                .FirstOrDefault();
            if (!openingBracketCandidate.HasValue) return false;
            if (openingBracketCandidate.Value >= tokens.Length) return false;

            var hasOpeningBracket = tokens[openingBracketCandidate.Value].Value == "<";
            if (!hasOpeningBracket) return false;

            var closingBracket = ParserHelper.FindClosingScope(tokens, openingBracketCandidate.Value);
            if (closingBracket < 0) return false;
            if (closingBracket != tokens.Length - 1) return false;

            var beforeBrackets = tokens.Take(openingBracketCandidate.Value);
            var couldBeComposite = couldBeCompositeType(beforeBrackets, out var _);

            compositeType = beforeBrackets;
            typeArgs = tokens.Skip(openingBracketCandidate.Value + 1).Take(closingBracket - openingBracketCandidate.Value - 1);
            return true;
        }

        private static System.Reflection.MethodInfo GetParsePreflight(Type op) {
            var methods = op.GetMethods();
            foreach (var m in methods) {
                if (!m.IsStatic) continue;

                var attr = m.GetCustomAttributes(
                    typeof(OperatorParsePreflight),
                    true
                ).FirstOrDefault() as OperatorParsePreflight;

                if (attr != null) return m;
            }

            return null;
        }
    }
}