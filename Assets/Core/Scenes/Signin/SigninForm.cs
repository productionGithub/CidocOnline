using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace StarterCore.Core.Scenes.Signin
{
    public class SigninForm : MonoBehaviour
    {
        [SerializeField] internal TMP_InputField _email;
        [SerializeField] internal TMP_InputField _password;

        [SerializeField] private Button _submitFormButton;
        [SerializeField] private Button _createAccountButton;

        public GameObject AlertNoAccount;
        public GameObject AlertEmailNotValid;
        public GameObject AlertPasswordNotValid;
        public GameObject AlertWrongCombination;
        public GameObject AlertRightCombination;

        public event Action OnSubmitSigninFormClickedEvent;

        public void Show()
        {
            AlertNoAccount.SetActive(false);
            AlertEmailNotValid.SetActive(false);
            AlertPasswordNotValid.SetActive(false);
            AlertWrongCombination.SetActive(false);
            AlertRightCombination.SetActive(false);
            _submitFormButton.onClick.AddListener(() => OnSubmitSigninFormClickedEvent?.Invoke());
        }

    }
}
