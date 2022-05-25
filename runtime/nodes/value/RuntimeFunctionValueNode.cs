using BCake.Parser.Syntax.Expressions;
using BCake.Parser.Syntax.Expressions.Nodes;
using BCake.Parser.Syntax.Expressions.Nodes.Functions;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime.Nodes.Operators;
using System.Linq;

namespace BCake.Runtime.Nodes.Value {
    public class RuntimeFunctionValueNode : RuntimeValueNode {
        public FunctionType Function => (FunctionType)Value;

        public RuntimeFunctionValueNode(FunctionType function, RuntimeScope scope)
            : base(null, function, scope) {
            Value = function;
        }

        public RuntimeValueNode Invoke(RuntimeScope scope, RuntimeValueNode[] arguments)
        {
            return new RuntimeFunction(
                Function,
                scope,
                arguments
            ).Evaluate();
        }

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
            throw new Exceptions.RuntimeException("", DefiningToken);
        }
        public override RuntimeValueNode OpSmaller(RuntimeValueNode other) {
            throw new Exceptions.RuntimeException("", DefiningToken);
        }
    }
}