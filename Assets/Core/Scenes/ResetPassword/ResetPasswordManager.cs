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
                var status = await CheckStatus(email);
                if (status.IsActive == true)
                {
                    //Get activation code
                    Debug.Log("[Reset Manager] Call async task GetActivationCode with email : " + email);
                    ActivationCodeModelUp emailUp = new ActivationCodeModelUp
                    {
                        Email = email
                    };
                    ActivationCodeModelDown code = await _net.PostActivationCode(emailUp);

                    Debug.Log("[Reset Manager] Got activation code: " + code.Code);


                    //Setup data to pass to php script.
                    ResetPasswordModelUp data = new ResetPasswordModelUp
                    {
                        Email = email,
                        Code = code.Code,
                        Lang = "fr"//TODO manage multilingual game. Backend OK!
                    };
                    ResetPasswordModelDown resetLinkSent = await _net.SendResetEmail(data);

                    //Alert of return value
                    if (resetLinkSent.EmailSent == true)
                    {
                        _controller.ConfirmEmailSent();
                    }
                    else
                    {
                        _controller.EmailNotSentError();
                    }
                }
                else
                {
                    _controller.StatusAlert();
                }
            }
        }

        private async UniTask<EmailValidationDown> CheckEmail(string email)
        {
            EmailValidationDown result = await _net.CheckEmail(email);
            return result;
        }

        private async UniTask<StatusModelDown> CheckStatus(string email)
        {
            StatusModelDown result = await _net.CheckStatus(email);
            return result;
        }


        private void BackEventClicked()
        {
            _navService.Pop();
        }
    }
}