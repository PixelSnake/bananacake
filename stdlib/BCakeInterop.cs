using BCake.Std;
using BCake.Array;
using BCake.Integer.Operators;
using BCake.Integer.Typecasts;
using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using BCake.Runtime.Interop;
using BCake.String.Operators;
using BCake.Std.IStringCast;

public class BCakeInterop : IInteropEntryPoint
{
    public Type[] Globals => new Type[]
    {
        Print.Implementation,
        Println.Implementation,
        Array.Implementation,
        IStringCast.Implementation,
    };

    public void Unsafe_Declare()
    {
        IntValueNode.Type.Scope.Declare(IntToStringCast.Implementation);
        IntValueNode.Type.Scope.Declare(
            IntegerOperatorPlus.Implementation
        );

        StringValueNode.Type.Scope.Declare(
            StringOperatorPlus.Implementation
        );
    }
}
