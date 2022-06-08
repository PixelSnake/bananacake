using BCake.Parser.Syntax.Types;

namespace BCake.Parser.Exceptions
{
    public class InvalidConstructorCallException : Error
    {
        /// <param name="type">The type that the constructor has been called on</param>
        public InvalidConstructorCallException(Type type, Token token) : base($"Invalid constructor call - type {type.FullName} cannot be instantiated", token) {}
    }
}
