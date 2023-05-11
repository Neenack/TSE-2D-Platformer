using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.Player.Items;

using Controllers.Utility.Statistics;

using Delegates.Actors.Player;
using Delegates.Utility;



namespace Actors.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        Item[] _inventory = new Item[4] {new BombItem(), new RopeItem(), null, null}; 
        int[] _amounts = new int[4] {5, 5, 0, 0};

        int _selectedItem;

        public void BeginSelf()
        {
            _selectedItem = 1;
            SetupDelegates();


            int difficulty = GameObject.Find("Level Generator").GetComponent<LevelGeneration>().difficulty;

            if(difficulty < 4)
            {
                _amounts[0] = 6;
                _amounts[1] = 6;
            }
            else if(difficulty < 7)
            {
                _amounts[0] = 5;
                _amounts[1] = 5;
            }
            else if(difficulty < 10)
            {
                _amounts[0] = 4;
                _amounts[1] = 4;
            }
            else
            {
                _amounts[0] = 3;
                _amounts[1] = 3;
            }

            if(GenerationDelegates.onInventoryInitialized != null)
            {
                GenerationDelegates.onInventoryInitialized();
            }
        }

        void OnDisable()
        {
            RemoveDelegates();
        }

        void SetupDelegates()
        {
            PlayerActionsDelegates.onPlayerSwitchItem += SwitchActiveItem;
            PlayerActionsDelegates.onPlayerUseItem += UseItem;
            PlayerActionsDelegates.onVaseDestroyed += RecieveNewItem;
        }

        void RemoveDelegates()
        {
            PlayerActionsDelegates.onPlayerSwitchItem -= SwitchActiveItem;
            PlayerActionsDelegates.onPlayerUseItem -= UseItem;
            PlayerActionsDelegates.onVaseDestroyed -= RecieveNewItem;
        }


        public int GetSelectedItem()
        {
            return _selectedItem;
        }

        public int GetAmount(int itemNum)
        {
            return _amounts[itemNum];
        }



        void SwitchActiveItem(int itemNum)
        {
            _selectedItem = itemNum;

            Debug.Log("Switched to " + _selectedItem);
        }

        void UseItem()
        {
            if(_amounts[_selectedItem - 1] > 0)
            {
                _inventory[_selectedItem - 1].UseItem();
                _amounts[_selectedItem - 1]--;

                if(StatisticsTrackingDelegates.onActionTracking != null) StatisticsTrackingDelegates.onActionTracking(ActionType.Item);
            }
        }


        // Unused parameter to not add new delegate
        // Unused parameter deleted, unsure if it was actually needed
        // Nothing seemed to break
        void RecieveNewItem()
        {
            // 2 because max exclusive for some reason I will never comprehend
            _amounts[Random.Range(0, 2)]++;

            // Used to update UI display
            if(StatisticsTrackingDelegates.onVaseDestroyed != null) StatisticsTrackingDelegates.onVaseDestroyed();
        }
    }
}