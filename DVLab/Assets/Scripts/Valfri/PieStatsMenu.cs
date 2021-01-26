using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieStatsMenu : MonoBehaviour
{
    public static bool MenuIsOpen = false;
    public GameObject MenuUI;
    public GameObject TextPrefab;
    public GameObject ListItemPrefab;

    public static bool PieIsShowing = false;
    private GameObject pieClone;

    public static bool EditMenuIsOpen = false;
    public GameObject EditMenu;
    public EditMenu EditMenuScript;

    private GameObject originalPie;

    public void CloseMenu()
    {
        MenuUI.SetActive(false);
        MenuIsOpen = false;

        if (PieIsShowing)
        {
            GameObject.Destroy(pieClone);
            PieIsShowing = false;

            var scrollView = MenuUI.transform.Find("StatsView");
            var scrollScript = (ScrollViewScript)scrollView.GetComponent(typeof(ScrollViewScript));
            scrollScript.ClearList();
        }

        CloseSubMenus();
    }
    public void OpenMenu()
    {
        CloseOpenMenus();

        MenuUI.SetActive(true);
        MenuIsOpen = true;
        MenuUI.transform.SetAsLastSibling();
    }

    public void ShowPie(GameObject pie)
    {
        originalPie = pie;

        if (PieIsShowing)
            GameObject.Destroy(pieClone);

        pieClone = Instantiate(pie);

        pieClone.transform.SetParent(MenuUI.transform);
        pieClone.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,150,0);

        PieIsShowing = true;
        
        ShowStats(pie);
    }
    public void ShowStats(GameObject pie)
    {
        var scrollView = MenuUI.transform.Find("StatsView");
        var scrollScript = (ScrollViewScript)scrollView.GetComponent(typeof(ScrollViewScript));
        var pieScript = pie.GetComponent<BigPie>();

        List<WedgeCirkle> wedgeCirkles = pieScript.WedgeObjects;

        foreach (var wedge in wedgeCirkles)
        {
            if (wedge is Wedge)
                scrollScript.CreateListItem((Wedge)wedge);
        }
    }

    public void OpenEditMenu()
    {
        EditMenuScript = (EditMenu)gameObject.GetComponent(typeof(EditMenu));

        MenuUI.SetActive(false);
        EditMenuScript.OpenMenu(originalPie);
    }


    private void CloseOpenMenus()
    {
        if (Datamenu.MenuIsOpen)
        {
            Datamenu dataMenu = (Datamenu)GameObject.Find("Canvas").GetComponent(typeof(Datamenu));
            dataMenu.CloseMenu();
        }
        if (ChangeKMenu.MenuIsOpen)
        {
            ChangeKMenu dataMenu = (ChangeKMenu)GameObject.Find("Canvas").GetComponent(typeof(ChangeKMenu));
            dataMenu.CloseMenu();
        }
    }

    private void CloseSubMenus()
    {
        EditMenuScript = (EditMenu)gameObject.GetComponent(typeof(EditMenu));

        EditMenuScript.CloseMenu();
    }
}
