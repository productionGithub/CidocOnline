using UnityEngine;
using System;
using System.Text.RegularExpressions;
using Zenject;
using TMPro;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.Signin
{
    public class SigninController : MonoBehaviour
    {
        public event Action<SigninEventData> OnSigninFormSubmittedEvent;

        public event Action OnSubmitSigninFormClickedEvent;
        public event Action OnForgotPasswordClickedEvent;
        public event Action OnCreateAccountClickedEvent;
        public event Action<string> OnLocalizationEvent;

        //test
        public event Action OnTestEvent;//Todo : delete after use

        //Localization
        //public event Action<string> OnLocalizationFlagClickedEvent;

        public event Action OnEnglishLocalizationFlagClickedEvent;
        public event Action OnFrenchLocalizationFlagClickedEvent;

        //Form
        [SerializeField] internal TMP_InputField _email;
        [SerializeField] internal TMP_InputField _password;
        [SerializeField] private Button _submitFormButton;
        [SerializeField] private Button _createAccountButton;
        [SerializeField] private Button _forgotPassword;

        //Localization
        [SerializeField] private Button _englishFlagButton;
        [SerializeField] private Button _frenchFlagButton;

        //Alert messages
        public GameObject AlertNoAccount;
        public GameObject AlertEmailNotValid;
        public GameObject AlertPasswordNotValid;
        public GameObject AlertWrongCombination;
        public GameObject AlertRightCombination;
        public GameObject AlertActivation;
        public GameObject AlertWaitingForCredentials;

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
            _submitFormButton.onClick.AddListener(OnSubmitSigninFormClicked);// => OnSubmitSigninFormClickedEvent?.Invoke());
            _forgotPassword.onClick.AddListener(OnForgotPasswordClicked); // => OnForgotPasswordClickedEvent?.Invoke());// ;
            _createAccountButton.onClick.AddListener(OnCreateAccountClicked);// => OnCreateAccountClickedEvent?.Invoke());

            //Localization flags
            //_englishFlagButton.onClick.AddListener(() => OnLocalizationFlagClicked("en")); //OnLocalizationFlagClickedEvent?.Invoke("en"));
            //_frenchFlagButton.onClick.AddListener(() => OnLocalizationFlagClicked("fr"));//=> OnLocalizationFlagClickedEvent?.Invoke("fr"));

            _englishFlagButton.onClick.AddListener(OnEnglishLocalizationFlagClicked); //OnLocalizationFlagClickedEvent?.Invoke("en"));
            _frenchFlagButton.onClick.AddListener(OnFrenchLocalizationFlagClicked); //OnLocalizationFlagClickedEvent?.Invoke("en"));
        }

        //Localization events
        //private void OnLocalizationFlagClicked(string locale)
        //{
        //    OnLocalizationEvent?.Invoke(locale);
        //}

        private void OnEnglishLocalizationFlagClicked()
        {
            OnEnglishLocalizationFlagClickedEvent?.Invoke();
        }

        private void OnFrenchLocalizationFlagClicked()
        {
            OnFrenchLocalizationFlagClickedEvent?.Invoke();
        }



        //Form events
        private void OnCreateAccountClicked()
        {
            OnCreateAccountClickedEvent?.Invoke();
        }

        private void OnSubmitSigninFormClicked()
        {
            if (ValidateForm())
            {
                string email = _email.text;
                string password = _password.text;

                var evt = new SigninEventData(email, password);

                OnSigninFormSubmittedEvent?.Invoke(evt);

                HideAllAlerts();
                AlertWaitingForCredentials.SetActive(true);
            }
        }

        private void OnForgotPasswordClicked()
        {
            OnForgotPasswordClickedEvent?.Invoke();
        }

        /// <summary>
        /// Form messages
        /// </summary>
        /// <returns></returns>
        public bool ValidateForm()//TODO Finish form validation with other checks
        {
            return ValidateEmail() && ValidatePassword();
        }

        private bool ValidateEmail()
        {
            Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,63})+)$");
            Match match = regex.Match(_email.text);
            if (match.Success)
                return true;
            else
            {
                HideAllAlerts();
                AlertEmailNotValid.SetActive(true);
                return false;
            }
        }

        private bool ValidatePassword()
        {
            if (_password.text.Length >= 6)
            {
                return true;
            }
            else
            {
                HideAllAlerts();
                AlertPasswordNotValid.SetActive(true);
                return false;
            }
        }

        internal void NoAccount()
        {
            HideAllAlerts();
            AlertNoAccount.SetActive(true);
        }

        internal void WaitMessage()
        {
            HideAllAlerts();
           AlertWaitingForCredentials.SetActive(true);
        }

        internal void HideAllAlerts()
        {
            AlertNoAccount.SetActive(false);
            AlertEmailNotValid.SetActive(false);
            AlertPasswordNotValid.SetActive(false);
            AlertWrongCombination.SetActive(false);
            AlertRightCombination.SetActive(false);
            AlertActivation.SetActive(false);
            AlertWaitingForCredentials.SetActive(false);
        }

        private void OnDestroy()
        {
            OnSubmitSigninFormClickedEvent -= OnSubmitSigninFormClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            OnForgotPasswordClickedEvent -= OnForgotPasswordClicked;
            OnCreateAccountClickedEvent -= OnCreateAccountClicked;

            //Localization events
            //OnLocalizationFlagClickedEvent -= OnLocalizationFlagClicked;

            _englishFlagButton.onClick.RemoveListener(OnEnglishLocalizationFlagClicked); //OnLocalizationFlagClickedEvent?.Invoke("en"));
            _frenchFlagButton.onClick.RemoveListener(OnFrenchLocalizationFlagClicked); //OnLocalizationFlagClickedEvent?.Invoke("en"));

        }
    }
}



/*
//Exemple d'injection sur objets dynamiques
//[Inject] private DiContainer _container;

//[....] in method Show() {
// Disable template that is instanciated
_template.gameObject.SetActive(false);

//Instanciate template, update DI container
formInstance = Instantiate(_template, _parent);
_container.InjectGameObject(formInstance.gameObject);//Inject dynamically : entity.gameObject injects on component AND children

formInstance.gameObject.SetActive(true);
}
*/
