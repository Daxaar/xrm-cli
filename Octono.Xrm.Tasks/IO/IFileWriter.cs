namespace Octono.Xrm.Tasks.IO
{
    public interface IFileWriter
    {
        void Write(byte[] file, string path);
    }
}
