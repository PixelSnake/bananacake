using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Errors
{
    public class OverrideMemberTypeMismatchError : Error
    {
        public OverrideMemberTypeMismatchError(Type proto, Type overr, Token token) : base(GenerateMessage(proto, overr), token) { }

        private static string GenerateMessage(Type proto, Type overr)
        {
            var overrParentType = overr.Scope.Parent.Type;
            var baseType = proto.Scope.Parent.Type;
            return $"Override member type mismatch - {overr.FullName} is a {Type.GetTypeDescription(overr)}, but because {Type.GetTypeDescription(overrParentType)} {overrParentType.Name} inherits from {Type.GetTypeDescription(baseType)} {baseType.Name}, a {Type.GetTypeDescription(proto)} is expected";
        }
    }
}
