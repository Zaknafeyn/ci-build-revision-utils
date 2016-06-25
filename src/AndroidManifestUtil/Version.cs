using System;
using System.Linq;

namespace AndroidManifestUtil
{
    public class Version
    {
        public static Version Parse(string newVersion)
        {
            var list = newVersion.Split('.').ToList();
            if (list.Count != 4)
                throw new ArgumentException("Invalid new version. Version should consist of 4 digits splitted by dots e.g. 1.2.3.4");

            return new Version
            {
                Major = int.Parse(list[0]),
                Minor = int.Parse(list[1]),
                Build = int.Parse(list[2]),
                Revision = int.Parse(list[3]),
            };
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Build { get; private set; }
        public int Revision { get; private set; }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}.{Revision}";
        }
    }
}