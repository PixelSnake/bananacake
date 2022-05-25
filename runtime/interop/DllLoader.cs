using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace BCake.Runtime.Interop
{
    public class DllLoader
    {
        public static IInteropEntryPoint LoadDll(string path)
        {
            var fullPath = Path.GetFullPath(path);
            if (!File.Exists(fullPath)) throw new FileNotFoundException();

            var dll = Assembly.LoadFrom(fullPath);
            var entryType = dll.GetType("BCakeInterop");
            if (entryType == null) return null;

            var entry = Activator.CreateInstance(entryType);
            return entry as IInteropEntryPoint;
        }
    }
}
