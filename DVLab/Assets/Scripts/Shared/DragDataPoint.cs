using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragDataPoint : MonoBehaviour
{ 
    private Vector3 mOffset;
    private float mZCoord;
    private Scene currScene;
    public GameObject CurrentlySelectedDataPoint;
    Scene currentScene;

    public void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        if (currentScene.name is "Scatterplot3D")
        {
            if (Dataset.NewDataPoints.Count > 0)
            {
                transform.position = GetMouseWorldPos() + mOffset;
            }
        }
        else
        {
            transform.position = GetMouseWorldPos() + mOffset;
        }
    }

    private void OnMouseUp()
    {
        if (Dataset.NewDataPoints.Count > 0)
        {
            currScene = SceneManager.GetActiveScene();
            Dataset.newPred = transform.position;
            Debug.Log(transform.position);
            if (currScene.name is "Scatterplot3D")
            {
                Dataset.NewPrediction();
            }
        }
    }
}
