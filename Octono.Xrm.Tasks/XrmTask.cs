namespace Octono.Xrm.Tasks
{
    //TODO: Remove this as redundant since RequiresConnection is no longer used.
    public abstract class XrmTask : IXrmTask
    {
        protected XrmTask(bool requiresServerConnection = true)
        {
            //RequiresServerConnection = requiresServerConnection;
        }
        public abstract void Execute(IXrmTaskContext context);
    }
}
