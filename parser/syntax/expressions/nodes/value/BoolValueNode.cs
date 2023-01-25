using System;
using System.Text.RegularExpressions;
using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Syntax.Expressions.Nodes.Value {
    public class BoolValueNode : ValueNode
    {
        public static PrimitiveType Type = new PrimitiveType(Namespace.Global.Scope, "bool", false, typeof(BoolValueNode));

        public override Types.Type ReturnType {
            get => Type;
        }

        public BoolValueNode(Token token, bool value) : base(token) {
            Value = value;
        }

        /// <summary>
        /// This method is used to force the given object into the required type.
        /// If in any way possible, convert it.
        /// </summary>
        public static bool ToValueNode(object value, out ValueNode node)
        {
            if (value is bool b)
            {
                node = new BoolValueNode(Token.Anonymous(""), b);
                return true;
            }

            node = null;
            return false;
        }

        public new static ValueNode Parse(Token token) {
            if (token.Value == "true") return new BoolValueNode(token, true);
            if (token.Value == "false") return new BoolValueNode(token, false);
            return null;
        }
    }
}