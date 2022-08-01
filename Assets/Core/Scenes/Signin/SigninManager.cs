using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;

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

        public void Initialize()
        {
            Debug.Log("SigninManager initialized!");
            _controller.Show();
            _controller.OnSigninFormSubmittedEvent += SubmitClicked;
            _controller.OnForgotPasswordClickedEvent += ForgotPassword;
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
                //var status = await CheckStatus(email);

                //if (status.DoesExist == true)
                //{
                    Debug.Log("Account activated, check credentials");
                    //Found email, check credentials
                    SigninModelUp credentials = new SigninModelUp
                    {
                        Email = signinData.Email,
                        Password = signinData.Password
                    };

                    Login(credentials).Forget();
                //}
                //else
                //{
                //    Debug.Log("ACCOOUNT NOT ACTIVATED");
                //    _controller.formInstance.AlertActivation.SetActive(true);
                //}
            }
        }


        //private async UniTask<EmailValidationDown> CheckStatus(string email)
        //{
        //    EmailValidationDown result = await _net.CheckStatus(email);
        //    return result;
        //}

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

        private void ForgotPassword()
        {
            //Load SignUp scene
            _navService.Push("ResetPasswordScene");
        }



        /*
        private async void ForgotPassword(string email)
        {
            //Get activation code
            //POst reset.php avec code


            Debug.Log("[Signin Manager] Call async task GetACtivationCode with email : " + email);
            ActivationCode code = await _net.PostActivationCode(email);
            Debug.Log("[Signin Manager] Got activation code: " + code.Code);


            Debug.Log("[NavService] current scene name : " + _navService.CurrentSceneName);
        }
        */
















        //activation code = GET activationcode.php(email)
        //GET reset.php (activationcode)
        //Check email
    }
}