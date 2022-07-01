using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.Signup
{
    // IInitializable : Zenject Start() equivalent
    // ITickable : Zenject Update() equivalent
    // IDisposable : C# OnDestroy() equivalent

    public class SignupManager : IInitializable
    {   
        [Inject] private MockNetService _net;
        [Inject] private SignupController _controller;

        string myUrl = "https://ontomatchgame.huma-num.fr/php/";
        string checkEmail = "checkemail.php";

        public void Initialize()
        {
            Debug.Log("SignupManager initialized!");
            _controller.Show();
            _controller.OnFormSubmittedEvent += SubmitClicked;
        }

        private void SubmitClicked(SignupEventData signupData)
        {
            //Check Email
            string email = signupData.Email;
            CheckEmail(email);


            //Register user
            SignupModelUp netModel = new SignupModelUp
            {
                Email = signupData.Email,
                Password = signupData.Password,
                Country = signupData.Country,
                Optin = signupData.Optin
            };

            RegisterUser(netModel).Forget();
        }

        private async UniTask<EmailValidationDown> CheckEmail(string email)
        {
            EmailValidationDown result =  await _net.CheckEmail(email);
            Debug.Log("Email availability is : " + result.IsValid);
            return result;
        }

        private async UniTaskVoid RegisterUser(SignupModelUp form)
        {
            SignupModelDown code = await _net.Register(form);
            Debug.Log("Activation code is :" + code.Code);
        }
    }
}



//Debug.Log("[SignupManager] Valid form has been submitted, call net service with : ");
//Debug.Log(string.Format("Email : {0}, password : {1}, Country : {2}, Optin : {3}",
//    c._email.text, c._password.text, c._country.text, c._toggleOptinButton.isOn));