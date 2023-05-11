using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

using TMPro;


using Actors.Player;
using Actors.Player.Items;

using Delegates.Actors.Player;
using Delegates.Utility;



namespace Controllers.UI.MainGame
{
    public class PauseUIController : MonoBehaviour
    {
        [SerializeField]
        GameObject _pauseMenuCanvas;

        [SerializeField]
        GameObject _controlsCanvas;

        [SerializeField]
        Button _pauseMenuButton;

        [SerializeField]
        Button _continueButton;

        [SerializeField]
        Button _controlsButton;

        [SerializeField]
        Button _closeControlsButton;

        [SerializeField]
        Button _mainMenuButton;


        public void BeginSelf()
        {
            _pauseMenuButton.onClick.AddListener(TogglePause);
            _continueButton.onClick.AddListener(TogglePause);

            _controlsButton.onClick.AddListener(ToggleControlsCanvas);
            _closeControlsButton.onClick.AddListener(ToggleControlsCanvas);

            _mainMenuButton.onClick.AddListener(ReturnToMainMenu);

            SetupDelegates();
        }

        public void OnDisable()
        {
            _pauseMenuButton.onClick.RemoveAllListeners();
            _continueButton.onClick.RemoveAllListeners();
            _controlsButton.onClick.RemoveAllListeners();
            _closeControlsButton.onClick.RemoveAllListeners();
            _mainMenuButton.onClick.RemoveAllListeners();

            RemoveDelegates();
        }

        void SetupDelegates()
        {
            UIDelegates.onPause += TogglePause;
        }

        void RemoveDelegates()
        {
            UIDelegates.onPause -= TogglePause;
        }


        void TogglePause()
        {
            if(_pauseMenuCanvas.activeSelf)
            {
                _pauseMenuCanvas.SetActive(false);
                _controlsCanvas.SetActive(false);

                if(FlowDelegates.onPauseGame != null) FlowDelegates.onPauseGame(false);
            }

            else
            {
                _pauseMenuCanvas.SetActive(true);

                if(FlowDelegates.onPauseGame != null) FlowDelegates.onPauseGame(true);
            }
        }

        void ToggleControlsCanvas()
        {
            if(_controlsCanvas.activeSelf)
            {
                _controlsCanvas.SetActive(false);
            }

            else
            {
                _controlsCanvas.SetActive(true);
            }
        }

        void ReturnToMainMenu()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}