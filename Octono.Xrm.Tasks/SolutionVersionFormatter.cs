using System;
using System.Globalization;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Ensures the version number when not already present follows the http://semver.org/ format
    /// </summary>
    public class SolutionVersionFormatter
    {
        public string Increment(string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                return "0.0.1";
            }

            string[] versions = version.Split('.');

            int revision = Convert.ToInt32(versions[versions.Length-1]);

            versions[versions.Length-1] = (++revision).ToString(CultureInfo.InvariantCulture);

            return string.Join(".", versions);
        }

        
    }
}