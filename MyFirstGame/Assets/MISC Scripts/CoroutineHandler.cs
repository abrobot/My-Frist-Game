using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// this script is used to get around the fact that you cant start coroutines in a static class

public class CoroutineHandler : MonoBehaviour
{

    public void callCoroutine(IEnumerator callback) {
        StartCoroutine(callback);
    }
}
