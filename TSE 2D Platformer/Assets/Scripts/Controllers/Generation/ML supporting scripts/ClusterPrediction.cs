using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.ML.Data;

namespace K_Means_Plus_Plus
{
    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")] //ID of predicted cluster
        public uint PredictedClusterId;

        [ColumnName("Score")] //Array of Euclidean distances to each
                              //cluster centorid: length = no# centroids
        public float[] Distances;
    }

}
