using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Localization;

using TMPro;
using UnityEngine.SceneManagement;

namespace StarterCore.Core.Scenes.Signin
{
    // IInitializable : Zenject Start() equivalent
    // ITickable : Zenject Update() equivalent
    // IDisposable : C# OnDestroy() equivalent

    public class SigninManager : IInitializable
    {
        [Inject] private MockNetService _net;
        [Inject] private SigninController _controller;
        [Inject] private NavigationService _navService;
        [Inject] private GameStateManager _gameState;

        public void Initialize()
        {
            //Set language
            //SetLocalization(_gameState.Locale);

            _controller.Show();
            _controller.OnSigninFormSubmittedEvent += SubmitClicked;
            _controller.OnForgotPasswordClickedEvent += ForgotPassword;
            _controller.OnCreateAccountClickedEvent += SignUp;

            //Localization
            _controller.OnLocalizationEvent += SetLocalization;
            _navService.OnSceneChangeEvent += SceneIsLoaded;

            
            //var cards = await _state.LoadCards();
            //_controller._cardName.GetComponent<TextMeshProUGUI>().text = cards[8].imageName;
        }

        private void SetLocalization(string locale)
        {
            _gameState.SetLocale(locale);
            //_navService.RefreshCurrentScene();
            //_ = _localizationController.ChangeLocale(locale);
        }

        private void SceneIsLoaded(string name)
        {

            Debug.Log("Scene is loaded, let's translate it -> " + name);
        }

        //SUBMIT FORM
        private async void SubmitClicked(SigninEventData signinData)
        {
            string email = signinData.Email;

            var result = await CheckEmail(email);

            //TEST
            //var cards = await _net.GetJsonFile();
            //_controller._cardName.GetComponent<TextMeshProUGUI>().text = cards[8].ImageName;
            //Debug.Log("Fetched JSON is : " + cards);

            if (result.DoesExist == false)//False = Did not find the email in DB
            {
                Debug.Log("No account !");
                _controller.NoAccount();
            }
            else
            {
                var status = await CheckStatus(email);
                Debug.Log("Check status result is ------> " + status.IsActive);

                if (status.IsActive == true)
                {
                    Debug.Log("Account activated, checking credentials");
                    //Found email, check credentials
                    SigninModelUp credentials = new SigninModelUp
                    {
                        Email = signinData.Email,
                        Password = signinData.Password
                    };

                    Login(credentials).Forget();
                }
                else
                {
                    _controller.HideAllAlerts();
                    _controller.formInstance.AlertActivation.SetActive(true);
                }
            }
        }

        //CHECK EMAIL
        private async UniTask<EmailValidationDown> CheckEmail(string email)
        {
            EmailValidationDown result = await _net.CheckEmail(email);
            return result;
        }

        //CHECK STATUS
        private async UniTask<StatusModelDown> CheckStatus(string email)
        {
            StatusModelDown result = await _net.CheckStatus(email);
            return result;
        }

        //LOGIN PROCESS
        private async UniTaskVoid Login(SigninModelUp formData)
        {
            SigninModelDown result = await _net.Login(formData);

            if (result.LoginResult == true)
            {
                _controller.HideAllAlerts();
                _controller.formInstance.AlertRightCombination.SetActive(true);
            }
            else
            {
                _controller.HideAllAlerts();
                _controller.formInstance.AlertWrongCombination.SetActive(true);
            }
        }

        //CLICK ON RESET PASSWORD LINK
        private void ForgotPassword()
        {
            //Load SignUp scene
            _navService.Push("ResetPasswordScene");
        }

        //CLICK ON CREATE ACCOUNT BUTTON
        private void SignUp()
        {
            //Load SignUp scene
            _navService.Push("SignupScene");
        }
    }
}