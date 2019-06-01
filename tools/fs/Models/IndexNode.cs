using System;
using System.Runtime.InteropServices;

namespace kOS.FS.Models
{
    class IndexNode
    {
        public const int NodeSize = 64;
        public const int ReservedSize = 16;
        public const int NameMaxLength = 32;

        public ushort ID { get; set; }

        public IndexFlags Flags { get; set; }

        public ushort ParentID { get; set; }

        public ushort DeviceID { get; set; }

        public uint DataLength { get; set; }

        public uint DataSector { get; set; }

        public byte[] Reserved { get; set; }

        public string Name { get; set; }
    }
}