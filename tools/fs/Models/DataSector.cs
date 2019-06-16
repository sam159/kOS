namespace kOS.FS.Models
{
    class DataSector
    {
        public const int NodeSize = 512;
        public const int ContentSize = NodeSize;
        
        public byte[] Content;
    }
}