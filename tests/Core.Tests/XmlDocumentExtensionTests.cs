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
            var errors = new Dictionary<string, string> {{"404", "/404NotFound"}, {"500", "/500InternalServerError"}};
            Document.InsertCustomErrorsSetting("ON","http://www.ritterim.com",errors);
            var node = Document.SelectSingleNode("//configuration/system.web/customErrors[1]");
            Assert.NotNull(node);
            Assert.Equal("ON", node.Attributes["mode"].Value);
            Assert.Equal("http://www.ritterim.com", node.Attributes["defaultRedirect"].Value);
            Assert.Equal("Insert", node.Attributes["Transform", ConfigurationTransformer.TransformNamespace].Value);

            Assert.Equal(2, node.ChildNodes.Count);
            Assert.Equal("Insert", node.FirstChild.Attributes["Transform", ConfigurationTransformer.TransformNamespace].Value);
            Assert.Equal("404", node.FirstChild.Attributes["statusCode"].Value);
            Assert.Equal("/404NotFound", node.FirstChild.Attributes["redirect"].Value);

            Assert.Equal("Insert", node.LastChild.Attributes["Transform", ConfigurationTransformer.TransformNamespace].Value);
            Assert.Equal("500", node.LastChild.Attributes["statusCode"].Value);
            Assert.Equal("/500InternalServerError", node.LastChild.Attributes["redirect"].Value);
        }
    }
}
