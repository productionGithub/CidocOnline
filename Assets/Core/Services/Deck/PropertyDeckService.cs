using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using UnityEngine;
using System.IO;
using System.Linq;
using StarterCore.Core.Services.Network;
using Zenject;
using StarterCore.Core.Scenes.Board.Card.Cards;
using Cysharp.Threading.Tasks;

public class PropertyDeckService : IInitializable
{
    /// <summary>
    /// Initialize List of property cards, based on Cidoc rdf file parsing (.xml).
    /// Provides public reference to deck instances.
    /// </summary>

    [Inject] MockNetService _netservice;

    //public Dictionary<int, PropertyCard> PropertyCards;
    public List<PropertyCard> PropertyCards;

    //Colors
    public Dictionary<string, Color32> ColorsDictionary;

    //Temporary dictionary for superClass deduction
    private Dictionary<string, List<string>> allSuperClassDic;

    //Icons enum
    //public enum Icons { Axis, Clock, Cube, Forms, Id, Idea, Location, Moebius, Ruler, User }

    //XPath parsing - Namespaces management -> Edit here if needed.
    private XmlNamespaceManager CidocCrmNamespaceManager;
    readonly private string rdfNamespace = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
    readonly private string rdfsNamespace = "http://www.w3.org/2000/01/rdf-schema#";
    readonly private string owlNamespace = "http://www.w3.org/2002/07/owl#";
    readonly private string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

    //Navigators allows to parse in memory XML file mapping
    XPathNavigator cidocRdfFileNavigator;//Source file for generating Entity and Property decks
    XPathNavigator propertyColorsFileNavigator;//Source file for updating Entity instances with ad-hoc icons and colors;

    //Raw xml string for Cidoc
    string _cidocXmlString;
    string _propertyXmlString;

    private Dictionary<string, List<string>> allSuperPropertyDic;


    public void Initialize()
    {
        PropertyCards = new List<PropertyCard>();

        InitColors();
        BuildPropertyDeck();
    }
    
    // <*************************** public methods ************************************>
    public List<PropertyCard> GetInitialDeck(string initString)
    {
        List<PropertyCard> partialDeck = new List<PropertyCard>();
        partialDeck.Clear();

        switch (initString[0])//string should starts with '*' or '-' to initiliaze with all ids or a partial list
        {
            case '*'://All cards
                partialDeck = PropertyCards;
                break;

            case '-'://Partial list of cards
                string wString = initString.Substring(1);
                string[] arrayOfIds = wString.Split(',').Select(id => id.Trim()).ToArray();
                List<string> listOfIds = arrayOfIds.ToList();
                partialDeck = PropertyCards.Where(c => listOfIds.Contains(c.id)).ToList();
                break;

            default:
                break;
        }

        return partialDeck;
    }


    // <********************            init of deck from CirdocCRM file         *********************>
    private async void BuildPropertyDeck()
    {
        //Fetch Cidoc Xml file
        _cidocXmlString = await _netservice.GetXmlCidocFile();

        //Fetch EntityIconsColorMapping Xml file
        _propertyXmlString = await _netservice.GetXmlPropertyColorsFile();

        InitXpathNavigators();
        InitPropertyDeck();//Is it necessary to await? To be tested.
    }

    private void InitXpathNavigators()
    {
        //Create Cidoc XML namespace manager
        NameTable nt = new NameTable();
        CidocCrmNamespaceManager = new XmlNamespaceManager(nt);
        CidocCrmNamespaceManager.AddNamespace("rdf", @rdfNamespace);
        CidocCrmNamespaceManager.AddNamespace("rdfs", @rdfsNamespace);
        CidocCrmNamespaceManager.AddNamespace("owl", @owlNamespace);
        CidocCrmNamespaceManager.AddNamespace("xml", @xmlNamespace);

        //Cidoc XML
        MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(_cidocXmlString));
        XPathDocument cidocXpathDocument = new XPathDocument(ms);
        cidocRdfFileNavigator = cidocXpathDocument.CreateNavigator();

        //PropertyColorsMapping
        MemoryStream msP = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(_propertyXmlString));
        XPathDocument propertyXpathDocument = new XPathDocument(msP);
        propertyColorsFileNavigator = propertyXpathDocument.CreateNavigator();
    }

    private void InitPropertyDeck()
    {
        //Colors
        //Create the list of colors with property.about as search key
        string rowExpression = "//row";
        XPathNodeIterator rows = (XPathNodeIterator)propertyColorsFileNavigator.Evaluate(rowExpression);

        int index = 0;

        //Get all 'Property' nodes
        string propertyExpression = "/rdf:RDF/rdf:Property";//Property xpath query
        XPathExpression propertyQuery = cidocRdfFileNavigator.Compile(propertyExpression);
        propertyQuery.SetContext(CidocCrmNamespaceManager);
        XPathNodeIterator properties = cidocRdfFileNavigator.Select(propertyQuery);

        //Initialize potentialSuperProperties dictionary with property names and empty list of potential subproperties for THAT property
        //Subprops will be populated on the next loop at property object creation time
        allSuperPropertyDic = new Dictionary<string, List<string>>();
        foreach (XPathNavigator property in properties)
        {
            allSuperPropertyDic.Add(property.GetAttribute("about", rdfNamespace), new List<string>());
        }

        foreach (XPathNavigator property in properties)
        {
            //Each property might be a subproperty of one or more super properties ('Parents')
            //We create a list of these potential 'Parents' properties
            List<string> superPropertiesList = new List<string>();

            XPathNodeIterator subProperties = property.Select("./rdfs:subPropertyOf", CidocCrmNamespaceManager);
            foreach (XPathNavigator sub in subProperties)
            {
                superPropertiesList.Add(sub?.GetAttribute("resource", rdfNamespace));//Build list of direct super property
            }

            PropertyCards.Add(new PropertyCard
            {
                index = index,
                id = property.GetAttribute("about", rdfNamespace).Split('_')[0],
                about = property.GetAttribute("about", rdfNamespace),
                label = property.SelectSingleNode("./rdfs:label[@xml:lang='en']", CidocCrmNamespaceManager).ToString(),
                comment = property.SelectSingleNode("./rdfs:comment", CidocCrmNamespaceManager)?.ToString(),
                domain = property.SelectSingleNode("./rdfs:domain", CidocCrmNamespaceManager)?.GetAttribute("resource", rdfNamespace).ToString(),
                range = property.SelectSingleNode("./rdfs:range", CidocCrmNamespaceManager)?.GetAttribute("resource", rdfNamespace),
                inverseOf = property.SelectSingleNode("./owl:inverseOf", CidocCrmNamespaceManager)?.GetAttribute("resource", rdfNamespace),
                domainColors = new List<string>(),
                rangeColors = new List<string>(),
                superProperties = superPropertiesList
            });


            ////Add this property as a sub property in the 'Parents' dictionary
            foreach (string supPropName in PropertyCards[index].superProperties)
            {
                if (PropertyCards[index].superProperties.Count > 0)
                {
                    if (allSuperPropertyDic.Keys.Contains(supPropName))
                    {
                        allSuperPropertyDic[supPropName].Add(PropertyCards[index].about);
                    }
                }
            }

            //about field
            string propAbout = property.GetAttribute("about", rdfNamespace);

            //Colors
            List<string> dColors = new List<string>();//domainColors
            List<string> rColors = new List<string>();//rangeColors

            foreach (XPathNavigator row in rows)
            {
                string about = row.SelectSingleNode("./Property_About").ToString();
                if (propAbout == about)
                {
                    XPathItem domainColor1 = row.SelectSingleNode("./Domain_Colour_1");
                    XPathItem domainColor2 = row.SelectSingleNode("./Domain_Colour_2");
                    XPathItem domainColor3 = row.SelectSingleNode("./Domain_Colour_3");

                    XPathItem rangeColor1 = row.SelectSingleNode("./Range_Colour_1");
                    XPathItem rangeColor2 = row.SelectSingleNode("./Range_Colour_2");
                    XPathItem rangeColor3 = row.SelectSingleNode("./Range_Colour_3");

                    if (domainColor1.ToString() != "")
                    {
                        dColors.Add((string)domainColor1.ToString());
                    }
                    if (domainColor2.ToString() != "")
                    {
                        dColors.Add((string)domainColor2.ToString());
                    }
                    if (domainColor3.ToString() != "")
                    {
                        dColors.Add((string)domainColor3.ToString());
                    }

                    if (rangeColor1.ToString() != "") rColors.Add((string)rangeColor1.ToString());
                    if (rangeColor2.ToString() != "") rColors.Add((string)rangeColor2.ToString());
                    if (rangeColor3.ToString() != "") rColors.Add((string)rangeColor3.ToString());

                    PropertyCards[index].domainColors = new List<string>(dColors);
                    PropertyCards[index].rangeColors = new List<string>(rColors);

                    break;
                }
            }

            dColors.Clear();
            rColors.Clear();
            index++;
        }

        //All properties 'subPropertyOf' has been parsed and stored in the allSuperPropertyDic
        //Updating subProperties field with allSuperPropertyDic entries previously set
        index = 0;
        foreach (KeyValuePair<string, List<string>> kvp in allSuperPropertyDic)
        {
            foreach (string name in kvp.Value)
            {
                PropertyCards[index].subProperties.Add(name);
            }
            index++;
        }
    }

    //Mapping Color32 / Name of colors
    private void InitColors()
    {
        ColorsDictionary = new Dictionary<string, Color32>()
        {
            {"White", new Color32(255, 255, 255, 255) },
            {"Blue", new Color32(130, 195, 236, 255) },
            {"Brown", new Color32(225, 186, 156, 255) },
            {"Yellow", new Color32(253, 220, 52, 255) },
            {"Pink", new Color32(255, 189, 202, 255) },
            {"Green", new Color32(148, 204, 119, 255) },
            {"BabyBlue", new Color32(134, 188, 200, 255) },
            {"Grey", new Color32(230, 228, 236, 255) },
            {"Purple", new Color32(215, 177, 255, 255) }
        };
    }
}
