using StarterCore.Core.Scenes.Board.Card.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace StarterCore.Core.Scenes.Board.Displayer
{
    public class EntityCardDisplayer : MonoBehaviour
    {
        /// <summary>
        /// Display content of card
        /// Contains a list of CARD_INTERACTABLE (Hierarchy, Ticks, Slider)
        /// </summary>

        [Inject] EntityDeckService _entityDeckService;

        //Card fields
        [SerializeField]
        private GameObject _bkg;
        [SerializeField]
        private GameObject _icon1;
        [SerializeField]
        private GameObject _icon2;
        [SerializeField]
        private TextMeshProUGUI _id;
        [SerializeField]
        private TextMeshProUGUI _label;

        [SerializeField]
        private GameObject _colorBarLeftTop;
        [SerializeField]
        private GameObject _colorBarLeftBottom;
        [SerializeField]
        private GameObject _colorBarRightTop;
        [SerializeField]
        private GameObject _colorBarRightBottom;
        [SerializeField]

        private GameObject _comment;

        public void Show(EntityCard card)
        {
            Debug.Log("[EntityCardService] Init OK");
            Refresh(card);
        }

        public void Refresh(EntityCard card)
        {
            _icon1.SetActive(false);
            _icon2.SetActive(false);

            //Icons
            if (card.icons[0] != "")
            {
                _icon1.SetActive(true);
                _icon1.GetComponent<Image>().sprite = _entityDeckService.iconsSprites[_entityDeckService.iconsDictionary[card.icons[0]]];
            }
            if(card.icons[1] != "")
            {
                _icon2.SetActive(true);
                _icon2.GetComponent<Image>().sprite = _entityDeckService.iconsSprites[_entityDeckService.iconsDictionary[card.icons[1]]];
            }

            _id.text = card.id;
            _label.text = card.label;

            //Colors
            int nbColors = card.colors.Count;
            switch (nbColors)
            {
                case 0:
                    Debug.LogError("[DeckCtrl] Card has no color: " + card.label);
                    break;
                case 1:
                    _colorBarLeftTop.GetComponent<Image>().color = card.colors[0];
                    _colorBarLeftBottom.GetComponent<Image>().color = card.colors[0];
                    _colorBarRightTop.GetComponent<Image>().color = card.colors[0];
                    _colorBarRightBottom.GetComponent<Image>().color = card.colors[0];
                    break;
                case 2:
                    _colorBarLeftTop.GetComponent<Image>().color = card.colors[0];
                    _colorBarLeftBottom.GetComponent<Image>().color = card.colors[1];
                    _colorBarRightTop.GetComponent<Image>().color = card.colors[0];
                    _colorBarRightBottom.GetComponent<Image>().color = card.colors[1];
                    break;
                default:
                    break;
            }

            //Comment
            _comment.GetComponent<TextMeshProUGUI>().text = card.comment;
        }

        public void GhostBackground()
        {
            _bkg.GetComponent<Image>().color = new Color32(204, 204, 204, 255);
        }

        public void ReinitBackground()
        {
            _bkg.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }
}
