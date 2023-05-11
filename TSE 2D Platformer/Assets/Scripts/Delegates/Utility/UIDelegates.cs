using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Delegates.Utility
{
    public class UIDelegates : MonoBehaviour
    {

        public delegate void OnPause();
        public static OnPause onPause;
    }
}