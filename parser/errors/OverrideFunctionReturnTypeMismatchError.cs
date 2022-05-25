using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Errors
{
    public class OverrideFunctionReturnTypeMismatchError : Error
    {
        public OverrideFunctionReturnTypeMismatchError(FunctionType proto, FunctionType overr, Token token) : base(GenerateMessage(proto, overr), token) { }

        private static string GenerateMessage(FunctionType proto, FunctionType overr)
        {
            var overrParentType = overr.Scope.Parent.Type;
            var baseType = proto.Scope.Parent.Type;
            var overrReturnType = overr.ReturnType.Name == "null" ? "void" : overr.ReturnType.Name;
            var protoReturnType = proto.ReturnType.Name == "null" ? "void" : proto.ReturnType.Name;
            return $"Override return type mismatch - {overr.FullName} has return type {overrReturnType}, but because {Type.GetTypeDescription(overrParentType)} {overrParentType.Name} inherits from {Type.GetTypeDescription(baseType)} {baseType.Name}, return type {protoReturnType} is expected";
        }
    }
}
