using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Controllers.Utility.Statistics;

using Delegates.Utility;
using K_Means_Plus_Plus;

namespace Controllers.Utility
{
    [RequireComponent (typeof(TimeStatisticsTrackerController))]
    [RequireComponent (typeof(RoomStatisticsTrackerController))]
    [RequireComponent (typeof(ActionsTrackerController))]
    [RequireComponent (typeof(CombatTrackerController))]
    [RequireComponent (typeof(ItemsTrackerController))]
    [RequireComponent (typeof(PlayerStateTrackerController))]
    public class StatisticsTrackerController : MonoBehaviour
    {
        bool first;

        [SerializeField]
        EnemySpawnerTrackerController _enemySpawnController;

        [SerializeField]
        TimeStatisticsTrackerController _timeStatisticsController;
        
        [SerializeField]
        RoomStatisticsTrackerController _roomStatisticsController;
        
        [SerializeField]
        ActionsTrackerController _actionsStatisticsController;
        
        [SerializeField]
        CombatTrackerController _combatStatisticsController;
        
        [SerializeField]
        ItemsTrackerController _itemsStatisticsController;

        [SerializeField]
        PlayerStateTrackerController _playerStateTrackerController;

        //If true, write stats to file
        [SerializeField]
        bool _trainingMode;
        //Needed for writing stats
        StreamWriter _writer;
        string _fileName = Directory.GetCurrentDirectory() + "/training_data.csv";
        GameObject _levelGen;
        LevelGeneration _levelGenScript;
        int _difficulty;
        int _newDifficulty;
        bool _storeStatsOnClosing = false;
        List<float> _dataPoint = new List<float>();
        string _dataPointString = string.Empty;


        void Awake()
        {
            first = true;
        }
        
        public void BeginSelf()
        {
            _enemySpawnController.BeginSelf();
            _timeStatisticsController.BeginSelf();
            _roomStatisticsController.BeginSelf();
            _actionsStatisticsController.BeginSelf();
            _combatStatisticsController.BeginSelf();
            _itemsStatisticsController.BeginSelf();
            _playerStateTrackerController.BeginSelf();

            SetupDelegates();

            //if in training mode, open file
            if (_trainingMode == true)
            {
                _writer = new StreamWriter( _fileName, true);
                _levelGen = GameObject.FindGameObjectWithTag("LevelGenerator");
                _levelGenScript = _levelGen.GetComponent<LevelGeneration>();
                _difficulty = _levelGenScript.difficulty;             
            }
        }

        //Needed for closing file
        void OnApplicationQuit()
        {
            if (_writer != null)
            {
                //Writes final stats (if level completed without difficulty change - only complete levels are overwritten)
                if (_storeStatsOnClosing == true)
                {
                    WriteDataPoint(false, true);
                }
                
                //Close file
                _writer.Close();
            }
        }

        void OnDisable()
        {
            RemoveDelegates();
        }


        void SetupDelegates()
        {
            ZoneDelegates.onZoneCompletion += OnZoneCompletion;
            ZoneDelegates.onZoneCompletionRestart += OnZoneCompletionRestart;
        }

        void RemoveDelegates()
        {
            ZoneDelegates.onZoneCompletion -= OnZoneCompletion;
            ZoneDelegates.onZoneCompletionRestart -= OnZoneCompletionRestart;
        }


        void OnZoneCompletion()
        {
            if(first)
            {
                first = false;
                return;
            }
            _enemySpawnController.OnZoneCompletion();
            _timeStatisticsController.OnZoneCompletion();
            _roomStatisticsController.OnZoneCompletion();
            _actionsStatisticsController.OnZoneCompletion();
            _combatStatisticsController.OnZoneCompletion();
            _itemsStatisticsController.OnZoneCompletion();
            _playerStateTrackerController.OnZoneCompletion();

            //If training mode is turned off, use machine learning algorithm

            //If difficulty changes, track previous results and write them
            if (_trainingMode == false)
            {
                return;
            }
            _newDifficulty = _levelGenScript.difficulty;
            if (_difficulty != _newDifficulty)
            {
                WriteDataPoint(true, true);
                _difficulty = _newDifficulty;
                _storeStatsOnClosing = false;

            }

            //If a level has been completed regardelss of difficulty change, stats should be stored when the game closes/restarts
            else
            {
                _storeStatsOnClosing = true;
                WriteDataPoint(true, false);
            }
        }

        void OnZoneCompletionRestart()
        {
            //Some functionality needed - easiest just to call and then reset over
            _enemySpawnController.OnZoneCompletion();
            _timeStatisticsController.OnZoneCompletion();
            _roomStatisticsController.OnZoneCompletion();
            _actionsStatisticsController.OnZoneCompletion();
            _combatStatisticsController.OnZoneCompletion();
            _itemsStatisticsController.OnZoneCompletion();
            _playerStateTrackerController.OnZoneCompletion();

            //Reset stats: allows player to reset their run if they want the predictions to be reset
            _enemySpawnController.ClearStats();
            _timeStatisticsController.ClearStats();
            _roomStatisticsController.ClearStats();
            _actionsStatisticsController.ClearStats();
            _combatStatisticsController.ClearStats();
            _itemsStatisticsController.ClearStats();
            _playerStateTrackerController.ClearStats();

            //If level has restarted, don't want OnZoneCompletion() to also be called
            first = true;

            //Write data for any completed levels
            if (_dataPointString != string.Empty)
            {
                WriteDataPoint(false, true);
                _storeStatsOnClosing = false;
            }
        }

        void WriteDataPoint(bool collectData, bool write)
        {
            

            //Add features to data point
            if (collectData == true)
            {
                //If data is being collected, rewrite existing data
                //Reset datapoint vars
                _dataPoint.Clear();
                _dataPointString = string.Empty;
                
                _dataPoint.Add(_enemySpawnController.GetEnemiesSpawnedAverage());
                _dataPoint.Add(_enemySpawnController.GetTrapsSpawnedAverage());
                _dataPoint.Add(_timeStatisticsController.GetZoneTimeAverage());
                _dataPoint.Add(_roomStatisticsController.GetRoomsExploredAverage());
                _dataPoint.Add(_roomStatisticsController.GetLongestRoomTimeAverage());
                _dataPoint.Add(_actionsStatisticsController.GetActionsPerformedAverage(ActionType.Jump));
                _dataPoint.Add(_actionsStatisticsController.GetActionsPerformedAverage(ActionType.Item));
                _dataPoint.Add(_actionsStatisticsController.GetActionsPerformedAverage(ActionType.Attack));
                _dataPoint.Add(_combatStatisticsController.GetEnemiesKilledAverage());
                _dataPoint.Add(_combatStatisticsController.GetNearMissesWithEnemyAverage());
                _dataPoint.Add(_combatStatisticsController.GetNearMissesWithProjectileAverage());
                _dataPoint.Add(_combatStatisticsController.GetBombKillsAverage());
                _dataPoint.Add(_itemsStatisticsController.GetRopesUsedAverage());
                _dataPoint.Add(_playerStateTrackerController.GetIdleTimeAverage());
                _dataPoint.Add(_playerStateTrackerController.GetEnemiesDetectedAverage());
                _dataPoint.Add(_playerStateTrackerController.GetDeathToAngryBobAverage());
                _dataPoint.Add(_playerStateTrackerController.GetDeathToScreamerAverage());
                _dataPoint.Add(_playerStateTrackerController.GetDeathToJumperAverage());
                _dataPoint.Add(_playerStateTrackerController.GetDeathToTrap());

                //Turn into string with commas to write to csv
                foreach (float feature in _dataPoint)
                {
                    _dataPointString += feature + ",";
                }
                //Remove final comma
                _dataPointString = _dataPointString.Substring(0, _dataPointString.Length - 1);

            }
            

            if (write == true)
            {
                Debug.Log("Writing: " + _dataPointString);
                _writer.WriteLine(_dataPointString);

                //Reset stats: want fresh stats for each difficulty when collecting training data
                _enemySpawnController.ClearStats();
                _timeStatisticsController.ClearStats();
                _roomStatisticsController.ClearStats();
                _actionsStatisticsController.ClearStats();
                _combatStatisticsController.ClearStats();
                _itemsStatisticsController.ClearStats();
                _playerStateTrackerController.ClearStats();

                //Reset datapoint vars
                _dataPoint.Clear();
                _dataPointString = string.Empty;

            }
           
        }

        //Needed by the ML algorithm
        //Traps spawned isn't used due to data still being innacurate
        public PlayerData GetStatsForPrediction()
        {

            PlayerData newPrediction = new PlayerData()
            {
                //Increase with skill
                EnemiesSpawned = _enemySpawnController.GetEnemiesSpawnedAverage(),
                RoomsExplored = _roomStatisticsController.GetRoomsExploredAverage(),
                ItemsUsed = _actionsStatisticsController.GetActionsPerformedAverage(ActionType.Item),
                EnemiesKilled = _combatStatisticsController.GetEnemiesKilledAverage(),
                NearMissesWithEnemies = _combatStatisticsController.GetNearMissesWithEnemyAverage(),
                NearMissesWithProjectiles = _combatStatisticsController.GetNearMissesWithProjectileAverage(),
                BombKills = _combatStatisticsController.GetBombKillsAverage(),
                RopesUsed = _itemsStatisticsController.GetRopesUsedAverage(),



                //Decrease with skill
                Time = _timeStatisticsController.GetZoneTimeAverage(),
                LongestTimeIn1Room = _roomStatisticsController.GetLongestRoomTimeAverage(),
                Jumps = _actionsStatisticsController.GetActionsPerformedAverage(ActionType.Jump),
                Attacks = _actionsStatisticsController.GetActionsPerformedAverage(ActionType.Attack),
                IdleTime = _playerStateTrackerController.GetIdleTimeAverage(),
                EnemiesDetected = _playerStateTrackerController.GetEnemiesDetectedAverage(),
                DeathByAngryBob = _playerStateTrackerController.GetDeathToAngryBobAverage(),
                DeathByScreamer = _playerStateTrackerController.GetDeathToScreamerAverage(),
                DeathByJumper = _playerStateTrackerController.GetDeathToJumperAverage(),
                DeathByTrap = _playerStateTrackerController.GetDeathToTrap(),
            };
            return newPrediction;

        }
         

    }
}