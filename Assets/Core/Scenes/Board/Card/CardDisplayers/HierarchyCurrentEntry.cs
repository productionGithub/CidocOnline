using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// List of super and sub classes in cards are buttons (supSubButton)
/// This class manages display of super and sub classes for an entity card
/// when a subSupButton is clicked.
/// </summary>

public class HierarchyCurrentEntry : MonoBehaviour
{
    public TextMeshProUGUI EntryLabel;
    public Button EntryButton;

    public void Show(string about)
    {
        EntryLabel.text = about;
    }
}
