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

public class DecksController : IInitializable
{
    /// <summary>
    /// Initialize List of Entity and property cards, based on Cidoc rdf file parsing (.xml).
    /// Provides public reference to decks' instances.
    /// </summary>

    [Inject] MockNetService _netservice;

    //Path to application '.../StreamingAssets' folder
    private string applicationStreamingAssetsPath;

    //Path to Cidoc RDF file (expected to be stored in the 'StreamingAssetsPath' folder)
    readonly private string CidocRdfFilePath = "DecksFiles/Cidoc";

    //Combination of StreamingAssets Folder + Path to Cidoc file
    private string fullCidocRdfFilePath;

    //Entity cards
    //Path to Icons and colors mapping file
    readonly private string EntityIconsColorsFilePath = "DecksFiles/EntityIconsColors";
    //Combination of StreamingAssets Folder + Path to Color and icon mapping file
    private string fullEntityIconsColorsFilePath;

    //Property cards
    //Path to Icons and colors mapping file
    readonly private string propertyColorsFilePath = "DecksFiles/PropertyColors";

    //Combination of StreamingAssets Folder + Path to Color mapping file
    private string fullPropertyColorsFilePath;

    //Path to instances.json file
    readonly private string jsonFileLocation = "DecksFiles/Instances/";
    private string jsonFilePath;
    private readonly string jsonFileName = "Instances.json";

    //Referential dictionaries of all possible cards created from the Cidoc RDF file + Other Xml files
    public Dictionary<int, EntityCard> entityCards;
    public Dictionary<int, PropertyCard> propertyCards;
    //Instance cards
    public List<InstanceCard> instanceCards;

    //Temporary dictionary for superClass deduction
    private Dictionary<string, List<string>> allSuperClassDic;

    //Temporary dictionary for superProperty deduction
    private Dictionary<string, List<string>> allSuperPropertyDic;

    //Icons enum
    public enum Icons { Axis, Clock, Cube, Forms, Id, Idea, Location, Moebius, Ruler, User }

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
    string _propertyXmlString;


    public async void Initialize()
    {
        Debug.Log("THIS IS DECKSCONTROLLER!");
        InitColors();

        ////Instanciation of decks
        entityCards = new Dictionary<int, EntityCard>();
        propertyCards = new Dictionary<int, PropertyCard>();
        iconsDictionary = new Dictionary<string, int>();
        instanceCards = new List<InstanceCard>();

        InitIcons();
        FetchXmlString();
        Debug.Log("[DecksController] FIN INITIALIZE");
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

        //PropertyColorsMapping
        MemoryStream msP = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(_propertyXmlString));
        XPathDocument propertyXpathDocument = new XPathDocument(msP);
        propertyColorsFileNavigator = propertyXpathDocument.CreateNavigator();
    }


    private async void FetchXmlString()
    {
        //Fetch Cidoc Xml file
        _cidocXmlString = await _netservice.GetXmlCidocFile();
        //Debug.Log("Got XML CIDOC string : " + _cidocXmlString);

        //Fetch EntityIconsColorMapping Xml file
        _entityXmlString = await _netservice.GetXmlEntityIconsColorsFile();
        //Debug.Log("Got XML ENTITY COLORS ICONS string : " + _entityXmlString);

        //Fetch PropertyColorMapping Xml file
        _propertyXmlString = await _netservice.GetXmlPropertyColorsFile();
        //Debug.Log("Got XML PROPERTY COLORS MAPPING string : " + _propertyXmlString);

        Debug.Log("[DecksCtrl] => " + _propertyXmlString.ToString());

        InitXpathNavigators();
        InitEntityDeck();

        Debug.Log("[DecksController] After INIT ENTITY DECK EDeck size is :" + entityCards.Count);
        //InitPropertyDeck();
        //InitInstancesDeck();
        //TestEntityDeck();
        //TestPropertyDeck();
    }

    /// <summary>
    /// Parse Cidoc rdf file
    /// Populate List of EntityCard instances
    /// </summary>
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
            Debug.Log("[DecksController] In Loop.");

            //Each entity might be a subclass of one or more parent entity (Ex: E4 has 2 direct parents)
            //We create a list of these Parents classes
            List<string> entityParents = new List<string>();

            XPathNodeIterator subClasses = entity.Select("./rdfs:subClassOf", CidocCrmNamespaceManager);
            foreach (XPathNavigator sub in subClasses)
            {
                entityParents.Add(sub?.GetAttribute("resource", rdfNamespace));//Build list of direct parents
                //Debug.Log("Got resource: " + sub?.GetAttribute("resource", rdfNamespace));
            }

            entityCards.Add(index, new EntityCard
            {
                index = index,
                id = entity.GetAttribute("about", rdfNamespace).Split('_')[0],
                about = entity.GetAttribute("about", rdfNamespace),
                label = entity.SelectSingleNode("./rdfs:label[@xml:lang='en']", CidocCrmNamespaceManager).ToString(),
                comment = entity.SelectSingleNode("./rdfs:comment", CidocCrmNamespaceManager).ToString(),
                parentsClassList = entityParents,//List of direct parents
            }); ;

            //Add this entity as a sub Class in the Parent dictionary
            foreach (string superClassName in entityCards[index].parentsClassList)
            {
                allSuperClassDic[superClassName].Add(entityCards[index].about);
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
                entityCards[index].ChildrenClassList.Add(name);
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

            entityCards[index].icons.Add(icon1.ToString());
            entityCards[index].icons.Add(icon2.ToString());

            XPathItem color1 = row.SelectSingleNode("./Colour_1");
            XPathItem color2 = row.SelectSingleNode("./Colour_2");
            XPathItem color3 = row.SelectSingleNode("./Colour_3");

            if (color1.ToString() != "")
            {
                entityCards[index].colors.Add(colorsDictionary[color1.ToString()]);
            }
            if (color2.ToString() != "")
            {
                entityCards[index].colors.Add(colorsDictionary[color2.ToString()]);
            }

            if (color3.ToString() != "")
            {
                entityCards[index].colors.Add(colorsDictionary[color3.ToString()]);
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
            }
        }
        else
        {
            Debug.LogError("[DecksController] Could not load sprites from Resources folder.");
        }
    }


    /*


    /// <summary>
    /// Idem for property cards
    /// </summary>
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

            propertyCards.Add(index, new PropertyCard
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
            foreach (string supPropName in propertyCards[index].superProperties)
            {
                if (propertyCards[index].superProperties.Count > 0)
                {
                    if (allSuperPropertyDic.Keys.Contains(supPropName))
                    {
                        allSuperPropertyDic[supPropName].Add(propertyCards[index].about);
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

                    propertyCards[index].domainColors = new List<string>(dColors);
                    propertyCards[index].rangeColors = new List<string>(rColors);

                    Debug.Log("PROPERTY TEST ID ***" + propertyCards[index].id);
                    Debug.Log("PROPERTY TEST Color 0 ***" + propertyCards[index].domainColors[0]);

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
                propertyCards[index].subProperties.Add(name);
            }
            index++;
        }
    }



    private void InitInstancesDeck()
    {
        jsonFilePath = Path.Combine(applicationStreamingAssetsPath, jsonFileLocation);
        string fullPathName = Path.Combine(jsonFilePath, jsonFileName);

        var sr = new StreamReader(fullPathName);
        string fileContent = sr.ReadToEnd();
        sr.Close();

        instanceCards = JsonConvert.DeserializeObject<List<InstanceCard>>(fileContent);
    }






    //Some accessors
    public EntityCard GetEntityCardById(string id)
    {
        return entityCards.FirstOrDefault(x => x.Value.id == id).Value;
    }



    public PropertyCard GetPropertyCardById(string id)
    {
        return propertyCards.FirstOrDefault(x => x.Value.id == id).Value;
    }



    public PropertyCard GetPropertyCardByIndex(int index)
    {
        return propertyCards[index];
    }


    */

    //DEBUG

    /*

    public void TestEntityDeck()
    {
        int index = 0;
        foreach (KeyValuePair<int, EntityCard> entity in entityCards)
        {
            Debug.Log("**********************************************");
            Debug.Log("Index: " + index + " Icon1: <" + entity.Value.icons[0] + ">" + "Icon2: <" + entity.Value.icons[1] + ">");
            Debug.Log("Id: <" + entity.Value.id + ">" + " About: < " + entity.Value.about + " > ");
            Debug.Log("Label: < " + entity.Value.label + " > ");
            Debug.Log("Comment: " + " < " + entity.Value.comment + " > ");

            if (entity.Value.colors.Count == 1) Debug.Log("Color 1: " + entity.Value.colors[0]);
            if (entity.Value.colors.Count == 2) Debug.Log("Color 2: " + entity.Value.colors[1]);
            if (entity.Value.colors.Count == 3) Debug.Log("Color 3: " + entity.Value.colors[2]);

            index++;
        }
    }



    public void TestPropertyDeck()
    {
        //Used to check number of properties and label spelling with colorPropertyMapping
        foreach (KeyValuePair<int, PropertyCard> property in propertyCards)
        {

            Debug.Log("**********************************************");
            Debug.Log("Index (Prop KEY): ===" + property.Key + " Id: <" + property.Value.id + ">" + " About: < " + property.Value.about + " > " + " Label: < " + property.Value.label + " > " + " Comment: " + " < " + property.Value.comment + " > " + " Domain: " + " < " + property.Value.domain + " > " + " Range: " + " < " + property.Value.range + " > " + " InverseOf: " + " < " + property.Value.inverseOf + " > ");

            //Domain colors
            Debug.Log("DOMAIN COLORS");
            switch (property.Value.domainColors.Count)
            {
                case 0:
                    Debug.Log("[DecksCtrl] Lacking domain color info for: " + property.Value.about);
                    break;
                case 1:
                    Debug.Log("Color 1: ===" + property.Value.domainColors[0]);
                    break;
                case 2:
                    Debug.Log("Color 1: ===" + property.Value.domainColors[0]);
                    Debug.Log("Color 2: ===" + property.Value.domainColors[1]);
                    break;
                case 3:
                    Debug.Log("Color 1: ===" + property.Value.domainColors[0]);
                    Debug.Log("Color 2: ===" + property.Value.domainColors[1]);
                    Debug.Log("Color 3: ===" + property.Value.domainColors[2]);
                    break;
                default:
                    break;
            }

            //Range colors
            Debug.Log("RANGE COLORS");
            switch (property.Value.rangeColors.Count)
            {
                case 0:
                    Debug.Log("[DecksCtrl] Lacking rangecolor info for: " + property.Value.about);
                    break;
                case 1:
                    Debug.Log("Color 1: ===" + property.Value.rangeColors[0]);
                    break;
                case 2:
                    Debug.Log("Color 1: ===" + property.Value.rangeColors[0]);
                    Debug.Log("Color 2: ===" + property.Value.rangeColors[1]);
                    break;
                case 3:
                    Debug.Log("Color 1: ===" + property.Value.rangeColors[0]);
                    Debug.Log("Color 2: ===" + property.Value.rangeColors[1]);
                    Debug.Log("Color 3: ===" + property.Value.rangeColors[2]);
                    break;
                default:
                    break;
            }
        }
    }

    */
}
