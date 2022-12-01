using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using StarterCore.Core.Scenes.Board.Card.Cards;
using StarterCore.Core.Services.Network.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

namespace StarterCore.Core.Scenes.Board.Displayer
{
    public class InstanceCardDisplayer : MonoBehaviour
    {
        /// <summary>
        /// Manage the display of instance cards
        /// </summary>

        [SerializeField] private TextMeshProUGUI _id;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private GameObject _image;

        public void Show(InstanceCardModelDown instance, Sprite iconSprite)
        {
            _id.text = instance.Id;
            _title.text = instance.Title;
            _label.text = instance.Label;
            _image.GetComponent<Image>().sprite = iconSprite;
        }
    }
}
