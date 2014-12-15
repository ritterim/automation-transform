using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net.Mail;
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
            document = ReplaceConnectionString(document, name, connectionString, "System.Data.SqlClient");
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

            if (builder != null)
                builder(new CustomErrorBuilder(document, customErrors));

            systemWeb.AppendChild(customErrors);
            document.DocumentElement.AppendChild(systemWeb);

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

        public static XmlDocument InsertSmtpSetting(this XmlDocument document, SmtpDeliveryFormat? smtpDeliveryFormat = null, SmtpDeliveryMethod? smtpDeliveryMethod = null, string from = null,
            Action<SmtpBuilder> smtpBuilder = null)
        {
            var systemNet = document.DocumentElement.SelectSingleNode("system.net") ??
                document.CreateNode(XmlNodeType.Element, "system.net", "");
            var mailSettingsNode = systemNet.SelectSingleNode("mailSettings") ??
                               document.CreateNode(XmlNodeType.Element, "mailSettings", "");
            var smtpNode = systemNet.SelectSingleNode("smtp") ??
                               document.CreateNode(XmlNodeType.Element, "smtp", "");

            XmlAttribute attribute;
            if (smtpDeliveryFormat != null)
            {
                attribute = document.CreateAttribute("smtpDeliveryFormat");
                attribute.Value = smtpDeliveryFormat.ToString();
                smtpNode.Attributes.Append(attribute);
            }
            if (smtpDeliveryMethod != null)
            {
                attribute = document.CreateAttribute("smtpDeliveryMethod");
                attribute.Value = smtpDeliveryMethod.ToString();
                smtpNode.Attributes.Append(attribute);
            }
            if (from != null)
            {
                attribute = document.CreateAttribute("from");
                attribute.Value = from;
                smtpNode.Attributes.Append(attribute);
            }
            attribute = document.CreateAttribute("Transform", ConfigurationTransformer.TransformNamespace);
            attribute.Value = "Insert";
            smtpNode.Attributes.Append(attribute);
        public static XmlDocument ReplaceSmtpSetting(this XmlDocument document, SmtpDeliveryFormat? smtpDeliveryFormat = null, SmtpDeliveryMethod? smtpDeliveryMethod = null, string from = null,
            Action<SmtpBuilder> smtpBuilder = null)
        {
            var systemNet = document.DocumentElement.SelectSingleNode("system.net") ??
                document.CreateNode(XmlNodeType.Element, "system.net", "");
            var mailSettingsNode = systemNet.SelectSingleNode("mailSettings") ??
                               document.CreateNode(XmlNodeType.Element, "mailSettings", "");
            var smtpNode = mailSettingsNode.SelectSingleNode("smtp") ??
                               document.CreateNode(XmlNodeType.Element, "smtp", "");

            XmlAttribute attribute;
            if (smtpDeliveryFormat != null)
            {
                attribute = document.CreateAttribute("smtpDeliveryFormat");
                attribute.Value = smtpDeliveryFormat.ToString();
                smtpNode.Attributes.Append(attribute);
            }
            if (smtpDeliveryMethod != null)
            {
                attribute = document.CreateAttribute("smtpDeliveryMethod");
                attribute.Value = smtpDeliveryMethod.ToString();
                smtpNode.Attributes.Append(attribute);
            }
            if (from != null)
            {
                attribute = document.CreateAttribute("from");
                attribute.Value = from;
                smtpNode.Attributes.Append(attribute);
            }
            attribute = document.CreateAttribute("Transform", ConfigurationTransformer.TransformNamespace);
            attribute.Value = "Replace";
            smtpNode.Attributes.Append(attribute);
            attribute = document.CreateAttribute("Locator", ConfigurationTransformer.TransformNamespace);
            attribute.Value = "Match(name)";
            smtpNode.Attributes.Append(attribute);

            if (smtpBuilder != null)
                smtpBuilder(new SmtpBuilder(document, smtpNode));

            mailSettingsNode.AppendChild(smtpNode);
            systemNet.AppendChild(mailSettingsNode);
            document.DocumentElement.AppendChild(systemNet);

            return document;
        }


        public class SmtpBuilder
        {
            private readonly XmlDocument _document;
            private readonly XmlNode _smtpElement;

            public SmtpBuilder(XmlDocument document, XmlNode smtpElement)
            {
                _document = document;
                _smtpElement = smtpElement;
            }

            public SmtpBuilder AddSpecifiedPickupDirectory(string pickupDirectoryLocation)
            {
                var element = _document.CreateElement("specifiedPickupDirectory");
                element.SetAttribute("pickupDirectoryLocation", pickupDirectoryLocation);
                _smtpElement.AppendChild(element);

                return this;
            }
            public SmtpBuilder AddNetwork(string clientDomain = null, bool defaultCredentials = false, bool enableSsl = false, string host = null, string password = null,
                int port = 25, string targetName = null, string userName = null)
            {
                var element = _document.CreateElement("network");
                if (clientDomain != null) { element.SetAttribute("clientDomain", clientDomain); }
                element.SetAttribute("defaultCredentials", defaultCredentials.ToString());
                element.SetAttribute("enableSSL", enableSsl.ToString());
                if (host!=null){ element.SetAttribute("host", host); }
                if (password!=null){element.SetAttribute("password", password);}
                element.SetAttribute("port", port.ToString());
                if (targetName!=null){element.SetAttribute("targetName", targetName);}
                if (userName!=null){element.SetAttribute("userName", userName);}
                _smtpElement.AppendChild(element);

                return this;
            }
        }
    }
}
