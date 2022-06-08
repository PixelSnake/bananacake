using BCake.Parser.Errors;
using BCake.Parser.Exceptions;
using BCake.Parser.Syntax.Expressions;
using BCake.Parser.Syntax.Expressions.Nodes;
using BCake.Parser.Syntax.Scopes;
using System.Collections.Generic;
using System.Linq;

namespace BCake.Parser.Syntax.Types
{
    public abstract class InheritableType : ComplexType
    {
        public InheritableType[] BaseTypes { get; protected set; }
        protected Token[][] baseTypeTokens;

        public InheritableType(Scope scope, string name, Access access) : base(scope, name, access) { }

        protected void ParseBaseTypes()
        {
            if (baseTypeTokens == null) return;

            var baseTypes = new List<InheritableType>();

            foreach (var tokens in baseTypeTokens)
            {
                if (!SymbolNode.CouldBeIdentifier(tokens)) throw new UnexpectedTokenException(tokens[0]);

                var type = Expression.Parse(Scope, tokens);
                if (type.Root is SymbolNode symbolNode && symbolNode.Symbol is InheritableType)
                {
                    baseTypes.Add(symbolNode.Symbol as InheritableType);

                    // TODO: Declare self to base type caster here

                    //var childTypeCaster = new FunctionType(
                    //    Token.Anonymous(""),
                    //    this,
                    //    Access.@public,
                    //    this,
                    //    $"!as_{ Name }",
                    //    new FunctionType.ParameterType[] {},
                    //    new Token[] {}
                    //);

                    //Scope.Declare()
                }
                else
                {
                    throw new UnexpectedTokenException(tokens[0]);
                }
            }

            BaseTypes = baseTypes.ToArray();
        }
        
        /// <summary>
        /// Validates if all requirements for implementation or inheritance are fulfilled.
        /// E.g. if all methods of an interface have been implemented.
        /// </summary>
        public void ValidateBaseTypeConstraints()
        {
            if (BaseTypes == null) return;

            foreach (var baseType in BaseTypes)
            {
                if (baseType is ClassType) throw new System.NotImplementedException("Class inheritance is not yet supported");
                if (baseType is InterfaceType interfaceType)
                {
                    InterfaceType.FulfillsInheritanceConstraints(interfaceType, this).Throw();
                }
            }
        }

        public bool IsDescendantOf(InheritableType parent) => IsDescendantOf(parent, this);
        public static bool IsDescendantOf(InheritableType parent, InheritableType child)
        {
            if (child.BaseTypes != null && (child.BaseTypes.Contains(parent) || child.BaseTypes.Any(t => IsDescendantOf(t, parent))))
            {
                return true;
            }
            else if (parent is InterfaceType i)
            {
                return !InterfaceType.FulfillsInheritanceConstraints(i, child).HasErrors();
            }

            return false;
        }
    }
}
