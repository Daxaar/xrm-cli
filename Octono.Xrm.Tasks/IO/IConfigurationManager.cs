namespace Octono.Xrm.Tasks.IO
{
    public interface IConfigurationManager
    {
        void Save(IXrmConfiguration configuration);
        IXrmConfiguration Load();
    }
}