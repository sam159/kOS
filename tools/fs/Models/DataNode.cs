using System;
using System.Runtime.InteropServices;

namespace kOS.FS.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 512, CharSet = CharSet.Ansi)]
    struct DataNode
    {
        [FieldOffset(0)] ushort ID;

        [FieldOffset(2)] ushort Size;

        [FieldOffset(6)] uint NextSector;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 504, ArraySubType = UnmanagedType.U1)]
        [FieldOffset(10)] byte[] Content;
    }
}