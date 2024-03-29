using BCake.Parser;
using BCake.Parser.Syntax.Expressions.Nodes.Value;

namespace BCake.Runtime.Nodes.Value {
    [RuntimeValueNode(
        Value = 0,
        ValueNodeType = typeof(IntValueNode)
    )]
    public class RuntimeIntValueNode : RuntimeValueNode {
        private static RuntimeScope _definitions;
        public static RuntimeScope Definitions {
            get {
                if (_definitions == null) {
                    _definitions = new RuntimeScope(null, IntValueNode.Type.Scope);
                }
                return _definitions;
            }
        }

        public RuntimeIntValueNode(IntValueNode valueNode, RuntimeScope scope)
            : base(valueNode, IntValueNode.Type, scope) { }

        public override RuntimeValueNode OpPlus(RuntimeValueNode other) {
            return Wrap((int)Value + (int)other.Value);
        }
        public override RuntimeValueNode OpMinus(RuntimeValueNode other) {
            return Wrap((int)Value - (int)other.Value);
        }
        public override RuntimeValueNode OpMultiply(RuntimeValueNode other) {
            return Wrap((int)Value * (int)other.Value);
        }
        public override RuntimeValueNode OpDivide(RuntimeValueNode other) {
            return Wrap((int)Value / (int)other.Value);
        }

        public override RuntimeValueNode OpGreater(RuntimeValueNode other) {
            return Wrap((int)Value > (int)other.Value);
        }
        public override RuntimeValueNode OpEqual(RuntimeValueNode other) {
            return Wrap((int)Value == (int)other.Value);
        }
        public override RuntimeValueNode OpSmaller(RuntimeValueNode other) {
            return Wrap((int)Value < (int)other.Value);
        }

        private RuntimeIntValueNode Wrap(int value) => Wrap(value, DefiningToken, RuntimeScope);
        private RuntimeBoolValueNode Wrap(bool value) => RuntimeBoolValueNode.Wrap(value, DefiningToken, RuntimeScope);

        public static RuntimeIntValueNode Wrap(int value, Token definingToken, RuntimeScope scope) {
            return new RuntimeIntValueNode(
                new IntValueNode(
                    definingToken,
                    value
                ),
                scope
            );
        }
    }
}