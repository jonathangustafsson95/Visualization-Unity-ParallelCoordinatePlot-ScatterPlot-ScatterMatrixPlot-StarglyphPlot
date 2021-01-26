using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Accord;
using System.Xml.XPath;

public enum KnnStates
{
    UniformWeight,
    DistanceWeight
}

public enum KnnAlgorithm
{
    Classification,
    Regression
}

public class IndexAndDistance : IComparable<IndexAndDistance>
{
    public int idx;  // index of a training item
    public double dist;  // distance to unknown
    public int CompareTo(IndexAndDistance other)
    {
        if (this.dist < other.dist) return -1;
        else if (this.dist > other.dist) return +1;
        else return 0;
    }
}

public class Knn : MonoBehaviour
{
    public static KnnStates currentState;
    public static KnnAlgorithm currentAlg;
    public GameObject lineRenderer;
    private List<IndexAndDistance> targetDistance = new List<IndexAndDistance>();
    public static IndexAndDistance[] info;

    public static double Classify(object[] unknown, object[][] trainData, int numClasses, int k)
    {
        int n = trainData.Length;
        info = new IndexAndDistance[n];
        for (int i = 0; i < n; ++i)
        {
            IndexAndDistance curr = new IndexAndDistance();
            double dist = Distance(unknown, trainData[i]);
            curr.idx = i;
            curr.dist = dist;
            info[i] = curr;
        }
        double result = Vote(info, trainData, numClasses, k);
        return result;
    }

    static double Vote(IndexAndDistance[] info, object[][] trainData, int numClasses, int k)
    {
        Array.Sort(info);
        double[] votes = new double[numClasses];
        try
        {
            if (currentAlg is KnnAlgorithm.Regression)
            {
                Debug.Log("Is regression: ");

                if (currentState == KnnStates.UniformWeight)
                {
                    double target = 0;
                    for (int i = 0; i < k; ++i)
                    {
                        int idx = info[i].idx;
                        double.TryParse(trainData[idx].Last().ToString(), out double result);
                        target += result;
                    }
                    return target / k;
                }
                else //Distance weighted
                {
                    double target = 0;
                    double sum = 0;
                    for (int i = 0; i < k; ++i)
                    {
                        int idx = info[i].idx;
                        target += (1 / Math.Pow(info[i].dist,2) * (int)trainData[idx].Last());
                        sum += 1 / Math.Pow(info[i].dist,2);
                    }
                    Debug.Log(sum);
                    Debug.Log(target);
                    return target / sum;
                }
            }

            else
            {
                if (currentState == KnnStates.UniformWeight)
                {
                    for (int i = 0; i < k; ++i)
                    {
                        int idx = info[i].idx;
                        int c = (int)trainData[idx].Last();
                        ++votes[c];                         // +1 för varje förekomst av klass
                    }
                }
                else //Distance weighted
                {
                    for (int i = 0; i < k; ++i)
                    {
                        int idx = info[i].idx;
                        int c = (int)trainData[idx].Last();
                        votes[c] += 1 / info[i].dist;       // Votes depending on distance
                    }
                }
                return (int)ClassifierVote(votes, numClasses);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Can´t use regression on classification dataset.");
            Debug.Log(ex.Message);
            return -1;
        }
    }

    static double Distance(object[] unknown, object[] data)
    {
        double sum = 0.0;
        for (int i = 0; i < unknown.Length; ++i) 
        {

            if (!(unknown[i] is string) && !(data[i] is string) && i != 0)
            {
                sum += (Convert.ToDouble(unknown[i]) - Convert.ToDouble(data[i])) * (Convert.ToDouble(unknown[i]) - Convert.ToDouble(data[i]));
            }
        }               
        return Math.Sqrt(sum);
    }

    public static void ChangeKnnState()
    {
        currentState = currentState == KnnStates.UniformWeight ? KnnStates.DistanceWeight : KnnStates.UniformWeight;
        GameObject.Find("CurrentKnnState").GetComponent<Text>().text = "Weight =  " + currentState.ToString();
    }

    public static void ChangeKnnAlgorithm()
    {
        currentAlg = currentAlg == KnnAlgorithm.Classification ? KnnAlgorithm.Regression : KnnAlgorithm.Classification;
        GameObject.Find("CurrentKnnAlg").GetComponent<Text>().text = "Algorithm =  " + currentAlg.ToString();
    }


    //static object[][] LoadData()
    //{
    //    var dataDictionary = Dataset.ListOfPoints;

    //    object[][] dataMatrix = DictToMatrix.DictionaryListToMatrix(dataDictionary);

    //    int numPoint = dataMatrix.Length;

    //    object[][] dataMatrix = new object[numPoint][];

    //    for (int i = 0; i < numPoint; i++)
    //    {
    //        dataMatrix[i] = points[i].Values.ToArray();
    //    }
    //    return dataMatrix;
    //}

    public static int Classify(double[] unknown, double[][] trainData, int numClasses, int k)
    {     

        int n = trainData.Length;
        info = new IndexAndDistance[n];
        for (int i = 0; i < n; ++i)
        {
            IndexAndDistance curr = new IndexAndDistance();
            double dist = Distance(unknown, trainData[i]);
            curr.idx = i;
            curr.dist = dist;
            info[i] = curr;
        }
        int result = Vote(info, trainData, numClasses, k);
        return result;
    }

    static int Vote(IndexAndDistance[] info, double[][] trainData, int numClasses, int k)
    {
        Array.Sort(info);
        double[] votes = new double[numClasses];
        try
        {
            if (currentAlg is KnnAlgorithm.Regression)
            {
                Debug.Log("Is regression: ");

                if (currentState == KnnStates.UniformWeight)
                {
                    double target = 0;
                    for (int i = 0; i < k; ++i)
                    {
                        int idx = info[i].idx;
                        double.TryParse(trainData[idx].Last().ToString(), out double result);
                        target += result;
                    }
                    return (int)(target / k);
                }
                else //Distance weighted
                {
                    double target = 0;
                    double sum = 0;
                    for (int i = 0; i < k; ++i)
                    {
                        int idx = info[i].idx;
                        target += (1 / Math.Pow(info[i].dist, 2) * (int)trainData[idx].Last());
                        sum += 1 / Math.Pow(info[i].dist, 2);
                    }
                    Debug.Log(sum);
                    Debug.Log(target);
                    return (int)(target / sum);
                }
            }

            else
            {
                if (currentState == KnnStates.UniformWeight)
                {
                    for (int i = 0; i < k; ++i)
                    {
                        int idx = info[i].idx;
                        int c = (int)trainData[idx].Last();
                        ++votes[c];                         // +1 för varje förekomst av klass
                    }
                }
                else //Distance weighted
                {
                    for (int i = 0; i < k; ++i)
                    {
                        int idx = info[i].idx;
                        int c = (int)trainData[idx].Last();
                        votes[c] += 1 / info[i].dist;       // Votes depending on distance
                    }
                }
                return (int)ClassifierVote(votes, numClasses);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Can´t use regression on classification dataset.");
            Debug.Log(ex.Message);
            return -1;
        }
    }

    static double ClassifierVote(double[] votes, int numClasses)
    {
        double mostVotes = 0;
        int classWithMostVotes = 0;
        for (int j = 0; j < numClasses; ++j)
        {
            if (votes[j] > mostVotes)
            {
                mostVotes = votes[j];
                classWithMostVotes = j;
            }
        }

        //Debug.Log("value from Knn: " + classWithMostVotes);
        return classWithMostVotes;
    }

    static double Distance(double[] unknown, double[] data)
    {
        double sum = 0.0;
        for (int i = 0; i < unknown.Length; ++i)
        {

            if (i != 0)
            {
                sum += (Convert.ToDouble(unknown[i]) - Convert.ToDouble(data[i])) * (Convert.ToDouble(unknown[i]) - Convert.ToDouble(data[i]));
            }
        }
        return Math.Sqrt(sum);
    }
}
