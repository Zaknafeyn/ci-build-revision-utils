using System;
using System.IO;
using System.Linq;

namespace AndroidManifestUtil
{
    static class Program
    {

        private const string FilenameToken = "-filename=";
        private const string NewVersionToken = "-newversion=";

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine(@"Usage: AndroidManifestUtil -filename=<path to AndroidManifest.xml> -newversion=<Version to apply>");

                Console.WriteLine(
                  @"e.g. AndroidManifestUtil -filename=..\src\MyProject\Properties\AndroidManifest.xml -newversion=1.2.3.4");

                Console.WriteLine(@"Will update the version code in manifest/android:versionCode, and build revision in manifest/android:versionName");
            }
            else
            {
                // remember in debug mode, this file passed on the debug command line will be in the bin\Debug\

                var fileName = args.FirstOrDefault(a => a.StartsWith(FilenameToken, StringComparison.OrdinalIgnoreCase))?.Substring(FilenameToken.Length);
                var newVersionStr = args.FirstOrDefault(a => a.StartsWith(NewVersionToken, StringComparison.OrdinalIgnoreCase))?.Substring(NewVersionToken.Length);

                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentException("Version file name must not be empty");
                if (string.IsNullOrWhiteSpace(NewVersionToken))
                    throw new ArgumentException("New version value must not be empty");
                if (!File.Exists(fileName))
                    throw new ArgumentException($"Couldn't locate file '{fileName}'");

                var newVersion = Version.Parse(newVersionStr);

                var androidManifest = AndroidManifest.Load(fileName);

                androidManifest.UpdateVersionCode(newVersion);
                androidManifest.UpdateVersionName(newVersion);
                androidManifest.SaveFile(fileName);
            }
        }
    }
}
