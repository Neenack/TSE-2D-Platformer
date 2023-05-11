using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace K_Means_Clustering
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

