using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using BCake.Parser.Exceptions;
using BCake.Parser.Syntax;
using BCake.Parser.Syntax.Expressions;
using BCake.Parser.Syntax.Expressions.Nodes;
using BCake.Parser.Syntax.Expressions.Nodes.Operators;
using BCake.Parser.Syntax.Scopes;
using BCake.Parser.Syntax.Types;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Validators;

namespace BCake.Parser {
    public class Parser {
        private static string separators = @"[\(\)\[\].,:;{}<>""]";
        private static string rxSeparators = $"(\\s*({separators})\\s*|\\s+({separators})?\\s*)";

        public static MemberRule[] RootRules = new MemberRule[] { MemberRule.NoPrototypes };

        public string Filename { get; private set; }
        private Token[] tokens;

        private Parser() {}

        public static Parser FromFile(string filename)
        {
            var parser = new Parser
            {
                Filename = filename
            };

            var content = File.ReadAllText(parser.Filename);
            parser.tokens = parser.SplitTokens(content);

            return parser;
        }

        public static Parser FromMemory(string text)
        {
            var parser = new Parser
            {
                Filename = "<from memory>"
            };
            parser.tokens = parser.SplitTokens(text);

            return parser;
        }

        public void Parse(Namespace globalNamespace)
        {
            ParseRoot(globalNamespace);

            var namespaces = globalNamespace.Scope.AllMembers.Select(elem => elem.Value).Where(elem => elem is Namespace).Cast<Namespace>();
            foreach (var ns in namespaces) ns.ParseInner();

            namespaces = namespaces.Append(globalNamespace);
            foreach (var ns in namespaces)
            {
                foreach (var m in ns.Scope.AllMembers.Select(elem => elem.Value).Where(elem => elem is ComplexType).Where(elem => !(elem is Namespace)).Cast<ComplexType>())
                {
                    m.ParseInner();
                }
            }
            foreach (var ns in namespaces)
            {
                foreach (var m in ns.Scope.AllMembers.Where(elem => elem.Value is ClassType))
                {
                    foreach (var f in m.Value.Scope.AllMembers.Select(elem => elem.Value).Where(elem => elem is FunctionType).Cast<FunctionType>())
                    {
                        f.ParseInner();
                    }
                }
            }
            foreach (var ns in namespaces)
            {
                foreach (var m in ns.Scope.AllMembers.Where(elem => elem.Value is ClassType))
                {
                    (m.Value as ClassType).ValidateBaseTypeConstraints();
                }
            }
        }

        private Token[] SplitTokens(string content) {
            var parts = Regex.Matches(content, rxSeparators).Cast<Match>().ToArray();
            var tokens = new List<Token>();
            int line = 1, lineBegin = 0;

            for (int i = 0; i <= parts.Length; ++i) {
                int posBegin, length;

                if (i == 0) {
                    posBegin = 0;
                    length = parts[i].Index;
                }
                else if (i == parts.Length) {
                    posBegin = parts[i - 1].Index + parts[i - 1].Length;
                    length = content.Length - posBegin;
                }
                else {
                    posBegin = parts[i - 1].Index + parts[i - 1].Length;
                    length = parts[i].Index - posBegin;
                }

                tokens.Add(new Token {
                    Value = content.Substring(posBegin, length),
                    FilePath = Filename,
                    Line = line,
                    Column = posBegin - lineBegin + 1
                });
                if (i < parts.Length) {
                    var separator = parts[i].Value.Trim();
                    if (separator == "\"") {
                        // beginning of string literal
                        var partUntrimmed = parts[i].Value;
                        var trimmedFront = partUntrimmed.Length - partUntrimmed.TrimStart().Length;
                        var str = ParserHelper.FindString(content, parts[i].Index + trimmedFront, out var lineBreaks, out var column, out var end);
                        if (str == null) throw new EndOfFileException(new Token {
                            FilePath = Filename,
                            Line = line,
                            Column = lineBreaks > 0 ? column : parts[i].Index - lineBegin + 1
                        });

                        tokens.Add(new Token {
                            Value = str,
                            FilePath = Filename,
                            Line = line,
                            Column = lineBreaks > 0 ? column : parts[i].Index - lineBegin + 1
                        });

                        line += lineBreaks;
                        if (lineBreaks > 0) lineBegin = end - column;

                        while (parts[i].Index < end) i++;
                        i--;
                    }
                    else {
                        if (separator.Length > 0) tokens.Add(new Token {
                            Value = separator,
                            FilePath = Filename,
                            Line = line,
                            Column = parts[i].Index - lineBegin + 1
                        });

                        var linebreaks = parts[i].Value.Count(c => c == '\n');
                        if (linebreaks > 0) {
                            line += linebreaks;
                            lineBegin = parts[i].Index + parts[i].Value.LastIndexOf('\n') + 1;
                        }
                    }
                }
            }

            return tokens.Where(t => t.Value.Trim().Length > 0).ToArray();
        }

        private void ParseRoot(Namespace globalNamespace) {
            ParseTypes(globalNamespace.Scope, tokens, new TokenTypeValidator(TokenType.@namespace, TokenType.@class, TokenType.function, TokenType.@interface).WithMemberRules(RootRules));
        }

        public static void ParseTypes(Scope targetScope, Token[] tokens, TokenTypeValidator _tokenTypeValidator) {
            Access access = Access.@default;
            TokenType type = TokenType.unknown;
            string name = null;
            Token definingToken = null;
            Syntax.Types.Type valueType = null;
            FunctionType.ParameterType[] argList = null;
            Token[] genericParamListTokens = null;
            Token[][] baseTypeListTokens = null;

            for (int i = 0; i < tokens.Length; ++i) {
                var token = tokens[i];
                var tokenTypeValidator = _tokenTypeValidator.WithToken(token);

                switch (token.Value) {
                    case "public":
                    case "protected":
                    case "private":
                        if (access != Access.@default) throw new UnexpectedTokenException(token);
                        access = accessFromString(token);
                        break;

                    case "void":
                        tokenTypeValidator.Validate(TokenType.function);
                        valueType = NullValueNode.Type;
                        break;

                    case "namespace":
                    case "class":
                    case "cast":
                    case "interface":
                        type = tokenTypeFromToken(token);
                        tokenTypeValidator.Validate(type);
                        break;

                    case "(":
                        if (type == TokenType.unknown) type = TokenType.function;
                        if (type == TokenType.function || type == TokenType.cast) {
                            tokenTypeValidator.Validate(type);

                            switch (type) {
                                case TokenType.function: {
                                        if (name != null && name.StartsWith("operator_")) {
                                            var @operator = name.Substring("operator_".Length);
                                            if (!Expression.OperatorOverloadableNames.Contains(@operator)) throw new InvalidOperatorDefinitionException(
                                                token,
                                                $"Cannot create overload for unknown operator \"{ @operator }\" - function names may not begin with \"operator\" because it is a keyword used for operator overloading"
                                            );

                                            // setting the name like this defines this function as an operator overload, no further action required
                                            name = $"!operator_{ @operator.ToLower() }";
                                            definingToken = token;
                                        }

                                        parseFunction(
                                            valueType,
                                            name, out name,
                                            out argList,
                                            targetScope,
                                            i, out i,
                                            tokens
                                        );
                                        break;
                                    }

                                case TokenType.cast:
                                    parseCaster(
                                        valueType,
                                        name, out name,
                                        out argList,
                                        targetScope,
                                        i, out i,
                                        tokens
                                    );
                                    definingToken = token;
                                    break;
                            }
                        }
                        else throw new UnexpectedTokenException(token);
                        break;

                    case "<":
                        if (type == TokenType.unknown) type = TokenType.@class;
                        if (type == TokenType.@class) {
                            tokenTypeValidator.Validate(type);

                            var beginGenericList = i;
                            i = ParserHelper.FindClosingScope(tokens, beginGenericList);
                            genericParamListTokens = tokens.Skip(beginGenericList + 1).Take(i - beginGenericList).ToArray();
                        }
                        break;

                    case "{":
                        {
                            if (type == TokenType.unknown || name == null) throw new UnexpectedTokenException(token);
                            if (access == Access.@default) access = Access.@public;

                            var beginScope = i;
                            i = ParserHelper.FindClosingScope(tokens, i);

                            if (genericParamListTokens != null && type != TokenType.@class && type != TokenType.@interface)
                            {
                                throw new UnexpectedTypeParameterException(token);
                            }

                            if (type == TokenType.@namespace) {
                                targetScope.Declare(
                                    new Namespace(
                                        targetScope,
                                        access,
                                        name,
                                        tokens.Skip(beginScope + 1).Take(i - beginScope - 1).ToArray()
                                    )
                                );
                            }
                            else if (type == TokenType.@class) {
                                ClassType classType;
                                if (genericParamListTokens != null) {
                                    classType = new GenericClassType(
                                        targetScope,
                                        access,
                                        name,
                                        definingToken,
                                        tokens.Skip(beginScope + 1).Take(i - beginScope - 1).ToArray(),
                                        genericParamListTokens
                                    );
                                } else {
                                    classType = new ClassType(
                                        targetScope,
                                        access,
                                        name,
                                        definingToken,
                                        tokens.Skip(beginScope + 1).Take(i - beginScope - 1).ToArray(),
                                        baseTypeListTokens
                                    );
                                }

                                targetScope.Declare(classType);
                                genericParamListTokens = null;
                            }
                            else if (type == TokenType.function || type == TokenType.cast) {
                                var newFunction = new FunctionType(
                                    definingToken,
                                    targetScope.Type,
                                    access,
                                    valueType,
                                    name,
                                    argList,
                                    tokens.Skip(beginScope + 1).Take(i - beginScope - 1).ToArray()
                                );
                                targetScope.Declare(newFunction);
                                argList = null;
                            }
                            else if (type == TokenType.@interface) {
                                if (access != Access.@public && access != Access.@default)
                                {
                                    throw new InvalidVisibilityModifierException("interfaces must be have public or default visibility", definingToken);
                                }

                                var newInterface = new InterfaceType(
                                    definingToken,
                                    targetScope,
                                    name,
                                    tokens.Skip(beginScope + 1).Take(i - beginScope - 1).ToArray()
                                );
                                targetScope.Declare(newInterface);
                            }

                            access = Access.@default;
                            type = TokenType.unknown;
                            name = null;
                            valueType = null;
                        }
                        break;

                    case ";":
                        if (type == TokenType.unknown) {
                            type = TokenType.variable;
                            tokenTypeValidator.Validate(type);
                            if (name == null || valueType == null) throw new UnexpectedTokenException(token);

                            var newMember = new MemberVariableType(
                                tokens[i - 1],
                                targetScope.Type,
                                access,
                                valueType,
                                name
                            );
                            targetScope.Declare(newMember);

                            access = Access.@default;
                            name = null;
                            type = TokenType.unknown;
                            valueType = null;
                        }
                        else if (!_tokenTypeValidator.HasMemberRule(MemberRule.NoPrototypes) && (type == TokenType.function || type == TokenType.cast))
                        {
                            var newFunction = new FunctionType(
                                    definingToken,
                                    targetScope.Type,
                                    access,
                                    valueType,
                                    name,
                                    argList,
                                    null
                                );
                            targetScope.Declare(newFunction);
                            argList = null;
                        } else throw new UnexpectedTokenException(token);
                        break;

                    case ":": {
                            if (type != TokenType.@class) throw new UnexpectedTokenException(token);

                            baseTypeListTokens = ClassType.SplitInheritanceListTokens(tokens, i + 1, targetScope, out var inheritanceListEnd);
                            i = inheritanceListEnd - 1;

                            break;
                        }

                    default: {
                            var temp = "";

                            while (true) {
                                if (SymbolNode.CouldBeIdentifier(token.InArray(), out var m)) temp += m.Value;
                                else throw new UnexpectedTokenException(token);

                                if (i + 2 < tokens.Length && tokens[i + 1].Value == ".") {
                                    temp += ".";
                                    i += 2;
                                    token = tokens[i];
                                }
                                else break;
                            }

                            if (type != TokenType.@class && type != TokenType.@interface && type != TokenType.@namespace && valueType == null) {
                                valueType = targetScope.GetSymbol(temp) ?? throw new UndefinedSymbolException(token, temp, targetScope);
                            }
                            else {
                                if (name != null) throw new UnexpectedTokenException(token);
                                name = temp;
                                definingToken = token;
                            }

                            break;
                        }
                }
            }
        }

        private static Access accessFromString(Token t) {
            switch (t.Value) {
                case "public": return Access.@public;
                case "private": return Access.@private;
                default: throw new UnexpectedTokenException(t);
            }
        }

        private static void parseFunction(
            Syntax.Types.Type valueType,
            string _name, out string name,
            out FunctionType.ParameterType[] argList,
            Scope targetScope,
            int _tokenIndex, out int tokenIndex,
            Token[] tokens
        ) {
            tokenIndex = _tokenIndex;
            name = _name;

            var token = tokens[tokenIndex];

            if (valueType == null) throw new UnexpectedTokenException(token);

            if (name == null && valueType == targetScope.Type) {
                name = "!constructor"; // the ! is used as a kind of "escape" because it is impossible for a user created function to contain a ! in its name
            }
            else if (name == null) {
                throw new UnexpectedTokenException(token);
            }

            var argListBegin = tokenIndex;
            tokenIndex = ParserHelper.FindClosingScope(tokens, tokenIndex);

            // +1 in Take to include closing bracket, makes things easier in the parse method
            argList = FunctionType.ParseArgumentList(targetScope, tokens.Skip(argListBegin + 1).Take(tokenIndex - argListBegin).ToArray());

            if (name != "!constructor") {
                foreach (var param in argList) {
                    if (param is FunctionType.InitializerParameterType) {
                        throw new Exceptions.InvalidParameterPropertyInitializerException(
                            param as FunctionType.InitializerParameterType,
                            "initializer parameters are only allowed in constructors"
                        );
                    }
                }
            }
        }

        private static void parseCaster(
            Syntax.Types.Type valueType,
            string _name, out string name,
            out FunctionType.ParameterType[] argList,
            Scope targetScope,
            int _tokenIndex, out int tokenIndex,
            Token[] tokens
        ) {
            tokenIndex = _tokenIndex;
            name = $"!as_{ valueType.Name }";

            parseFunction(
                valueType,
                name, out name,
                out argList,
                targetScope,
                tokenIndex, out tokenIndex,
                tokens
            );

            if (argList.Length > 0) throw new Exceptions.InvalidCasterDefinitionException(
                argList.First().DefiningToken,
                "Casters may not have parameters"
            );
        }

        //private static void parseInheritanceList(
            
        //)
        //{

        //}

        public static TokenType tokenTypeFromToken(Token token) {
            switch (token.Value) {
                case "namespace": return TokenType.@namespace;
                case "class": return TokenType.@class;
                case "function": return TokenType.@function;
                case "interface": return TokenType.@interface;
                case "cast": return TokenType.@cast;
                case "variable": return TokenType.@variable;
                default: throw new UnexpectedTokenException(token);
            }
        }
    }
}