using System.IO;

namespace Xrm
{
    public class SystemFileReader : IFileReader
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}