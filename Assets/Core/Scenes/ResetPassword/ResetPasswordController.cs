using UnityEngine;
using System;
using Zenject;

using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace StarterCore.Core.Scenes.ResetPassword
{
    public class ResetPasswordController : MonoBehaviour
    {
        [SerializeField] internal TMP_InputField _email;
        [SerializeField] private Button _ResetButton;
        [SerializeField] private Button _BackButton;

        public GameObject AlertNoAccount;
        public GameObject AlertEmailNotValid;
        public GameObject AlertCheckEmail;
        public GameObject AlertConfirmation;
        public GameObject AlertEmailNotSent;
        public GameObject AlertStatus;






















        //[SerializeField] private ResetPasswordForm _template;
        //[SerializeField] private Transform _parent;

        //[Inject] private DiContainer _container;

        //public ResetPasswordForm 

        public event Action<string> OnResetPasswordEvent;
        public event Action OnBackEvent;

        public void Show()
        {
            Debug.Log("ResetController instanciated");
            HideAllAlerts();

            //_template.gameObject.SetActive(false); // Disable template

            //= Instantiate(_template, _parent);
            //_container.InjectGameObject(gameObject);//Inject dynamically : entity.gameObject injects on component AND children


            //gameObject.SetActive(true);
            _ResetButton.onClick.AddListener(OnResetPasswordFormClicked);
            _BackButton.onClick.AddListener(OnBackClicked);

            //OnBackClickedEvent += OnBackClicked; // Equiv to += () => OnSubmitSignupFormClicked();
            //Show();
        }

        private void OnBackClicked()
        {
            Debug.Log("Back btn event ok");
            OnBackEvent?.Invoke();
        }

        private void OnResetPasswordFormClicked()
        {
            HideAllAlerts();

            if (ValidateForm())
            {
                AlertCheckEmail.SetActive(true);
                OnResetPasswordEvent?.Invoke(_email.text);
            }
        }

        internal void NoAccountFound()
        {
            HideAllAlerts();
            AlertNoAccount.SetActive(true);
        }

        internal void ConfirmEmailSent()
        {
            HideAllAlerts();
            AlertConfirmation.SetActive(true);
        }

        internal void EmailNotSentError()
        {
            HideAllAlerts();
            AlertEmailNotSent.SetActive(true);
        }

        internal void StatusAlert()
        {
            HideAllAlerts();
            AlertStatus.SetActive(true);
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
            Match match = regex.Match(_email.text);
            if (match.Success)
                return true;
            else
            {
                AlertEmailNotValid.SetActive(true);
                return false;
            }
        }

        private void HideAllAlerts()
        {
            AlertNoAccount.SetActive(false);
            AlertEmailNotValid.SetActive(false);
            AlertCheckEmail.SetActive(false);
            AlertConfirmation.SetActive(false);
            AlertEmailNotSent.SetActive(false);
            AlertStatus.SetActive(false);
        }

        private void OnDestroy()
        {
            //OnResetPasswordClickedEvent -= OnResetPasswordFormClicked;
            //OnBackClickedEvent -= OnBackClicked;
        }
    }
}


