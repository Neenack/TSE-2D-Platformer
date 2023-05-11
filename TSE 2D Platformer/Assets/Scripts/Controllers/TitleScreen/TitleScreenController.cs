using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleScreenController : MonoBehaviour
{
    [SerializeField]
    GameObject _controlsCanvas;

    [SerializeField]
    Button _playButton;

    [SerializeField]
    Button _controlsButton;

    [SerializeField]
    Button _exitButton;


    [SerializeField]
    Button _quitControlsButton;



    void Start()
    {
        _playButton.onClick.AddListener(StartGame);
        _controlsButton.onClick.AddListener(() => SetControlsCanvas(true));
        _exitButton.onClick.AddListener(EndGame);
        _quitControlsButton.onClick.AddListener(() => SetControlsCanvas(false));

        // Taken from docs.unity3d for use when listeners need parameters
        /*m_YourSecondButton.onClick.AddListener(delegate {TaskWithParameters("Hello"); });
        m_YourThirdButton.onClick.AddListener(() => ButtonClicked(42));
        m_YourThirdButton.onClick.AddListener(TaskOnClick);*/
    }

    void OnDisable()
    {
        _playButton.onClick.RemoveAllListeners();
        _controlsButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
        _quitControlsButton.onClick.RemoveAllListeners();
    }

    void StartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    void SetControlsCanvas(bool active)
    {
        _controlsCanvas.SetActive(active);
    }

    void EndGame()
    {
        Debug.Log("QUITTING THE GAME WILL NOT TRIGGER WITHIN THE EDITOR, ONLY IN BUILDS OF THE GAME");
        Application.Quit();
    }
}
