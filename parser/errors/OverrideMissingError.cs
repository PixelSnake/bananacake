
using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Errors
{
    public class OverrideMissingError : Error
    {
        public OverrideMissingError(Type protoMember, Type derivedType, Token token) : base(GenerateMessage(protoMember, derivedType), token) { }

        private static string GenerateMessage(Type protoMember, Type derivedType)
        {
            var protoDescription = $"{Type.GetTypeDescription(protoMember)} {protoMember.Name}";
            if (protoMember is FunctionType ft && ft.Name.StartsWith("!as_"))
            {
                protoDescription = $"cast to {ft.Name.Substring(4)}";
            }

            var protoType = protoMember.Scope.Parent.Type;
            return $"Missing override - because {Type.GetTypeDescription(derivedType)} {derivedType.Name} inherits from {Type.GetTypeDescription(protoType)} {protoType.Name}, an override for {protoDescription} is expected, but none was provided";
        }
    }
}
