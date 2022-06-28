using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StarterCore.Core.Scenes.Signup
{
    public class SignupForm : MonoBehaviour
    {
        [SerializeField] public GameObject _alertEmail;

        [SerializeField] internal TMP_InputField _email;
        [SerializeField] internal TMP_InputField _password;
        [SerializeField] internal TMP_Text _country;
        [SerializeField] internal Toggle _toggleOptinButton;

        [SerializeField] private Button _submitButton;


        //public event Action OnOptinClickedEvent;
        public event Action OnSubmitSignupFormClickedEvent;

        // Start is called before the first frame update
        //void Start()
        //{
        //    Show();
        //}

        // Update is called once per frame
        public void Show()
        {
            _alertEmail.SetActive(false);
            _submitButton.onClick.AddListener(() => OnSubmitSignupFormClickedEvent?.Invoke());
        }
    }
}