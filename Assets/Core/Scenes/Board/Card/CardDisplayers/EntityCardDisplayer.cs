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
        private GameObject bkg;
        [SerializeField]
        private GameObject icon1;
        [SerializeField]
        private GameObject icon2;
        [SerializeField]
        private TextMeshProUGUI Id;
        [SerializeField]
        private TextMeshProUGUI label;
        [SerializeField]
        private GameObject scrollView;
        [SerializeField]
        private GameObject classHierachyContent;
        [SerializeField]
        private GameObject colorBarLeftTop;
        [SerializeField]
        private GameObject colorBarLeftBottom;
        [SerializeField]
        private GameObject colorBarRightTop;
        [SerializeField]
        private GameObject colorBarRightBottom;
        [SerializeField]
        private GameObject comment;

        public void Show(EntityCard card)
        {
            Debug.Log("[EntityCardService] Init OK");
            Refresh(card);
        }

        public void Refresh(EntityCard card)
        {
            icon1.SetActive(false);
            icon2.SetActive(false);

            //Icons
            if (card.icons[0] != "")
            {
                //Debug.Log("Icon1 name is : " + card.icons[0]);
                icon1.SetActive(true);
                icon1.GetComponent<Image>().sprite = _entityDeckService.iconsSprites[_entityDeckService.iconsDictionary[card.icons[0]]];
            }
            if(card.icons[1] != "")
            {
                //Debug.Log("Icon2 name is : " + card.icons[1]);
                icon2.SetActive(true);
                icon2.GetComponent<Image>().sprite = _entityDeckService.iconsSprites[_entityDeckService.iconsDictionary[card.icons[1]]];
            }

            Id.text = card.id;
            label.text = card.label;

            //Hierarchy
            if (classHierachyContent.transform.childCount > 0)
            {
                //Empty list of children
                foreach (Transform child in classHierachyContent.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            if (card.parentsClassList.Count > 0 || card.ChildrenClassList.Count > 0)
            {
                var ss = scrollView.GetComponent<SubSupContentList>();
                ss.UpdateSubSupClass(card.parentsClassList, card.about, card.ChildrenClassList);
            }

            //Colors
            //Colors
            int nbColors = card.colors.Count;
            switch (nbColors)
            {
                case 0:
                    Debug.LogError("[DeckCtrl] Card has no color: " + card.label);
                    break;
                case 1:
                    colorBarLeftTop.GetComponent<Image>().color = card.colors[0];
                    colorBarLeftBottom.GetComponent<Image>().color = card.colors[0];
                    colorBarRightTop.GetComponent<Image>().color = card.colors[0];
                    colorBarRightBottom.GetComponent<Image>().color = card.colors[0];
                    break;
                case 2:
                    colorBarLeftTop.GetComponent<Image>().color = card.colors[0];
                    colorBarLeftBottom.GetComponent<Image>().color = card.colors[1];
                    colorBarRightTop.GetComponent<Image>().color = card.colors[0];
                    colorBarRightBottom.GetComponent<Image>().color = card.colors[1];
                    break;
                default:
                    break;
            }

            //Comment
            comment.GetComponent<TextMeshProUGUI>().text = card.comment;
        }

        public void GhostBackground()
        {
            Debug.Log("CHANGE COLOR");
            bkg.GetComponent<Image>().color = new Color32(204, 204, 204, 255);
        }

        public void ReinitBackground()
        {
            bkg.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }
}
