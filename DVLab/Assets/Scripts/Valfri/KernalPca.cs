using Accord.Statistics.Analysis;
using Accord.Statistics.Kernels;
using Accord.Statistics.Models.Regression;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KernalPca : MonoBehaviour
{
    static MultivariateKernelRegression model;
    public static void Train(double[][] dataMatrix)
    {
        Gaussian kernel = new Gaussian(10.0);
        var kpca = new KernelPrincipalComponentAnalysis(kernel);
        model = kpca.Learn(dataMatrix);
    }

    public static double[][] Transform(double[][] input)
    {
        double[][] output = model.Transform(input);
        return output;
    }

    public static double[] Transform(double[] input)
    {
        double[] output = model.Transform(input);
        return output;
    }
}
