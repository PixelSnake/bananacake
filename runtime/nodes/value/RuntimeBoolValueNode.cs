using BCake.Parser.Syntax.Expressions.Nodes.Value;

namespace BCake.Runtime.Nodes.Value {
    [RuntimeValueNode(
        Value = false,
        ValueNodeType = typeof(BoolValueNode)
    )]
    public class RuntimeBoolValueNode : RuntimeValueNode {
        public RuntimeBoolValueNode(BoolValueNode valueNode, RuntimeScope scope)
            : base(valueNode, BoolValueNode.Type, scope) {}

        public override RuntimeValueNode OpPlus(RuntimeValueNode other) {
            throw new Exceptions.RuntimeException("", DefiningToken);
        }
        public override RuntimeValueNode OpMinus(RuntimeValueNode other) {
            throw new Exceptions.RuntimeException("", DefiningToken);
        }
        public override RuntimeValueNode OpMultiply(RuntimeValueNode other) {
            throw new Exceptions.RuntimeException("", DefiningToken);
        }
        public override RuntimeValueNode OpDivide(RuntimeValueNode other) {
            throw new Exceptions.RuntimeException("", DefiningToken);
        }

        public override RuntimeValueNode OpGreater(RuntimeValueNode other) {
            throw new Exceptions.RuntimeException("", DefiningToken);
        }
        public override RuntimeValueNode OpEqual(RuntimeValueNode other) {
            return Wrap((bool)Value == (bool)other.Value);
        }
        public override RuntimeValueNode OpSmaller(RuntimeValueNode other) {
            throw new Exceptions.RuntimeException("", DefiningToken);
        }

        private RuntimeBoolValueNode Wrap(bool value) {
            return new RuntimeBoolValueNode(
                new BoolValueNode(
                    DefiningToken,
                    value
                ),
                RuntimeScope
            );
        }
    }
}