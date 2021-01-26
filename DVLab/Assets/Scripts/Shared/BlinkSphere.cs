using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlinkSphere
{
    public static GameObject ball;
    public static Color PreviousColor;
    public static int frame = 0;
    // Start is called before the first frame update

    public static void sphere_blink()
    {
        //Debug.Log(PreviousColor.ToString());
        if (ball.GetComponent<Renderer>().material.color != Color.white)
            //if (frame > 100)
            //{
            ball.GetComponent<Renderer>().material.color = Color.white;
                //frame = 0;
            //}
            //else
            //    frame++;
        else
        //    if (frame > 100)
        //{
            ball.GetComponent<Renderer>().material.color = PreviousColor;
            //frame = 0;
        //}
        //else
        //    frame++;
    }
}
