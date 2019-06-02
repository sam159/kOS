using System;
using System.IO;
using System.Linq;
using kOS.FS.IO;
using kOS.FS.Models;

namespace kOS.FS
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 4) {
                Console.WriteLine($"Usage: dotnet fs.dll <file> <sectors> <%index> <rootfs>");
                return 1;
            }

            var file = args[0];
            var sectors = uint.Parse(args[1]);
            var indexes = (uint)(((sectors * 8) / 100) * int.Parse(args[2]));
            var rootFs = args[3];

            var fs = new FileSystem(sectors, indexes);

            Console.WriteLine($"Creating kOS {fs.Header.VersionMajor}.{fs.Header.VersionMinor} filesystem from {rootFs}");

            var pop = new Populate(fs);
            pop.ImportPath(rootFs, "/");

            Console.WriteLine($"In use: {fs.IndexNodes.Count(x => x != null)}/{indexes} indexes, {fs.DataSectors.Count(x => x!=null)}/{fs.DataSectors.Length} data sectors");

            using (var fsWriter = new FSWriter(File.Open(file, FileMode.Create, FileAccess.Write)))
            {
                fsWriter.WriteFS(fs);
            }

            return 0;
        }
    }
}
