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
        /// Display content of card
        /// Contains a list of CARD_INTERACTABLE (Hierarchy, Ticks, Slider)
        /// </summary>

        [Inject] InstanceDeckService _entityDeckService;

        [SerializeField]
        private TextMeshProUGUI _id;
        [SerializeField]
        private TextMeshProUGUI _title;
        [SerializeField]
        private TextMeshProUGUI _label;
        [SerializeField]
        private GameObject _image;

        public void Show(InstanceCardModelDown instance, Sprite iconSprite)
        {
            _id.text = instance.Id;
            _title.text = instance.Title;
            _label.text = instance.Label;
            _image.GetComponent<Image>().sprite = iconSprite;
        }

        //IEnumerator GetIconImage()
        //{
        //    UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://ontomatchgame.huma-num.fr/StreamingAssets/scenarii/MarmoutierFR/Instances/Images/02_archaeologist.jpg");
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        Debug.Log(www.error);
        //    }
        //    else
        //    {
        //        Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        //        Sprite sprite = Sprite.Create(myTexture, new Rect(0, 0, 300, 300), new Vector2());
        //        _image.GetComponent<Image>().sprite = sprite;
        //    }
        //}















        private IEnumerator TestCoroutine()
        {
            yield return null;
        }

        private async UniTask TestAsync()
        {
            await TestCoroutine().ToUniTask();
        }
    }
}
