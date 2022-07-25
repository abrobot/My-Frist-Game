using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{

    public CameraScript cameraScript;
    bool _firstPerson = false;

    public bool firstPerson
    {
        get { return _firstPerson; }
        set
        {
            if (value == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                cameraScript.freeze = false;

            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                cameraScript.freeze = true;
            }
            _firstPerson = value;
        }
    }

    void Start()
    {
        firstPerson = true;
    }

    void Update()
    {

    }
}
