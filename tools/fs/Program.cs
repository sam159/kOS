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
            if (args.Length != 5) {
                Console.WriteLine($"Usage: dotnet fs.dll <file> <sectors> <%index> <rootfs> <bootloader>");
                return 1;
            }

            var file = args[0];
            var sectors = uint.Parse(args[1]);
            var indexes = (uint)(((sectors * 8) / 100) * int.Parse(args[2]));
            var rootFs = args[3];
            var bootloader = args[4];

            var fs = new FileSystem(sectors, indexes);
            indexes = fs.Header.IndexNodes;

            Console.WriteLine($"Creating kOS {fs.Header.VersionMajor}.{fs.Header.VersionMinor} filesystem from path {rootFs}");

            var pop = new Populate(fs);
            pop.ImportPath(rootFs, "/");

            if (!string.IsNullOrEmpty(bootloader)) {
                var bootloaderNode = fs.GetIndexByPath(bootloader);
                if (bootloaderNode == null) {
                    throw new ArgumentException("Bootloader file not found");
                }
                else if (bootloaderNode.Flags.HasFlag(IndexFlags.Directory)) {
                    throw new ArgumentException("Bootloader file is a directory");
                }
                else if (!bootloaderNode.Flags.HasFlag(IndexFlags.Contigious))  {
                    throw new ArgumentException("Bootloader file not contigious");
                }
                fs.Header.BootLoaderID = bootloaderNode.ID;
                fs.Header.BootLoaderFirstSector = bootloaderNode.DataSector;
                fs.Header.BootLoaderSectorCount = bootloaderNode.DataSectorCount;
            }

            Console.WriteLine($"In use: {fs.IndexNodes.Count(x => x != null)}/{indexes} indexes, {fs.DataSectors.Count(x => x!=null)}/{fs.DataSectors.Length} data sectors");
            if (fs.Header.BootLoaderID > 0) {
                Console.WriteLine($"Bootloader ID: {fs.Header.BootLoaderID}");
            }

            using (var fsWriter = new FSWriter(File.Open(file, FileMode.Create, FileAccess.Write)))
            {
                fsWriter.WriteFS(fs);
            }

            return 0;
        }
    }
}
