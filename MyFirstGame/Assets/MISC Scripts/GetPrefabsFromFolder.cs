using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

static public class GetPrefabsFromFolder
{
    static public List<GameObject> Get(string folderName) {
        Object[] data = Resources.LoadAll(folderName, typeof(GameObject));

        List<GameObject> prefabs = new List<GameObject>();
        foreach ( GameObject prefab in data) {
            if (PrefabUtility.GetPrefabAssetType(prefab) !=  PrefabAssetType.NotAPrefab) {
                prefabs.Add(prefab);
            }
        }
        return prefabs;
    }
}
