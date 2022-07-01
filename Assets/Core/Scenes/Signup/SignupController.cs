using UnityEngine;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using UnityEngine.Networking;

namespace StarterCore.Core.Scenes.Signup
{
    public class SignupController : MonoBehaviour
    {

        [SerializeField] private SignupForm _template;
        [SerializeField] private Transform _parent;

        public SignupForm formInstance;

        public event Action<SignupEventData> OnFormSubmittedEvent;

        public void Show()
        {
            Debug.Log("SignupController instanciated");
            _template.gameObject.SetActive(false); // Disable template

            formInstance = Instantiate(_template, _parent);
            formInstance.gameObject.SetActive(true);

            formInstance.OnSubmitSignupFormClickedEvent += OnSubmitSignupFormClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            formInstance.Show();
        }

        private void OnSubmitSignupFormClicked()
        {
            if (ValidateForm())
            {
                formInstance.AlertEmail.SetActive(false);
                string email = formInstance._email.text;
                string password = formInstance._password.text;
                string country = formInstance._country.text;
                bool optin = formInstance._toggleOptinButton.isOn;

                var evt = new SignupEventData(email, password, country, optin);

                OnFormSubmittedEvent?.Invoke(evt);
            }
            else
            {
                Debug.Log("[SignupController] Form not valid (lacking email)");
                formInstance.AlertEmail.SetActive(true);
            }
        }

        public bool ValidateForm()//TODO Finish form validation with other checks
        {
            return ValidateEmail();
        }

        private bool ValidateEmail()
        {
            Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,63})+)$");
            Match match = regex.Match(formInstance._email.text);
            if (match.Success)
                return true;
            else
                return false;
        }
    }
}

