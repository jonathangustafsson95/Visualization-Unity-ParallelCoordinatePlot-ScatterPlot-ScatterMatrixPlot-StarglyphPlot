using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeKMenu : MonoBehaviour
{
    public static bool MenuIsOpen = false;
    public GameObject KMenuUI;

    public Text KText;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
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
        KMenuUI.SetActive(false);
        MenuIsOpen = false;
    }
    void OpenMenu()
    {
        KMenuUI.SetActive(true);
        MenuIsOpen = true;

        KMenuUI.transform.SetAsLastSibling();
    }

    private void CloseMenus()
    {
        if (PieStatsMenu.MenuIsOpen)
        {
            PieStatsMenu dataMenu = (PieStatsMenu)GameObject.Find("Canvas").GetComponent(typeof(PieStatsMenu));
            dataMenu.CloseMenu();
        }
        if (Datamenu.MenuIsOpen)
        {
            Datamenu dataMenu = (Datamenu)GameObject.Find("Canvas").GetComponent(typeof(Datamenu));
            dataMenu.CloseMenu();
        }
    }


    public void ChagneK()
    {
        InputField KInput =  GameObject.Find("KInput").GetComponent<InputField>();

        string kInputText = KInput.text;
        if(Int32.TryParse(kInputText, out int newK))
        {
            Dataset.K = newK;
            KText.text = "K = " + newK;
            //var canvas = GameObject.Find("Canvas");
            GameObject dataInitiator = GameObject.Find("InitiateScene");
            InitiateValfri dataInitiatorScript = (InitiateValfri)dataInitiator.GetComponent(typeof(InitiateValfri));
            dataInitiatorScript.Init();
        }
    }
}
