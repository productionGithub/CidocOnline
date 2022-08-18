using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network.Models;
using System.Collections.Generic;
using Zenject;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.IO;
using Newtonsoft.Json;

namespace StarterCore.Core.Services.Network
{
    public class MockNetService
    {
        [Inject] private NetworkService _net;

        const string HomeUrl = "https://ontomatchgame.huma-num.fr/";
        const string LanguagesFolder = "StreamingAssets/Languages/";

        //Signin & Signup process
        private string URL_CREATE_USER = Path.Combine(HomeUrl, "php/userSave.php");
        private string URL_CHECK_EMAIL = Path.Combine(HomeUrl, "php/checkemail.php?email={0}");
        private string URL_CHECK_STATUS = Path.Combine(HomeUrl, "php/checkstatus.php?email={0}");
        private string URL_LOGIN = Path.Combine(HomeUrl, "php/login.php");
        private string URL_GET_ACTIVATION_CODE = Path.Combine(HomeUrl, "php/getactivationcode.php");
        private string URL_SEND_RESET_EMAIL = Path.Combine(HomeUrl, "php/sendresetlink.php");
        private string URL_GET_LOCALES_MANIFEST = "StreamingAssets/Languages/manifest.json";

        //FETCH LANGUAGE MANIFEST JSON FILE
        public async UniTask<LocalesManifestModel> GetLocalesManifestFile()
        {
            string url = Path.Combine(HomeUrl, URL_GET_LOCALES_MANIFEST);

            LocalesManifestModel result = await _net.GetAsync<LocalesManifestModel>(url);

            //Debug.Log("[MockNet Service] Resutl returned from GetLocalesManifestFile()" + result.Locales.ToString());
            return result;
        }

        //FETCH LANGUAGE DICTIONARY FOR LOCALIZATION
        public async UniTask<TranslationsModel> GetLocaleDictionary(string locale)
        {
            //Debug.Log("[MockNetService] Get local dictionary from remote language file]");

            string localePath = LanguagesFolder + locale;
            string fileNamePath = localePath + "/lang-" + locale + ".json";
            string languagePath = Path.Combine(HomeUrl, fileNamePath);
            Debug.Log("LanguageFile path ===> " + languagePath);

            var dictionary = await _net.GetAsync<TranslationsModel>(languagePath);
            return dictionary;
        }

        //CHECK STATUS
        public async UniTask<StatusModelDown> CheckStatus(string email)
        {
            string url = string.Format(URL_CHECK_STATUS, email);
            StatusModelDown result = await _net.GetAsync<StatusModelDown>(url);
            return result;
        }

        //CHECK EMAIL
        public async UniTask<EmailValidationDown> CheckEmail(string email)
        {
            string url = string.Format(URL_CHECK_EMAIL, email);
            EmailValidationDown result = await _net.GetAsync<EmailValidationDown>(url);
            return result;
        }

        //REGISTER
        public async UniTask<SignupModelDown> Register(SignupModelUp formData)
        {
            SignupModelDown result = await _net.PostAsync<SignupModelDown>(URL_CREATE_USER, formData);
            return result;
        }

        //LOGIN
        public async UniTask<SigninModelDown> Login(SigninModelUp formData)
        {
            SigninModelDown result = await _net.PostAsync<SigninModelDown>(URL_LOGIN, formData);
            return result;
        }

        //ACTIVATION CODE
        public async UniTask<ActivationCodeModelDown> PostActivationCode(ActivationCodeModelUp email)
        {
            ActivationCodeModelDown code = await _net.PostAsync<ActivationCodeModelDown>(URL_GET_ACTIVATION_CODE, email);
            return code;
        }

        //SEND RESET PASSWORD EMAIL
        public async UniTask<ResetPasswordModelDown> SendResetEmail(ResetPasswordModelUp data)
        {
            ResetPasswordModelDown emailSent = await _net.PostAsync<ResetPasswordModelDown>(URL_SEND_RESET_EMAIL, data);
            Debug.Log("Returned from reset.php script ===> " + emailSent.EmailSent);
            return emailSent;
        }
    }
}

//Decks
//private string URL_JSON_SA = Path.Combine(HomeUrl, "StreamingAssets/DecksFiles/Instances/Instances.json");
//private string URL_JSON = Path.Combine(HomeUrl, "json/instance.json");
//Localisation

/*
   //EXAMPLE OF DYNAMIC FETCH & DISPLAY
        public async UniTask<List<User>> GetRandomUsersAsync(string name)
        {
            string url = string.Format(URL_FORMAT_User, name);
            List<User> result = await _net.GetAsync<List<User>>(url);
            return result;
        }
*/

/*
//Path to instances.json file
readonly private string jsonFileLocation = "DecksFiles/Instances/";
private string jsonFilePath;
private readonly string jsonFileName = "Instances.json";

//Instance cards
public List<InstanceCard> instanceCards;

//GET JSON FILE
public async UniTask<List<InstanceCardModelDown>> GetJsonFile()
{
    //string url = string.Format(URL_RESET_PASSWORD, data);
    //Debug.Log("String url is : " + url);
    List<InstanceCardModelDown> jsonString = await _net.GetAsync<List<InstanceCardModelDown>>(URL_JSON_SA);
    return jsonString;
}
*/