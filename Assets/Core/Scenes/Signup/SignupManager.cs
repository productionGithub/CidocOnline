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

        public void Initialize()
        {
            Debug.Log("SignupManager initialized!");
            _controller.Show();
            _controller.OnFormSubmittedEvent += SubmitClicked;
        }

        private void SubmitClicked(SignupEventData signupData)
        {
            SignupModelUp netModel = new SignupModelUp
            {
                Email = signupData.Email,
                Password = signupData.Password,
                Country = signupData.Country,
                Optin = signupData.Optin
            };

            Debug.Log("[SignupManager] Valid form has been submitted, call net service with email : " + _controller.formInstance._email.text);
            SubmitAsync(netModel).Forget();
            //Debug.Log("[SignupManager] Valid form has been submitted, call net service with : ");
            //Debug.Log(string.Format("Email : {0}, password : {1}, Country : {2}, Optin : {3}",
            //    c._email.text, c._password.text, c._country.text, c._toggleOptinButton.isOn));
        }       


        private async UniTaskVoid SubmitAsync(SignupModelUp form)
        {
            //SignupForm f = _controller.formInstance;

            //UserProfile profile = new UserProfile
            //{
            //    Gamename = "OntoMatchGame",//TODO : Refactor not hard coded
            //    Email = f._email.text,
            //    Password = f._password.text,
            //    Country = f._country.text,
            //    Optin = f._toggleOptinButton.isOn.ToString()
            //};

            SignupModelDown code = await _net.TestJSON(form);
            //Test result
            Debug.Log("Activation code is :" + code.Code);
        }
    }
}
