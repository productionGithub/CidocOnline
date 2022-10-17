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

    GameObject o;

    public void Show(GameObject obj)
    {
        o = obj;

        _slider.onValueChanged.AddListener(OnSliderValuerChanged);
    }

    private void OnSliderValuerChanged(float value)
    {
        OnSliderValueChangedUI?.Invoke(value);
    }
}
