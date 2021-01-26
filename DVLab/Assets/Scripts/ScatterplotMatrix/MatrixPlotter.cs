using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;

public class MatrixPlotter : MonoBehaviour
{
    private List<Dictionary<string, object>> listOfPoints;      // List of points from ReadCSV
    private FeaturesCheckBox toggles;
    public GameObject CurrentlySelectedDataPoint;
    public GameObject DataPoint;
    public Text AttributeName;
    public GameObject XattributesAxisHolder;
    public GameObject YattributesAxisHolder;
    public GameObject DataPointHolder;
    private GraphColors graphColors;
    public GameObject NewPoint;
    public IEnumerable<object> targets;
    public int numberOfTargets;
    public List<KeyValuePair<int, GameObject>> knnPoints;
    public List<GameObject> allPoints;

    private float scaleOfMatrix = 10f;

    // Start is called before the first frame update
    void Start()
    {
        knnPoints = new List<KeyValuePair<int, GameObject>>();
        allPoints = new List<GameObject>();
        targets = Dataset.GetDataSetTargets();
        numberOfTargets = Dataset.GetNumberOfTargets(targets);
        graphColors = new GraphColors();
        listOfPoints = Dataset.ListOfPoints;
        toggles = GameObject.Find("MatrixCanvas").GetComponent<FeaturesCheckBox>();
        CreatePoints();
    }

    public void CreatePoints()
    {
        CreateAttributeAxis();
        graphColors.SetMinMax();

        List<float> xPoints = new List<float>(), yPoints = new List<float>();       
        
        for (int i = 0; i < toggles.XToggles.Count; i++)
        {
            xPoints.Clear();
            foreach (var point in listOfPoints)
            {
                xPoints.Add(Convert.ToSingle(point[toggles.XToggles[i]]));              
            }

            if (Dataset.NewDataPoints.Count > 0)
            {
                xPoints.Add(Convert.ToSingle(Dataset.NewDataPoints[0][toggles.XToggles[i]]));
            }

            for (int j = 0; j < toggles.YToggles.Count; j++)
            {
                yPoints.Clear();
                foreach (var point in listOfPoints)
                {
                    yPoints.Add(Convert.ToSingle(point[toggles.YToggles[j]]));
                }

                if (Dataset.NewDataPoints.Count > 0)
                {
                    yPoints.Add(Convert.ToSingle(Dataset.NewDataPoints[0][toggles.YToggles[j]]));
                }
                allPoints.Clear();
                PlotPoints(xPoints, yPoints, i * 2, j * 2);
            }  
        }
        if (Dataset.NewDataPoints.Count > 0)
        {
            InvokeRepeating("spheres_blink", 1.0f, 1.0f);
        }
    }

    // Plotta punkter
    private void PlotPoints(List<float> xPoints, List<float> yPoints, int xOffset, int yOffset)
    {
        float minX = FindMin(xPoints);      //Hitta min/max i x och y mängderna  för att kunna normalisera.
        float minY = FindMin(yPoints);
        float maxX = FindMax(xPoints);
        float maxY = FindMax(yPoints);

        for (var i = 0; xPoints != null && i < xPoints.Count; i++)
        {
            float NormalizedX = (Convert.ToSingle(xPoints[i]) - minX) / (maxX - minX);  //Normalisera                               
            float NormalizedY = (Convert.ToSingle(yPoints[i]) - minY) / (maxY - minY);

            if (Dataset.NewDataPoints.Count > 0 && i == xPoints.Count - 1)
            {
                GameObject newPoint = Instantiate(NewPoint, new Vector3(NormalizedX + xOffset, NormalizedY + yOffset, 0f) * scaleOfMatrix, Quaternion.identity);
                newPoint.transform.SetParent(DataPointHolder.transform);
                newPoint.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                newPoint.GetComponent<Renderer>().material.color = graphColors.Evaluate((Convert.ToSingle(Dataset.NewDataPoints[0]
                [Dataset.NewDataPoints[0].Keys.ElementAt(Dataset.NewDataPoints[0].Keys.Count - 1)])));

                for (int r = 0; r < Dataset.K; r++)
                {
                    knnPoints.Add(new KeyValuePair<int, GameObject>(Knn.info[r].idx, allPoints[Knn.info[r].idx]));
                }
            }
            else
            {
                GameObject point = Instantiate(DataPoint, new Vector3(NormalizedX + xOffset, NormalizedY + yOffset, 0f) * scaleOfMatrix, Quaternion.identity);   //Skapa en instans för datapunkt
                point.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                point.transform.SetParent(DataPointHolder.transform);                                                                                             // Assigna  till föräldern för  att hålla datapunkter
                point.GetComponent<Renderer>().material.color = graphColors.Evaluate(Convert.ToSingle(targets.ElementAt(i)));                 // Instansiera punkten  med en färg TODO: Färg för kategori?
                allPoints.Add(point);
             }
        }
    }

    // Hitta max i lista
    private float FindMax(List<float> list)
    {
        float maxVal = Convert.ToSingle(list[0]);
        
        for (int i = 0; i < list.Count; i++)
        {
            maxVal = maxVal > Convert.ToSingle(list[i]) ? maxVal : Convert.ToSingle(list[i]);
        }
        return maxVal;
    }

    // Hitta min i lista
    private float FindMin(List<float> list)
    {
        float minVal = Convert.ToSingle(list[0]);

        for (int i = 0; i < list.Count; i++)
        {
            minVal = minVal < Convert.ToSingle(list[i]) ? minVal : Convert.ToSingle(list[i]);
        }
        return minVal;
    }

    // Metod för att ta bort punkter och attribut i matris
    public void RemovePointsAndAxes()
    {
        Transform pointsParent = GameObject.Find("DataPointHolder").transform;
        Transform xParent = GameObject.Find("AttributesX").transform;
        Transform yParent = GameObject.Find("AttributesY").transform;

        if (pointsParent.childCount>0)
        {
            while (pointsParent.childCount>0)
            {
                Transform child = pointsParent.GetChild(0);
                child.transform.SetParent(null);
                Destroy(child.gameObject);
            }
        }

        while (xParent.childCount > 0 || yParent.childCount > 0)
        {
            if (xParent.childCount > 0)
            {
                Transform child = xParent.GetChild(0);
                child.transform.SetParent(null);
                Destroy(child.gameObject);
            }
            else
            {
                Transform child = yParent.GetChild(0);
                child.transform.SetParent(null);
                Destroy(child.gameObject);
            }
        }
        knnPoints.Clear();
        CancelInvoke();
    }

    public void CreateAttributeAxis()
    {
        for (int i = 0; i < toggles.XToggles.Count ; i++)
        {
            Text XattrName = Instantiate(AttributeName);
            XattrName.transform.SetParent(XattributesAxisHolder.transform);
            XattrName.text = toggles.XToggles[i];
        }

        for (int i = 0; i < toggles.YToggles.Count; i++)
        {
            Text YattrName = Instantiate(AttributeName);
            YattrName.transform.SetParent(YattributesAxisHolder.transform);
            YattrName.text = toggles.YToggles[i];
        }

    }

    public void spheres_blink()
    {
        foreach (KeyValuePair<int, GameObject> point in knnPoints)
        {
            Color previousColor = point.Value.GetComponent<Renderer>().material.color;
            point.Value.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            if (point.Value==null)
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
                    Debug.Log(hit.transform.gameObject.transform.position.x);
                    Debug.Log(hit.transform.gameObject.transform.position.y);
                    Debug.Log(hit.transform.gameObject.transform.position.z);
                }
            }
        }
    }
}
