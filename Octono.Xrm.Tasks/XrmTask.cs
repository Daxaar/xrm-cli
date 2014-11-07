namespace Octono.Xrm.Tasks
{
    public abstract class XrmTask : IXrmTask
    {
        protected XrmTask(bool requiresServerConnection = true)
        {
            RequiresServerConnection = requiresServerConnection;
        }
        public abstract void Execute(IXrmTaskContext context);
        public bool RequiresServerConnection { get; private set; }
    }
}
