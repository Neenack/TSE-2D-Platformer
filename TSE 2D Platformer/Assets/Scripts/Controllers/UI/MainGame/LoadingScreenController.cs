using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Utility;
using TMPro;



namespace Controllers.UI.MainGame
{
    public class LoadingScreenController : MonoBehaviour
    {
        [SerializeField]
        Canvas _thisCanvas;

        [SerializeField]
        GameObject _loadingBar;

        const int EXPECTED_TICKS = 22;
        int _currentTicks;
        
        const float MIN_BAR_OFFSET = 1420f;
        const float MAX_BAR_OFFSET = 500f;
        const float BAR_OFFSET_FROM_BOTTOM = 300f;
        const float BAR_OFFSET_FROM_TOP = 700f;


        [SerializeField]
        TextMeshProUGUI _loadingScreenText;


        public void BeginSelf()
        {
            SetupDelegates();

            BeginLoading();
        }

        public void OnDestroy()
        {
            RemoveDelegates();
        }

        void SetupDelegates()
        {
            ZoneDelegates.onZoneCompletion += BeginLoading;

            ZoneDelegates.onZoneTickUpdate += UpdateBar;
            ZoneDelegates.onZoneGenerationFinish += FinishLoading;
        }

        void RemoveDelegates()
        {
            //Debug.Log("LoadingScreenDelegatesRemoved");

            ZoneDelegates.onZoneCompletion -= BeginLoading;

            ZoneDelegates.onZoneTickUpdate -= UpdateBar;
            ZoneDelegates.onZoneGenerationFinish -= FinishLoading;
        }

        
        void BeginLoading()
        {
            _thisCanvas.enabled = true;

            Debug.ClearDeveloperConsole();
            //Debug.Log("LoadingScreenBegin.");

            _loadingBar.GetComponent<RectTransform>().offsetMin = new Vector2(MAX_BAR_OFFSET, BAR_OFFSET_FROM_BOTTOM);
            _loadingBar.GetComponent<RectTransform>().offsetMax = new Vector2(-MIN_BAR_OFFSET, -BAR_OFFSET_FROM_TOP);
            ResetTicks();

            StartCoroutine(TextUpdate());
        }

        void ResetTicks()
        {
            _currentTicks = 0;
        }

        void UpdateBar()
        {
            _currentTicks++;

            float barFurtherOffset = (MIN_BAR_OFFSET - MAX_BAR_OFFSET) * ((float)_currentTicks / (float)EXPECTED_TICKS);
            //Debug.Log(_currentTicks);

            _loadingBar.GetComponent<RectTransform>().offsetMin = new Vector2(MAX_BAR_OFFSET, BAR_OFFSET_FROM_BOTTOM);
            _loadingBar.GetComponent<RectTransform>().offsetMax = new Vector2(-MIN_BAR_OFFSET + barFurtherOffset, -BAR_OFFSET_FROM_TOP);
        }

        void FinishLoading()
        {
            _loadingBar.GetComponent<RectTransform>().offsetMin = new Vector2(MAX_BAR_OFFSET, BAR_OFFSET_FROM_BOTTOM);
            _loadingBar.GetComponent<RectTransform>().offsetMax = new Vector2(-MAX_BAR_OFFSET, -BAR_OFFSET_FROM_TOP);

            StartCoroutine(OnFinishFinalWait());
        }


        IEnumerator TextUpdate()
        {
            while(1 == 1)
            {
                _loadingScreenText.text = "LOADING";
                yield return new WaitForSeconds(0.25f);

                _loadingScreenText.text = "LOADING.";
                yield return new WaitForSeconds(0.25f);

                _loadingScreenText.text = "LOADING..";
                yield return new WaitForSeconds(0.25f);

                _loadingScreenText.text = "LOADING...";
                yield return new WaitForSeconds(0.25f);
            }
        }


        IEnumerator OnFinishFinalWait()
        {
            yield return new WaitForSeconds(1.5f);

            StopCoroutine(TextUpdate());

            _thisCanvas.enabled = false;
        }
    }
}