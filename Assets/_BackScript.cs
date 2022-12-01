using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using UnityEngine.UI;
using System;

public class _BackScript : MonoBehaviour
{
    [Inject]
    private NavigationService _navService;

    [SerializeField]
    Button back;

    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(GoMainMenu);
    }

    private void GoMainMenu()
    {
        _navService.Pop();
    }
}
