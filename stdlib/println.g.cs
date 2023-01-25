using BCake.Parser.Syntax;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime;
using BCake.Runtime.Nodes.Value;

namespace BCake.Std {
    public class println : NativeFunctionType {
        public static NativeFunctionType Implementation = new println(IStringCast.IStringCast.Implementation, true);

        public override bool ExpectsThisArg => false;

        private println(Type typearg1, bool initOverloads = false) : base(
            Namespace.Global.Scope,
            null,
            "println",
            new ParameterType[] {
                 new ParameterType(null, typearg1, "p1"),
            },
            initOverloads ? new NativeFunctionType[]
            {
                new println(StringValueNode.Type),
            } : null
        ) {}

        public override RuntimeValueNode Evaluate(RuntimeScope scope, RuntimeValueNode[] arguments) {
                        
            var arg = arguments[0].Value;
                        
            switch (arg)
            {
                case RuntimeClassInstanceValueNode civn:
                    var caster = civn.RuntimeScope.GetValue("!as_string") as RuntimeFunctionValueNode;
            
                    System.Console.WriteLine((string)caster.Invoke(civn.RuntimeScope, arguments).Value);
                    break;
            
                default:
                    System.Console.WriteLine(arg);
                    break;
            }
            
            return new RuntimeNullValueNode(DefiningToken);
            
        }
    }
}
