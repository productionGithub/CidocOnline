using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using StarterCore.Core.Scenes.Board.Deck;

/// <summary>
/// Manage the UI of Ticks.
/// If White is selected, all others are OFF
/// If at least one but the White is selected:
/// White is OFF, others might be ON
/// </summary>

public class TickCtrl : MonoBehaviour
{
    public enum TickType { Domain, Range }

    [SerializeField]
    private TickType type;

    public bool IsTicked { get => ticked; }
    public TickType typeOfTick { get => type; }
    public TickColor colorOfTick { get => tickColor; }

    public enum TickColor
    {
        White,
        Blue,
        Brown,
        Yellow,
        Pink,
        Green,
        BabyBlue,
        Grey,
        Purple
    }

    [SerializeField]
    private GameObject ticksContainer;//Reference to parent deck
    [SerializeField]
    private TickColor tickColor;//Color of the tick from the enum above.

    private bool ticked = false;
    private float translateUnits;//When un/ticked, the tick is translated of 'translateValue' Unity distance units
    private readonly float translateValue = 12.0f;

    //GameObject clickSounds;



    //Initialize translate offset depending on side of the card the ticks are
    //Domain ticks -> 'Off' is on the right
    //Range ticks -> 'Off' is on the left
    private void OnEnable()
    {
        if (type == TickType.Domain)
        {
            translateUnits = translateValue;
        }
        else
        {
            translateUnits = -translateValue;
        }
    }



    void Start()
    {
        //clickSounds = GameObject.Find("ClickSounds");
        //Add listener to button of Tick object
        if (GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.AddListener(Clicked);
        }
        else
        {
            throw new ArgumentNullException("SliderCtrl");
        }
    }



    //When clicked, the business logic is managed by TicksController component
    //TicksController acts as a proxy : It calls the ad-hoc 'updateColorFilter'
    //Depending on the type of deck attached to this tick (Entity or Property).
    private void Clicked()
    {
        //AudioSource[] sounds = clickSounds.GetComponents<AudioSource>();
        //foreach (AudioSource sound in sounds)
        //{
        //    string soundToPlay = "TickClick2";

        //    if (sound.clip.name == soundToPlay)
        //    {
        //        sound.Play();
        //        break;
        //    }
        //}

        ticksContainer.GetComponent<TicksController>().TickClicked(gameObject, colorOfTick);
    }



    //UI of tick, set it 'Off' or 'On'
    public void TickOn()
    {
        if (IsTicked == false)
        {
            transform.parent.Translate(0.0f, -translateUnits, 0.0f);
            ticked = true;
        }
    }



    //UI of tick, set it 'Off' or 
    public void TickOff()
    {
        if (IsTicked == true)
        {
            transform.parent.Translate(0.0f, translateUnits, 0.0f);
            ticked = false;
        }
    }
}