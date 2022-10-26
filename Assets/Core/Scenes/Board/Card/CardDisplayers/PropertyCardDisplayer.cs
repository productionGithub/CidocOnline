using System;
using StarterCore.Core.Scenes.Board.Card.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace StarterCore.Core.Scenes.Board.Displayer
{
    public class PropertyCardDisplayer : MonoBehaviour
    {
        /// <summary>
        /// Display content of card
        /// Contains a list of CARD_INTERACTABLE (Hierarchy, Ticks, Slider)
        /// </summary>

        [Inject] PropertyDeckService _entityDeckService;

        //Card fields
        [SerializeField]
        private GameObject _bkg;
        [SerializeField]
        private TextMeshProUGUI _id;
        [SerializeField]
        private TextMeshProUGUI _label;

        //Left colors
        [SerializeField]
        private GameObject _colorBarLeftTop;
        [SerializeField]
        private GameObject _colorBarLeftCenterTop;
        [SerializeField]
        private GameObject _colorBarLeftCenterBottom;
        [SerializeField]
        private GameObject _colorBarLeftBottom;

        //Right colors
        [SerializeField]
        private GameObject _colorBarRightTop;
        [SerializeField]
        private GameObject _colorBarRightCenterTop;
        [SerializeField]
        private GameObject _colorBarRightCenterBottom;
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

        public void Show(PropertyCard card)
        {
            _fullTextButton.onClick.AddListener(FullTextClicked);
            Refresh(card);
            InitScopeNoteScrollView();
        }

        private void InitScopeNoteScrollView()
        {
            _fullTextScrollView.SetActive(false);
            //_fullTextContent.text = _comment.text;
            _fullTextButtonState = false;
            _fullTextButton.GetComponentInChildren<TextMeshProUGUI>().text = "full text";
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

        public void Refresh(PropertyCard card)
        {
            _id.text = card.id;
            _label.text = card.label;

            /*********************************************************/
            //Domain colors
            int nbDomainColors = card.domainColors.Count;
            switch (nbDomainColors)
            {
                case 0:
                    Debug.LogError("[PropertyDeckCtrl] Card has no color: " + card.label);
                    break;

                case 1:
                    //Left colors
                    _colorBarLeftTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[0]];
                    _colorBarLeftCenterTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[0]];
                    _colorBarLeftCenterBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[0]];
                    _colorBarLeftBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[0]];
                    break;

                case 2:
                    //left colors
                    _colorBarLeftTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[0]];
                    _colorBarLeftCenterTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[0]];
                    _colorBarLeftCenterBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[1]];
                    _colorBarLeftBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[1]];
                    break;

                case 3:
                    //left colors
                    _colorBarLeftTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[0]];
                    _colorBarLeftCenterTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[1]];
                    _colorBarLeftCenterBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[1]];
                    _colorBarLeftBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[2]];
                    Debug.Log("");
                    break;

                default:
                    break;
            }

            //Range colors
            int nbRangeColors = card.rangeColors.Count;
            switch (nbRangeColors)
            {
                case 0:
                    Debug.LogError("[PropertyDeckCtrl] Card has no color: " + card.label);
                    break;

                case 1:
                    //rightcolors
                    _colorBarRightTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[0]];
                    _colorBarRightCenterTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.domainColors[0]];
                    _colorBarRightCenterBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.rangeColors[0]];
                    _colorBarRightBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.rangeColors[0]];
                    break;

                case 2:
                    //right colors
                    _colorBarRightTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.rangeColors[0]];
                    _colorBarRightCenterTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.rangeColors[0]];
                    _colorBarRightCenterBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.rangeColors[1]];
                    _colorBarRightBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.rangeColors[1]];
                    break;

                case 3:
                    //right colors
                    _colorBarRightTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.rangeColors[0]];
                    _colorBarRightCenterTop.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.rangeColors[1]];
                    _colorBarRightCenterBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.rangeColors[1]];
                    _colorBarRightBottom.GetComponent<Image>().color = _entityDeckService.ColorsDictionary[card.rangeColors[2]];
                    break;

                default:
                    break;
            }
            /*******************************************************************/










            /*
            //Colors
            int nbColors = card.colors.Count;
            switch (nbColors)
            {
                case 0:
                    Debug.LogError("[DeckCtrl] Card has no color: " + card.label);
                    break;
                case 1:
                    __colorBarLeftTop.GetComponent<Image>().color = card.colors[0];
                    _colorBarLeftBottom.GetComponent<Image>().color = card.colors[0];
                    __colorBarRightTop.GetComponent<Image>().color = card.colors[0];
                    _colorBarRightBottom.GetComponent<Image>().color = card.colors[0];
                    break;
                case 2:
                    __colorBarLeftTop.GetComponent<Image>().color = card.colors[0];
                    _colorBarLeftBottom.GetComponent<Image>().color = card.colors[1];
                    __colorBarRightTop.GetComponent<Image>().color = card.colors[0];
                    _colorBarRightBottom.GetComponent<Image>().color = card.colors[1];
                    break;
                default:
                    break;
            }

            */
            //Comment
            _comment.text = card.comment;
            InitScopeNoteScrollView();
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
