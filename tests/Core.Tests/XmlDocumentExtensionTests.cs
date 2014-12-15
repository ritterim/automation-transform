using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Xml;
using Xunit;

namespace RimDev.Automation.Transform
{
    public class XmlDocumentExtensionTests
    {
        public XmlDocument Document { get; set; }

        public XmlDocumentExtensionTests()
        {
            Document = new XmlDocument();
            Document.LoadXml(ConfigurationTransformer.EmptyTransform);
        }

        [Fact]
        public void Can_add_insert_app_setting()
        {
            Document.InsertAppSetting("test", "case");
            var node = Document.SelectSingleNode("//configuration/appSettings/add[1]");
            Assert.NotNull(node);
            Assert.Equal("test", node.Attributes["key"].Value);
            Assert.Equal("case", node.Attributes["value"].Value);
            Assert.Equal("Insert", node.Attributes["Transform", ConfigurationTransformer.TransformNamespace].Value);
        }

        [Fact]
        public void Can_add_replace_app_setting()
        {
            Document.ReplaceAppSetting("test", "case");
            var node = Document.SelectSingleNode("//configuration/appSettings/add[1]");
            Assert.NotNull(node);
            Assert.Equal("test", node.Attributes["key"].Value);
            Assert.Equal("case", node.Attributes["value"].Value);
            Assert.Equal("Replace", node.Attributes["Transform", ConfigurationTransformer.TransformNamespace].Value);
        }

        [Fact]
        public void Can_add_insert_connection_string()
        {
            Document.InsertSqlConnectionString("test", "case");
            var node = Document.SelectSingleNode("//configuration/connectionStrings/add[1]");
            Assert.NotNull(node);
            Assert.Equal("test", node.Attributes["name"].Value);
            Assert.Equal("case", node.Attributes["connectionString"].Value);
            Assert.Equal("System.Data.SqlClient", node.Attributes["providerName"].Value);
            Assert.Equal("Insert", node.Attributes["Transform", ConfigurationTransformer.TransformNamespace].Value);
        }

        [Fact]
        public void Can_add_replace_connection_string()
        {
            Document.ReplaceSqlConnectionString("test", "case");
            var node = Document.SelectSingleNode("//configuration/connectionStrings/add[1]");
            Assert.NotNull(node);
            Assert.Equal("test", node.Attributes["name"].Value);
            Assert.Equal("case", node.Attributes["connectionString"].Value);
            Assert.Equal("System.Data.SqlClient", node.Attributes["providerName"].Value);
            Assert.Equal("Replace", node.Attributes["Transform", ConfigurationTransformer.TransformNamespace].Value);
        }

        [Fact]
        public void Can_add_insert_customError_setting()
        {
            Document.InsertCustomErrorsSetting("ON", "/error", customErrors =>
            {
                customErrors.AddError(400, "~/error/400");
                customErrors.AddError(500, "~/error/500");
            });

            var node = Document.SelectSingleNode("//configuration/system.web[1]");
            Assert.NotNull(node);
            node = Document.SelectSingleNode("//configuration/system.web/customErrors[1]");
            Assert.NotNull(node);
            Assert.Equal("ON", node.Attributes["mode"].Value);
            Assert.Equal("/error", node.Attributes["defaultRedirect"].Value);
            Assert.Equal("Insert", node.Attributes["Transform", ConfigurationTransformer.TransformNamespace].Value);

            Assert.Equal(2, node.ChildNodes.Count);
            Assert.Equal("400", node.FirstChild.Attributes["statusCode"].Value);
            Assert.Equal("~/error/400", node.FirstChild.Attributes["redirect"].Value);

            Assert.Equal("500", node.LastChild.Attributes["statusCode"].Value);
            Assert.Equal("~/error/500", node.LastChild.Attributes["redirect"].Value);
        }

        [Fact]
        public void Can_add_replace_customError_setting()
        {
            Document.ReplaceCustomErrorsSetting("ON", "/error", customErrors =>
            {
                customErrors.AddError(400, "~/error/400");
                customErrors.AddError(500, "~/error/500");
                });

            var node = Document.SelectSingleNode("//configuration/system.web[1]");
            Assert.NotNull(node);
            node = Document.SelectSingleNode("//configuration/system.web/customErrors[1]");
            Assert.NotNull(node);
            Assert.Equal("ON", node.Attributes["mode"].Value);
            Assert.Equal("/error", node.Attributes["defaultRedirect"].Value);
            Assert.Equal("Replace", node.Attributes["Transform", ConfigurationTransformer.TransformNamespace].Value);

            Assert.Equal(2, node.ChildNodes.Count);
            Assert.Equal("400", node.FirstChild.Attributes["statusCode"].Value);
            Assert.Equal("~/error/400", node.FirstChild.Attributes["redirect"].Value);

            Assert.Equal("500", node.LastChild.Attributes["statusCode"].Value);
            Assert.Equal("~/error/500", node.LastChild.Attributes["redirect"].Value);
        }

        [Fact]
        public void Can_add_insert_customError_setting_systemWeb_exists()
        {
            var systemWebNode = Document.CreateNode(XmlNodeType.Element, "system.web", "");
            Document.DocumentElement.AppendChild(systemWebNode);
            var document = Document.InsertCustomErrorsSetting("ON", "/error", customErrors =>
            {
                customErrors.AddError(400, "~/error/400");
                customErrors.AddError(500, "~/error/500");
            });
            var nodes = document.SelectNodes("//configuration/system.web");
            Assert.Equal(1,nodes.Count); 
        }

        [Fact]
        public void Can_add_replace_customError_setting_systemWeb_exists()
        {
            var systemWebNode = Document.CreateNode(XmlNodeType.Element, "system.web", "");
            Document.DocumentElement.AppendChild(systemWebNode);
            var document = Document.ReplaceCustomErrorsSetting("ON", "/error", customErrors =>
            {
                customErrors.AddError(400, "~/error/400");
                customErrors.AddError(500, "~/error/500");
            });
            var nodes = document.SelectNodes("//configuration/system.web");
            Assert.Equal(1, nodes.Count);
        }

        [Fact]
        public void Can_add_insert_customError_setting_systemWeb_customerrors_exists()
        {
            var systemWebNode = Document.CreateNode(XmlNodeType.Element, "system.web", "");
            var customErrorsSettings = Document.CreateElement("customErrors");
            systemWebNode.AppendChild(customErrorsSettings);
            Document.DocumentElement.AppendChild(systemWebNode);
            var document = Document.InsertCustomErrorsSetting("ON", "/error", customErrors =>
            {
                customErrors.AddError(400, "~/error/400");
                customErrors.AddError(500, "~/error/500");
            });
            var nodes = document.SelectNodes("//configuration/system.web");
            Assert.Equal(1, nodes.Count);
            nodes = document.SelectNodes("//configuration/system.web/customErrors");
            Assert.Equal(1, nodes.Count);
        }

        [Fact]
        public void Can_add_replace_customError_setting_systemWeb_customerrors_exists()
        {
            var systemWebNode = Document.CreateNode(XmlNodeType.Element, "system.web", "");
            var customErrorsSettings = Document.CreateElement("customErrors");
            systemWebNode.AppendChild(customErrorsSettings);
            Document.DocumentElement.AppendChild(systemWebNode);
            var document = Document.ReplaceCustomErrorsSetting("ON", "/error", customErrors =>
            {
                customErrors.AddError(400, "~/error/400");
                customErrors.AddError(500, "~/error/500");
            });
            var nodes = document.SelectNodes("//configuration/system.web");
            Assert.Equal(1, nodes.Count);
            nodes = document.SelectNodes("//configuration/system.web/customErrors");
            Assert.Equal(1, nodes.Count);
        }

        [Fact]
        public void Can_add_insert_smtp_setting_defaultValues()
        {
            Document.InsertSmtpSetting(smtpBuilder: smtpBuilder =>
            {
                smtpBuilder.AddSpecifiedPickupDirectory(@"c:\email");
                smtpBuilder.AddNetwork();
            });

            var node = Document.SelectSingleNode("//configuration/system.net[1]");
            Assert.NotNull(node);
            node = Document.SelectSingleNode("//configuration/system.net/mailSettings[1]");
            Assert.NotNull(node);
            node = Document.SelectSingleNode("//configuration/system.net/mailSettings/smtp[1]");
            Assert.NotNull(node);
            Assert.Equal(null, node.Attributes["smtpDeliveryMethod"]);
            Assert.Equal(null, node.Attributes["smtpDeliveryFormat"]);
            Assert.Equal(null, node.Attributes["from"]);
            Assert.Equal("Insert", node.Attributes["Transform", ConfigurationTransformer.TransformNamespace].Value);

            Assert.Equal(2, node.ChildNodes.Count);
            Assert.Equal(@"c:\email", node.FirstChild.Attributes["pickupDirectoryLocation"].Value);

            Assert.Equal(null, node.LastChild.Attributes["clientDomain"]);
            Assert.Equal("False",node.LastChild.Attributes["defaultCredentials"].Value);
            Assert.Equal("False", node.LastChild.Attributes["enableSSL"].Value);
            Assert.Equal(null, node.LastChild.Attributes["host"]);
            Assert.Equal(null, node.LastChild.Attributes["password"]);
            Assert.Equal("25", node.LastChild.Attributes["port"].Value);
            Assert.Equal(null, node.LastChild.Attributes["targetName"]);
            Assert.Equal(null, node.LastChild.Attributes["userName"]);
        }

        [Fact]
        public void Can_add_insert_smtp_setting()
        {
            Document.InsertSmtpSetting(SmtpDeliveryFormat.SevenBit, SmtpDeliveryMethod.Network, "test@test.com", smtpBuilder =>
            {
                smtpBuilder.AddSpecifiedPickupDirectory(@"c:\email");
                smtpBuilder.AddNetwork("clientDomain", true, true, "host", "password", 180, "targetName", "userName");
            });

            var node = Document.SelectSingleNode("//configuration/system.net[1]");
            Assert.NotNull(node);
            node = Document.SelectSingleNode("//configuration/system.net/mailSettings[1]");
            Assert.NotNull(node);
            node = Document.SelectSingleNode("//configuration/system.net/mailSettings/smtp[1]");
            Assert.NotNull(node);

            Assert.Equal(SmtpDeliveryFormat.SevenBit.ToString(), node.Attributes["smtpDeliveryFormat"].Value);
            Assert.Equal(SmtpDeliveryMethod.Network.ToString(), node.Attributes["smtpDeliveryMethod"].Value);
            Assert.Equal("test@test.com", node.Attributes["from"].Value);
            Assert.Equal("Insert", node.Attributes["Transform", ConfigurationTransformer.TransformNamespace].Value);

            Assert.Equal(2, node.ChildNodes.Count);
            Assert.Equal(@"c:\email", node.FirstChild.Attributes["pickupDirectoryLocation"].Value);

            Assert.Equal("clientDomain", node.LastChild.Attributes["clientDomain"].Value);
            Assert.Equal("True", node.LastChild.Attributes["defaultCredentials"].Value);
            Assert.Equal("True", node.LastChild.Attributes["enableSSL"].Value);
            Assert.Equal("host", node.LastChild.Attributes["host"].Value);
            Assert.Equal("password", node.LastChild.Attributes["password"].Value);
            Assert.Equal("180", node.LastChild.Attributes["port"].Value);
            Assert.Equal("targetName", node.LastChild.Attributes["targetName"].Value);
            Assert.Equal("userName", node.LastChild.Attributes["userName"].Value);
        }
    }
}
