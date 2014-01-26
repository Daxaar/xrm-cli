using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Octono.Xrm.Tasks
{
    public class SystemFileWriter : IFileWriter
    {
        public void Write(byte[] file, string path)
        {
            File.WriteAllBytes(path,file);
        }
    }
}
