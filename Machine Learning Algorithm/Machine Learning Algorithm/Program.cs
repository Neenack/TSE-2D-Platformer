using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using K_Means_Clustering;

//File path to data file for training the model
string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "FakeData.csv");

//File path to file where trained model is stored
string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "KMeansClusteringModel.zip");

//Sets up ML Context: allows useful mechanisms like logging and entry points
var mlContext = new MLContext(seed: 0);

//Sets up data loading: the transfer of the data from the file to the program
IDataView dataView = mlContext.Data.LoadFromTextFile<PlayerData>(_dataPath, hasHeader: false, separatorChar: ',');

//Creates a learning pipeline. In this context, the pipeline formats the data, and trains the algorithm using it
string featuresColunName = "Features";
var pipeline = mlContext.Transforms.Concatenate(featuresColunName, "EnemiesSpawned", "RoomsExplored",
    "ItemsUsed", "EnemiesKilled", "NearMissesWithEnemies", "NearMissesWithProjectiles", "BombKills", "RopesUsed",
    "Time", "LongestTimeIn1Room", "Jumps", "Attacks", "IdleTime", "EnemiesDetected", "DeathByAngryBob", "DeathByScreamer",
    "DeathByJumper", "DeathByTrap")
    .Append(mlContext.Clustering.Trainers.KMeans(featuresColunName, numberOfClusters: 3)); // Splits into 3 clusters



//Executes the pipeline
var model = pipeline.Fit(dataView);

//Saves the model to a zip file
using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
{
    mlContext.Model.Save(model, dataView.Schema, fileStream);
}

//Code to compare centroids: can use this to give meaning to clusterID
VBuffer<float>[] centroids = default;
//As the pipeline contains multiple transforms, LastTransformer gets the cluster prediction transformer, which
//contains the internal workings of the model
var modelParams = model.LastTransformer.Model;
modelParams.GetClusterCentroids(ref centroids, out var k);

int badCluster = -1;
int goodCluster = -1;

//Create lists to store the values of the features of the centroids
List<float> increasingValues = new List<float>(){0,0,0};
List<float> decreasingValues = new List<float>(){0,0,0};

Console.WriteLine("Centroids values:");
for (int i = 0; i < centroids.Length; i++)
{
    string centroidValues = string.Empty;
    Console.WriteLine();
    Console.WriteLine("Centroid ID: " + i);
    for (int j = 0; j < centroids[i].Length; j++)
    {
        centroidValues += centroids[i].GetValues().ToArray()[j] + ", ";
    }
    Console.WriteLine(centroidValues);
}

for (int i = 0; i < k; i++)
{

    for (int j = 0; j < 8; j++) //Loops through all increasing values
    {
        increasingValues[i] += centroids[i].GetValues().ToArray()[j];
    }
    for (int j = 8; j < centroids[0].GetValues().ToArray().Length; j++)//Loops through all decreasing values
    {
        decreasingValues[i] += centroids[i].GetValues().ToArray()[j];
    }
}

//Inefficient as takes two passes over vector, but not important as time of creating model is irrelevent
int increasingValuesMax = increasingValues.IndexOf(increasingValues.Max());
int increasingValuesMin = increasingValues.IndexOf(increasingValues.Min());
int decreasingValuesMax = decreasingValues.IndexOf(decreasingValues.Max());
int decreasingValuesMin = decreasingValues.IndexOf(decreasingValues.Min());

//Check to see if the minimum and maximum values of increasing and decreasing features match indexes. 
//If they do, there is a confident "good" and "bad" cluster

if (increasingValuesMax == decreasingValuesMin)
{
    Console.WriteLine("Clear good cluster");
    goodCluster = increasingValuesMax + 1; //Index is 1 lower than clusterID
}
else
{
    Console.WriteLine("NO CLEAR GOOD CLUSTER. Decreasing values will be used to determine good " +
        "cluster, as these are the best indications of skill of the 2 categories");
    goodCluster = decreasingValuesMin + 1; //Index is 1 lower than clusterID
}

if (increasingValuesMin == decreasingValuesMax)
{
    Console.WriteLine("Clear bad cluster");
    badCluster = increasingValuesMin + 1;//Index is 1 lower than clusterID
}
else
{
    Console.WriteLine("NO CLEAR BAD CLUSTER. .Decreasing values will be used to determine bad " +
        "cluster, as these are the best indications of skill of the 2 categories");
    badCluster= decreasingValuesMax + 1;
}

// Write the "good" and "bad" clusters to a file: needed for analysis in game
using (StreamWriter outputFile = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "Data", "ClusterContext.txt")))
{   
    outputFile.WriteLine("{0}{1}", "Good,", goodCluster); //Uses commas as delimitters
    outputFile.WriteLine("{0}{1}", "Bad,", badCluster); //Uses commas as delimitters
}


//Code from here on is only for testing purposes - can ignore

Console.WriteLine("{0} {1}", increasingValues[0], decreasingValues[0]);
Console.WriteLine("{0} {1}", increasingValues[1], decreasingValues[1]);
Console.WriteLine("{0} {1}", increasingValues[2], decreasingValues[2]);
;
Console.WriteLine("Bad Cluster: " + badCluster + " Good Cluster: " + goodCluster); //If -1 is printed, cluster variables have not been assigned

//Using the model to make a prediction (Note - not thread safe)
//The Prediction Enginge class is an API to make running predictions simpler
var predictor = mlContext.Model.CreatePredictionEngine<PlayerData, ClusterPrediction>(model);

//Prediction for TestIrisData.cs
var prediction = predictor.Predict(TestPlayerData.BadPlayer);
Console.WriteLine($"Cluster: {prediction.PredictedClusterId}");
Console.WriteLine($"Distances: {string.Join(" ", prediction.Distances)}");
if (prediction.PredictedClusterId == badCluster)
{
    Console.WriteLine("Bad");
}
else if (prediction.PredictedClusterId == goodCluster)
{
    Console.WriteLine("Good");
}

else
{
    Console.WriteLine("Average");
}


var prediction2 = predictor.Predict(TestPlayerData.AveragePlayer);
Console.WriteLine($"Cluster: {prediction2.PredictedClusterId}");
Console.WriteLine($"Distances: {string.Join(" ", prediction2.Distances)}");

if (prediction2.PredictedClusterId == badCluster)
{
    Console.WriteLine("Bad");
}
else if (prediction2.PredictedClusterId == goodCluster)
{
    Console.WriteLine("Good");
}

else
{
    Console.WriteLine("Average");
}

var prediction3 = predictor.Predict(TestPlayerData.GoodPlayer);
Console.WriteLine($"Cluster: {prediction3.PredictedClusterId}");
Console.WriteLine($"Distances: {string.Join(" ", prediction3.Distances)}");

if (prediction3.PredictedClusterId == badCluster)
{
    Console.WriteLine("Bad");
}
else if (prediction3.PredictedClusterId == goodCluster)
{
    Console.WriteLine("Good");
}

else
{
    Console.WriteLine("Average");
}
