namespace Octono.Xrm.Tasks
{
    public interface IXrmTask
    {
        void Execute(IXrmTaskContext context);
        bool RequiresServerConnection { get; }
    }
}
