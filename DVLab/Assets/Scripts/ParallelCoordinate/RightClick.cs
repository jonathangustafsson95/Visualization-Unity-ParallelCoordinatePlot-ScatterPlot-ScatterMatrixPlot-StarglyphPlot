using Accord.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RightClick : MonoBehaviour
{
    public GameObject toggleFeatures;
    public GameObject graphWindow;
    public Toggle toggle;
    private List<string> attributes;

    // Start is called before the first frame update
    void Start()
    {
        attributes = Dataset.GetDataSetFeatures();

        foreach (var attribute in attributes)
        {
            Toggle toggle = Instantiate(this.toggle);
            toggle.transform.SetParent(toggleFeatures.transform);
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
            toggle.GetComponentInChildren<Text>().text = attribute;
        }

        CanvasGroup canvasGroup = toggleFeatures.GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
    }

    private void OnToggleValueChanged(bool arg0)
    {
        List<Toggle> toggles = toggleFeatures.GetComponentsInChildren<Toggle>().ToList();
        attributes.Clear();
        graphWindow.GetComponent<ParallelCoordinatePlot>().RemoveThings();

        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                attributes.Add(toggle.GetComponentInChildren<Text>().text);
            }
        }
        if (attributes.Count > 1)
            graphWindow.GetComponent<ParallelCoordinatePlot>().ShowGraph(attributes);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            DisplayToggleFeatures(Input.mousePosition);
    }

    private void DisplayToggleFeatures(Vector2 anchoredPosition)
    {
        CanvasGroup canvasGroup = toggleFeatures.GetComponent<CanvasGroup>();

        if (canvasGroup.interactable)
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0;
        }
        else
        {
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1;

            RectTransform rectTransform = toggleFeatures.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
        }
    }
}
