using BCake.Parser.Exceptions;
using BCake.Parser.Syntax.Expressions;
using BCake.Parser.Syntax.Expressions.Nodes;
using BCake.Parser.Syntax.Scopes;
using BCake.Parser.Validators;
using System.Collections.Generic;
using System.Linq;

namespace BCake.Parser.Syntax.Types {
    public class ClassType : InheritableType {
        public override string FullName { get => Scope.FullName; }

        public ClassType(Scope parent, Access access, string name, Token definingToken, Token[] tokens)
            : base(null, name, access) {
            Access = access;
            Tokens = tokens;
            DefiningToken = definingToken;

            Scope = new Scope(parent, this);

            var thisMember = new MemberVariableType(
                DefiningToken,
                this,
                Access.@private,
                this,
                "this"
            );
            Scope.Declare(thisMember, "this");
        }

        public ClassType(Scope parent, Access access, string name, Token definingToken, Token[] tokens, Token[][] baseTypeTokens) : this(parent, access, name, definingToken, tokens)
        {
            this.baseTypeTokens = baseTypeTokens;
        }

        public override void ParseInner() {
            var validator = new TokenTypeValidator(TokenType.function, TokenType.cast, TokenType.variable)
                .WithMemberRules(MemberRule.NoPrototypes);

            ParseBaseTypes();

            Parser.ParseTypes(Scope, Tokens, validator);
        }

        public static Token[][] SplitInheritanceListTokens(Token[] tokens, int tokenStartPos, Scope scope, out int listEnd)
        {
            var returnTypeTokens = new List<Token[]>();
            var tokenIndex = tokenStartPos;

            while (tokenIndex < tokens.Length)
            {
                var listItemEnd = ParserHelper.FindListItemEnd(tokens, tokenIndex, new string[] { "{" });
                var listItemTokens = tokens.Skip(tokenIndex).Take(listItemEnd - tokenIndex).ToArray();
                returnTypeTokens.Add(listItemTokens);

                if (tokens[listItemEnd].Value == "{")
                {
                    tokenIndex = listItemEnd;
                    break;
                }

                tokenIndex = listItemEnd + 1;
            }

            listEnd = tokenIndex;
            return returnTypeTokens.ToArray();
        }

        public virtual FunctionType GenerateDefaultConstructor()
        {
            var function = new FunctionType(DefiningToken, this, Access.@public, this, "!constructor", new FunctionType.ParameterType[] { }, new Token[] { });
            Scope.Declare(function);
            return function;
        }
    }
}