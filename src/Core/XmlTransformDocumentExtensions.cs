using System.Xml;

namespace RimDev.Automation.Transform
{
    public static class XmlTransformDocumentExtensions
    {
        public static void InsertAppSetting(this XmlDocument document, string key, string value)
        {
            var appSettings = document.DocumentElement.SelectSingleNode("appSettings");

            var node = document.CreateElement("add");
            node.SetAttribute("key", key);
            node.SetAttribute("value", value);
            node.SetAttribute("Transform", ConfigurationTransformer.TransformNamespace, "Insert");

            appSettings.AppendChild(node);
        }

        public static void ReplaceAppSetting(this XmlDocument document, string key, string value)
        {
            var appSettings = document.DocumentElement.SelectSingleNode("appSettings");

            var node = document.CreateElement("add");
            node.SetAttribute("key", key);
            node.SetAttribute("value", value);
            node.SetAttribute("Transform", ConfigurationTransformer.TransformNamespace, "Replace");
            node.SetAttribute("Locator", ConfigurationTransformer.TransformNamespace, "Match(key)");

            appSettings.AppendChild(node);
        }

        public static void InsertConnectionString(this XmlDocument document, string name, string connectionString, string providerName = null)
        {
            var appSettings = document.DocumentElement.SelectSingleNode("connectionStrings");

            var node = document.CreateElement("add");
            node.SetAttribute("name", name);
            node.SetAttribute("connectionString", connectionString);

            if (!string.IsNullOrWhiteSpace(providerName))
                node.SetAttribute("providerName", providerName);

            node.SetAttribute("Transform", ConfigurationTransformer.TransformNamespace, "Insert");

            appSettings.AppendChild(node);
        }

        public static void ReplaceConnectionString(this XmlDocument document, string name, string connectionString, string providerName = null)
        {
            var appSettings = document.DocumentElement.SelectSingleNode("connectionStrings");

            var node = document.CreateElement("add");
            node.SetAttribute("name", name);
            node.SetAttribute("connectionString", connectionString);

            if (!string.IsNullOrWhiteSpace(providerName))
                node.SetAttribute("providerName", providerName);

            node.SetAttribute("Transform", ConfigurationTransformer.TransformNamespace, "Replace");
            node.SetAttribute("Locator", ConfigurationTransformer.TransformNamespace, "Match(name)");

            appSettings.AppendChild(node);
        }

        public static void InsertSqlConnectionString(this XmlDocument document, string name, string connectionString)
        {
            InsertConnectionString(document, name, connectionString, "System.Data.SqlClient");
        }

        public static void ReplaceSqlConnectionString(this XmlDocument document, string name, string connectionString)
        {
            ReplaceConnectionString(document, name, connectionString, "System.Data.SqlClient");
        }
    }
}
