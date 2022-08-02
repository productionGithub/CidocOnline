using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace StarterCore.Core.Scenes.ResetPassword
{
    public class ResetPasswordForm : MonoBehaviour
    {
        [SerializeField] internal TMP_InputField _email;
        [SerializeField] private Button _ResetButton;
        [SerializeField] private Button _BackButton;

        public GameObject AlertNoAccount;
        public GameObject AlertEmailNotValid;
        public GameObject AlertCheckEmail;
        public GameObject AlertConfirmation;
        public GameObject AlertEmailNotSent;
        

        public event Action OnResetPasswordClickedEvent;
        public event Action OnBackClickedEvent;

        public void Show()
        {
            Debug.Log("[ResetForm Initialized!");

            AlertNoAccount.SetActive(false);
            AlertEmailNotValid.SetActive(false);
            AlertCheckEmail.SetActive(false);
            AlertConfirmation.SetActive(false);
            AlertEmailNotSent.SetActive(false);

            _ResetButton.onClick.AddListener(() => OnResetPasswordClickedEvent?.Invoke());
            _BackButton.onClick.AddListener(() => OnBackClickedEvent?.Invoke());
        }
    }
}
