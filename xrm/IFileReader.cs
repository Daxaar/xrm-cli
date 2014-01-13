namespace Xrm
{
    /// <summary>
    /// Testable File Access
    /// </summary>
    public interface IFileReader
    {
        byte[] ReadAllBytes(string path);
    }
}
