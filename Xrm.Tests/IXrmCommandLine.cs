using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xrm.Tests
{
    public interface IXrmCommandLine
    {
        void Parse(string command);
    }
}
