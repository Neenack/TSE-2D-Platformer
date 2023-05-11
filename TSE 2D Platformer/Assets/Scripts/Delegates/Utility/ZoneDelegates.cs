using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Delegates.Utility
{
    public class ZoneDelegates : MonoBehaviour
    {
        public delegate void OnZoneCompletion();
        public static OnZoneCompletion onZoneCompletion;

        public delegate void OnZoneCompletionRestart();
        public static OnZoneCompletionRestart onZoneCompletionRestart;

        public delegate void OnZoneGenerationStart();
        public static OnZoneGenerationStart onZoneGenerationStart;

        public delegate void OnZoneTickUpdate();
        public static OnZoneTickUpdate onZoneTickUpdate;

        public delegate void OnZoneGenerationFinish();
        public static OnZoneGenerationFinish onZoneGenerationFinish;

        public delegate void OnDifficultyDecided();
        public static OnDifficultyDecided onDifficultyDecided;
    }
}