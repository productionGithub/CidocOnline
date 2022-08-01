using UnityEngine;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace StarterCore.Core.Scenes.ResetPassword
{
    public class ResetPasswordController : MonoBehaviour
    {

        [SerializeField] private ResetPasswordForm _template;
        [SerializeField] private Transform _parent;

        public ResetPasswordForm formInstance;

        public event Action<string> OnResetPasswordEvent;
        public event Action OnBackEvent;

        public void Show()
        {
            Debug.Log("ResetController instanciated");
            _template.gameObject.SetActive(false); // Disable template

            formInstance = Instantiate(_template, _parent);
            formInstance.gameObject.SetActive(true);
            formInstance.OnResetPasswordClickedEvent += OnResetPasswordFormClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            formInstance.OnBackClickedEvent += OnBackClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            formInstance.Show();
        }

        private void OnBackClicked()
        {
            OnBackEvent?.Invoke();
        }

        private void OnResetPasswordFormClicked()
        {
            formInstance.AlertEmailNotValid.SetActive(false);
            formInstance.AlertNoAccount.SetActive(false);
            formInstance.AlertCheckEmail.SetActive(false);
            formInstance.ConfirmationMsg.SetActive(false);

            if (ValidateForm())
            {
                formInstance.AlertCheckEmail.SetActive(true);
                OnResetPasswordEvent?.Invoke(formInstance._email.text);
            }
        }

        internal void NoAccountFound()
        {
            formInstance.AlertCheckEmail.SetActive(false);
            formInstance.AlertNoAccount.SetActive(true);
        }


        /// <summary>
        /// Form validation
        /// </summary>
        /// <returns></returns>
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
            {
                formInstance.AlertEmailNotValid.SetActive(true);
                return false;
            }
        }

        //internal void NoAccount()
        //{
        //    formInstance.AlertNoAccount.SetActive(true);
        //}

    }
}


