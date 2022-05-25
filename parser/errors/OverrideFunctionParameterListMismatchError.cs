
using BCake.Parser.Syntax.Types;
using System.Linq;

namespace BCake.Parser.Errors
{
    public class OverrideFunctionParameterListMismatchError : Error
    {
        public OverrideFunctionParameterListMismatchError(FunctionType proto, FunctionType overr, Token token) : base(GenerateMessage(proto, overr), token) { }

        private static string GenerateMessage(FunctionType proto, FunctionType overr)
        {
            var overrParentType = overr.Scope.Parent.Type;
            var baseType = proto.Scope.Parent.Type;
            var overrReturnType = overr.ReturnType.Name == "null" ? "void" : overr.ReturnType.Name;
            var protoReturnType = proto.ReturnType.Name == "null" ? "void" : proto.ReturnType.Name;

            var overrSignatureStr = overr.Parameters.Select(p => p.Type.Name).JoinString(", ");
            var protoSignatureStr = proto.Parameters.Select(p => p.Type.Name).JoinString(", ");
            return $"Override function signature mismatch - {overr.FullName} has signature ({overrSignatureStr}), but because {Type.GetTypeDescription(overrParentType)} {overrParentType.Name} inherits from {Type.GetTypeDescription(baseType)} {baseType.Name}, signature ({protoSignatureStr}) is expected";
        }
    }
}
