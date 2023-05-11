using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Delegates.Utility
{
    public class FlowDelegates : MonoBehaviour
    {
        public delegate void OnPauseGame(bool pause);
        public static OnPauseGame onPauseGame;
    }
}