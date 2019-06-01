using System;
using System.IO;
using kOS.FS.IO;
using kOS.FS.Models;

namespace kOS.FS
{
    class Program
    {
        static void Main(string[] args)
        {

            var fs = new FileSystem(10000, 1000);

            var pop = new Populate(fs);
            pop.ImportPath("/home/sam/Projects/kOS/rootfs", "/");

            using (var fsWriter = new FSWriter(File.Open("disk.part", FileMode.Create, FileAccess.Write)))
            {
                fsWriter.WriteFS(fs);
            }
        }
    }
}
