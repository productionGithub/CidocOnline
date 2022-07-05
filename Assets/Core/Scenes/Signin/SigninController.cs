using UnityEngine;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using UnityEngine.Networking;

namespace StarterCore.Core.Scenes.Signin
{
    public class SigninController : MonoBehaviour
    {

        [SerializeField] private SigninForm _template;
        [SerializeField] private Transform _parent;

        public SigninForm formInstance;

        public event Action<SigninEventData> OnSigninFormSubmittedEvent;

        public void Show()
        {
            Debug.Log("SigninController instanciated");
            _template.gameObject.SetActive(false); // Disable template

            formInstance = Instantiate(_template, _parent);
            formInstance.gameObject.SetActive(true);

            formInstance.OnSubmitSigninFormClickedEvent += OnSubmitSigninFormClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            formInstance.Show();
        }

        private void OnSubmitSigninFormClicked()
        {
            formInstance.AlertNoAccount.SetActive(false);
            formInstance.AlertEmailNotValid.SetActive(false);
            formInstance.AlertPasswordNotValid.SetActive(false);
            formInstance.AlertWrongCombination.SetActive(false);
            formInstance.AlertRightCombination.SetActive(false);

            if (ValidateForm())
            {
                string email = formInstance._email.text;
                string password = formInstance._password.text;

                var evt = new SigninEventData(email, password);

                OnSigninFormSubmittedEvent?.Invoke(evt);
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
            if (formInstance._password.text.Length >= 8)
            {
                return true;
            }
            else
            {
                formInstance.AlertPasswordNotValid.SetActive(true);
                return false;
            }
        }

        internal void NoAccount()
        {
            formInstance.AlertNoAccount.SetActive(true);
        }
    }
}

