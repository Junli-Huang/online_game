using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Move : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;

    private void Update()
    {
        float forwardMovement = Input.GetAxis("Vertical");

        if (forwardMovement != 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * forwardMovement);
        }

        float turnMovement = Input.GetAxis("Horizontal");
        if (turnMovement != 0)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed * turnMovement);
        }
    }
            
}
