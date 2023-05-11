using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


using Actors.Player;
using Actors.Player.Items;

using Delegates.Actors.Player;
using Delegates.Utility;



namespace Controllers.UI.MainGame
{
    public class ItemsSelectionController : MonoBehaviour
    {
        PlayerInventory _playerInventory;

        [SerializeField]
        List<GameObject> _backgrounds;

        [SerializeField]
        List<TextMeshProUGUI> _texts;


        void OnInventoryInitialized()
        {
            _playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

            SetUI(_playerInventory.GetSelectedItem());
        }
        
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
            GenerationDelegates.onInventoryInitialized += OnInventoryInitialized;
            PlayerActionsDelegates.onPlayerSwitchItem += SetBackgrounds;
            PlayerActionsDelegates.onPlayerUseItem += SetText;

            StatisticsTrackingDelegates.onVaseDestroyed += SetText;
        }

        void RemoveDelegates()
        {
            GenerationDelegates.onInventoryInitialized -= OnInventoryInitialized;
            PlayerActionsDelegates.onPlayerSwitchItem -= SetBackgrounds;
            PlayerActionsDelegates.onPlayerUseItem -= SetText;

            StatisticsTrackingDelegates.onVaseDestroyed -= SetText;
        }


        void SetUI(int ItemSelected)
        {
            SetBackgrounds(ItemSelected);

            SetText();
        }

        void SetBackgrounds(int ItemSelected)
        {
            for(int i = 0; i < _backgrounds.Count; i++)
            {
                if(i != ItemSelected - 1)
                {
                    _backgrounds[i].SetActive(false);
                }
                else
                {
                    _backgrounds[i].SetActive(true);
                }
            }
        }

        void SetText()
        {
            //for(int i = 0; i <= 3; i++)
            //{
            //    _texts[i].text = _playerInventory.GetAmount(i) == 0 ? "0" : _playerInventory.GetAmount(i).ToString();
            //}

            StartCoroutine(SetTextOnNextFrame());
        }

        // Game flow is being weird, don't like it
        // Item amount was being decremented *after* the text was updated
        // Sooo I decided to tell it to do this on the next frame rather than immedaitely.
        IEnumerator SetTextOnNextFrame()
        {
            yield return null;
            
            for(int i = 0; i < _backgrounds.Count; i++)
            {
                if(_playerInventory.GetAmount(i) == 0)
                {
                    _texts[i].color = new Color(255, 0, 0, 255);
                }
                else
                {
                    _texts[i].color = new Color(255, 255, 0, 255);
                }

                _texts[i].text = _playerInventory.GetAmount(i) == 0 ? "0" : _playerInventory.GetAmount(i).ToString();
            }
        }
    }
}