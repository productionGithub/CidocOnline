using UnityEngine;
using TMPro;

/// <summary>
/// List of super and sub classes in cards are buttons (supSubButton)
/// This class manages display of super and sub classes for an entity card
/// when a subSupButton is clicked.
/// </summary>

public class SubSupButton : MonoBehaviour
{
    /*
    private EntityDeckCtrl[] entityDeckCtrls;
    private EntityDeckCtrl entityDeckCtrl;

    private TextMeshProUGUI aboutLabel;

    DecksController decksCtrl;

    GameObject clickSounds;



    private void Start()
    {
        clickSounds = GameObject.Find("ClickSounds");

        decksCtrl = GameManager.Instance.GetSystemPrefab("DecksController").GetComponent<DecksController>();

        //Get reference to the Deck controller (EntityDeckController)
        entityDeckCtrls = gameObject.GetComponentsInParent<EntityDeckCtrl>();
        if (entityDeckCtrls.Length > 0) entityDeckCtrl = entityDeckCtrls[0];//Only 1 EntityDeckCtrl possible

        //Get reference to the label of the button clicked
        aboutLabel = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }



    public void DisplaySubSupCard()
    {
        //Play click sound
        AudioSource[] sounds = clickSounds.GetComponents<AudioSource>();
        foreach (AudioSource sound in sounds)
        {
            if (sound.clip.name == "CardNavClick")
            {
                sound.Play();
                break;
            }
        }

        //Get the ID of the card associated with this button
        string labelString = aboutLabel.GetComponent<TextMeshProUGUI>().text;
        string id = labelString.Split('_')[0];

        bool display = false;//Flag to check if card is found and displayed.

        //Is card Id in the current deck content, refresh the deck with ad-hoc index
        for (int index = 0; index < entityDeckCtrl.DeckContent.Count; index++)
        {
            if (decksCtrl.entityCards[entityDeckCtrl.DeckContent[index]].id == id)
            {
                entityDeckCtrl.refreshFromSlider = false;
                entityDeckCtrl.RefreshCard(index);
                display = true;
                break;
            }
        }

        //If not, if card belongs to initial deck, reset filtering and display card
        if (display == false)
        {
            EntityCard card = decksCtrl.GetEntityCardById(id);

            if (entityDeckCtrl.InitialDeckContent.Contains(card.index))
            {
                entityDeckCtrl.ReinitFilters();

                //Search index of card in initial deck
                //If found, refresh deck with card index
                for (int i = 0; i < entityDeckCtrl.InitialDeckContent.Count; i++)
                {
                    if (card.index == entityDeckCtrl.InitialDeckContent[i])
                    {
                        entityDeckCtrl.RefreshCard(i);
                        display = true;
                    }
                }

            }
        }

        //if not, add it.
        if (display == false)
        {
            EntityCard card = decksCtrl.GetEntityCardById(id);
            entityDeckCtrl.ReinitFilters();
            entityDeckCtrl.AddEntityCard(card.index);
        }
    }
    */
}
