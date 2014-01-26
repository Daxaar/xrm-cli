using System.IO;

namespace Octono.Xrm.Tasks.IO
{
    public class SystemFileWriter : IFileWriter
    {
        public void Write(byte[] file, string path)
        {
            File.WriteAllBytes(path,file);
        }
    }
}
