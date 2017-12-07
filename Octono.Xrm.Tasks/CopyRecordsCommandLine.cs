using System;
using System.Collections.Generic;

namespace Octono.Xrm.Tasks
{
    public class CopyRecordsCommandLine : CommandLine
    {
        public CopyRecordsCommandLine(IList<string> args) : base(args)
        {
        }

        public IDictionary<string, string> Entities { get; private set; }

        public DateTime FromDate { get; set; }

        public string Destination { get; set; }
    }
}