using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xrm
{
    public class SystemFileWriter : IFileWriter
    {
        public void Write(byte[] file, string path)
        {
            File.WriteAllBytes(path,file);
        }
    }
}
