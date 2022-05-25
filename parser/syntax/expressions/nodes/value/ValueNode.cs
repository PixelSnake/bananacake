using System;

namespace BCake.Parser.Syntax.Expressions.Nodes.Value {
    public abstract class ValueNode : Node, IRValue {
        public virtual object Value { get; protected set; }

        public ValueNode(Token token) : base(token) {}

        public static ValueNode Parse(Token token) {
            ValueNode node;

            if ((node = IntValueNode.Parse(token)) != null) return node;
            if ((node = BoolValueNode.Parse(token)) != null) return node;
            if ((node = NullValueNode.Parse(token)) != null) return node;
            if ((node = StringValueNode.Parse(token)) != null) return node;

            return null;
        }
    }
}