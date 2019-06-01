using System;
using System.Text;
using System.Runtime.InteropServices;

namespace kOS.FS.Models
{
    class Header
    {
        public const string MagicIdentifier = "kOS.FS\0\0";
        public const int MagicSize = 8;
        public const int Size = 512;
        public const int ReservedSize = 478;

        public byte[] Magic { get; set; }

        public byte VersionMajor { get; set; }

        public byte VersionMinor { get; set; }

        public uint Sectors { get; set; }

        public uint IndexNodes { get; set; }

        public uint IndexSectors => IndexNodes / 8 + (IndexNodes % 8 != 0 ? 1U : 0U);

        public uint BitmapSectors { get; set; }

        public uint BitmapLength { get; set; }

        public uint BootLoaderID { get; set; }

        public byte[] Reserved { get; set; }
    }
}