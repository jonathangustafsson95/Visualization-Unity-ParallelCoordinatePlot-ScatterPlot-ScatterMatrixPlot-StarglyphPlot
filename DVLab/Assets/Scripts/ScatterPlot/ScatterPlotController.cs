using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Accord;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScatterPlotController : MonoBehaviour
{
    public List<string> Attributes { get; set; } = new List<string>(Dataset.ListOfPoints[1].Keys);
    public GameObject DataPoint;
    public GameObject NewDataPoint;
    public GameObject CurrentlySelectedDataPoint;
    private GraphColors graphColors;
    public GameObject NewPoint;
    public IEnumerable<object> targets;
    public int numberOfTargets;
    float x = 0;
    float y = 0;
    float z = 0;
    public List<GameObject> allPoints;
    float dataPointX;
    float dataPointY;
    float dataPointZ;
    public TextMesh xV;
    public TextMesh yV;
    public TextMesh zV;
    public List<KeyValuePair<int, GameObject>> knnPoints;


    void Start()
    {
        xV = GameObject.Find("XV").GetComponent<TextMesh>();
        yV = GameObject.Find("YV").GetComponent<TextMesh>();
        zV = GameObject.Find("ZV").GetComponent<TextMesh>();
        dataPointX = 0;
        dataPointY = 0;
        dataPointZ = 0;
        allPoints = new List<GameObject>();
        knnPoints = new List<KeyValuePair<int, GameObject>>();
        allPoints.Clear();
        knnPoints.Clear();
        targets = Dataset.GetDataSetTargets();
        numberOfTargets = Dataset.GetNumberOfTargets(targets);
        graphColors = new GraphColors();
        graphColors.SetMinMax();
        AdjustBlockSize();
        if (UseDefault())
        {
            SetDefaultFeatureRepresentation();
            PlotDataDefaultData();
        }
        else
        {
            SetChangedFeatureRepresentation();
            PlotDataChangedFeatureRepresentation();
        }
        if (Dataset.NewDataPoints.Count > 0)
        {
            AdjustBlockSizeNewDP();
            List<string> NewDataPointAttributes = new List<string>(Dataset.NewDataPoints[0].Keys);
            PlotNewDataPoint(NewDataPointAttributes);
        }
    }

    private void AdjustBlockSizeNewDP()
    {
        var xBlock = GameObject.Find("X-axis").GetComponent<Rigidbody>();
        var yBlock = GameObject.Find("Y-axis").GetComponent<Rigidbody>();
        var zBlock = GameObject.Find("Z-axis").GetComponent<Rigidbody>();
        if (Dataset.usedFeatures.Count == 0)
        {
            foreach (var dict in Dataset.NewDataPoints)
            {
                if (x < Convert.ToSingle(dict[Dataset.NewDataPoints[0].Keys.ElementAt(1)]))
                {
                    x = Convert.ToSingle(dict[Dataset.NewDataPoints[0].Keys.ElementAt(1)]);
                }
                if (y < Convert.ToSingle(dict[Dataset.NewDataPoints[0].Keys.ElementAt(2)]))
                {
                    y = Convert.ToSingle(dict[Dataset.NewDataPoints[0].Keys.ElementAt(2)]);
                }
                if (z < Convert.ToSingle(dict[Dataset.NewDataPoints[0].Keys.ElementAt(3)]))
                {
                    z = Convert.ToSingle(dict[Dataset.NewDataPoints[0].Keys.ElementAt(3)]);
                }
            }
        }
        else
        {
            foreach (var dict in Dataset.NewDataPoints)
            {
                if (x < Convert.ToSingle(dict[Dataset.usedFeatures[0]]))
                {
                    x = Convert.ToSingle(dict[Dataset.usedFeatures[0]]);
                }
                if (y < Convert.ToSingle(dict[Dataset.usedFeatures[1]]))
                {
                    y = Convert.ToSingle(dict[Dataset.usedFeatures[1]]);
                }
                if (z < Convert.ToSingle(dict[Dataset.usedFeatures[2]]))
                {
                    z = Convert.ToSingle(dict[Dataset.usedFeatures[2]]);
                }
            }
        }
        xBlock.transform.localScale = new Vector3(x, 1, z);
        yBlock.transform.localScale = new Vector3(1, y, z);
        zBlock.transform.localScale = new Vector3(x, y, 1);
        xBlock.transform.position = new Vector3(x / 2, xBlock.transform.position.y, z / 2);
        yBlock.transform.position = new Vector3(yBlock.transform.position.x, y / 2, z / 2);
        zBlock.transform.position = new Vector3(x / 2, y / 2, z + Convert.ToSingle(0.5));
    }

    private void AdjustBlockSize()
    {
        var xBlock = GameObject.Find("X-axis").GetComponent<Rigidbody>();
        var yBlock = GameObject.Find("Y-axis").GetComponent<Rigidbody>();
        var zBlock = GameObject.Find("Z-axis").GetComponent<Rigidbody>();
        if (Dataset.usedFeatures.Count == 0)
        {
            foreach (var dict in Dataset.ListOfPoints)
            {
                if (x < Convert.ToSingle(dict[Dataset.ListOfPoints[0].Keys.ElementAt(1)]))
                {
                    x = Convert.ToSingle(dict[Dataset.ListOfPoints[0].Keys.ElementAt(1)]);
                }
                if (y < Convert.ToSingle(dict[Dataset.ListOfPoints[0].Keys.ElementAt(2)]))
                {
                    y = Convert.ToSingle(dict[Dataset.ListOfPoints[0].Keys.ElementAt(2)]);
                }
                if (z < Convert.ToSingle(dict[Dataset.ListOfPoints[0].Keys.ElementAt(3)]))
                {
                    z = Convert.ToSingle(dict[Dataset.ListOfPoints[0].Keys.ElementAt(3)]);
                }
            }
        }
        else
        {
            foreach (var dict in Dataset.ListOfPoints)
            {
                if (x < Convert.ToSingle(dict[Dataset.usedFeatures[0]]))
                {
                    x = Convert.ToSingle(dict[Dataset.usedFeatures[0]]);
                }
                if (y < Convert.ToSingle(dict[Dataset.usedFeatures[1]]))
                {
                    y = Convert.ToSingle(dict[Dataset.usedFeatures[1]]);
                }
                if (z < Convert.ToSingle(dict[Dataset.usedFeatures[2]]))
                {
                    z = Convert.ToSingle(dict[Dataset.usedFeatures[2]]);
                }
            }
        }
        xBlock.transform.localScale = new Vector3(x, 1, z);
        yBlock.transform.localScale = new Vector3(1, y, z);
        zBlock.transform.localScale = new Vector3(x, y, 1);
        xBlock.transform.position = new Vector3(x / 2, xBlock.transform.position.y, z / 2);
        yBlock.transform.position = new Vector3(yBlock.transform.position.x, y / 2, z / 2);
        zBlock.transform.position = new Vector3(x / 2, y / 2, z + Convert.ToSingle(0.5));
    }

    private bool UseDefault()
    {
        if (Dataset.usedFeatures.Count < 1)
            return true;
        return false;
    }
    private void PlotDataChangedFeatureRepresentation()
    {
        string xName = Dataset.usedFeatures[0];
        string yName = Dataset.usedFeatures[1];
        string zName = Dataset.usedFeatures[2];
        InstantiateDataPoint(xName, yName, zName);
    }
    private void InstantiateDataPoint(string xName, string yName, string zName)
    {
        for (var i = 0; i < Dataset.ListOfPoints.Count; i++)
        {
            float x = Convert.ToSingle(Dataset.ListOfPoints[i][xName]);
            float y = Convert.ToSingle(Dataset.ListOfPoints[i][yName]);
            float z = Convert.ToSingle(Dataset.ListOfPoints[i][zName]);

            GameObject point = Instantiate(DataPoint, new Vector3(x, y, z), Quaternion.identity);
            point.GetComponent<Renderer>().material.color = graphColors.Evaluate(Convert.ToSingle(targets.ElementAt(i)));
            //point.GetComponent<Renderer>().material.color = SetColor.GetColor(Convert.ToSingle(targets.ElementAt(i)));
            allPoints.Add(point);
        }
    }
    private void SetChangedFeatureRepresentation()
    {
        GameObject.Find("X-axis-text").GetComponent<Text>().text = "X-axis: " + Dataset.usedFeatures[0];
        GameObject.Find("Y-axis-text").GetComponent<Text>().text = "Y-axis: " + Dataset.usedFeatures[1];
        GameObject.Find("Z-axis-text").GetComponent<Text>().text = "Z-axis: " + Dataset.usedFeatures[2];
    }
    private void PlotDataDefaultData()
    {
        string xName = Attributes[1];
        string yName = Attributes[2];
        string zName = Attributes[3];
        InstantiateDataPoint(xName, yName, zName);
    }
    private void SetDefaultFeatureRepresentation()
    {
        if (Attributes.Count >= 3)
        {
            GameObject.Find("X-axis-text").GetComponent<Text>().text = "X-axis: " + Attributes[1];
            GameObject.Find("Y-axis-text").GetComponent<Text>().text = "Y-axis: " + Attributes[2];
            GameObject.Find("Z-axis-text").GetComponent<Text>().text = "Z-axis: " + Attributes[3];
        }
    }
    private void PlotNewDataPoint(List<string> newDataPointAttributes)
    {
        if (Dataset.usedFeatures.Count == 0)
        {
            Dataset.usedFeatures.Add(Dataset.ListOfPoints[0].Keys.ElementAt(1));
            Dataset.usedFeatures.Add(Dataset.ListOfPoints[0].Keys.ElementAt(2));
            Dataset.usedFeatures.Add(Dataset.ListOfPoints[0].Keys.ElementAt(3));
        }
        string xName = Dataset.usedFeatures[0];
        string yName = Dataset.usedFeatures[1];
        string zName = Dataset.usedFeatures[2];
        InstantiateNewDataPoint(xName, yName, zName);
                for (int i = 0; i < Dataset.K; i++)
        {
            knnPoints.Add(new KeyValuePair<int, GameObject>(Knn.info[i].idx, allPoints[Knn.info[i].idx]));
        }
    }
    private void InstantiateNewDataPoint(string xName, string yName, string zName)
    {
        IEnumerable<object> targets = Dataset.GetDataSetTargets();
        int numberOfTargets = Dataset.GetNumberOfTargets(targets);
        for (var i = 0; i < Dataset.NewDataPoints.Count; i++)
        {
            float x = Convert.ToSingle(Dataset.NewDataPoints[i][xName]);
            float y = Convert.ToSingle(Dataset.NewDataPoints[i][yName]);
            float z = Convert.ToSingle(Dataset.NewDataPoints[i][zName]);

            NewPoint = Instantiate(NewPoint, new Vector3(x, y, z), Quaternion.identity);
            NewPoint.GetComponent<Renderer>().material.color = graphColors.Evaluate((Convert.ToSingle(Dataset.NewDataPoints[0]
                [Dataset.NewDataPoints[0].Keys.ElementAt(Dataset.NewDataPoints[0].Keys.Count - 1)])));
            InvokeRepeating("spheres_blink", 1.0f, 1.0f);

        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform)
                {
                    if (CurrentlySelectedDataPoint)
                    {
                        CurrentlySelectedDataPoint.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                    }
                    hit.transform.gameObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                    CurrentlySelectedDataPoint = hit.transform.gameObject;
                }
            }
        }

        if (Input.GetKeyDown("i"))
        {
            RaycastHit hitInfo;
            Ray rayInfo = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayInfo, out hitInfo, 100.0f))
            {
                if (hitInfo.transform)
                {
                    if (CurrentlySelectedDataPoint)
                    {
                        CurrentlySelectedDataPoint.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                    }
                    hitInfo.transform.gameObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                    CurrentlySelectedDataPoint = hitInfo.transform.gameObject;

                    dataPointX = hitInfo.transform.gameObject.transform.position.x;
                    dataPointY = hitInfo.transform.gameObject.transform.position.y;
                    dataPointZ = hitInfo.transform.gameObject.transform.position.z;
                    if (Dataset.usedFeatures.Count > 0)
                    {
                        xV.text = Dataset.usedFeatures[0] + " " + dataPointX.ToString();
                        yV.text = Dataset.usedFeatures[1] + " " + dataPointY.ToString();
                        zV.text = Dataset.usedFeatures[2] + " " + dataPointZ.ToString();
                    }
                    else
                    {
                        xV.text = Dataset.ListOfPoints[0].Keys.ElementAt(1) + " " + dataPointX.ToString();
                        yV.text = Dataset.ListOfPoints[0].Keys.ElementAt(2) + " " + dataPointY.ToString();
                        zV.text = Dataset.ListOfPoints[0].Keys.ElementAt(3) + " " + dataPointZ.ToString();
                    }
                    xV.transform.localPosition = new Vector3(dataPointX, dataPointY + 3, dataPointZ);
                    yV.transform.localPosition = new Vector3(dataPointX, dataPointY + 2, dataPointZ);
                    zV.transform.localPosition = new Vector3(dataPointX, dataPointY + 1, dataPointZ);
                }
            }
        }
    }

    public void spheres_blink()
    {
        foreach (KeyValuePair<int, GameObject> point in knnPoints)
        {
            Color previousColor = point.Value.GetComponent<Renderer>().material.color;
            point.Value.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            if (point.Value == null)
            {
                CancelInvoke();
                break;
            }
            if (point.Value.GetComponent<Renderer>().material.color != Color.white)
                point.Value.GetComponent<Renderer>().material.color = Color.white;
            else
                point.Value.GetComponent<Renderer>().material.color = graphColors.Evaluate(Convert.ToSingle(targets.ElementAt(point.Key)));
        }
    }
}
