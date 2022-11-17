using UnityEngine;
using TMPro;

using StarterCore.Core.Scenes.Board.Card.Cards;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

/// <summary>
/// List of super and sub classes in cards are buttons (supSubButton)
/// This class manages display of super and sub classes for an entity card
/// when a subSupButton is clicked.
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
