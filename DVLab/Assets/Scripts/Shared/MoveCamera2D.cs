using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera2D : MonoBehaviour
{
    public float movementSpeed = 0.8f;

    void Update()
    {

        //move sideways
        if (Input.GetKey(KeyCode.A))
            transform.position -= transform.right * movementSpeed;
        else if (Input.GetKey(KeyCode.D))
            transform.position += transform.right * movementSpeed;

        ////move foreward and backwards
        if (Input.GetKey(KeyCode.UpArrow))
            transform.position += transform.forward * movementSpeed;
        else if (Input.GetKey(KeyCode.DownArrow))
            transform.position -= transform.forward * movementSpeed;

        //move foreward and backwards
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.up * movementSpeed;
        else if (Input.GetKey(KeyCode.S))
            transform.position -= transform.up * movementSpeed;

    }
}
