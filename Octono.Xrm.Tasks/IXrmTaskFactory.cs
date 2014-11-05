using System.Collections.Generic;

namespace Octono.Xrm.Tasks
{
    public interface IXrmTaskFactory
    {
        IXrmTask CreateTask(IList<string> args);
    }
}