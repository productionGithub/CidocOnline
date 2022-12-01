using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.GameState;

using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.Signup
{
    public class SignupManager : IInitializable
    {   
        [Inject] private MockNetService _net;
        [Inject] private SignupController _controller;
        [Inject] private NavigationService _navService;
        [Inject] private GameStateManager _gameState;

        public void Initialize()
        {
            _controller.Init();
            _controller.Show();
            _controller.OnFormSubmittedEvent += SubmitClicked;
            _controller.OnBackEvent += BackEventClicked;
        }

        private async void SubmitClicked(SignupEventData signupData)
        {
            //Check Username
            string username = signupData.Username;
            var resultUsername = await CheckUsername(username);

            if (resultUsername.DoesExist == false)
            {
                //Check Email
                string email = signupData.Email;
                var result = await CheckEmail(email);

                if (result.DoesExist == false)//False = Did not find the email in DB
                {
                    //Register user
                    SignupModelUp netModel = new SignupModelUp
                    {
                        Username = signupData.Username,
                        Email = signupData.Email,
                        Password = signupData.Password,
                        Country = signupData.Country,
                        CountryCode = signupData.CountryCode,
                        Optin = signupData.Optin,
                        Lang = _gameState.Locale//Locale of the game when account created
                    };

                    RegisterUser(netModel).Forget();
                }
                else
                {
                    _controller.HideAllAlerts();
                    _controller.EmailAlreadyExists();
                }
            }
            else
            {
                _controller.HideAllAlerts();
                _controller.UsernameAlreadyExists();
            }
        }

        private async UniTask<ExistValidationDown> CheckUsername(string username)
        {
            ExistValidationDown result = await _net.CheckUsername(username);
            return result;
        }

        private async UniTask<ExistValidationDown> CheckEmail(string email)
        {
            ExistValidationDown result =  await _net.CheckEmail(email);
            return result;
        }

        private async UniTaskVoid RegisterUser(SignupModelUp form)
        {
            SignupModelDown code = await _net.Register(form);
            if (code.Code != "")
            {
                _controller.HideAllAlerts();
                _controller.AccountCreated();
            }
            else
            {
                _controller.HideAllAlerts();
                _controller.AccountCreatedFailed();
                Debug.LogError("[Server Error] : Sorry, could not create account. Please retry or contact administrator.");
            }
        }

        private void BackEventClicked()
        {
            _navService.Pop();
        }

        public void OnDestroy()
        {
            _controller.OnFormSubmittedEvent -= SubmitClicked;
            _controller.OnBackEvent -= BackEventClicked;
        }
    }
}