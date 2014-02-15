using System.Collections.Specialized;
using System.Configuration;

namespace Octono.Xrm.Tasks.IO
{
    public interface IConfigurationManager
    {
        NameValueCollection AppSettings{get;}

        ConnectionStringSettingsCollection ConnectionStrings { get; }

        T GetSection<T>(string sectionName);
        void Save();
        void Add(string key, string value);
    }
}