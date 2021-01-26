using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

public class ParallelDrag : MonoBehaviour
{
    private RectTransform thingsContainer;

    private Vector3 mOffset;
    private Vector3 mCoord;

    private void Start()
    {
        thingsContainer = transform.parent.parent.GetComponent<RectTransform>();
    }
    public void beginDrag()
    {
        mCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mCoord.z;
        mousePoint.x = mCoord.x;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    public void drag()
    {
        transform.position = GetMouseWorldPos() + mOffset;
    }
    public void endDrag()
    {
        float containerHeight = thingsContainer.sizeDelta.y;

        float yPos = transform.localPosition.y + containerHeight / 2;

        string attribute = GetComponent<DotController>().attribute;
        Dictionary<string, Dictionary<string, float>> minMax = Dataset.GetMinMax(new List<string> { attribute });

        float yMin = minMax[attribute]["min"];
        float yMax = minMax[attribute]["max"];

        float newValue = yPos / containerHeight * (yMax - yMin) + yMin;

        Dataset.NewDataPoints[0][attribute] = newValue;
        thingsContainer.parent.GetComponent<ParallelCoordinatePlot>().RemoveThings();
        Dataset.ReClassify();
        thingsContainer.parent.GetComponent<ParallelCoordinatePlot>().ShowGraph();
    }
}
