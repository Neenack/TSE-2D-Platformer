using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Utility;
using Controllers.UI.MainGame;



namespace Controllers.UI
{
    public class UIController : MonoBehaviour
    {
        LoadingScreenController _loadingScreenController;

        ItemsSelectionController _itemsSelectionController;

        PauseUIController _pauseUIController;

        public void BeginSelf()
        {
            _loadingScreenController = GetComponent<LoadingScreenController>();
            _loadingScreenController.BeginSelf();

            _itemsSelectionController = GetComponent<ItemsSelectionController>();

            _pauseUIController = GetComponent<PauseUIController>();
            _pauseUIController.BeginSelf();
        }
    }
}