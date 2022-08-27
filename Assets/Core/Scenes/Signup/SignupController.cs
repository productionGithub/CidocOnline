using UnityEngine;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using UnityEngine.SceneManagement;
using Zenject;
using System.Collections.Generic;
using TMPro;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Services.GameState;

using Newtonsoft.Json;
using Cysharp.Threading.Tasks;


namespace StarterCore.Core.Scenes.Signup
{
    public class SignupController : MonoBehaviour
    {
        [SerializeField] private SignupForm _template;
        [SerializeField] private Transform _parent;

        [Inject] private DiContainer _container;
        [Inject] private MockNetService _MockNetService;
        [Inject] private GameStateManager _gameState;


        public SignupForm formInstance;
        public CountriesModelDown _countriesDic;

        public int OnBackButtonClicked { get; private set; }

        public event Action<SignupEventData> OnFormSubmittedEvent;
        public event Action OnBackEvent;
        public event Action OnFetchCountriesList;

        public void Show()
        {
            // Disable template that is instanciated
            _template.gameObject.SetActive(false);

            //Instanciate template, update DI container
            formInstance = Instantiate(_template, _parent);
            _container.InjectGameObject(formInstance.gameObject);//Inject dynamically : entity.gameObject injects on component AND children

            formInstance.gameObject.SetActive(true);

            formInstance.OnSubmitSignupFormClickedEvent += OnSubmitSignupFormClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            formInstance.OnBackClickedEvent += OnBackClicked; // Equiv to += () => OnSubmitSignupFormClicked();

            UpdateCountryDropDown();

            formInstance.Show();

            //OnFetchCountriesList?.Invoke();
            //PopulateCountries();
        }

        //Populate list of countries
        public async void UpdateCountryDropDown()
        {
            List<string> countryNames = new List<string>();

            _countriesDic = await _MockNetService.GetCountriesJson(_gameState.Locale);
            //var countriesDic = await _networkService.GetCountriesJson();
            for (int i = 0; i < _countriesDic.Countries.Count; i++)
            {
                countryNames.Add(_countriesDic.Countries[i].Name);
                Debug.Log("[Signup Manager] Adding country ->" + _countriesDic.Countries[i].Name);
            }

            //Populate item DropDown list with strings
            formInstance._dropDown.options.Clear();
            formInstance._dropDown.AddOptions(countryNames);
            formInstance._dropDown.RefreshShownValue();


            //Write names to file
            //Serialize List
            //var Json = JsonConvert.SerializeObject(countryNames);
        }


        private void OnBackClicked()
        {
            OnBackEvent?.Invoke();
        }

        private void OnSubmitSignupFormClicked()
        {
            if (ValidateForm())
            {
                string code="";

                for (int i=0; i < _countriesDic.Countries.Count; i++)
                {
                    if(_countriesDic.Countries[i].Name == formInstance._country.text)
                    {
                        code = _countriesDic.Countries[i].Code;
                    }
                }

                WaitingForCredentials();//Display 'Wait' message
                string email = formInstance._email.text;
                string password = formInstance._password.text;
                string country = formInstance._country.text;
                string countryCode = code;
                bool optin = formInstance._toggleOptinButton.isOn;

                var evt = new SignupEventData(email, password, country, countryCode, optin);

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
                EmailNotValid();
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
                PasswordNotValid();
                return false;
            }
        }


        

    

        /// <summary>
        /// Call this after modifying options while the dropdown is displayed
        /// to make sure the visual is up to date.
        /// </summary>
        //public static void RefreshOptions(this TMPro.TMP_Dropdown dropdown)
        //{
        //    dropdown.enabled = false;
        //    dropdown.enabled = true;
        //    dropdown.Show();
        //}


        public async UniTask<CountriesModelDown> GetCountries(string locale)
        {
            var result = await _MockNetService.GetCountriesJson(locale);
            return result;
        }


        //Messages hide/Show
        internal void EmailAlreadyExists()
        {
            HideAllAlerts();
            formInstance.AlertEmailAlreadyExists.SetActive(true);
        }

        internal void AccountCreated()
        {
            HideAllAlerts();
            formInstance.AlertUserAccountCreated.SetActive(true);
        }

        internal void AccountCreatedFailed()
        {
            HideAllAlerts();
            formInstance.AlertUserAcccountCreatedFailed.SetActive(true);
        }

        internal void WaitingForCredentials()
        {
            HideAllAlerts();
            formInstance.AlertWaitingForCredentials.SetActive(true);
        }

        internal void EmailNotValid()
        {
            HideAllAlerts();
            formInstance.AlertEmailNotValid.SetActive(true);
        }

        internal void PasswordNotValid()
        {
            HideAllAlerts();
            formInstance.AlertPasswordNotValid.SetActive(true);
        }

        internal void HideAllAlerts()
        {
            formInstance.AlertEmailNotValid.SetActive(false);
            formInstance.AlertEmailAlreadyExists.SetActive(false);
            formInstance.AlertPasswordNotValid.SetActive(false);
            formInstance.AlertWaitingForCredentials.SetActive(false);
            formInstance.AlertUserAcccountCreatedFailed.SetActive(false);
            formInstance.AlertUserAccountCreated.SetActive(false);
        }

        private void OnDestroy()
        {
            formInstance.OnSubmitSignupFormClickedEvent -= OnSubmitSignupFormClicked;
            formInstance.OnBackClickedEvent -= OnBackClicked;
        }
    }
}

