using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

namespace StarterCore.Core.Services.Network
{
    public class MockNetService
    {
        // More examples data here : https://random-data-api.com/documentation

        private const string URL_FORMAT_User = "https://www.talgorn.me/phpTest/hello.php?name={0}";
        private const string URL_CREATE_USER = "https://ontomatchgame.huma-num.fr/php/userSave.php";
        private const string URL_TEST_PHP = "https://ontomatchgame.huma-num.fr/php/phptest-tbd.php";

        [Inject] private NetworkService _net;

        public async UniTask<List<User>> GetRandomUsersAsync(string name)
        {
            //var timestamp = UnityEngine.Time.time;
            string url = string.Format(URL_FORMAT_User, name);
            List<User> result = await _net.GetAsync<List<User>>(url);
            return result;
        }

        public async UniTask<ActivationCode> CreateUserAccountAsync(UserProfile profile)
        {
            ActivationCode result = await _net.PostAsync<ActivationCode>(URL_CREATE_USER, profile);
            return result;
        }

        public async UniTask<SignupModelDown> TestJSON(SignupModelUp form)
        {

            Debug.Log(form.Email);
            SignupModelDown result = await _net.PostAsync<SignupModelDown>(URL_TEST_PHP, form);
            Debug.Log(result.Code);
            return result;
        }
        // More methods about the API
    }
}