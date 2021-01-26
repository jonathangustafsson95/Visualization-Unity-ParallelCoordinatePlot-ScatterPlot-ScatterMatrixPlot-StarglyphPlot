using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Skin : MonoBehaviour
{
    public GameObject featureNameHolder;
    public GameObject KNNSettings;
    public GameObject dataInput;
    public GameObject dataInputButton;
    public Button addInstanceButton;
    public Button KNNSettingsButton;
    public GameObject changeKValue;
    public GameObject kValueInput;
    List<string> attributes;
    

    // Start is called before the first frame update
    void Start()
    {
        //attributes = Dataset.GetDataSetFeatures();

        //foreach (var field in Dataset.InputFields)
        //{
        //    GameObject.Destroy(field);
        //}
        //Dataset.InputFields = new List<InputField>();
        //Dataset.EditInputFields = new List<InputField>();
                            
        //for (int i = 0; i < attributes.Count; i++)
        //{
        //    GameObject field = Instantiate(dataInput);
        //    field.transform.SetParent(featureNameHolder.transform);
        //    field.name = attributes[i] + "Field";
        //    field.GetComponent<InputField>().placeholder.GetComponent<Text>().text = attributes[i];
        //    Dataset.InputFields.Add(field.GetComponent<InputField>());
        //}
        //GameObject Button = Instantiate(dataInputButton);
        //Button.transform.SetParent(featureNameHolder.transform);
        //Button myButton = Button.GetComponent<Button>();
        //myButton.GetComponentInChildren<Text>().text = "Add";
        //myButton.onClick.AddListener(Dataset.InstantiateNewDataPoint);

        Button changeKValue = GameObject.Find("ChangeK").GetComponent<Button>();
        InputField kInput = (InputField)GameObject.Find("KInputField").GetComponent(typeof(InputField));
        changeKValue.onClick.AddListener(() => Dataset.ChangeKValue(kInput.text));

        Button changeKnnWeight = GameObject.Find("ChangeWeight").GetComponent<Button>();
        changeKnnWeight.onClick.AddListener(() => Knn.ChangeKnnState());

        Button changeKnnAlg = GameObject.Find("ChangeKnnAlg").GetComponent<Button>();
        changeKnnAlg.onClick.AddListener(() => Knn.ChangeKnnAlgorithm());

        GameObject.Find("CurrentK").GetComponent<Text>().text = "K = " + Dataset.K.ToString();
        GameObject.Find("CurrentKnnState").GetComponent<Text>().text = "Weight =  " + Knn.currentState.ToString();
        GameObject.Find("CurrentKnnAlg").GetComponent<Text>().text = "Algorithm =  " + Knn.currentAlg.ToString();

        CanvasGroup canvasGroupFeature = featureNameHolder.GetComponent<CanvasGroup>();
        canvasGroupFeature.interactable = false;
        canvasGroupFeature.alpha = 0;

        CanvasGroup canvasGroupKNN = KNNSettings.GetComponent<CanvasGroup>();
        canvasGroupKNN.interactable = false;
        canvasGroupKNN.alpha = 0;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void ToggleAddInstance()
    {
        CanvasGroup canvasGroup = featureNameHolder.GetComponent<CanvasGroup>();

        if (!canvasGroup.interactable)
        {
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1;
            addInstanceButton.GetComponentInChildren<Text>().text = "Cancel";
        }
        else
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0;
            addInstanceButton.GetComponentInChildren<Text>().text = "Add Instance";
        }
    }
    public void ToggleKNNSettings()
    {
        CanvasGroup canvasGroup = KNNSettings.GetComponent<CanvasGroup>();

        if (!canvasGroup.interactable)
        {
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1;
            KNNSettingsButton.GetComponentInChildren<Text>().text = "Back";
        }
        else
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0;
            KNNSettingsButton.GetComponentInChildren<Text>().text = "KNN Settings";
        }
    }
}
