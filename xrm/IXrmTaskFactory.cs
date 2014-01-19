namespace Xrm
{
    public interface IXrmTaskFactory
    {
        IXrmTask CreateTask(string[] args);
    }
}