using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace StarterCore.Core.Scenes.ResetPassword
{
    // IInitializable : Zenject Start() equivalent
    // ITickable : Zenject Update() equivalent
    // IDisposable : C# OnDestroy() equivalent

    public class ResetPasswordManager : IInitializable
    {
        [Inject] private MockNetService _net;
        [Inject] private ResetPasswordController _controller;

        public void Initialize()
        {
            Debug.Log("ResetPassword Manager initialized!");
            _controller.Show();
            _controller.OnResetPasswordEvent += CreateNewPassword;
        }

        private async void CreateNewPassword(string email)
        {
            var result = await CheckEmail(email);

            if (result.DoesExist == false)//False = Did not find the email in DB
            {
                _controller.NoAccountFound();
            }
            else
            {
                //Get activation code
                Debug.Log("[Reset Manager] Call async task GetActivationCode with email : " + email);
                ActivationCode code = await _net.PostActivationCode(email);
                Debug.Log("[Reset Manager] Got activation code: " + code.Code);
            }
        }

        private async UniTask<EmailValidationDown> CheckEmail(string email)
        {
            EmailValidationDown result = await _net.CheckEmail(email);
            return result;
        }

    }
}