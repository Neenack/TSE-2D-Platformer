using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using TMPro;
public class DifficultyUI : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI _currentDifficultyText;

    public Image _forcedNextBackground;
    public TextMeshProUGUI _forcedNextDifficultyText;

    public GameObject _levelGen;
    LevelGeneration _levelGenScript;

    // Start is called before the first frame update
    void Start()
    {
        _levelGenScript = _levelGen.GetComponent<LevelGeneration>();
        
    }

    // Update is called once per frame
    void Update()
    {
        int difficulty = _levelGenScript.difficulty;
        int nextDifficulty = _levelGenScript._forcedNextDifficulty;

        float currentGreen;
        float currentRed;


        // Current
        if(difficulty >= 5)
        {
            currentGreen = 255 - (((float)difficulty - 5) * 255 / 5);
            currentRed = 255;
        }
        else
        {
            currentGreen = 255;
            currentRed = 0 + (((float)difficulty - 1) * 255 / 4);
        }
        _currentDifficultyText.color = new Color(currentRed / 255, currentGreen / 255, 0, 255);
        _currentDifficultyText.text = "CurrentDifficulty: " + difficulty;


        // Forced
        float NextGreen;
        float NextRed;

        if(nextDifficulty >= 5)
        {
            NextGreen = 255 - (((float)nextDifficulty - 5) * 255 / 5);
            NextRed = 255;
        }
        else
        {
            NextGreen = 255;
            NextRed = 0 + (((float)nextDifficulty - 1) * 255 / 4);
        }


        _forcedNextDifficultyText.color = new Color(NextRed / 255, NextGreen / 255, 0, 255);
        _forcedNextDifficultyText.text = "Forced Next Difficulty: " + nextDifficulty;

        if(_levelGenScript._forcedNext)
        {
            _forcedNextDifficultyText.enabled = true;
            _forcedNextBackground.enabled = true;
        }

        else
        {
            _forcedNextDifficultyText.enabled = false;
            _forcedNextBackground.enabled = false;
        }
    }
}
