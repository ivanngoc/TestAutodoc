using System.Reflection;
using System.Xml.XPath;

namespace IziHardGames.Libs.ForSwagger;

public class XmlRepo
{
    private readonly Dictionary<Assembly, XPathDocument> docs;
    private readonly Dictionary<Assembly, XPathNavigator> navigators = new Dictionary<Assembly, XPathNavigator>();
    public XPathNavigator this[Assembly key] => navigators[key];
    public XPathNavigator this[Type key] => navigators[key.Assembly];
    public XmlRepo(Dictionary<Assembly, XPathDocument> docs)
    {
        this.docs = docs;
        foreach (var item in docs)
        {
            navigators.Add(item.Key, item.Value.CreateNavigator());
        }
    }
}
