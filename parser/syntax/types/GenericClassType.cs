using System.Collections;
using System.Collections.Generic;
using BCake.Parser.Syntax.Expressions.Nodes;
using BCake.Parser.Exceptions;

namespace BCake.Parser.Syntax.Types {
    public class GenericClassType : ClassType {
        public GenericTypeParameter[] GenericTypeParameters;
        public string GenericName;

        public GenericClassType(Scopes.Scope parent, Access access, string name, Token definingToken, Token[] tokens, Token[] genericParamsList)
            : base(parent, access, name, definingToken, tokens) {
            if (genericParamsList != null) {
                GenericTypeParameters = ParseTypeParameterList(Scope, genericParamsList);
                GenericName = Name;
                // Can't be changed so easily because then it's no longer accessible by the plain name
                // Name = $"{ name }<{ string.Join(", ", GenericTypeParameters.Select(p => p.Name)) }>";

                declareTypeArguments();
            }
        }

        protected GenericClassType(Scopes.Scope parent, Access access, string name, Token definingToken, GenericTypeParameter[] genericParamsList)
            : this(parent, access, name, definingToken, new Token[] {}, null) {
            GenericTypeParameters = genericParamsList;
            GenericName = name;

            declareTypeArguments();
        }

        private void declareTypeArguments() {
            for (int i = 0; i < GenericTypeParameters.Length; i++)
                Scope.Declare(new PlaceholderType(Scope, GenericTypeParameters[i].Name, GenericTypeParameters[i].DefiningToken));
        }

        private static GenericTypeParameter[] ParseTypeParameterList(Scopes.Scope targetScope, Token[] tokens) {
            var paramList = new List<GenericTypeParameter>();
            Token currentTypeToken = null;

            for (int i = 0; i < tokens.Length; ++i) {
                var token = tokens[i];

                switch (token.Value) {
                    case ">":
                    case ",":
                        if (!SymbolNode.CouldBeIdentifier(currentTypeToken.InArray())) throw new UnexpectedTokenException(currentTypeToken);
                        paramList.Add(new GenericTypeParameter(currentTypeToken, targetScope, currentTypeToken.Value));
                        currentTypeToken = null;
                        break;

                    default:
                        if (currentTypeToken == null) currentTypeToken = token;
                        else throw new UnexpectedTokenException(token);
                        break;

                }
            }

            return paramList.ToArray();
        }

        public class GenericTypeParameter : Type {
            public GenericTypeParameter(Token token, Scopes.Scope scope, string name)
                : base(scope, name, null) {
                DefiningToken = token;
            }
        }
    }
}