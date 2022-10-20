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

public class EntityDeckService : IInitializable
{
    /// <summary>
    /// Initialize List of Entity and property cards, based on Cidoc rdf file parsing (.xml).
    /// Provides public reference to decks' instances.
    /// </summary>

    [Inject] MockNetService _netservice;

    //public Dictionary<int, EntityCard> EntityCards;
    public List<EntityCard> EntityCards;

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
    XPathNavigator entityColorsIconsFileNavigator;//Source file for updating Entity instances with ad-hoc icons and colors;
    XPathNavigator propertyColorsFileNavigator;//Source file for updating Entity instances with ad-hoc icons and colors;

    //Icon sprites
    public Sprite[] iconsSprites;
    public Dictionary<string, int> iconsDictionary;//Get reference index in spriteSheet by name of icon (more readable)

    //Colors
    public Dictionary<string, Color32> colorsDictionary;

    //Raw xml string for Cidoc
    string _cidocXmlString;
    string _entityXmlString;


    public void Initialize()
    {
        Debug.Log("THIS IS DECKSCONTROLLER!");

        ////Instanciation of decks
        EntityCards = new List<EntityCard>();
        iconsDictionary = new Dictionary<string, int>();

        InitColors();
        InitIcons();
        BuildEntityDeck();
    }

    // <*************************** public methods ************************************>
    public List<EntityCard> GetInitialDeck(string initString)
    {
        List<EntityCard> partialDeck = new List<EntityCard>();
        partialDeck.Clear();

        switch (initString[0])//string should starts with '*' or '-' to initiliaze with all ids or a partial list
        {
            case '*'://All cards
                partialDeck = EntityCards;
                break;

            case '-'://Partial list of cards
                string wString = initString.Substring(1);
                string[] arrayOfIds = wString.Split(',').Select(id => id.Trim()).ToArray();
                List<string> listOfIds = arrayOfIds.ToList();
                partialDeck = EntityCards.Where(c => listOfIds.Contains(c.id)).ToList();
                break;

            default:
                break;
        }

        return partialDeck;
    }


    // <********************            Init of deck from CirdocCRM file         *********************>
    private async void BuildEntityDeck()
    {
        //Fetch Cidoc Xml file
        _cidocXmlString = await _netservice.GetXmlCidocFile();
        //Debug.Log("Got XML CIDOC string : " + _cidocXmlString);

        //Fetch EntityIconsColorMapping Xml file
        _entityXmlString = await _netservice.GetXmlEntityIconsColorsFile();
        //Debug.Log("Got XML ENTITY COLORS ICONS string : " + _entityXmlString);

        InitXpathNavigators();
        InitEntityDeck();
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

        //EntityIconsColorsMapping
        MemoryStream msE = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(_entityXmlString));
        XPathDocument entityXpathDocument = new XPathDocument(msE);
        entityColorsIconsFileNavigator = entityXpathDocument.CreateNavigator();
    }

    private void InitEntityDeck()
    {
        int index;

        //Get all 'Class' nodes
        string entityExpression = "/rdf:RDF/rdfs:Class";//Entity xpath query
        XPathExpression entityQuery = cidocRdfFileNavigator.Compile(entityExpression);
        entityQuery.SetContext(CidocCrmNamespaceManager);
        XPathNodeIterator entities = cidocRdfFileNavigator.Select(entityQuery);

        //Initialize potentialSuperClassOf dictionary with entity names and empty list of potential subclasses for THAT entity
        //Subclasses will be populated on the next loop at entity creation time
        allSuperClassDic = new Dictionary<string, List<string>>();
        foreach (XPathNavigator entity in entities)
        {
            allSuperClassDic.Add(entity.GetAttribute("about", rdfNamespace), new List<string>());
        }

        //Populate entities
        index = 0;
        foreach (XPathNavigator entity in entities)
        {
            //Each entity might be a subclass of one or more parent entity (Ex: E4 has 2 direct parents)
            //We create a list of these Parents classes
            List<string> entityParents = new List<string>();

            XPathNodeIterator subClasses = entity.Select("./rdfs:subClassOf", CidocCrmNamespaceManager);
            foreach (XPathNavigator sub in subClasses)
            {
                entityParents.Add(sub?.GetAttribute("resource", rdfNamespace));//Build list of direct parents
                //Debug.Log("Got resource: " + sub?.GetAttribute("resource", rdfNamespace));
            }

            EntityCards.Add(new EntityCard()
            {
                //index = index,
                id = entity.GetAttribute("about", rdfNamespace).Split('_')[0],
                about = entity.GetAttribute("about", rdfNamespace),
                label = entity.SelectSingleNode("./rdfs:label[@xml:lang='en']", CidocCrmNamespaceManager).ToString(),
                comment = entity.SelectSingleNode("./rdfs:comment", CidocCrmNamespaceManager).ToString(),
                parentsClassList = entityParents,//List of direct parents
            }); ;

            //Add this entity as a sub Class in the Parent dictionary
            foreach (string superClassName in EntityCards[index].parentsClassList)
            {
                allSuperClassDic[superClassName].Add(EntityCards[index].about);
            }
            index++;
        }

        //All entities 'subClassOf' has been parsed and stored in the allSuperClassDic
        //Updating childrenClassList entity field with allSuperClassDic entries previously set
        index = 0;
        foreach (KeyValuePair<string, List<string>> kvp in allSuperClassDic)
        {
            foreach (string name in kvp.Value)
            {
                EntityCards[index].ChildrenClassList.Add(name);
            }
            index++;
        }

        //Populate icons names and colors
        //Get all 'row' nodes
        string rowExpression = "//row";
        XPathNodeIterator rows = (XPathNodeIterator)entityColorsIconsFileNavigator.Evaluate(rowExpression);

        index = 0;
        foreach (XPathNavigator row in rows)
        {
            XPathItem icon1 = row.SelectSingleNode("./Icon_1");
            XPathItem icon2 = row.SelectSingleNode("./Icon_2");

            EntityCards[index].icons.Add(icon1.ToString());
            EntityCards[index].icons.Add(icon2.ToString());

            XPathItem color1 = row.SelectSingleNode("./Colour_1");
            XPathItem color2 = row.SelectSingleNode("./Colour_2");
            XPathItem color3 = row.SelectSingleNode("./Colour_3");

            if (color1.ToString() != "")
            {
                EntityCards[index].colors.Add(colorsDictionary[color1.ToString()]);
            }
            if (color2.ToString() != "")
            {
                EntityCards[index].colors.Add(colorsDictionary[color2.ToString()]);
            }

            if (color3.ToString() != "")
            {
                EntityCards[index].colors.Add(colorsDictionary[color3.ToString()]);
            }

            index++;
        }
    }

    //Mapping Color32 / Name of colors
    private void InitColors()
    {
        colorsDictionary = new Dictionary<string, Color32>()
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

    private void InitIcons()
    {
        iconsSprites = Resources.LoadAll<Sprite>("Graphics/CIDOC - SpriteSheet");

        if (iconsSprites != null)
        {
            for (int i = 9; i <= 18; i++)//Icons are from index 9 TO 18
            {
                iconsDictionary.Add(iconsSprites[i].name, i);
                Debug.Log("Icon sprite names : " + iconsSprites[i].name);
            }
        }
        else
        {
            Debug.LogError("[DecksController] Could not load sprites from Resources folder.");
        }
    }
}
