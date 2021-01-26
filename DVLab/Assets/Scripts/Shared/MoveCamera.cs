using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    float movementSpeed = 0.8f;
    float rotationSpeed = 1;

    float XRotation;
    float YRotation;


    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        //rotate when left click is down
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * rotationSpeed, +Input.GetAxis("Mouse X") * rotationSpeed, 0));
            XRotation = transform.rotation.eulerAngles.x;
            YRotation = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(XRotation, YRotation, 0);
        }

        //move sideways
        if (Input.GetKey(KeyCode.A))
            transform.position -= transform.right * movementSpeed;
        else if (Input.GetKey(KeyCode.D))
            transform.position += transform.right * movementSpeed;

        //move foreward and backwards
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.forward * movementSpeed;
        else if (Input.GetKey(KeyCode.S))
            transform.position -= transform.forward * movementSpeed;

    }
}
