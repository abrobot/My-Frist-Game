using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(NoiseMapTester), true), CanEditMultipleObjects]
public class NoiseMapTesterEditerHandling : Editor
{
    public override void OnInspectorGUI()
    {
        NoiseMapTester tester = (NoiseMapTester)target;

        if (GUILayout.Button ("Generate")) {
            tester.GenerateNoiseMap();
        }

        if (DrawDefaultInspector()) {
            if (tester.autoUpdate) {
                tester.GenerateNoiseMap();
            }
        }
    }


}

