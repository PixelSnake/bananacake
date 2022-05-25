using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using BCake.Parser.Exceptions;
using BCake.Parser.Syntax;
using BCake.Parser.Syntax.Types;

namespace BCake {
    public class Application {
        static List<FileInfo> Files = new List<FileInfo>();

        public static int Main(string[] args) {
            Console.WriteLine("BananaCake Compiler Version 0.1.0");
            Console.WriteLine("Copyright PixelSnake 2019-2022, all rights reserved");
            Console.WriteLine();

            ParseArguments(args);

            try
            {
                if (Files.Count > 1)
                {
                    Console.WriteLine("Multiple input files are not supported.");
                    return 1;
                }

                Parser.Parser parser = Parser.Parser.FromFile(Files.First().FullName);
                var globalNamespace = new Namespace();

                var lib = Runtime.Interop.DllLoader.LoadDll(@"C:\Users\tn\privat\bananacake\stdlib\bin\Debug\net5.0\stdlib.dll");
                foreach (var global in lib.Globals)
                {
                    globalNamespace.Scope.Declare(global);
                }
                // TODO check if signed by developer? Would be fun to add something like this
                lib.Unsafe_Declare();

                parser.Parse(globalNamespace);

                var interpreter = new Runtime.Interpreter(globalNamespace);
                return interpreter.Run();
            } catch (TokenException e) {
                Console.WriteLine(e.Message);
                return 1;
            } catch (Exception e) {
                Console.WriteLine("Internal error: " + e.Message);
                return 1;
            }
        }

        private static void ParseArguments(string[] args) {
            for (var i = 0; i < args.Length; ++i) {
                var arg = args[i];

                Files.Add(new FileInfo(arg));
            }
        }
    }
}
