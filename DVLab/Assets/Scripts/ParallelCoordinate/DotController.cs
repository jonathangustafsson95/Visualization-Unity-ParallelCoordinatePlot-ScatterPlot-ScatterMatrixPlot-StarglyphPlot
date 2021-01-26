using Accord.Statistics.Kernels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DotController : MonoBehaviour
{
    public Vector3 scale { get; set; }
    public GameObject attributeValueHolder;
    public Color color;
    public string attribute;

    private void Start()
    {
        scale = transform.localScale;
    }
    public void highlightAll()
    {
        transform.parent.GetComponent<InstanceController>().highlight();
    }
    public void highlight()
    {
        transform.localScale = scale + new Vector3(0.1f, 0.1f, 0.1f);
    }
    public void restoreAll()
    {
        transform.parent.GetComponent<InstanceController>().restore();
    }
    public void restore()
    {
        transform.localScale = scale;
    }
    public void printAttributes()
    {
        transform.parent.GetComponent<InstanceController>().printAttributes();
    }
    public void printAttribute()
    {
        attributeValueHolder.SetActive(true);
    }
    public void hideAttributes()
    {
        transform.parent.GetComponent<InstanceController>().hideAttributes();
    }
    public void hideAttribute()
    {
        attributeValueHolder.SetActive(false);
    }
}
