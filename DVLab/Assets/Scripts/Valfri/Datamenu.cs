using Accord.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Datamenu : MonoBehaviour
{
    public static bool MenuIsOpen = false;
    public GameObject DataMenuUI;
    public ScrollRect scrollView;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (MenuIsOpen)
            {
                CloseMenu();
            }
            else
            {
                CloseMenus();
                OpenMenu();
            }
        }
    }

    public void CloseMenu()
    {
        DataMenuUI.SetActive(false);
        MenuIsOpen = false;
    }
    void OpenMenu()
    {
        DataMenuUI.SetActive(true);
        MenuIsOpen = true;

        DataMenuUI.transform.SetAsLastSibling();
    }

    //public void AddPoint()
    //{
    //    double[] point = new double[4];

    //    point[0] = InputToDouble("PetalWidthInput");
    //    point[1] = InputToDouble("SepalWidthInput");
    //    point[2] = InputToDouble("PetalLengthInput");
    //    point[3] = InputToDouble("PetalLengthInput");

    //    GameObject pieInitiator = GameObject.Find("DataInitiator");

    //    //var pieInitiatorScript = (Pie)pieInitiator.GetComponent(typeof(Pie));
    //    var pieInitiatorScript = (PieController)pieInitiator.GetComponent(typeof(PieController));

    //    int target = Knn.Classify(point, Dataset.MatrixOfPoints, Dataset.NumberOfFeatures, Dataset.K);
        
    //    var canvas = GameObject.Find("Canvas");
    //    //var pie = pieInitiatorScript.CreatePie(Normalize.Point(point), target);
        
    //    var pie = pieInitiatorScript.CreatePie(point, target);
    //    var KPCA = KernalPca.Transform(point);

    //    Display.Pie(pie, canvas);
    //    Display.SetPosition(pie, (float)KPCA[0], (float)KPCA[1], 0);
    //}

    private double InputToDouble(string FieldName)
    {
        InputField Input = GameObject.Find(FieldName).GetComponent<InputField>();

        if (double.TryParse(Input.text, out double value))
            return value;
        else
            return 0;
    }

    private void CloseMenus()
    {
        if (PieStatsMenu.MenuIsOpen)
        {
            PieStatsMenu dataMenu = (PieStatsMenu)GameObject.Find("Canvas").GetComponent(typeof(PieStatsMenu));
            dataMenu.CloseMenu();
        }
        if (ChangeKMenu.MenuIsOpen)
        {
            ChangeKMenu dataMenu = (ChangeKMenu)GameObject.Find("Canvas").GetComponent(typeof(ChangeKMenu));
            dataMenu.CloseMenu();
        }
    }

    public void AddPoint()
    {
        ScrollAddScript script = (ScrollAddScript)scrollView.GetComponent(typeof(ScrollAddScript));

        List<double> point = script.GetInputs();

        //int target = (int)point[point.Count - 1];
        point.RemoveAt(point.Count - 1);

        GameObject InitiateObject = GameObject.Find("InitiateScene");
        var InitiateScript = (InitiateValfri)InitiateObject.GetComponent(typeof(InitiateValfri));
        ValfriStatic.AddedPoints.Add(point);


        InitiateScript.Init();

        //GameObject pieInitiator = GameObject.Find("DataInitiator");

        //var pieInitiatorScript = (PieController)pieInitiator.GetComponent(typeof(PieController));


        //if(target == -1)
        //    target = Knn.Classify(point.ToArray(), Dataset.MatrixOfPoints, Dataset.NumberOfFeatures, Dataset.K);

        //var canvas = GameObject.Find("Canvas");

        //var pie = pieInitiatorScript.CreatePie(point.ToArray(), target);



        //Dataset.AddedPoints.Add(point);

        //var KPCA = KernalPca.Transform(point.ToArray());

        //Display.Pie(pie, canvas);
        //Display.SetPosition(pie, (float)KPCA[0], (float)KPCA[1], 0);
    }
}
