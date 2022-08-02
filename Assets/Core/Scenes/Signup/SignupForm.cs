using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StarterCore.Core.Scenes.Signup
{
    public class SignupForm : MonoBehaviour
    {
        public GameObject AlertEmailNotValid;
        public GameObject AlertEmailAlreadyExists;
        public GameObject AlertPasswordNotValid;
        public GameObject AlertUserAcccountCreated;

        [SerializeField] internal TMP_InputField _email;
        [SerializeField] internal TMP_InputField _password;
        [SerializeField] internal TMP_Text _country;
        [SerializeField] internal Toggle _toggleOptinButton;
        [SerializeField] private Button _submitButton;
        [SerializeField] private Button _backButton;

        public event Action OnSubmitSignupFormClickedEvent;
        public event Action OnBackClickedEvent;

        public void Show()
        {
            AlertEmailNotValid.SetActive(false);
            AlertEmailAlreadyExists.SetActive(false);
            AlertPasswordNotValid.SetActive(false);
            AlertUserAcccountCreated.SetActive(false);

            _submitButton.onClick.AddListener(() => OnSubmitSignupFormClickedEvent?.Invoke());
            _backButton.onClick.AddListener(() => OnBackClickedEvent?.Invoke());
        }
    }
}