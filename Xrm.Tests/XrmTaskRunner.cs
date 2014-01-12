namespace Xrm.Tests
{
    public class XrmTaskRunner
    {
        private readonly XrmTaskFactory _factory;
        private readonly ILog _log;

        public XrmTaskRunner(XrmTaskFactory xrmTaskFactory, ILog log)
        {
            _factory = xrmTaskFactory;
            _log = log;
        }

        public void Run()
        {
            var task = _factory.CreateTask();
            task.Execute();
        }
    }

    public interface ILog
    {
    }
}