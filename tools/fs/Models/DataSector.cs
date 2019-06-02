using System;
using System.Runtime.InteropServices;

namespace kOS.FS.Models
{
    class DataSector
    {
        public const int NodeSize = 512;
        public const int ContentSize = NodeSize - sizeof(ushort) - sizeof(uint) - sizeof(uint);
        
        public uint ID;

        public ushort Length;

        public uint NextSector;

        public byte[] Content;
    }
}