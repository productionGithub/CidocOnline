using UnityEngine;
using TMPro;

using StarterCore.Core.Scenes.Board.Card.Cards;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

/// <summary>
/// Class that represent an entry in the Entity scrollable hierarchy
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
        OnHierarchyEntityClickEvent?.Invoke(_label);
    }
}
