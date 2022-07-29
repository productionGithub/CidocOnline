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

        public GameObject AlertNoAccount;
        public GameObject AlertEmailNotValid;
        public GameObject ConfirmationMsg;

        public event Action OnResetPasswordClickedEvent;

        public void Show()
        {
            Debug.Log("[ResetForm Initialized!");

            AlertNoAccount.SetActive(false);
            AlertEmailNotValid.SetActive(false);
            ConfirmationMsg.SetActive(false);

            _ResetButton.onClick.AddListener(() => OnResetPasswordClickedEvent?.Invoke());
        }
    }
}
