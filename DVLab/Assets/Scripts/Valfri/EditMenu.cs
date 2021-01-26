using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditMenu : MonoBehaviour
{
    public static bool MenuIsOpen = false;
    public GameObject EditMenuUI;

    public GameObject PieStatsMenu;

    private GameObject pie;
    public ScrollRect scrollView;
    ScrollEditScript scrollScript;


    public void CloseMenu()
    {
        EditMenuUI.SetActive(false);
        MenuIsOpen = false;
    }

    public void OpenMenu(GameObject pie)
    {
        this.pie = pie;

        scrollScript = (ScrollEditScript)scrollView.GetComponent(typeof(ScrollEditScript));
        scrollScript.Show(pie);

        EditMenuUI.SetActive(true);
        MenuIsOpen = true;

        EditMenuUI.transform.SetAsLastSibling();
    }

    public void Save()
    {
        List<double> point = scrollScript.GetInputs();

        Debug.Log(point.Count);


        GameObject pieInitiator = GameObject.Find("DataInitiator");

        var pieInitiatorScript = (PieController)pieInitiator.GetComponent(typeof(PieController));

        int target = Knn.Classify(point.ToArray(), ValfriStatic.MatrixOfPoints, Dataset.NumberOfFeatures, Dataset.K);

        var canvas = GameObject.Find("Canvas");

        var newPie = pieInitiatorScript.CreatePie(point.ToArray(), target);
        var KPCA = KernalPca.Transform(point.ToArray());

        Display.Pie(newPie, canvas);
        Display.SetPosition(newPie, (float)KPCA[0], (float)KPCA[1], 0, true);

        Delete();
    }
    public void Delete()
    {
        Destroy(pie);
        PieStatsMenu PieStatsMenuScript = (PieStatsMenu)gameObject.GetComponent(typeof(PieStatsMenu));
        PieStatsMenuScript.CloseMenu();
    }

}
