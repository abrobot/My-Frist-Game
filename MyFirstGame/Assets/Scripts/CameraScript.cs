using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public float mouseSesitivity = 100f;
    float xRotation = 0f;
    public Transform playerBody;
    public float acceleration = 1.05f;
    float currentAcceleration = .5f;
    float LastValidUpdate = 0;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        // if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0)
        // {
        //     if (Time.time - LastValidUpdate > .2) {
        //         currentAcceleration =.1f;
        //         print("restart");
        //     }
        // }
        // else
        // {
            // currentAcceleration = Mathf.Clamp((currentAcceleration * acceleration), .1f , 1);
            // print(currentAcceleration);
            float mouseX = Input.GetAxis("Mouse X") * mouseSesitivity * acceleration;// * currentAcceleration;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSesitivity * acceleration;// * currentAcceleration;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
            // LastValidUpdate = Time.time;
        // }
    }
}
