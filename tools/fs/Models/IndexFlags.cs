using System;

namespace kOS.FS.Models
{
    [Flags]
    enum IndexFlags : ushort {
        None = 0,
        Directory = 1 << 0,
        Valid = 1 << 1,
        Device = 1 << 2,
        HasData = 1 << 3
    }
}