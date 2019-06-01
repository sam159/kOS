using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using kOS.FS.Models;

namespace kOS.FS.IO
{
    class FSWriter : BinaryWriter
    {
        public FSWriter(Stream stream) : base(stream, Encoding.ASCII)
        {
        }

        public void WriteFS(FileSystem fs)
        {
            fs.UpdateBitmap();
            WriteHeader(fs.Header);
            WriteBitmap(fs.Bitmap);
            var emptyIndex = new IndexNode();
            for (long i = 0; i < fs.Header.IndexNodes; i++)
            {
                if (i < fs.IndexNodes.LongLength)
                {
                    WriteIndex(fs.IndexNodes[i] ?? emptyIndex);
                }
                else
                {
                    WriteIndex(emptyIndex);
                }
            }
            var emptyData = new DataSector();
            var dataCount = fs.Header.Sectors - fs.Header.IndexNodes - fs.Header.BitmapSectors - 1;
            for (long i = 0; i < dataCount; i++)
            {
                if (i < fs.DataSectors.LongLength)
                {
                    WriteData(fs.DataSectors[i] ?? emptyData);
                }
                else
                {
                    WriteData(emptyData);
                }
            }
        }

        private void WriteHeader(Header header)
        {
            if (header.Magic == null || header.Magic.Length != Header.MagicSize)
            {
                Write(System.Text.Encoding.ASCII.GetBytes(Header.MagicIdentifier));
            }
            else
            {
                Write(header.Magic);
            }
            Write(header.VersionMajor);
            Write(header.VersionMinor);
            Write(header.Sectors);
            Write(header.IndexNodes);
            Write(header.IndexSectors);
            Write(header.BitmapSectors);
            Write(header.BitmapLength);
            Write(header.BootLoaderID);
            if (header.Reserved == null || header.Reserved.Length != Header.ReservedSize)
            {
                Write(new byte[Header.ReservedSize]);
            }
            else
            {
                Write(header.Reserved);
            }
        }

        private void WriteBitmap(SectorBitmap bitmap)
        {
            Write(bitmap.Bitmap);
            var sectorsLength = bitmap.BitmapSectors * 512;
            if (bitmap.Bitmap.Length < sectorsLength)
            {
                Write(new byte[sectorsLength - bitmap.Bitmap.Length]);
            }
        }

        private void WriteIndex(IndexNode node)
        {
            Write(node.ID);
            Write((ushort)node.Flags);
            Write(node.ParentID);
            Write(node.DeviceID);
            Write(node.DataLength);
            Write(node.DataSector);
            if (node.Reserved == null || node.Reserved.Length != IndexNode.ReservedSize)
            {
                Write(new byte[IndexNode.ReservedSize]);
            }
            else
            {
                Write(node.Reserved);
            }
            var name = node.Name ?? "";
            var nameBytes = System.Text.Encoding.ASCII.GetBytes(
                name.Substring(0, Math.Min(name.Length, IndexNode.NameMaxLength - 1))
            );
            Array.Resize(ref nameBytes, IndexNode.NameMaxLength);
            nameBytes[nameBytes.Length - 1] = 0;
            Write(nameBytes);
        }

        private void WriteData(DataSector data)
        {
            Write(data.ID);
            Write(data.Length);
            Write(data.NextSector);
            if (data.Content == null || data.Content.Length != DataSector.ContentSize)
            {
                Write(new byte[DataSector.ContentSize]);
            }
            else
            {
                Write(data.Content);
            }
        }
    }
}