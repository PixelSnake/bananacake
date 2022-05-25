using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCake.Parser.Exceptions
{
    public class InvalidVisibilityModifierException : TokenException
    {
        public InvalidVisibilityModifierException(string message, Token token) : base("Invalid visibility modifier - " + message, token) { }
    }
}
