using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace StarterCore.Core.Scenes.Signin
{
    public class SigninForm : MonoBehaviour
    {
        /*
        [SerializeField] internal TMP_InputField _email;
        [SerializeField] internal TMP_InputField _password;

        [SerializeField] private Button _submitFormButton;
        [SerializeField] private Button _createAccountButton;
        [SerializeField] private Button _forgotPassword;

        [SerializeField] private Button _test;//Todo : delete after use

        //Localization
        [SerializeField] private Button _englishFlagButton;
        [SerializeField] private Button _frenchFlagButton; 

        public GameObject AlertNoAccount;
        public GameObject AlertEmailNotValid;
        public GameObject AlertPasswordNotValid;
        public GameObject AlertWrongCombination;
        public GameObject AlertRightCombination;
        public GameObject AlertActivation;
        public GameObject AlertWaitingForCredentials;

        public event Action OnSubmitSigninFormClickedEvent;
        public event Action OnForgotPasswordClickedEvent;
        public event Action OnCreateAccountClickedEvent;

        public event Action OnTestClickedEvent;//Todo : delete after use

        //Localization
        public event Action<string> OnLocalizationFlagClickedEvent;

        public void Show()
        {
            AlertNoAccount.SetActive(false);
            AlertEmailNotValid.SetActive(false);
            AlertPasswordNotValid.SetActive(false);
            AlertWrongCombination.SetActive(false);
            AlertRightCombination.SetActive(false);
            AlertActivation.SetActive(false);
            AlertWaitingForCredentials.SetActive(false);

            //Form
            _submitFormButton.onClick.AddListener(() => OnSubmitSigninFormClickedEvent?.Invoke());
            _forgotPassword.onClick.AddListener(() => OnForgotPasswordClickedEvent?.Invoke());
            _createAccountButton.onClick.AddListener(() => OnCreateAccountClickedEvent?.Invoke());

            //Localization flags
            _englishFlagButton.onClick.AddListener(() => OnLocalizationFlagClickedEvent?.Invoke("en"));
            _frenchFlagButton.onClick.AddListener(() => OnLocalizationFlagClickedEvent?.Invoke("fr"));

            //Test
            _test.onClick.AddListener(() => OnTestClickedEvent?.Invoke());

        }
        */
    }
}
