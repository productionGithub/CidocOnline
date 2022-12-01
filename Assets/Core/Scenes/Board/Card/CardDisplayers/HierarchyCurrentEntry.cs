using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// This class represents the current card selected in the Entity or Property hierarchy scrollable view
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
