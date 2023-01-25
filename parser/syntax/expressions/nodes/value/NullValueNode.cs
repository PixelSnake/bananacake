using System;
using System.Text.RegularExpressions;
using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Syntax.Expressions.Nodes.Value {
    public class NullValueNode : ValueNode {
        public static Types.PrimitiveType Type = new PrimitiveType(null, "null", null, typeof(NullValueNode));

        public override Types.Type ReturnType {
            get => Type;
        }

        public NullValueNode(Token token) : base(token) {
            Value = null;
        }

        /// <summary>
        /// This method is used to force the given object into the required type.
        /// If in any way possible, convert it.
        /// </summary>
        public static bool ToValueNode(object value, out ValueNode node)
        {
            node = new NullValueNode(Token.Anonymous(""));
            return true;
        }

        public new static ValueNode Parse(Token token) {
            if (token.Value == "null") return new NullValueNode(token);
            else return null;
        }
    }
}