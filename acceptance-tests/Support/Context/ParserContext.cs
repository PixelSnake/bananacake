using BCake.Parser.Syntax.Expressions.Nodes.Value;
using BCake.Parser.Syntax.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static BCake.Parser.Syntax.Expressions.Nodes.Operators.OperatorAttribute;

namespace BcakeAcceptanceTests.Support.Context
{
    public class ParserContext
    {
        private string BCakeExePath => AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\bananacake\bin\Debug\netcoreapp5.0\bcake.exe";

        public bool HasErrors { get; private set; }
        public string? ErrorMessage { get; private set; }
        public int? ExitCode { get; private set; }
        public string Output { get; private set; }

        public string Code => _code;

        /// <summary>
        /// The code by default contains a few test logging functions
        /// </summary>
        private string _code = @"
            void TEST(string property, int value) {
                string valStr = value as string;
                println(""<%%TEST:"" + property + ""%%>"" + valStr + ""</%%TEST%%>"");
            }
            void TEST(string property, string value) {
                println(""<%%TEST:"" + property + ""%%>"" + value + ""</%%TEST%%>"");
            }
        ";

        public void AddCode(string code)
        {
            _code += "\n\n" + code;
        }

        public void Run()
        {
            var tempFile = "temp\\" + Guid.NewGuid().ToString() + ".bcake";

            if (!Directory.Exists("temp")) Directory.CreateDirectory("temp");
            File.WriteAllText(tempFile, _code);

            var process = new Process();
            process.StartInfo.FileName = BCakeExePath;
            process.StartInfo.Arguments = tempFile;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            CheckForExitCode(output);
            CheckForErrors(output);

            Console.WriteLine("====================");
            Console.WriteLine("File was: " + tempFile);
            Console.WriteLine("This was the output:");
            Console.Write(output);

            Output = output;
        }

        private void CheckForErrors(string output)
        {
            var pattern = @"(Internal )?[Ee]rror( in (.*))?: ((.|\n)*)";
            var match = Regex.Match(output, pattern);
            if (!match.Success)
            {
                HasErrors = false;
                ErrorMessage = null;
                return;
            }

            var errorStr = match.Groups[4].Value;
            HasErrors = true;
            ErrorMessage = errorStr;
        }

        private void CheckForExitCode(string output)
        {
            var pattern = @"Process ended with exit code ([0-9]+)";
            var match = Regex.Match(output, pattern);

            if (!match.Success)
            {
                ExitCode = null;
                return;
            }

            var exitCodeStr = match.Groups[1].Value;

            if (int.TryParse(exitCodeStr, out var exitCode))
                ExitCode = exitCode;
            else
                ExitCode = null;
        }
    }
}
