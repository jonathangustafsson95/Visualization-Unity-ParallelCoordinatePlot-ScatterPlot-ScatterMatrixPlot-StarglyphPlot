using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Display : MonoBehaviour
{
    public static void Pie(GameObject pie, GameObject canvas)
    {
        pie.transform.SetParent(canvas.transform);
    }

    public static void SetPosition(GameObject pie, float x, float y, float z, bool display = false)
    {
        DisplayHelper displayHelper = new DisplayHelper()
        {
            pie = pie,
            X = x,
            Y = y,
            Z = z
        };

        if (display)
            displayHelper.Display();

    }

    public static void DisplayAll()
    {
        DisplayHelper.DiplayAll();
    }
}

public class DisplayHelper
{
    private static List<DisplayHelper> displayHelpers = new List<DisplayHelper>();
    private static float minX;
    private static float maxX;
    private static float minY;
    private static float maxY;

    public DisplayHelper()
    {
        displayHelpers.Add(this);
    }

    public GameObject pie;
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; } = 0;
    public float NormX { get; set; } = -1;
    public float NormY { get; set; } = -1;
    public float NormZ { get; set; } = 0;

    public static void Reset()
    {
        displayHelpers = new List<DisplayHelper>();
        minX = maxX = minY = maxY = 0;
    }

    public static void DiplayAll()
    {
        foreach (var helper in displayHelpers)
        {
            helper.Display();
        }
    }
    public void Display()
    {
        if(NormX == -1)
        {
            // If between min and max, dont need to redo normalize for all
            if(X < minX || X > maxX || Y < minY || Y > maxY)
            {
                NormalizeAll();
            }
            else
            {
                Normalize();
            }
        }
        else
        {
            //pie.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1f * 700 , -1f * 350, NormX);
            //pie.GetComponent<RectTransform>().anchoredPosition = new Vector3(1 * 1400 - 700, 1 * 750 - 375, NormX);
            pie.GetComponent<RectTransform>().anchoredPosition = new Vector3(NormX * 1400 - 700, NormY * 750 - 375, NormX);
            //pie.GetComponent<RectTransform>().anchoredPosition = new Vector3(NormX * 100 -700, NormX * 200-350, NormX);
        }
    }

    private void Normalize()
    {
        NormX = ((X - minX) / (maxX - minX));
        NormY = ((Y - minY) / (maxY - minY));
    }

    private void NormalizeAll()
    {
        SetMinMax();
        foreach (var helper in displayHelpers)
        {
            helper.Normalize();
        }
    }
    private void SetMinMax()
    {
        minX = displayHelpers[0].X;
        minY = displayHelpers[0].Y;
        maxX = displayHelpers[0].X;
        maxY = displayHelpers[0].Y;
        foreach (var helper in displayHelpers)
        {
            if (helper.X < minX)
                minX = helper.X;
            if (helper.X > maxX)
                maxX = helper.X;
            if (helper.Y < minY)
                minY = helper.Y;
            if (helper.Y > maxY)
                maxY = helper.Y;
        }
    }
}

