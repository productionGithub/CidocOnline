using UnityEngine;
using System;
using Zenject;

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

        [Inject] private DiContainer _container;

        public ResetPasswordForm formInstance;

        public event Action<string> OnResetPasswordEvent;
        public event Action OnBackEvent;

        public void Show()
        {
            Debug.Log("ResetController instanciated");
            _template.gameObject.SetActive(false); // Disable template

            formInstance = Instantiate(_template, _parent);
            _container.InjectGameObject(formInstance.gameObject);//Inject dynamically : entity.gameObject injects on component AND children


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
            HideAllAlerts();

            if (ValidateForm())
            {
                formInstance.AlertCheckEmail.SetActive(true);
                OnResetPasswordEvent?.Invoke(formInstance._email.text);
            }
        }

        internal void NoAccountFound()
        {
            HideAllAlerts();
            formInstance.AlertNoAccount.SetActive(true);
        }

        internal void ConfirmEmailSent()
        {
            HideAllAlerts();
            formInstance.AlertConfirmation.SetActive(true);
        }

        internal void EmailNotSentError()
        {
            HideAllAlerts();
            formInstance.AlertEmailNotSent.SetActive(true);
        }

        internal void AlertStatus()
        {
            HideAllAlerts();
            formInstance.AlertStatus.SetActive(true);
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

        private void HideAllAlerts()
        {
            formInstance.AlertEmailNotValid.SetActive(false);
            formInstance.AlertNoAccount.SetActive(false);
            formInstance.AlertCheckEmail.SetActive(false);
            formInstance.AlertConfirmation.SetActive(false);
            formInstance.AlertEmailNotSent.SetActive(false);
        }

        private void OnDestroy()
        {
            formInstance.OnResetPasswordClickedEvent -= OnResetPasswordFormClicked;
            formInstance.OnBackClickedEvent -= OnBackClicked;
        }
    }
}


