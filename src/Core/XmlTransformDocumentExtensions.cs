using System;
using System.Collections.Generic;
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

        public static XmlDocument InsertCustomErrorsSetting(this XmlDocument document, string mode, string defaultRedirect, Action<CustomErrorBuilder> builder = null)
        {
            var systemWeb = document.DocumentElement.SelectSingleNode("system.web") ??
                            document.CreateNode(XmlNodeType.Element, "system.web", "");

            var customErrorsSettings = document.CreateElement("customErrors");//here i am assuming that customErrors doesnt already exist. what if they just wanted to insert an <errors> child?
            customErrorsSettings.SetAttribute("mode", mode);
            customErrorsSettings.SetAttribute("defaultRedirect", defaultRedirect);
            customErrorsSettings.SetAttribute("Transform", ConfigurationTransformer.TransformNamespace, "Insert");

            if (builder != null)
                builder(new CustomErrorBuilder(document, customErrorsSettings));

            systemWeb.AppendChild(customErrorsSettings);
            document.DocumentElement.AppendChild(systemWeb); //if this already existed i probably don't want to do this?

            return document;
        }

        public static XmlDocument ReplaceCustomErrorsSetting(this XmlDocument document, string mode, string defaultRedirect, Action<CustomErrorBuilder> builder = null)
        {
            var systemWeb = document.DocumentElement.SelectSingleNode("system.web") ??
                            document.CreateNode(XmlNodeType.Element, "system.web", "");

            var customErrorsSettings = document.CreateElement("customErrors");
            customErrorsSettings.SetAttribute("mode", mode);
            customErrorsSettings.SetAttribute("defaultRedirect", defaultRedirect);
            customErrorsSettings.SetAttribute("Transform", ConfigurationTransformer.TransformNamespace, "Replace");
            customErrorsSettings.SetAttribute("Locator", ConfigurationTransformer.TransformNamespace, "Match(name)");

            if (builder != null)
                builder(new CustomErrorBuilder(document, customErrorsSettings));
            
            systemWeb.AppendChild(customErrorsSettings);
            document.DocumentElement.AppendChild(systemWeb);

            return document;
        }

        public class CustomErrorBuilder
        {
            private readonly XmlDocument _document;
            private readonly XmlElement _customErrorElement;

            public CustomErrorBuilder(XmlDocument document, XmlElement customErrorElement)
            {
                _document = document;
                _customErrorElement = customErrorElement;
            }

            public CustomErrorBuilder AddError(int statusCode, string redirect)
            {
                var element = _document.CreateElement("error");
                element.SetAttribute("statusCode", statusCode.ToString());
                element.SetAttribute("redirect", redirect);
                _customErrorElement.AppendChild(element);

                return this;
            }

            public XmlDocument Done()
            {
                return _document;
            }
        }
    }
}
