using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octono.Xrm.Tasks
{
    public interface IFileWriter
    {
        void Write(byte[] file, string path);
    }
}
