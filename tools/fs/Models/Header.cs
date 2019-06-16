using System;
using System.Text;
using System.Runtime.InteropServices;

namespace kOS.FS.Models
{
    class Header
    {
        public const string MagicIdentifier = "kOS.FS";
        public const int MagicSize = 6;
        public const int Size = 512;
        public const int ReservedSize = Size - MagicSize - (sizeof(byte) * 2) - (sizeof(uint) * 6);

        public byte[] Magic { get; set; }

        public byte VersionMajor { get; set; }

        public byte VersionMinor { get; set; }

        public uint Sectors { get; set; }

        public uint IndexNodes { get; set; }

        public uint IndexSectors => (IndexNodes * IndexNode.NodeSize) / 512;

        public uint BitmapSectors { get; set; }

        public uint BitmapLength { get; set; }

        public uint BootLoaderID { get; set; }

        public byte[] Reserved { get; set; }
    }
}