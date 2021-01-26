using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AddDataPoint : MonoBehaviour
{
    public GameObject FeatureNameHolder;
    public GameObject DataInput;
    public GameObject DataInputButton;
    public GameObject DataPoint;
    public Text AttributeName;
    List<string> attributes;
    List<InputField> inputFields = new List<InputField>();


    // Start is called before the first frame update
    void Start()
    {
    }

    public void InstantiateNewDataPoint()
    {
        Debug.Log(inputFields.Count);
        List<int> values = new List<int>();
        foreach (var inputfield in inputFields)
        {
            Int32.TryParse(inputfield.text, out int result);
            values.Add(result);
        }
        Dictionary<string, object> dict = new Dictionary<string, object>();
        int valueToAdd = 0;
        dict.Add("id", 1);
        foreach (var key in Dataset.ListOfPoints[0].Keys)
        {
            if (key != "Id" && key != "Species")
            {
                Debug.Log(key);
                dict.Add(key, values[valueToAdd]);
                valueToAdd++;
                Debug.Log("added value" + valueToAdd);
            }
        }
        dict.Add("Species", "");
        Debug.Log("GetFirstKey");
        Debug.Log(dict.Keys.First().ToString());
        Dataset.NewDataPoints.Add(dict);
        Debug.Log(Dataset.NewDataPoints[0].Keys.Count());
        Debug.Log("Here I Will instantiate new datapoint");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
