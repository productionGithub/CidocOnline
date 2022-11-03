#define TRACE_ON
using System;
using System.Linq;
using StarterCore.Core.Scenes.Board.Card.Cards;
using StarterCore.Core.Scenes.Board.Deck;
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
        /// Show conditional comment when full text button is clicked
        /// </summary>

        [Inject] EntityDeckService _entityDeckService;

        public event Action OnFullTextClick;

        public EntityCard CurrentCard;
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

        //Scope note
        [SerializeField]
        private TextMeshProUGUI _comment;
        [SerializeField]
        private Button _fullTextButton;
        [SerializeField]
        private GameObject _fullTextScrollView;
        [SerializeField]
        private TextMeshProUGUI _fullTextContent;

        private bool _fullTextButtonState = false;//False not clicked (hence scroll view disabled)

        //init events
        public void Init()
        {
            Trace.Log("[EntityCardDisplayer] Init!");
            _fullTextButton.onClick.AddListener(FullTextClicked);
        }

        
        //Show UI
        public void Show(EntityCard card)
        {
            CurrentCard = card;

            //reset full text button and scope note view
            _fullTextScrollView.SetActive(false);
            _fullTextButtonState = false;
            _fullTextButton.GetComponentInChildren<TextMeshProUGUI>().text = "full text";

            _icon1.SetActive(false);
            _icon2.SetActive(false);

            //Icons
            if (card.icons[0] != "")
            {
                _icon1.SetActive(true);
                _icon1.GetComponent<Image>().sprite = _entityDeckService.IconsSprites[_entityDeckService.IconsDictionary[card.icons[0]]];
            }
            if (card.icons[1] != "")
            {
                _icon2.SetActive(true);
                _icon2.GetComponent<Image>().sprite = _entityDeckService.IconsSprites[_entityDeckService.IconsDictionary[card.icons[1]]];
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
            _comment.text = card.comment;
        }

        private void FullTextClicked()
        {
            if (!_fullTextButtonState)
            {
                //False -> We want the scrollview to be visible
                _fullTextScrollView.SetActive(true);
                _fullTextContent.text = _comment.text;
                _fullTextButton.GetComponentInChildren<TextMeshProUGUI>().text = "close";
            }
            else
            {
                //Invisible
                _fullTextScrollView.SetActive(false);
                _fullTextButton.GetComponentInChildren<TextMeshProUGUI>().text = "full text";
            }
            _fullTextButtonState = !_fullTextButtonState;
        }

        public void GhostBackground()
        {
            _bkg.GetComponent<Image>().color = new Color32(204, 204, 204, 255);
        }

        public void ReinitBackground()
        {
            _bkg.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        public void ResetToFirstCard()
        {
            Show(_entityDeckService.EntityCards[0]);
        }

        private void OnDestroy()
        {
            _fullTextButton.onClick.RemoveListener(FullTextClicked);
        }
    }
}
