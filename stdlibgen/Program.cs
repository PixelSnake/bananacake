using Newtonsoft.Json.Linq;

namespace stdlibgen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new Exception("Please provide a directory");
            }

            var rootFolderPath = args[0];
            var rootFolder = new DirectoryInfo(rootFolderPath);

            if (!rootFolder.Exists)
            {
                throw new Exception("The given path does not exist");
            }

            Console.WriteLine("Running stdlib generator on " + rootFolder.FullName + " ...");

            GenerateLib(rootFolder);
        }

        private static void GenerateLib(DirectoryInfo dir)
        {
            foreach (var file in dir.GetFiles())
            {
                if (file.Extension == ".bcakedef")
                {
                    GenerateDefinitions(file);
                }
            }

            foreach (var subdir in dir.GetDirectories())
            {
                GenerateLib(subdir);
            }
        }

        private static void GenerateDefinitions(FileInfo file)
        {
            try
            {
                var content = File.ReadAllText(file.FullName);
                var parts = content.Split("---");

                var header = JObject.Parse(parts[0]);
                var code = parts[1];

                NativeFunctionTypeGenerator.Generate(Path.Combine(file.DirectoryName, file.Name.Replace(".bcakedef", ".g.cs")), header, code);
            }
            catch (Exception)
            {

            }
        }
    }
}