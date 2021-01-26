using Accord.Math;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class InitiateValfri : MonoBehaviour
{
    // used to set position of a point
    double[][] kpcaMatrix;
    // used to create point
    double[][] normalizedMatrix;
    double[][] matrixOfPoints;
    double[] targets;
    int numberOfPoints;

    //Pie pieInitiatorScript;
    PieController pieInitiatorScript;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        ClearCanvas();
       

        matrixOfPoints = CombinedOldAndNewData();
        KernalPca.Train(matrixOfPoints);
        kpcaMatrix = KernalPca.Transform(matrixOfPoints);

        Normalize.Initiate(matrixOfPoints);
        normalizedMatrix = Normalize.Matrix(matrixOfPoints);
        //targets = CombinedOldAndNewTargets();
        numberOfPoints = matrixOfPoints.Length;
        GameObject pieInitiator = GameObject.Find("DataInitiator");
        pieInitiatorScript = (PieController)pieInitiator.GetComponent(typeof(PieController));
        
        var oldPies = CreatePies(ValfriStatic.MatrixOfPoints, ValfriStatic.Targets);
        var addedPies = new List<GameObject>();
        try
        {
            addedPies = CreatePies(ValfriStatic.AddedPoints.GetRange(0, ValfriStatic.AddedPoints.Count - 1));
            addedPies.AddRange(CreatePies(ValfriStatic.AddedPoints.GetRange(ValfriStatic.AddedPoints.Count - 1, 1), true));
        }
        catch (System.Exception)
        {
        }


        oldPies.AddRange(addedPies);

        DipslayPies(oldPies);


        var knnPoints = new List<GameObject>();
        try
        {
            for (int i = 0; i < Dataset.K; i++)
            {
                knnPoints.Add(oldPies[Knn.info[i].idx]);
            }
        }
        catch (System.Exception)
        {
        }

        foreach (var pie in knnPoints)
        {
            var pieScript = (BigPie)pie.GetComponent(typeof(BigPie));
            pieScript.CreateBigPie();
        }

    }

    double[][] CombinedOldAndNewData()
    {
        double[][] loadedData = ValfriStatic.MatrixOfPoints;

        if(ValfriStatic.AddedPoints == null || loadedData == null)
        {
            Debug.Log("returrrn");
            return loadedData;
        }

        double[][] addedData = ListToMatrix(ValfriStatic.AddedPoints);
           

        double[][] combinedData = new double[loadedData.Length + addedData.Length][];

        int i = 0;
        foreach (var point in loadedData)
        {
            combinedData[i++] = point;
        }
        foreach (var point in addedData)
        {
            combinedData[i++] = point;
        }
        return combinedData;
    }

    double[] CombinedOldAndNewTargets()
    {
        double[] loadedTargets = ValfriStatic.Targets;

        if (ValfriStatic.AddedPoints == null)
        {
            return loadedTargets;
        }

        double[][] addedTargets = ListToMatrix(ValfriStatic.AddedPoints);

        double[] combinedData = new double[loadedTargets.Length + addedTargets.Length];

        int i = 0;
        foreach (var point in loadedTargets)
        {
            combinedData[i++] = point;
        }
        foreach (var point in addedTargets)
        {
            combinedData[i++] = Knn.Classify(point, ValfriStatic.MatrixOfPoints, Dataset.NumberOfFeatures, Dataset.K);
        }
        return combinedData;
    }

    List<GameObject> CreatePies(List<List<double>> matrix, bool mark = false)
    {
        List<GameObject> pies = new List<GameObject>();
        for (int i = 0; i < matrix.Count; i++)
        {
            GameObject pie = CreatePie(matrix[i].ToArray(), Knn.Classify(matrix[i].ToArray(), ValfriStatic.MatrixOfPoints, Dataset.NumberOfFeatures, Dataset.K), mark);
            pies.Add(pie);
        }
        return pies;
    }

    List<GameObject> CreatePies(double[][] matrix, double[] targets)
    {
        List<GameObject> pies = new List<GameObject>();
        for (int i = 0; i < matrix.Length; i++)
        {
            GameObject pie = CreatePie(matrix[i], (int)targets[i]);
            pies.Add(pie);
        }
        return pies;
    }
    GameObject CreatePie(double[] point, int target, bool added = false)
    {
        GameObject pie = pieInitiatorScript.CreatePie(point, target, added);
        return pie;
    }

    void DipslayPies(List<GameObject> pies)
    {
        var canvas = GameObject.Find("Canvas");
        float x, y, z;
        DisplayHelper.Reset();
        for (int i = 0; i < numberOfPoints; i++)
        {
            x = (float)kpcaMatrix[i][0];
            y = (float)kpcaMatrix[i][1];
            z = 0f;

            Display.Pie(pies[i], canvas);
            Display.SetPosition(pies[i], x, y, z);

        }
        Display.DisplayAll();
    }

    double[][] ListToMatrix(List<List<double>> listMatrix)
    {
        double[][] output = new double[listMatrix.Count][];
        for (int i = 0; i < listMatrix.Count; i++)
        {
            output[i] = listMatrix[i].ToArray();
        }
        return output;
    }
    
    void ClearCanvas()
    {
        foreach (var pie in ValfriStatic.pies)
        {
            GameObject.Destroy(pie);
        }
        ValfriStatic.pies = new List<GameObject>();
    }
}
