using System;
using System.Runtime.CompilerServices;
using System.Xml;
using AndroidManifestUtil.Services;

namespace AndroidManifestUtil
{
    public class AndroidManifest
    {
        private readonly XmlDocument _manifestDoc;
        private ILogger _logger;

        protected AndroidManifest(XmlDocument manifestDoc, ILogger logger)
        {
            _manifestDoc = manifestDoc;
            _logger = logger;
        }

        public static AndroidManifest Load(string manifestFilePath)
        {
            var logger = new ConsoleLogger();
            logger.Log($"Loading {manifestFilePath}", LogSeverity.Debug);

            var xmldoc = new XmlDocument();
            xmldoc.Load(manifestFilePath);

            logger.Log($"Loaded {manifestFilePath}", LogSeverity.Debug);

            return new AndroidManifest(xmldoc, logger);
        }

        public static AndroidManifest Load(XmlDocument manifestDoc)
        {
            return new AndroidManifest(manifestDoc, new ConsoleLogger());
        }

        public void UpdateAttribute(string nodeName, string attributeName, string newValue)
        {
            var node = GetNode(nodeName);
            var attribute = GetAttribute(node, attributeName);

            if (attribute != null)
            {
                _logger.Log($"Found {attributeName} attribute: {attribute.Value}");

                attribute.Value = $"{newValue}";

                _logger.Log($"Updating {attributeName} attribute to {attribute.Value}");

                node.Attributes?.SetNamedItem(attribute);
            }
            else
            {
                _logger.Log($"Did not find {attributeName} attribute, aborting!", LogSeverity.Error);
            }

            _logger.Log("Done", LogSeverity.Debug);
        }

        private XmlNode GetAttribute(XmlNode node, string attributeName)
        {
            _logger.Log($"Getting attribute {attributeName} ...");

            var attributes = node?.Attributes;
            var attribute = attributes?.GetNamedItem(attributeName);

            return attribute;
        }

        public void UpdateVersionCode(Version version)
        {
            UpdateAttribute("manifest", "android:versionCode", version.Revision.ToString());
        }

        public void UpdateVersionName(Version version)
        {
            UpdateAttribute("manifest", "android:versionName", version.ToString());
        }

        public void SaveFile(string fileName)
        {
            _logger.Log($"Writing out updated manifest file: {fileName}");
            _manifestDoc.Save(fileName);
        }

        private XmlNode GetNode(string nodeName)
        {
            _logger.Log($"Loading {nodeName} node...");

            var manifestNode = _manifestDoc.SelectSingleNode($"/{nodeName}");

            return manifestNode;
        }
    }
}