using BCake.Parser.Syntax.Types;

using BCake.Runtime.Nodes.Value;

namespace BCake.Runtime.Nodes {
    public class RuntimeFunction : RuntimeNode {
        public FunctionType Function { get; protected set; }
        public RuntimeValueNode[] Arguments { get; protected set; }

        public RuntimeFunction(
            FunctionType function,
            RuntimeScope scope,
            RuntimeValueNode[] arguments
        ) : base(function.DefiningToken, scope) {
            Function = function;
            Arguments = arguments;

            RuntimeScope = scope;

            var argsBegin = 0;

            // If functions expect a this arg, this means that the first argument given contains the reference to the
            // object it has been called on. Because methods do not explicitly list the this arg in the
            // list of parameters, it does not appear in the arguments list either and the number of arguments
            // and number of parameters are therefore off by one, which needs to be corrected.
            if (Function.ExpectsThisArg)
            {
                argsBegin = 1;

                foreach (var (key, _) in Arguments[0].RuntimeScope.Scope.AllMembers)
                {
                    var value = Arguments[0].RuntimeScope.GetValue(key);
                    RuntimeScope.Parent.SetValue(key, value);
                }
            }

            for (var i = 0; i < Function.Parameters.Length; ++i) {
                var param = Function.Parameters[i];
                var arg = arguments[i + argsBegin];

                if (param is FunctionType.InitializerParameterType)
                {
                    RuntimeScope.Parent.SetValue(param.Name, arg);
                }

                scope.SetValue(param.Name, arg);
            }
        }

        public override RuntimeValueNode Evaluate() {
            if (Function is NativeFunctionType) {
                return (Function as NativeFunctionType).Evaluate(RuntimeScope, Arguments);
            }
            else
            {
                return new RuntimeScopeNode(RuntimeScope, Function.Root).Evaluate();
            }
        }
    }
}