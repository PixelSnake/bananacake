using System.Linq;
using BCake.Parser.Exceptions;

namespace BCake.Parser.Validators {
    public class TokenTypeValidator {
        private TokenType[] tokenTypes;
        private MemberRule[] memberRules;
        private Token token;

        public TokenTypeValidator(params TokenType[] _tokenTypes) {
            tokenTypes = _tokenTypes;
        }

        public TokenTypeValidator(Token _token, params TokenType[] _tokenTypes) : this(_tokenTypes) {
            token = _token;
        }

        public TokenTypeValidator WithToken(Token token) {
            return new TokenTypeValidator(token, tokenTypes);
        }

        public TokenTypeValidator WithMemberRules(params MemberRule[] _memberRules)
        {
            memberRules = _memberRules;
            return this;
        }

        public bool HasMemberRule(MemberRule rule)
        {
            if (memberRules == null) return false;
            return memberRules.Contains(rule);
        }

        public void Validate(TokenType type) {
            if (!tokenTypes.Contains(type)) throw new UnexpectedTokenException(token);
        }

        public static void guardTokenType(TokenType type, TokenType[] allowedTypes, Token token) {
            if (!allowedTypes.Contains(type)) throw new UnexpectedTokenException(token);
        }
    }

    public enum TokenType {
        unknown = 0,
        @namespace,
        @class,
        @function,
        @interface,
        @cast,
        @variable,
    }

    public enum MemberRule
    {
        NoPrototypes = 0b1
    }
}