using System;
using System.Collections.Generic;
using System.Linq;

namespace kOS.FS.Models
{
    class FileSystem
    {
        private ushort NextIndexId = 1;

        public Header Header { get; set; }
        public IndexNode[] IndexNodes { get; set; }
        public DataSector[] DataSectors { get; set; }
        public SectorBitmap Bitmap { get; set; }

        public ushort GenerateIndexId()
        {
            return NextIndexId++;
        }

        public FileSystem()
        {
        }

        public FileSystem(uint sectors, uint indexNodes)
        {
            //Ensure index nodes use whole sectors
            indexNodes += 8 - indexNodes % 8;

            Header = new Header()
            {
                Sectors = sectors,
                IndexNodes = indexNodes,
                VersionMajor = 1,
                VersionMinor = 0
            };
            IndexNodes = new IndexNode[indexNodes];

            var dataSectors = Header.Sectors - Header.IndexSectors - 1;
            var bitmapSectors = 0U;
            while (bitmapSectors * (512 * 8) < dataSectors)
            {
                bitmapSectors++;
                dataSectors--;
            }

            Bitmap = new SectorBitmap(dataSectors);
            Header.BitmapLength = (uint)Bitmap.Bitmap.LongLength;
            Header.BitmapSectors = Bitmap.BitmapSectors;

            DataSectors = new DataSector[dataSectors];
        }

        public void UpdateBitmap()
        {
            Bitmap.Clear();
            //Mark header and bitmap in use
            Bitmap[0] = true;
            for (uint i = 0; i < Header.BitmapSectors; i++)
            {
                Bitmap[1 + i] = true;
            }

            //update index sectors
            var sector = 1 + Header.BitmapSectors;
            for (uint i = 0; i < Header.IndexSectors; i++)
            {
                Bitmap[sector++] = true;
            }

            //update data sectors
            for (uint i = 0; i < DataSectors.LongLength; i++)
            {
                Bitmap[i] = DataSectors[i] != null;
            }
        }

        public IndexNode GetIndexNode(ushort id)
        {
            return IndexNodes.FirstOrDefault(
                x => x != null && x.ID == id && x.Flags.HasFlag(IndexFlags.Valid)
            );
        }

        public IEnumerable<IndexNode> GetChildren(uint parentId)
        {
            return from n in IndexNodes
                   where n != null && n.ParentID == parentId && n.Flags.HasFlag(IndexFlags.Valid)
                   select n;
        }

        public void InsertIndexNode(IndexNode node)
        {
            var index = Array.FindIndex(
                IndexNodes,
                x => x == null || !x.Flags.HasFlag(IndexFlags.Valid)
            );
            if (index == -1)
            {
                throw new InvalidOperationException("Exausted index nodes");
            }
            IndexNodes[index] = node;
        }

        public uint InsertDataSector(DataSector data)
        {
            var index = Array.FindIndex(DataSectors, x => x == null);
            if (index == -1)
            {
                throw new InvalidOperationException("Exausted data capacity");
            }
            DataSectors[index] = data;
            return (uint)index;
        }
    }

}