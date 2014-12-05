using Xunit;

namespace Automation.Transform.Tests
{
    public class ConfigurationTransformerTests
    {
        [Fact]
        public void Can_Create_ConfigurationTransformer()
        {
            using (var transformer = new ConfigurationTransformer())
            {
                Assert.NotNull(transformer);
            }
        }

        [Fact]
        public void Can_load_source_xml_from_string()
        {
            using (var transformer = new ConfigurationTransformer())
            {
                const string expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration></configuration>";

                transformer.SetSourceFromString(expected);
                Assert.Equal(expected, transformer.Source.InnerXml);
            }
        }

        [Fact]
        public void Can_load_source_xml_from_file()
        {
            using (var transformer = new ConfigurationTransformer())
            {
                const string expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration></configuration>";

                transformer.SetSourceFromFile("app.config");
                Assert.Equal(expected, transformer.Source.InnerXml);
            }
        }

        [Fact]
        public void Can_load_transform_xml_from_string()
        {
            using (var transformer = new ConfigurationTransformer())
            {
                const string expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration></configuration>";

                transformer.SetTransformFromString(expected);
                Assert.Equal(expected, transformer.Transform.InnerXml);
            }
        }

        [Fact]
        public void Can_load_trasnform_xml_from_file()
        {
            using (var transformer = new ConfigurationTransformer())
            {
                const string expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration></configuration>";

                transformer.SetTranformFromFile("app.transform.config");
                Assert.Equal(expected, transformer.Transform.InnerXml);
            }
        }

        [Fact]
        public void Can_apply_configuration_transform()
        {
            using (var transformer = new ConfigurationTransformer())
            {
                const string source = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration></configuration>";
                const string transform = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\"><appSettings xdt:Transform=\"Insert\"></appSettings></configuration>";
                const string expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration><appSettings></appSettings></configuration>";

                var result = transformer
                    .SetSourceFromString(source)
                    .SetTransformFromString(transform)
                    .Apply();

                Assert.Equal(expected, result.InnerXml);
            }
        }

        [Fact]
        public void Can_apply_configuration_transform_via_modified_transform()
        {
            using (var transformer = new ConfigurationTransformer())
            {
                const string expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration><appSettings><add key=\"hello\" value=\"world\" /></appSettings></configuration>";
                const string source = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration><appSettings></appSettings></configuration>";

                transformer
                    .SetSourceFromString(source)
                    .Transform.InsertAppSetting("hello", "world");

                var result = transformer.Apply();

                Assert.Equal(expected, result.InnerXml);
            }
        }
    }
}
