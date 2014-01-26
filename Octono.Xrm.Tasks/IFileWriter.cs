namespace Octono.Xrm.Tasks
{
    public interface IFileWriter
    {
        void Write(byte[] file, string path);
    }
}
