using System;

namespace kOS.FS.Models
{
    class SectorBitmap
    {
        public uint Sectors { get; private set; }

        public uint BitmapSize => (uint)((Sectors / 8) + (Sectors % 8 != 0 ? 1 : 0));

        public uint BitmapSectors => (uint)((BitmapSize / 512) + (BitmapSize % 512 != 0 ? 1 : 0));

        public byte[] Bitmap { get; private set; }

        public SectorBitmap(uint sectors)
        {
            Sectors = sectors;
            Bitmap = new byte[BitmapSize];
        }

        public bool this[uint sector] {
            get {
                return IsOccupied(sector);
            }
            set {
                Update(sector, value);
            }
        }

        public void Clear()
        {
            Array.Fill<byte>(Bitmap, 0);
        }

        private (uint mapByte, byte mapBit) GetLocation(uint sector) 
        {
            return (
                mapByte: sector /8, 
                mapBit: (byte) (7 - (sector % 8))
            );
        }

        public void Update(uint sector, bool occupied)
        {
            var loc = GetLocation(sector);
            if (occupied) {
                Bitmap[loc.mapByte] |= (byte)(1 << loc.mapBit);
            }
            else {
                Bitmap[loc.mapByte] &= (byte)~(1 << loc.mapBit);
            }
        }

        public bool IsOccupied(uint sector)
        {
            var loc = GetLocation(sector);

            return (Bitmap[loc.mapByte] & (byte)(1 << loc.mapBit)) != 0; 
        }
    }
}