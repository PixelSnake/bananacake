using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCake.Parser.Errors
{
    public class Error : Result
    {
        public override bool IsError => true;
        public string Message;

        private Error() { }

        public Error(string message, Token token)
        {
            Message = $"Error in {token.FilePath}({token.Line},{token.Column}): {message}";
        }

        public Exception ToException() => new Exception(Message);

        public static Error Multiple(params Error[] errors)
        {
            return new Error
            {
                Message = errors.Select(e => e.Message).JoinString("\n\n")
            };
        }
    }
}
