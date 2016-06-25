using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace AndroidManifestUtil
{
    class Program
    {

        private const string FilenameToken = "-filename=";
        private const string NewVersionToken = "-newversion=";

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine(@"Usage: AndroidManifestUtil -filename=<path to AndroidManifest.xml> -newversion=<Version to apply>");

                Console.WriteLine(
                  @"e.g. AndroidManifestUtil ..\src\MyProject\Properties\AndroidManifest.xml");
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

                LoadAndUpdateVersionCodeFor(fileName, newVersion);
                LoadAndUpdateBuildRevisionFor(fileName, newVersion);
            }
        }

        private static void LoadAndUpdateVersionCodeFor(string fileName, Version version)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Loading {0}", fileName);

            var xmldoc = new XmlDocument();
            xmldoc.Load(fileName);

            Console.WriteLine("Loaded {0}", fileName);
            Console.WriteLine("Loading manifest node...");

            var manifestNode = xmldoc.SelectSingleNode("/manifest");

            Console.WriteLine("Getting manifest attributes...");

            var attributes = manifestNode.Attributes;
            var versionCode = attributes.GetNamedItem("android:versionCode");
            if (versionCode != null)
            {
                Console.WriteLine("Found android:versionCode attribute: {0}", versionCode.Value);

                int newValue = version.Revision;
                versionCode.Value = string.Format("{0}", newValue);

                Console.WriteLine("Updating android:versionName attribute to {0}", versionCode.Value);

                manifestNode.Attributes.SetNamedItem(versionCode);

                Console.WriteLine("Writing out updated manifest file: {0}", fileName);

                xmldoc.Save(fileName);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Did not find android:versionCode attribute, aborting!");
                Console.ResetColor();
            }
            Console.ResetColor();
            Console.WriteLine("Done");
        }

        private static void LoadAndUpdateBuildRevisionFor(string fileName, Version version)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Loading {0}", fileName);
            var xmldoc = new XmlDocument();
            xmldoc.Load(fileName);
            Console.WriteLine("Loaded {0}", fileName);

            Console.WriteLine("Loading manifest node...");
            var manifestNode = xmldoc.SelectSingleNode("/manifest");
            Console.WriteLine("Getting manifest attributes...");
            var attributes = manifestNode.Attributes;
            var versionName = attributes.GetNamedItem("android:versionName");
            if (versionName != null)
            {
                Console.WriteLine("Found android:versionName attribute: {0}", versionName.Value);
                XmlNode newVersionName = ApplyRevision(versionName, version);
                Console.WriteLine("Updating android:versionName attribute to {0}", newVersionName.Value);
                manifestNode.Attributes.SetNamedItem(newVersionName);
                Console.WriteLine("Writing out updated manifest file: {0}", fileName);
                xmldoc.Save(fileName);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Did not find android:versionName attribute, aborting!");
                Console.ResetColor();
            }
            Console.ResetColor();
            Console.WriteLine("Done");
        }

        private static XmlNode ApplyRevision(XmlNode versionNameAttribute, Version version)
        {
            versionNameAttribute.Value = version.ToString();
            return versionNameAttribute;
        }
    }
}
