using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime;
using BCake.Runtime.Nodes.Value;

namespace BCake.Integer.Operators
{
    public class IntegerOperatorMinus : NativeFunctionType
    {
        public static NativeFunctionType Implementation = new IntegerOperatorMinus();

        private IntegerOperatorMinus() : base(
            IntValueNode.Type.Scope,
            IntValueNode.Type,
            "!operator_minus",
            new ParameterType[] {
                new ParameterType(null, IntValueNode.Type, "other")
            }
        )
        {
            Scope.Declare(
                new ParameterType(DefiningToken, IntValueNode.Type, "other")
            );
        }

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments)
        {
            var left = (int)arguments[0].Value;
            var right = (int)arguments[1].Value;

            return new RuntimeIntValueNode(
                new IntValueNode(
                    DefiningToken,
                    left - right
                ),
                null
            );
        }
    }
}
