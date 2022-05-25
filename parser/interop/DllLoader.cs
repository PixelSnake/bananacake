using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace BCake.Parser.Interop
{
    internal class DllLoader
    {
        public static void LoadDll(string path)
        {
            var fullPath = Path.GetFullPath(path);
            if (!File.Exists(fullPath)) throw new FileNotFoundException();

            var dll = Assembly.LoadFrom(fullPath);

        }
    }
}
