using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BCake.Parser.Exceptions;
using BCake.Parser.Syntax.Expressions.Nodes;
using BCake.Parser.Syntax.Expressions.Nodes.Operators;

namespace BCake.Parser.Syntax.Types {
    public class FunctionType : ComplexType {
        /// <summary>
        /// Used for native function implementations
        /// </summary>
        public virtual bool ExpectsThisArg => false;
        
        public Type Parent { get; protected set; }
        public Type ReturnType { get; protected set; }
        public override string FullName { get { return Parent == null ? Name : Parent.FullName + ":" + Name; } }
        public ScopeNode Root { get; protected set; }
        public ParameterType[] Parameters { get; protected set; }
        public FunctionType[] Overloads { get; set; } = new FunctionType[] { };
        public bool IsPrototype => Tokens == null;


        protected FunctionType(Scopes.Scope scope, Type returnType, string name, ParameterType[] parameters)
                : base(scope, name, Access.@public) {
            ReturnType = returnType;
            Parameters = parameters;
        }

        public FunctionType(Token token, Type parent, Access access, Type returnType, string name, ParameterType[] parameters, Token[] tokens)
                : base(null, name, access) {
            DefiningToken = token;
            Parent = parent;
            ReturnType = returnType;
            Parameters = parameters;
            Tokens = tokens;

            Scope = new Scopes.Scope(parent.Scope, this);
            foreach (var p in parameters) p.SetScope(Scope);
            Scope.Declare(parameters);

            var argListStr = string.Join(", ", parameters.Select(a => $"{a.Type.FullName} {a.Name}"));
        }

        public static ParameterType[] ParseArgumentList(Scopes.Scope scope, Token[] tokens) {
            bool propertyInitializer = false;
            List<Token> propertyInitializerTokens = null;

            string name = null;
            Type type = null;
            var arguments = new List<ParameterType>();

            for (int i = 0; i < tokens.Length; ++i) {
                var token = tokens[i];

                switch (token.Value) {
                    case ")":
                    case ",":
                        if (propertyInitializer) {
                            var exp = Expressions.Expression.Parse(scope, propertyInitializerTokens.ToArray());

                            var opAccess = exp.Root as OperatorAccess;
                            if (opAccess == null) {
                                throw new InvalidParameterPropertyInitializerException(propertyInitializerTokens.ToArray());
                            }

                            var member = opAccess.ReturnSymbol as MemberVariableType;
                            if (member == null) {
                                throw new InvalidParameterPropertyInitializerException(propertyInitializerTokens.ToArray());
                            }

                            arguments.Add(new InitializerParameterType(propertyInitializerTokens.FirstOrDefault(), member));
                            propertyInitializer = false;
                        } else {
                            if (type == null && name == null) break;
                            if (type == null || name == null) throw new UnexpectedTokenException(token);
                            arguments.Add(new ParameterType(token, type, name));
                            name = null;
                            type = null;
                        }
                        break;

                    case "this":
                        propertyInitializer = true;
                        propertyInitializerTokens = new List<Token>() { token };
                        break;

                    default:
                        if (propertyInitializer) {
                            propertyInitializerTokens.Add(token);
                            break;
                        }

                        if (!SymbolNode.CouldBeIdentifier(token.InArray()))
                            throw new UnexpectedTokenException(token);

                        if (type == null) {
                            type = scope.GetSymbol(token.Value)
                                ?? throw new Exceptions.UndefinedSymbolException(token, token.Value, scope);
                        } else if (name == null) {
                            name = token.Value;
                        } else {
                            throw new UnexpectedTokenException(token);
                        }
                        break;
                }
            }

            return arguments.ToArray();
        }

        public override void ParseInner() {
            if (IsPrototype) return;

            Root = ScopeNode.Parse(DefiningToken, Scope, Tokens);

            foreach (var o in Overloads) o.Root = ScopeNode.Parse(o.DefiningToken, o.Scope, o.Tokens);
        }

        public FunctionType GetMatchingOverload(Expressions.Nodes.Functions.ArgumentsNode.Argument[] args)
        {
            return GetMatchingOverload(args.Select(a => a.Expression.ReturnType).ToArray());
        }
        public FunctionType GetMatchingOverload(Type[] argTypes)
        {
            var overloads = Overloads.Prepend(this);
            foreach (var o in overloads)
                if (!o.ParameterListDiffers(argTypes))
                    return o;
            return null;
        }

        public bool ParameterListDiffers(FunctionType other) {
            return ParameterListDiffers(other.Parameters.Select(p => p.Type));
        }
        public bool ParameterListDiffers(IEnumerable<Type> _arguments) {
            var arguments = _arguments.ToList();

            if (Parameters.Length != arguments.Count) return true;

            for (var i = 0; i < Parameters.Length; ++i) {
                if (Parameters[i].Type.FullName != arguments[i].FullName)
                {
                    var typeParam = Parameters[i].Type;
                    var typeArg = arguments[i];

                    if (!(typeParam is InheritableType iParam && typeArg is InheritableType iArg && InheritableType.IsDescendantOf(iParam, iArg)))
                        return true;
                }
            }

            return false;
        }

        public class ParameterType : Type {
            public Type Type { get; protected set; }
            public ParameterType(Token token, Type type, string name)
                : base(null, name, type.DefaultValue) {
                DefiningToken = token;
                Type = type;
            }

            public void SetScope(Scopes.Scope s) {
                Scope = s;
            }
        }

        public class InitializerParameterType : ParameterType {
            public MemberVariableType Member { get; protected set; }
            public InitializerParameterType(Token token, MemberVariableType member)
                    : base(token, member.Type, member.Name) {
                Member = member;
            }
        }
    }
}