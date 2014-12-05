using System;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Web.XmlTransform;

namespace Automation.Transform
{
    public class ConfigurationTransformer : IDisposable
    {
        public const string TransformNamespace = "http://schemas.microsoft.com/XML-Document-Transform";
        public XmlTransformableDocument Source { get; protected set; }
        public XmlDocument Transform { get; protected set; }
        public static string EmptyTransform { get { return "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration xmlns:xdt=\"http://schemas.microsoft.com/XML-Document-Transform\"><appSettings/><connectionStrings/></configuration>";} }

        public ConfigurationTransformer()
        {
            Source = new XmlTransformableDocument();
            Transform = new XmlDocument();
            SetTransformFromString(EmptyTransform);
        }

        public ConfigurationTransformer SetSourceFromFile(string filePath)
        {
            Source.Load(filePath);
            return this;
        }

        public ConfigurationTransformer SetSourceFromString(string xml)
        {
            Source.LoadXml(xml);
            return this;
        }

        public ConfigurationTransformer SetTranformFromFile(string filePath)
        {
            Transform = new XmlDocument();
            Transform.Load(filePath);
            return this;
        }

        public ConfigurationTransformer SetTransformFromString(string xml)
        {
            Transform.LoadXml(xml);
            return this;
        }

        public XmlDocument Apply()
        {
            using (var tranform = new XmlTransformation(new MemoryStream(Encoding.UTF8.GetBytes(Transform.InnerXml)), new TraceTransformationLogger()))
            {
                if (tranform.Apply(Source))
                {
                    return Source;
                }

                throw new ApplicationException("the transformation was not successful");
            }
        }

        public XmlDocument Apply(string savePath)
        {
            var result = Apply();
            result.Save(savePath);
            return result;
        }

        public void Dispose()
        {
            Source.Dispose();
        }
    }
}
