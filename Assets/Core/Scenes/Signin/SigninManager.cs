using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.Signin
{
    // IInitializable : Zenject Start() equivalent
    // ITickable : Zenject Update() equivalent
    // IDisposable : C# OnDestroy() equivalent

    public class SigninManager : IInitializable
    {
        [Inject] private MockNetService _net;
        [Inject] private SigninController _controller;

        public void Initialize()
        {
            Debug.Log("SigninManager initialized!");
            _controller.Show();
            _controller.OnSigninFormSubmittedEvent += SubmitClicked;
        }

        private async void SubmitClicked(SigninEventData signinData)
        {
            //Check Email
            string email = signinData.Email;

            var result = await CheckEmail(email);

            if (result.DoesExist == false)//False = Did not find the email in DB
            {
                Debug.Log("No account !");
                _controller.NoAccount();
            }
            else
            {
                Debug.Log("Account found check credentials");
                //Found email, check credentials
                SigninModelUp credentials = new SigninModelUp
                {
                    Email = signinData.Email,
                    Password = signinData.Password
                };

                Login(credentials).Forget();
            }
        }

        private async UniTask<EmailValidationDown> CheckEmail(string email)
        {
            EmailValidationDown result = await _net.CheckEmail(email);
            return result;
        }

        private async UniTaskVoid Login(SigninModelUp formData)
        {
            SigninModelDown result = await _net.Login(formData);

            if (result.LoginResult == true)
            {
                _controller.formInstance.AlertRightCombination.SetActive(true);
                Debug.Log("You are login !");
            }
            else
            {
                _controller.formInstance.AlertWrongCombination.SetActive(true);
                Debug.Log("Sorry, wrong email/password combination.");
            }
        }
    }
}