using UnityEngine;
using System;
using System.Text.RegularExpressions;
using Zenject;
using System.Collections.Generic;
using TMPro;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Services.GameState;

using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.Signup
{
    public class SignupController : MonoBehaviour
    {
        public GameObject AlertUsernameNotValid;
        public GameObject AlertUsernameAlreadyExists;

        public GameObject AlertEmailNotValid;
        public GameObject AlertEmailAlreadyExists;
        public GameObject AlertPasswordNotValid;
        public GameObject AlertUserAccountCreated;
        public GameObject AlertUserAcccountCreatedFailed;
        public GameObject AlertWaitingForCredentials;


        [SerializeField] internal TMP_InputField _username;
        [SerializeField] internal TMP_InputField _email;
        [SerializeField] internal TMP_InputField _password;
        [SerializeField] internal TMP_Text _country;
        [SerializeField] internal Toggle _toggleOptinButton;
        [SerializeField] private Button _registerButton;
        [SerializeField] internal TMP_Dropdown _dropDown;

        [SerializeField] private Button _backButton;

        [Inject] private APIService _MockNetService;
        [Inject] private GameStateManager _gameState;


        private CountriesModelDown _countriesDic;

        public event Action<SignupEventData> OnFormSubmittedEvent;
        public event Action OnBackEvent;
        //public event Action OnFetchCountriesList;

        public void Init()
        {
            _registerButton.onClick.AddListener(OnSubmitSignupFormClicked);
            _backButton.onClick.AddListener(OnBackClicked);
        }

        public void Show()
        {
            HideAllAlerts();
            UpdateCountryDropDown();
        }

        //Populate list of countries
        public async void UpdateCountryDropDown()
        {
            List<string> countryNames = new List<string>();

            _countriesDic = await _MockNetService.GetCountriesJson(_gameState.Locale);

            for (int i = 0; i < _countriesDic.Countries.Count; i++)
            {
                countryNames.Add(_countriesDic.Countries[i].Name);
            }

            //Populate item DropDown list with strings
            _dropDown.options.Clear();
            _dropDown.AddOptions(countryNames);
            _dropDown.RefreshShownValue();
        }


        private void OnBackClicked()
        {
            OnBackEvent?.Invoke();
        }

        private void OnSubmitSignupFormClicked()
        {
            if (ValidateForm())
            {
                string code ="";

                for (int i=0; i < _countriesDic.Countries.Count; i++)
                {
                    if (_countriesDic.Countries[i].Name.Equals(_country.text))
                    {
                        code = _countriesDic.Countries[i].Code;
                    }
                }

                WaitingForCredentials();//Display 'Wait' message
                string username = _username.text;
                string email = _email.text;
                string password = _password.text;
                string country = _country.text;
                string countryCode = code;
                bool optin = _toggleOptinButton.isOn;

                SignupEventData evt = new SignupEventData(username, email, password, country, countryCode, optin);

                if (evt != null)
                {
                    OnFormSubmittedEvent?.Invoke(evt);
                }
            }
        }

        public bool ValidateForm()//TODO Finish form validation with other checks
        {
            return ValidateUsername() && ValidateEmail() && ValidatePassword();
        }

        private bool ValidateEmail()
        {
            Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,63})+)$");
            Match match = regex.Match(_email.text);
            if (match.Success)
                return true;
            else
            {
                EmailNotValid();
                return false;
            }
        }

        private bool ValidateUsername()
        {
            if (string.IsNullOrEmpty(_username.text))
            {
                UsernameNotValid();
                return false;
            }

            Regex regex = new(@"[^\u0020-\u03FF]+");
            Match match = regex.Match(_username.text);

            if (!match.Success)
                return true;
            else
            {
                UsernameNotValid();
                return false;
            }
        }

        private bool ValidatePassword()
        {
            if(_password.text.Length >= 6)
            {
                return true;
            }
            else
            {
                PasswordNotValid();
                return false;
            }
        }

        public async UniTask<CountriesModelDown> GetCountries(string locale)
        {
            var result = await _MockNetService.GetCountriesJson(locale);
            return result;
        }

        //Messages hide/Show
        internal void UsernameAlreadyExists()
        {
            HideAllAlerts();
            AlertUsernameAlreadyExists.SetActive(true);
        }

        internal void UsernameNotValid()
        {
            HideAllAlerts();
            AlertUsernameNotValid.SetActive(true);
        }

        internal void EmailAlreadyExists()
        {
            HideAllAlerts();
            AlertEmailAlreadyExists.SetActive(true);
        }

        internal void EmailNotValid()
        {
            HideAllAlerts();
            AlertEmailNotValid.SetActive(true);
        }

        internal void AccountCreated()
        {
            HideAllAlerts();
            AlertUserAccountCreated.SetActive(true);
        }

        internal void AccountCreatedFailed()
        {
            HideAllAlerts();
            AlertUserAcccountCreatedFailed.SetActive(true);
        }

        internal void WaitingForCredentials()
        {
            HideAllAlerts();
            AlertWaitingForCredentials.SetActive(true);
        }

        internal void PasswordNotValid()
        {
            HideAllAlerts();
            AlertPasswordNotValid.SetActive(true);
        }

        internal void HideAllAlerts()
        {
            AlertUsernameNotValid.SetActive(false);
            AlertUsernameAlreadyExists.SetActive(false);
            AlertEmailNotValid.SetActive(false);
            AlertEmailAlreadyExists.SetActive(false);
            AlertPasswordNotValid.SetActive(false);
            AlertWaitingForCredentials.SetActive(false);
            AlertUserAcccountCreatedFailed.SetActive(false);
            AlertUserAccountCreated.SetActive(false);
        }

        private void OnDestroy()
        {
            _registerButton.onClick.RemoveListener(OnSubmitSignupFormClicked);
            _backButton.onClick.RemoveListener(OnBackClicked);
        }
    }
}

