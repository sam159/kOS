using System;
using System.Runtime.InteropServices;

namespace kOS.FS.Models
{
    class IndexNode
    {
        public const int NodeSize = 64;
        public const int NameMaxLength = 32;
        public const int ReservedSize = NodeSize - NameMaxLength - (sizeof(ushort) * 2) - (sizeof(uint) * 5);

        public uint ID { get; set; }

        public IndexFlags Flags { get; set; }

        public uint ParentID { get; set; }

        public ushort DeviceID { get; set; }

        public uint DataLength { get; set; }

        public uint DataSector { get; set; }

        public uint DataSectorCount { get; set; }

        public byte[] Reserved { get; set; }

        public string Name { get; set; }
    }
}