using System;
using System.Collections.Generic;
using System.Xml;

namespace RimDev.Automation.Transform
{
    public static class XmlTransformDocumentExtensions
    {
        public static XmlDocument InsertAppSetting(this XmlDocument document, string key, string value)
        {
            var appSettings = document.DocumentElement.SelectSingleNode("appSettings");

            var node = document.CreateElement("add");
            node.SetAttribute("key", key);
            node.SetAttribute("value", value);
            node.SetAttribute("Transform", ConfigurationTransformer.TransformNamespace, "Insert");

            appSettings.AppendChild(node);
            return document;
        }

        public static XmlDocument ReplaceAppSetting(this XmlDocument document, string key, string value)
        {
            var appSettings = document.DocumentElement.SelectSingleNode("appSettings");

            var node = document.CreateElement("add");
            node.SetAttribute("key", key);
            node.SetAttribute("value", value);
            node.SetAttribute("Transform", ConfigurationTransformer.TransformNamespace, "Replace");
            node.SetAttribute("Locator", ConfigurationTransformer.TransformNamespace, "Match(key)");

            appSettings.AppendChild(node);
            return document;
        }

        public static XmlDocument InsertConnectionString(this XmlDocument document, string name, string connectionString, string providerName = null)
        {
            var appSettings = document.DocumentElement.SelectSingleNode("connectionStrings");

            var node = document.CreateElement("add");
            node.SetAttribute("name", name);
            node.SetAttribute("connectionString", connectionString);

            if (!string.IsNullOrWhiteSpace(providerName))
                node.SetAttribute("providerName", providerName);

            node.SetAttribute("Transform", ConfigurationTransformer.TransformNamespace, "Insert");

            appSettings.AppendChild(node);
            return document;
        }

        public static XmlDocument ReplaceConnectionString(this XmlDocument document, string name, string connectionString, string providerName = null)
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
            return document;
        }

        public static XmlDocument InsertSqlConnectionString(this XmlDocument document, string name, string connectionString)
        {
            document = InsertConnectionString(document, name, connectionString, "System.Data.SqlClient");
            return document;
        }

        public static XmlDocument ReplaceSqlConnectionString(this XmlDocument document, string name, string connectionString)
        {
            document= ReplaceConnectionString(document, name, connectionString, "System.Data.SqlClient");
            return document;
        }

        public static XmlDocument InsertCustomErrorsSetting(this XmlDocument document, string mode, string defaultRedirect, Action<CustomErrorBuilder> builder = null)
        {
            var systemWeb = document.DocumentElement.SelectSingleNode("system.web") ??
                            document.CreateNode(XmlNodeType.Element, "system.web", "");
            var customErrors = systemWeb.SelectSingleNode("customErrors") ??
                               document.CreateNode(XmlNodeType.Element, "customErrors", "");

            var attribute = document.CreateAttribute("mode");
            attribute.Value = mode;
            customErrors.Attributes.Append(attribute);
            attribute = document.CreateAttribute("defaultRedirect");
            attribute.Value = defaultRedirect;
            customErrors.Attributes.Append(attribute);
            attribute = document.CreateAttribute("Transform", ConfigurationTransformer.TransformNamespace);
            attribute.Value = "Insert";
            customErrors.Attributes.Append(attribute);

            //var customErrorsSettings = document.CreateElement("customErrors");//here i am assuming that customErrors doesnt already exist. what if they just wanted to insert an <errors> child?
            //customErrorsSettings.SetAttribute("mode", mode);
            //customErrorsSettings.SetAttribute("defaultRedirect", defaultRedirect);
            //customErrorsSettings.SetAttribute("Transform", ConfigurationTransformer.TransformNamespace, "Insert");

            if (builder != null)
                builder(new CustomErrorBuilder(document, customErrors));

            systemWeb.AppendChild(customErrors);
            document.DocumentElement.AppendChild(systemWeb); //if this already existed i probably don't want to do this?

            return document;
        }

        public static XmlDocument ReplaceCustomErrorsSetting(this XmlDocument document, string mode, string defaultRedirect, Action<CustomErrorBuilder> builder = null)
        {
            var systemWeb = document.DocumentElement.SelectSingleNode("system.web") ??
                            document.CreateNode(XmlNodeType.Element, "system.web", "");
            var customErrors = systemWeb.SelectSingleNode("customErrors") ??
                               document.CreateNode(XmlNodeType.Element, "customErrors", "");

            var attribute = document.CreateAttribute("mode");
            attribute.Value = mode;
            customErrors.Attributes.Append(attribute);
            attribute = document.CreateAttribute("defaultRedirect");
            attribute.Value = defaultRedirect;
            customErrors.Attributes.Append(attribute);
            attribute = document.CreateAttribute("Transform", ConfigurationTransformer.TransformNamespace);
            attribute.Value = "Replace";
            customErrors.Attributes.Append(attribute);
            attribute = document.CreateAttribute("Locator", ConfigurationTransformer.TransformNamespace);
            attribute.Value = "Match(name)";
            customErrors.Attributes.Append(attribute);

            if (builder != null)
                builder(new CustomErrorBuilder(document, customErrors));

            systemWeb.AppendChild(customErrors);
            document.DocumentElement.AppendChild(systemWeb);

            return document;
        }

        public class CustomErrorBuilder
        {
            private readonly XmlDocument _document;
            private readonly XmlNode _customErrorElement;

            public CustomErrorBuilder(XmlDocument document, XmlNode customErrorElement)
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
        }
    }
}
