using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;

namespace StarterCore.Core.Scenes.ResetPassword
{
    // IInitializable : Zenject Start() equivalent
    // ITickable : Zenject Update() equivalent
    // IDisposable : C# OnDestroy() equivalent

    public class ResetPasswordManager : IInitializable
    {
        [Inject] private MockNetService _net;
        [Inject] private ResetPasswordController _controller;
        [Inject] private NavigationService _navService;

        public event Action<string> ActivationCodeOKEvent;

        public void Initialize()
        {
            Debug.Log("ResetPassword Manager initialized!");
            _controller.Show();
            _controller.OnResetPasswordEvent += CreateNewPassword;
            _controller.OnBackEvent += BackEventClicked;
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
                ActivationCodeModelUp emailUp = new ActivationCodeModelUp
                {
                    Email = email
                };
                ActivationCodeModelDown code = await _net.PostActivationCode(emailUp);
                Debug.Log("[Reset Manager] Got activation code: " + code.Code);


                //Next : call update password.
                //ActivationCodeOKEvent?.Invoke(code.Code);
                ResetPasswordModelUp data = new ResetPasswordModelUp
                {
                    Email = email,
                    Code = code.Code
                };

                UpdatePasswordModelDown updated = await _net.SendResetEmail(data);
                Debug.Log("An email has been sent to reset your password!");
            }
        }

        private async UniTask<EmailValidationDown> CheckEmail(string email)
        {
            EmailValidationDown result = await _net.CheckEmail(email);
            return result;
        }




        private void BackEventClicked()
        {
            _navService.Pop();
        }
    }
}