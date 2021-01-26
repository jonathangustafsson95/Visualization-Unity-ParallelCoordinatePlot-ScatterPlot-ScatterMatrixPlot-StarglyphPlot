//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class Pie : MonoBehaviour
//{
//    public Image WedgePrefab;
//    public GameObject PiePrefab;
//    public Image EventLayer;

//    private GameObject pie;
//    private BigPie pieScript;

//    public GameObject CreatePie(double[] point, int target)
//    {
//        pie = Instantiate(PiePrefab);
//        pieScript = pie.GetComponent<BigPie>();

//        CreateVisiblePie(point, target);
//        CreatePieEventLayer();

//        return pie;
//    }
 

//    private void CreateVisiblePie(double[] point, int target)
//    {
//        // en vit backgrund
//        //Image backgorund = CreateCirkle(Color.white, new Vector2(30f, 30f));
//        //SetParant(backgorund, pie);

//        CreatePieWedges(point, target);
//    }
//    private void CreatePieWedges(double[] point, int target)
//    {
//        Image newWedge;
//        float zRotation = 0f;
//        for (int i = 0; i < Dataset.NumberOfFeatures; i++)
//        {
//            //Vector2 size = new Vector2((float)point[i + 1] * 30, (float)point[i + 1] * 30);
//            Vector2 size = new Vector2((float)point[i] * 30, (float)point[i] * 30);
//            newWedge = CreateWedge(GetColor(target), size, zRotation);
//            zRotation -= newWedge.fillAmount * 360f;
//            SetParant(newWedge, pie);
//        }
//    }


//    private void CreatePieEventLayer()
//    {
//        Image clickable = CreateClickableLayer(new Color(1.0f, 1.0f, 1.0f, 0f), new Vector2(10f, 10f));
//        SetParant(clickable, pie);

//        // add event on enter
//        EventTrigger trigger = clickable.gameObject.GetComponent<EventTrigger>();
//        AddEvent(trigger, EventTriggerType.PointerEnter, pieScript.CreateBigPie);
//        AddEvent(trigger, EventTriggerType.PointerExit, pieScript.SmallPie);
//        AddEvent(trigger, EventTriggerType.PointerClick, pieScript.ShowClickedPie);
//    }
//    private void AddEvent(EventTrigger trigger, EventTriggerType triggerType, System.Action<BaseEventData> callback)
//    {
//        EventTrigger.Entry triggerEnter = new EventTrigger.Entry();
//        triggerEnter.eventID = triggerType;
//        triggerEnter.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
//        trigger.triggers.Add(triggerEnter);
//    }
//    private Image CreateClickableLayer(Color color, Vector2 size)
//    {
//        Image clickable = Instantiate(EventLayer) as Image;
//        SetCirkleConfig(clickable, color, size);
//        return clickable;
//    }


//    private Image CreateWedge(Color color, Vector2 size, float zRotation)
//    {
//        var wedge = CreateCirkle(color, size);
//        wedge.fillAmount = (float)1 / (float)Dataset.NumberOfFeatures;
//        wedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));

//        return wedge;
//    }
//    private Image CreateCirkle(Color color, Vector2 size)
//    {
//        Image newWedge = Instantiate(WedgePrefab) as Image; ;
//        SetCirkleConfig(newWedge, color, size);
//        return newWedge;
//    }
//    private void SetCirkleConfig(Image image, Color color, Vector2 size)
//    {
//        image.color = color;
//        image.rectTransform.sizeDelta = size;
//        image.fillAmount = 1f;
//    }


//    private Color GetColor(double target)
//    {
//        if (target == 0)
//            return Color.red;
//        else if (target == 1)
//            return Color.blue;
//        else if (target == 2)
//            return Color.green;
//        else
//            return Color.red;
//    }
//    private void SetParant(Image wedge, GameObject parent)
//    {
//        wedge.transform.SetParent(parent.transform);
//        wedge.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
//        pieScript.wedges.Add(wedge);

//        var piePartsScript = pie.GetComponent<PieParts>();
//        //piePartsScript.wedges.Add(wedge);
//    }
//}
