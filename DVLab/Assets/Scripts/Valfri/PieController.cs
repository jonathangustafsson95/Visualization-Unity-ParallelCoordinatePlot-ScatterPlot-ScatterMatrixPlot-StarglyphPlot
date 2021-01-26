//using Accord.Math;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PieController : MonoBehaviour
{
    public Image WedgePrefab;
    public GameObject PiePrefab;
    public Image EventLayer;

    private GameObject pie;
    private BigPie pieScript;


    public GameObject CreatePie(double[] point, int target, bool added = false)
    {
        pie = Instantiate(PiePrefab);
        pieScript = pie.GetComponent<BigPie>();

        CreateBackgroundWedge(added);
        CreateVisiblePie(point, target);
        CreatePieEventLayer();

        ValfriStatic.pies.Add(pie);

        return pie;
    }
    private void CreateVisiblePie(double[] point, int target)
    {
        CreatePieWedges(point, target);
    }

    private void CreateBackgroundWedge(bool added)
    {
        // en vit backgrund
        Image backgorund = Instantiate(WedgePrefab);
        backgorund.color = Color.white;

        WedgeCirkle backgroundCirkle = new WedgeCirkle(backgorund);
        backgroundCirkle.SetParant(pie);

        if(!added)
            backgorund.enabled = false;
    }
    private void CreatePieWedges(double[] point, int target)
    {
        Image wedgeImage;
        float zRotation = 0f;

        for (int i = 0; i < Dataset.NumberOfFeatures; i++)
        {
            wedgeImage = Instantiate(WedgePrefab);

            Wedge newWedge = new Wedge(wedgeImage)
            {
                Name = Dataset.FeatureNames[i],
                Value = point[i],
                NormalizedValue = Normalize.Point(point)[i],
                Size = (float)Normalize.Point(point)[i],
                Rotation = zRotation
            };
            newWedge.SetParant(pie);
            newWedge.SetColor(Dataset.GetColor(target));

            zRotation -= wedgeImage.fillAmount * 360f;
            pieScript.WedgeObjects.Add(newWedge);

            if (!ValfriStatic.FeaturesToDisplay.Contains(newWedge.Name))
            {
                newWedge.Image.enabled = false;
            }

        }
    }
    private void CreatePieEventLayer()
    {
        Image clickableImage = Instantiate(EventLayer) as Image;
        WedgeEventLayer clickableCirkle = new WedgeEventLayer(clickableImage)
        {
            Size = 10f
        };
        clickableCirkle.SetParant(pie);

        // add event on enter
        SetEvents(clickableCirkle);
        pieScript.WedgeObjects.Add(clickableCirkle);
    }
    private void SetEvents(WedgeEventLayer clickableCirkle)
    {
        EventTrigger trigger = clickableCirkle.Image.gameObject.GetComponent<EventTrigger>();
        clickableCirkle.AddEvent(trigger, EventTriggerType.PointerEnter, pieScript.CreateBigPie);
        clickableCirkle.AddEvent(trigger, EventTriggerType.PointerExit, pieScript.SmallPie);
        clickableCirkle.AddEvent(trigger, EventTriggerType.PointerClick, pieScript.ShowClickedPie);
    }
}
