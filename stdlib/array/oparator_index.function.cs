using System;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime;
using BCake.Runtime.Nodes.Value;
using BCake.Std.Array;
using Type = BCake.Parser.Syntax.Types.Type;

namespace BCake.Array {
    public class OperatorIndex : NativeFunctionGenericMemberType {
        public static OperatorIndex Implementation = new OperatorIndex();
        public override bool ExpectsThisArg => true;

        private OperatorIndex() : base(
            Array.Implementation.Scope,
            IntValueNode.Type, // TODO: generic
            "!operator_index",
            new ParameterType[] {
                 new ParameterType(null, IntValueNode.Type, "i")
            },
            new OperatorIndex[] {}
        ) { }

        private OperatorIndex(Type parent, Type returnType, ParameterType[] parameters) : base(
            Array.Implementation.Scope,
            parent,
            returnType,
            "!operator_index",
            parameters
        )
        { }

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments) {
            var index = (int)arguments[1].Value;
            var __id = (int)(scope.GetValue("__id") as RuntimeIntValueNode)!.Value;

            var typeT = ArrayValueStore.Types[__id];
            var value = ArrayValueStore.Arrays[__id][index];
            ValueNode valueNode;

            switch (typeT)
            {
                case PrimitiveType primitiveType:
                    valueNode = primitiveType.ToValueNode(value);
                    break;

                default:
                    throw new Exception("Unable to convert array value back to it's original type");
            }

            return RuntimeValueNode.Create(valueNode, scope);
        }

        public override FunctionType MakeConcrete(ConcreteClassType parent, Type concreteReturnType, ParameterType[] concreteParameters)
        {
            return new OperatorIndex(parent, concreteReturnType, concreteParameters);
        }
    }
}