using Assets.Scripts;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetColor
{
    static GraphColors graphColors = new GraphColors();
    public static Color GetColor(double target)
    {
        //double normalizedValue = Normalize(target);
        return graphColors.Evaluate((float)target);

        //return new Color((float)normalizedValue, 0, 0);
    }

    public static void Init(double[] targets)
    {
        graphColors.SetMinMax();
        SetMinMax(targets);
    }
    public static void Init(IEnumerable<object> targets)
    {
        graphColors.SetMinMax();

        List<double> doubleTargets = new List<double>();
        foreach (var target in targets)
        {
            double doubleTarget;
            if (double.TryParse(target.ToString(), out doubleTarget))
                doubleTargets.Add(doubleTarget);
                
        }

        SetMinMax(doubleTargets.ToArray());
    }
    private static double min;
    private static double max;
    private static double Normalize(double target)
    {
        double normalizedValue;

        normalizedValue = ((target - min) / (max - min));

        return normalizedValue;
    }
    private static void SetMinMax(double[] targets)
    {
        min = targets[0];
        max = targets[0];

        foreach (var target in targets)
        {
            if (target < min)
                min = target;
            if (target > max)
                max = target;
        }
    }
}
