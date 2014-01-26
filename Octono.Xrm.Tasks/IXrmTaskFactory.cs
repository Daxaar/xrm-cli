namespace Octono.Xrm.Tasks
{
    public interface IXrmTaskFactory
    {
        IXrmTask CreateTask(string[] args);
    }
}