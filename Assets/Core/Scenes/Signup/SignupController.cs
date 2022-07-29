using UnityEngine;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using UnityEngine.SceneManagement;

namespace StarterCore.Core.Scenes.Signup
{
    public class SignupController : MonoBehaviour
    {

        [SerializeField] private SignupForm _template;
        [SerializeField] private Transform _parent;

        public SignupForm formInstance;

        public int OnBackButtonClicked { get; private set; }

        public event Action<SignupEventData> OnFormSubmittedEvent;
        public event Action OnBackEvent;

        public void Show()
        {
            Debug.Log("SignupController instanciated");
            _template.gameObject.SetActive(false); // Disable template

            formInstance = Instantiate(_template, _parent);
            formInstance.gameObject.SetActive(true);

            formInstance.OnSubmitSignupFormClickedEvent += OnSubmitSignupFormClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            formInstance.OnBackClickedEvent += OnBackClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            formInstance.Show();
        }

        private void OnBackClicked()
        {
            //SceneManager.LoadSceneAsync("SigninScene");
            //SceneManager.UnloadSceneAsync("SignupScene");
            OnBackEvent?.Invoke();
        }

        private void OnSubmitSignupFormClicked()
        {
            formInstance.AlertEmailNotValid.SetActive(false);
            formInstance.AlertEmailAlreadyExists.SetActive(false);
            formInstance.AlertPasswordNotValid.SetActive(false);
            if (ValidateForm())
            {
                string email = formInstance._email.text;
                string password = formInstance._password.text;
                string country = formInstance._country.text;
                bool optin = formInstance._toggleOptinButton.isOn;

                var evt = new SignupEventData(email, password, country, optin);

                OnFormSubmittedEvent?.Invoke(evt);
            }
        }

        public bool ValidateForm()//TODO Finish form validation with other checks
        {
            return ValidateEmail() && ValidatePassword();
        }

        private bool ValidateEmail()
        {
            Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,63})+)$");
            Match match = regex.Match(formInstance._email.text);
            if (match.Success)
                return true;
            else
            {
                formInstance.AlertEmailNotValid.SetActive(true);
                return false;
            }
        }

        private bool ValidatePassword()
        {
            if(formInstance._password.text.Length >= 6)
            {
                return true;
            }
            else
            {
                formInstance.AlertPasswordNotValid.SetActive(true);
                return false;
            }
        }

        internal void EmailAlreadyExists()
        {
            formInstance.AlertEmailAlreadyExists.SetActive(true);
        }

        internal void AccountCreated()
        {
            formInstance.AlertUserAcccountCreated.SetActive(true);
        }
    }
}

