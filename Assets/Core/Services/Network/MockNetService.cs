using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using Zenject;
using UnityEngine;
using System.Threading.Tasks;
using System;

namespace StarterCore.Core.Services.Network
{
    public class MockNetService
    {
        // More examples data here : https://random-data-api.com/documentation

        private const string URL_FORMAT_User = "https://www.talgorn.me/phpTest/hello.php?name={0}";
        private const string URL_CREATE_USER = "https://ontomatchgame.huma-num.fr/php/userSave.php";
        private const string URL_TEST_PHP = "https://ontomatchgame.huma-num.fr/php/phptest-tbd.php";
        private const string URL_CHECK_EMAIL = "https://ontomatchgame.huma-num.fr/php/checkemail.php?email={0}";
        private const string URL_CHECK_STATUS = "https://ontomatchgame.huma-num.fr/php/checkstatus.php?email={0}";
        private const string URL_LOGIN = "https://ontomatchgame.huma-num.fr/php/login.php";
        private const string URL_GET_ACTIVATION_CODE = "https://ontomatchgame.huma-num.fr/php/getactivationcode.php";
        private const string URL_RESET_PASSWORD = "https://ontomatchgame.huma-num.fr/php/reset.php";

        [Inject] private NetworkService _net;

        //CALL RESET PHP PAGE
        public async UniTask<ResetPasswordModelDown> CallResetPasswordForm(ResetPasswordModelUp data)
        {
            string url = string.Format(URL_RESET_PASSWORD, data);
            Debug.Log("String url is : " + url);
            var result = await _net.GetAsync<ResetPasswordModelDown>(url);
            //GetActivation code
            //Call sendResetLink with activation code
            Debug.Log("Called reset.php script !!!" + result.EmailSent);
            return result;
        }


        //CHECK EMAIL
        public async UniTask<EmailValidationDown> CheckEmail(string email)
        {
            string url = string.Format(URL_CHECK_EMAIL, email);
            EmailValidationDown result = await _net.GetAsync<EmailValidationDown>(url);
            return result;
        }

        ////CHECK STATUS
        //public async UniTask<EmailValidationDown> CheckStatus(string email)
        //{
        //    string url = string.Format(URL_CHECK_STATUS, email);
        //    EmailValidationDown result = await _net.GetAsync<EmailValidationDown>(url);
        //    return result;
        //}

        //public async UniTask<ActivationCode> CreateUserAccountAsync(UserProfile profile)
        //{
        //    ActivationCode result = await _net.PostAsync<ActivationCode>(URL_CREATE_USER, profile);
        //    return result;
        //}


        //REGISTER
        public async UniTask<SignupModelDown> Register(SignupModelUp formData)
        {
            SignupModelDown result = await _net.PostAsync<SignupModelDown>(URL_CREATE_USER, formData);
            //Debug.Log(result.Code);
            return result;
        }

        //LOGIN
        public async UniTask<SigninModelDown> Login(SigninModelUp formData)
        {
            SigninModelDown result = await _net.PostAsync<SigninModelDown>(URL_LOGIN, formData);

            Debug.Log("[Login] UNITASK Loginresult _> " + result.LoginResult);
            return result;
        }

        public async UniTask<ActivationCode> PostActivationCode(string email)
        {
            ActivationCode code = await _net.PostAsync<ActivationCode>(URL_GET_ACTIVATION_CODE, email);
            return code;
        }

        // More methods about the API
    }
}


/*
   //EXAMPLE OF DYNAMIC FETCH & DISPLAY
        public async UniTask<List<User>> GetRandomUsersAsync(string name)
        {
            string url = string.Format(URL_FORMAT_User, name);
            List<User> result = await _net.GetAsync<List<User>>(url);
            return result;
        }
*/