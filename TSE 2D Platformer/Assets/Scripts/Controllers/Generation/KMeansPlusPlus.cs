using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.ML;
using System.IO;
using K_Means_Plus_Plus;

using Delegates.Utility;
using Controllers.Utility;
using TMPro;
public class KMeansPlusPlus : MonoBehaviour
{
    int goodClusterID;
    int badClusterID;

    [SerializeField]
    GameObject _statsTracker;
    StatisticsTrackerController _statsTrackerController;
    GameObject _levelGen;
    LevelGeneration _levelGenScript;
    int _currentDifficulty;
    int _newDifficulty;


    PredictionEngine<PlayerData, ClusterPrediction> predictor;
    bool _first = false;



    public GameObject _text;
    public TextMeshProUGUI _textMesh;

    // Start is called before the first frame update
    void Start()
    {
        _levelGen = GameObject.FindGameObjectWithTag("LevelGenerator");
        _levelGenScript = _levelGen.GetComponent<LevelGeneration>();
        _currentDifficulty = _levelGenScript.difficulty;
        //Debug.Log(_levelGenScript.difficulty);
        _statsTrackerController = _statsTracker.GetComponent<StatisticsTrackerController>();
        SetupDelegates();
        _textMesh = _text.GetComponent<TextMeshProUGUI>();
        _textMesh.text = "First Level";

        //Setting up ML algorithm

        //File path to file where trained model is stored
        string _modelPath = Path.Combine(Directory.GetCurrentDirectory(), "Model", "KMeansClusteringModel.zip");

        //Sets up ML Context: allows useful mechanisms like logging and entry points
        var mlContext = new MLContext(seed: 0);

        //For the data preperation pipeline
        DataViewSchema modelSchema;

        //Loads trained model
        var trainedModel = mlContext.Model.Load(_modelPath, out modelSchema);
        Debug.Log(trainedModel);
        //Using the model to make a prediction (Note - not thread safe)
        //The Prediction Enginge class is an API to make running predictions simpler
        predictor = mlContext.Model.CreatePredictionEngine<PlayerData, ClusterPrediction>(trainedModel);
        Debug.Log("TESTING END");
        //Loads cluster context
        string[] goodClusterLine;
        string[] badClusterLine;

        using (var sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Model", "ClusterContext.txt")))
        {
            string goodCluster = sr.ReadLine();
            goodClusterLine = goodCluster.Split(",");

            string badCluster = sr.ReadLine();
            badClusterLine = badCluster.Split(",");
        }
        //Checks expeted data found
        if (badClusterLine[0] != "Bad" | goodClusterLine[0] != "Good")
        {
            Debug.Log("INCORRECT CLUSTER ANALYSIS");
        }

        goodClusterID = int.Parse(goodClusterLine[1]);
        badClusterID = int.Parse(badClusterLine[1]);

        ////Prediction for TestIrisData.cs
        //var prediction = predictor.Predict(TestPlayerData.BadPlayer);
        //Debug.Log($"Cluster: {prediction.PredictedClusterId}");
        //Debug.Log($"Distances: {string.Join(" ", prediction.Distances)}");
        //PrintClusterContext(prediction.PredictedClusterId);

        //var prediction2 = predictor.Predict(TestPlayerData.AveragePlayer);
        //Debug.Log($"Cluster: {prediction2.PredictedClusterId}");
        //Debug.Log($"Distances: {string.Join(" ", prediction2.Distances)}");
        //PrintClusterContext(prediction2.PredictedClusterId);

        //var prediction3 = predictor.Predict(TestPlayerData.GoodPlayer);
        //Debug.Log($"Cluster: {prediction3.PredictedClusterId}");
        //Debug.Log($"Distances: {string.Join(" ", prediction3.Distances)}");
        //PrintClusterContext(prediction3.PredictedClusterId);

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

    void PrintClusterContext(uint clusterID)
    {
        if (clusterID == goodClusterID)
        {
            _textMesh.text = "Level Up";
        }
        else if (clusterID == badClusterID)
        {
            _textMesh.text = "Level Down";
        }
        else
        {
            _textMesh.text = "Same Level";
        }
    }

    //Changes difficulty based on prediction
    void ChangeDifficulty(uint clusterID)
    {
        if (clusterID == goodClusterID)
        {
            if (_currentDifficulty < 10)
            {
                _levelGenScript.difficulty = _currentDifficulty + 1;
                //Update local variables
                _currentDifficulty++;
            }

        }
        else if (clusterID == badClusterID)
        {
            if (_currentDifficulty > 1)
            {
                _levelGenScript.difficulty = _currentDifficulty - 1;
                //Update local variables
                _currentDifficulty--;
            }
            
        }
        //No change on average performance
        


        // Overwrite previous with forced next difficulty set by user
        if(_levelGenScript._forcedNext)
        {
            _levelGenScript.difficulty = _levelGenScript._forcedNextDifficulty;
        }

        if(ZoneDelegates.onDifficultyDecided != null) ZoneDelegates.onDifficultyDecided();
    }

    void OnZoneCompletion()
    {
        //Don't want to make a prediction when first level is made/level is reloaded
        if (_first)
        {
            _first = false;
            _currentDifficulty = _levelGenScript.difficulty;
            return;
        }

        //Make prediction if difficulty hasn't been manually changed
        _newDifficulty = _levelGenScript.difficulty;

        Debug.Log(_newDifficulty);
        Debug.Log(_currentDifficulty);
        if (_newDifficulty != _currentDifficulty)
        {
            _textMesh.text = "Level manually changed";
            _currentDifficulty = _newDifficulty;
            return;
        }
        

        PlayerData newPrediction = _statsTrackerController.GetStatsForPrediction();
        var prediction = predictor.Predict(newPrediction);
        PrintClusterContext(prediction.PredictedClusterId);
        ChangeDifficulty(prediction.PredictedClusterId);
    }

    void OnZoneCompletionRestart()
    {
        //Prevent prediction
        _first = true;
        _textMesh.text = "Level Reset";

        // Overwrite previous with forced next difficulty set by user
        if(_levelGenScript._forcedNext)
        {
            ChangeDifficulty(0);
        }
    }
}
