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
        public GameObject AlertUserAccountCreated;
        public GameObject AlertUserAcccountCreatedFailed;
        public GameObject AlertWaitingForCredentials;

        [SerializeField] internal TMP_InputField _email;
        [SerializeField] internal TMP_InputField _password;
        [SerializeField] internal TMP_Text _country;
        [SerializeField] internal Toggle _toggleOptinButton;
        [SerializeField] private Button _registerButton;
        [SerializeField] private Button _backButton;
        [SerializeField] internal TMP_Dropdown _dropDown;

        public event Action OnSubmitSignupFormClickedEvent;
        public event Action OnBackClickedEvent;

        public void Show()
        {
            AlertEmailNotValid.SetActive(false);
            AlertEmailAlreadyExists.SetActive(false);
            AlertPasswordNotValid.SetActive(false);
            AlertUserAccountCreated.SetActive(false);
            AlertUserAcccountCreatedFailed.SetActive(false);
            AlertWaitingForCredentials.SetActive(false);

            _registerButton.onClick.AddListener(() => OnSubmitSignupFormClickedEvent?.Invoke());
            _backButton.onClick.AddListener(() => OnBackClickedEvent?.Invoke());
            //PopulateCountries();
        }



        /*
        //Populate list of countries
        public void PopulateCountries()
        {
            //Mock populating
            List<string> optionsList = new List<string>();


            optionsList.Add("HAGLAGLA ANTICONSTITUTIONELEMENT");
            optionsList.Add("--> ANTICONSTITUTIONELEMENT");
            optionsList.Add("------> ANTICONSTITUTIONELEMENT");

            _dropDown.options.Clear();
            _dropDown.AddOptions(optionsList);

            Debug.Log("DROPDOWN is " + _dropDown.name);

            //_dropDown.options.Clear();
            //_dropDown.options.Add(new TMP_Dropdown.OptionData("ANTICONSTITUTIONELEMENT"));
            //_dropDown.options.Add(new TMP_Dropdown.OptionData("--> ANTICONSTITUTIONELEMENT"));
            //_dropDown.options.Add(new TMP_Dropdown.OptionData("------> ANTICONSTITUTIONELEMENT"));

            //Debug.Log("countries are " + _dropDown.options.Count);

            //_dropDown.RefreshShownValue();

            //_dropDown.gameObject.SetActive(false);
            //_dropDown.gameObject.SetActive(true);
        }
        */

    }
}