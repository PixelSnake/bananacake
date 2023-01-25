using System;
using System.Reflection;
using BCake.Parser.Syntax.Expressions.Nodes.Value;

namespace BCake.Parser.Syntax.Types {
    public class PrimitiveType : Type
    {
        public System.Type ValueNodeType { get; }

        public PrimitiveType(Scopes.Scope parent, string name, object defaultValue, System.Type valueNodeType)
            : base(parent, name, defaultValue)
        {
            ValueNodeType = valueNodeType;
        }

        public ValueNode ToValueNode(object value)
        {
            var convertMethod = ValueNodeType.GetMethod("ToValueNode", BindingFlags.Static | BindingFlags.Public);

            var inputParams = new object[] {value, new IntValueNode(Token.Anonymous(""), 0)};
            var result = (bool)convertMethod.Invoke(null, inputParams);

            if (result)
            {
                return (ValueNode)inputParams[1];
            }

            return null;
        }
    }
}