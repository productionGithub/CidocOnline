using StarterCore.Core.Services.Network.Models;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.Form
{
    // One element of the list, in the view

    public class FormEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameTxt;
        [SerializeField] private TextMeshProUGUI _emailTxt;
        [SerializeField] private Button _deleteButton;

        public event Action OnDeleteClickEvent;

        public void Show(User user)
        {
            _nameTxt.text = $"{user.Username} {user.Password} ({user.HistoryID})";
            _emailTxt.text = user.Email;

            _deleteButton.onClick.AddListener(() => OnDeleteClickEvent?.Invoke());
        }
    }
}
