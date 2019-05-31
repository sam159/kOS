using System;
using System.Runtime.InteropServices;

namespace kOS.FS.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 64, CharSet = CharSet.Ansi)]
    struct IndexNode
    {
        [FieldOffset(0)] ushort ID;

        [MarshalAs(UnmanagedType.U2)]
        [FieldOffset(2)] IndexFlags Flags;

        [FieldOffset(4)] ushort ParentID;

        [FieldOffset(6)] ushort DeviceID;

        [FieldOffset(8)] uint Size;

        [FieldOffset(12)] uint DataSector;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U1)]
        [FieldOffset(16)] byte[] Reserved;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        [FieldOffset(32)] string Name;
    }
}