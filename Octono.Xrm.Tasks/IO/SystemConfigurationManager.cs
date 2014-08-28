using System.Collections.Specialized;
using System.Configuration;

namespace Octono.Xrm.Tasks.IO
{
    /// <summary>
    /// Testable ConfigurationManager
    /// </summary>
    public class SystemConfigurationManager : IConfigurationManager
    {
        private readonly Configuration _config;

        public SystemConfigurationManager()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public NameValueCollection AppSettings
        {
            get { return ConfigurationManager.AppSettings; }
        }

        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return ConfigurationManager.ConnectionStrings; }
        }

        public T GetSection<T>(string sectionName)
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }

        public void Save()
        {
            _config.Save();
        }

        public void Add(string key, string value)
        {
            AppSettingsSection section = _config.AppSettings;
            section.Settings.Remove(key);
            section.Settings.Add(key,value);
        }
    }
}