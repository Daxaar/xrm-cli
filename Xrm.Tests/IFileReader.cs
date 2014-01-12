using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xrm.Tests
{
    /// <summary>
    /// Testable File Access
    /// </summary>
    public interface IFileReader
    {
        byte[] ReadAllBytes(string path);
    }
}
