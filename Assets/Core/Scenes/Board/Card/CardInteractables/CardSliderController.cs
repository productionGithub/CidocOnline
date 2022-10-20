using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CardSliderController : MonoBehaviour
{
    public event Action<float> OnSliderValueChangedUI;

    [SerializeField]
    Slider _slider;

    public void Show(int maxValue)
    {
        Debug.Log("[CardSliderController] Init OK");
        SetSliderRange(maxValue);
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        //Debug.Log("[CardSliderController] Value : " + (int) value);
        OnSliderValueChangedUI?.Invoke(value);
    }

    //Deck size in Model.Controller is updated after a deck is filtered by the colored Ticks.
    //We refresh the slider's range according to the new size of the filtered deck
    public void SetSliderRange(int value)
    {
        _slider.maxValue = value;
        _slider.maxValue = value;
        _slider.value = 0;
    }
}
