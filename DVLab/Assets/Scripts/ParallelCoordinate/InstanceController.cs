using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InstanceController : MonoBehaviour
{
    public List<GameObject> instanceDots { get; set; }
    public Color color;

    public void highlight()
    {
        foreach (GameObject dot in instanceDots)
        {
            dot.GetComponent<DotController>().highlight();
        }
    }
    public void restore()
    {
        foreach (GameObject dot in instanceDots)
        {
            dot.GetComponent<DotController>().restore();
        }
    }
    public void printAttributes()
    {
        foreach (GameObject dot in instanceDots)
        {
            dot.GetComponent<DotController>().printAttribute();
        }
    }
    public void hideAttributes()
    {
        foreach (GameObject dot in instanceDots)
        {
            dot.GetComponent<DotController>().hideAttribute();
        }
    }
}
