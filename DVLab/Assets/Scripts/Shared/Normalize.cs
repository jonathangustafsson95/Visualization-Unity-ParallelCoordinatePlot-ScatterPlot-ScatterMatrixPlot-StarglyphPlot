using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normalize : MonoBehaviour
{
    private static double[] min;
    private static double[] max;

    public static void Initiate(double[][] NormalMatrix)
    {
        (min, max) = GetMinMaxArrays(NormalMatrix);
    }
    public static double[][] Matrix(double[][] NormalMatrix)
    {
        //int numberOfColumns = matrix[0].Length;

        //double[] min = new double[numberOfColumns];
        //double[] max = new double[numberOfColumns];

        //for (int i = 0; i < numberOfColumns; i++)
        //{
        //    (min[i], max[i]) = GetMinMax(matrix, i);
        //}

        //double[] min;
        //double[] max;
        //(min, max) = GetMinMaxArrays(NormalMatrix);


        double[][] normalizedMatrix = new double[NormalMatrix.Length][];

        for (int i = 0; i < NormalMatrix.Length; i++)
        {
            normalizedMatrix[i] = Point(NormalMatrix[i]);
        }

        return normalizedMatrix;
    }
    public static double[] Point(double[] point)
    {
        double[] normalizedPoint = new double[point.Length];


        for (int i = 0; i < point.Length; i++)
        {
            normalizedPoint[i] = ((point[i] - min[i]) / (max[i] - min[i]));
        }
        return normalizedPoint;
    }


    private static (double[], double[]) GetMinMaxArrays(double[][] matrix)
    {
        int numberOfColumns = matrix[0].Length;

        double[] min = new double[numberOfColumns];
        double[] max = new double[numberOfColumns];

        for (int i = 0; i < numberOfColumns; i++)
        {
            (min[i], max[i]) = GetMinMax(matrix, i);
        }

        return (min, max);
    }
    private static (double min, double max) GetMinMax(double[][] matrix, int column)
    {
        double min = matrix[0][column];
        double max = matrix[0][column];
        foreach (var row in matrix)
        {
            if (row[column] < min)
                min = row[column];
            if (row[column] > max)
                max = row[column];
        }
        return (min, max);
    }
}
