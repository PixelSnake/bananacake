using System.Collections.Generic;
using System.Linq;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime;
using BCake.Runtime.Nodes.Value;
using BCake.Std.Array;

namespace BCake.Array {
    public class ArrayConstructor : NativeFunctionGenericMemberType {
        public static NativeFunctionType Implementation = new ArrayConstructor();

        private static int idCounter = 0;
        private int id;

        private ArrayConstructor() : base(
            Array.Implementation.Scope,
            Array.Implementation,
            "!constructor",
            new ParameterType[]
            {
                new ParameterType(null, IntValueNode.Type, "size"),
            },
            new ArrayConstructor[] {}
        ) { }

        private ArrayConstructor(Type parent, Type returnType, ParameterType[] parameters) : base(
            parent.Scope,
            parent,
            returnType,
            "!constructor",
            parameters
        ) { }

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments)
        {
            id = ++idCounter;
            scope.SetValue("__id", RuntimeIntValueNode.Wrap(id, DefiningToken, scope));

            var typeT = (scope.GetValue("T") as RuntimeTypeValueNode).Type;
            var size = (int)arguments[0].Value;

            ArrayValueStore.Arrays.Add(id, Enumerable.Repeat(typeT.DefaultValue, size).ToArray());
            ArrayValueStore.Types.Add(id, typeT);

            return new RuntimeNullValueNode(DefiningToken);
        }

        public override FunctionType MakeConcrete(ConcreteClassType parent, Type concreteReturnType, ParameterType[] concreteParameters)
        {
            return new ArrayConstructor(parent, concreteReturnType, concreteParameters);
        }
    }
}