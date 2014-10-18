using System;

namespace Octono.Xrm.Tasks
{
    public static class StringExtensions
    {
        public static string ToBase64String(this string input)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
        }
        public static string FromBase64String(this string input)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }
    }
}