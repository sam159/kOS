using System;
using System.Text;
using System.Runtime.InteropServices;

namespace kOS.FS.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 512, CharSet = CharSet.Ansi)]
    struct Header
    {
        public const string MagicIdentifier = "kOS.FS";

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U1)]
        [FieldOffset(0)] byte[] Magic;

        [FieldOffset(6)] byte VersionMajor;

        [FieldOffset(7)] byte VersionMinor;

        [FieldOffset(8)] uint Sectors;

        [FieldOffset(12)] uint IndexNodes;

        [FieldOffset(16)] uint BootLoaderID;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 492, ArraySubType = UnmanagedType.U1)]
        [FieldOffset(20)] byte[] Reserved;
    }
}