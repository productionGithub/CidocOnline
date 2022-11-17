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

public class HierarchyEntityEntry : MonoBehaviour
{
    public TextMeshProUGUI EntryLabel;
    public Button EntryButton;

    public event Action<string> OnHierarchyEntityClickEvent;

    private string _label;

    public void Show(string arrow, string label)
    {
        _label = label;
        EntryLabel.text = arrow + " " + label;
        EntryButton.onClick.AddListener(HierarchyEntityClicked);
    }

    private void HierarchyEntityClicked()
    {
        //Debug.Log(string.Format("[-EntityEntry (Prefab)] {0} clicked.", _label));
        OnHierarchyEntityClickEvent?.Invoke(_label);
    }
}
