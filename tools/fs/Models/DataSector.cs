using System;
using System.Runtime.InteropServices;

namespace kOS.FS.Models
{
    class DataSector
    {
        public const int NodeSize = 512;
        public const int ContentSize = 504;
        
        public ushort ID;

        public ushort Length;

        public uint NextSector;

        public byte[] Content;
    }
}