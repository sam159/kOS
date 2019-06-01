using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using kOS.FS.Models;

namespace kOS.FS.IO
{
    class Populate
    {
        private readonly FileSystem FS;

        public Populate(FileSystem fs)
        {
            FS = fs;
        }

        private IndexNode GetOrCreateDir(string name, ushort parentId)
        {
            var node = FS.GetChildren(parentId).FirstOrDefault(x => x.Name == name);
            if (node == null)
            {
                node = new IndexNode
                {
                    ID = FS.GenerateIndexId(),
                    Flags = IndexFlags.Directory | IndexFlags.Valid,
                    ParentID = parentId,
                    Name = name
                };
                FS.InsertIndexNode(node);
                return node;
            }
            if (!node.Flags.HasFlag(IndexFlags.Directory))
            {
                throw new InvalidOperationException("existing index record is not a directory");
            }
            return node;
        }

        private void AddFile(FileInfo file, IndexNode dir)
        {
            var node = new IndexNode
            {
                ID = FS.GenerateIndexId(),
                ParentID = dir.ID,
                Flags = IndexFlags.Valid,
                Name = file.Name,
                DataLength = (uint)file.Length
            };
            FS.InsertIndexNode(node);
            if (file.Length > 0)
            {
                using (var fileStream = file.Open(FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[DataSector.ContentSize];
                    var read = 0;
                    DataSector prevSector = null;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        var sector = new DataSector
                        {
                            ID = node.ID,
                            Length = (ushort)read,
                            NextSector = 0,
                            Content = new byte[DataSector.ContentSize]
                        };
                        Array.Copy(buffer, sector.Content, read);
                        var sectorId = FS.InsertDataSector(sector);
                        if (!node.Flags.HasFlag(IndexFlags.HasData)) {
                            node.Flags |= IndexFlags.HasData;
                            node.DataSector = sectorId;
                        }
                        if (prevSector != null)
                        {
                            prevSector.NextSector = sectorId;
                        }
                        prevSector = sector;
                    }
                }
            }
        }

        public void ImportPath(string source, string dest)
        {
            var sourceInfo = new DirectoryInfo(source);
            if (!sourceInfo.Exists)
            {
                throw new ArgumentException("Source folder does not exist", nameof(source));
            }
            if (!dest.StartsWith('/'))
            {
                throw new ArgumentException("Destination path must start with /", nameof(dest));
            }
            var destPath = dest.TrimEnd('/').Split("/");
            IndexNode destRoot = null;
            for (var i = 0; i < destPath.Length; i++)
            {
                destRoot = GetOrCreateDir(destPath[i], destRoot?.ParentID ?? 0);
            }
            CopyIn(sourceInfo, destRoot);

            void CopyIn(DirectoryInfo dirInfo, IndexNode parent)
            {
                foreach (var childDirInfo in dirInfo.EnumerateDirectories())
                {
                    var childDir = GetOrCreateDir(childDirInfo.Name, parent.ID);
                    CopyIn(childDirInfo, childDir);
                }

                foreach (var fileInfo in dirInfo.EnumerateFiles())
                {
                    AddFile(fileInfo, parent);
                }
            }
        }

    }
}