using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class ValfriStatic
{
	public static List<GameObject> pies = new List<GameObject>();
	public static List<WedgeCirkle> WedgeObjects = new List<WedgeCirkle>();

    public static double[][] MatrixOfPoints { get; set; }
    public static double[][] NormalizedMatrixOfPoints { get; set; }
    public static double[] Targets { get; set; }
    public static List<List<double>> AddedPoints { get; set; } = new List<List<double>>();

    public static List<string> FeaturesToDisplay { get; set; } = new List<string>();
}
