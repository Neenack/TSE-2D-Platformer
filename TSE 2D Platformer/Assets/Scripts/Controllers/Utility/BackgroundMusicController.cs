using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Utility;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioSource sound;
    public AudioClip slow, normal, fast;

    void Start()
    {
        SetupDelegates();
    }

    void OnDisable()
    {
        RemoveDelegates();
    }

    void SetupDelegates()
    {
        ZoneDelegates.onDifficultyDecided += ChangeMusic;
    }

    void RemoveDelegates()
    {
        ZoneDelegates.onDifficultyDecided -= ChangeMusic;
    }

    public void ChangeMusic()
    {
        int difficulty = GameObject.Find("Level Generator").GetComponent<LevelGeneration>().difficulty;

        if (difficulty <= 6) sound.clip = slow; sound.Play();
        if (difficulty > 6 && difficulty< 9) sound.clip = normal; sound.Play();
        if (difficulty >= 9) sound.clip = fast; sound.Play();
    }
}
