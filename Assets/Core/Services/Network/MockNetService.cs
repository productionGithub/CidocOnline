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


        /*
        //const string HomeUrl = "https://ontomatchgame.huma-num.fr/";
        const string HomeUrl = "";//offline
        const string LanguagesFolder = "StreamingAssets/Languages/";

        //Signin & Signup process
        private string URL_CREATE_USER = Path.Combine(HomeUrl, "php/userSave.php");
        private string URL_CHECK_USERNAME = Path.Combine(HomeUrl, "php/checkusername.php?username={0}");
        private string URL_GET_USERNAME = Path.Combine(HomeUrl, "php/getusername.php?email={0}");
        private string URL_CHECK_EMAIL = Path.Combine(HomeUrl, "php/checkemail.php?email={0}");
        private string URL_CHECK_STATUS = Path.Combine(HomeUrl, "php/checkstatus.php?email={0}");
        private string URL_LOGIN = Path.Combine(HomeUrl, "php/login.php");
        private string URL_GET_ACTIVATION_CODE = Path.Combine(HomeUrl, "php/getactivationcode.php");
        private string URL_SEND_RESET_EMAIL = Path.Combine(HomeUrl, "php/sendresetlink.php");
        private string URL_GET_LOCALES_MANIFEST = "StreamingAssets/Languages/manifest.json";
        //private string URL_GET_LOCALES_MANIFEST = "/Users/Fix/IndytionProd/OntoMatchGame/Assets/StreamingAssets/Languages/manifest.json";//offline

        private string URL_GET_COUNTRIES = "StreamingAssets/Games/Marmoutier/marmoutier.json";
        //private string URL_GET_COUNTRIES = "";

        private string URL_SCENARII_CATALOG = "StreamingAssets/scenarii/scenariiCatalog.json";
        //private string URL_SCENARII_CATALOG = "/Users/Fix/IndytionProd/OntoMatchGame/Assets/StreamingAssets/scenarii/scenariiCatalog.json";//offline


        //Test load games
        private string URL_GET_GAMES = "http://ontomatchgame.huma-num.fr/StreamingAssets/Games/Marmoutier/marmoutier.json";


        */

        const string HomeUrl = "https://ontomatchgame.huma-num.fr/";
        const string LanguagesFolder = "StreamingAssets/Languages/";

        //Signin & Signup process
        private string URL_CREATE_USER = Path.Combine(HomeUrl, "php/userSave.php");
        private string URL_CHECK_USERNAME = Path.Combine(HomeUrl, "php/checkusername.php?username={0}");
        private string URL_GET_USERNAME = Path.Combine(HomeUrl, "php/getusername.php?email={0}");
        private string URL_CHECK_EMAIL = Path.Combine(HomeUrl, "php/checkemail.php?email={0}");
        private string URL_CHECK_STATUS = Path.Combine(HomeUrl, "php/checkstatus.php?email={0}");
        private string URL_LOGIN = Path.Combine(HomeUrl, "php/login.php");
        private string URL_GET_ACTIVATION_CODE = Path.Combine(HomeUrl, "php/getactivationcode.php");
        private string URL_SEND_RESET_EMAIL = Path.Combine(HomeUrl, "php/sendresetlink.php");
        private string URL_GET_LOCALES_MANIFEST = "StreamingAssets/Languages/manifest.json";
        private string URL_GET_COUNTRIES = "StreamingAssets/Games/Marmoutier/marmoutier.json";
        private string URL_SCENARII_CATALOG = "StreamingAssets/scenarii/scenariiCatalog.json";


        //Test load games
        private string URL_GET_GAMES = "http://ontomatchgame.huma-num.fr/StreamingAssets/Games/Marmoutier/marmoutier.json";



        //TODO Refactor GetCountriesJson with URL_GET_COUNTRIES

        //FETCH SCENARII CATALOG
        public async UniTask<ScenariiModelDown> GetCatalog()
        {
            //string url = Path.Combine(HomeUrl, URL_SCENARII_CATALOG);
            string url = Path.Combine(HomeUrl, URL_SCENARII_CATALOG);

            ScenariiModelDown result = await _net.GetAsync<ScenariiModelDown>(url);

            Debug.Log("[MockNet Service] Result returned from GetCatalog" + result.ToString());
            return result;
        }

        //FETCH LANGUAGE MANIFEST JSON FILE
        public async UniTask<LocalesManifestModel> GetLocalesManifestFile()
        {
            string url = Path.Combine(HomeUrl, URL_GET_LOCALES_MANIFEST);
            //string url = URL_GET_LOCALES_MANIFEST;//offline

            LocalesManifestModel result = await _net.GetAsync<LocalesManifestModel>(url);

            //Debug.Log("[MockNet Service] Resutl returned from GetLocalesManifestFile()" + result.Locales.ToString());
            return result;
        }

        //FETCH COUNTRIES JSON FILE
        public async UniTask<CountriesModelDown> GetCountriesJson(string locale)
        {

            string countriesPath = LanguagesFolder + locale + "/countries/countries.json";

            string url = Path.Combine(HomeUrl, countriesPath);
            //Debug.Log("LOCALE URL = " + url);
            CountriesModelDown result = await _net.GetAsync<CountriesModelDown>(url);
            return result;
        }

        //FETCH LANGUAGE DICTIONARY FOR LOCALIZATION
        public async UniTask<TranslationsModel> GetLocaleDictionary(string locale)
        {
            //Debug.Log("[MockNetService] Get local dictionary from remote language file]");

            string localePath = LanguagesFolder + locale;
            string fileNamePath = localePath + "/lang-" + locale + ".json";
            string languagePath = Path.Combine(HomeUrl, fileNamePath);
            //Debug.Log("LanguageFile path ===> " + languagePath);

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

        //CHECK USERNAME
        public async UniTask<ExistValidationDown> CheckUsername(string username)
        {
            string url = string.Format(URL_CHECK_USERNAME, username);
            ExistValidationDown result = await _net.GetAsync<ExistValidationDown>(url);
            return result;
        }

        //GET USERNAME
        public async UniTask<UsernameModelDown> GetUsername(string email)
        {
            Debug.Log("[MockNetService] GetUsername param email" + email);
            string url = string.Format(URL_GET_USERNAME, email);
            Debug.Log("[MockNetService] GetUsername param url" + url);
            UsernameModelDown result = await _net.GetAsync<UsernameModelDown>(url);
            return result;
        }

        //CHECK EMAIL
        public async UniTask<ExistValidationDown> CheckEmail(string email)
        {
            Debug.Log("[MockNetService] CheckEmail");
            string url = string.Format(URL_CHECK_EMAIL, email);
            ExistValidationDown result = await _net.GetAsync<ExistValidationDown>(url);
            return result;
        }

        //REGISTER
        public async UniTask<SignupModelDown> Register(SignupModelUp formData)
        {
            SignupModelDown result = await _net.PostAsync<SignupModelDown>(URL_CREATE_USER, formData);
            return result;
        }


        public async UniTask<GameModelDown> LoadGame()
        {
            GameModelDown result = await _net.GetAsync<GameModelDown>(URL_GET_GAMES);
            return result;
        }


        //LOGIN
        public async UniTask<SigninModelDown> Login(SigninModelUp formData)
        {
            Debug.Log("[MockNetService] Login");
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