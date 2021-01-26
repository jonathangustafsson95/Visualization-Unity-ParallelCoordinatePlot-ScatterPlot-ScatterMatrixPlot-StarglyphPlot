using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FeaturesCheckBox : MonoBehaviour
{
    public GameObject Xcol;
    public GameObject Ycol;
    public Toggle toggleOn;
    public Toggle toggleOff;
    [HideInInspector] public List<string> XToggles;
    [HideInInspector] public List<string> YToggles;
    private List<string> attributes;
    private MatrixPlotter plotMethod;


    // Start is called before the first frame update
    void Start()
    {
        XToggles = new List<string>();
        YToggles = new List<string>();

        attributes = Dataset.GetDataSetFeatures();
        plotMethod = GameObject.Find("Visualizer").GetComponent<MatrixPlotter>();

        if (Dataset.GetDataSetFeatures().Count > 10)
        {
            foreach (var attr in attributes)
            {
                Toggle Xtoggle = Instantiate(toggleOff);
                Xtoggle.transform.SetParent(Xcol.transform);
                Xtoggle.onValueChanged.AddListener(OnToggleValueChanged);
                Xtoggle.transform.localScale = new Vector3(2.0f, 1.0f, 0.0f);
                Xtoggle.GetComponentInChildren<Text>().text = attr;

                Toggle Ytoggle = Instantiate(toggleOff);
                Ytoggle.transform.SetParent(Ycol.transform);
                Ytoggle.onValueChanged.AddListener(OnToggleValueChanged);
                Ytoggle.transform.localScale = new Vector3(2.0f, 1.0f, 0.0f);
                Ytoggle.GetComponentInChildren<Text>().text = attr;
            }
        }
        else
        {
            foreach (var attr in attributes)
            {
                Toggle Xtoggle = Instantiate(toggleOn);
                Xtoggle.transform.SetParent(Xcol.transform);
                Xtoggle.onValueChanged.AddListener(OnToggleValueChanged);
                Xtoggle.transform.localScale = new Vector3(2.0f, 1.0f, 0.0f);
                Xtoggle.GetComponentInChildren<Text>().text = attr;
                XToggles.Add(Xtoggle.GetComponentInChildren<Text>().text);

                Toggle Ytoggle = Instantiate(toggleOn);
                Ytoggle.transform.SetParent(Ycol.transform);
                Ytoggle.onValueChanged.AddListener(OnToggleValueChanged);
                Ytoggle.transform.localScale = new Vector3(2.0f, 1.0f, 0.0f);
                Ytoggle.GetComponentInChildren<Text>().text = attr;
                YToggles.Add(Xtoggle.GetComponentInChildren<Text>().text);
            }
        }
    }

    public void OnToggleValueChanged(bool isOn)
    {
        XToggles.Clear();
        YToggles.Clear();
        List<Toggle> toggles = GetComponentsInChildren<Toggle>().ToList();
        foreach (Toggle toggle in toggles)
        {
            if (toggle.transform.parent.name.Contains("X") && toggle.isOn)
            {
                XToggles.Add(toggle.GetComponentInChildren<Text>().text);
            }
            else if (toggle.transform.parent.name.Contains("Y") && toggle.isOn)
            {
                YToggles.Add(toggle.GetComponentInChildren<Text>().text);
            }
        }
        plotMethod.RemovePointsAndAxes();
        plotMethod.CreatePoints();
    }
}
