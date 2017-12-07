using System;

namespace Octono.Xrm.Tasks
{
    public static class WebResourceType
    {
        public const int Html = 1;
        public const int Css = 2;
        public const int Js = 3;
        public const int Xml = 4;
        public const int Png = 5;
        public const int Jpg = 6;
        public const int Gif = 7;
        public const int Xap = 8;
        public const int Xsl = 9;
        public const int Ico = 10;

        public static string ToFileExtension(int value)
        {
            switch (value)
            {
                case 1:
                    return ".htm";
                case 2:
                    return ".css";
                case 3:
                    return ".js";
                case 4:
                    return ".xml";
                case 5:
                    return ".png";
                case 6:
                    return ".jpg";
                case 7:
                    return ".gif";
                case 8:
                    return ".xap";
                case 9:
                    return ".xsl";
                case 10:
                    return ".ico";
                default:
                    throw new ArgumentOutOfRangeException($"The web resource type with value {value} is unknown.");
            }
        }
    }
}