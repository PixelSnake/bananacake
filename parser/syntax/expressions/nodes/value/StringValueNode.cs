using System;
using System.Text.RegularExpressions;
using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Syntax.Expressions.Nodes.Value {
    public class StringValueNode : ValueNode {
        public static Types.PrimitiveType Type = new PrimitiveType(Namespace.Global.Scope, "string", null, typeof(StringValueNode));

        public override Types.Type ReturnType {
            get => Type;
        }

        public StringValueNode(Token token, string value) : base(token) {
            Value = value;
        }

        /// <summary>
        /// This method is used to force the given object into the required type.
        /// If in any way possible, convert it.
        /// </summary>
        public static bool ToValueNode(object value, out ValueNode node)
        {
            if (value is string s)
            {
                node = new StringValueNode(Token.Anonymous(""), s);
                return true;
            }
            else if (value is null)
            {
                return NullValueNode.ToValueNode(null, out node);
            }

            node = null;
            return false;
        }

        public new static ValueNode Parse(Token token) {
            if (token.Value[0] == '\"') return new StringValueNode(token, token.Value.Substring(1, token.Value.Length - 2));
            return null;
        }
    }
}