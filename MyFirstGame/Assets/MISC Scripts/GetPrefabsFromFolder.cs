using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class GetPrefabsFromFolder
{
    static public List<GameObject> Get(string folderName) {
        Object[] data = Resources.LoadAll(folderName, typeof(GameObject));

        List<GameObject> prefabs = new List<GameObject>();
        foreach ( GameObject prefab in data) {
            prefabs.Add(prefab);
        }
        return prefabs;
    }

        static public List<InventoryItemSO> GetInventroySO(string folderName) {
        Object[] data = Resources.LoadAll(folderName, typeof(InventoryItemSO));
        List<InventoryItemSO> SOs = new List<InventoryItemSO>();
        foreach ( InventoryItemSO SO in data) {
            SOs.Add(SO);
        }
        return SOs;
    }



}
