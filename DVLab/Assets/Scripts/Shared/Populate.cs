using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Populate : MonoBehaviour
{
    public GameObject dataInput;
    List<string> attributes;
    public GameObject dataInputButton;

    // Start is called before the first frame update
    void Start()
    {
        PopulateScroll();
    }

    void PopulateScroll()
    {
        attributes = Dataset.GetDataSetFeatures();

        foreach (var field in Dataset.InputFields)
        {
            Destroy(field);
        }
        Dataset.InputFields = new List<InputField>();
        Dataset.EditInputFields = new List<InputField>();

        for (int i = 0; i < Dataset.FeatureNames.Count; i++)
        {
            GameObject field = Instantiate(dataInput, transform);
            field.name = attributes[i] + "Field";
            field.GetComponent<InputField>().placeholder.GetComponent<Text>().text = attributes[i];
            Dataset.InputFields.Add(field.GetComponent<InputField>());
        }
        GameObject Button = Instantiate(dataInputButton, transform);
        Button myButton = Button.GetComponent<Button>();
        myButton.GetComponentInChildren<Text>().text = "Add";
        myButton.onClick.AddListener(Dataset.InstantiateNewDataPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
