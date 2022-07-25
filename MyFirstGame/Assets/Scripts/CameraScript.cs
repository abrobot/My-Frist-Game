using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{


    public float mouseSesitivity = 100f;
    float xRotation = 0f;
    public Transform playerBody;
    public bool freeze = false;


    void Update()
    {
        if (!freeze)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSesitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSesitivity * Time.deltaTime;


            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

    }
}
