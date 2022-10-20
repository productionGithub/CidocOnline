using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using StarterCore.Core.Services.Network.Models;

public class TBD_TESTLINQ : MonoBehaviour
{
    /*
    // Start is called before the first frame update
    void Start()
    {
        List<string> domainList = new List<string>()
        {
            "ARC", "HIS", "HUM"
        };

        List<string> languageList = new List<string>()
        {
            "Français", "English", "Ελληνική"
        };

        Scenario s1 = new Scenario(
            "MarmoutierFR",
            new List<string>() { "CidocCRM", "ric-O" },
            new List<string>() { "History", "Archeology" },
            new List<string>() { "HIS", "ARC" },
            new List<string>() { "Masa", "Huma-num" },
            "Français",
            "Ceci est une description en Français du scénario Abbaye de Marmoutier situé à Tours.",
            new List<Chapter>() {
                new Chapter("Les bases de l'ontologie",  "Description du chapitre..."),
                new Chapter("Les entités",  "Description du chapitre...")
            });
        Scenario s2 = new Scenario(
            "AsinouEN",
            new List<string>() { "CidocCRM", "ric-O" },
            new List<string>() { "History", "Cult.Her" },
            new List<string>() { "ARC" },
            new List<string>() { "Masa", "Bnf" },
            "English",
            "This is an english description of Asinou.",
            new List<Chapter>() {
                        new Chapter("Ontology basics",  "Chapter Description..."),
                        new Chapter("Entities",  "Chapter Description...")
            });

        List<Scenario> scenarii = new List<Scenario>();
        scenarii.Add(s1);
        scenarii.Add(s2);

        foreach(Scenario s in scenarii)
        {
            Debug.Log("Scenario Title is : " + s.ScenarioTitle);
        }
        var filteredList = scenarii.Where(x => x.Chapters.Any(y => y.ChapterTitle.Equals("Ontology basics")));

        //var filtered = listOfAllVenuses.Where(x => !listOfBlockedVenues.Any(y => y.VenueId == x.Id));



        Debug.Log("_________________ FILTERED __________________");
        foreach (Scenario s in filteredList)
        {
            Debug.Log("Scenario Title is : " + s.ScenarioTitle);

        }
        //Debug.Log("");

    }
    */
}