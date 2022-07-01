using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StarterCore.Core.Scenes.Signup
{
    public class SignupForm : MonoBehaviour
    {
        public GameObject AlertEmail;

        [SerializeField] internal TMP_InputField _email;
        [SerializeField] internal TMP_InputField _password;
        [SerializeField] internal TMP_Text _country;
        [SerializeField] internal Toggle _toggleOptinButton;
        [SerializeField] private Button _submitButton;


        public event Action OnSubmitSignupFormClickedEvent;

        public void Show()
        {
            AlertEmail.SetActive(false);
            _submitButton.onClick.AddListener(() => OnSubmitSignupFormClickedEvent?.Invoke());
        }
    }
}