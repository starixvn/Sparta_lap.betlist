namespace Fanex.Athena.Extension.Utility
{
    using System.Xml.Linq;
    using Interfaces;

    public class XDocumentWrapper : IXDocumentWrapper
    {
        public XDocument Load(string filePath)
        {
            return XDocument.Load(filePath);
        }
    }
}