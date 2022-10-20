using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Instantiate super and sub classes of a card
/// List of super and sub classes are a list of Buttons
/// </summary>

public class SubSupContentList : MonoBehaviour
{
    [SerializeField]
    GameObject subSupButtonPrefab;
    [SerializeField]
    GameObject currentSubSupButtonPrefab;

    public GameObject contentList;


    //Populate the SuperSub Classes ScrollView of a card with list of cards IDs
    //Thgis method is called by EntityDeckController and PropertyDeckController
    //when a card is refresh.
    public void UpdateSubSupClass(List<string> parent, string current, List<string> children)
    {
        //Parent classes
        if (parent.Count > 0)
        {
            foreach (string name in parent)
            {
                GameObject parentButton = Instantiate(subSupButtonPrefab);
                parentButton.SetActive(true);
                parentButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                parentButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.MidlineLeft;
                parentButton.transform.SetParent(contentList.transform, false);
            }
        }

        //Current
        GameObject currentButton = Instantiate(currentSubSupButtonPrefab);
        currentButton.SetActive(true);
        currentButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = current;
        currentButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        currentButton.transform.SetParent(contentList.transform, false);

        //Children
        if (children.Count > 0)
        {
            foreach (string name in children)
            {
                GameObject childrenButton = Instantiate(subSupButtonPrefab);
                childrenButton.SetActive(true);
                childrenButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                childrenButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.MidlineLeft;
                childrenButton.transform.SetParent(contentList.transform, false);
            }
        }
    }
}
