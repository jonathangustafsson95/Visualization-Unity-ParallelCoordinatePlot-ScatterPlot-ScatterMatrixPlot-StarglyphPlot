using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DictToMatrix : MonoBehaviour
{
    public static double[][] GetMatrix(List<Dictionary<string, object>> dataDictionary)
    {
        object[][] objectMatrix = DictionaryListToMatrix(dataDictionary);
        double[][] doubleMatrix = ObjectMatrixToDoubleMatrix(objectMatrix);
        return doubleMatrix;
    }

    public static object[][] DictionaryListToMatrix(List<Dictionary<string, object>> points)
    {

        int numPoint = points.Count;

        object[][] dataMatrix = new object[numPoint][];

        for (int i = 0; i < numPoint; i++)
        {
            dataMatrix[i] = points[i].Values.ToArray();
        }
        return dataMatrix;
    }

    private static double[][] ObjectMatrixToDoubleMatrix(object[][] matrix)
    {
        double[][] doubleMatrix = new double[matrix.Length][];
        for (int i = 0; i < matrix.Length; i++)
        {
            List<double> doubleList = new List<double>();
            foreach (var columnValue in matrix[i])
            {
                double doubleValue;
                if (double.TryParse(columnValue.ToString().Replace(".", ","), out doubleValue))
                {
                    doubleList.Add(doubleValue);
                }
            }
            doubleMatrix[i] = doubleList.ToArray();
        }
        return doubleMatrix;
    }

    private static double[] GetTargetFromDataSet(double[][] matrix)
    {
        List<double> targets = new List<double>();
        foreach (var row in matrix)
        {
            targets.Add(row[row.Length - 1]);
        }
        return targets.ToArray();
    }
}
