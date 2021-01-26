//using Accord.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string dataFile;                                     //Data to plot
    private List<Dictionary<string, object>> listOfPoints;      // List of points from ReadCSV
    void Start()
    {
        listOfPoints = ReadCSV.Read(dataFile);
        Dataset.ListOfPoints = listOfPoints;
        //FileThnigs();

        Dataset.K = 3;
        Dataset.FeatureNames = Dataset.ListOfPoints[0].Keys.ToList();
        Dataset.FeatureNames.Remove(Dataset.FeatureNames.Last());
        Dataset.FeatureNames.Remove(Dataset.FeatureNames.First());

        //TODO ändra så att det inte hårkodas in 4
        Dataset.NumberOfFeatures = Dataset.ListOfPoints[0].Keys.Count;
        Debug.Log("Number of features: "+Dataset.NumberOfFeatures);

        SetFeatureInDataset();
    }

    private void SetFeatureInDataset()
    {
        List<string> featureNames = new List<string>();
        int numberOfFeatures = 0;


        foreach (var item in Dataset.ListOfPoints[0].Values)
        {
            if (double.TryParse(item.ToString().Replace(".", ","), out double x))
            {
                numberOfFeatures++;
            }
        }
        Dataset.NumberOfFeatures = numberOfFeatures - 2;

    }


    public void Plot() // TODO: Valfri teknik
    {
        // SceneManager.LoadScene("Plot2D");
    }

    public void Valfri()
    {
        try
        {
            InitValfri();
        }
        catch (System.Exception)
        {
            throw new System.Exception("No data");
        }
        SceneManager.LoadScene("Valfri");
    }

    public void ScatterPlot() 
    {
        SetColor.Init(Dataset.GetDataSetTargets());
        SceneManager.LoadScene("Scatterplot3D");
    }

    public void FeatureMatrix() 
    {
        SceneManager.LoadScene("ScatterPlotMatrix");
    }

    public void ParallelCoordinatePlot() 
    {
        SceneManager.LoadScene("ParallelCoordinatePlot");
    }

    public void Quit()
    {
        // Shutdown
    }

    static double[] GetTargetFromDataSet(double[][] matrix)
    {
        List<double> targets = new List<double>();
        foreach (var row in matrix)
        {
            targets.Add(row[row.Length - 1]);
        }
        return targets.ToArray();
    }

    static double[][] RemoveIndexAndTarget(double[][] matrix)
    {
        double[][] output = new double[matrix.Length][];
        double[] newPoint;
        for (int i = 0; i < matrix.Length; i++)
        {
            newPoint = new double[matrix[0].Length - 2];

            for (int j = 0; j < newPoint.Length; j++)
            {
                newPoint[j] = matrix[i][j + 1];
            }
            output[i] = newPoint;
        }
        return output;
    }

    static void InitValfri()
    {


        ValfriStatic.MatrixOfPoints = DictToMatrix.GetMatrix(Dataset.ListOfPoints);
        ValfriStatic.Targets = GetTargetFromDataSet(ValfriStatic.MatrixOfPoints);
        SetColor.Init(ValfriStatic.Targets);
        ValfriStatic.MatrixOfPoints = RemoveIndexAndTarget(ValfriStatic.MatrixOfPoints);

        Normalize.Initiate(ValfriStatic.MatrixOfPoints);
        ValfriStatic.NormalizedMatrixOfPoints = Normalize.Matrix(ValfriStatic.MatrixOfPoints);

        ValfriStatic.FeaturesToDisplay = Dataset.FeatureNames;
    }

    private static void FileThnigs()
    {
        var lines = File.ReadAllLines(@"C:\Users\blocket\Desktop\pri.txt").ToList();
        Debug.Log(lines[1]);

        var newLines = new List<string>();
        foreach (var line in lines)
        {
            var values = line.Split(',').ToList();
            values.Add(values[2]);
            values.RemoveAt(2);
            values.RemoveAt(1);

            string newLine = "";
            foreach (var item in values)
            {
                
                string newValue = item.Replace('"', '*');                

                newLine += $"{newValue},";
            }

            newLine = newLine.Remove(newLine.Length - 1, 1);
            //newLine = newLine.Trim('*');
            newLine = newLine.Replace("*", string.Empty);
            Debug.Log(newLine);
            //newLine = newLine.Replace('*', string.Empty);
            if (newLine.Contains("e"))
            {
                if (newLines.Count != 0)
                    continue;
            }

            newLines.Add(newLine);
        }
        File.WriteAllLines(@"C:\Users\blocket\Desktop\out.txt", newLines);
    }
}
