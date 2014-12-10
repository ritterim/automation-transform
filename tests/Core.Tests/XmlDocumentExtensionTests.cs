using System.Collections.Generic;
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
            var document = Document.InsertCustomErrorsSetting("ON", "/error", customErrors =>
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
            var document = Document.ReplaceCustomErrorsSetting("ON", "/error", customErrors =>
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
    }
}
