using UnityEngine;
using TMPro;

using StarterCore.Core.Scenes.Board.Card.Cards;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

/// <summary>
/// Class that represent an entry in the Property scrollable hierarchy
/// </summary>

public class HierarchyPropertyEntry : MonoBehaviour
{
    public TextMeshProUGUI EntryLabel;
    public Button EntryButton;

    public event Action<string> OnHierarchyPropertyClickEvent;

    private string _label;

    public void Show(string arrow, string label)
    {
        _label = label;
        EntryLabel.text = arrow + " " + label;
        EntryButton.onClick.AddListener(HierarchyPropertyClicked);
    }

    private void HierarchyPropertyClicked()
    {
        //Debug.Log(string.Format("[-PropertyEntry (Prefab)] {0} clicked.", _label));
        OnHierarchyPropertyClickEvent?.Invoke(_label);
    }
}
