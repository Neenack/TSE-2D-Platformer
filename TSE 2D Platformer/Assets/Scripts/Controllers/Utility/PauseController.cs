using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Delegates.Utility;


namespace Controllers.Utility
{
    public class PauseController : MonoBehaviour
    {
        public void BeginSelf()
        {
            SetPause(false);

            SetupDelegates();
        }

        void OnDisable()
        {
            SetPause(false);

            RemoveDelegates();
        }

        void SetupDelegates()
        {
            FlowDelegates.onPauseGame += SetPause;
        }

        void RemoveDelegates()
        {
            FlowDelegates.onPauseGame -= SetPause;
        }


        void SetPause(bool pause)
        {
            if(pause)
            {
                GameController._isPaused = true;
                Time.timeScale = 0;
            }

            else
            {
                GameController._isPaused = false;
                Time.timeScale = 1;
            }
        }
    }
}