#define TRACE_ON
using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.GameState;
using System;

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

            //Localization
            _controller.OnLocalizationEvent += SetLocalization;

            //TEST
            _controller.OnTestEvent += LoadTestScene;
        }

        private void LoadTestScene()
        {
            _navService.Push("GameSelectionScene");
        }

        private void SetLocalization(string locale)
        {
            _gameState.SetLocale(locale);
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
            //Trace.Log("[SigninManager] Login param email : " + credentials.Email);
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
            Trace.Log("Login with username : " + _gameState.Username);

            //Get User Id
            UserIdModelDown userId = await _net.GetUserId(playerEmail);
            _gameState.GameStateModel.UserId = userId.UserId;

            Trace.Log("[SignIn Manager] From GetUserId, userId is : " + _gameState.GameStateModel.UserId);

            ////Get player history
            //HistoryModelDown history = await _net.GetHistory(userId.UserId);

            //if (history != null)
            //{

            //    Trace.Log("[SignIn Manager] From gethistory, scenario is : " + history.ScenarioName);

            //    //Update game state model
            //    _gameState.GameStateModel.CurrentScenario = history.ScenarioName;
            //    _gameState.GameStateModel.CurrentChapter = history.ChapterName;
            //    if (history.ChallengeId != string.Empty)
            //    {
            //        _gameState.GameStateModel.CurrentChallengeIndex = Int32.Parse(history.ChallengeId);
            //    }
            //    if (history.Score != null)
            //    {
            //        Trace.Log("history score = " + "-" + history.Score + "-");
            //        //_gameState.GameStateModel.CurrentScore = Int32.Parse(history.Score.ToString());
            //    }

            //}
            //else
            //{
            //    Trace.Log("[SignInMgr] History is null");
            //    _gameState.GameStateModel.CurrentScenario = string.Empty;
            //    _gameState.GameStateModel.CurrentChapter = string.Empty;
            //    _gameState.GameStateModel.CurrentChallengeIndex = 0;
            //    _gameState.GameStateModel.CurrentScore = 0;
            //}

            //_navService.Push("MainMenuScene", history);
            _navService.Push("MainMenuScene");
        }

        //CLICK ON RESET PASSWORD LINK
        private void ForgotPassword()
        {
            //Load SignUp scene
            _navService.Push("ResetPasswordScene");
        }

        //CLICK ON CREATE ACCOUNT BUTTON
        private void SignUp()
        {
            //Load SignUp scene
            _navService.Push("SignupScene");
        }

        //private async UniTask<HistoryModelDown> GetHistory(string id)
        //{
        //    HistoryModelDown result = await _net.GetHistory(id);
        //    return result;
        //}

    }
}