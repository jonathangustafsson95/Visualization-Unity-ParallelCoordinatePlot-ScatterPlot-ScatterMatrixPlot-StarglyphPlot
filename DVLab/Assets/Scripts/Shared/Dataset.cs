using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class Dataset
{
    public static Vector3 newPred;
    public static List<Dictionary<string, object>> ListOfPoints { get; set; }
    public static List<string> usedFeatures = new List<string>();
    public static List<Dictionary<string, object>> NewDataPoints { get; set; } = new List<Dictionary<string, object>>();
    public static int K { get; set; }

    public static int NumberOfFeatures { get; set; }
    public static List<string> FeatureNames { get; set; }

    public static List<InputField> InputFields { get; set; } = new List<InputField>();
    public static List<InputField> EditInputFields { get; set; } = new List<InputField>();

    public static bool IsRegression { get; set; }


    public static List<string> GetDataSetFeatures()
    {
        List<string> attributes = new List<string>(ListOfPoints[0].Keys);
        attributes.RemoveAt(0);                         // Remove ID column
        attributes.RemoveAt(attributes.Count - 1);      // Remove target column

        return attributes;
    }

    public static IEnumerable<object> GetDataSetTargets()
    {
        List<string> attributes = new List<string>(ListOfPoints[0].Keys);

        string targetName = attributes[attributes.Count - 1];

        IEnumerable<object> targets = ListOfPoints.Select(d => d[targetName]);

        return targets;
    }

    public static int GetNumberOfTargets(IEnumerable<object> targets)
    {
        int numberOfTargets = targets.GroupBy(t => t).Select(g => g.First()).ToList().Count;
        return numberOfTargets;
    }

    public static Color GetColor(double target)
    {
        //if (target == 0)
        //    return Color.red;
        //else if (target == 1)
        //    return Color.blue;
        //else if (target == 2)
        //    return Color.green;
        //else
        //    return Color.red;

        return SetColor.GetColor(target);
    }

    public static void NewPrediction()
    {

        if (usedFeatures.Count > 0)
        {
            NewDataPoints[0][usedFeatures[0]] = newPred[0];
            NewDataPoints[0][usedFeatures[1]] = newPred[1];
            NewDataPoints[0][usedFeatures[2]] = newPred[2];
        }
        else
        {
            NewDataPoints[0][NewDataPoints[0].Keys.ElementAt(1)] = newPred[0];
            NewDataPoints[0][NewDataPoints[0].Keys.ElementAt(2)] = newPred[1];
            NewDataPoints[0][NewDataPoints[0].Keys.ElementAt(3)] = newPred[2];
        }

        var obj = CreateNewObject(NewDataPoints[0].Keys.Count() - 2, NewDataPoints[0]);
        var classified = Knn.Classify(obj, DictToMatrix.DictionaryListToMatrix(ListOfPoints), GetNumberOfTargets(GetDataSetTargets()), K);
        if (classified is -1)
        {
            return;
        }
        else
        {
            NewDataPoints[0][NewDataPoints[0].Keys.ElementAt(NewDataPoints[0].Keys.Count - 1)] = classified;
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
    public static void InstantiateNewDataPoint()
    {
        List<float> values = new List<float>();

        values.Clear();
        var scene = SceneManager.GetActiveScene();

        foreach (var inputfield in InputFields)
        {
            float.TryParse(inputfield.text, out float result);
            values.Add(result);
        }

        if (NewDataPoints.Count > 0)
            NewDataPoints.Clear();
        Dictionary<string, object> dict = new Dictionary<string, object>();
        int valueToAdd = 0;
        foreach (var key in Dataset.ListOfPoints[0].Keys)
        {
            if (key == ListOfPoints[0].Keys.ElementAt(0))
                dict.Add(ListOfPoints[0].Keys.ElementAt(0), 1);
            else if (key != ListOfPoints[0].Keys.ElementAt(ListOfPoints[0].Keys.Count-1))
            {
                dict.Add(key, values[valueToAdd]);
                valueToAdd++;
            }
        }
        dict.Add(ListOfPoints[0].Keys.ElementAt(ListOfPoints[0].Keys.Count-1), "");
        var obj = CreateNewObject(dict.Keys.Count()-2, dict);
        var classified = Knn.Classify(obj, DictToMatrix.DictionaryListToMatrix(ListOfPoints),GetNumberOfTargets(GetDataSetTargets()),K);
        if (classified is -1)
        {
            return;
        }
        else
        {
            Debug.Log("target " + classified);
            dict[dict.Keys.ElementAt(dict.Keys.Count - 1)] = classified;
            NewDataPoints.Add(dict);
            if (scene.name is "ScatterplotMatrix")
            {
                MatrixPlotter plotMethod = GameObject.Find("Visualizer").GetComponent<MatrixPlotter>();
                plotMethod.RemovePointsAndAxes();
                plotMethod.CreatePoints();
            }
            else
            {
                SceneManager.LoadScene(scene.name);
            }
        }
    }
    public static void ReClassify()
    {
        var obj = CreateNewObject(NewDataPoints[0].Keys.Count() - 2, NewDataPoints[0]);
        var classified = Knn.Classify(obj, DictToMatrix.DictionaryListToMatrix(ListOfPoints), GetNumberOfTargets(GetDataSetTargets()), K);
        NewDataPoints[0][NewDataPoints[0].Keys.ElementAt(NewDataPoints[0].Keys.Count - 1)] = classified;
    }

    private static object[] CreateNewObject(int amount, Dictionary<string, object> dict)
    {
        var obj = new object[amount];
        for (int i = 1; i <= amount; i++)
        {
            obj[i - 1] = dict[dict.Keys.ElementAt(i)];
        }
        return obj;
    }

    public static void ChangeKValue(string k)
    {
        if (Int32.TryParse(k, out int kValue))
        {
            K = kValue;
            GameObject.Find("CurrentK").GetComponent<Text>().text = "K = " + K.ToString();
            Debug.Log("Current k val: " + K);
        }
        else
        {
            Debug.Log("Not a valid k value.");
        }
    }
    public static Dictionary<string, Dictionary<string, float>> GetMinMax(List<string> attributes)
    {
        Dictionary<string, Dictionary<string, float>> minMax = new Dictionary<string, Dictionary<string, float>>();

        for (int i = 0; i < attributes.Count; i++)
        {
            minMax[attributes[i]] = new Dictionary<string, float>()
            {
                { "min", FindMin(attributes[i]) },
                { "max", FindMax(attributes[i]) }
            };
        }

        return minMax;
    }

    public static float FindMax(string attribute)
    {
        float maxVal = Convert.ToSingle(Dataset.ListOfPoints[0][attribute]);

        for (int i = 0; i < Dataset.ListOfPoints.Count; i++)
        {
            maxVal = maxVal > Convert.ToSingle(Dataset.ListOfPoints[i][attribute]) ? maxVal : Convert.ToSingle(Dataset.ListOfPoints[i][attribute]);
        }
        if (Dataset.NewDataPoints.Count > 0)
            maxVal = maxVal > Convert.ToSingle(Dataset.NewDataPoints[0][attribute]) ? maxVal : Convert.ToSingle(Dataset.NewDataPoints[0][attribute]);

        return maxVal;
    }

    public static float FindMin(string attribute)
    {
        float minVal = Convert.ToSingle(Dataset.ListOfPoints[0][attribute]);

        for (int i = 0; i < Dataset.ListOfPoints.Count; i++)
        {
            minVal = minVal < Convert.ToSingle(Dataset.ListOfPoints[i][attribute]) ? minVal : Convert.ToSingle(Dataset.ListOfPoints[i][attribute]);
        }
        if (Dataset.NewDataPoints.Count > 0)
            minVal = minVal < Convert.ToSingle(Dataset.NewDataPoints[0][attribute]) ? minVal : Convert.ToSingle(Dataset.NewDataPoints[0][attribute]);

        return minVal;
    }
}
