using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using System;
using UnityEngine.UI;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;
using System.Data;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using Accord.Math.Geometry;
using UnityEngine.Rendering;
using TMPro;

public class ParallelCoordinatePlot : MonoBehaviour
{
    [SerializeField] private Sprite axisSprite;
    public GameObject instance;
    public GameObject dotSprite;
    public GameObject attributeValueHolder;
    public GameObject dotSpriteNew;

    private RectTransform thingsContainer;
    private RectTransform graphWindow;
    private RectTransform attribute;

    private GraphColors graphColors;

    public string dataFile;
    private List<GameObject> instances;
    private List<GameObject> blinkingInstances;

    List<string> attributes;

    // Start is called before the first frame update
    void Start()
    {
        thingsContainer = transform.Find("ThingsContainer").GetComponent<RectTransform>();
        graphWindow = GetComponent<RectTransform>();
        attribute = graphWindow.Find("Attribute").GetComponent<RectTransform>();

        graphColors = new GraphColors();
        instances = new List<GameObject>();

        attributes = Dataset.GetDataSetFeatures();

        ShowGraph(attributes);
    }

    public void ShowGraph(List<string> attributes = null)
    {
        if (attributes is null)
            attributes = this.attributes;
        else
            this.attributes = attributes;

        instances.Clear();
        Dictionary<string, Dictionary<string, float>> minMax = Dataset.GetMinMax(attributes);
        IEnumerable<object> targets = Dataset.GetDataSetTargets();
        int numberOfTargets = Dataset.GetNumberOfTargets(targets);

        Color color;
        graphColors.SetMinMax();

        DrawAxes(attributes, minMax);

        for (var i = 0; i < (Dataset.ListOfPoints.Count); i++)
        {
            color = graphColors.Evaluate(Convert.ToSingle(targets.ElementAt(i)));
            ShowOneInstance(Dataset.ListOfPoints, attributes, minMax, color, CreateDot, i);
        }

        thingsContainer.SetAsLastSibling();

        if (Dataset.NewDataPoints.Count > 0)
        {
            color = graphColors.Evaluate(Convert.ToSingle(Dataset.NewDataPoints[0]
                [Dataset.NewDataPoints[0].Keys.ElementAt(Dataset.NewDataPoints[0].Keys.Count - 1)]));

            ShowOneInstance(Dataset.NewDataPoints, attributes, minMax, color, CreateNewDot, 0);
            blinkingInstances = new List<GameObject>();

            for (int i = 0; i < Dataset.K; i++)
            {
                Debug.Log(Dataset.NewDataPoints[0][Dataset.NewDataPoints[0].Keys.ElementAt(Dataset.NewDataPoints[0].Keys.Count - 1)]);
                blinkingInstances.Add(instances[Knn.info[i].idx]);
            }
            foreach (GameObject blinkingInstance in blinkingInstances)
            {
                blinkingInstance.GetComponent<LineRenderer>().sortingOrder = 1;
                foreach (GameObject dot in blinkingInstance.GetComponent<InstanceController>().instanceDots)
                {
                    dot.GetComponent<SortingGroup>().sortingOrder = 2;
                }
            }
            InvokeRepeating("InstanceBlink", 1f, 1f);
        }
    }
    public void InstanceBlink()
    {
        LineRenderer lineRenderer;
        foreach (GameObject blinkInst in blinkingInstances)
        {
            if (blinkInst == null)
            {
                CancelInvoke();
                break;
            }
            blinkInst.transform.SetAsLastSibling();
            lineRenderer = blinkInst.GetComponent<LineRenderer>();

            if (lineRenderer.material.color != Color.white)
            {
                lineRenderer.material.color = Color.white;
                foreach (var item in blinkInst.GetComponent<InstanceController>().instanceDots)
                {
                    item.GetComponent<Image>().color = Color.white;
                    item.transform.SetAsLastSibling();
                }
            }
            else
            {
                lineRenderer.material.color = blinkInst.GetComponent<InstanceController>().color;
                foreach (var item in blinkInst.GetComponent<InstanceController>().instanceDots)
                {
                    item.GetComponent<Image>().color = item.GetComponent<DotController>().color;
                    item.transform.SetAsLastSibling();
                }
            }
        }
    }

    private void ShowOneInstance(List<Dictionary<string, object>> Data, List<string> attributes, Dictionary<string, Dictionary<string, float>> minMax, Color color, Func<Vector2, Color, float, string, GameObject> Create, int i = 0)
    {
        float containerHeight = thingsContainer.sizeDelta.y;
        float containerWidth = thingsContainer.sizeDelta.x;

        GameObject[] dots = new GameObject[attributes.Count];

        for (int j = 0; j < attributes.Count; j++)
        {
            float yMin = minMax[attributes[j]]["min"];
            float yMax = minMax[attributes[j]]["max"];

            float xPos = (containerWidth / (attributes.Count - 1)) * j;
            float yPos = (Convert.ToSingle((Data[i][attributes[j]])) - yMin) / (yMax - yMin) * containerHeight;

            GameObject point = Create(new Vector2(xPos, yPos), color, Convert.ToSingle(Data[i][attributes[j]]), attributes[j]);

            dots[j] = point;
        }
        instances.Add(DrawConnection(dots, color));
    }

    private void DrawAxes(List<string> attributes, Dictionary<string, Dictionary<string, float>> minMax)
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            DrawAxis(
                new Vector2(thingsContainer.sizeDelta.x / (attributes.Count - 1) * i, thingsContainer.anchoredPosition.y),
                minMax[attributes[i]]["min"],
                minMax[attributes[i]]["max"]);
            WriteAttributes(new Vector2(thingsContainer.sizeDelta.x / (attributes.Count - 1) * i, 0), attributes[i]);
        }
    }

    private void DrawAxis(Vector2 anchoredPosition, float min, float max) // min max för att presentera min och max värde för varje attribut = senare attribut.
    {
        GameObject axis = new GameObject("axis", typeof(Image));
        axis.transform.SetParent(graphWindow, false);
        axis.GetComponent<Image>().sprite = axisSprite;
        axis.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);

        RectTransform rectTransform = axis.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(1, graphWindow.sizeDelta.y + 20);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    private void WriteAttributes(Vector2 anchoredPosition, string attributeName)
    {
        RectTransform axisLabel = Instantiate(attribute);
        axisLabel.SetParent(graphWindow);
        axisLabel.gameObject.SetActive(true);
        axisLabel.GetComponent<Text>().text = attributeName;
        axisLabel.GetComponent<Text>().color = Color.black;
        axisLabel.anchoredPosition = new Vector2(anchoredPosition.x - 10, -20);
        axisLabel.anchorMin = new Vector2(0, 0);
        axisLabel.anchorMax = new Vector2(0, 0);
        axisLabel.localScale = new Vector2(1, 1);
        axisLabel.SetAsFirstSibling();
    }
    private GameObject DrawConnection(GameObject[] dots, Color color)
    {
        GameObject line = Instantiate(instance);
        line.transform.SetParent(thingsContainer, false);

        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = color;
        lineRenderer.startWidth = 0.025f;
        lineRenderer.endWidth = 0.025f;
        lineRenderer.sortingOrder = 0;

        Vector3[] positions = new Vector3[dots.Length];
        for (int i = 0; i < dots.Length; i++)
        {
            positions[i] = dots[i].transform.position;
            dots[i].transform.SetParent(line.transform);
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        lineRenderer.sortingOrder = 0;

        InstanceController instanceController = line.GetComponent<InstanceController>();
        instanceController.instanceDots = new List<GameObject>(dots);
        instanceController.color = color;

        line.SetActive(true);

        return line;
    }
    private GameObject CreateDot(Vector2 anchoredPosition, Color color, float attributeValue, string attribute)
    {
        GameObject dot = Instantiate(dotSprite);
        dot.transform.SetParent(thingsContainer, false);
        dot.transform.localScale = new Vector2(0.05f, 0.05f);

        Image image = dot.GetComponent<Image>();
        image.color = color;

        RectTransform rectTransform = dot.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        Text dotAttributeValue = dot.transform.Find("AttributeValueHolder").GetComponent<Text>();
        dotAttributeValue.text = attributeValue.ToString();

        RectTransform rectTransformAtt = dotAttributeValue.GetComponent<RectTransform>();
        rectTransformAtt.localScale = new Vector2(10, 10);

        DotController dotController = dot.GetComponent<DotController>();
        dotController.color = color;
        dotController.attribute = attribute;

        dot.SetActive(true);
        return dot;
    }

    private GameObject CreateNewDot(Vector2 anchoredPosition, Color color, float attributeValue, string attribute)
    {
        GameObject dot = Instantiate(dotSpriteNew);
        dot.transform.SetParent(thingsContainer, false);
        dot.transform.localScale = new Vector2(0.15f, 0.15f);

        Image image = dot.GetComponent<Image>();
        image.color = color;

        RectTransform rectTransform = dot.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        Text dotAttributeValue = dot.transform.Find("AttributeValueHolder").GetComponent<Text>();
        dotAttributeValue.text = attributeValue.ToString();

        RectTransform rectTransformAtt = dotAttributeValue.GetComponent<RectTransform>();
        rectTransformAtt.localScale = new Vector2(5, 5);

        DotController dotController = dot.GetComponent<DotController>();
        dotController.color = color;
        dotController.attribute = attribute;

        dot.SetActive(true);
        return dot;
    }
    public void RemoveThings()
    {
        for (int i = 0; i < thingsContainer.childCount; i++)
        {
            Transform child = thingsContainer.GetChild(i);
            if (child.gameObject.activeSelf)
                Destroy(child.gameObject);
        }
        for (int i = 0; i < graphWindow.childCount; i++)
        {
            Transform child = graphWindow.GetChild(i);
            if (child.gameObject.activeSelf && child.gameObject != thingsContainer.gameObject)
                Destroy(child.gameObject);
        }
        CancelInvoke();
    }
}
