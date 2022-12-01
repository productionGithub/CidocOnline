using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.GameState;

namespace StarterCore.Core.Scenes.Signin
{
    public class SigninManager : IInitializable
    {
        [Inject] private MockNetService _net;
        [Inject] private SigninController _controller;
        [Inject] private NavigationService _navService;
        [Inject] private GameStateManager _gameState;

        public void Initialize()
        {
            _controller.Show();
            _controller.OnSigninFormSubmittedEvent += SubmitClicked;
            _controller.OnForgotPasswordClickedEvent += ForgotPassword;
            _controller.OnCreateAccountClickedEvent += SignUp;
            _controller.OnEnglishLocalizationFlagClickedEvent += SetEnglishLocalization;
            _controller.OnFrenchLocalizationFlagClickedEvent += SetFrenchLocalization;
        }

        private void SetFrenchLocalization()
        {
            _gameState.SetLocale("fr");
        }

        private void SetEnglishLocalization()
        {
            _gameState.SetLocale("en");
        }

        //SUBMIT FORM
        private async void SubmitClicked(SigninEventData signinData)
        {
            string email = signinData.Email;

            var result = await CheckEmail(email);
            if (result.DoesExist == false)//False = Did not find the email in DB
            {
                _controller.NoAccount();
            }
            else
            {
                var status = await CheckStatus(email);
                if (status.IsActive == true)
                {
                    //Found email, check credentials
                    SigninModelUp credentials = new SigninModelUp
                    {
                        Email = signinData.Email,
                        Password = signinData.Password
                    };

                    Login(credentials).Forget();
                }
                else
                {
                    _controller.HideAllAlerts();
                    _controller.AlertActivation.SetActive(true);
                }
            }
        }

        //CHECK EMAIL
        private async UniTask<ExistValidationDown> CheckEmail(string email)
        {
            ExistValidationDown result = await _net.CheckEmail(email);
            return result;
        }

        //CHECK STATUS
        private async UniTask<StatusModelDown> CheckStatus(string email)
        {
            StatusModelDown result = await _net.CheckStatus(email);
            return result;
        }

        //LOGIN PROCESS
        private async UniTaskVoid Login(SigninModelUp credentials)
        {
            SigninModelDown result = await _net.Login(credentials);

            if (result.LoginResult == true)
            {
                _controller.HideAllAlerts();
                _controller.AlertRightCombination.SetActive(true);
                InitGame(credentials.Email);
            }
            else
            {
                _controller.HideAllAlerts();
                _controller.AlertWrongCombination.SetActive(true);
            }
        }

        private async void InitGame(string playerEmail)
        {
            //Login OK, get progression and load main menu screen
            UserNameModelDown userName = await _net.GetUsername(playerEmail);
            _gameState.Username = userName.Username;

            //Get User Id
            UserIdModelDown userId = await _net.GetUserId(playerEmail);
            _gameState.GameStateModel.UserId = userId.UserId;
            _navService.Push("MainMenuScene");
        }

        //CLICK ON RESET PASSWORD LINK
        private void ForgotPassword()
        {
            _navService.Push("ResetPasswordScene");
        }

        //CLICK ON CREATE ACCOUNT BUTTON
        private void SignUp()
        {
            _navService.Push("SignupScene");
        }

    }
}