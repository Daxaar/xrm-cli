using System;

namespace Octono.Xrm.Tasks.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class XrmTaskAttribute : Attribute
    {
        public XrmTaskAttribute(Type commandLine, string name)
        {
            CommandLine = commandLine;
            Name = name;
        }
        public Type CommandLine { get; set; }
        public string Name { get; set; }

        public string[] Aliases { get; set; }
    }
}