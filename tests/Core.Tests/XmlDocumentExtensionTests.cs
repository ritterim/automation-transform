using System.Xml;
using Xunit;

namespace Automation.Transform.Tests
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
    }
}
