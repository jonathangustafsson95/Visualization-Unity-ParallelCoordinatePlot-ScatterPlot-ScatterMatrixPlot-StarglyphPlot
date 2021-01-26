using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeAxis : MonoBehaviour
{
    private List<string> attributes = new List<string>(Dataset.ListOfPoints[1].Keys);


    private Dropdown xAxis;
    private Dropdown yAxis;
    private Dropdown zAxis;
    private List<Dropdown.OptionData> dropdownOptions;
    // Start is called before the first frame update
    void Start()
    {
        attributes = new List<string>(Dataset.ListOfPoints[1].Keys);
        attributes.RemoveAt(0);                         // Remove ID column
        attributes.RemoveAt(attributes.Count - 1);
        xAxis = GameObject.Find("X-Axis").GetComponent<Dropdown>();
        yAxis = GameObject.Find("Y-Axis").GetComponent<Dropdown>();
        zAxis = GameObject.Find("Z-Axis").GetComponent<Dropdown>();
        AddAxisOptions();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ConfirmNonIdenticalFeatures()
    {
        if (XYZNonIdentical() && YZNonIdentical() && NoFeatureOptionIsFeatures())
        {
            Dataset.usedFeatures.Clear();
            Dataset.usedFeatures.Add(xAxis.options[xAxis.value].text);
            Dataset.usedFeatures.Add(yAxis.options[yAxis.value].text);
            Dataset.usedFeatures.Add(zAxis.options[zAxis.value].text);
            SceneManager.LoadScene("Scatterplot3D");
        }
        else
        {
            Text errorText = GameObject.Find("ErrorText").GetComponent<Text>();
            Text errorText2 = GameObject.Find("ErrorText2").GetComponent<Text>();
            errorText.text = "You cannot have the same feature on several axes";
            errorText2.text = "and none of the options can be 'features'";
        }
    }

    public bool XYZNonIdentical()
    {
        if (xAxis.options[xAxis.value].text != yAxis.options[yAxis.value].text && xAxis.options[xAxis.value].text != zAxis.options[zAxis.value].text)
            return true;
        return false;
    }

    public bool NoFeatureOptionIsFeatures()
    {
        if (xAxis.options[xAxis.value].text == "Features" || yAxis.options[yAxis.value].text == "Features" || zAxis.options[zAxis.value].text == "Features")
            return false;
        return true;
    }

    public bool YZNonIdentical()
    {
        if (yAxis.options[yAxis.value].text != zAxis.options[zAxis.value].text)
            return true;
        return false;
    }

    public void AddAxisOptions()
    {
        xAxis.AddOptions(attributes);
        yAxis.AddOptions(attributes);
        zAxis.AddOptions(attributes);
    }
    public void ClearAxisOptions()
    {
        xAxis.options.RemoveRange(1, xAxis.options.Count-1);
        yAxis.options.RemoveRange(1, yAxis.options.Count-1);
        zAxis.options.RemoveRange(1, zAxis.options.Count-1);
    }
}
