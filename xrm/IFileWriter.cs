using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xrm
{
    public interface IFileWriter
    {
        void Write(byte[] file, string path);
    }
}
