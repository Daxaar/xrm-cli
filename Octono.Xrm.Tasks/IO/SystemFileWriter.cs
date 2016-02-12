using System;
using System.IO;

namespace Octono.Xrm.Tasks.IO
{
    public class SystemFileWriter : IFileWriter
    {
        public void Write(byte[] file, string path)
        {
            if(string.IsNullOrEmpty(path)) throw new ArgumentNullException("path","Path cannot be null");
            
            var dirName = Path.GetDirectoryName(path);
            if(string.IsNullOrEmpty(dirName)) throw new NullReferenceException(string.Format("Unable to build directory name for path {0}", path));
            Directory.CreateDirectory(dirName);

            File.WriteAllBytes(path,file);
        }

        public void Write(string content, string path)
        {
            Write(System.Text.Encoding.UTF8.GetBytes(content), path);
        }
    }
}
