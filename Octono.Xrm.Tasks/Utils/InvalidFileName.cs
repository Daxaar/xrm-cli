using System;
using System.Text.RegularExpressions;

namespace Octono.Xrm.Tasks.Utils
{
    /// <summary>
    /// It's quite common to name web resources following a path name convention thus:
    /// new_/scripts/myresource.js.  Any webresources following this naming convention must have their
    /// name escaped before saving to disk as the name isn't a valid filename.  The app would actually create
    /// the pseudo directory structure.  So this sample file would end up in a file called myresource.js under the
    /// scripts directory.
    /// </summary>
    static class InvalidFileName
    {
        private const string InvalidCharacters = @"""\/?:<>*|";
        private const string EscapeCharacter = "%";

        static readonly Regex EscapeRegEx = new Regex("[" + Regex.Escape(EscapeCharacter + InvalidCharacters) + "]",RegexOptions.Compiled);
        static readonly Regex UnEscapeRegex = new Regex(Regex.Escape(EscapeCharacter) + "([0-9A-Z]{4})", RegexOptions.Compiled);

        public static string Escape(string path)
        {
            return path;
            return EscapeRegEx.Replace(path, m => EscapeCharacter + ((short)(m.Value[0])).ToString("X4"));
        }

        public static string Unescape(string path)
        {
            return path;
            return UnEscapeRegex.Replace(path, m => ((char)Convert.ToInt16(m.Groups[1].Value, 16)).ToString());
        }
    }
}