using UnityEngine;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;

namespace StarterCore.Core.Scenes.Signup
{
    public class SignupController : MonoBehaviour
    {

        [SerializeField] private SignupForm _template;
        [SerializeField] private Transform _parent;

        public SignupForm formInstance;

        WWWForm wwwForm;//Content of formInstance fields
        
        public event Action OnFormSubmittedEvent;

        //private void Start()
        //{
        //    Show();
        //}

        public void Show()
        {
            Debug.Log("SignupController instanciated");
            _template.gameObject.SetActive(false); // Disable template

            formInstance = Instantiate(_template, _parent);
            Debug.Log("SHOW - formInstance ID = " + formInstance.GetInstanceID().ToString());
            formInstance.gameObject.SetActive(true);

            Debug.Log("formInstance._email.text = " + formInstance._email.text);

            formInstance.OnSubmitSignupFormClickedEvent += OnSubmitSignupFormClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            formInstance.Show();
        }

        private void OnSubmitSignupFormClicked()
        {   
            if (ValidateForm() == true)
            {
                //Create a Unity WWWForm
                //populate it with formInstance field values
                //Fire OnFormSubmittedEvent event
                OnFormSubmittedEvent?.Invoke();
                Debug.Log("[SignupController] Form validated, Email is : " + formInstance._email.text);
            }
            else
            {
                Debug.Log("[SignupController] Form not valid (lacking email)");
            }
            
        }

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

