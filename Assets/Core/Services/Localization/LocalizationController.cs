using UnityEngine;
using System.Collections.Generic;
using StarterCore.Core.Services.Navigation;
using Zenject;
using TMPro;

using StarterCore.Core.Utils;
using StarterCore.Core.Services.Localization.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StarterCore.Core.Services.Localization
{
    public class LocalizationController : MonoBehaviour
    {
        Translations t;//DTO for Json language file
        TextMeshProUGUI[] _tmProUGUIList;//Array of all TMProUGUI object in current scene

        [Inject] private NavigationService _navService;


        //Fetch proper JSON language depending on the Locale of the game
        private string _langDictionary = @"{
   ""locale"": ""en"",
""static-toxt"" : {
   ""SigninScene"": {
      ""title-text"": ""Welcome to OntoMatchGame!"",
      ""intro-text"": ""Semantic Data Representation should let you generate, share and consume Cultural Heritage data with colleagues around the world, to ask and answer questions against more data and more integrated knowledge nets."",
      ""SigninScene-email-label"": ""Email address"",
      ""SigninScene-password-label"": ""Password"",
      ""SigninScene-forgot-label"": ""Forgot your password?"",
      ""SigninScene-ok-button"": ""Forgot your password?"",
      ""SigninScene-fail-text"": ""Wrong email / password combination-Try again-"",
      ""SigninScene-success-text"": ""LOGIN SUCCESSFUL!"",
      ""SigninScene-activation-text"": ""Account not activated-Check your email and click on activation link-"",
      ""SigninScene-waiting-text"": ""Checking credentials-Please wait..."",
      ""SigninScene-signup-label"": ""Dont have an account?"",
      ""SigninScene-create-button"": ""create my account"",
      ""SigninScene-dumbfield-text"": ""This is a test""
   },
      ""SignupScene"": {
      ""SignupScene-title-text"": ""Welcome AGAIN!"",
      ""SignupScene-intro-text"": ""Semantic Data Representation should let you generate, share and consume Cultural Heritage data with colleagues around the world, to ask and answer questions against more data and more integrated knowledge nets."",
      ""SignupScene-email-label"": ""Email address"",
      ""SignupScene-password-label"": ""Password"",
      ""SignupScene-forgot-label"": ""Forgot your password?"",
      ""SignupScene-ok-button"": ""Forgot your password?"",
      ""SignupScene-fail-text"": ""Wrong email / password combination-Try again-"",
      ""SignupScene-success-text"": ""LOGIN SUCCESSFUL!"",
      ""SignupScene-activation-text"": ""Account not activated-Check your email and click on activation link-"",
      ""SignupScene-waiting-text"": ""Checking credentials-Please wait..."",
      ""SignupScene-signup-label"": ""Dont have an account?"",
      ""SignupScene-create-button"": ""create my account"",
      ""SignupScene-dumbfield-text"": ""This is a test""
    }
    }
}"
;

        void Start()
        {
            //Check locale
            //Get Json Locale dictionary
            //Translate scene
            TranslateStaticText(_langDictionary);

        }

        private void TranslateStaticText(string json)
        {
            _tmProUGUIList = FindObjectsOfType<TextMeshProUGUI>();
            JSON.TryDeserialize<Translations>(json, out t);

            //Parse scene names in JSON
            foreach (KeyValuePair<string, Dictionary<string, string>> snKey in t.StaticText)
            {
                //if scene name is current scene name
                if (snKey.Key.Equals(_navService.CurrentSceneName))
                {
                    //Parse all TMPro text fields
                    foreach (TextMeshProUGUI tmpObj in _tmProUGUIList)
                    {
                        //Parse all JSON Key for language dictionary
                        int index = 0;// index needed for SetValue() method.
                        foreach (KeyValuePair<string, string> dicS in t.StaticText[snKey.Key])
                        {
                            //if field text matches Json key, update Filed content with JSon value
                            if (dicS.Key.Equals(tmpObj.text))
                            {
                                tmpObj.text = dicS.Value;
                                break;
                            }
                            index++;
                        }
                    }
                }
            }
        }

    }
}