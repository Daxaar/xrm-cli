using System.Collections.Generic;
using Octono.Xrm.Tasks.Utils;

namespace Octono.Xrm.Tasks
{
    public class EntityAttributesCommandLine : CommandLine
    {
        public EntityAttributesCommandLine(IList<string> args) : base(args)
        {
            EntityName = args[1];

            Filter = args.ArgumentValue("-f");
        }

        public string Filter { get; set; }

        public string EntityName { get; set; }
    }
}