using BCake.Parser.Syntax.Types;

namespace BCake.Runtime.Interop
{
    public interface IInteropEntryPoint
    {
        public Type[] Globals { get; }

        public void Unsafe_Declare();
    }
}
