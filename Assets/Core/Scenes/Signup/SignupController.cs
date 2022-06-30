using UnityEngine;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using UnityEngine.Networking;

namespace StarterCore.Core.Scenes.Signup
{
    public class SignupController : MonoBehaviour
    {

        [SerializeField] private SignupForm _template;
        [SerializeField] private Transform _parent;

        public SignupForm formInstance;

        WWWForm wwwForm;//Content of formInstance fields
        
        public event Action<SignupEventData> OnFormSubmittedEvent;

        //private void Start()
        //{
        //    Show();
        //}

        string myUrl = "https://ontomatchgame.huma-num.fr/php/";
        string insertUrl = "userSave.php";

        public void Show()
        {
            Debug.Log("SignupController instanciated");
            _template.gameObject.SetActive(false); // Disable template

            formInstance = Instantiate(_template, _parent);
            formInstance.gameObject.SetActive(true);

            formInstance.OnSubmitSignupFormClickedEvent += OnSubmitSignupFormClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            formInstance.Show();
        }

        private void OnSubmitSignupFormClicked()
        {


            //StartCoroutine(SubmitAsync());


            if (ValidateForm())
            {
                string email = formInstance._email.text;
                string password = formInstance._password.text;
                string country = formInstance._country.text;
                bool optin = formInstance._toggleOptinButton.isOn;

                var evt = new SignupEventData(email, password, country, optin);

                OnFormSubmittedEvent?.Invoke(evt);


                //StartCoroutine(SubmitAsync());


                //Create a Unity WWWForm

                /*
                WWWForm form = new WWWForm();
                form.AddField("gamename", "GamingNamos");
                form.AddField("email", formInstance._email.text);
                form.AddField("password", formInstance._password.text);
                form.AddField("firstname", "Bitos");
                form.AddField("lastname", "aka Bitman");
                form.AddField("country", formInstance._country.text);

                Debug.Log(formInstance._email.text);

                //Fire OnFormSubmittedEvent event
                //OnFormSubmittedEvent?.Invoke();
                //Debug.Log("[SignupController] Form validated, Email is : " + formInstance._email.text);
                //OnFormSubmittedEvent?.Invoke(formInstance._email.text);
                OnFormSubmittedEvent?.Invoke(form);

                */
            }
            else
            {
                Debug.Log("[SignupController] Form not valid (lacking email)");
            }
        }


        /*

        // insert a player to database
        IEnumerator SubmitAsync()
        {
            string urlTest = "https://ontomatchgame.huma-num.fr/php/phptest-tbd.php";


            UnityWebRequest www = UnityWebRequest.Get(urlTest);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                // You can use Debug logs 
                Debug.Log(www.error);
            }
            else
            {
                // You can use Debug logs 
                Debug.Log("php test return content: " + www.downloadHandler.text);
                OnFormSubmittedEvent?.Invoke(formInstance._email.text);
            }
            */
        /*
        WWWForm form = new WWWForm();
        form.AddField("gamename", "GamingNamos");
        form.AddField("email", formInstance._email.text);
        form.AddField("password", formInstance._password.text);
        form.AddField("firstname", "Bitos");
        form.AddField("lastname", "aka Bitman");
        form.AddField("country", formInstance._country.text);

        UnityWebRequest www = UnityWebRequest.Get(myUrl + insertUrl);//, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            // You can use Debug logs 
            Debug.Log(www.error);
        }
        else
        {
            string activationCode = www.downloadHandler.text;
            // You can use Debug logs 
            Debug.Log("Form insert complete! code is : " + activationCode);
        }

    }
                    */

        public bool ValidateForm()
        {
            return ValidateEmail();
        }

        private bool ValidateEmail()
        {
            Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,63})+)$");
            Match match = regex.Match(formInstance._email.text);
            if (match.Success)
                return true;
            else
                return false;
        }
    }
}

