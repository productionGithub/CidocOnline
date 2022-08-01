using UnityEngine;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace StarterCore.Core.Scenes.Signin
{
    public class SigninController : MonoBehaviour
    {

        [SerializeField] private SigninForm _template;
        [SerializeField] private Transform _parent;

        public SigninForm formInstance;

        public event Action<SigninEventData> OnSigninFormSubmittedEvent;
        public event Action OnForgotPasswordClickedEvent;

        public void Show()
        {
            Debug.Log("SigninController instanciated");
            _template.gameObject.SetActive(false); // Disable template

            formInstance = Instantiate(_template, _parent);
            formInstance.gameObject.SetActive(true);

            formInstance.OnSubmitSigninFormClickedEvent += OnSubmitSigninFormClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            formInstance.OnForgotPasswordClickedEvent += OnForgotPasswordClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            formInstance.OnCreateAccountClickedEvent += OnCreateAccountClicked; // Equiv to += () => OnSubmitSignupFormClicked();

            formInstance.Show();
        }

        private void OnCreateAccountClicked()
        {
            Debug.Log("Create account !");
            SceneManager.LoadSceneAsync("SignupScene");
            SceneManager.UnloadSceneAsync("SigninScene");
        }

        private void OnSubmitSigninFormClicked()
        {
            formInstance.AlertNoAccount.SetActive(false);
            formInstance.AlertEmailNotValid.SetActive(false);
            formInstance.AlertPasswordNotValid.SetActive(false);
            formInstance.AlertWrongCombination.SetActive(false);
            formInstance.AlertRightCombination.SetActive(false);
            //formInstance.AlertActivation.SetActive(false);

            if (ValidateForm())
            {
                string email = formInstance._email.text;
                string password = formInstance._password.text;

                var evt = new SigninEventData(email, password);

                OnSigninFormSubmittedEvent?.Invoke(evt);
            }
        }

        private void OnForgotPasswordClicked()
        {
            OnForgotPasswordClickedEvent?.Invoke();
            Debug.Log("[Signin controller] OnForgotPasswordClickedEvent invoked !");
        }


        /// <summary>
        /// Form validation
        /// </summary>
        /// <returns></returns>
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
            if (formInstance._password.text.Length >= 6)
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

