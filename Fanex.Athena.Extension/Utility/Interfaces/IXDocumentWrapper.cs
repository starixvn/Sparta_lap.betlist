namespace Fanex.Athena.Extension.Utility.Interfaces
{
    using System.Xml.Linq;

    public interface IXDocumentWrapper
    {
        XDocument Load(string filePath);
    }
}